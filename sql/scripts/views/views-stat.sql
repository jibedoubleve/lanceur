/*
 * Helper view to list all the hours in the day.
 * This view is used amongnst other in 
 * 'stat_usage_per_hour_in_day_v'
 */
drop view if exists helper_hour_in_day;
create view helper_hour_in_day as
	select 0 as exec_count, '00:00' as hour_in_day
    union select 0 as exec_count, '01:00' as hour_in_day
    union select 0 as exec_count, '02:00' as hour_in_day
    union select 0 as exec_count, '03:00' as hour_in_day
    union select 0 as exec_count, '04:00' as hour_in_day
    union select 0 as exec_count, '05:00' as hour_in_day
    union select 0 as exec_count, '06:00' as hour_in_day
    union select 0 as exec_count, '07:00' as hour_in_day
    union select 0 as exec_count, '08:00' as hour_in_day
    union select 0 as exec_count, '09:00' as hour_in_day
    union select 0 as exec_count, '10:00' as hour_in_day
    union select 0 as exec_count, '11:00' as hour_in_day
    union select 0 as exec_count, '12:00' as hour_in_day
    union select 0 as exec_count, '13:00' as hour_in_day
    union select 0 as exec_count, '14:00' as hour_in_day
    union select 0 as exec_count, '15:00' as hour_in_day
    union select 0 as exec_count, '16:00' as hour_in_day
    union select 0 as exec_count, '17:00' as hour_in_day
    union select 0 as exec_count, '18:00' as hour_in_day
    union select 0 as exec_count, '19:00' as hour_in_day
    union select 0 as exec_count, '20:00' as hour_in_day
    union select 0 as exec_count, '21:00' as hour_in_day
    union select 0 as exec_count, '22:00' as hour_in_day
    union select 0 as exec_count, '23:00' as hour_in_day
    order by hour_in_day;

/* 
 * View with the count of execution by keyword
 */
drop view if exists stat_execution_count_v;
create view stat_execution_count_v as
    select 
        id_keyword as id_keyword,
        count(*)   as exec_count, 
        keywords   as keywords
    from 
        stat_history_v sh 
    group by 
        id_keyword 
    order by exec_count	desc; 
-------------------------------------------------------------------------------
/*
 * Fix history issue in the view
 */
drop view if exists stat_history_v;
create view stat_history_v as
    select 
    	s.id                        as id_keyword,
    	group_concat(sn.name, ', ') as keywords, 
    	su.time_stamp               as time_stamp 
    from 
    	alias_usage su
    	inner join alias s on su.id_alias = s.id
    	inner join alias_name sn on s.id = sn.id_alias	
	group by su.time_stamp;    
-------------------------------------------------------------------------------
/*
 * Show usage per day/month/year
 */
drop view if exists stat_usage_per_day_v;
create view stat_usage_per_day_v as
    select 
        count(*)                         as exec_count,
        strftime('%Y-%m-%d', time_stamp) as day
    from 
    	alias_usage
    where 
		strftime('%Y-%m-%d', time_stamp) < strftime('%Y-%m-01', date())
    group by 
        strftime('%Y-%m-%d', time_stamp)
    order by 	
        time_stamp;
-------------------------------------------------------------------------------
/*
 * Show usage per month/year
 */
drop view if exists stat_usage_per_month_v;
create view stat_usage_per_month_v as
    select 
        count(*)                         as exec_count,
        strftime('%Y-%m-01', time_stamp) as month
    from 
    	alias_usage 
    where 
		strftime('%Y-%m-%d', time_stamp) < strftime('%Y-%m-01', date())
    group by 
        strftime('%Y-%m-01', time_stamp)
    order by 	
        time_stamp;    
-------------------------------------------------------------------------------
/*
 * Show usage per day of week
 */
drop view if exists stat_usage_per_day_of_week_v;
create view stat_usage_per_day_of_week_v as 
    select 
        count(*)                    as exec_count,
        case cast(strftime('%w', time_stamp) as integer)
        	when 0 then 7
        	else cast(strftime('%w', time_stamp) as integer)
        end as day_of_week,
        case cast(strftime('%w', time_stamp) as integer)
        	when 0 then 'Sunday'
        	when 1 then 'Monday'
        	when 2 then 'Tuesday'
        	when 3 then 'Wednesday'
        	when 4 then 'Thursday'
        	when 5 then 'Friday'
        	when 6 then 'Saterday'
        	else 'error'
		end as day_name
    from 
    	alias_usage 
    group by 
        strftime('%w', time_stamp)
    order by 	
		day_of_week;                            
-------------------------------------------------------------------------------
/*
 * Show history per hour in day
 */
drop view if exists stat_usage_per_hour_in_day_v;
create view stat_usage_per_hour_in_day_v as 
	select 
		sum(exec_count) as exec_count,
		hour_in_day     as hour_in_day
	from (	
		select *  	
		from (
			select 
		        count(*)                      as exec_count,
		    	strftime('%H:00', time_stamp) as hour_in_day
			from 
		    	alias_usage
		    where 
				strftime('%Y-%m-%d', time_stamp) < strftime('%Y-%m-01', date())
			group by strftime('%H:00', time_stamp)    	
		)
		union all select * from helper_hour_in_day
	)	
	group by hour_in_day
	order by hour_in_day