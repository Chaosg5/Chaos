------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : RatingSystemSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.RatingSystemSave
-- Date: 2018-04-11
-- Release: 1.0
-- Summary:
--   * Saves a rating system and it''s values.
-- Param:
--   @ratingSystemId int
--     * The id of the movie type.
--   @titles dbo.LanguageTitleCollection
--     * The titles of the movie type.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[RatingSystemSave]
	@ratingSystemId int
	,@titles dbo.LanguageTitleCollection readonly
	,@values dbo.RatingSystemValueCollection readonly
as
begin
	if exists (
		select 1
		from (
			select ParentRatingTypeId
				,sum(v.Weight) as Weight
			from @values as v
			inner join dbo.RatingTypes as rt on rt.RatingTypeId = v.RatingTypeId
			group by ParentRatingTypeId
		) as x where x.Weight <> 100
	)
	begin
		;throw 50000, ''Invalid total sum. All rating system values for each parent group needs to sum to 100.'', 1;
	end;

	begin transaction;
	begin try
		declare @output dbo.IdCollection

		if (@ratingSystemId = 0)
		begin
			insert dbo.RatingSystems
			output inserted.RatingSystemId into @output
			default values;
		end
		else
		begin
			insert @output
			select @ratingSystemId
		end;

		merge dbo.RatingSystemTitles as t
		using (
			select r.Id as RatingSystemId
				,mt.Language
				,mt.Title
			from @titles as mt
			inner join @output as r on 1 = 1
		) as s on s.RatingSystemId = t.RatingSystemId
			and s.Language = t.Language
		when not matched by target then
			insert (RatingSystemId, Language, Title)
			values (s.RatingSystemId, s.Language, s.Title)
		when matched then
			update
			set t.Title = s.Title;

		merge dbo.RatingSystemValues as t
		using (
			select r.Id as RatingSystemId
				,v.RatingTypeId
				,v.Weight
			from @values as v
			inner join @output as r on 1 = 1
		) as s on s.RatingSystemId = t.RatingSystemId
			and s.RatingTypeId = t.RatingTypeId
		when not matched by target then
			insert (RatingSystemId, RatingTypeId, Weight)
			values (s.RatingSystemId, s.RatingTypeId, s.Weight)
		when matched then
			update
			set t.Weight = s.Weight;

		exec dbo.RatingSystemGet @output;
		commit transaction;
	end try
	begin catch
		rollback transaction;
		throw;
	end catch;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RatingSystemSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[RatingSystemSave] 
GRANT EXECUTE ON [dbo].[RatingSystemSave] TO [rCMDB] 
GO

------------------------------------- 
