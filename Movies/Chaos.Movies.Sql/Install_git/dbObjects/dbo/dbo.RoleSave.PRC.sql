------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : RoleSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'
------------------------------------------------------------------------
-- Name: dbo.RoleSave
-- Date: 2018-05-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[RoleSave]
	@roleId int
	,@titles dbo.LanguageTitleCollection readonly
as
begin
	begin transaction;
	begin try
		declare @output dbo.IdCollection;

		if (@roleId = 0)
		begin
			insert dbo.Roles
			output inserted.RoleId into @output
			default values;
		end
		else
		begin
			insert @output
			select @roleId;
		end;

		merge dbo.RoleTitles as t
		using (
			select r.Id as RoleId
				,mt.Language
				,mt.Title
			from @titles as mt
			inner join @output as r on 1 = 1
		) as s on s.RoleId = t.RoleId
			and s.Language = t.Language
		when not matched by target then
			insert (RoleId, Language, Title)
			values (s.RoleId, s.Language, s.Title)
		when matched then
			update
			set t.Title = s.Title;

		exec dbo.RoleGet @output;

		commit transaction;
	end try
	begin catch
		rollback transaction;
		throw;
	end catch;
end;
' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RoleSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[RoleSave] 
GRANT EXECUTE ON [dbo].[RoleSave] TO [rCMDB] 
GO

------------------------------------- 
