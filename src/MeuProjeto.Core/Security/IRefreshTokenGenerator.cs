﻿namespace MeuProjeto.Core.Security
{
    public interface IRefreshTokenGenerator
    {
        string GenerateToken();
        DateTime GetExpirationDate();
        bool Verify(DateTime? refreshTokenExpiryTime);
    }
}
