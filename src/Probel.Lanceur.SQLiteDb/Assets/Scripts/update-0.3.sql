drop table if exists alias_usage_temp;
create table alias_usage_temp (
    id         integer primary key,
    id_alias   integer,
    time_stamp timestamp default current_timestamp  ,
    foreign key(id_alias) references alias
);


insert into alias_usage_temp 
	select * from alias_usage;

drop table if exists alias_usage;
create table alias_usage (
    id         integer primary key,
    id_alias   integer,
    id_session integer,
    time_stamp timestamp default current_timestamp,
    foreign key(id_alias)   references alias
    foreign key(id_session) references alias_session
);

insert into alias_usage 
	select 
		id         as id,
		id_alias   as id_alias,
		1          as id_session,
		time_stamp as time_stamp
	from alias_usage_temp;

drop table if exists alias_usage_temp;	

update alias_usage 
set
	id_session  = 1;