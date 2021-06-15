using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace AtomicSeller
{
    public class PackLink
    {
        private static string APIKey = "738da9c77a06cd1d15c8ac3c260d1b0b1527564b60b5406f2d3cc5a3bbe7900c";

        private static string PackLinkURLProduction = "https://api.packlink.com/v1";
        private static string PackLinkURLSandbox = "https://apisandbox.packlink.com/v1";

        public static void GetPacklinkShippingRates(AtomicSeller.ViewModels.CSOPIVM _CSOPIVM)
        {

            string _ParcelHeight = _CSOPIVM.ShipmentsVM.First().Shipment.ParcelHeight;
            string _ParcelWeight = _CSOPIVM.ShipmentsVM.First().Shipment.ParcelWeight;
            string _ParcelLength = _CSOPIVM.ShipmentsVM.First().Shipment.ParcelLenght;
            string _ParcelWidth = _CSOPIVM.ShipmentsVM.First().Shipment.ParcelWidth;
            string _ParcelValue = _CSOPIVM.ShipmentsVM.First().Shipment.ParcelValue;
            string _ParcelVolume = _CSOPIVM.ShipmentsVM.First().Shipment.ParcelVolume;
            string _RecipCity = _CSOPIVM.ShipmentsVM.First().Shipment.RecipCity;
            string _RecipCountryISOCode = _CSOPIVM.ShipmentsVM.First().Shipment.RecipCountryISOCode;
            string _InsurranceYN = _CSOPIVM.ShipmentsVM.First().Shipment.InsurranceYN;


            PackLinkShippingRatesModel.Rootobject Rootobject = new PackLinkShippingRatesModel.Rootobject();

            Rootobject.from = new PackLinkShippingRatesModel.from();
            Rootobject.to = new PackLinkShippingRatesModel.to();
            Rootobject.packages = new PackLinkShippingRatesModel.package[1];
            Rootobject.packages[0] = new AtomicSeller.PackLinkShippingRatesModel.package();
            Rootobject.packages[0].height = (float)Tools.ConvertStringToDecimal(_ParcelHeight);
            Rootobject.packages[0].weight = (float)Tools.ConvertStringToDecimal(_ParcelWeight);
            Rootobject.packages[0].width = (float)Tools.ConvertStringToDecimal(_ParcelWidth);
            Rootobject.packages[0].length = (float)Tools.ConvertStringToDecimal(_ParcelLength);
            Rootobject.from.zip = _CSOPIVM.ShipmentsVM.First().Shipment.RecipZip;
            Rootobject.from.country = _CSOPIVM.ShipmentsVM.First().Shipment.RecipCountryISOCode;
            Rootobject.to.zip = _CSOPIVM.ShipmentsVM.First().Shipment.SenderZip;
            Rootobject.to.country = _CSOPIVM.ShipmentsVM.First().Shipment.SendercountryCode;

            string JSON = JsonConvert.SerializeObject(Rootobject);
            //JSON = JSON.Replace(",\"code_pays\":null", "");
            string ResponseJSON = string.Empty;

            if (!string.IsNullOrEmpty(JSON))
            {
                #region Calling Api & reading response

                AtomicSeller.PackLink obj = new AtomicSeller.PackLink();

                HttpResponseMessage resp = null;
                /*
                200 : OK. Une réponse normale est transmise.
                400 : Votre requête comporte une erreur ; vérifiez les paramètres obligatoires ainsi que le format des données. Un message d'erreur sera renvoyé avec le détail du problème. Corrigez votre requête et essayez à nouveau.
                404 : L'API demandé n'existe pas. Vérifiez l'URL.
                405 : La méthode n'est pas reconnue ; l'API MyPackLink n'utilise que les méthodes GET, POST, UPDATE, et DELETE.
                401 : Accès non autorisé ; l'authentification a échoué : la clé d'authentification n'a pas été envoyée ou n'est pas reconnue.
                500 : Erreur générale coté serveur. Un message d'erreur sera renvoyé avec le détail du problème.
                 */

                string HttpMethod = "GET";
                try
                {
                    //"Authorization", "Bearer " + accessToken);
                    resp = obj.PackLinkAPIJSONConsumer(PackLinkURLSandbox + "/services", HttpMethod, JSON, "Authorization", APIKey, null, null);

                    ResponseJSON = resp.StatusCode.ToString();

                    if (resp.StatusCode.ToString() == "OK")// If status is 200, then we will move further to read content of the API response
                    {
                        var _Response = resp.Content.ReadAsStringAsync();
                        ResponseJSON = _Response.Result;

                        PackLinkResponseModel.DVPutEnvoiResponse _DVResponse = new PackLinkResponseModel.DVPutEnvoiResponse();
                        _DVResponse = JsonConvert.DeserializeObject<PackLinkResponseModel.DVPutEnvoiResponse>(ResponseJSON);
                        //string toto = _Response.documents_supports;

                        ProcessReponse(_DVResponse);
                    }
                    else
                    {
                        var _Response = resp.Content.ReadAsStringAsync();
                        ResponseJSON = _Response.Result;
                        PackLinkErrorModel.PackLinkError _PackLinkError = JsonConvert.DeserializeObject<PackLinkErrorModel.PackLinkError>(ResponseJSON);

                        Tools.ErrorHandler(ResponseJSON + "\r\n" + JSON, null, false, true, false);
                        //return false;
                    }
                }
                catch (Exception ex)
                {
                    ResponseJSON += "Exception occured:" + ex.ToString();
                    Tools.ErrorHandler(ResponseJSON + " " + JSON, null, false, true, false);

                }
                #endregion
            }
            else
            {
                ResponseJSON += "Request is null or empty";
            }
            string ErrorMessage = ResponseJSON;
            //return false;
        }

        public HttpResponseMessage PackLinkAPIJSONConsumer(string URL, string HttpMethod, string JSON, string Key1 = null, string Value1 = null, string Key2 = null, string Value2 = null)
        {
            HttpResponseMessage Response = new HttpResponseMessage();
            HttpClient Client = new HttpClient();

            // Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "Your Oauth token");
            // Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            // Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

            if (Key1 != null) Client.DefaultRequestHeaders.Add(Key1, Value1);
            if (Key2 != null) Client.DefaultRequestHeaders.Add(Key2, Value2);
            //Client.DefaultRequestHeaders.Add("wsse:Password", "");

            Client.BaseAddress = new Uri(URL); // It is the base URL of API, which will be remain same for all the APIs, it is kept in web.config

            // Contourne pb SSL
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            // Not sure to be needed
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

            var StringContent = new StringContent(JSON, Encoding.UTF8, "application/json");

            try
            {
                if (HttpMethod == "POST")
                    Response = Client.PostAsync(URL, StringContent).Result;
                else if (HttpMethod == "GET")
                    Response = Client.GetAsync(URL).Result;
            }
            catch (Exception ex)
            {
                Tools.ErrorHandler("PackLink APIJSONConsumer error (HttpMethod=" + HttpMethod + ") JSON=(" + JSON + ")", ex, false, true, true);
            }

            return Response;
        }
        private static bool ProcessReponse(PackLinkResponseModel.DVPutEnvoiResponse _DVResponse)
        {
            return true;
        }

    }
}