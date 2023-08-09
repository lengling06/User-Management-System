using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UM.Model;

namespace UM.IRepository
{
    public interface IUserRepository : IBaseRepository<User>
    {
        /// <summary>
        /// 查找用户接口
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        User SelectOne(string username, string password);
        User SelectOne(string username);    // 通过用户名


        /// <summary>
        /// 通过用户ID获取角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<string> GetRolesByUserId(int userId);
    }
}
