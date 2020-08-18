
using HTTAPI.Helpers;
using System.Threading.Tasks;

namespace HTTAPI.Manager.Contract
{
    /// <summary>
    /// View Render Service interface
    /// </summary>
    public interface IViewRenderService
    {
        /// <summary>
        /// Render view to string
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<IResult> RenderToStringAsync(string viewName, object model);
    }
}

