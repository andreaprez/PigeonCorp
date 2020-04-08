namespace PigeonCorp.Persistence.Gateway
{
    public interface IUserDataGateway
    {
        T Get<T>() where T : class, new();
        void Update<T>(T data) where T: class;
    }
}