using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OrderSys.Models;

namespace OrderSys.Controllers
{
    public class Pre_OrdersController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            using(OrderSystemEntities db = new OrderSystemEntities())
            {
                var pre_orders = db.Pre_Orders.Select(po => new Pre_OrderView
                {
                    ProductID = po.ProductID,
                    Mail = po.Mail
                }).ToList();
                if (pre_orders.Any())
                    return Ok(pre_orders);
                else
                    return NotFound();
            }
        }

        [HttpGet]
        public IHttpActionResult Get(int id)    //可列出追蹤該產品的Email 以利系統一次發出通知信
        {
            using (OrderSystemEntities db = new OrderSystemEntities())
            {
                var pre_orders = db.Pre_Orders.Where(po => po.ProductID == id).Select(po => new Pre_OrderView
                {
                    ProductID = po.ProductID,
                    Mail = po.Mail
                }).ToList();
                if (pre_orders.Any())
                    return Ok(pre_orders);
                else
                    return NotFound();
            }
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody]Pre_OrderView value)
        {
            using (OrderSystemEntities db = new OrderSystemEntities())
            {
                var pre_order = new Pre_Orders
                {
                    ProductID = value.ProductID,
                    Mail = value.Mail
                };
                db.Pre_Orders.Add(pre_order);
                db.SaveChanges();
                return Ok();
            }
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id) //補貨通知完後可刪除全部追蹤該產品的Email
        {
            using (OrderSystemEntities db = new OrderSystemEntities())
            {
                var pre_orders = db.Pre_Orders.Where(po => po.ProductID == id);
                foreach(Pre_Orders pre_order in pre_orders)
                {
                    db.Pre_Orders.Remove(pre_order);
                }
                db.SaveChanges();
                return Ok();

            }
        }
    }
}
