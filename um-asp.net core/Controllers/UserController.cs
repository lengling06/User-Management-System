using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UM.IService;
using UM.Model;
using UM.Model.Auth;
using UM.Utility;
using um_asp.net_core.Utility.ApiResult;

namespace um_asp.net_core.Controllers
{
    // [Route("api/[controller]")]
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly IUserService _iUserService;
        public UserController(IUserService iUserService) 
        {
            this._iUserService = iUserService;
        }

        [HttpGet("getAllUser")]
        public ActionResult<ApiResult> GetAllUsers()
        {
            var users = _iUserService.GetAll();
            if (null != users)
            {
                return ApiResultHelper.Success(users);
            }
            return ApiResultHelper.Error(ApiStatusCode.BadRequest, "没有查询到用户！");
        }


        [HttpPost("login")]
        public ActionResult<ApiResult> Login([FromBody] LoginResponseModel model)
        {
            var data = _iUserService.Login(model);
            if (null != data)
            {
                return ApiResultHelper.Success(data);
            }
            return ApiResultHelper.Error(ApiStatusCode.BadRequest, "用户名或密码错误！");
        }


        [HttpGet("info")]
        public ActionResult<ApiResult> GetUserInfo([FromQuery(Name = "token")]string token) 
        {
            // 根据token获取用户信息， redis
            var data = _iUserService.GetUserInfo(token);
            if (null != data)
            {
                return ApiResultHelper.Success(data);
            }
            return ApiResultHelper.Error(ApiStatusCode.BadRequest, "登录信息无效，请重新登录！");
        }


        [HttpPost("logout")]
        public ActionResult<ApiResult> Logout([FromHeader(Name = "X-Token")]string token) 
        {
            _iUserService.Logout(token);
            return ApiResultHelper.Success(null);
        }


        [HttpGet("list")]
        public async Task<ActionResult<ApiResult>> GetUserListAsync(
            [FromQuery(Name = "username")] string username,
            [FromQuery(Name = "phone")] string phone,
            [FromQuery(Name = "pageNo")] int pageNo,
            [FromQuery(Name = "pageSize")] int pageSize) 
        {
            var data = await _iUserService.GetUserListAsync(username, phone, pageNo, pageSize);
            return ApiResultHelper.Success(data);
        }


        [HttpPost("create")]
        public ActionResult<ApiResult> AddUser(User user)
        {
            if (null != user)
            {
                user.Password = SHA256Helper.SHA256(user.Password);
                if (_iUserService.Create(user))
                {
                    return ApiResultHelper.Success(null, "新增用户成功！");
                }
            }
            return ApiResultHelper.Error(ApiStatusCode.BadRequest, "添加失败！");
        }


        [HttpPut("{id}")]
        public ActionResult<ApiResult> UpdateUser(int id, UpdateResponseModel userModel)
        {
            if (null != userModel)
            {
                var userToUpdate = _iUserService.GetById(id);   // 通过id查询用户
                // 然后把传过来的值赋值给查询到的对象
                userToUpdate.Username = userModel.Username;
                userToUpdate.Phone = userModel.Phone;
                userToUpdate.Status = userModel.Status;
                userToUpdate.Email = userModel.Email;
                if (_iUserService.Update(userToUpdate))
                {
                    return ApiResultHelper.Success(null, "用户信息已更新！");
                }
            }
            return ApiResultHelper.Error(ApiStatusCode.BadRequest, "更新用户信息失败！");
        }

        [HttpGet("{id}")]
        public ActionResult<ApiResult> GetUserById(int id) 
        {
            var user = _iUserService.GetById(id);
            if (null != user)
            {
                UpdateResponseModel userModel = new UpdateResponseModel
                {
                    Username = user.Username,
                    Phone = user.Phone,
                    Status = user.Status,
                    Email = user.Email
                };
                return ApiResultHelper.Success(userModel, "获取成功！");
            }
            return ApiResultHelper.Error(ApiStatusCode.BadRequest, "获取用户失败！");
        }


        [HttpDelete("{id}")]
        public ActionResult<ApiResult> DeleteUserById(int id)
        {
            var userToDelete = _iUserService.GetById(id);

            if (null != userToDelete)
            {
                if (_iUserService.Delete(userToDelete))
                {
                    return ApiResultHelper.Success(null, "删除用户成功！");
                }
            }
            return ApiResultHelper.Error(ApiStatusCode.BadRequest, "删除失败！");
        }
    }
}
