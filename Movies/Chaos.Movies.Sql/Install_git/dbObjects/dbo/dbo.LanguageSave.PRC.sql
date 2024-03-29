------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : LanguageSave 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.LanguageSave
-- Date: 2017-07-07
-- Release: 1.0
-- Summary:
--   * Saves a language and it''s titles.
-- Param:
--   @languageId nvarchar(8)
--     * The id of the language.
--   @titles dbo.LanguagesTitlesCollection
--     * The titles of the language.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[LanguageSave]
	@language nvarchar(8)
	,@titles dbo.LanguagesTitlesCollection readonly
as
begin
	if (select count(1) from @titles) = 0
	begin
		throw 50000, ''Invalid titles count. A language can''''t be saved witout any titles.'', 1;
	end;

	if not exists (select 1 from @titles where InLanguage = ''en-US'')
	begin
		throw 50000, ''Missing default title. A language can''''t be saved witout a title in the default language (en-US).'', 1;
	end;

	begin transaction;
	begin try
		declare @output table(Language nvarchar(8));

		if not exists (
			select 1
			from dbo.Languages
			where Language = @language
			)
		begin
			insert dbo.Languages
			output inserted.Language into @output
			default values;
		end
		else
		begin		
			insert @output
			select @language
		end;

		merge dbo.LanguageTitles as t
		using (
			select r.Language
				,lt.InLanguage
				,lt.Title
			from @titles as lt
			inner join @output as r on 1 = 1
		) as s on s.Language = t.Language
			and s.InLanguage = t.InLanguage
		when not matched by target then
			insert (Language, InLanguage, Title)
			values (s.Language, s.InLanguage, s.Title)
		when matched then
			update
			set t.Title = s.Title;

		select l.Language
			,t.InLanguage
			,t.Title
		from dbo.Languages as l
		inner join dbo.LanguageTitles as t on t.Language = l.Language
		inner join @output as o on o.Language = l.Language;
		commit transaction;
	end try
	begin catch
		rollback transaction;
		throw;
	end catch;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LanguageSave]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[LanguageSave] 
GRANT EXECUTE ON [dbo].[LanguageSave] TO [rCMDB] 
GO

------------------------------------- 
