use CMDB;

;with cteTableGuests as (
	select tg.Position
		,tg.GuestId
		,tg.TableId
		,g.Name
	from wed.TableGuests as tg
	inner join wed.Guests as g on g.GuestId = tg.GuestId
),
cteTables as (
	select TableId
		,Name
		,1 as Type
	from wed.Tables
	union all
	select TableId
		,Name
		,2 as Type
	from wed.Tables
)
select t1.Name
	--,t1.TableId
	,g1.Name
	,g2.Name
	,t2.Name
	--,t2.TableId
	,g3.Name
	,g4.Name
from cteTables as t1
left join wed.Tables as t2 on t2.TableId = t1.TableId + 1
left join cteTableGuests as g1 on g1.TableId = t1.TableId
	and ((t1.Type = 1 and g1.Position = 1) or (t1.Type = 2 and g1.Position = 3))
left join cteTableGuests as g2 on g2.TableId = t1.TableId
	and ((t1.Type = 1 and g2.Position = 2) or (t1.Type = 2 and g2.Position = 4))
left join cteTableGuests as g3 on g3.TableId = t2.TableId
	and ((t1.Type = 2 and g3.Position = 1) or (t1.Type = 1 and g3.Position = 3))
left join cteTableGuests as g4 on g4.TableId = t2.TableId
	and ((t1.Type = 2 and g4.Position = 2) or (t1.Type = 1 and g4.Position = 4))
where t1.TableId % 2 = 0
order by t1.TableId

/*
insert wed.TableGuests select 59, 15, 1
insert wed.TableGuests select 60, 15, 2
insert wed.TableGuests select 61, 15, 3
insert wed.TableGuests select 58, 15, 4
*/


SELECT g.GuestId
	,tg.TableId
	,tg.Position
	--,g.Sex
	,g.Name
	,wg.*
	,a.*
	,g.Name
	,wg.Information
  FROM CMDB.wed.WeddingGuests as wg
  full join CMDB.wed.Guests as g  on g.GuestId = wg.GuestId
  left join cmdb.wed.GuestAddresses as ga on ga.GuestId = g.GuestId
  left join CMDB.wed.TableGuests as tg on tg.GuestId = g.GuestId
  left join CMDB.wed.Addresses as a on a.AddressId = ga.AddressId
where isnull(wg.Dinner, 0) in (1,2)
	--and g.GuestId in (31, 52, 53, 55, 59, 60, 61, 62, 85, 92, 98, 86, 56, 57, 58) -- kommer inte
	and g.GuestId not in (68, 65, 19, 23, 102, 103) -- barn
	--and g.GuestId not in (0)
--and tg.TableId is null
order by g.GuestId
--order by sex desc


select *
--delete
from CMDB.wed.GuestAddresses
where GuestId = 31

select *
from CMDB.wed.WeddingGuests
-- update CMDB.wed.WeddingGuests set Reception = 2, Dinner = 2
where GuestId in (54)

/*
insert @titles (Language, Title, Description) values ('sv-SE', N'Svenska', N'')
insert @titles (Language, Title, Description) values ('en-US', N'English', N'')
*/


/*





QuestionHints
QuestionLocks

Hitta ett namn på en kung i en text.




Caroline
--------
1)
Vilket av föjande är en Sadelgjordspännare? => https://originellt.se/handverktyg/%C3%96vriga%20verktyg
Som skulle kunna användas för denna fåtölj Torparen av formgivaren Gustav Axel Berg.

2)
Vad kommer hända men följande vippbräda?
Faller åt vänster
Balanserar jämt
Faller åt höger

3)
Vilket av följande möbeltillverkande företag förknippas i huvudsak med sängar? => http://www.largestcompanies.se/topplistor/sverige/de-storsta-foretagen-efter-omsattning/bransch/tillverkning-av-mobler
Nobia
Hilding Anders
ITAB
Kinnarps

4
Vem grundade det kända inredningsföretaget Svenskt Tenn?
X Estrid Ericson
* Josef Frank
* Kjell och Märta Beijer

https://bloggar.aftonbladet.se/retromania/2014/11/test-kanner-du-igen-1900-talets-storsta-svenska-formgivare/

5
Vilken sysselsättning är följande designer mest kända för?
Märta Måås-Fjetterström (1873-1941) - Textilkonstnär
Stig Lindberg (1916-1982) - Keramiker och illustratör
Nils och Kajsa Strinning (1917-2006/1922-2017) - Arkitekt och formgivare

6
Vilken sysselsättning är följande designer mest kända för?
Sigvard Bernadotte - Formgivare och industridesigner
Yngve Ekström - Möbelformgivare och skulptör
Lisa Larson - Keramikskulptör

7
Vilken sysselsättning är följande designer mest kända för?
Marianne Westman - Kermaiker hushållsporslin
Bruno Mathsson (1907-1988) - Möbelformgivare
Josef Frank - Arkitekt och formgivare

8
Vilken skala är ritningen i om möbeln på bliden är 60cm hög?
1:6
1:7
1:8

9
Vilka av följande sammanfogningar matchar de bilder som visas?
* Sinkning
* Laskning
* Gering

10
Vad kallas det när man ritar object för en digital 3D-miljö?
* Skissa
* Modellera
* Forma

11
Vad betyder uttrycket FPS på svenska?
* Bildrutor per scen
* Figurer per sekund
* Bildrutor per sekund
* Figurer per scen


Skiljefråga: Hur många polygoner innehåller denna bild som Caroline har renderat?

*/



