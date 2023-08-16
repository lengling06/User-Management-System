using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UM.Utility.ApiResult
{
    // 封装状态码
    public enum ApiStatusCode
    {
        Success = 200,  // 请求成功
        BadRequest = 400,   // 请求失败
        Unauthorized = 401, // 无权限
        NotFound = 404, // 服务器找不到资源
        InternalServerError = 500   // 服务器内部错误
    }

    //  API 响应对象的实用类
    public static class ApiResultHelper
    {
        // 成功后返回的数据
        public static ApiResult Success(dynamic data)
        {
            return new ApiResult
            {
                // 自定义返回结果
                Code = (int)ApiStatusCode.Success,
                Data = data,
                Msg = "操作成功！",
                //Total = 0
            };
        }

        public static ApiResult Success(dynamic data, string msg)
        {
            return new ApiResult
            {
                // 自定义返回结果
                Code = (int)ApiStatusCode.Success,
                Data = data,
                Msg = msg,
                //Total = 0
            };
        }

        // 分页
        public static ApiResult Success(dynamic data,int total)
        {
            return new ApiResult
            {
                Code = (int)ApiStatusCode.Success,
                Data = data,
                Msg = "操作成功！",
                // Total = total
            };
        }

        // 错误信息
        public static ApiResult Error(ApiStatusCode statusCode, string msg)
        {
            return new ApiResult
            {
                Code = (int)statusCode,
                Data = null,
                Msg = msg,
                //Total = 0
            };
        }
    }
}
