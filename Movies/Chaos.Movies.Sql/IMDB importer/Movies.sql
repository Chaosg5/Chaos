use CMDB;
set nocount on;

if not exists (
	select *
	from tempdb..sysobjects
	where (name like '#movies%')
		and type = 'U'
	)
begin
	create table #movies (
		MovieId int
		,MovieTypeId int
		,ImdbId nvarchar(50) collate Finnish_Swedish_CI_AS
		,MovieType nvarchar(50) collate Finnish_Swedish_CI_AS
		,DefaultTitle nvarchar(100) collate Finnish_Swedish_CI_AS
		,OriginalTitle nvarchar(100) collate Finnish_Swedish_CI_AS
		,StartYear int
		,EndYear int
		,RunTime int
		,Genre1 nvarchar(50) collate Finnish_Swedish_CI_AS
		,Genre2 nvarchar(50) collate Finnish_Swedish_CI_AS
		,Genre3 nvarchar(50) collate Finnish_Swedish_CI_AS
	);
end;

if not exists (
	select *
	from tempdb..sysobjects
	where (name like '#titles%')
		and type = 'U'
	)
begin
	create table #titles (
		MovieId int
		,language nvarchar(8) collate Finnish_Swedish_CI_AS
		,ImdbId nvarchar(50) collate Finnish_Swedish_CI_AS
		,Title nvarchar(100) collate Finnish_Swedish_CI_AS
		,Country nvarchar(50) collate Finnish_Swedish_CI_AS
		,Lang nvarchar(50) collate Finnish_Swedish_CI_AS
		,LangType nvarchar(50) collate Finnish_Swedish_CI_AS
	);
end;

if not exists (
	select *
	from tempdb..sysobjects
	where (name like '#ratings%')
		and type = 'U'
	)
begin
	create table #ratings (
		MovieId int
		,ImdbId nvarchar(50) collate Finnish_Swedish_CI_AS
		,Rating float
		,RatingCount int
	);
end;

if not exists (
	select *
	from tempdb..sysobjects
	where (name like '#people%')
		and type = 'U'
	)
begin
	create table #people (
		PersonId int
		,ImdbId nvarchar(50) collate Finnish_Swedish_CI_AS
		,name nvarchar(100) collate Finnish_Swedish_CI_AS
		,BirthYear date
		,DeathYear date
	);
end;

/*

create table #movies (
	MovieId int
	,MovieTypeId int
	,ImdbId nvarchar(50) collate Finnish_Swedish_CI_AS
	,MovieType nvarchar(50) collate Finnish_Swedish_CI_AS
	,DefaultTitle nvarchar(100) collate Finnish_Swedish_CI_AS
	,OriginalTitle nvarchar(100) collate Finnish_Swedish_CI_AS
	,StartYear int
	,EndYear int
	,RunTime int
	,Genre1 nvarchar(50) collate Finnish_Swedish_CI_AS
	,Genre2 nvarchar(50) collate Finnish_Swedish_CI_AS
	,Genre3 nvarchar(50) collate Finnish_Swedish_CI_AS
);

create table #titles (
	MovieId int
	,language nvarchar(5) collate Finnish_Swedish_CI_AS
	,ImdbId nvarchar(50) collate Finnish_Swedish_CI_AS
	,Title nvarchar(100) collate Finnish_Swedish_CI_AS
	,Country nvarchar(50) collate Finnish_Swedish_CI_AS
	,Lang nvarchar(50) collate Finnish_Swedish_CI_AS
	,LangType nvarchar(50) collate Finnish_Swedish_CI_AS
);

create table #ratings (
	MovieId int
	,ImdbId nvarchar(50) collate Finnish_Swedish_CI_AS
	,Rating float
	,RatingCount int
);

create table #people (
	PersonId int
	,ImdbId nvarchar(50) collate Finnish_Swedish_CI_AS
	,name nvarchar(100) collate Finnish_Swedish_CI_AS
	,BirthYear date
	,DeathYear date
);

*/

