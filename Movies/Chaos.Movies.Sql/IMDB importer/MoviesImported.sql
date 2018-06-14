use CMDB;
set nocount on;

declare @imdbExernalSourceId int = (select top 1 ExternalSourceId from dbo.ExternalSources as s where s.Name = 'IMDB');
declare @imdbId nvarchar(50);

/**********************************************/
/**********************************************/
/**********************************************/
/*************                    *************/
/*************        DATA        *************/
/*************                    *************/
/**********************************************/
/**********************************************/
/**********************************************/



/**********************************************/
/**********************************************/
/**********************************************/
/*************                    *************/
/*************     VALIDATION     *************/
/*************                    *************/
/**********************************************/
/**********************************************/
/**********************************************/
if (@imdbExernalSourceId is null)
begin
	;throw 50000, 'ExternalSource for IMDB not found.', 1;
end;

-- MovieType
if exists (
	select 1
	from dbo.MovieTypeTitles as t
	full join (
		select distinct titleType
		from dbo.ImportMovies
	) as m on m.titleType = t.Title
		and Language = 'default'
	where t.MovieTypeId is null
)
begin
	select *
	from dbo.MovieTypeTitles as t
	full join (
		select distinct titleType
		from dbo.ImportMovies
	) as m on m.titleType = t.Title
		and Language = 'default'
	where t.MovieTypeId is null

	;throw 50000, 'Invalid Movie Type found', 1;
end;

-- ToDo:
-- Genre
/*
if exists (
		select 1
		from dbo.GenreTitles t
		full join (
			select distinct Genre1 as Genre
			from #movies
			union
			select distinct Genre2
			from #movies
			union
			select distinct Genre3
			from #movies
		) as m on m.Genre = t.Title
			and Language = 'default'
		where m.Genre <> ''
			and t.GenreId is null
	)
begin
	select *
		from dbo.GenreTitles t
		full join (
			select distinct Genre1 as Genre
			from #movies
			union
			select distinct Genre2
			from #movies
			union
			select distinct Genre3
			from #movies
		) as m on m.Genre = t.Title
			and Language = 'default'
		where m.Genre <> ''
			and t.GenreId is null;

	;throw 50000, 'Invalid Genre found', 1;
end;
*/

-- Movies
if exists (
	select 1
	from dbo.ImportMovies as m
	right join dbo.Movies as e on e.ImdbId = m.tconst
	where m.tconst is null
)
begin
	select top 1 *
	from dbo.ImportMovies as m
	right join dbo.Movies as e on e.ImdbId = m.tconst
	where m.tconst is null;

	;throw 50000, 'Invalid Movie found', 1;
end

-- Titles
if exists (
	select 1
	from dbo.ImportLanguageTitles as t
	inner join dbo.ImportMovies as m on m.tconst = t.titleId
	left join dbo.Movies as e on e.ImdbId = t.titleId
	where e.MovieId is null
)
begin
	select top 1 *
	from dbo.ImportLanguageTitles as t
	inner join dbo.ImportMovies as m on m.tconst = t.titleId
	left join dbo.Movies as e on e.ImdbId = t.titleId
	where e.MovieId is null;

	;throw 50000, 'Invalid Title found', 1;
end;

/**********************************************/
/**********************************************/
/**********************************************/
/*************                    *************/
/*************        MERGE       *************/
/*************                    *************/
/**********************************************/
/**********************************************/
/**********************************************/

------------
-- Movies --
------------

insert dbo.Movies
select t.MovieTypeId
	,m.startYear
	,case m.endYear when '\N' then m.startYear else m.startYear end
	,case m.runTimeMinutes when '\N' then 0 else m.runTimeMinutes end
	,m.tconst
from dbo.ImportMovies as m
left join dbo.Movies as e on e.ImdbId = m.tconst
left join dbo.MovieTypeTitles as t on t.Title = m.titleType
	and t.Language = 'default'
where e.MovieId is null;

-- declare @imdbExernalSourceId int = (select top 1 ExternalSourceId from dbo.ExternalSources as s where s.Name = 'IMDB');
insert dbo.MovieExternalLookup
select e.MovieId
	,@imdbExernalSourceId
	,m.tconst
