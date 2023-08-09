using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UM.Model.Auth
{
    // 登录请求模型 用于请求成功后，返回的数据
    public class LoginResponseModel
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
