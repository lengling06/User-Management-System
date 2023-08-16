using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UM.IService;
using UM.Utility;
using UM.Utility.ApiResult;

namespace UM.JWT
{
    public class JwtService : IJwtService
    {
        /*
        private readonly IUserService _iUserService;
        public JwtService(IUserService iUserService)
        {
            _iUserService = iUserService;
        }

        public ActionResult<ApiResult> GenerateToken(string username, string password)
        {
            // 验证用户是否存在
            var user = _iUserService.SelectOne(username, password);
            if (null != user)
            {
                // 用户存在，则授权
                var claims = new Claim[]
                {
                    // 不要放敏感信息 比如密码
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.UserRoles.ToString()),
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SDMC-CJAS1-SAD-DFSFA-SADHJVF-VF"));
                // issuer代表颁发Token的Web应用程序，audience是Token的受理者
                var token = new JwtSecurityToken(
                    issuer: "http://localhost:6060",
                    audience: "http://localhost:5000",
                    claims: claims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );
                var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                return ApiResultHelper.Success(jwtToken);
            }
            return ApiResultHelper.Error(ApiStatusCode.BadRequest, "用户名或密码错误！");
        }
        */

        public string GenerateTokenString(object data)
        {
            //// 验证用户是否存在
            //var user = _iUserService.SelectOne(username, password);
            if (null != data)
            {
                // 用户存在，则授权
                var claims = new Claim[]
                {
                    // 不要放敏感信息 比如密码
                    //new Claim(ClaimTypes.Name, user.Username),
                    //new Claim("Id", user.Id.ToString()),

                    // Subject (sub): JWT 的主题，通常用于存放用户的标识信息。
                    // 将用户的数据对象通过 JSON 序列化转换成字符串后作为主题内容存放在 JWT 中。
                    new Claim(JwtRegisteredClaimNames.Sub, JsonConvert.SerializeObject(data)),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                // "SDMC-CJAS1-SAD-DFSFA-SADHJVF-VF"
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("c2FuZ3hpYW95dXlhc2FuZ3hpYW95dXlh"));
                // issuer代表颁发Token的Web应用程序，audience是Token的受理者
                var token = new JwtSecurityToken(
                    issuer: "http://localhost:6060",
                    audience: "http://localhost:5000",
                    claims: claims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );
                var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                return jwtToken;
            }
            return "用户名或密码错误！";
        }

        public object ParseToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            // 配置密钥，和生成 JWT 时使用的密钥一致
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("c2FuZ3hpYW95dXlhc2FuZ3hpYW95dXlh"));
            // 配置 Token 验证参数
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = "http://localhost:6060",
                ValidateAudience = true,
                ValidAudience = "http://localhost:5000",
                ValidateLifetime = true,
                // 不允许时钟偏移
                // 设置为 TimeSpan.Zero 可以确保 Token 必须在过期时间之前解析
                ClockSkew = TimeSpan.Zero
            };

            // 解析 JWT 令牌
            SecurityToken securityToken;
            var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

            // 获取 Identities 中的第一个 ClaimsIdentity，并从中查找相关的声明
            var claimsIdentity = claimsPrincipal.Identities.First();

            // 获取 Subject 中的数据（用户信息）
            // 获取第一个声明（用户信息）
            var firstClaim = claimsIdentity.Claims.FirstOrDefault();

            if (firstClaim != null && !string.IsNullOrEmpty(firstClaim.Value))
            {
                // 将 Subject Claim 中设置的 JSON 数据反序列化为对象
                object userData = JsonConvert.DeserializeObject(firstClaim.Value);
                return userData;
            }

            return null; // 解析失败或没有 Subject Claim
        }
    }
}
