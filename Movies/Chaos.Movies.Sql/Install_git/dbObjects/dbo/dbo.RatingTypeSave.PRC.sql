------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : RatingTypeSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.RatingTypeSave
-- Date: 2018-04-12
-- Release: 1.0
-- Summary:
--   * Saves a rating.
-- Param:
--   @ratingTypeId int
--     * The id of the rating type.
--   @parentRatingTypeId int
--     * The id of the rating type.
--   @titles dbo.LanguageTitleCollection
--     * The titles of the rating type.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[RatingTypeSave]
	@ratingTypeId int
	,@parentRatingTypeId int = null
	,@titles dbo.LanguageDescriptionCollection readonly
as
begin
	if (select count(1) from @titles) = 0
	begin
		;throw 50000, ''Invalid titles count. A rating type can''''t be saved without any titles.'', 1;
	end;

	if not exists (select 1 from @titles where Language = ''en-US'')
	begin
		;throw 50000, ''Missing default title. A rating type can''''t be saved without a title in the default language (en-US).'', 1;
	end;

	begin transaction;
	begin try
		declare @output dbo.IdCollection

		if (@ratingTypeId = 0)
		begin
			insert dbo.RatingTypes
			output inserted.RatingTypeId into @output
			select @parentRatingTypeId;
		end
		else
		begin		
			insert @output
			select @ratingTypeId
		end;

		merge dbo.RatingTypeTitles as t
		using (
			select r.Id
				,mt.Language
				,mt.Title
				,mt.Description
			from @titles as mt
			inner join @output as r on 1 = 1
		) as s on s.Id = t.RatingTypeId
			and s.Language = t.Language
		when not matched by target then
			insert (RatingTypeId, Language, Title, Description)
			values (s.Id, s.Language, s.Title, s.Description)
		when matched then
			update
			set t.Title = s.Title
				,t.Description = s.Description;

		exec dbo.RatingTypeGet @output;
		commit transaction;
	end try
	begin catch
		rollback transaction;
		throw;
	end catch;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RatingTypeSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[RatingTypeSave] 
GRANT EXECUTE ON [dbo].[RatingTypeSave] TO [rCMDB] 
GO

------------------------------------- 
