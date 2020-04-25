use master;
DECLARE @dbname nvarchar(128)
SET @dbname = 'NcSoftBase';

IF (EXISTS (SELECT name 
FROM master.dbo.sysdatabases 
WHERE ('[' + name + ']' = @dbname 
OR name = @dbname)))
	drop database NcSoftBase;
go
create database NcSoftBase;
go
use NcSoftBase;

create table settings
(
	id int primary key default 0,
	captcha_api_key nvarchar(40) not null,
	count_threads int not null default 1
)
insert into settings(captcha_api_key)values('d2046ad540a4366975d2893791dc6d9e');
go
create table types_proxy
(
	id int primary key identity(1,1),
	text_type nvarchar(10) not null
)
insert into types_proxy(text_type) values('ssh');
insert into types_proxy(text_type) values('https');
insert into types_proxy(text_type) values('socks');
create table countrys
(
	id int primary key identity(1,1),
	country nvarchar(50) unique not null
)
insert into countrys(country) values('anywhere');
create table results_using_proxy
(
	id int primary key identity(1,1),
	text nvarchar(20)
)
insert into results_using_proxy(text) values('error');
insert into results_using_proxy(text) values('registered');
create table proxys
(
	id int primary key identity(1,1),
	ip nvarchar(15) not null unique,
	port int not null,
	login_ nvarchar(30),
	password_ nvarchar(30),
	type_id int foreign key references types_proxy(id),
	country_id int foreign key references countrys(id),
	date_use datetime,
	result_id int,
	text_error nvarchar(50),
)
insert into proxys(ip,port) values('152.235.214.250',22)
create table emails
(
	id int primary key identity(1,1),
	email nvarchar(50) unique,
	password_ nvarchar(16),
	country_id int foreign key references countrys(id) default 0,
	confirm_email nvarchar(50),
	phone_confirm_email nvarchar(11)

)
insert into emails(email, password_,country_id) values('emali', 'password_email',1);
create table statuses_registration
(
	id int primary key identity(1,1),
	text_status nvarchar(50) unique
)
insert into statuses_registration(text_status) values('created');
insert into statuses_registration(text_status) values('registered');
insert into statuses_registration(text_status) values('confirmed');
create table user_agents
(
	id int primary key identity(1,1),
	value nvarchar(150) not null unique
)
create table accounts
(
	id int primary key identity(1,1),
	email_id int foreign key references emails(id) not null unique,
	password_ nvarchar(16),
	display_name nvarchar(16),
	date_of_birth datetime not null,
	proxy_id int foreign key references proxys(id) not null,
	status_id int foreign key references statuses_registration(id),
	date_created datetime not null,
	date_registered datetime,
	date_confirmed datetime,
	count_try int default 1,
	user_agent_id int foreign key references user_agents(id)
)
insert into accounts(email_id, password_, proxy_id,date_created,date_of_birth,status_id) values(1,'passwordAcc', 1, '2016.10.11','2016.10.11',3)
create table open_socks_tunnels
(
	id int primary key identity(1,1),
	proxy_id int foreign key references proxys(id),
	status_defiant nvarchar(50),
	status_observing nvarchar(50),
	local_port int
)


go
create view created_accounts as
select
statuses_registration.text_status as status_,  
email as email,
accounts.password_ as password_,
proxys.ip as ip, 
proxys.port as port, 
count_try, 
accounts.date_created as date_created, 
accounts.date_registered as date_registered,
accounts.date_confirmed as date_confirmed

from emails inner join accounts on emails.id = accounts.email_id
inner join proxys on accounts.proxy_id = proxys.id
inner join statuses_registration on statuses_registration.id = accounts.status_id
where statuses_registration.text_status='confirmed'
go
select * from created_accounts
go