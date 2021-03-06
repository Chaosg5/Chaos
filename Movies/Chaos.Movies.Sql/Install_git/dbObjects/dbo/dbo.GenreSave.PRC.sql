------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : GenreSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'
------------------------------------------------------------------------
-- Name: dbo.GenreSave
-- Date: 2018-04-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[GenreSave]
	@genreId int
	,@titles dbo.LanguageTitleCollection readonly
	,@externalLookups dbo.ExternalLookupIdCollection readonly
as
begin
	begin transaction;
	begin try
		declare @output dbo.IdCollection;

		if (@genreId = 0)
		begin
			insert dbo.Genres
			output inserted.GenreId into @output
			default values;
		end
		else
		begin
			insert @output
			select @genreId;
		end;

		merge dbo.GenreTitles as t
		using (
			select r.Id as GenreId
				,mt.Language
				,mt.Title
			from @titles as mt
			inner join @output as r on 1 = 1
		) as s on s.GenreId = t.GenreId
			and s.Language = t.Language
		when not matched by target then
			insert (GenreId, Language, Title)
			values (s.GenreId, s.Language, s.Title)
		when matched then
			update
			set t.Title = s.Title;

		merge dbo.GenreExternalLookup as t
		using (
			select r.Id
				,x.ExternalSourceId
				,x.ExternalId
			from @externalLookups as x
			inner join @output as r on 1 = 1
		) as s on s.Id = t.GenreId
			and s.ExternalSourceId = t.ExternalSourceId
		when not matched by target then
			insert (GenreId, ExternalSourceId, ExternalId)
			values (s.Id, s.ExternalSourceId, s.ExternalId)
		when matched then
			update
			set t.ExternalId = s.ExternalId
		when not matched by source then
			delete;

		exec dbo.GenreGet @output;

		commit transaction;
	end try
	begin catch
		rollback transaction;
		throw;
	end catch;
end;
' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GenreSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[GenreSave] 
GRANT EXECUTE ON [dbo].[GenreSave] TO [rCMDB] 
GO

------------------------------------- 
