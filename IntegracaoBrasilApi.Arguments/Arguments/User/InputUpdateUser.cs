﻿namespace IntegracaoBrasilApi.Arguments;

public class InputUpdateUser(string username, string password)
{
    public string Username { get; private set; } = username;
    public string Password { get; private set; } = password;
}