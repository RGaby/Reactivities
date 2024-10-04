using System.Security.Claims;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Security
{
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor m_HttpContextAccessor;

        public UserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            this.m_HttpContextAccessor = httpContextAccessor;

        }

        public string GetUsername()
        {
            return m_HttpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        }
    }
}