using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UM.Model;
using UM.Model.Auth;

namespace UM.IService
{
    /*
     * 服务类
     */
    public interface IUserService : IBaseService<User>
    {
        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Dictionary<string, object> Login(LoginResponseModel model);


        /// <summary>
        /// 获取用户信息接口
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Dictionary<string, object> GetUserInfo(string token);


        /// <summary>
        /// 注销
        /// </summary>
        /// <param name="token"></param>
        void Logout(string token);


        /// <summary>
        /// 查询用户数据
        /// </summary>
        /// <param name="username"></param>
        /// <param name="phone"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PagedResult<UserListResponseModel>> GetUserListAsync(string username, string phone, int pageNo, int pageSize);
    }
}
