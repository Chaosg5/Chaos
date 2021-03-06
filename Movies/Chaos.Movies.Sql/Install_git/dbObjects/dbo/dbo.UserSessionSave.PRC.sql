------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : UserSessionSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.UserSessionSave
-- Date: 2018-03-19
-- Release: 1.0
-- Summary:
--   * Saves a user session.
-- Param:
--   @xxx int
--     * xxx.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[UserSessionSave]
	@userSessionId uniqueidentifier = null
	,@clientIp nvarchar(50)
	,@userId int = null
	,@activeFrom datetime
	,@activeTo datetime
	,@username nvarchar(50) = null
	,@password nvarchar(250) = null
as
begin
		declare @output dbo.GuidCollection;
		declare @existingUserSessionId uniqueidentifier;

		select @existingUserSessionId = UserSessionId
			from dbo.UserSessions as s
			inner join dbo.Users as u on u.UserId = s.UserId
			where u.UserName = @username
				and u.Password = @password
				and s.ActiveTo > getdate();

		if (@existingUserSessionId is not null)
		begin
			insert @output
			select @existingUserSessionId

			update dbo.UserSessions
			set ActiveTo = @activeTo
		end
		else if (@userSessionId is null)
		begin
			select @userId = UserId
			from dbo.Users
			where UserName = @username
				and "Password" = @password;

			if (@userId is null)
			begin
				;throw 50000, ''The specified username and/or password is not valid.'', 1;
			end;

			insert dbo.UserSessions
			output inserted.UserSessionId into @output
			select newid()
				,@clientIp
				,@userId
				,@activeFrom
				,@activeTo
		end
		else
		begin
			update dbo.UserSessions
			set ClientIp = @clientIp
				,UserId = @userId
				,ActiveTo = @activeTo
			output inserted.UserSessionId into @output
			where UserSessionId = @userSessionId;
		end;

		exec dbo.UserSessionGet @output;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserSessionSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[UserSessionSave] 
GRANT EXECUTE ON [dbo].[UserSessionSave] TO [rCMDB] 
GO

------------------------------------- 
