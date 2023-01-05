using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OrderSys.Models;

namespace OrderSys.Controllers
{
    public class OrdersController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            using(OrderSystemEntities db = new OrderSystemEntities())
            {
                var orders = db.Orders.Select(o => new OrderView
                {
                    OrderID = o.OrderID,
                    ContactName = o.Customers.ContactName,
                    OrderDate = o.OrderDate,
                    Order_Details = db.Order_Detail.Where(od => od.OrderID == o.OrderID).Select(od => new Order_DetailView
                    {
                        ProductName = od.Products.ProductName,
                        UnitPrice = od.Products.UnitPrice,
                        Quantity = od.Quantity
                    }).ToList()
                }).ToList();
                if (orders.Any())
                    return Ok(orders);
                else
                    return NotFound();
            }
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            using (OrderSystemEntities db = new OrderSystemEntities())
            {
                var order = db.Orders.Where(o => o.OrderID == id).Select(o => new OrderView
                {
                    OrderID = o.OrderID,
                    ContactName = o.Customers.ContactName,
                    OrderDate = o.OrderDate,
                    Order_Details = db.Order_Detail.Where(od => od.OrderID == o.OrderID).Select(od => new Order_DetailView
                    {
                        ProductName = od.Products.ProductName,
                        UnitPrice = od.Products.UnitPrice,
                        Quantity = od.Quantity
                    }).ToList()
                }).SingleOrDefault();
                if (order == null)
                    return NotFound();
                else
                    return Ok(order);
            }
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody]OrderRequest value)
        {
            using (OrderSystemEntities db = new OrderSystemEntities())
            {
                var customer = db.Customers.SingleOrDefault(c => c.Phone == value.Phone);   //用電話辨別客戶
                if (customer == null)   //新增至客戶資料表
                {
                    customer = new Customers
                    {
                        CompanyName = value.CompanyName,
                        ContactName = value.ContactName,
                        Address = value.Address,
                        Phone = value.Phone
                    };
                    db.Customers.Add(customer);
                    db.SaveChanges();   //令資料庫顧客ID自動生成
                }
                var order = new Orders
                {
                    Customers = customer,
                    OrderDate = DateTime.Now,
                };
                db.Orders.Add(order);
                db.SaveChanges();   //令訂單ID自動生成

                foreach(Product_Quantity pq in value.Order_Details) //將每筆產品與數量加入Order_Details資料表
                {
                    var order_detail = new Order_Detail
                    {
                        Orders = order,
                        ProductID = pq.ProductID,
                        Quantity = pq.Quantity
                    };
                    var product = db.Products.SingleOrDefault(p => p.ProductID == pq.ProductID);
                    product.Stock -= pq.Quantity;   //庫存減少
                    db.Order_Detail.Add(order_detail);
                }
                db.SaveChanges();
                return Ok();
            }
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            using (OrderSystemEntities db = new OrderSystemEntities())
            {
                var order = db.Orders.SingleOrDefault(o => o.OrderID == id);
                if (order == null)
                    return NotFound();
                else
                {
                    db.Orders.Remove(order);    //已於MSSQL內設定串聯刪除，可以連帶將Order_Details表內屬於此訂單的資料刪除
                    db.SaveChanges();
                    return Ok();
                }
            }
        }
    }
}
