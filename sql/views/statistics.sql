drop view if exists stat_history;
create view stat_history as
    select sn.name, su.time_stamp
    from shortcut_usage su
    inner join shortcut s on su.id_shortcut = s.id
    inner join shortcut_name sn on s.id = sn.id_shortcut