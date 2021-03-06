------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : IconTypeSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'
------------------------------------------------------------------------
-- Name: dbo.IconTypeSave
-- Date: 2018-04-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[IconTypeSave]
	@iconTypeId int
	,@order int
	,@titles dbo.LanguageTitleCollection readonly
	,@roleIds dbo.IdCollection readonly
as
begin
	begin transaction;
	begin try
		declare @output dbo.IdCollection;

		if (@iconTypeId = 0)
		begin
			insert dbo.IconTypes
			output inserted.IconTypeId into @output
			default values;
		end
		else
		begin
			insert @output
			select @iconTypeId;
		end;

		merge dbo.IconTypeTitles as t
		using (
			select r.Id as IconTypeId
				,mt.Language
				,mt.Title
			from @titles as mt
			inner join @output as r on 1 = 1
		) as s on s.IconTypeId = t.IconTypeId
			and s.Language = t.Language
		when not matched by target then
			insert (IconTypeId, Language, Title)
			values (s.IconTypeId, s.Language, s.Title)
		when matched then
			update
			set t.Title = s.Title;

		exec dbo.IconTypeGet @output;

		commit transaction;
	end try
	begin catch
		rollback transaction;
		throw;
	end catch;
end;
' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IconTypeSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[IconTypeSave] 
GRANT EXECUTE ON [dbo].[IconTypeSave] TO [rCMDB] 
GO

------------------------------------- 
