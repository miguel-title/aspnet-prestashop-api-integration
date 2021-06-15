using AtomicSeller.Models;
using AtomicSeller.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using PrestashopOrderAPI.Models;
using Bukimedia.PrestaSharp.Entities;
using Bukimedia.PrestaSharp.Factories;
using Bukimedia.PrestaSharp;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace AtomicSeller
{
    class Prestashop
    {
        //private static string BaseUrl = "http://ahairdo.online/atomic/api";
        //private static string Account = "GY8LUW2LYT5D2Q8168G86WLUY56RNATF";
        private static string Password = "";


        //private static string BaseUrl = "https://www.testfa.clic-and-see.com/api";
        //private static string Account = "6P7TMXG66Y148Q9EWDQ54THVDZHF5MQD";
        private static string Token = "799f708c0aea75c56d4ebf25ace6c4aa";
        private static string Username = "user@user.com";
        private static string UserPassword = "useruser";
        private static string BaseUrl = "https://www.importmarketsales.com";
        private static string Account = "IMF9PF4YC6L682TQHBI3HL8KLL8SZ5NC";


        [HttpPost]
        public GetOrdersResponse GetOrders(GetOrdersRequest OrderRequest)
        {
            GetOrdersResponse _GetOrdersResponse = new GetOrdersResponse();

            _GetOrdersResponse.Response = new OrdersResponse();
            _GetOrdersResponse.Response.Orders = BuildOrdersList(OrderRequest.Request.OrderStatus, OrderRequest.Request.OrderStartDate, OrderRequest.Request.OrderEndDate);

            if (_GetOrdersResponse.Header == null)
                _GetOrdersResponse.Header = new ResponseHeader
                {
                    LanguageCode = "En",
                    RequestStatus = "Ok",
                    ReturnCode = "AS0000",
                    ReturnMessage = _GetOrdersResponse.Response.Orders.Count().ToString() + " order(s) found"
                };

            return _GetOrdersResponse;
        }

        [HttpPost]
        public PutOrderStatusResponse PutOrderStatus(PutOrderStatusRequest PutRequest)
        {

            PutOrderStatusResponse _PutOrderStatusResponse = new PutOrderStatusResponse();

            String result = PutOrderShipmentStatus(PutRequest.Request.OrderID, PutRequest.Request.ShippingID, PutRequest.Request.TrackingNumber, PutRequest.Request.TrackingUrl);

            if (_PutOrderStatusResponse.Header == null)
                _PutOrderStatusResponse.Header = new ResponseHeader
                {
                    LanguageCode = "En",
                    RequestStatus = "Ok",
                    ReturnCode = "AS0000",
                    ReturnMessage = result
                };

            return _PutOrderStatusResponse;
        }

        public String PutOrderShipmentStatus(String OrderID, String ShippingID, String TrackingNumber, String TrackingUrl)
        {

            if (TrackingNumber == null)
            {
                string res = "Traking Number is empty!";
                return res;
            }

            OrderFactory orderFactory = new OrderFactory(BaseUrl, Account, Password);
            order _Order = orderFactory.Get(Convert.ToInt64(OrderID));
            _Order.shipping_number = TrackingNumber;
            orderFactory.Update(_Order);

            return "Success";
        }

        private List<Order> BuildOrdersList(string OrderStatus = null, string OrderStartDate = null, string OrderEndDate = null)
        {
            Dictionary<string, string> filter = new Dictionary<string, string>();
            if (OrderStartDate != null && OrderEndDate != null)
            {
                DateTime StartDate = Convert.ToDateTime(OrderStartDate);
                DateTime EndDate = Convert.ToDateTime(OrderEndDate);
                string dFrom = string.Format("{0:yyyy-MM-dd HH:mm:ss}", StartDate);
                string dTo = string.Format("{0:yyyy-MM-dd HH:mm:ss}", EndDate);
                filter.Add("date_add", "[" + dFrom + "," + dTo + "]");
            }

            // SSL
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            //ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

            List<Order> OrderList = new List<Order>();
            try
            {
                OrderFactory orderFactory = new OrderFactory(BaseUrl, Account, Password);

                List<order> orders = new List<order>();
                var _GetOrders = orderFactory.GetAllAsync();
                TaskStatus status = _GetOrders.Status;

                if (status.ToString() == "WaitingForActivation")
                {
                    return OrderList;
                }
                if (_GetOrders != null)
                    orders = _GetOrders.Result;

                orders = orderFactory.GetByFilter(filter, "id_DESC", null);

                if (orders!=null)
                foreach (order order in orders)
                {
                    Order _Order = new Order();
                    _Order._Order = order;
                    AddressFactory addressFactory = new AddressFactory(BaseUrl, Account, Password);
                    if (order.id_address_delivery != null)
                    {
                        _Order.ShippingAddress = addressFactory.Get((long)order.id_address_delivery);
                        if (_Order.ShippingAddress.id_country != null)
                        {
                            CountryFactory countryFactory = new CountryFactory(BaseUrl, Account, Password);
                            _Order.Country = countryFactory.Get((long)_Order.ShippingAddress.id_country);
                        }
                    }
                    if (order.id_address_invoice != null)
                        _Order.BillingAddress = addressFactory.Get((long)order.id_address_invoice);
                    if (order.id_customer != null)
                    {
                        CustomerFactory customerFactory = new CustomerFactory(BaseUrl, Account, Password);
                        _Order.Customer = customerFactory.Get((long)order.id_customer);
                    }
                    CarrierFactory carrierFactory = new CarrierFactory(BaseUrl, Account, Password);
                    if (order.id_carrier != null)
                        _Order.Shipping = carrierFactory.Get((long)order.id_carrier);
                    OrderStateFactory orderStateFactory = new OrderStateFactory(BaseUrl, Account, Password);
                    if (order.current_state != null)
                        _Order.OrderState = orderStateFactory.Get((long)order.current_state);
                    CurrencyFactory currencyFactory = new CurrencyFactory(BaseUrl, Account, Password);
                    if (order.id_currency != null)
                        _Order.Currency = currencyFactory.Get((long)order.id_currency);

                    OrderList.Add(_Order);
                }
            }
            catch (WebException Wex)
            {
                string ErrorMessage = "Prestashop Update Order State error: ";
                ErrorMessage += Wex.Message;
            }
            catch (Exception ex)
            {
                string ErrorMessage = "Prestashop Update Order State error: ";
            }

            return OrderList;
        }
 

        public static List<CSOPIVM> PrestaShop_GetOrdersToCSOPIVM(string OrderStatus, string OrderStartDate = null, string OrderEndDate = null)
        {

            DateTime orderStartDate = Tools.ConvertStringToDate(OrderStartDate, "fr-FR");
            DateTime orderEndDate = Tools.ConvertStringToDate(OrderEndDate, "fr-FR");

            AtomicSeller.ViewModels.CSOPIVM _CSOPIVM = null;
            using (var client = new HttpClient())
            {
                Prestashop _BuildOrdersList = new Prestashop();
                List<Order> OrderList = _BuildOrdersList.BuildOrdersList(OrderStatus, OrderStartDate, OrderEndDate);

                _CSOPIVM = new ViewModels.CSOPIVM();
                _CSOPIVM.InitFirst();

                for (int i = 0; i < OrderList.Count(); i++)
                {
                    OrderVM order = new OrderVM();
                    order.Order = new OrderDM();
                    order.Order.Address_Street1 = "";
                    order.Order.Address_Street2 = "";
                    order.Order.Address_Street3 = "";
                    if (OrderList[i].ShippingAddress != null)
                    {
                        order.Order.Buyer_s_Phone = OrderList[i].ShippingAddress.phone;
                        order.Order.Ebay_Buyer_first_name = OrderList[i].ShippingAddress.firstname;
                        order.Order.EBAY_Buyer_UserLastName = OrderList[i].ShippingAddress.lastname;
                        order.Order.postal_code = OrderList[i].ShippingAddress.postcode;
                        order.Order.Shipping_last_name = OrderList[i].ShippingAddress.lastname;
                        order.Order.ship_city = OrderList[i].ShippingAddress.city;
                        order.Order.ship_country = OrderList[i].Country.name[0].Value;
                        order.Order.Ship_phone_number = OrderList[i].ShippingAddress.phone;
                    }
                    order.Order.CheckoutMessage = "";
                    order.Order.CreationDate = Convert.ToDateTime(OrderList[i]._Order.date_add);
                    order.Order.Currency = OrderList[i].Currency.name;
                    order.Order.FulfillmentChannel = "";
                    order.Order.MerchantKey = "";
                    order.Order.ModificationDate = new DateTime();
                    order.Order.OrderID = (long)OrderList[i]._Order.id;
                    // order.Order.OrderID_Ext = OrderList[i].;
                    order.Order.OrderKey = "";
                    order.Order.OrderType = "";
                    order.Order.Order_BuyerUserID = "";
                    if (OrderList[i].Customer != null)
                        order.Order.Order_CustomerID = OrderList[i].Customer.id.ToString();
                    order.Order.Order_Shipment_ID = 0;
                    order.Order.Order_Status = OrderList[i].OrderState.name[0].Value;
                    
                    order.Order.PaymentReferenceID = ""; // or payment_mode
                    order.Order.Payment_Date = new DateTime();
                    order.Order.Purchase_date = new DateTime();
                    order.Order.SalesRecordNumber = "12";
                    order.Order.ShipmentTrackingNumber = OrderList[i]._Order.shipping_number;
                    order.Order.ShippingAddress_Name = "";
                    order.Order.ShippingCarrierUsed = OrderList[i].Shipping.name;
                    order.Order.shipping_price = OrderList[i]._Order.total_shipping.ToString();

                    order.Order.ship_service_level = "";
                    order.Order.Store_Name = "";
                    order.Order.Store_Type = "";
                    order.Order.TransactionPrice = "";

                    var invoice = new InvoiceDM();
                    if (OrderList[i].BillingAddress != null)
                    {
                        invoice.BillingAdr0 = OrderList[i].BillingAddress.address1;
                        invoice.BillingCity = OrderList[i].BillingAddress.city;
                        invoice.BillingCompany = OrderList[i].BillingAddress.company;
                        invoice.BillingCountry = OrderList[i].Country.name[0].Value;
                        invoice.BillingCountryCode = OrderList[i].Country.iso_code;
                        invoice.BillingFirstName = OrderList[i].BillingAddress.firstname;
                        invoice.BillingLastName = OrderList[i].BillingAddress.lastname;
                        invoice.BillingPhone = OrderList[i].BillingAddress.phone;

                        invoice.BillingZipCode = OrderList[i].BillingAddress.postcode;
                    }
                    invoice.Currency = OrderList[i].Currency.name;
                    if (OrderList[i].Customer != null)
                        invoice.CustomerID = OrderList[i].Customer.id.ToString();
                    invoice.customerMarking = "";
                    invoice.delTerms = "";
                    invoice.delTransportTerms = "";
                    invoice.InvoiceBase64 = "";
                    invoice.InvoiceID = 12;
                    invoice.InvoiceDate = Convert.ToDateTime(OrderList[i]._Order.date_add);
                    invoice.InvoiceNr = "";
                    invoice.OrderID = OrderList[i]._Order.id.ToString();
                    invoice.OrderKey = "";
                    invoice.orderReference = "";
                    invoice.payTerms = "";
                    invoice.salesman = "";
                    invoice.TotalAmount = OrderList[i]._Order.total_paid_real.ToString();
                    invoice.TotalExclVAT = OrderList[i]._Order.total_paid_tax_excl.ToString();

                    invoice.TotalShipping = OrderList[i]._Order.total_shipping.ToString();
                    order.Invoices = new List<InvoiceDM>();
                    order.Invoices.Add(invoice);

                    order.Products = new List<OrderProductDM>();
                    Bukimedia.PrestaSharp.Entities.AuxEntities.AssociationsOrder shippings = OrderList[i]._Order.associations;
                   
                    for (int j = 0; j < shippings.order_rows.Count(); j++)
                    {
                        OrderProductDM product = new OrderProductDM();
                        product.BundleID = "";
                        product.CN23CategoryID = 0;
                        product.Depth = "";
                        product.EANCode = "";
                        product.ItemID = "";
                        product.Length = "";
                        product.OrderID = order.Order.OrderID.ToString();
                        product.OrderProductID = (int)shippings.order_rows[j].product_id;//sku.id;
                        product.Price = shippings.order_rows[j].product_price.ToString();
                        product.PriceExclTax = shippings.order_rows[j].unit_price_tax_excl.ToString();
                        product.ProductName = shippings.order_rows[j].product_name;
                        product.Quantity = shippings.order_rows[j].product_quantity;
                        product.Rate = "";
                        product.SKU = shippings.order_rows[j].product_id.ToString();
                        product.SubTotalPriceExclTax = "";
                        product.SubTotalTax = "";
                        product.Tax = "";
                        product.VariationID = "";
                        product.Width = "";
                        order.Products.Add(product);
                    }
                    _CSOPIVM.ShipmentsVM.First().OrdersVM.Add(order);
                }

            }
            //}

            List<CSOPIVM> _CsopVMs = new List<CSOPIVM>();
            if (_CSOPIVM != null)
                _CsopVMs.Add(_CSOPIVM);

            return (_CsopVMs);

        }


        public static string PrestaShop_PutTrackingInfo(String OrderID = null, String ShippingID = null, String TrackingNumber = null, String TrackingUrl = null)
        {
            if (TrackingNumber == null)
            {
                string res = "Traking Number is empty!";
                return res;
            }

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            try
            {
                OrderFactory orderFactory = new OrderFactory(BaseUrl, Account, Password);
                
                order _Order = orderFactory.Get(Convert.ToInt64(OrderID));
                //_Order.shipping_number = TrackingNumber;
                //orderFactory.Update(_Order);

                CarrierFactory CarrierFactory = new CarrierFactory(BaseUrl, Account, Password);

                carrier _Carrier = CarrierFactory.Get((long)_Order.id_carrier);

                OrderCarrierFactory OrderCarrierFactory = new OrderCarrierFactory(BaseUrl, Account, Password);

                Dictionary<string, string> filter = new Dictionary<string, string>();

                long Id_Order = (long)_Order.id;
                long Id_Carrier = (long)_Order.id_carrier;

                filter.Add("id_order", "[" + Id_Order + "]");
                filter.Add("id_carrier", "[" + Id_Carrier + "]");

                var FilteredOrderCarrier = OrderCarrierFactory.GetByFilter(filter, "id_DESC", null);

                order_carrier _OrderCarrier = FilteredOrderCarrier[0];

                _OrderCarrier.tracking_number = TrackingNumber;
                OrderCarrierFactory.Update(_OrderCarrier);

                _Order.current_state = 4; // Expédié
                //orderFactory.Update(_Order);
                orderFactory.UpdateAsync(_Order);

                //get Invoice////////////////////////////////////////////////////
                OrderInvoiceFactory _invoice = new OrderInvoiceFactory(BaseUrl, Account, Password);
                List<order_invoice> lstdata = _invoice.GetAll();
                order_invoice curorderInvoice = new order_invoice();
                foreach(order_invoice data in lstdata)
                {
                    if (data.id_order == _Order.id.Value)
                    {
                        curorderInvoice = data;
                    }
                }
                long? invoiceID = curorderInvoice.id;
                using (WebClient webClient = new WebClient())
                {
                    CookieContainer cookieJar = new CookieContainer();
                    CookieAwareWebClient http = new CookieAwareWebClient(cookieJar);
                    //webClient.Credentials = new NetworkCredential(Username, UserPassword);
                    string url = "https://www.testfa.clic-and-see.com/admintestfa/index.php?controller=AdminPdf&submitAction=generateInvoicePDF&id_order_invoice=" + invoiceID.Value.ToString() + "&token=" + Token;
                    
                    System.Collections.Specialized.NameValueCollection postData =
                        new System.Collections.Specialized.NameValueCollection()
                       {
                              { "controller", "AdminLogin" },
                              { "redirect", "AdminPdf" },
                              { "email", Username },
                              { "passwd", UserPassword },
                              { "submitLogin", "1" }
                       };
                    string pagesource = Encoding.UTF8.GetString(http.UploadValues(url, postData));


                    http.DownloadFile(new Uri(url), @"D:\invoice_" + OrderID + ".pdf");
                }

                ///////////////////////////////////////////////////
                
                return "Success";
            }
            catch (WebException Wex)
            {
                string ErrorMessage = "Prestashop Update Order State error: ";
                ErrorMessage += Wex.Message;
                return "Error";
            }
            catch (Exception ex)
            {
                string ErrorMessage = "Prestashop Update Order State error: ";
                return "Error";
            }

            ////

            return "Success";
        }   
    }

    public class CookieAwareWebClient : WebClient
    {
        public CookieContainer CookieContainer { get; set; }
        public Uri Uri { get; set; }

        public CookieAwareWebClient()
            : this(new CookieContainer())
        {
        }

        public CookieAwareWebClient(CookieContainer cookies)
        {
            this.CookieContainer = cookies;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            if (request is HttpWebRequest)
            {
                (request as HttpWebRequest).CookieContainer = this.CookieContainer;
            }
            HttpWebRequest httpRequest = (HttpWebRequest)request;
            httpRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            return httpRequest;
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            WebResponse response = base.GetWebResponse(request);
            String setCookieHeader = response.Headers[HttpResponseHeader.SetCookie];

            if (setCookieHeader != null)
            {
                //do something if needed to parse out the cookie.
                if (setCookieHeader != null)
                {
                    Cookie cookie = new Cookie("__utmc", "#########") { Domain = "www.testfa.clic-and-see.com" } ; //create cookie
                    this.CookieContainer.Add(cookie);
                }
            }
            return response;
        }
    }
}