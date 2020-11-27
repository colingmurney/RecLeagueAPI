USE RecLeagueV2;

--CREATE PROCEDURE SelectScheduledGames @PlayerId int
--AS
--BEGIN
--SELECT Date, home.TeamName as Home, away.TeamName as Away, VenueName, City, Address
--FROM Game 
--JOIN Team home ON HomeTeamId = home.TeamId
--JOIN Team away ON AwayTeamId = away.TeamId
--JOIN Venue ON Venue.VenueId = Game.VenueId
--JOIN Player ON Player.TeamId = home.TeamId
--WHERE Date > GETDATE() AND PlayerId = @PlayerId
--UNION
--SELECT Date, home.TeamName as Home, away.TeamName as Away, VenueName, City, Address
--FROM Game 
--JOIN Team home ON HomeTeamId = home.TeamId
--JOIN Team away ON AwayTeamId = away.TeamId
--JOIN Venue ON Venue.VenueId = Game.VenueId
--JOIN Player ON Player.TeamId = away.TeamId	
--WHERE Date > GETDATE() AND PlayerId = @PlayerId
--END;

--EXEC dbo.SelectScheduledGames @PlayerId = 1;

--CREATE PROCEDURE SelectGameResults @PlayerId int
--AS
--BEGIN
--SELECT Date, home.TeamName as Home, HomeTeamHomeScore as HomeScore, away.TeamName as Away, HomeTeamAwayScore as AwayScore
--FROM Game 
--JOIN Team home ON HomeTeamId = home.TeamId
--JOIN Team away ON AwayTeamId = away.TeamId
--JOIN Player ON Player.TeamId = home.TeamId
--WHERE Date < GETDATE() AND PlayerId = @PlayerId AND HomeTeamHomeScore = AwayTeamHomeScore AND HomeTeamAwayScore = AwayTeamAwayScore
--UNION
--SELECT Date, home.TeamName as Home, HomeTeamHomeScore as HomeScore, away.TeamName as Away, HomeTeamAwayScore as AwayScore
--FROM Game 
--JOIN Team home ON HomeTeamId = home.TeamId
--JOIN Team away ON AwayTeamId = away.TeamId
--JOIN Player ON Player.TeamId = away.TeamId
--WHERE Date < GETDATE() AND PlayerId = @PlayerId AND HomeTeamHomeScore = AwayTeamHomeScore AND HomeTeamAwayScore = AwayTeamAwayScore;
--END;

--CREATE PROCEDURE PendingCaptainReports @PlayerId int
--AS
--BEGIN
--SELECT DISTINCT Date, t1.TeamName as Home, t2.TeamName as Away, VenueName
--FROM Game
--LEFT JOIN Player p1 ON HomeTeamId = p1.TeamId
--LEFT JOIN Player p2 ON AwayTeamId = p2.TeamId
--JOIN Team t1 ON HomeTeamId = t1.TeamId
--JOIN Team t2 ON AwayTeamId = t2.TeamId
--JOIN Venue ON Game.VenueId = Venue.VenueId
--WHERE (p1.PlayerId = @PlayerId OR p2.PlayerId = @PlayerId)
--	AND (AwayTeamAwayScore IS NULL OR AwayTeamHomeScore IS NULL OR HomeTeamAwayScore IS NULL OR HomeTeamHomeScore IS NULL)
--	AND DATE < GETDATE()
--END;

--CREATE PROCEDURE SelectGameStatuses @PlayerId int
--AS
--BEGIN
--SELECT GameStatusName FROM Player
--JOIN GameStatus ON Player.GameStatusId = GameStatus.GameStatusId
--WHERE PlayerId = @PlayerId
--END;

