------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : UserGet 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.UserGet
-- Date: 2018-04-12
-- Release: 1.0
-- Summary:
--   * Gets the specified user.
-- Param:
--   @userIds dbo.IdCollection
--     * The list of ids of the users to get.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[UserGet]
	@userIds dbo.IdCollection readonly
as
begin
	select e.UserId
		,e.Username
		,e.Name
		,e.Email
	from dbo.Users e
	inner join @userIds as i on i.Id = e.UserId
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserGet]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[UserGet] 
GRANT EXECUTE ON [dbo].[UserGet] TO [rCMDB] 
GO

------------------------------------- 
