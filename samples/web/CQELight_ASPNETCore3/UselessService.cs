namespace CQELight_ASPNETCore3
{
    public interface IUselessService
    {
        string GetData();
    }
    public class UselessService : IUselessService
    {
        public string GetData()
            => "Welcome";
    }
}
