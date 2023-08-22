using Microsoft.EntityFrameworkCore;

namespace Api.Models.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Dependent> Dependents => Set<Dependent>();
        public DbSet<Employee> Employees => Set<Employee>();
    }
}
