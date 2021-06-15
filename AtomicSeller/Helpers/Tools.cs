using AtomicSeller.Controllers;
using AtomicSeller.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
//using Microsoft.Extensions.Logging;
using NLog.Web;
using NLog;
using NLog.Targets;
using System.Web.Mvc;
using AtomicSeller.Models;
using System.Net;
using System.Data.Entity.SqlServer;
//using NLog.Extensions.Logging;

namespace AtomicSeller
{
    public class Tools
    {

        public static double StringToDouble(string Value)
        {
            Value = Regex.Replace(Value, ",", ".");
            if (string.IsNullOrEmpty(Value)) return 0;
            decimal DValue = 0;
            try
            {
                DValue = decimal.Parse(Value, new CultureInfo("en-US"));
            }
            catch
            {
                DValue = 0;
            }
            return Convert.ToDouble(DValue);
        }

        public static string RoundStringValue(string Value)
        {
            int IntValue = (int)StringToDouble(Value);
            return IntValue.ToString();
        }




        public static decimal ConvertStringToDecimal(string Value)
        {
            if (string.IsNullOrEmpty(Value)) return 0;
            Value = Regex.Replace(Value, "[^\\d,^\\.,^\\,]*", "");
            Value = Value.Replace(",", ".");
            if (string.IsNullOrEmpty(Value))
                return 0;
            else
                return Convert.ToDecimal(Value, new CultureInfo("en-US"));
        }

        public static string ConvertDateToString(DateTime? InputDate, string DateFormat=null)
        {
            DateTime _InputDate;
            if (InputDate == null)
                _InputDate = new DateTime(1900, 1, 1);
            else
                _InputDate = (DateTime)InputDate;

            if (DateFormat==null)
                return _InputDate.ToString("yyyyMMdd");
            else
                return _InputDate.ToString(DateFormat);
        }

        public static string ConvertDateToString(DateTime InputDate, string DateFormat = null)
        {
            if (DateFormat == null)
                return InputDate.ToString("yyyyMMdd");
            else
                return InputDate.ToString(DateFormat);
        }

        public static DateTime ConvertStringToDate(string InputString, string _Culture = null)
        {
            List<string> formats = new List<string>();

            if (string.IsNullOrEmpty(_Culture)) _Culture = "";
            switch (_Culture)
            {
                case "fr-FR":
                    //"21\/02\/2019"
                    formats.Add("dd/MM/yyyy");
                    formats.Add("dd/M/yyyy");
                    formats.Add("d/M/yyyy");
                    formats.Add("d/MM/yyyy");
                    formats.Add("dd/MM/yy");
                    formats.Add("dd/M/yy");
                    formats.Add("d/M/yy");
                    formats.Add("d/MM/yy");
                    formats.Add("ddd dd MMM yyyy h:mm tt zzz");
                    formats.Add("dd-MM-yyyy");
                    formats.Add("dd/MM/yyyy HH:mm:ss");
                    //"MM/dd/yyyy HH:mm:ss",
                    formats.Add("dd/MM/yyyy HH:mm");
                    //"MM/dd/yyyy HH:mm"
                    formats.Add("yyyy-MM-dd HH:mm:ss");
                    break;
                default:
                    //"21\/02\/2019"
                    formats.Add("dd/MM/yyyy");
                    formats.Add("dd/M/yyyy");
                    formats.Add("d/M/yyyy");
                    formats.Add("d/MM/yyyy");
                    formats.Add("dd/MM/yy");
                    formats.Add("dd/M/yy");
                    formats.Add("d/M/yy");
                    formats.Add("d/MM/yy");
                    formats.Add("ddd dd MMM yyyy h:mm tt zzz");
                    formats.Add("dd-MM-yyyy");
                    formats.Add("dd/MM/yyyy HH:mm:ss");
                    //"MM/dd/yyyy HH:mm:ss",
                    formats.Add("dd/MM/yyyy HH:mm");
                    //"MM/dd/yyyy HH:mm"
                    formats.Add("yyyy-MM-dd HH:mm:ss");
                    break;
            }


            DateTime ResultDate;

            //            if (!DateTime.TryParse(InputString, out ResultDate))
            if (DateTime.TryParseExact(InputString, formats.ToArray(),
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out ResultDate))
                return ResultDate;

            //if (DateTime.TryParse(InputString, out ResultDate)) return ResultDate;

            return new DateTime(1900, 1, 1);
        }


