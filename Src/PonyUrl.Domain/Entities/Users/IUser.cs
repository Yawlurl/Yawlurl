namespace PonyUrl.Domain.Entities
{
    public interface IUser
    {
        string UserId { get; }

        string UserName { get; }

        string DisplayName { get; }
    }
}
