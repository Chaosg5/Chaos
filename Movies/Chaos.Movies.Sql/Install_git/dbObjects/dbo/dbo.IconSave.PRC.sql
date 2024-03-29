------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : IconSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.IconSave
-- Date: 2018-04-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[IconSave]
	@iconId int
	,@iconTypeId int
	,@iconUrl nvarchar(500)
	,@dataSize int
	,@data varbinary(max)
as
begin
		declare @output dbo.IdCollection;

		if (@iconId = 0)
		begin
			insert dbo.Icons
			output inserted.IconId into @output
			select @iconTypeId
				,@iconUrl
				,@dataSize
				,@data;
		end
		else
		begin		
			update dbo.Icons
			set IconTypeId = @iconTypeId
				,IconUrl = @iconUrl
				,DataSize = @dataSize
				,"Data" = @data
			output inserted.IconId into @output
			where IconId = @iconId;
		end;

		exec dbo.IconGet @output;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IconSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[IconSave] 
GRANT EXECUTE ON [dbo].[IconSave] TO [rCMDB] 
GO

------------------------------------- 
