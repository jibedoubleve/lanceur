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
 * Helper view to list all the days in the week.
 * This view is used amongnst other in 
 * 'stat_usage_per_hour_in_day_v'
 */
drop view if exists helper_day_in_week;
create view helper_day_in_week as
	      select 0 as exec_count, 'Sunday'    as day_name, 7 as day_of_week 
    union select 0 as exec_count, 'Monday'    as day_name, 1 as day_of_week
    union select 0 as exec_count, 'Tuesday'   as day_name, 2 as day_of_week
    union select 0 as exec_count, 'Wednesday' as day_name, 3 as day_of_week
    union select 0 as exec_count, 'Thursday'  as day_name, 4 as day_of_week
    union select 0 as exec_count, 'Friday'    as day_name, 5 as day_of_week
    union select 0 as exec_count, 'Saterday'  as day_name, 6 as day_of_week
    order by day_of_week;    