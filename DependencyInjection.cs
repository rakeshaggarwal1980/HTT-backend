using HTTAPI.Manager.Contract;
using HTTAPI.Manager.Service;
using HTTAPI.Repository;
using HTTAPI.Repository.Contracts;
using HTTAPI.Repository.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Principal;

namespace HTTAPI
{
    public class DependencyInjection
    {
        internal void ConfigureRepositories(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Context>(options =>
               options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc();

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);

            #region Manager
            services.AddTransient<IHealthTrackService, HealthTrackService>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IRequestService, RequestService>();

            #endregion

            #region Repositories
            services.AddTransient<IHealthTrackRepository, HealthTrackRepository>();
            services.AddTransient<IEmployeeRepository, EmployeeRepository>();
            services.AddTransient<IRequestRepository, RequestRepository>();
            #endregion

        }
    }
}
