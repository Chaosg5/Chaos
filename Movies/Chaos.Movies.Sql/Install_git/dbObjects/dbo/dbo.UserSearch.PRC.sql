------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : UserSearch 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.UserSearch
-- Date: 2018-04-15
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[UserSearch]
	@searchText nvarchar(100)
	,@searchLimit int
	,@requireExactMatch bit
as
begin
	if (@searchLimit <= 0)
	begin
		set @searchLimit = 10;
	end;

	if (@requireExactMatch = 1 and @searchLimit = 1)
	begin
		select top 1 c.UserId
		from dbo.Users as c
		where Username = @searchText;
	end
	else if (@requireExactMatch = 1)
	begin
		select top(@searchLimit) c.UserId
		from dbo.Users as c
		where Name = @searchText
			or Username = @searchText;
	end
	else
	begin
		select top(@searchLimit) c.UserId
		from dbo.Users as c
		where Name like ''%'' + @searchText + ''%''
			or Username like ''%'' + @searchText + ''%'';
	end;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserSearch]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[UserSearch] 
GRANT EXECUTE ON [dbo].[UserSearch] TO [rCMDB] 
GO

------------------------------------- 
