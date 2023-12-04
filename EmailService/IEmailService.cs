using Dreamland_Suit_API.Models;

namespace Dreamland_Suit_API.EmailService
{
    public interface IEmailService
    {
        void SendEmail(EmailDto request);
    }
}
