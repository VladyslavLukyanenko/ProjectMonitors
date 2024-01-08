using Microsoft.EntityFrameworkCore;
using ProjectIndustries.Snkrs.Monitors.Core.Manager;
using ProjectIndustries.Snkrs.Monitors.Core.Manager.Primitives;
using ProjectIndustries.Snkrs.Monitors.Core.Manager.Services;

namespace ProjectIndustries.Snkrs.Monitors.Web.Manager.Api.Infra.Repositories
{
  public class EfImageInfoRepository : EfCrudRepository<ImageInfo, string>, IImageInfoRepository
  {
    public EfImageInfoRepository(DbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
    {
    }
  }
}