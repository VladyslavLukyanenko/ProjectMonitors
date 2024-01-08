namespace ProjectIndustries.Snkrs.Monitors.Core.Manager.Primitives
{
  public interface IEntity<out TKey>
  {
    TKey Id { get; }
  }
}