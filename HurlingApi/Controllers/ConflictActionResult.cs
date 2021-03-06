﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;

namespace HurlingApi.Controllers
{
    public class ConflictActionResult : IHttpActionResult
    {
        private readonly string _message;
        private readonly HttpRequestMessage _request;

        public ConflictActionResult(HttpRequestMessage request, string message)
        {
            _message = message;
            _request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = _request.CreateResponse(HttpStatusCode.Conflict);
            response.Content = new StringContent(_message);
            return Task.FromResult(response);
        }
    }
}