using System;
using System.Collections.Generic;
using UM.IRepository;
using UM.IService;
using UM.Model;

namespace UM.Service
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class, new()
    {
        // 通过依赖注入在子类的构造函数中传入 IUserRepository 实例
        protected IBaseRepository<TEntity> _iBaseService;

        public bool Create(TEntity entity)
        {
            return _iBaseService.Create(entity);
        }

        //public bool Delete(TEntity entity)
        //{
        //    return _iBaseService.Delete(entity);
        //}

        public bool Delete(User entity)
        {
            return _iBaseService.Delete(entity);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _iBaseService.GetAll();
        }

        public TEntity GetById(int id)
        {
            return _iBaseService.GetById(id);
        }

        public bool Update(TEntity entity)
        {
            return _iBaseService.Update(entity);
        }
    }
}
