using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderSys.Models
{
    public class ProductView
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Stock { get; set; }
    }
    public class CustomerView
    {
        public int CustomerID { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
    public class OrderView
    {
        public int OrderID { get; set; }
        public string ContactName { get; set; }
        public List<Order_DetailView> Order_Details { get; set; }
        public System.DateTime OrderDate { get; set; }
    }
    public class OrderRequest
    {
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public List<Product_Quantity> Order_Details { get; set; }
    }
    public class Pre_OrderView
    {
        public int ProductID { get; set; }
        public string Mail { get; set; }
    }
    public class Order_DetailView
    {
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
    public class Product_Quantity
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }
    }
}