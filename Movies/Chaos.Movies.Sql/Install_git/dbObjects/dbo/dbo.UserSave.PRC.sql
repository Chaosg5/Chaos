------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : UserSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.UserSave
-- Date: 2018-04-15
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[UserSave]
	@userId int
	,@userName nvarchar(50)
	,@name nvarchar(255)
	,@email nvarchar(255)
as
begin
		declare @output dbo.IdCollection;

		if (@userId = 0)
		begin
			insert dbo.Users
			output inserted.UserId into @output
			select @userName
				,''''
				,@name
				,@email;
		end
		else
		begin		
			update dbo.Users
			set Name = @name
				,Email = @email
			output inserted.UserId into @output
			where UserId = @userId;
		end;

		exec dbo.UserGet @output;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[UserSave] 
GRANT EXECUTE ON [dbo].[UserSave] TO [rCMDB] 
GO

------------------------------------- 
