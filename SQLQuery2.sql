create database UMSystemDB
use UMSystemDB

-- 1.用户表
create table Users
(
	id int identity primary key,
	username varchar(50) not null,
	[password] varchar(100) default null,
	email varchar(50) default null,
	phone varchar(20) default null,
	[status] bit default null,
	avatar varchar(200) default null,
	deleted bit default 0
)

-- 用户表插入数据
insert into Users(username, [password], email, phone, [status], avatar, deleted)
values
('admin','123456','super@aliyun.com','18677778888','1','D:\BZ\avatar.jpg','0'),
('iKun','123456','ikun@qq.com','16833336666','1','D:\BZ\avatar.jpg','0'),
('鸡哥','123456','jige@gmail.com','16385649986','1','D:\BZ\avatar.jpg','0'),
('坤坤','123456','kk@163.com','17654688888','1','D:\BZ\avatar.jpg','0'),
('小冰','123456','ice@outlook.com','18866668888','1','D:\BZ\avatar.jpg','0'),
('晓晓','123456','xiaoxiao@qq.com','16542358846','1','D:\BZ\avatar.jpg','0')


select * from Users,[dbo].[Roles]


-- 2.角色表
create table Roles 
(
	Role_id int not null primary key identity,
	Role_name varchar(50) default null,
	Role_desc varchar(100) default null
)

insert into Roles (Role_name, Role_desc) values('admin','超级管理员');
insert into Roles (Role_name, Role_desc) values('hr','人事专员');
insert into Roles (Role_name, Role_desc) values('normal','普通员工');



-- 3.用户角色映射表
create table UserRoles 
(
  Id int not null primary key identity,
  [User_id] int not null,
  [Role_id] int not null,
)


insert into UserRoles ([User_id], Role_id) values('1','1');



select * from [dbo].[Users]
select * from [dbo].[Roles]
select * from [dbo].[UserRoles]


select a.Role_name from [dbo].[Roles] a, [dbo].[UserRoles] b
where a.Role_id = b.Role_id and b.User_id = 1 