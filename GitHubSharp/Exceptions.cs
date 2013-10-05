using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;

namespace GitHubSharp
{
    public class ForbiddenException : StatusCodeException
    {
        public ForbiddenException(Dictionary<string, string> headers = null)
        : base(HttpStatusCode.Forbidden, "You do not have the permissions to access or modify this resource.", headers) { }
    }

    public class NotFoundException : StatusCodeException
    {
        public NotFoundException(Dictionary<string, string> headers = null)
        : base(HttpStatusCode.NotFound, "The server is unable to locate the requested resource.", headers) { }
    }

    public class NotModifiedException : StatusCodeException
    {
        public NotModifiedException(Dictionary<string, string> headers = null)
        : base(HttpStatusCode.NotModified, "This resource has not been modified since the last request.", headers) { }
    }

    public class UnauthorizedException : StatusCodeException
    {
        public UnauthorizedException(Dictionary<string, string> headers = null)
        : base(HttpStatusCode.Unauthorized, "You are unauthorized to view the requested resource.", headers) { }
    }

    public class InternalServerException : StatusCodeException
    {
        public InternalServerException(Dictionary<string, string> headers = null)
        : base(HttpStatusCode.InternalServerError, "The request was unable to be processed due to an interal server error.", headers) { }
    }

    public class StatusCodeException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }

        public Dictionary<string, string> Headers { get; private set; }

        public StatusCodeException(HttpStatusCode statusCode, Dictionary<string, string> headers)
            : this(statusCode, statusCode.ToString(), headers)
        {
        }

        public StatusCodeException(HttpStatusCode statusCode, string message, Dictionary<string, string> headers)
            : base(message)
        {
            StatusCode = statusCode;
            Headers = headers;
        }

        internal static StatusCodeException FactoryCreate(RestSharp.IRestResponse response)
        {
            var headers = new Dictionary<string, string>();
            foreach (var h in response.Headers)
                headers.Add(h.Name, h.Value.ToString());

            switch (response.StatusCode)
            {
                case HttpStatusCode.Forbidden:
                    return new ForbiddenException(headers);
                case HttpStatusCode.NotFound:
                    return new NotFoundException(headers);
                case HttpStatusCode.InternalServerError:
                    return new InternalServerException(headers);
                case HttpStatusCode.Unauthorized:
                    return new UnauthorizedException(headers);
                case HttpStatusCode.NotModified:
                    return new NotModifiedException(headers);
                default:
                    return new StatusCodeException(response.StatusCode, headers);
            }
        }
    }
}

