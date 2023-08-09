using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UM.Model;
using UM.IService;
using UM.IRepository;
using UM.Model.Auth;
using StackExchange.Redis;
using Newtonsoft.Json;
using UM.DataAccess;
using Microsoft.EntityFrameworkCore;
using UM.Utility;

namespace UM.Service
{
    public class UserService : BaseService<User>, IUserService
    {
        private readonly IUserRepository _iUserRepository;
        private readonly IConnectionMultiplexer _redis;
        private readonly UmDbContext _context;

        public UserService(IUserRepository iUserRepository, IConnectionMultiplexer redis, UmDbContext context) 
        {
            // 通过调用 base._iBaseService = iUserRepository
            // 将 IUserRepository 实例传递给基类 BaseService<User> 的 _iBaseService 字段。
            base._iBaseService = iUserRepository;
            _iUserRepository = iUserRepository;
            _redis = redis;
            _context = context;
        }


        #region 密码未加密的登录方法
        /*
        public Dictionary<string, object> Login(LoginRequestModel model)
        {
            // 根据用户名和密码查询
            User user = _iUserRepository.GetUserByUsernameAndPassword(model.username, model.password);
            // 结果不为空，则生成token，并将用户信息存入redis
            if (user != null)
            {
                // 暂时用UUID，终极方案是用JWT
                string key = "user:" + Guid.NewGuid();

                user.Password = null; // 设置密码为空，不存入redis
                // 将用户信息存入 Redis 缓存
                IDatabase db = _redis.GetDatabase();
                // 使用 JSON 序列化将 User 对象转换为 JSON 字符串并存储
                string jsonUser = JsonConvert.SerializeObject(user);
                db.StringSet(key, jsonUser, TimeSpan.FromMinutes(30)); // 设置30分钟有效期

                // 返回数据
                var data = new Dictionary<string, object>
                {
                    // 添加数据，相当于Add("token", key)
                    { "token", key }
                };
                return data;
            }
            return null;
        }
        */
        #endregion

        #region SHA256加密
        public Dictionary<string, object> Login(LoginResponseModel model)
        {
            // 根据用户名查询
            User user = _iUserRepository.SelectOne(model.username);
            // 结果不为空，则生成token，并将用户信息存入redis
            if (user != null && (SHA256Helper.SHA256(model.password) == user.Password))
            {
                // 暂时用UUID，终极方案是用JWT
                string key = "user:" + Guid.NewGuid();

                user.Password = null; // 设置密码为空，不存入redis
                // 将用户信息存入 Redis 缓存
                IDatabase db = _redis.GetDatabase();
                // 使用 JSON 序列化将 User 对象转换为 JSON 字符串并存储
                string jsonUser = JsonConvert.SerializeObject(user);
                db.StringSet(key, jsonUser, TimeSpan.FromMinutes(30)); // 设置30分钟有效期

                // 返回数据
                var data = new Dictionary<string, object>
                {
                    // 添加数据，相当于Add("token", key)
                    { "token", key }
                };
                return data;
            }
            return null;
        }
        #endregion



        public Dictionary<string, object> GetUserInfo(string token)
        {
            // 根据token获取用户信息，redis
            IDatabase db = _redis.GetDatabase();
            string jsonUser = db.StringGet(token);
            if (null != jsonUser)
            {
                // 使用 JSON 反序列化将 JSON 字符串转换回 User 对象
                User loginUser = JsonConvert.DeserializeObject<User>(jsonUser);

                // 返回用户信息
                var data = new Dictionary<string, object>
                {
                    { "name", loginUser.Username },
                    { "avatar", loginUser.Avatar }
                };
                // 获取用户角色
                List<string> roleList = _iUserRepository.GetRolesByUserId(loginUser.Id);
                data.Add("roles", roleList);

                return data;
            }
            // 如果找不到对应的用户信息，返回 null 或其他指定的信息
            return null;
        }


        public void Logout(string token)
        {
            IDatabase db = _redis.GetDatabase();
            db.KeyDelete(token);
        }
        

        async Task<PagedResult<UserListResponseModel>> IUserService.GetUserListAsync(string username, string phone, int pageNo, int pageSize)
        {
            IQueryable<UserListResponseModel> query = _context.Users
                .Where(u => string.IsNullOrEmpty(username) || u.Username.Contains(username))
                .Where(u => string.IsNullOrEmpty(phone) || u.Phone.Contains(phone))
                .Select(u => new UserListResponseModel
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    Phone = u.Phone,
                    Status = u.Status,
                    Avatar = u.Avatar,
                    Deleted = u.Deleted
                });

            int totalCount = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            List<UserListResponseModel> users = await query.Skip((pageNo - 1) * pageSize)
                                                    .Take(pageSize)
                                                    .ToListAsync();

            return new PagedResult<UserListResponseModel>
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                Rows = users
            };
        }
    }
}