--CREATE PROCEDURE HomeTeamPlayerStatuses @PlayerId int
--AS
--BEGIN
--SELECT Date, TeamName, FirstName, LastName, GameStatusName FROM Game
--JOIN Player ON HomeTeamId = TeamId
--JOIN Team ON Player.TeamId = Team.TeamId
--JOIN GameStatus ON Player.GameStatusId = GameStatus.GameStatusId
--WHERE GameId = (
--	SELECT TOP 1 GameId FROM Game
--	JOIN Player p1 ON HomeTeamId = p1.TeamId
--	JOIN Player p2 ON AwayTeamId = p2.TeamId
--	WHERE Date > GETDATE() 
--	AND (p1.PlayerId = @PlayerId or p2.PlayerId = @PlayerId)
--	ORDER BY Date
--	)
--END;

--CREATE PROCEDURE AwayTeamPlayerStatuses @PlayerId int
--AS
--BEGIN
--SELECT Date, TeamName, FirstName, LastName, GameStatusName FROM Game
--JOIN Player ON AwayTeamId = TeamId
--JOIN Team ON Player.TeamId = Team.TeamId
--JOIN GameStatus ON Player.GameStatusId = GameStatus.GameStatusId
--WHERE GameId = (
--	SELECT TOP 1 GameId FROM Game
--	JOIN Player p1 ON HomeTeamId = p1.TeamId
--	JOIN Player p2 ON AwayTeamId = p2.TeamId
--	WHERE Date > GETDATE() 
--	AND (p1.PlayerId = @PlayerId or p2.PlayerId = @PlayerId)
--	ORDER BY Date
--	)
--END

--EXEC dbo.AwayTeamPlayerStatuses @PlayerId = 1;


--CREATE PROCEDURE UpdatePlayerGameStatus @PlayerId int, @GameStatusName varchar(50)
--AS
--BEGIN
--UPDATE Player 
--SET Player.GameStatusId = GameStatus.GameStatusId
--FROM Player, GameStatus 
--WHERE PlayerId = @PlayerId AND GameStatusName = @GameStatusName
--END;

--EXEC dbo.UpdatePlayerGameStatus @PlayerId = 1, @GameStatusName = 'attending';

--CREATE PROCEDURE SelectAvailableSports @RegionName varchar(50)
--AS
--BEGIN
--SELECT SportName FROM Sport
--JOIN League ON League.SportId = Sport.SportId
--JOIN Region ON Region.RegionId = League.RegionId
--WHERE RegionName = @RegionName
--END;

--EXEC dbo.SelectAvailableSports @RegionName = 'Moncton';

--CREATE PROCEDURE SelectAvailableTiers @RegionName varchar(50), @SportName varchar(50)
--AS
--BEGIN
--SELECT Tier FROM League
--JOIN Region ON Region.RegionId = League.RegionId
--JOIN Sport ON Sport.SportId = League.SportId
--WHERE RegionName = @RegionName AND SportName = @SportName
--END;

--EXEC dbo.SelectAvailableTiers @RegionName = 'Moncton', @SportName = 'Hockey';

--CREATE PROCEDURE SelectAvailableTeams @RegionName varchar(50), @SportName varchar(50), @Tier int
--AS
--BEGIN
--SELECT TeamName FROM Team
--JOIN League ON League.LeagueId = Team.LeagueId
--JOIN Region ON Region.RegionId = League.RegionId
--JOIN Sport ON Sport.SportId = League.SportId
--WHERE RegionName = @RegionName AND SportName = @SportName AND Tier = @Tier
--END;

--EXEC dbo.SelectAvailableTeams @RegionName = 'Moncton', @SportName = 'Hockey', @Tier = 1;

--CREATE PROCEDURE CreateTeam @TeamName varchar(50), @RegionName varchar(50), @SportName varchar(50), @Tier int
--AS
--BEGIN
--INSERT INTO Team VALUES (@TeamName, (
--	SELECT LeagueId FROM League
--	JOIN Region ON Region.RegionId = League.RegionId
--	JOIN Sport ON Sport.SportId = League.LeagueId
--	WHERE RegionName = @RegionName AND SportName = @SportName AND Tier = @Tier
--	));
--	SELECT * FROM Team
--	WHERE TeamName = @RegionName;
--END;

--EXEC dbo.CreateTeam @TeamName = 'Test team3', @RegionName = 'Moncton', @SportName = 'Hockey', @Tier = 1;