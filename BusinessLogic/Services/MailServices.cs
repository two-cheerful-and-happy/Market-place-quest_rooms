﻿using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;


namespace BusinessLogic.Services
{
    public class MailService : IMailService
    {
        private readonly ILogger<MailService> _loger;
        private readonly string _from = "pizzalog711@gmail.com";
        private readonly string _token = "meihapujqkiajgnq";

        public MailService(ILogger<MailService> logger)
        {
            _loger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Admin of website", _from));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                await client.AuthenticateAsync(_from, _token);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }


    }
}