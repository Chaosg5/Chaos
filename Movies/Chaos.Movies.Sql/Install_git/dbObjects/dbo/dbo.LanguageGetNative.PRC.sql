------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : LanguageGetNative 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.LanguageGetNative
-- Date: 2017-07-06
-- Release: 1.0
-- Summary:
--   * Gets all language titles in their native language, or en-US if missing in the native language.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[LanguageGetNative]
as
begin
	;with Natives as (
		select t.*
		from dbo.Languages as l
		inner join dbo.LanguageTitles as t on t.Language = l.Language
			and t.InLanguage = l.Language
	)
	select l.Language
		,isnull(Natives.InLanguage, t.InLanguage) as InLanguage
		,isnull(Natives.Title, t.Title) as Title
	from dbo.Languages as l
	left join Natives on Natives.Language = l.Language
	left join dbo.LanguageTitles as t on t.Language = l.Language
		and t.InLanguage = ''en-US''
	order by l.Language;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LanguageGetNative]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[LanguageGetNative] 
GRANT EXECUTE ON [dbo].[LanguageGetNative] TO [rCMDB] 
GO

------------------------------------- 
