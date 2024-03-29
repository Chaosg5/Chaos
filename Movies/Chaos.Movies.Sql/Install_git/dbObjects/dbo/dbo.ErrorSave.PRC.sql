------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : ErrorSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.ErrorSave
-- Date: 2018-04-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[ErrorSave]
	@errorId int
	,@userId int
	,@time datetime2(7)
	,@type nvarchar(100)
	,@source nvarchar(255)
	,@targetSite nvarchar(255)
	,@message nvarchar(255)
	,@stackTrace nvarchar(max) 
as
begin
		declare @output dbo.IdCollection;

		if (@errorId = 0)
		begin
			insert dbo.Errors
			output inserted.ErrorId into @output
			select @userId
				,@time
				,@type
				,@source
				,@targetSite
				,@message
				,@stackTrace;
		end
		else
		begin		
			update dbo.Errors
			set UserId = @userId
				,"Time" = @time
				,"Type" = @type
				,"Source" = @source
				,TargetSite = @targetSite
				,"Message" = @message
				,StackTrace = @stackTrace
			output inserted.ErrorId into @output
			where ErrorId = @errorId;
		end;

		exec dbo.ErrorGet @output;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ErrorSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[ErrorSave] 
GRANT EXECUTE ON [dbo].[ErrorSave] TO [rCMDB] 
GO

------------------------------------- 
