using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UM.Model.Auth
{
    // 用户查询时返回的数据模型
    public class UpdateResponseModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; }
    }
}
