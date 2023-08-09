using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using UM.DataAccess;
using UM.IRepository;
using UM.Model;

namespace UM.Repository
{
    // 实现IBaseRepository中的接口
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        // 通过依赖注入获取 UmDbContext 的实例
        // 依赖注入是一种设计模式，通过将类所需的依赖项从外部传递给类来实现解耦。
        // 使用构造函数注入的方式，BaseRepository 类不负责创建 UmDbContext 的实例，
        // 而是将 UmDbContext 的实例通过构造函数传递进来。
        protected readonly UmDbContext _context;
        public BaseRepository(UmDbContext context) 
        {
            _context = context;
        }

        public bool Create(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            return _context.SaveChanges() > 0;
        }

        // 物理删除
        //public bool Delete(TEntity entity)
        //{
        //    _context.Set<TEntity>().Remove(entity);
        //    return _context.SaveChanges() > 0;
        //}

        // 逻辑删除：只在数据库中标记，不执行删除
        public bool Delete(User entity)
        {
            entity.Deleted = true;
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChanges() > 0;
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().ToList();
        }

        public TEntity GetById(int id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public bool Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            return _context.SaveChanges() > 0;
        }
    }
}
