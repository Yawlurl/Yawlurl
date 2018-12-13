namespace PonyUrl.Core.Entities
{
    public interface IEntity<TId>
    {
        TId Id { get; set; }
    }
}