from dbo.ImportMovies as m
left join dbo.Movies as e on e.ImdbId = m.tconst
left join dbo.MovieExternalLookup as l on l.MovieId = e.MovieId
	and l.ExternalSourceId = @imdbExernalSourceId
where l.MovieId is null;

insert dbo.MovieTitles
select e.MovieId
	,'default'
	,left(m.primaryTitle, 100)
from dbo.ImportMovies as m
left join dbo.Movies as e on e.ImdbId = m.tconst
left join dbo.MovieTitles as t on t.MovieId = e.MovieId
	and t.Language = 'default'
where t.MovieId is null;

insert dbo.MovieTitles
select e.MovieId
	,'original'
	,left(m.originalTitle, 100)
from dbo.ImportMovies as m
left join dbo.Movies as e on e.ImdbId = m.tconst
left join dbo.MovieTitles as t on t.MovieId = e.MovieId
	and t.Language = 'original'
where t.MovieId is null;

-----------
-- Genre --
-----------

insert dbo.MoviesInGenres
select e.MovieId
	,g.GenreId
from dbo.ImportMovies as m
cross apply string_split(m.genres, ',') as s
left join dbo.Movies as e on e.ImdbId = m.tconst
left join dbo.GenreTitles as g on g.Title = s.value
	and g.Language = 'default'
left join dbo.MoviesInGenres as f on f.GenreId = g.GenreId
	and f.MovieId = e.MovieId
where m.genres <> '\N'
	and f.MovieId is null;

------------
-- Titles --
------------

;with cte as (
	select t.titleId
		,left(t.title, 100) as title
		,t.region
		,t.language
		,row_number() over(
			partition by t.titleId, t.region
			order by case
				when o.titleId is not null then 0
				else 1
			end
		) as RowNumber
	from dbo.ImportLanguageTitles as t
	left join dbo.ImportLanguageTitles as o on o.titleId = t.titleId
		and o.types = 'Original'
		and o.title = t.title
	--where t.titleId = 'tt6394300'
)
insert dbo.MovieTitles
select e.MovieId
	,g.Language
	,max(c.Title)
from cte as c
inner join dbo.Movies as e on e.ImdbId = c.titleId -- There titles for invalid movies in the source
left join dbo.Languages as g on right(g.Language, 2) = c.region
	and (c.language = '\N' or c.language = left(g.Language, 2))
left join dbo.MovieTitles as m on m.Language = g.Language
	and m.MovieId = e.MovieId
where m.MovieId is null
	and g.Language is not null
	and c.RowNumber = 1
group by e.MovieId
	,g.Language;

-------------
-- Ratings --
-------------
-- ToDo:
/*
update r
set r.MovieId = l.MovieId
from #ratings as r
left join dbo.MovieExternalLookup as l on l.ExternalId = r.ImdbId
	and l.ExternalSourceId = @imdbExernalSourceId;

if exists (
	select 1
	from #ratings as r
	where r.MovieId is null
)
begin
	select top 1 *
	from #ratings as r
	where r.MovieId is null;

	;throw 50000, 'Invalid Rating found', 1;
end;

insert dbo.MovieExternalRatings
select r.MovieId
	,@imdbExernalSourceId
	,r.Rating
	,r.RatingCount
from #ratings as r
left join dbo.MovieExternalRatings as m on m.MovieId = r.MovieId
	and m.ExternalSourceId = @imdbExernalSourceId
where m.MovieId is null;

update m
set m.ExternalRating = r.Rating
	,m.RatingCount = r.RatingCount
from #ratings as r
inner join dbo.MovieExternalRatings as m on m.MovieId = r.MovieId
	and m.ExternalSourceId = @imdbExernalSourceId
where m.ExternalRating <> r.Rating
	or m.RatingCount <> r.RatingCount;
*/
------------
-- People --
------------
-- ToDo:
/*
declare @birthDate date;
declare @deathDate date;
declare @name nvarchar(100);
declare @icons dbo.IdOrderCollection;

update p
set p.PersonId = l.PersonId
from #people as p
left join dbo.PersonExternalLookup as l on l.ExternalId = p.ImdbId
	and l.ExternalSourceId = @imdbExernalSourceId;

insert dbo.People
select p.Name
	,p.BirthYear
	,p.DeathYear
	,p.ImdbId
from #people as p
left join dbo.People as e on e.ImdbId = p.ImdbId
where p.PersonId is null
	and e.PersonId is null;

insert dbo.PersonExternalLookup
select e.PersonId, @imdbExernalSourceId, p.ImdbId
from #people as p
inner join dbo.People as e on e.ImdbId = p.ImdbId
left join dbo.PersonExternalLookup as l on l.PersonId = e.PersonId
	and l.ExternalSourceId = @imdbExernalSourceId
where l.PersonId is null;

update p
set p.PersonId = l.PersonId
from #people as p
left join dbo.PersonExternalLookup as l on l.ExternalId = p.ImdbId
	and l.ExternalSourceId = @imdbExernalSourceId
where p.PersonId is null;

if exists (
	select 1
	from #people as p
	where p.PersonId is null
)
begin
	select top 1 *
	from #people as p
	where p.PersonId is null;

	;throw 50000, 'Invalid Person found', 1;
end;

update e
set e.DeathDate = p.DeathYear
from #people as p
left join dbo.People as e on e.PersonId = p.PersonId
where e.DeathDate is null
	and p.DeathYear is not null;
*/

