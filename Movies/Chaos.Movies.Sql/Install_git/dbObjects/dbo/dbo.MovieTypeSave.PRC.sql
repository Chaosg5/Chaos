------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieTypeSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.MovieTypeSave
-- Date: 2018-04-11
-- Release: 1.0
-- Summary:
--   * Saves a movie.
-- Param:
--   @movieTypeId int
--     * The id of the movie type.
--   @titles dbo.LanguageTitleCollection
--     * The titles of the movie type.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[MovieTypeSave]
	@movieTypeId int
	,@titles dbo.LanguageTitleCollection readonly
as
begin
	begin transaction;
	begin try
		declare @output dbo.IdCollection

		if (@movieTypeId = 0)
		begin
			insert dbo.MovieTypes
			output inserted.MovieTypeId into @output
			default values;
		end
		else
		begin
			insert @output
			select @movieTypeId
		end;

		merge dbo.MovieTypeTitles as t
		using (
			select r.Id as MovieTypeId
				,mt.Language
				,mt.Title
			from @titles as mt
			inner join @output as r on 1 = 1
		) as s on s.MovieTypeId = t.MovieTypeId
			and s.Language = t.Language
		when not matched by target then
			insert (MovieTypeId, Language, Title)
			values (s.MovieTypeId, s.Language, s.Title)
		when matched then
			update
			set t.Title = s.Title;

		exec dbo.MovieTypeGet @output;
		commit transaction;
	end try
	begin catch
		rollback transaction;
		throw;
	end catch;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieTypeSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[MovieTypeSave] 
GRANT EXECUTE ON [dbo].[MovieTypeSave] TO [rCMDB] 
GO

------------------------------------- 
