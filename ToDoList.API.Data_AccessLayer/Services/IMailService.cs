namespace ToDoListInfo.API.Data_AccessLayer.Services
{
    public interface IMailService
    {
        void Send(string subject, string message);
    }
}