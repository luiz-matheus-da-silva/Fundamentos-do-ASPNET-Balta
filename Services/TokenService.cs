
using Blog.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Blog.Services
{
    public class TokenService
    {
        // Método responsável por gerar um token JWT a partir de um usuário
        public string GenerateToken(User user)
        {
            // Cria um manipulador de tokens JWT
            var tokenHandler = new JwtSecurityTokenHandler();

            // Converte a chave secreta definida na configuração para um array de bytes
            var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);

            // Cria uma descrição do token.
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new (ClaimTypes.Name, "Luiz"),
                    new (ClaimTypes.Role, "admin"),
                }),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            // Gera um token baseado na descrição
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Retorna o token gerado como uma string
            return tokenHandler.WriteToken(token);
        }
    }
}