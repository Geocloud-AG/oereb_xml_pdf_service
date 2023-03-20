using System;
using System.Net;

namespace Oereb.Service.DataContracts
{
    public class ExtException : Exception
    {
        private int _errorCode { get; set; }
        private object _caller { get; set; }
        private HttpStatusCode _httpStatusCode { get; set; }

        public string ErrorCode => _caller == null ? _errorCode.ToString() : $"{_caller.GetType().Name}.{_errorCode}";
        public HttpStatusCode HttpStatusCode => _httpStatusCode;

        public ExtException(string message, Exception exception, int errorCode, HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError) : base(message, exception)
        {
            _errorCode = errorCode;
            _httpStatusCode = httpStatusCode;
        }

        public ExtException(string message, int errorCode, HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError) : base(message)
        {
            _errorCode = errorCode;
            _httpStatusCode = httpStatusCode;
        }

        public ExtException( string message, object caller, int errorCode, HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError) : base(message)
        {
            _caller = caller;
            _errorCode = errorCode;
            _httpStatusCode = httpStatusCode;
        }
    }
}
