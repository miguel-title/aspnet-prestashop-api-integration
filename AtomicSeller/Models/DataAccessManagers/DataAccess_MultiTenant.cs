using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data.Entity;
using AtomicSeller.Models;
using AtomicSeller.ViewModels;
using AtomicSeller.Controllers;
using AtomicSeller.Helpers;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace AtomicSeller
{
    public class DA_MT_TEST
    {
        public enum ShippingCarrierType : int
        {
            None = 0,
            Labels = 1,
            Enveloppes = 2,
            ExcelExport = 3,
            Colissimo = 4,
            MondialRelay = 5,
            UPS = 6,
            RoyalMail = 7,
            DHLExpress = 8,
            RelaisColis = 9,
            DHLParcel = 10
        }


        public static string CleanFieldForXML(string Field)
        {
            if (Field == null) return null;
            Field = Field.Replace(",", "");
            Field = Field.Replace("'", " ");
            Field = Field.Replace("&", "&amp;");
            Field = Field.Replace("<", "&lt;");
            Field = Field.Replace(">", "&gt;");
            Field = Field.Replace("\"", "&quot;");

            return (Field);

        }

        public static string CleanFieldForSQLServer(string Field)
        {
            //ajoute un anti - slash aux caractères suivants: NULL, \x00, \n, \r, \, ', " et \x1a.
            //if (Field.Contains ("ancienne"))
            //{
            //    MessageBox.Show(Field + "\n" + Field.Replace("'", "\\'"));
            //}
            if (Field == null) return null;
            Field = Field.Replace(",", "");
            Field = Field.Replace("'", " "); // !!!!!!!!!!
            Field = Field.Replace("  ", " ");
            Field = Field.Replace("\"", "");
            Field = Field.Replace("/", " ");
            return (Field);
            //return (Field.Replace("'", "\\\'"));
        }


        #region Order
        
        
        public static List<OrderDM> GetAllOrders()
        {
            return null;
        }
        
        public static List<OrderDM> GetOrdersByStatus(List<string> Order_StatusList, string StoreName)
        {
            return null;  
        }

   
        #endregion

        #region Shipment
        
        public static IEnumerable<string> ExtractValidationMessages(DbContext context)
        {
            var errors = context.GetValidationErrors();
            foreach (var error in errors)
            {
                yield return "Validation error on " + error.Entry.Entity.GetType().FullName + "{" + error.Entry.Entity.ToString() + "}";
                foreach (var errorMessage in error.ValidationErrors)
                {
                    yield return "Property " + errorMessage.PropertyName + " " + errorMessage.ErrorMessage;
                }
            }
        }

        public static string ValidationErrorMessage(DbContext context)
        {
            var toto = ExtractValidationMessages(context);
            string detail = string.Empty;

            foreach (var titi in toto) detail += titi + "\n";

            return detail;
        }




        

        public static List<ShipmentDM> GetAllShipments()
        {
            return null;
        }
        #endregion

     
        #region Orderproduct

        public static List<OrderProductDM> GetOrdersProductsByOrderID(int OrderID)
        {
            return null;
        }

        #endregion


        public static bool TestExistingShipment(int ShipmentID)
        {

            return false;
        }

        public static bool TestExistingOrderProduct(int OrderProductID)
        {

            return false;
        }

        public static int TestExistingOrder(string OrderKey, string OrderType=null, string Order_StoreType=null, string MerchantKey = null)
        {
            if (string.IsNullOrEmpty(OrderKey)) return 0;

            return 0;
        }



        public static InvoiceDM GetInvoiceByOrderID(int OrderID)
        {
            return null;
        }

        public static InvoiceDM GetInvoiceByInvoiceNr(string InvoiceNr)
        {

            return null;
        }

        


        public static void InsertUpdateInvoice(InvoiceDM InvoiceTMP)
        {
        }

        public static int InsertInvoice(InvoiceDM InvoiceTMP)
        {
            return 0;
        }



        public static List<CSOPIVM> LoadCSOPIVMsFromDB(DateTime? CreationDate=null, DateTime? LastModifiedDate = null, string OrderType=null)
        {
            List<CSOPIVM> _CSOPIVMs = new List<CSOPIVM>();

            CSOPIVM _CSOPIVM = null;

            OrderDM _Order = new ViewModels.OrderDM();
            _Order.OrderKey = "1234";
            _Order.Order_Status = "Processing";
            _Order.Purchase_date = DateTime.Today;
            _Order.Payment_Date = DateTime.Today;

            ShipmentDM _Shipment = new ShipmentDM();

            _Shipment.DeliveryInstructions1 = "lhiuguykf";
            //_Shipment.Shipment_ID = "";
            //_Shipment.ShippingService = "";
            _Shipment.ShippingDate = DateTime.Today;
            _Shipment.ShippingPoint = "";
            _Shipment.TrackingNumber = "";
            _Shipment.ShipmentStatus = "T";
            _Shipment.ParcelWeight = "";
            _Shipment.Signed = "1";
            _Shipment.ParcelValue = "";
            _Shipment.ParcelInsuranceValue = "";
            _Shipment.LabelPath = "";
            _Shipment.DepositDate = DateTime.Today;
            _Shipment.MailboxPicking = "";
            _Shipment.MailboxPickingDate = DateTime.Today;
            _Shipment.recommendationLevel = "";
            _Shipment.nonMachinable = "";
            _Shipment.DeliveryAvisage = "";
            _Shipment.DeliveryInstructions1 = "";
            _Shipment.DeliveryInstructions2 = "";
            _Shipment.DeliveryInstructions3 = "";
            _Shipment.DeliveryRelayCountryCode = "";
            _Shipment.DeliveryRelayNumber = "";
            _Shipment.DeliveryReturn = "";
            _Shipment.DeliveryMontage = "";
            _Shipment.pickupLocationId = "";
            _Shipment.RecipCompanyName = "";
            _Shipment.RecipAdr0 = "";
            _Shipment.RecipAdr1 = "";
            _Shipment.RecipAdr2 = "";
            _Shipment.RecipAdr3 = "";
            _Shipment.RecipZip = "";
            _Shipment.RecipCity = "";
            _Shipment.RecipCountryISOCode = "";
            _Shipment.RecipPhoneNumber = "";
            _Shipment.RecipMobileNumber = "";
            _Shipment.RecipFirstName = "";
            _Shipment.RecipLastName = "";
            _Shipment.RecipEmail = "";
            _Shipment.RecipCompanyCode = "";
            _Shipment.RecipCustomerCode = "";
            _Shipment.RecipLanguageCode = "";
            _Shipment.RecipCountryLib = "";
            _Shipment.RecipDoorCode1 = "";
            _Shipment.RecipDoorCode2 = "";
            _Shipment.RecipIntercom = "";
            _Shipment.RecipStage = "";
            _Shipment.RecipInhabitationType = "";
            _Shipment.RecipElevator = "";
            _Shipment.SenderName = "";
            _Shipment.SenderAddr1 = "";
            _Shipment.SenderAddr2 = "";
            _Shipment.SenderAddr3 = "";
            _Shipment.SenderZip = "";
            _Shipment.SenderCity = "";
            _Shipment.SenderCountryLib = "";
            _Shipment.SendercountryCode = "";
            _Shipment.SenderDoorCode1 = "";
            _Shipment.SenderDoorCode2 = "";
            _Shipment.Senderintercom = "";
            _Shipment.SenderRelayCountry = "";
            _Shipment.SenderRelayNumber = "";
            _Shipment.SenderCompanyName = "";
            _Shipment.SenderPhoneNumber = "";
            _Shipment.SenderEmail = "";
            _Shipment.ProductCode = "";
            _Shipment.ErrorMessage = "";
            _Shipment.ErrorStatus = "";
            _Shipment.ErrorCode = "";
            _Shipment.ProductCategory = "";
            _Shipment.senderParcelRef = "";
            _Shipment.CustomsDeclarations = "";
            _Shipment.CustomsDeclarationsBase64 = null;
            _Shipment.CustomDeclarationPath = "";
            _Shipment.EDIStatus = "";
            _Shipment.ParcelLenght = "";
            _Shipment.ParcelHeight = "";
            _Shipment.ParcelWidth = "";
            _Shipment.ParcelSizeCode = "";
            _Shipment.ParcelVolume = "";
            _Shipment.WarehouseID = "";
            _Shipment.InsurranceYN = "";
            _Shipment.InsurranceCurrency = "";
            _Shipment.ParcelValueCurrency = "";
            _Shipment.LabelFileFormat = "";
            _Shipment.ShipmentIdentificationNumber = "";
            _Shipment.TrackingInfo = "";
            _Shipment.ShippingAmount = "";
            _Shipment.Shipping_method_id = "";
            _Shipment.Shipping_method_title = "";
            _Shipment.parcelNumberPartner = "";
            _Shipment.ShippingTax = "";
            _Shipment.MerchantKey = "";
            _Shipment.SpecialServiceTypeCode = "";

            OrderProductDM _OrderProduct = new OrderProductDM();

            _OrderProduct.OrderProductID = 1;
            _OrderProduct.ItemID = "";
            _OrderProduct.SKU = "";
            _OrderProduct.ProductName = "";
            _OrderProduct.Quantity = 1;
            _OrderProduct.Price = "";
            _OrderProduct.Tax = "";
            _OrderProduct.Weight = "";
            _OrderProduct.CN23CategoryID = 3;
            _OrderProduct.PriceExclTax = "";
            _OrderProduct.Rate = "";
            _OrderProduct.SubTotalPriceExclTax = "";
            _OrderProduct.SubTotalTax = "";
            _OrderProduct.EANCode = "";
            _OrderProduct.Width = "";
            _OrderProduct.Depth = "";
            _OrderProduct.Length = "";
            _OrderProduct.VariationID = "";
            _OrderProduct.BundleID = "";
            _OrderProduct.OrderID = "1";

            //Invoice _Invoice = _QueryElement.Invoice;

            _CSOPIVM.ShipmentsVM.First().Shipment = _Shipment;
            _CSOPIVM.ShipmentsVM.First().OrdersVM.First().Order = _Order;
            _CSOPIVM.ShipmentsVM.First().OrdersVM.First().Products.Add(_OrderProduct);

            _CSOPIVMs.Add(_CSOPIVM);

            return _CSOPIVMs;
        }


    }

}