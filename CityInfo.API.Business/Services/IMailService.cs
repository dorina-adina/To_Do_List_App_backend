namespace CityInfo.API.Businsess_Layer.Services
{
    public interface IMailService
    {
        void Send(string subject, string message);
    }
}