------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : GenreGet 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.GenreGet
-- Date: 2018-04-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstag. All rights reserveg.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[GenreGet]
	@genreIds dbo.IdCollection readonly
as
begin
	select g.GenreId
	from dbo.Genres as g
	inner join @genreIds as i on i.Id = g.GenreId;

	select g.GenreId
		,ti.Language
		,ti.Title
	from dbo.Genres g
	inner join dbo.GenreTitles as ti on ti.GenreId = g.GenreId
	inner join @genreIds as i on i.Id = g.GenreId;

	select x.ExternalSourceId
		,x.ExternalId
	from dbo.Genres as g
	inner join dbo.GenreExternalLookup as x on x.GenreId = g.GenreId
	inner join @genreIds as r on r.Id = g.GenreId;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GenreGet]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[GenreGet] 
GRANT EXECUTE ON [dbo].[GenreGet] TO [rCMDB] 
GO

------------------------------------- 
