namespace Durga.Api.Application.Ports;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
    Task SendEmailAsync(IEnumerable<string> to, string subject, string body);
    Task SendEmailConfirmationAsync(string email, string confirmationLink);
    Task SendPasswordResetAsync(string email, string resetLink);
    Task SendWelcomeEmailAsync(string email, string userName);
    Task SendTeamInvitationAsync(string email, string teamName, string inviteLink);
}