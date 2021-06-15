using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AtomicSeller.Models;
using Bukimedia.PrestaSharp.Entities;
using Bukimedia.PrestaSharp.Factories;
using Bukimedia.PrestaSharp;

namespace PrestashopOrderAPI.Models
{
    public class GetOrdersRequest
    {
        public Header Header { get; set; }
        public GetOrdersData Request { get; set; }
    }

    public class PutOrderStatusRequest
    {
        public Header Header { get; set; }
        public PutOrderStatusData Request { get; set; }

    }
    public class Header
    {
        public string Token { get; set; }
    }

    public class GetOrdersData
    {
        public string OrderStartDate { get; set; }
        public string OrderEndDate { get; set; }
        public string LanguageCode { get; set; }
        public string OrderStatus { get; set; }

    }

    public class PutOrderStatusData
    {
        public string OrderID { get; set; }
        public string ShippingID { get; set; }
        public string LanguageCode { get; set; }
        public string TrackingNumber { get; set; }
        public string TrackingUrl { get; set; }

    }

    public class PutOrderStatusResponse
    {
        public ResponseHeader Header { get; set; }
    }

    public class GetOrdersResponse
    {
        public ResponseHeader Header { get; set; }
        public OrdersResponse Response { get; set; }
    }

    public class ResponseHeader
    {
        public string RequestStatus { get; set; }

        public string ReturnCode { get; set; }

        public string ReturnMessage { get; set; }

        public string LanguageCode { get; set; }
    }

    public class OrdersResponse
    {
        public List<Order> Orders { get; set; }
    }


    public class PutOrderParam
    {
        public string tracking_number { get; set; }
        public string id { get; set; }
    }

    public class Order
    {
        public order _Order;
        public address ShippingAddress;
        public address BillingAddress;
        public customer Customer;
        public carrier Shipping;
        public order_state OrderState;
        public currency Currency;
        public country Country;
     }

}