/*

Add Answer explaination

https://www.sitepoint.com/create-qr-code-reader-mobile-website/

--------------
SubjectTypes
--------------
Math	fas fa-square-root-alt
History	fas fa-landmark
Movies	fas fa-film
Food	fas fa-utensils
Chemistry	fas fa-flask
Geography	fas fa-globe-europe
Sport	fas fa-futbol
Science	fas fa-atom
Countries	fas fa-flag
Economics	fas fa-coins
Music	fas fa-music
Biology	fas fa-fish
Literature	fas fa-book
Religion	fas fa-church
Art	fas fa-palette
Health	fas fa-heartbeat
Weather	fas fa-cloud-sun-rain
Architecture	fas fa-building
Design	fas fa-pencil-ruler
Language	fas fa-language
Enviroment	fas fa-tree
Culture	fas fa-theater-masks
Jewelry	far fa-gem
Transport	fas fa-car
Travel	fas fa-plane
Military	fas fa-fighter-jet
Law	fas fa-gavel
Children	fas fa-baby
Statistics	fas fa-chart-bar
Board games	fas fa-chess-knight
Video games	fas fa-gamepad
Alchol	fas fa-cocktail
Hygiene	fas fa-pump-soap
Space	fas fa-user-astronaut
Agriculture	fas fa-tractor
Mechanics	fas fa-cogs
Craft	fas fa-hammer

--------------
-- ChallengeTypes
--------------
SingleChoice	fas fa-list-ol
MultiChoice	fas fa-tasks
Text	fas fa-font
MultiText	fas fa-bars
TrueOrFalse	fas fa-question
Sort	fas fa-sort-alpha-up
SortAndMatch	fas fa-th
Pussle	fas fa-puzzle-piece
WordScramble	fas fa-random
OddOneOut	fas fa-balance-scale
Rebus	fas fa-icons
ImageRebus	fas fa-images
SpellCheck	fas fa-spell-check
ClozeTest	fas fa-terminal

På Question lägg till LabelType => nummer => 1x2 o.s.v.

------------
--- GAME ---
------------
ChallengeTypes
------------
* SingleChoice
Vilket år är Erik född?
2087
1987
1887
------------
* MultiChoice
Vilket är Eriks favoritmat?
Korvstroganoff
Lasagne
Kyckling
------------
* Text
Vilket är Eriks favoritmat?
------------
* MultiText
Vilka djur finns på följande bild?
(Forest animals, Farm animals, Religioner)
Katt
Hund
Ko
Gris
------------
* TrueOrFalse
Caroline är en sångare!
Sant --- Falskt
--------------
* Sort
--------------
* SortAndMatch
1979 - Star wars - Mark Hamil
2009 - Avatar - Sam Worthington
--------------
* Pussle????
--------------
* WordScramble
declare @word nvarchar(50) = 'WordScramble';
declare @table table (Id uniqueidentifier, Letter nvarchar(1));
declare @position int = 0;
declare @id uniqueidentifier;
while (@position < len(@word))
begin
	set @position = @position + 1;
	insert @table
	select newid(), substring(@word, @position, 1);
end;
set @word = '';
while exists (select 1 from @table)
begin
	set @id = (select top 1 Id from @table order by Id);
	set @word = @word + (select top 1 Letter from @table where Id = @id);
	delete from @table where Id = @id;
end;
select @word;
--------------
* OddOneOut
Flygande fåglar
Kråka - Sparv - Ekkorre - Spjuv
--------------
* Rebus
Det var en gång en {fa-squirrel}
---------------
* ImageRebus
[HUS][HUND][GRÄS] = Lilla huset på prärien
---------------
* SpellCheck
---------------
* ClozeTest
---------------
* 
---------------
* 
---------------
* 
--------------------------------

Jämföra ord => https://stackoverflow.com/questions/6944056/c-sharp-compare-string-similarity


----
* Sherlock - 
* Poirot - 
* Robert Langdon - 
* Yoda - 
* Jack Reacher

*/
