------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : ExternalSourceSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.ExternalSourceSave
-- Date: 2018-04-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[ExternalSourceSave]
	@externalSourceId int
	,@name nvarchar(50)
	,@baseAddress nvarchar(250)
	,@peopleAddress nvarchar(150)
	,@characterAddress nvarchar(150)
	,@genreAddress nvarchar(150)
	,@episodeAddress nvarchar(150)
as
begin
		declare @output dbo.IdCollection;

		if (@externalSourceId = 0)
		begin
			insert dbo.ExternalSources
			output inserted.ExternalSourceId into @output
			select @name
				,@baseAddress
				,@peopleAddress
				,@characterAddress
				,@genreAddress
				,@episodeAddress;
		end
		else
		begin
			update dbo.ExternalSources
			set Name = @name
				,BaseAddress = @baseAddress
				,PeopleAddress = @peopleAddress
				,CharacterAddress = @characterAddress
				,GenreAddress = @genreAddress
				,EpisodeAddress = @episodeAddress
			output inserted.ExternalSourceId into @output
			where ExternalSourceId = @externalSourceId;
		end;

		exec dbo.ExternalSourceGet @output;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExternalSourceSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[ExternalSourceSave] 
GRANT EXECUTE ON [dbo].[ExternalSourceSave] TO [rCMDB] 
GO

------------------------------------- 
