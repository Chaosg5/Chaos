------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : PersonGet 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.PersonGet
-- Date: 2018-04-12
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[PersonGet]
	@personIds dbo.IdCollection readonly
as
begin
	select p.PersonId
		,p.Name
	from dbo.People as p
	inner join @personIds as r on r.Id = p.PersonId;

	select p.PersonId
		,x.IconId
	from dbo.People as p
	inner join dbo.IconsInPeople as x on x.PersonId = p.PersonId
	inner join @personIds as r on r.Id = p.PersonId
	order by x."Order" asc;

	select x.ExternalSourceId
		,x.ExternalId
	from dbo.People as p
	inner join dbo.PersonExternalLookup as x on x.PersonId = p.PersonId
	inner join @personIds as r on r.Id = p.PersonId;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PersonGet]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[PersonGet] 
GRANT EXECUTE ON [dbo].[PersonGet] TO [rCMDB] 
GO

------------------------------------- 
