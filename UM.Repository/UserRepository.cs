using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UM.Model;
using UM.IRepository;
using UM.DataAccess;

namespace UM.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        // 通过调用 base(context) 调用父类BaseRepository的构造方法，获取UmDbContext的实例
        public UserRepository(UmDbContext context) : base(context)
        {
        }


        /// <summary>
        /// 通过用户ID获取角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<string> GetRolesByUserId(int userId)
        {
            var roleNames = _context.Roles
            .Join(_context.UserRoles,
                role => role.Role_id,
                userRole => userRole.Role_id,
                (role, userRole) => new { Role = role, UserRole = userRole })
            .Where(joinResult => joinResult.UserRole.User_id == userId)
            .Select(joinResult => joinResult.Role.Role_name)
            .ToList();

            return roleNames;
        }


        /// <summary>
        /// 查找用户
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public User SelectOne(string username, string password)
        {
            // FirstOrDefault 方法用于获取满足条件的第一个元素或默认值，是 LINQ 查询中常用的方法之一。
            return _context.Users.FirstOrDefault(
                u => u.Username == username && u.Password == password);
        }

        public User SelectOne(string username)
        {
            // FirstOrDefault 方法用于获取满足条件的第一个元素或默认值，是 LINQ 查询中常用的方法之一。
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }
    }
}
