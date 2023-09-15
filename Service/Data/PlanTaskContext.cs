using System.Data.Entity;

namespace Service.Data
{
   
    public class PlanTaskContext : DbContext
    {
        /// <summary>
        /// Connect application to database by connection string
        /// </summary>
        public PlanTaskContext()
            : base("Planning_system")
        { }
        /// <summary>
        /// Set of PlanTask entities
        /// </summary>
        public DbSet<PlanTask> Tasks { get; set; }
    }
}
