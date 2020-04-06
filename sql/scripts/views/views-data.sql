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
        left join (
            select 	
	            a.file_name,
	            a.arguments,
    	        a.run_as,
                a.working_dir
            from 
    	        alias a
            group by 
    	        a.file_name,
    	        a.arguments,
    	        a.run_as,
                a.working_dir
            having 
    	        count(a.id) >= 2
        ) t on a.file_name = t.file_name
    where 
        t.file_name is not null	
    group by a.id	
    order by a.file_name;    