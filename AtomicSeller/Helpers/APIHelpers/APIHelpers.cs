using AtomicSeller;
using AtomicSeller.Helpers;
using AtomicSeller.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrestashopAPI.Helpers
{
    public static class Helper
    {

        private static int TOKEN_LENGTH = 24;
        public static bool ValidateToken(string Token)
        {
            bool result = false;
            if (Token.Length > TOKEN_LENGTH)
            {
                result = false;
            }
            else
            {
                //result = ApplicationWrapper.TokenList.Exists(x => x.Equals(Token));
                result = CheckToken(Token);
            }

            return result;
        }

        private static bool CheckToken(string Token)
        {
            return true;

            //User user = null;
            //Clients Client = null;

            //try
            //{
            //    using (var db = new AtomicLoginDataEntities())
            //    {
            //        Client = db.Clients.FirstOrDefault(c => c.Token == Token);
            //        user = db.User.FirstOrDefault(u => u.ClientID == Client.ClientID);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Tools.ErrorHandler(ex.Message + " " + ex.InnerException + "\n" + ex.StackTrace, null, false, true, false);
            //    return false;
            //}

            //// Authentification fail
            //if (user == null)
            //{
            //    Tools.ErrorHandler("Token not found", null, false, true, false);
            //    return false;
            //}
            //else
            //{
            //    System.Web.SessionState.HttpSessionState session = HttpContext.Current.Session;

            //    var sessionBag = SessionBag.Instance;

            //    sessionBag = SessionBag.CreateNew(session);

            //    sessionBag.ConnectionString = Client.ConnectionString;
            //    sessionBag.TenantDirectory = Client.TenantDirectory;

            //    Tools.ErrorHandler("API " + Client.CompanyName + " " + user.UserName + " connects to : " + sessionBag.TenantDirectory, null, false, true, false);

            //    return true;
            //}

        }

    }
}