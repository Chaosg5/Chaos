------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : CharacterSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'
------------------------------------------------------------------------
-- Name: dbo.CharacterSave
-- Date: 2018-04-12
-- Release: 1.0
-- Summary:
--   * Saves a character.
-- Param:
--   @characterId int
--     * The id of the character.
--   @name nvarchar(100)
--     * The name of the character.
--   @icons dbo.IdOrderCollection
--     * The id of the icons of the character.
--   @externalLookups dbo.ExternalLookupIdCollection
--     * The id of the character in external sources.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[CharacterSave]
	@characterId int
	,@name nvarchar(100)
	,@icons dbo.IdOrderCollection readonly
	,@externalLookups dbo.ExternalLookupIdCollection readonly
as
begin
	begin transaction;
	begin try
		declare @output dbo.IdCollection;

		if (@characterId = 0)
		begin
			insert dbo.Characters
			output inserted.CharacterId into @output
			select @name;
		end
		else
		begin
			update dbo.Characters
			set Name = @name
			output inserted.CharacterId into @output
			where CharacterId = @characterId;
		end;

		merge dbo.IconsInCharacters as t
		using (
			select r.Id as CharacterId
				,x.Id as IconId
				,x."Order"
			from @icons as x
			inner join @output as r on 1 = 1
		) as s on s.CharacterId = t.CharacterId
			and s.IconId = t.IconId
		when not matched by target then
			insert (CharacterId, IconId, "Order")
			values (s.CharacterId, s.IconId, s."Order")
		when matched then
			update
			set "Order" = s."Order"
		when not matched by source then
			delete;

		merge dbo.CharacterExternalLookup as t
		using (
			select r.Id as CharacterId
				,x.ExternalSourceId
				,x.ExternalId
			from @externalLookups as x
			inner join @output as r on 1 = 1
		) as s on s.CharacterId = t.CharacterId
			and s.ExternalSourceId = t.ExternalSourceId
		when not matched by target then
			insert (CharacterId, ExternalSourceId, ExternalId)
			values (s.CharacterId, s.ExternalSourceId, s.ExternalId)
		when matched then
			update
			set t.ExternalId = s.ExternalId
		when not matched by source then
			delete;

		exec dbo.CharacterGet @output;

		commit transaction;
	end try
	begin catch
		rollback transaction;
		throw;
	end catch;
end;
' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CharacterSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[CharacterSave] 
GRANT EXECUTE ON [dbo].[CharacterSave] TO [rCMDB] 
GO

------------------------------------- 
