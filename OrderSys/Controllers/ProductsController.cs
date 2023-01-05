using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OrderSys.Models;

namespace OrderSys.Controllers
{
    public class ProductsController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
           using(OrderSystemEntities db = new OrderSystemEntities())
            {
                var products = db.Products.Select(p => new ProductView
                {
                    ProductID = p.ProductID,
                    ProductName = p.ProductName,
                    UnitPrice = p.UnitPrice,
                    Stock = p.Stock
                }).ToList();
                if (products.Any())
                    return Ok(products);
                else
                    return NotFound();
            }
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            using (OrderSystemEntities db = new OrderSystemEntities())
            {
                var product = db.Products.Where(p => p.ProductID == id).Select(p => new ProductView
                {
                    ProductID = p.ProductID,
                    ProductName = p.ProductName,
                    UnitPrice = p.UnitPrice,
                    Stock = p.Stock
                }).SingleOrDefault();
                if (product == null)
                    return NotFound();
                else
                    return Ok(product);
            }
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody]ProductView value)
        {
            using(OrderSystemEntities db = new OrderSystemEntities())
            {
                var product = db.Products.SingleOrDefault(p => p.ProductName == value.ProductName);
                if (product == null)
                {
                    product = new Products
                    {
                        ProductName = value.ProductName,
                        UnitPrice = value.UnitPrice,
                        Stock = value.Stock
                    };
                    db.Products.Add(product);
                    db.SaveChanges();
                    return Ok();
                }
                else
                    return Conflict();  //已有相同名稱的產品
            }
        }
        
        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody]ProductView value)   //補貨用API
        {
            using(OrderSystemEntities db = new OrderSystemEntities())
            {
                var product = db.Products.SingleOrDefault(p => p.ProductID == id);
                if (product == null)
                    return NotFound();
                else
                {
                    product.ProductName = value.ProductName;
                    product.UnitPrice = value.UnitPrice;
                    product.Stock += value.Stock;    //補貨數量
                    db.SaveChanges();
                    return Ok();
                }
            }
        }
        
    }
}
