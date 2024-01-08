using Microsoft.EntityFrameworkCore;

namespace ProjectIndustries.Snkrs.Monitors.Web.Manager.Api.Infra
{
  public class MonitorsContext : DbContext
  {
    public MonitorsContext(DbContextOptions<MonitorsContext> options)
      : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
      modelBuilder.UseSnakeCaseNamingConvention();
      base.OnModelCreating(modelBuilder);
    }
  }
}