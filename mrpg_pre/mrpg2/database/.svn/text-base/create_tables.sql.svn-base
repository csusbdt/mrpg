drop table if exists inventory;
drop table if exists avatar;
drop table if exists login_credentials;

-----------------------------------------------------------------------------
-- login credentials

create table login_credentials
(
   username varchar(255),
   password varchar(255),
   primary key (username)
) engine=InnoDB default charset=latin1;

insert into login_credentials (username, password) values ('un', 'pw');
insert into login_credentials (username, password) values ('x', 'x');


-----------------------------------------------------------------------------
-- avatar

create table avatar
(
   avatar_id varchar(255),
   username varchar(255),
   avatar_class varchar(255) default 'mage',
   max_health_points float default 100,
   primary key (avatar_id),
   foreign key (username) references login_credentials(username)
) engine=InnoDB default charset=latin1;

insert into avatar(avatar_id, username) values ('xa', 'x');
insert into avatar(avatar_id, username) values ('xb', 'x');
insert into avatar(avatar_id, username) values ('una', 'un');

-----------------------------------------------------------------------------
-- inventory

create table inventory
(
   entity_id varchar(255),
   entity_class varchar(255) default NULL,
   avatar_id varchar(255),
   primary key (entity_id),
   foreign key (avatar_id) references avatar(avatar_id)
) engine=InnoDB default charset=latin1;

insert into inventory(entity_id, entity_class, avatar_id) 
values ('daggerxa', 'dagger', 'xa');


