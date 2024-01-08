using Microsoft.EntityFrameworkCore;
using ProjectIndustries.Snkrs.Monitors.Core.Manager.Primitives;

namespace ProjectIndustries.Snkrs.Monitors.Web.Manager.Api.Infra.Repositories
{
  public abstract class EfCrudRepository<T>
    : EfCrudRepository<T, long>
    where T : class, IEntity<long>
  {
    protected EfCrudRepository(DbContext context, IUnitOfWork unitOfWork)
      : base(context, unitOfWork)
    {
    }
  }
}