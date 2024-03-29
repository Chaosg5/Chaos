------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : UserSessionGet 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.UserSessionGet
-- Date: 2018-03-19
-- Release: 1.0
-- Summary:
--   * Gets the specified user session(s).
-- Param:
--   @userSessionIds dbo.GuidCollection
--     * The list of ids of the user session(s) to get.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[UserSessionGet]
	@userSessionIds dbo.GuidCollection readonly
as
begin
	select s.UserSessionId
		,s.ClientIp
		,UserId
		,ActiveFrom
		,ActiveTo
	from dbo.UserSessions s
	inner join @userSessionIds as i on i.Id = s.UserSessionId
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserSessionGet]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[UserSessionGet] 
GRANT EXECUTE ON [dbo].[UserSessionGet] TO [rCMDB] 
GO

------------------------------------- 
