------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : EpisodeSearch 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.EpisodeSearch
-- Date: 2018-04-12
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[EpisodeSearch]
	@searchText nvarchar(100)
	,@searchLimit int
	,@requireExactMatch bit
as
begin
if (@searchLimit <= 0)
	begin
		set @searchLimit = 10;
	end;

	if (@requireExactMatch = 1)
	begin
		select distinct top(@searchLimit) c.EpisodeId
		from dbo.Episodes as c
		inner join dbo.EpisodeTitles as ti on ti.EpisodeId = c.EpisodeId
		where ti.Title = @searchText;
	end
	else
	begin
		select distinct top(@searchLimit) c.EpisodeId
		from dbo.Episodes as c
		inner join dbo.EpisodeTitles as ti on ti.EpisodeId = c.EpisodeId
		where ti.Title like ''%'' + @searchText + ''%'';
	end;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EpisodeSearch]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[EpisodeSearch] 
GRANT EXECUTE ON [dbo].[EpisodeSearch] TO [rCMDB] 
GO

------------------------------------- 
