using BARSTOC_DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BARSTOC_BLL.Servicios
{
    public class JwtService
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expireMinutes;

        public JwtService(IConfiguration configuration)
        {
            _secretKey = configuration["Jwt:Key"] ?? "Barstoc_Secret_Key_Minimo_64_Caracteres_Para_Seguridad_2024_";
            _issuer = configuration["Jwt:Issuer"] ?? "BARSTOC_API";
            _audience = configuration["Jwt:Audience"] ?? "BARSTOC_Client";

            // ✅ CORREGIDO: Manejar conversión segura
            if (int.TryParse(configuration["Jwt:ExpireMinutes"], out int expireMinutes))
            {
                _expireMinutes = expireMinutes;
            }
            else
            {
                _expireMinutes = 3; // Valor por defecto
            }
        }

        public string GenerateToken(DTO_Sesion usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name, $"{usuario.NombreUsuario} {usuario.ApellidoUsuario}"),
                new Claim(ClaimTypes.Email, usuario.correo),
                new Claim(ClaimTypes.Role, usuario.DescripcionRol ?? "Usuario"),
                new Claim("SedeId", usuario.IdSede.ToString()), // ✅ AHORA IdSede EXISTE
                new Claim("UsuarioLogin", usuario.UsuarioLogin),
                new Claim("NombreSede", usuario.NombreSede ?? "Sin sede")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_expireMinutes),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_secretKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                return tokenHandler.ValidateToken(token, validationParameters, out _);
            }
            catch
            {
                return null;
            }
        }
    }
}