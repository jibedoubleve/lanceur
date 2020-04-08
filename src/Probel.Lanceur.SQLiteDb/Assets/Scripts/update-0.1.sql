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

