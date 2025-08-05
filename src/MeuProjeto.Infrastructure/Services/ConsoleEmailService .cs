using MeuProjeto.Core.Services;

namespace MeuProjeto.Infrastructure.Services
{
    public class ConsoleEmailService : IEmailService
    {
        public Task SendEmailAsync(string to, string subject, string body)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("----- E-MAIL SIMULADO -----");
            Console.WriteLine($"Para: {to}");
            Console.WriteLine($"Assunto: {subject}");
            Console.WriteLine("Corpo:");
            Console.WriteLine(body);
            Console.WriteLine("---------------------------");
            Console.ResetColor();

            return Task.CompletedTask;
        }
    }
}