/*

drop table #movies;
drop table #titles;
drop table #ratings;
drop table #people;

*/

declare @imdbExernalSourceId int = (select top 1 ExternalSourceId from dbo.ExternalSources as s where s.Name = 'IMDB');
declare @imdbId nvarchar(50);

/*

delete from #movies;
delete from #titles;
delete from #ratings;
delete from #people;

*/

/*

select max(ImdbId)
from #titles


select count(1)
from #titles

select count(distinct ImdbId)
from #titles

*/

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

if exists (
	select 1
	from dbo.MovieTypeTitles as t
	full join (
		select distinct MovieType
		from #movies
	) as m on m.MovieType = t.Title
		and Language = 'default'
	where t.MovieTypeId is null
)
begin
	select *
	from dbo.MovieTypeTitles as t
	full join (
		select distinct MovieType
		from #movies
	) as m on m.MovieType = t.Title
		and Language = 'default'
	where t.MovieTypeId is null;

	;throw 50000, 'Invalid Movie Type found', 1;
end;

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

declare @movieTitles dbo.LanguageTitleCollection;
declare @externalLookups dbo.ExternalLookupIdCollection;
declare @externalRatings dbo.ExternalRatingCollection;

declare @movieTypeId int;
declare @year int;
declare @endYear int;
declare @runTime int;

update m
set m.MovieId = l.MovieId
	,m.MovieTypeId = t.MovieTypeId
from #movies m
left join dbo.MovieTypeTitles as t on t.Title = m.MovieType
	and t.Language = 'default'
left join dbo.MovieExternalLookup as l on l.ExternalId = m.ImdbId
	and l.ExternalSourceId = @imdbExernalSourceId;

insert dbo.Movies
select m.MovieTypeId
	,m.StartYear
	,isnull(m.EndYear, m.StartYear)
	,m.RunTime
	,m.ImdbId
from #movies as m
left join dbo.Movies as e on e.ImdbId = m.ImdbId
where m.MovieId is null
	and e.MovieId is null;

insert dbo.MovieExternalLookup
select e.MovieId, @imdbExernalSourceId, m.ImdbId
from #movies as m
inner join dbo.Movies as e on e.ImdbId = m.ImdbId
left join dbo.MovieExternalLookup as l on l.MovieId = e.MovieId
	and l.ExternalSourceId = @imdbExernalSourceId
where l.MovieId is null;

update m
set m.MovieId = l.MovieId
from #movies as m
left join dbo.MovieExternalLookup as l on l.ExternalId = m.ImdbId
	and l.ExternalSourceId = @imdbExernalSourceId
where m.MovieId is null;

if exists (
	select 1
	from #movies as m
	where m.MovieId is null
)
begin
	select top 1 *
	from #movies as m
	where m.MovieId is null;

	;throw 50000, 'Invalid Movie found', 1;
end

insert dbo.MovieTitles
select m.MovieId
	,'default'
	,m.DefaultTitle
from #movies as m
left join dbo.MovieTitles as t on t.MovieId = m.MovieId
	and t.Language = 'default'
where t.MovieId is null;

insert dbo.MovieTitles
select m.MovieId
	,'original'
	,m.OriginalTitle
from #movies as m
left join dbo.MovieTitles as t on t.MovieId = m.MovieId
	and t.Language = 'original'
where t.MovieId is null;

-----------
-- Genre --
-----------

if exists (
	select 1
	from #movies as m
	where m.MovieId is null
)
begin
	select top 1 *
	from #movies as m
	where m.MovieId is null;

	;throw 50000, 'Invalid Genre found', 1;
end;

insert dbo.MoviesInGenres
select m.MovieId
	,g.GenreId
from #movies as m
inner join dbo.GenreTitles as g on g.Title = m.Genre1
	and g.Language = 'default'
left join dbo.MoviesInGenres as e on e.GenreId = g.GenreId
	and e.MovieId = m.MovieId
where e.MovieId is null;

insert dbo.MoviesInGenres
select m.MovieId
	,g.GenreId
