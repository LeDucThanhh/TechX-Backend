using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TechX.API.Helpers;

namespace TechX.API.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, JwtHelper jwtHelper)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var principal = jwtHelper.ValidateToken(token);
                    if (principal != null)
                    {
                        AttachUserToContext(context, principal);
                    }
                }
                catch (Exception)
                {
                    // Token is invalid, but we don't throw here
                    // Let the authorization middleware handle it
                }
            }

            await _next(context);
        }

        private void AttachUserToContext(HttpContext context, ClaimsPrincipal principal)
        {
            var userId = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var userEmail = principal.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var userName = principal.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
            var userRole = principal.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                // Attach user info to context for use in controllers
                context.Items["UserId"] = userId;
                context.Items["UserEmail"] = userEmail;
                context.Items["UserName"] = userName;
                context.Items["UserRole"] = userRole;
            }
        }
    }
} 