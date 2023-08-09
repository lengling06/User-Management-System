using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UM.Model
{
    public class UserRole
    {
        public int Id { get; set; } // 添加一个Id字段作为中间表的主键

        public int User_id { get; set; } // 外键，引用到User表的主键
        [ForeignKey("User_id")]
        public User User { get; set; }

        public int Role_id { get; set; } // 外键，引用到Role表的主键
        [ForeignKey("Role_id")]
        public Role Role { get; set; }
    }
}
