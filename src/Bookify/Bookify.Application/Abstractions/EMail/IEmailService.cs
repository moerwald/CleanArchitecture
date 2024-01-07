namespace Bookify.Application.Abstractions.EMail;

public interface IEmailService
{
    Task SendAsync(Domain.Users.Email recipient, string subject, string body);
}
