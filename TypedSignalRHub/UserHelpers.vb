Imports System.IdentityModel.Tokens.Jwt
Imports System.Text
Imports Microsoft.IdentityModel.Tokens

Module UserHelpers
    Public Function ValidateJWT(ByVal JWT As String) As String
        Dim securityKey = New SymmetricSecurityKey(Encoding.UTF8.GetBytes(Startup._Configuration("Jwt:Key")))
        Dim JwtChecker = New JwtSecurityTokenHandler()
        Dim validatedToken As SecurityToken = Nothing
        JwtChecker.ValidateToken(JWT, New TokenValidationParameters With {
            .ValidateIssuerSigningKey = True,
            .ValidateIssuer = True,
            .ValidateAudience = True,
            .ValidIssuer = Startup._Configuration("Jwt:Issuer"),
            .ValidAudience = Startup._Configuration("Jwt:Issuer"),
            .IssuerSigningKey = securityKey
        }, validatedToken)
        Return validatedToken.Id
    End Function
End Module
