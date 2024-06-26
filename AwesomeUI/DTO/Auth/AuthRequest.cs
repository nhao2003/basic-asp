﻿using System.Text.Json.Serialization;

namespace AwesomeUI.DTO.Auth;

public class AuthRequest
{
    public string Username { get; set; }
    
    public string Password { get; set; }
    
    public AuthRequest(string username, string password)
    {
        Username = username;
        Password = password;
    }
    
}