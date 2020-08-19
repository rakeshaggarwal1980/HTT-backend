using HTTAPI.Helpers;
using HTTAPI.ViewModels;
using System.Threading.Tasks;

namespace HTTAPI.Manager.Contract
{
    /// <summary>
    /// interface for ContentDetailService 
    /// </summary>
    public interface IHealthTrackService
    {

        /// <summary>
        ///  Create the CreateHealthTrack
        /// </summary>
        /// <param name="healthTrackViewModel"></param>
        /// <returns></returns>
        Task<IResult> CreateHealthTrack(HealthTrackViewModel healthTrackViewModel);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IResult> GetDeclarationFormData();
    }

}
