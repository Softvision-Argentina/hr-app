using Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;

namespace ApiServer.Controllers
{
    [ApiController]
    public abstract class BaseController<TController> : Controller where TController : Controller
    {
        protected ILog<TController> Logger { get; set; }

        //TODO: is not call from anywhere can we remove it?
        public Guid ClientSystemId { get; private set; }

        protected BaseController(ILog<TController> logger)
        {
            Logger = logger;
        }

        protected IActionResult ApiAction(Func<IActionResult> action)
        {

            if (Logger.IsDebugEnabled)
            {
                if (HttpContext.Request.Method == "GET")
                {
                    if (HttpContext.Request.QueryString.HasValue)
                        Logger.LogDebug($"GET ==== Request: {HttpContext.Request.QueryString.Value}");
                }
                else
                {
                    Logger.LogDebug($"{HttpContext.Request.Method} ==== Body: @{GetRequestBody()}");
                }
            }
            try
            {
                return action();
            }
            catch (BusinessValidationException bvex)
            {
                Logger.LogError(bvex, "Validation Error");
                foreach (var item in bvex.ValidationMessages)
                {
                    ModelState.AddModelError(item.Key, item.Value);
                }
                return BadRequest(ModelState);
            }
            catch (BusinessException ex)
            {
                Logger.LogError(ex, "Business Exception");
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = ex.Message,
                    errorCode = ex.ErrorCode,
                    additionalData = ex.Data
                });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Not expected Exception");
                throw ex;
            }
        }

        private string GetRequestBody()
        {
            var jsonData = string.Empty;
            try
            {
                using (var stream = Request.Body)
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
    }
}
