------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.MovieSave
-- Date: 2017-07-07
-- Release: 1.0
-- Summary:
--   * Saves a movie.
-- Param:
--   @xxx
--     * xxx
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[MovieSave]
	@movieId int
	,@movieTypeId int
	,@year date
	,@titles dbo.LanguageTitleCollection readonly
	,@externalLookups dbo.ExternalLookupIdCollection readonly
	,@externalRatings dbo.ExternalRatingCollection readonly
as
begin
	begin transaction;
	begin try
		declare @output table(MovieId int);

		if (@movieId = 0)
		begin
			insert dbo.Movies
			output inserted.MovieId into @output
			select @movieTypeId
				,@year;
		end
		else
		begin
			update dbo.Movies
			set MovieTypeId = @movieTypeId
				,"Year" = @year
			output inserted.MovieId into @output
			where MovieId = @movieId;
		end;

		merge dbo.MovieTitles as t
		using (
			select r.MovieId
				,mt.Language
				,mt.Title
			from @titles as mt
			inner join @output as r on 1 = 1
		) as s on s.MovieId = t.MovieId
			and s.Language = t.Language
		when not matched by target then
			insert (MovieId, Language, Title)
			values (s.MovieId, s.Language, s.Title)
		when matched then
			update
			set t.Title = s.Title;

		merge dbo.MovieExternalLookup as t
		using (
			select r.MovieId
				,x.ExternalSourceId
				,x.ExternalId
			from @externalLookups as x
			inner join @output as r on 1 = 1
		) as s on s.MovieId = t.MovieId
			and s.ExternalSourceId = t.ExternalSourceId
		when not matched by target then
			insert (MovieId, ExternalSourceId, ExternalId)
			values (s.MovieId, s.ExternalSourceId, s.ExternalId)
		when matched then
			update
			set t.ExternalId = s.ExternalId;

		--select m.MovieId
		--	,
		--from dbo.Movies as m
		--inner join @output as r on r.MovieId = m.MovieId;

		commit transaction;
	end try
	begin catch
		rollback transaction;
		throw;
	end catch;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[MovieSave] 
GRANT EXECUTE ON [dbo].[MovieSave] TO [rCMDB] 
GO

------------------------------------- 