        public static string FixedLength(string input, int length)
        {
            if (string.IsNullOrEmpty(input))
                return new string(' ', length);

            if (input.Length > length)
                return input.Substring(0, length);
            else
                return input.PadRight(length, ' ');
        }

            public static string Truncate(string input, int maxLength)
            {
                if (string.IsNullOrEmpty(input)) return input;
                return input.Length <= maxLength ? input : input.Substring(0, maxLength);
            }


        public static string GetRandomString()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", ""); // Remove period.
            return path;
        }


        public static Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly TempDataDictionary tempData; 

        public static void ErrorHandler(string Message, Exception ex, bool DisplayMessage, bool TraceLog, bool DisplayTrace, BaseController controller = null)
        {
            if (DisplayTrace && ex != null && controller != null)
                controller.Flash(new FlashMessage(Message + "\n" + ex.Message + "\n" + ex.StackTrace, FlashMessageType.Error));
            else if (DisplayMessage && controller != null)
                controller.Flash(new FlashMessage(Message, FlashMessageType.Error));

            if (TraceLog)
            {
                if (logger == null) logger = NLog.LogManager.GetCurrentClassLogger();

                // Database
                logger = LogManager.GetLogger("databaseLogger");

                if (ex == null)
                {
                    try
                    {
                    }
                    catch
                    {
                        logger.Debug(Message + "\n");
                    }
                }
                else
                    try
                    {
                        logger.Error(ex, Message + "\n" + ex.Message + "\n" + ex.InnerException.Message + "\n" + ex.StackTrace);
                    }
                    catch (Exception ex2)
                    {
                        try
                        {
                            logger.Error(ex2, Message + "\n" + ex.Message + "\n" + ex.StackTrace);
                        }
                        catch
                        {
                            logger.Error(Message + "\n");
                        }
                    }

            }
        }

        public static string ReportLogFile()
        {
            string fileName = null;
            try
            {

                var fileTarget = (FileTarget)LogManager.Configuration.FindTargetByName("file");
                // Need to set timestamp here if filename uses date. 
                // For example - filename="${basedir}/logs/${shortdate}/trace.log"
                var logEventInfo = new LogEventInfo { TimeStamp = DateTime.Now };
                fileName = fileTarget.FileName.Render(logEventInfo);
                if (!File.Exists(fileName))
                    throw new Exception(Local.TranslatedMessage("LOGFILENOTEXIST"));
                return fileName;
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
                //Tools.ErrorHandler("", ex, false, true, false);
                return fileName;
            }
            //M essageBox.Show(fileName);
            //logger.logddir ArchiveFileName = { '${specialfolder:folder=ApplicationData}/Atomic//AtomicSeller${shortdate}.{##}.log'}
        }




        

        public static string Base64Encode(string pdfPath)
        {
            byte[] pdfBytes = null;

            try
            {
                pdfBytes = File.ReadAllBytes(pdfPath);
            }
            catch
            {
                WebClient _Client = new WebClient();
                   pdfBytes = _Client.DownloadData(pdfPath);
            }

            string pdfBase64 = Convert.ToBase64String(pdfBytes);
            //string pdfBase64 = Convert.ToBase64String(pdfBytes);


            //var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            //return System.Convert.ToBase64String(plainTextBytes);
            return pdfBase64;
        }

        public static bool CheckValidLicence(int StoreType)
        {
            return true;
        }
        public static void LicenceUpdateNbOrders(int NbOrders, string OrdersDate)
        {
        }
        public static void LicenceWriteEncryptedToProperties()
        {
        }
        public static int NbOrdersLeft_Get()//int StoreType)
        {
            return 10000;
        }

        public static List<string> ExtractEmails(string textToScrape)
        {
            Regex reg = new Regex(@"[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,6}", RegexOptions.IgnoreCase);
            Match match;

            List<string> results = new List<string>();
            for (match = reg.Match(textToScrape); match.Success; match = match.NextMatch())
            {
                if (!(results.Contains(match.Value)))
                    results.Add(match.Value);
            }

            return results;
        }



    }
}
