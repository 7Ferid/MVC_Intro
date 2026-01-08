namespace MVC_Intro.Abstraction
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
