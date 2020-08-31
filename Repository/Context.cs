using HTTAPI.Repository.SeedData;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace HTTAPI.Repository
{
    /// <summary>
    /// HTT context
    /// </summary>
    public partial class Context : DbContext
    {
        /// <summary>
        /// ctor default
        /// </summary>
        protected Context()
        {
            Log.Logger.Debug("At Context ctor");
        }

        /// <summary>
        /// ctor 
        /// </summary>
        /// <param name="options"></param>
        public Context(DbContextOptions<Context> options) : base(options)
        {
            Log.Logger.Debug("DbContextOptions at Context ctor");
        }

        /// <summary>
        /// On model creating event
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Log.Logger.Debug("OnModelCreating at Context");

            // configure master tables data...
            modelBuilder.ApplyConfiguration(new LocationConfiguration());
            modelBuilder.ApplyConfiguration(new ZoneConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionConfiguration());
            modelBuilder.ApplyConfiguration(new SymptomConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }

        /// <summary>
        /// Get set data on db creation
        /// </summary>
        public void SeedData()
        {
            Log.Logger.Debug("Seed at Context");
            ModelBuilderExtensions.Seed(this);
        }

    }

    internal static class ModelBuilderExtensions
    {
        /// <summary>
        /// Get set data on db creation
        /// </summary>
        /// <param name="context"></param>
        public static void Seed(Context context)
        {
            Log.Logger.Debug("Seed at ModelBuilderExtensions");
        }
    }
}
