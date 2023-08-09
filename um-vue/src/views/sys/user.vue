<template>
  <div>
    <!--    搜索栏-->
    <el-card class="search">
      <el-row>
        <el-col :span="20">
          <!--    clearable属性，可在一键清除输入框里的内容      -->
          <el-input v-model="searchModel.username" placeholder="用户名" clearable />
          <el-input v-model="searchModel.phone" placeholder="电话" clearable />
          <el-button type="primary" round icon="el-icon-search" @click="getUserList">查询</el-button>
        </el-col>
        <el-col :span="4" align="right">
          <!--    添加用户      -->
          <el-button type="primary" icon="el-icon-plus" circle @click="openEditUI(null)" />
        </el-col>
      </el-row>
    </el-card>

    <!--    查询结果-->
    <el-card>
      <el-table
        :data="userList"
        stripe
        style="width: 100%"
      >
        <el-table-column type="index" label="#" width="80">
          <!--    直接访问index，访问不到，需要使用插槽      -->
          <template slot-scope="scope">
            <!--     计算 # 行的序号：(pageNo - 1) * pageSize + index + 1    -->
            {{ (searchModel.pageNo - 1) * searchModel.pageSize + scope.$index+1 }}
          </template>
        </el-table-column>
        <el-table-column
          prop="id"
          label="用户ID"
          width="180"
        />
        <el-table-column
          prop="username"
          label="用户名"
          width="180"
        />
        <el-table-column
          prop="phone"
          label="联系电话"
          width="180"
        />
        <el-table-column prop="status" label="用户状态" width="180">
          <template slot-scope="scope">
            <!--     row：代表当前行       -->
            <el-tag v-if="scope.row.status == 1">正常</el-tag>
            <el-tag v-else type="danger">禁用</el-tag>
          </template>
        </el-table-column>
        <el-table-column
          prop="email"
          label="电子邮箱"
        />
        <el-table-column label="操作" width="180">
          <template slot-scope="scope">
            <el-button type="primary" icon="el-icon-edit" circle @click="openEditUI(scope.row.id)" />
            <el-button type="danger" icon="el-icon-delete" circle @click="deleteUser(scope.row)" />
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <!--    分页组件-->
    <el-pagination
      :current-page="this.searchModel.pageNo"
      :page-sizes="[5, 10, 20, 50]"
      :page-size="this.searchModel.pageSize"
      layout="total, sizes, prev, pager, next, jumper"
      :total="this.total"
      @size-change="handleSizeChange"
      @current-change="handleCurrentChange"
    />

    <!--    用户信息编辑对话框-->
    <el-dialog :title="title" :visible.sync="dialogFormVisible" @close="clearForm">
      <el-form ref="userFormRef" :model="userForm" :rules="rules">
        <el-form-item label="用户名" :label-width="formLabelWidth" prop="username">
          <el-input v-model="userForm.username" autocomplete="off" />
        </el-form-item>
        <el-form-item v-if="this.tag === null || this.tag === undefined" label="密码" :label-width="formLabelWidth" prop="password">
          <el-input v-model="userForm.password" autocomplete="off" type="password" />
        </el-form-item>
        <el-form-item label="联系电话" :label-width="formLabelWidth">
          <el-input v-model="userForm.phone" autocomplete="off" />
        </el-form-item>
        <el-form-item label="用户状态" :label-width="formLabelWidth">
          <el-switch
            v-model="userForm.status"
            :active-value="1"
            :inactive-value="0"
          />
        </el-form-item>
        <el-form-item label="电子邮件" :label-width="formLabelWidth" prop="email">
          <el-input v-model="userForm.email" autocomplete="off" />
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">取 消</el-button>
        <el-button type="primary" @click="saveUser">确 定</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import userApi from '@/api/userManage'

export default {
  name: 'User',
  data() {
    // 电子邮件验证
    const checkEmail = (rule, value, callback) => {
      const reg = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/
      if (!reg.test(value)) {
        return callback(new Error('请输入正确的邮箱地址！'))
      }
      callback()
    }
    return {
      userId: '', // 存储用户id，用于更新数据
      tag: '', // 控制密码框的显示和隐藏
      title: '', // 对话框标题
      total: 0, // 总数
      dialogFormVisible: false, // 用于控制对话框是否可见
      userForm: {}, // 对话框中的表单数据
      formLabelWidth: '130px', // 对话框中的标签宽度
      searchModel: {
        pageNo: 1, // 当前页
        pageSize: 10 // 每页要显示的数量
      },
      userList: [], // 用于存放查询到的用户数据
      rules: { // 数据验证
        username: [
          { required: true, message: '请输入用户名', trigger: 'blur' },
          { min: 2, max: 6, message: '长度在 2 到 6 个字符', trigger: 'blur' }
        ],
        password: [
          { required: true, message: '请输入密码', trigger: 'blur' },
          { min: 6, max: 16, message: '长度在 6 到 16 个字符', trigger: 'blur' }
        ],
        email: [
          { required: true, message: '请输入电子邮箱', trigger: 'blur' },
          { validator: checkEmail, trigger: 'blur' }
        ]
      }
    }
  },
  created() {
    this.getUserList()
  },
  methods: {
    // 删除用户
    deleteUser(user) {
      this.$confirm(`确认删除用户 ${user.username} ? 此操作不可逆！`, '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        userApi.deleteUserById(user.id).then(response => {
          this.$message({
            type: 'success',
            message: response.msg
          })
          this.getUserList()
        })
      }).catch(() => {
        this.$message({
          type: 'info',
          message: '已取消删除'
        })
      })
    },
    // 添加用户
    saveUser() {
      // 触发表单验证
      this.$refs.userFormRef.validate((valid) => {
        if (valid) {
          // 提交请求到后台
          userApi.saveUser(this.userForm, this.userId).then(response => {
            // 提交成功提示
            this.$message({
              message: response.msg,
              type: 'success'
            })
            // 关闭对话框
            this.dialogFormVisible = false
            // 刷新表格
            this.getUserList()
          })
        } else {
          console.log('error submit!!')
          return false
        }
      })
    },
    clearForm() {
      // 关闭对话框时，清空输入的内容
      this.userForm = {}
      this.$refs.userFormRef.clearValidate()
    },
    openEditUI(id) {
      // 打开编辑框，判断传进来的参数，如果为null，就说明是新增用户，反之则为修改
      this.tag = id // 传递给hide变量，如果为null则显示密码框，反之则隐藏
      if (id == null) { // 为空时，打开新增用户对话框
        this.title = '新增用户'
      } else {
        this.userId = id // 把当前点击行的用户ID传递给 userId，用于传递给后端查询用户后修改
        this.title = '修改用户'
        // 根据id查询用户数据
        userApi.getUserById(id).then(response => {
          this.userForm = response.data
        })
      }
      this.dialogFormVisible = true
    },
    handleSizeChange(pageSize) {
      this.searchModel.pageSize = pageSize
      this.getUserList()
    },
    handleCurrentChange(pageNo) {
      this.searchModel.pageNo = pageNo
      this.getUserList()
    },
    getUserList() { // 获取用户数据
      userApi.getUserList(this.searchModel).then(response => {
        this.userList = response.data.rows
        this.total = response.data.totalCount
      })
    }
  }
}
</script>

<style lang="scss" scoped>
.search .el-input {
  width: 200px;
  margin-right: 30px;
}
.el-dialog {
  .el-input {
    width: 85%;
  }
}
</style>
