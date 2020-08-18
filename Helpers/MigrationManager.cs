using HTTAPI.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace HTTAPI.Helpers
{
    /// <summary>
    /// web host extension to run migration and seed data 
    /// </summary>
    public static class WebHostExtension
    {
        /// <summary>
        /// run migrations
        /// </summary>
        /// <param name="webHost"></param>
        /// <returns></returns>
        public static IWebHost MigrateDatabase(this IWebHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<Context>())
                {
                    //   appContext.Database.Migrate();
                }
            }
            return webHost;
        }
    }
}
