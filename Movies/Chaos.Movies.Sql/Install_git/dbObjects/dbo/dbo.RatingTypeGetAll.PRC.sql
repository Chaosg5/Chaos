------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : RatingTypeGetAll 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.RatingTypesGetAll
-- Date: 2018-04-12
-- Release: 1.0
-- Summary:
--   * Gets all rating types.
-- Param:
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[RatingTypeGetAll]
as
begin
	select t.RatingTypeId
	from dbo.RatingTypes t

	select t.RatingTypeId
		,ti.Language
		,ti.Title
	from dbo.RatingTypes t
	inner join dbo.RatingTypeTitles as ti on ti.RatingTypeId = t.RatingTypeId
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RatingTypeGetAll]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[RatingTypeGetAll] 
GRANT EXECUTE ON [dbo].[RatingTypeGetAll] TO [rCMDB] 
GO

------------------------------------- 