------------
-- Actors --
------------

/*

CREATE NONCLUSTERED INDEX ImportActorsInMoviesPersonIndex
ON [dbo].[ImportActorsInMovies] ([nconst])
INCLUDE ([tconst],[category])

CREATE NONCLUSTERED INDEX ImportActorsInMoviesCharactersIndex
ON [dbo].[ImportActorsInMovies] ([category])
INCLUDE ([characters])

*/

-- People

insert dbo.PeopleInMovies
select p.PersonId
	,m.MovieId
	,rd.RoleInDepartmentId
from dbo.People as p
left join dbo.ImportActorsInMovies as a on a.nconst = p.ImdbId
inner join dbo.Movies as m on m.ImdbId = a.tconst
left join dbo.RoleTitles as rt on rt.Language = 'default'
	and rt.Title = a.category
left join dbo.RolesInDepartments as rd on rd.RoleId = rt.RoleId
left join dbo.PeopleInMovies as pm on pm.PersonId = p.PersonId
	and pm.MovieId = m.MovieId
	and pm.RoleInDepartmentId = rd.RoleInDepartmentId
where pm.PersonInMovieId is null
group by p.PersonId
	,m.MovieId
	,rd.RoleInDepartmentId

-- Characters
declare @olddelim nvarchar(32) = N'","', @newdelim nchar(1) = NCHAR(9999);
insert dbo.Characters
select x.CharacterName
from (
	select distinct left(replace(replace(s.value, '"]', ''), '["', ''), 100) as CharacterName
	from dbo.ImportActorsInMovies as a
	cross apply string_split(replace(a.characters, @olddelim, @newdelim), @newdelim) as s
	where a.category in ('actor', 'actress','self')
		and a.characters <> N'\N'
) as x
left join dbo.Characters as c on c.name = x.CharacterName
where c.CharacterId is null

-- People as Characters
--declare @olddelim nvarchar(32) = N'","', @newdelim nchar(1) = nchar(9999);
insert dbo.ActorsAsCharacters
select pm.PersonInMovieId
	,c.CharacterId
from (
	select a.nconst
		,a.tconst
		,a.category
		,left(replace(replace(s.value, '"]', ''), '["', ''), 100) as CharacterName
	from dbo.ImportActorsInMovies as a
	cross apply string_split(replace(a.characters, @olddelim, @newdelim), @newdelim) as s
	where a.category in ('actor', 'actress','self')
		and a.characters <> N'\N'
) as a
left join dbo.Characters as c on c.name = a.CharacterName
inner join dbo.People as p on p.ImdbId = a.nconst
inner join dbo.Movies as m on m.ImdbId = a.tconst
left join dbo.PeopleInMovies as pm on pm.PersonId = p.PersonId
	and pm.MovieId = m.MovieId
left join dbo.ActorsAsCharacters as ac on ac.PersonInMovieId = pm.PersonInMovieId
	and ac.CharacterId = c.CharacterId
where ac.ActorAsCharacterId is null
group by pm.PersonInMovieId
	,c.CharacterId;

--------------
-- Episodes --
--------------

-------------
-- Writers --
-------------