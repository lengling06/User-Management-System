create database UMSystemDB
use UMSystemDB

-- �����û���
create table um_user
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

-- �û����������
insert into um_user(username, [password], email, phone, [status], avatar, deleted)
values
('admin','123456','super@aliyun.com','18677778888','1','D:\BZ\avatar.jpg','0'),
('iKun','123456','ikun@qq.com','16833336666','1','D:\BZ\avatar.jpg','0'),
('����','123456','jige@gmail.com','16385649986','1','D:\BZ\avatar.jpg','0'),
('����','123456','kk@163.com','17654688888','1','D:\BZ\avatar.jpg','0'),
('С��','123456','ice@outlook.com','18866668888','1','D:\BZ\avatar.jpg','0'),
('����','123456','xiaoxiao@qq.com','16542358846','1','D:\BZ\avatar.jpg','0')


select * from um_user