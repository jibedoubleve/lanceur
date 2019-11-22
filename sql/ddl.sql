/******************************************************************************
 * Drop all previous tables and data
 ******************************************************************************/
drop table if exists shortcut_session;
drop table if exists shortcut;
drop table if exists shortcut_name;
drop table if exists shortcut_usage;

/******************************************************************************
 * Build the tables
 ******************************************************************************/
create table shortcut_session (
    id    integer primary key,
    name  text,
    notes text
);

create table shortcut (
    id         integer primary key,
    arguments  text,
    file_name  text,
    notes      text,
    run_as     text,
    start_mode text,
    id_session integer,
    foreign key(id_session) references shortcut_session
);
  
create table shortcut_name (
    id          integer primary key,
    id_shortcut integer,
    name        text,
    foreign key(id_shortcut) references shortcut
);

create table shortcut_usage (
    id           integer primary key,
    id_shortcut integer,
    time_stamp   timestamp default current_timestamp  ,
    foreign key(id_shortcut) references shortcut
);

/******************************************************************************
 * Fill with default data
 ******************************************************************************/
 insert into shortcut_session (id, name, notes) values (1, 'home', 'session when you''re at home');
 insert into shortcut_session (id, name, notes) values (2, 'work', 'session when you''re at work');