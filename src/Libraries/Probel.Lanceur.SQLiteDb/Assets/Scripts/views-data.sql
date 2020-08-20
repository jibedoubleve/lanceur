-------------------------------------------------------------------------------
/*
 * View with all the unused keywords 
 */
drop view if exists data_not_used_v;
create view data_not_used_v as
    select 	
        a.id,
        a.id_session, 
        group_concat(sn.name, ', ') as keywords,	
        a.file_name,
        a.arguments,
        a.run_as,
        a.working_dir	
    from
        alias a
        inner join alias_name sn on a.id = sn.id_alias
        left join stat_execution_count_v s on a.id = s.id_keyword 
    where 
        s.exec_count is null	
        or s.exec_count = 0
    group by a.id;
-------------------------------------------------------------------------------
/*
 * Displays all the doubloons
 */
drop view if exists data_doubloons_v;
create view data_doubloons_v as
    select 
        d.*,
        group_concat(an.name, ', ') as keywords
    from (
        select 
            a.*
        from alias a 
        inner join (
            select 
                file_name   as file_name, 
                id_session  as id_session ,
                arguments   as arguments ,
                run_as      as run_as ,
                working_dir as working_dir,  
                id_session  as id_session
            from alias
            group by 
                file_name, 
                id_session,		
                arguments,
                run_as,
                working_dir 
            having count(*) > 1
        ) d on  
            a.file_name         = d.file_name
            and a.id_session    = d.id_session
            and a.run_as        = d.run_as
            and ((a.working_dir = d.working_dir) is null or a.working_dir = d.working_dir) 
            and ((a.arguments   = d.arguments) is null   or a.arguments =d.arguments)
        order by a.file_name
    ) d
    inner join alias_name an on d.id = an.id_alias
    group by d.id;