﻿namespace MeuProjeto.Core.Events.Messages
{
    public class SendWelcomeEmailMessage
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
