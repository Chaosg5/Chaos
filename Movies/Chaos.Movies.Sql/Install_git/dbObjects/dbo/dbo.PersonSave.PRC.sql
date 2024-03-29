------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : PersonSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'
------------------------------------------------------------------------
-- Name: dbo.PersonSave
-- Date: 2018-04-12
-- Release: 1.0
-- Summary:
--   * Saves a person.
-- Param:
--   @personId int
--     * The id of the person.
--   @name nvarchar(100)
--     * The name of the person.
--   @icons dbo.IdOrderCollection
--     * The id of the icons of the person.
--   @externalLookups dbo.ExternalLookupIdCollection
--     * The id of the person in external sources.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[PersonSave]
	@personId int
	,@name nvarchar(100)
	,@icons dbo.IdOrderCollection readonly
	,@externalLookups dbo.ExternalLookupIdCollection readonly
as
begin
	begin transaction;
	begin try
		declare @output dbo.IdCollection;

		if (@personId = 0)
		begin
			insert dbo.People
			output inserted.PersonId into @output
			select @name;
		end
		else
		begin
			update dbo.People
			set Name = @name
			output inserted.PersonId into @output
			where PersonId = @personId;
		end;

		merge dbo.IconsInPeople as t
		using (
			select r.Id as PersonId
				,x.Id as IconId
				,x."Order"
			from @icons as x
			inner join @output as r on 1 = 1
		) as s on s.PersonId = t.PersonId
			and s.IconId = t.IconId
		when not matched by target then
			insert (PersonId, IconId, "Order")
			values (s.PersonId, s.IconId, s."Order")
		when matched then
			update
			set "Order" = s."Order"
		when not matched by source then
			delete;

		merge dbo.PersonExternalLookup as t
		using (
			select r.Id as PersonId
				,x.ExternalSourceId
				,x.ExternalId
			from @externalLookups as x
			inner join @output as r on 1 = 1
		) as s on s.PersonId = t.PersonId
			and s.ExternalSourceId = t.ExternalSourceId
		when not matched by target then
			insert (PersonId, ExternalSourceId, ExternalId)
			values (s.PersonId, s.ExternalSourceId, s.ExternalId)
		when matched then
			update
			set t.ExternalId = s.ExternalId
		when not matched by source then
			delete;

		exec dbo.PersonGet @output;

		commit transaction;
	end try
	begin catch
		rollback transaction;
		throw;
	end catch;
end;
' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PersonSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[PersonSave] 
GRANT EXECUTE ON [dbo].[PersonSave] TO [rCMDB] 
GO

------------------------------------- 
