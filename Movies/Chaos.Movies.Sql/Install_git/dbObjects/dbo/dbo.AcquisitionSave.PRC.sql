------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : AcquisitionSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.AcquisitionSave
-- Date: 2016-10-20
-- Release: 1.0
-- Summary:
--   * Saves an acquisition.
-- Param:
--   @acquisitionId int
--     * The id of the acquisition.
--   @movieId int
--     * The id of the movie which was acquired.
--   @userId int
--     * The id of the user that acquired the movie.
--   @acquiredAt date
--     * The date of the acquisition.
--   @dateUncertain bit
--     * Whether or not the date of the acquisition is uncertain since the exact date is not known.
--   @vendorId int
--     * The id of the vendor from which the movie was acquired.
--   @price int
--     * The price payed for the movie.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[AcquisitionSave]
	@acquisitionId int
	,@movieId int
	,@userId int
	,@acquiredAt date = null
	,@dateUncertain bit = 0
	,@vendorId int = null
	,@price float = 0
as
begin
	declare @outputIds table(AcquisitionId int);
	declare @moviesOwnedByUserId int = (
		select MoviesOwnedByUserId
		from dbo.MoviesOwnedByUser
		where UserId = @userId
			and MovieId = @movieId
	);

	if (@moviesOwnedByUserId is null)
	begin
		return;
	end;

	if (@acquisitionId = 0)
	begin
		insert dbo.Acquisitions
		output inserted.AcquisitionId into @outputIds
		select @moviesOwnedByUserId
			,@acquiredAt
			,@dateUncertain
			,@vendorId
			,@price;
	end
	else
	begin
		if not exists (
			select 1
			from dbo.Acquisitions
			where AcquisitionId = @acquisitionId
				and MoviesOwnedByUserId = @moviesOwnedByUserId
		)
		begin
			return;
		end;

		update dbo.Acquisitions
		set AcquiredAt = @acquiredAt
			,DateUncertain = @dateUncertain
			,VendorId = @vendorId
			,Price = @price
		output inserted.AcquisitionId into @outputIds
		where AcquisitionId = @acquisitionId;
	end;

	select a.AcquisitionId
		,a.MoviesOwnedByUserId
		,a.AcquiredAt
		,a.DateUncertain
		,a.VendorId
		,a.Price
	from dbo.Acquisitions as a
	inner join @outputIds as r on r.AcquisitionId = a.AcquisitionId;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AcquisitionSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[AcquisitionSave] 
GRANT EXECUTE ON [dbo].[AcquisitionSave] TO [rCMDB] 
GO

------------------------------------- 
