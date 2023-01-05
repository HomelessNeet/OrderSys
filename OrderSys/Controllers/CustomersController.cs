using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OrderSys.Models;

namespace OrderSys.Controllers
{
    public class CustomersController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            using(OrderSystemEntities db = new OrderSystemEntities())
            {
                var customers = db.Customers.Select(c => new CustomerView
                {
                    CustomerID = c.CustomerID,
                    CompanyName = c.CompanyName,
                    ContactName = c.ContactName,
                    Address = c.Address,
                    Phone = c.Phone
                }).ToList();
                if (customers.Any())
                    return Ok(customers);
                else
                    return NotFound();
            }
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            using (OrderSystemEntities db = new OrderSystemEntities())
            {
                var customers = db.Customers.Where(c => c.CustomerID == id).Select(c => new CustomerView
                {
                    CustomerID = c.CustomerID,
                    CompanyName = c.CompanyName,
                    ContactName = c.ContactName,
                    Address = c.Address,
                    Phone = c.Phone
                }).SingleOrDefault();
                if (customers == null)
                    return NotFound();
                else
                    return Ok(customers);
            }
        }

       
        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody]CustomerView value)
        {
            using (OrderSystemEntities db = new OrderSystemEntities())
            {
                var customer = db.Customers.SingleOrDefault(c => c.CustomerID == id);
                if (customer == null)
                    return NotFound();
                else
                {
                    customer.CompanyName = value.CompanyName;
                    customer.ContactName = value.ContactName;
                    customer.Address = value.Address;
                    customer.Phone = value.Phone;
                    db.SaveChanges();
                    return Ok();
                }
            }
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            using (OrderSystemEntities db = new OrderSystemEntities())
            {
                var customer = db.Customers.SingleOrDefault(c => c.CustomerID == id);
                if (customer == null)
                    return NotFound();
                else
                {
                    db.Customers.Remove(customer);
                    db.SaveChanges();
                    return Ok();
                }
            }
        }
    }
}
