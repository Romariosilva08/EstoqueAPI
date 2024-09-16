using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task EnviarEmailAsync(string destinatario, string assunto, string mensagem)
    {
        var smtpConfig = _configuration.GetSection("Smtp");

        using (var smtpClient = new SmtpClient(smtpConfig["Host"], int.Parse(smtpConfig["Port"])))
        {
            smtpClient.EnableSsl = bool.Parse(smtpConfig["EnableSsl"]);
            smtpClient.Credentials = new NetworkCredential(smtpConfig["Username"], smtpConfig["Password"]);

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpConfig["From"]),
                Subject = assunto,
                Body = mensagem,
                IsBodyHtml = true
            };

            mailMessage.To.Add(destinatario);

            // Adicionar cabeçalhos personalizados
            mailMessage.Headers.Add("X-Priority", "1"); // Alta prioridade
            mailMessage.Headers.Add("X-MSMail-Priority", "High");

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
                Console.WriteLine("E-mail enviado com sucesso.");
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
                throw new InvalidOperationException("Erro ao enviar e-mail.", ex);
            }
        }
    }

}
