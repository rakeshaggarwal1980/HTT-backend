using HTTAPI.Enums;
using HTTAPI.Helpers;
using HTTAPI.Manager.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;


namespace HTT.Manager.Service
{
    /// <summary>
    /// Render view service
    /// </summary>
    public class ViewRenderService : IViewRenderService
    {
        /// <summary>
        /// logger ContentDetailService
        /// </summary>
        readonly ILogger<ViewRenderService> _logger;
        /// <summary>
        /// http context accessor
        /// </summary>
        readonly IHttpContextAccessor _accessor;
        /// <summary>
        /// razor engine
        /// </summary>
        readonly IRazorViewEngine _razorViewEngine;
        /// <summary>
        /// temp data initializer
        /// </summary>
        readonly ITempDataProvider _tempDataProvider;


        /// <summary>
        /// CTOR
        /// </summary> 
        /// <param name="logger"></param>
        /// <param name="accessor"></param>
        /// <param name="razorViewEngine"></param>
        /// <param name="tempDataProvider"></param>

        public ViewRenderService(ILogger<ViewRenderService> logger, IHttpContextAccessor accessor, IRazorViewEngine razorViewEngine, ITempDataProvider tempDataProvider)
        {
            _logger = logger;
            _accessor = accessor;
            _razorViewEngine = razorViewEngine;
            _tempDataProvider = tempDataProvider;
        }

        /// <summary>
        /// Render view to string
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IResult> RenderToStringAsync(string viewName, object model)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success,
                StatusCode = HttpStatusCode.OK
            };

            try
            {
                var httpContext = _accessor.HttpContext; // new DefaultHttpContext { RequestServices = this.serviceProvider };
                var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

                using (var sw = new StringWriter())
                {
                    var viewResult = _razorViewEngine.GetView(null, viewName, false);

                    if (viewResult.View == null)
                    {
                        var msg = $"{viewName} does not match any available view";

                        _logger.LogError(msg);
                        result.Status = Status.Error;
                        result.Message = msg;
                        result.StatusCode = HttpStatusCode.BadRequest;
                        return result;
                    }

                    var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                    { Model = model };

                    var viewContext = new ViewContext(actionContext, viewResult.View, viewDictionary,
                        new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                        sw,
                        new HtmlHelperOptions());

                    await viewResult.View.RenderAsync(viewContext);

                    result.Body = sw.ToString();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                result.Status = Status.Error;
                result.Message = ex.Message;
                result.StatusCode = HttpStatusCode.InternalServerError;
            }
            return result;
        }
    }
}