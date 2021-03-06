------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : DepartmentSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'
------------------------------------------------------------------------
-- Name: dbo.DepartmentSave
-- Date: 2018-05-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[DepartmentSave]
	@departmentId int
	,@order int = null
	,@titles dbo.LanguageTitleCollection readonly
	,@roles dbo.IdCollection readonly
as
begin
	begin transaction;
	begin try
		declare @output dbo.IdCollection;

		if (@departmentId = 0)
		begin
			if (@order is null)
			begin
				select @order = isnull(max("Order"), -1) + 1
				from dbo.Departments
			end;

			insert dbo.Departments
			output inserted.DepartmentId into @output
			select @order;
		end
		else if (@order is not null)
		begin
			update dbo.Departments
			set "Order" = @order
			output inserted.DepartmentId into @output
			where DepartmentId = @departmentId;
		end;

		merge dbo.DepartmentTitles as t
		using (
			select r.Id as DepartmentId
				,mt.Language
				,mt.Title
			from @titles as mt
			inner join @output as r on 1 = 1
		) as s on s.DepartmentId = t.DepartmentId
			and s.Language = t.Language
		when not matched by target then
			insert (DepartmentId, Language, Title)
			values (s.DepartmentId, s.Language, s.Title)
		when matched then
			update
			set t.Title = s.Title;

		merge dbo.RolesInDepartments as t
		using (
			select r.Id as DepartmentId
				,dr.Id as RoleId
			from @roles as dr
			inner join @output as r on 1 = 1
		) as s on s.DepartmentId = t.DepartmentId
			and s.RoleId = t.RoleId
		when not matched by target then
			insert (DepartmentId, RoleId)
			values (s.DepartmentId, s.RoleId)
		when not matched by source then
			delete;

		exec dbo.DepartmentGet @output;

		commit transaction;
	end try
	begin catch
		rollback transaction;
		throw;
	end catch;
end;
' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DepartmentSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[DepartmentSave] 
GRANT EXECUTE ON [dbo].[DepartmentSave] TO [rCMDB] 
GO

------------------------------------- 
