using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UM.Utility.ApiResult;

namespace UM.JWT
{
    public interface IJwtService
    {
        /// <summary>
        /// 暂时废弃
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        // ActionResult<ApiResult> GenerateToken(string username, string password);
        
        /// <summary>
        /// 生成JWT令牌
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        string GenerateTokenString(object data);

        /// <summary>
        /// 根据JWT令牌解析出JSON数据对象
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        object ParseToken(string token);
    }
}
