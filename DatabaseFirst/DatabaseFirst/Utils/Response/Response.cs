﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseFirst.Utils.Response
{

    public enum ResponseMessage
    {
        /// <summary>
        /// Response message type for success
        /// </summary>
        Success,
        /// <summary>
        /// Response message type for exception
        /// </summary>
        Exception,
        /// <summary>
        /// Response message type for miscellaneous error
        /// </summary>
        MiscError
    }

    public enum ResponseState
    {
        /// <summary>
        /// Success response state
        /// </summary>
        Success,
        /// <summary>
        /// Error response state
        /// </summary>
        Error,
        /// <summary>
        /// Exception response state
        /// </summary>
        Exception
    }

    public class Response
    {
        /// <summary>
        /// Specifies the state of the response
        /// </summary>
        public ResponseState State { get; set; }
        /// <summary>
        /// Specifies whether the response state is success or not
        /// </summary>
        public bool WasSuccess
        {
            get { return (State == ResponseState.Success); }
        }
        /// <summary>
        /// Specifies the outcome message
        /// </summary>
        public ResponseMessage Message { get; set; }
        /// <summary>
        /// Returns the message text of a <see cref="ResponseMessage"/>.
        /// </summary>
        public string MessageText
        {
            get
            {
                if (Message == ResponseMessage.MiscError)
                    return ErrorText;
                return Message.ToString();
            }
        }
        /// <summary>
        /// When the response is a failure without an exception or prefined error, this is the error message
        /// </summary>
        public string ErrorText { get; set; }
        public Exception Exception { get; set; }
        /// <summary>
        /// Creates a success response.
        /// </summary>
        public static Response Success()
        {
            return new Response
            {
                State = ResponseState.Success,
                Message = ResponseMessage.Success,
                ErrorText = null,
                Exception = null
            };
        }
        /// <summary>
        /// Creates a validation error response.
        /// </summary>
        /// <param name="message">Error message type</param>
        public static Response Error(ResponseMessage message)
        {
            return new Response
            {
                State = ResponseState.Error,
                Message = message,
                ErrorText = null,
                Exception = null
            };
        }
        /// <summary>
        /// Creates a validation error response.
        /// </summary>
        /// <param name="errorText">Error message explaining the failure</param>
        public static Response Error(string errorText)
        {
            return new Response
            {
                State = ResponseState.Error,
                Message = ResponseMessage.MiscError,
                ErrorText = errorText,
                Exception = null
            };
        }
        /// <summary>
        /// Creates an exception response.
        /// </summary>
        /// <param name="exception">Exception that caused the failure</param>
        public static Response Error(Exception exception)
        {
            return new Response
            {
                State = ResponseState.Exception,
                Message = ResponseMessage.Exception,
                ErrorText = null,
                Exception = exception
            };
        }
    }


    /// <summary>
    /// Response class with ResponseObject
    /// </summary>
    public class Response<T> : Response
    {
        /// <summary>
        /// The object that contains response values.
        /// </summary>
        public T ResponseObject { get; set; }
        /// <summary>
        /// Creates a success response of the specified type.
        /// </summary>
        /// <param name="responseObject">Object to return in the response</param>
        /// <returns></returns>
        public static Response<T> Success(T responseObject)
        {
            return new Response<T>
            {
                State = ResponseState.Success,
                Message = ResponseMessage.Success,
                Exception = null,
                ErrorText = null,
                ResponseObject = responseObject
            };
        }
        /// <summary>
        /// Creates a validation error response of the specified type.
        /// </summary>
        /// <param name="message">Error message type</param>
        /// <returns></returns>
        public new static Response<T> Error(ResponseMessage message)
        {
            return new Response<T>
            {
                State = ResponseState.Error,
                Message = message,
            };
        }
        /// <summary>
        /// Creates a validation error response of the specified type.
        /// </summary>
        /// <param name="errortText">Error message explaining the failure</param>
        /// <returns></returns>
        public new static Response<T> Error(string errortText)
        {
            return new Response<T>
            {
                State = ResponseState.Error,
                Message = ResponseMessage.MiscError,
                ErrorText = errortText,
            };
        }
        /// <summary>
        /// Creates a exception error response of the specified type.
        /// </summary>
        /// <param name="exception">Exception that caused the failure</param>
        /// <returns></returns>
        public new static Response<T> Error(Exception exception)
        {
            return new Response<T>
            {
                State = ResponseState.Exception,
                Message = ResponseMessage.Exception,
                Exception = exception,
            };
        }
    }

}
