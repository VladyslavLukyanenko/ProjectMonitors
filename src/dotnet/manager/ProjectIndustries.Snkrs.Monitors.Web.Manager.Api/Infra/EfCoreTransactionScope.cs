using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using ProjectIndustries.Snkrs.Monitors.Core.Manager.Primitives;

namespace ProjectIndustries.Snkrs.Monitors.Web.Manager.Api.Infra
{
  public class EfCoreTransactionScope
    : ITransactionScope
  {
    private readonly bool _autoCommit;
    private readonly IDbContextTransaction _tx;

    public EfCoreTransactionScope(IDbContextTransaction tx, bool autoCommit)
    {
      _tx = tx;
      _autoCommit = autoCommit;
    }

    public void Dispose()
    {
      if (_autoCommit)
      {
        _tx.Commit();
      }

      _tx.Dispose();
    }

    public Task RollbackAsync(CancellationToken ct = default)
    {
      return _tx.RollbackAsync(ct);
    }

    public Task CommitAsync(CancellationToken ct = default)
    {
      return _tx.CommitAsync(ct);
    }
  }
}