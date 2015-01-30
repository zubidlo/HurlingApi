using System;
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
    /// <summary>
    /// 
    /// </summary>
    public class NotFoundActionResult : IHttpActionResult
    {
        private readonly string _message;
        private readonly HttpRequestMessage _request;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="message"></param>
        public NotFoundActionResult(HttpRequestMessage request, string message)
        {
            _message = message;
            _request = request;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = _request.CreateResponse(HttpStatusCode.NotFound);
            response.Content = new StringContent(_message);
            return Task.FromResult(response);
        }

    }
}