------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MoviesOwnedByUserSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.MoviesOwnedByUserSave
-- Date: 2016-09-10
-- Release: 1.0
-- Summary:
--   * Saves a movie as owned by a user.
-- Param:
--   @movieId int
--     * The id of the movie.
--   @userId int
--     * The id of the user.
--   @dvd bit
--     * If the user owns the movie on DVD.
--   @bd bit
--     * If the user owns the movie on Blu-Ray.
--   @digital bit
--     * If the user owns the movie in a digital format.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[MoviesOwnedByUserSave]
	@movieId int
	,@userId int
	,@dvd bit = 0
	,@bd bit = 0
	,@digital bit = 0
as
begin
	declare @output table(MoviesOwnedByUserId int);

	if not exists (
		select 1
		from dbo.MoviesOwnedByUser
		where MovieId = @movieId
			and UserId = @userId
	)
	begin
		insert dbo.MoviesOwnedByUser
		output inserted.MoviesOwnedByUserId into @output
		select @movieId
			,@userId
			,@dvd
			,@bd
			,@digital;
	end
	else
	begin
		update dbo.MoviesOwnedByUser
		set DVD = @dvd
			,BD = @bd
			,Digital = @digital
		output inserted.MoviesOwnedByUserId into @output
		where MovieId = @movieId
			and UserId = @userId;
	end;

	select m.MovieId
		,m.UserId
		,m.DVD
		,m.BD
		,m.Digital
	from dbo.MoviesOwnedByUser as m
	inner join @output as o on o.MoviesOwnedByUserId = m.MoviesOwnedByUserId;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MoviesOwnedByUserSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[MoviesOwnedByUserSave] 
GRANT EXECUTE ON [dbo].[MoviesOwnedByUserSave] TO [rCMDB] 
GO

------------------------------------- 
