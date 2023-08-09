using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UM.Model
{
    public class Role
    {
        [Key] // 将 Role_id 定义为主键
        public int Role_id { get; set; }
        public string Role_name { get; set; }
        public string Role_desc { get; set; }

        // 导航属性：一个职位可以有多个用户
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
