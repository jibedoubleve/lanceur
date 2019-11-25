drop view if exists stat_history;
create view stat_history as
    select sn.name, su.time_stamp
    from alias_usage su
    inner join alias s on su.id_alias = s.id
    inner join alias_name sn on s.id = sn.id_alias