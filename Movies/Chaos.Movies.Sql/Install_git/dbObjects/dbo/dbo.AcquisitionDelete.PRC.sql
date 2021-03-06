------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : AcquisitionDelete 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.AcquisitionDelete
-- Date: 2016-10-21
-- Release: 1.0
-- Summary:
--   * Deletes the specified acquisition.
-- Param:
--   @acquisitionId int
--     * The id of the acquisition.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[AcquisitionDelete]
	@acquisitionId int
as
begin
	delete
	from dbo.Acquisitions
	where AcquisitionId = @acquisitionId
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AcquisitionDelete]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[AcquisitionDelete] 
GRANT EXECUTE ON [dbo].[AcquisitionDelete] TO [rCMDB] 
GO

------------------------------------- 
