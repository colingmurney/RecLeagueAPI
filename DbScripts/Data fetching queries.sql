USE RecLeagueV2;

------Upcoming games for users league
------Currently will show all a users games even if they are in multiple leagues
--SELECT Date, home.TeamName as Home, away.TeamName as Away, VenueName, City, Address
--FROM Game 
--JOIN Team home ON HomeTeamId = home.TeamId
--JOIN Team away ON AwayTeamId = away.TeamId
--JOIN Venue ON Venue.VenueId = Game.VenueId
--JOIN Player ON Player.TeamId = home.TeamId	--home
--WHERE Date > GETDATE() AND PlayerId = 1		--must equal below
--UNION
--SELECT Date, home.TeamName as Home, away.TeamName as Away, VenueName, City, Address
--FROM Game 
--JOIN Team home ON HomeTeamId = home.TeamId
--JOIN Team away ON AwayTeamId = away.TeamId
--JOIN Venue ON Venue.VenueId = Game.VenueId
--JOIN Player ON Player.TeamId = away.TeamId	--away
--WHERE Date > GETDATE() AND PlayerId = 1;	--must equal above

----Previous game with scores for user. For now, only if home team and away team reported scores equal.
----Currently will show all a users games even if they are in multiple leagues
--SELECT Date, home.TeamName as Home, HomeTeamHomeScore as HomeScore, away.TeamName as Away, HomeTeamAwayScore as AwayScore
--FROM Game 
--JOIN Team home ON HomeTeamId = home.TeamId
--JOIN Team away ON AwayTeamId = away.TeamId
--JOIN Player ON Player.TeamId = home.TeamId	--home
--WHERE Date < GETDATE() AND PlayerId = 1 AND HomeTeamHomeScore = AwayTeamHomeScore AND HomeTeamAwayScore = AwayTeamAwayScore -- only show past games with no descrepancy in the reported scores
--UNION
--SELECT Date, home.TeamName as Home, HomeTeamHomeScore as HomeScore, away.TeamName as Away, HomeTeamAwayScore as AwayScore
--FROM Game 
--JOIN Team home ON HomeTeamId = home.TeamId
--JOIN Team away ON AwayTeamId = away.TeamId
--JOIN Player ON Player.TeamId = away.TeamId	--away
--WHERE Date < GETDATE() AND PlayerId = 1 AND HomeTeamHomeScore = AwayTeamHomeScore AND HomeTeamAwayScore = AwayTeamAwayScore;

--If user is a captain, find any past games that they haven't submitted a captains report for (AKA if they were home team, the HomeTeamAwayScore and HomeTeamHomeScore are still NULL)
--Since the get player query is always at the beginning, the endpoint will only run this query is the player.isCaptain is true
--Therefore, it is not necessary to check for isCaptain in query.
--SELECT DISTINCT Date, t1.TeamName as Home, t2.TeamName as Away, VenueName
--FROM Game
--LEFT JOIN Player p1 ON HomeTeamId = p1.TeamId
--LEFT JOIN Player p2 ON AwayTeamId = p2.TeamId
--JOIN Team t1 ON HomeTeamId = t1.TeamId
--JOIN Team t2 ON AwayTeamId = t2.TeamId
--JOIN Venue ON Game.VenueId = Venue.VenueId
--WHERE (p1.PlayerId = 1 OR p2.PlayerId = 1)
--	AND (AwayTeamAwayScore IS NULL OR AwayTeamHomeScore IS NULL OR HomeTeamAwayScore IS NULL OR HomeTeamHomeScore IS NULL)
--	AND DATE < GETDATE();

--Status for next game
--There will be a trigger or event that will reset the GameStatus for every player in a game one the game date has passed
--SELECT GameStatusName FROM Player
--JOIN GameStatus ON Player.GameStatusId = GameStatus.GameStatusId
--WHERE PlayerId = 1;


--Game status for each player on home team for the next game
--SELECT Date, TeamName, FirstName, LastName, GameStatusName FROM Game
--JOIN Player ON HomeTeamId = TeamId --HomeTeamId
--JOIN Team ON Player.TeamId = Team.TeamId
--JOIN GameStatus ON Player.GameStatusId = GameStatus.GameStatusId
--WHERE GameId = (
--	SELECT TOP 1 GameId FROM Game
--	JOIN Player p1 ON HomeTeamId = p1.TeamId
--	JOIN Player p2 ON AwayTeamId = p2.TeamId
--	WHERE Date > GETDATE() 
--	AND (p1.PlayerId = 1 or p2.PlayerId = 1)
--	ORDER BY Date
--	);

--Game status for each player on way team for the next game
SELECT Date, TeamName, FirstName, LastName, GameStatusName FROM Game
JOIN Player ON AwayTeamId = TeamId --AwayTeamId
JOIN Team ON Player.TeamId = Team.TeamId
JOIN GameStatus ON Player.GameStatusId = GameStatus.GameStatusId
WHERE GameId = (
	SELECT TOP 1 GameId FROM Game
	JOIN Player p1 ON HomeTeamId = p1.TeamId
	JOIN Player p2 ON AwayTeamId = p2.TeamId
	WHERE Date > GETDATE() 
	AND (p1.PlayerId = 1 or p2.PlayerId = 1)
	ORDER BY Date
	);


