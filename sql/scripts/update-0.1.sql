/*
 * Create tables and value to manage dabatase version.
 * This data is used to know whether to make an update
 * of the database
 */
create table settings (
    id    integer primary key,
    s_key   text,
    s_value text
);

insert into settings (s_key, s_value) values ('db_version','0.1');

/*
 * Fix history issue in the view
 */
drop view if exists stat_history;
create view stat_history as
    select 
    	s.id                        as id_keyword,
    	group_concat(sn.name, ', ') as keywords, 
    	su.time_stamp               as time_stamp 
    from 
    	alias_usage su
    	inner join alias s on su.id_alias = s.id
    	inner join alias_name sn on s.id = sn.id_alias
	group by su.time_stamp; 