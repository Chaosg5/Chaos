------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : ExternalSourceGetAll 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.ExternalSourceGetAll
-- Date: 2018-04-15
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[ExternalSourceGetAll]
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
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExternalSourceGetAll]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[ExternalSourceGetAll] 
GRANT EXECUTE ON [dbo].[ExternalSourceGetAll] TO [rCMDB] 
GO

------------------------------------- 
