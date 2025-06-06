------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : ExternalSourceGet 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.ExternalSourceGet
-- Date: 2018-03-19
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[ExternalSourceGet]
	@externalSourceIds dbo.IdCollection readonly
as
begin
	select e.ExternalSourceId
		,e.Name
		,e.BaseAddress
		,e.PeopleAddress
		,e.CharacterAddress
		,e.GenreAddress
		,e.EpisodeAddress
	from dbo.ExternalSources e
	inner join @externalSourceIds as i on i.Id = e.ExternalSourceId
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExternalSourceGet]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[ExternalSourceGet] 
GRANT EXECUTE ON [dbo].[ExternalSourceGet] TO [rCMDB] 
GO

------------------------------------- 
