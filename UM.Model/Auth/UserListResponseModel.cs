using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UM.Model.Auth
{
    // 查询用户返回的模型，避免敏感信息返回给前端
    public class UserListResponseModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; }
        public string Avatar { get; set; }
        public bool Deleted { get; set; }
    }
}
