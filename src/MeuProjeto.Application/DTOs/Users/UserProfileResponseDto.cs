﻿namespace MeuProjeto.Application.DTOs.Users
{
    public class UserProfileResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
