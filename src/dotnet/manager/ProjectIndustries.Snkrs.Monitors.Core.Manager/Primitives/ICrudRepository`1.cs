﻿namespace ProjectIndustries.Snkrs.Monitors.Core.Manager.Primitives
{
  public interface ICrudRepository<T>
    : ICrudRepository<T, long>
    where T : class, IEntity<long>
  {
  }
}