namespace CQELight_ASPNETCore2_1
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
