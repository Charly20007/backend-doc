using Surgeon.Domain.Entities;

namespace Surgeon.Application.Interfaces;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verificar(string password, string hash);
}

public interface ITokenService
{
    (string Token, DateTime ExpiraEn) GenerarToken(Usuario usuario, string rol);
}
