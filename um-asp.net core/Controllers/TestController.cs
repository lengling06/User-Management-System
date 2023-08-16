using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UM.JWT;
using UM.Model;

namespace um_asp.net_core.Controllers
{
    // 测试 JWT授权与鉴权
    public class TestController : Controller
    {
        private readonly IJwtService _iJwtService;
        public TestController(IJwtService iJwtService)
        {
            _iJwtService = iJwtService;
        }

        // 无授权
        [HttpGet("NoAuthorize")]
        public string NoAuthorize()
        {
            User user = new User
            {
                Username = "admin",
                Phone = "123456"
            };
            return _iJwtService.GenerateTokenString(user);
            //return "this is NoAuthorize";
        }

        // 有授权
        [Authorize]
        [HttpGet("Authorize")]
        public object Authorize([FromQuery] string token)
        {
            object data = _iJwtService.ParseToken(token);
            string jsonData = JsonConvert.SerializeObject(data);
            return jsonData;
            //return "this is Authorize";
        }
    }
}
