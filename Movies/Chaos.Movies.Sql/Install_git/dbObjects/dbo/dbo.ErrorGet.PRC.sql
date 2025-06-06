------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : ErrorGet 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.ErrorGet
-- Date: 2018-03-19
-- Release: 1.0
-- Summary:
--   * Gets the specified error.
-- Param:
--   @errorIds dbo.IdCollection
--     * The list of ids of the errors to get.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[ErrorGet]
	@errorIds dbo.IdCollection readonly
as
begin
	select e.ErrorId
		,e.UserId
		,e.Time
		,e.Type
		,e.Source
		,e.TargetSite
		,e.Message
		,e.StackTrace
	from dbo.Errors e
	inner join @errorIds as i on i.Id = e.ErrorId
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ErrorGet]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[ErrorGet] 
GRANT EXECUTE ON [dbo].[ErrorGet] TO [rCMDB] 
GO

------------------------------------- 
