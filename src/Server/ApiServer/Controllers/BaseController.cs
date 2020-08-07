// <copyright file="BaseController.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using Core;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    public abstract class BaseController<TController> : Controller where TController : Controller
    {
        protected ILog<TController> Logger { get; set; }

        // TODO: is not call from anywhere can we remove it?
        public Guid ClientSystemId { get; private set; }

        protected BaseController(ILog<TController> logger)
        {
            this.Logger = logger;
        }

        protected IActionResult ApiAction(Func<IActionResult> action)
        {
            if (this.Logger.IsDebugEnabled)
            {
                if (this.HttpContext.Request.Method == "GET")
                {
                    if (this.HttpContext.Request.QueryString.HasValue)
                    {
                        this.Logger.LogDebug($"GET ==== Request: {this.HttpContext.Request.QueryString.Value}");
                    }
                }
                else
                {
                    this.Logger.LogDebug($"{this.HttpContext.Request.Method} ==== Body: @{this.GetRequestBody()}");
                }
            }

            try
            {
                return action();
            }
            catch (BusinessValidationException bvex)
            {
                this.Logger.LogError(bvex, "Validation Error");

                var exceptionData = new ExceptionData()
                {
                    HttpStatusCode = (int)HttpStatusCode.BadRequest,
                    ErrorCode = bvex.ErrorCode,
                    ValidationErrors = this.GetValidationErrors(bvex.ValidationMessages),
                    ExceptionMessage = bvex.Message,
                    InnerExceptionMessage = bvex.InnerException != null ? bvex.InnerException.Message : string.Empty,
                    AdditionalInfo = bvex.Data,
                };

                return this.BadRequest(exceptionData);
            }
            catch (BusinessException ex)
            {
                this.Logger.LogError(ex, "Business Exception");

                var exceptionData = new ExceptionData()
                {
                    HttpStatusCode = (int)HttpStatusCode.BadRequest,
                    ErrorCode = ex.ErrorCode,
                    ValidationErrors = null,
                    ExceptionMessage = ex.Message ?? "Business Exception",
                    InnerExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : string.Empty,
                    AdditionalInfo = ex.Data,
                };

                return this.BadRequest(exceptionData);
            }
            catch (Exception ex)
            {
                var exceptionData = new ExceptionData()
                {
                    HttpStatusCode = (int)HttpStatusCode.InternalServerError,
                    ErrorCode = (int)ApplicationErrorMainCodes.NotExpected,
                    ValidationErrors = null,
                    ExceptionMessage = ex.Message ?? "Not expected exception message",
                    InnerExceptionMessage = this.GetInnerException(ex),
                    AdditionalInfo = ex.Data,
                };

                return this.StatusCode((int)HttpStatusCode.InternalServerError, exceptionData);
            }
        }

        private string GetInnerException(Exception exception)
        {
            string defaultMessage = "Not expected inner exception message";
            return exception?.InnerException?.InnerException?.Message ?? defaultMessage;
        }

        private string GetRequestBody()
        {
            var jsonData = string.Empty;
            try
            {
                using (var stream = this.Request.Body)
                {
                    if (stream.CanRead)
                    {
                        stream.Position = 0;
                        using (var reader = new StreamReader(stream))
                        {
                            jsonData = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch
            {
            }

            return jsonData;
        }

        private List<ValidationError> GetValidationErrors(IReadOnlyList<KeyValuePair<string, string>> keyValueErrors)
        {
            var validationsErrors = new List<ValidationError>();

            keyValueErrors.ToList().ForEach((error) =>
            {
                validationsErrors.Add(new ValidationError() { Name = error.Key, Description = error.Value });
            });

            return validationsErrors;
        }
    }
}
