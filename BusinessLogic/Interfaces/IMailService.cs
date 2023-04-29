﻿namespace BusinessLogic.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}