﻿namespace Domain.ViewModels.Account;

public class ChangePasswordViewModel
{
    public string Token { get; set; }
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
    public string NewPasswordConfirm { get; set; }
}
