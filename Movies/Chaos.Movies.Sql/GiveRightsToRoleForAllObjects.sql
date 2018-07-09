/*************************************************/
/* This Script requires SQLCMD mode to be active */
/*               Query > SQLCMD Mode             */
/*************************************************/
-- Rename database names if needed.

:setvar ContentDatabase "CMDB"

-- Missing databases will cause errors, comment away the section below if the database should not be run

--------------------------------------------------------
--------------------------------------------------------
--------------------------------------------------------
--------------------------------------------------------
--------------------------------------------------------

declare @results table(Script nvarchar(1000), DatabaseName nvarchar(100), ScriptOrder int);

-------------------------------
-------------------------------
-----     DDM_Content     -----
-------------------------------
-------------------------------

use $(ContentDatabase);
insert @results
select 'use ' + db_name() + ';'
	,db_name()
	,1
union
select 'grant execute on type :: ' + db_name() + '.' + s.name + '.' + t.name + ' to rCMDB;'
	,db_name()
	,2
from sys.types as t
inner join sys.schemas as s on s.schema_id = t.schema_id
where is_user_defined = 1
union
select 'grant execute on ' + db_name() + '.' + s.name + '.' + o.name + ' to rCMDB;'
	,db_name()
	,2
from sys.objects as o
inner join sys.schemas as s on s.schema_id = o.schema_id
where type in ('FN', 'P')
union
select 'grant select on ' + db_name() + '.' + s.name + '.' + o.name + ' to rCMDB;'
	,db_name()
	,2
from sys.objects as o
inner join sys.schemas as s on s.schema_id = o.schema_id
where type = 'TF';

select * from @results