/*

Typ			Match	Enval
Enkel		4-6		3
Lätt		6-8		4
Normal		8-10	5
Svår		10-12	6
Extrem		12-14	7

ChallengeTypeId 
1 Enval
5 Sant eller falskt

2 Flerval
6 Sortera
15 Kombinera

*/
-- ToDo: Make this CTE

select q.QuestionId
     ,q.ChallengeId
     ,q.ChallengeTypeId
     ,q.ChallengeSubjectId
     ,q.DifficultyId
     ,q.ImageId
     ,q.CorrectAlternatives
     ,q.TotalAlternatives
	 ,q.ScorePenalty
	 ,q.QuestionId
     ,q.ScoreValue
     ,q.NewScore
     ,q.AlternativeScore
	 ,q.AlternativeScore * q.CorrectAlternatives as NewTotal
	,'update game.Alternatives set ScoreValue = ' + cast(q.AlternativeScore as nvarchar(50)) + ' where QuestionId = ' + cast(q.QuestionId as nvarchar(50)) + ' and IsCorrect = 1;'
from (
	select *
		,round(q.NewScore / q.CorrectAlternatives, 0) as AlternativeScore		
	from (
		select q.*
			,round (
				case
					when q.ChallengeTypeId = 1 then q.CorrectAlternatives - 1 + q.DifficultyId
					when q.ChallengeTypeId = 5 then q.TotalAlternatives - 1 + q.DifficultyId
					when q.ChallengeTypeId = 2 then (2 + q.DifficultyId * 1.5) / q.ScorePenalty
					when q.ChallengeTypeId = 6 then (2 + q.DifficultyId * 1.75) / q.ScorePenalty
					when q.ChallengeTypeId = 15 then (2 + q.DifficultyId * 1.75) / q.ScorePenalty
				end
				,0)
			as NewScore
		from (
			select q.*
				,qa.Antal as CorrectAlternatives
				,qt.Antal as TotalAlternatives
				,qa.ScoreValue
			-- select distinct q.ChallengeTypeId
				,cast(qa.Antal as float) / cast(qt.Antal as float) * 2 as ScorePenalty
				--,cast(qa.Antal as float) / cast(qt.Antal as float) + 4 / cast(qt.Antal as float) as ScorePenalty
			from game.Questions as q
			left join (
				select count(1) as Antal, a.QuestionId, sum(a.ScoreValue) as ScoreValue
				from game.Questions as q
				inner join game.Alternatives as a on a.QuestionId = q.QuestionId
				where a.IsCorrect = 1
					and (q.ChallengeTypeId <> 15 or a.CorrectColumn <> 1)
				group by a.QuestionId
			) as qa on qa.QuestionId = q.QuestionId
			left join (
				select count(1) as Antal, a.QuestionId
				from game.Alternatives as a 
				group by a.QuestionId
			) as qt on qt.QuestionId = q.QuestionId
		) as q
	) as q
) as q
--where q.questionId>=57
--where q.ChallengeTypeId in (15)
where q.ScoreValue <> q.AlternativeScore * q.CorrectAlternatives
