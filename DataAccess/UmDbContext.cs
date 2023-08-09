using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UM.Model;

namespace UM.DataAccess
{
    public class UmDbContext:DbContext
    {
        public UmDbContext(DbContextOptions<UmDbContext> options) : base(options) 
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }


        // OnModelCreating() 方法在数据库上下文初始化时被调用，
        // 可以用于定义如何映射实体到数据库中的表以及各种约束和关系。
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => ur.Id); // 将UserRole实体类型配置为中间表，并定义Id为主键

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User) // 定义UserRole到User的关系
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.User_id);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role) // 定义UserRole到Role的关系
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.Role_id);

            // 配置实体的默认查询过滤条件来排除已经被标记为删除的记录
            modelBuilder.Entity<User>().HasQueryFilter(u => !u.Deleted);

            base.OnModelCreating(modelBuilder);
        }
    }
}
