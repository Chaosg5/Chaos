------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : VendorSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.VendorSave
-- Date: 2016-09-10
-- Release: 1.0
-- Summary:
--   * Saves a vendor.
-- Param:
--   @vendorId int
--     * The id of the vendor.
--   @name nvarchar(100)
--     * The name of the vendor.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[VendorSave]
	@vendorId int
	,@name nvarchar(100)
as
begin
	declare @output table(VendorId int);

	if (@vendorId = 0)
	begin
		insert dbo.Vendors
		output inserted.VendorId into @output
		select @name;
	end
	else
	begin
		update dbo.Vendors
		set Name = @name
		output inserted.VendorId into @output
		where VendorId = @vendorId;
	end;

	select v.VendorId
		,v.Name
	from dbo.Vendors as v
	inner join @output as o on o.VendorId = v.VendorId;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VendorSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[VendorSave] 
GRANT EXECUTE ON [dbo].[VendorSave] TO [rCMDB] 
GO

------------------------------------- 
