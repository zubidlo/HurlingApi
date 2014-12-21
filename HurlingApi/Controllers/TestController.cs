using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using HurlingApi.Models;

namespace HurlingApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TestController : ApiController
    {

        private HurlingModelContext db = new HurlingModelContext();

        public IQueryable<int> Get()
        {
            List<int> grades = new List<int> { 78, 92, 100, 37, 81 };
            return grades.AsQueryable();
        }

        public HttpResponseMessage Post()
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent("POST: Test message")
            };
        }

        public HttpResponseMessage Put()
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent("PUT: Test message")
            };
        }
    }
}
