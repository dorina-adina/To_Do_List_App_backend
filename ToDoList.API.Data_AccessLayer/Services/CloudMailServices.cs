namespace ToDoList.API.Data_AccessLayer.Services
{
    public class CloudMailServices : IMailService
    {
        private string _mailTo = string.Empty;
        private string _mailFrom = string.Empty;

        public void Send(string subject, string message)
        {
            Console.WriteLine($"Mail from {_mailFrom} to {_mailTo}, with {nameof(LocalMailService)}.");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
        }
    }
}
