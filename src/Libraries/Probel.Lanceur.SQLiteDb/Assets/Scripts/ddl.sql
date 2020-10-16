/******************************************************************************
 * Drop all previous tables and data
 ******************************************************************************/
drop table if exists alias_session;
drop table if exists alias;
drop table if exists alias_name;
drop table if exists alias_usage;

/******************************************************************************
 * Build the tables
 ******************************************************************************/
create table alias_session (
    id    integer primary key,
    name  text,
    notes text
);

create table alias (
    id          integer primary key,
    arguments   text,
    file_name   text,
    notes       text,
    run_as      text,
    start_mode  text,
    working_dir text,
    id_session  integer,
    foreign key(id_session) references alias_session(id)
);
  
create table alias_name (
    id          integer primary key,
    id_alias integer,
    name        text,
    foreign key(id_alias) references alias(id)
);

create table alias_usage (
    id           integer primary key,
    id_alias integer,
    time_stamp   timestamp default current_timestamp  ,
    foreign key(id_alias) references alias(id)
);

/******************************************************************************
 * Fill with default data
 ******************************************************************************/
 insert into alias_session (id, name, notes) values (1, 'home', 'session when you''re at home');
 insert into alias_session (id, name, notes) values (2, 'work', 'session when you''re at work');