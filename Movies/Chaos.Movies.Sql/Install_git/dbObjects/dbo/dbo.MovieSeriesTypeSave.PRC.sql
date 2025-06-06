------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieSeriesTypeSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.MovieSeriesTypeSave
-- Date: 2018-04-11
-- Release: 1.0
-- Summary:
--   * Saves a movie.
-- Param:
--   @movieSeriesTypeId int
--     * The id of the movie series type.
--   @titles dbo.LanguageTitleCollection
--     * The titles of the movie series type.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[MovieSeriesTypeSave]
	@movieSeriesTypeId int
	,@titles dbo.LanguageTitleCollection readonly
as
begin
	begin transaction;
	begin try
		declare @output table(MovieSeriesTypeId int);

		if (@movieSeriesTypeId = 0)
		begin
			insert dbo.MovieSeriesTypes
			output inserted.MovieSeriesTypeId into @output
			default values;
		end
		else
		begin		
			insert @output
			select @movieSeriesTypeId
		end;

		merge dbo.MovieSeriesTypeTitles as t
		using (
			select r.MovieSeriesTypeId
				,mt.Language
				,mt.Title
			from @titles as mt
			inner join @output as r on 1 = 1
		) as s on s.MovieSeriesTypeId = t.MovieSeriesTypeId
			and s.Language = t.Language
		when not matched by target then
			insert (MovieSeriesTypeId, Language, Title)
			values (s.MovieSeriesTypeId, s.Language, s.Title)
		when matched then
			update
			set t.Title = s.Title;

		select m.MovieSeriesTypeId
			,t.Language
			,t.Title
		from dbo.MovieSeriesTypes as m
		inner join dbo.MovieSeriesTypeTitles as t on t.MovieSeriesTypeId = m.MovieSeriesTypeId
		inner join @output as o on o.MovieSeriesTypeId = m.MovieSeriesTypeId;

		commit transaction;
	end try
	begin catch
		rollback transaction;
		throw;
	end catch;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieSeriesTypeSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[MovieSeriesTypeSave] 
GRANT EXECUTE ON [dbo].[MovieSeriesTypeSave] TO [rCMDB] 
GO

------------------------------------- 
