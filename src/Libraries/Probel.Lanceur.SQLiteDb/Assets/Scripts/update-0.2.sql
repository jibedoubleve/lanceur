-------------------------------------------------------------------------------
-------------------------------------------------------------------------------
--- DATA RECONCILIATION
-------------------------------------------------------------------------------
update alias set arguments   = null where arguments   = '';
update alias set notes       = null where notes       = '';
update alias set working_dir = null where working_dir = '';

drop view if exists data_doubloons;
drop view if exists stat_history;
drop view if exists stat_execution_count;
 
-------------------------------------------------------------------------------
-------------------------------------------------------------------------------
--- TRIGGERS
-------------------------------------------------------------------------------
drop trigger if exists on_alias_update;
create trigger on_alias_update update on alias
begin
    update alias set arguments   = null where arguments   = '' and id = old.id;
    update alias set notes       = null where notes       = '' and id = old.id;
    update alias set working_dir = null where working_dir = '' and id = old.id;
end;