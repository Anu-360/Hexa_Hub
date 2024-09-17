namespace Hexa_Hub.Interface
{
   
        public interface IEmail
        {
            Task SendEmailAsync(string fromEmail, string fromName, string toEmail, string subject, string message);
        }

    

}
