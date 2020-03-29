/* View with the count of execution by keyword
 */
drop view if exists stat_execution_count;
create view stat_execution_count as
    select 
        id_keyword as id_keyword,
        count(*)   as exec_count, 
        keywords 
    from 
        stat_history sh 
    group by 
        id_keyword 
    order by exec_count	desc 