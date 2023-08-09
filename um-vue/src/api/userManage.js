import request from '@/utils/request'
import qs from 'qs' // 用于序列化和解析 URL 查询字符串。

// 定义接口信息，用于对接后端接口

export function login(data) {
  return request({
    url: '/user/login',
    method: 'post',
    data
  })
}

export default {
  getUserList(searchModel) {
    return request({
      url: '/user/list',
      method: 'get',
      params: {
        pageNo: searchModel.pageNo,
        pageSize: searchModel.pageSize,
        username: searchModel.username,
        phone: searchModel.phone
      }
    })
  },
  addUser(user) {
    return request({
      url: '/user/create',
      method: 'post',
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded'
      },
      // 直接传递user到后端，后端会因为数据类型的问题，接收不到数据
      // 所以需要使用使用 qs 库将 POST 请求的数据序列化为合适的格式，以便在请求体中发送。
      data: qs.stringify(user)
    })
  },
  upDateUser(user, id) {
    return request({
      url: `/user/${id}`,
      method: 'put',
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded'
      },
      data: qs.stringify(user)
    })
  },
  saveUser(user, id) { // 判断密码是否为空，来决定更新用户操作还是新增用户操作。 （修改操作没有密码项）
    if (user.password === null || user.password === undefined) {
      return this.upDateUser(user, id)
    }
    return this.addUser(user)
  },
  getUserById(id) {
    return request({
      // url: '/user/create/' + id,
      url: `/user/${id}`,
      method: 'get'
    })
  },
  deleteUserById(id) {
    return request({
      url: `/user/${id}`,
      method: 'delete'
    })
  }
}

