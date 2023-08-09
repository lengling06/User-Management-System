using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace um_asp.net_core.Utility.ApiResult
{
    // 公共响应类 用于将 API 调用的结果统一封装成一个对象
    public class ApiResult
    {
        public int Code { get; set; }   // 状态码
        public string Msg { get; set; } // 提示信息
        // public int Total { get; set; }  // 分页

        // 不同的API返回的数据类型不同，所以可以指定为 dynamic 动态数据类型。
        // dynamic 关键字声明的变量可以在编译时不指定其类型，而是在运行时决定其类型。
        public dynamic Data { get; set; }  // 数据
    }
}
