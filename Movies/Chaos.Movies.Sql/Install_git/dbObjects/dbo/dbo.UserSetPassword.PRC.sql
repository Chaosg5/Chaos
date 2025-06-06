------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : UserSetPassword 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.UserSetPassword
-- Date: 2018-04-15
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[UserSetPassword]
	@userId int
	,@oldUserName nvarchar(50)
	,@oldPassword nvarchar(255)
	,@userName nvarchar(50)
	,@password nvarchar(255)
as
begin
		declare @output dbo.IdCollection;

		if not exists (
			select 1
			from dbo.Users
			where UserId = @userId
				and UserName = @oldUserName
				and "Password" = @oldPassword
				and cast("Password" as varbinary(4000)) = cast(@oldPassword as varbinary(4000))
		) and not exists (
			select 1
			from dbo.Users
			where UserId = @userId
				and UserName = @oldUserName
				and "Password" = ''''
		)
		begin
			;throw 50000, ''The specified username and/or password is not valid.'', 1;
		end;

		update dbo.Users
			set UserName = @userName
				,"Password" = @password
			output inserted.UserId into @output
			where UserId = @userId;

		exec dbo.UserGet @output;
end;' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserSetPassword]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[UserSetPassword] 
GRANT EXECUTE ON [dbo].[UserSetPassword] TO [rCMDB] 
GO

------------------------------------- 
