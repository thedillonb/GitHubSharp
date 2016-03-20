using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace GitHubSharp
{
    public class ForbiddenException : StatusCodeException
    {
		public ForbiddenException(string message, HttpResponseHeaders headers = null)
        : base(HttpStatusCode.Forbidden, message, headers) { }
    }

    public class NotFoundException : StatusCodeException
    {
		public NotFoundException(string message, HttpResponseHeaders headers = null)
        : base(HttpStatusCode.NotFound, message, headers) { }
    }

    public class NotModifiedException : StatusCodeException
    {
		public NotModifiedException(string message, HttpResponseHeaders headers = null)
        : base(HttpStatusCode.NotModified, message, headers) { }
    }

    public class UnauthorizedException : StatusCodeException
    {
		public UnauthorizedException(string message, HttpResponseHeaders headers = null)
        : base(HttpStatusCode.Unauthorized, message, headers) { }
    }

    public class InternalServerException : StatusCodeException
    {
		public InternalServerException(string message, HttpResponseHeaders headers = null)
        : base(HttpStatusCode.InternalServerError, message, headers) { }
    }

    public class StatusCodeException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }

		public HttpResponseHeaders Headers { get; private set; }

		public StatusCodeException(HttpStatusCode statusCode, HttpResponseHeaders headers)
            : this(statusCode, statusCode.ToString(), headers)
        {
        }

		public StatusCodeException(HttpStatusCode statusCode, string message, HttpResponseHeaders headers)
            : base(message)
        {
            StatusCode = statusCode;
            Headers = headers;
        }

		internal static StatusCodeException FactoryCreate(HttpResponseMessage response, string data)
        {
			var headers = response.Headers;

            string errorStr = null;
            try
            {
				errorStr = Client.Serializer.Deserialize<GitHubSharp.Models.ErrorModel>(data).Message;
            }
            catch 
            {
                //Do nothing
            }

            switch (response.StatusCode)
            {
                case HttpStatusCode.Forbidden:
                    return new ForbiddenException("You do not have the permissions to access or modify this resource.", headers);
                case HttpStatusCode.NotFound:
                    return new NotFoundException("The server is unable to locate the requested resource.", headers);
                case HttpStatusCode.InternalServerError:
                    return new InternalServerException("The request was unable to be processed due to an interal server error.", headers);
                case HttpStatusCode.Unauthorized:
                    return new UnauthorizedException("You are unauthorized to view the requested resource.", headers);
                case HttpStatusCode.NotModified:
                    return new NotModifiedException("This resource has not been modified since the last request.", headers);
                default:
                    return new StatusCodeException(response.StatusCode, errorStr ?? response.StatusCode.ToString(), headers);
            }
        }
    }
}

