------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : CharacterGet 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.CharacterGet
-- Date: 2018-04-23
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[CharacterGet]
	@characterIds dbo.IdCollection readonly
as
begin
	;with cteRatings as (
		select i.CharacterId
			,avg(r.Rating) as Rating
		from dbo.UserCharacterInMovieRatings as r
		inner join dbo.CharactersInMovies as i on i.CharacterInMovieId = r.CharacterInMovieId
		group by i.CharacterId
	)
	select c.CharacterId
		,c.Name
		,isnull(t.Rating, 0) as TotalRating
		,0 UserId
		,0 UserRating
	from dbo.Characters as c
	inner join @characterIds as r on r.Id = c.CharacterId
	left join cteRatings as t on t.CharacterId = c.CharacterId;

	select c.CharacterId
		,x.IconId
	from dbo.Characters as c
	inner join dbo.IconsInCharacters as x on x.CharacterId = c.CharacterId
	inner join @characterIds as r on r.Id = c.CharacterId
	order by x."Order" asc;

	select x.ExternalSourceId
		,x.ExternalId
	from dbo.Characters as c
	inner join dbo.CharacterExternalLookup as x on x.CharacterId = c.CharacterId
	inner join @characterIds as r on r.Id = c.CharacterId;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CharacterGet]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[CharacterGet] 
GRANT EXECUTE ON [dbo].[CharacterGet] TO [rCMDB] 
GO

------------------------------------- 
