------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : LanguageGet 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.LanguageGet
-- Date: 2017-07-06
-- Release: 1.0
-- Summary:
--   * Gets all language titles the specified language, or en-US if missing in the native language.
-- Param:
--   @language varchar(8)
--     * The language to get the titles in.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[LanguageGet]
	@language varchar(8)
as
begin
	select l.Language
		,isnull(Specific.InLanguage, t.InLanguage) as InLanguage
		,isnull(Specific.Title, t.Title) as Title
	from dbo.Languages as l
	left join dbo.LanguageTitles as Specific on Specific.Language = l.Language
		and Specific.InLanguage = @language
	left join dbo.LanguageTitles as t on t.Language = l.Language
		and t.InLanguage = ''en-US''
	order by l.Language;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LanguageGet]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[LanguageGet] 
GRANT EXECUTE ON [dbo].[LanguageGet] TO [rCMDB] 
GO

------------------------------------- 