from #movies as m
inner join dbo.GenreTitles as g on g.Title = m.Genre2
	and g.Language = 'default'
left join dbo.MoviesInGenres as e on e.GenreId = g.GenreId
	and e.MovieId = m.MovieId
where e.MovieId is null;

insert dbo.MoviesInGenres
select m.MovieId
	,g.GenreId
from #movies as m
inner join dbo.GenreTitles as g on g.Title = m.Genre3
	and g.Language = 'default'
left join dbo.MoviesInGenres as e on e.GenreId = g.GenreId
	and e.MovieId = m.MovieId
where e.MovieId is null;

------------
-- Titles --
------------

update t
set t.MovieId = l.MovieId
	,t.Language = g.Language
from #titles as t
left join dbo.MovieExternalLookup as l on l.ExternalId = t.ImdbId
	and l.ExternalSourceId = @imdbExernalSourceId
left join dbo.Languages as g on right(g.Language, 2) = t.Country
	and (t.Lang is null or t.Lang = left(g.Language, 2));

if exists (
	select 1
	from #titles as t
	where t.MovieId is null
)
begin
	select top 1 *
	from #titles as t
	where t.MovieId is null;

	;throw 50000, 'Invalid Title found', 1;

	/*
	delete
	from #titles
	where MovieId is null;
	*/
end;

	/*
-- US
;with cte as (
	select *
		,row_number() over(
			partition by t.ImdbId
			order by case
				when t.Country = 'US' then 0
				when t.Lang = 'en' then 1
				when t.LangType = 'Original' then 2
				else 3
			end
		) as RowNumber
	from #titles as t
)
select *
from cte
where RowNumber = 1
*/
-- Langs
;with cte as (
	select t.*
		,row_number() over(
			partition by t.ImdbId, t.Language
			order by case
				when o.ImdbId is not null then 0
				else 1
			end
		) as RowNumber
	from #titles as t
	left join #titles as o on o.ImdbId = t.ImdbId
		and o.LangType = 'Original'
		and o.Title = t.Title
)
insert dbo.MovieTitles
select c.MovieId
	,c.Language
	,c.Title
from cte as c
left join dbo.MovieTitles as m on m.Language = c.Language
	and m.MovieId = c.MovieId
where m.MovieId is null
	and c.Language is not null
	and c.RowNumber = 1;


/*

select *
from (
	select distinct t.ImdbId
	from #titles as t
) as x
cross apply (
	select top 1 t.ImdbId
		,t.Title
	from #titles as t
	left join #titles as o on o.ImdbId = t.ImdbId
		and o.LangType = 'Original'
		and o.Title = t.Title
	where t.ImdbId = x.ImdbId
		and (
			t.Country = 'US'
			or t.LangType = 'Original'
		)
	order by t.ImdbId
		,case when o.ImdbId is not null then 0 else 1 end
		,case when t.Country = 'US' then 0 else 1 end
) as u
order by x.ImdbId

*/

-------------
-- Ratings --
-------------

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

------------
-- People --
------------

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

/*


*/


/*
select * from dbo.Languages as l
inner join (
select right(l.Language, 2) as Lang, count(1) as Antal
from dbo.Languages as l
group by right(l.Language, 2)
) as x on x.Antal > 1
and x.Lang = right(l.Language, 2)
*/



/*

select count(1)
from dbo.movietitles as p


select top 1 *
from dbo.movietitles as p
order by movieId desc

select max(ImdbId)
from dbo.movietitles as p

select *
from dbo.movies as m
left join dbo.movietitles as t on t.MovieId = m.MovieId
where m.MovieId < 999997
	and t.MovieId is null


declare @imdbExernalSourceId int = (select top 1 ExternalSourceId from dbo.ExternalSources as s where s.Name = 'IMDB');

select *
from dbo.People as p
left join dbo.PersonExternalLookup as l on l.PersonId = p.PersonId
	and l.ExternalSourceId = @imdbExernalSourceId
where p.ImdbId is null
	or p.ImdbId <> l.ExternalId

*/