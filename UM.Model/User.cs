using System;
using System.Collections.Generic;

namespace UM.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; }
        public string Avatar { get; set; }
        public bool Deleted { get; set; }


        // 导航属性：一个用户可以有多个职位
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
