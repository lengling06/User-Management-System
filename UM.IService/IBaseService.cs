using System;
using System.Collections.Generic;
using UM.Model;

namespace UM.IService
{
    // TEntity 目标实体的数据类型
    // 约束：where T : class   必须是一个类
    //       where T : new() | T必须要有一个无参构造函数
    public interface IBaseService<TEntity> where TEntity : class, new()
    {
        TEntity GetById(int id);
        IEnumerable<TEntity> GetAll();
        bool Create(TEntity entity);
        bool Update(TEntity entity);
        //bool Delete(TEntity entity);
        bool Delete(User entity);
    }
}
