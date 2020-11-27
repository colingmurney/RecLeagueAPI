USE RecLeagueV2;

--SET IDENTITY_INSERT GameStatus ON;
--INSERT INTO GameStatus (GameStatusId, GameStatusName) VALUES (1, 'Unreported');
--INSERT INTO GameStatus (GameStatusId, GameStatusName) VALUES (2, 'Absent');
--INSERT INTO GameStatus (GameStatusId, GameStatusName) VALUES (3, 'Attending');
--SET IDENTITY_INSERT GameStatus OFF;

--SET IDENTITY_INSERT Sport ON;
--INSERT INTO Sport (SportId, SportName) VALUES (1, 'Basketball');
--INSERT INTO Sport (SportId, SportName) VALUES (2, 'Hockey');
--INSERT INTO Sport (SportId, SportName) VALUES (3, 'Soccer');
--SET IDENTITY_INSERT Sport OFF;

--SET IDENTITY_INSERT Region ON;
--INSERT INTO Region (RegionId, RegionName, Province) VALUES (1, 'Halifax', 'NS');
--INSERT INTO Region (RegionId, RegionName, Province) VALUES (2, 'Moncton', 'NB');
--INSERT INTO Region (RegionId, RegionName, Province) VALUES (3, 'Valley', 'NS');
--SET IDENTITY_INSERT Region OFF;

--SET IDENTITY_INSERT Venue ON;
--INSERT INTO Venue (VenueId, VenueName, City, Address, RegionId) VALUES (2, 'Metro Centre', 'Halifax', '101 Brunswick Street', 1);
--INSERT INTO Venue (VenueId, VenueName, City, Address, RegionId) VALUES (3, '4-Pad', 'Some town in Moncton', '98 Clyde Boulevard', 2);
--INSERT INTO Venue (VenueId, VenueName, City, Address, RegionId) VALUES (4, 'Tony''s Court', 'Another town in Moncton', '101 Brunswick Street', 2);
--INSERT INTO Venue (VenueId, VenueName, City, Address, RegionId) VALUES (5, 'LeBrun Arena', 'Bedford', '43 Dykeman Road', 1);
--SET IDENTITY_INSERT Venue OFF;

--SET IDENTITY_INSERT League ON;
--INSERT INTO League (LeagueId, SportId, RegionId, Tier) VALUES (1, 1, 1, 3);
--INSERT INTO League (LeagueId, SportId, RegionId, Tier) VALUES (2, 2, 2, 1);
--INSERT INTO League (LeagueId, SportId, RegionId, Tier) VALUES (3, 3, 3, 4);
--INSERT INTO League (LeagueId, SportId, RegionId, Tier) VALUES (5, 2, 2, 2);
--INSERT INTO League (LeagueId, SportId, RegionId, Tier) VALUES (6, 1, 2, 1);
--SET IDENTITY_INSERT League OFF;

--SET IDENTITY_INSERT Team ON;
--INSERT INTO Team (TeamId, TeamName, LeagueId) VALUES (1, 'Waverley Gold Diggers', 2);
--INSERT INTO Team (TeamId, TeamName, LeagueId) VALUES (3, 'Holland Road Hoggers', 2);
--INSERT INTO Team (TeamId, TeamName, LeagueId) VALUES (4, 'Fall River Poppers', 1);
--INSERT INTO Team (TeamId, TeamName, LeagueId) VALUES (5, 'Moncton Mothers', 2);
--INSERT INTO Team (TeamId, TeamName, LeagueId) VALUES (6, 'Halifax Honkers', 2);
--INSERT INTO Team (TeamId, TeamName, LeagueId) VALUES (7, 'Truro GlueStix', 5);
--INSERT INTO Team (TeamId, TeamName, LeagueId) VALUES (8, 'Levi LogHeads', 1);
--SET IDENTITY_INSERT Team OFF;

--SET IDENTITY_INSERT Game ON;
--INSERT INTO Game (GameId, Date, HomeTeamId, AwayTeamId, VenueId, AwayTeamAwayScore, AwayTeamHomeScore, HomeTeamAwayScore, HomeTeamHomeScore) VALUES (2, '2020-10-28 18:00:00', 1, 3, 2, 3, 6, 3, 6);
--INSERT INTO Game (GameId, Date, HomeTeamId, AwayTeamId, VenueId, AwayTeamAwayScore, AwayTeamHomeScore, HomeTeamAwayScore, HomeTeamHomeScore) VALUES (3, '2020-11-19 12:00:00', 5, 1, 3, 0, 0, 0, 0);
--INSERT INTO Game (GameId, Date, HomeTeamId, AwayTeamId, VenueId, AwayTeamAwayScore, AwayTeamHomeScore, HomeTeamAwayScore, HomeTeamHomeScore) VALUES (5, '2020-12-02 19:00:00', 1, 5, 4, 0, 0, 0, 0);
--INSERT INTO Game (GameId, Date, HomeTeamId, AwayTeamId, VenueId, AwayTeamAwayScore, AwayTeamHomeScore, HomeTeamAwayScore, HomeTeamHomeScore) VALUES (6, '2020-12-05 19:00:00', 3, 1, 2, 0, 0, 0, 0);
--INSERT INTO Game (GameId, Date, HomeTeamId, AwayTeamId, VenueId, AwayTeamAwayScore, AwayTeamHomeScore, HomeTeamAwayScore, HomeTeamHomeScore) VALUES (7, '2020-12-20 19:00:00', 1, 3, 2, 0, 0, 0, 0);
--INSERT INTO Game (GameId, Date, HomeTeamId, AwayTeamId, VenueId, AwayTeamAwayScore, AwayTeamHomeScore, HomeTeamAwayScore, HomeTeamHomeScore) VALUES (8, '2020-11-02 19:00:00', 1, 6, 3, 0, 0, 0, 0);
--INSERT INTO Game (GameId, Date, HomeTeamId, AwayTeamId, VenueId, AwayTeamAwayScore, AwayTeamHomeScore, HomeTeamAwayScore, HomeTeamHomeScore) VALUES (8, '2020-11-02 19:00:00', 1, 6, 3, 0, 0, 0, 0);
--INSERT INTO Game (GameId, Date, HomeTeamId, AwayTeamId, VenueId, AwayTeamAwayScore, AwayTeamHomeScore, HomeTeamAwayScore, HomeTeamHomeScore) VALUES (9, '2021-01-05 13:00:00', 6, 1, 5, 0, 0, 0, 0);
--INSERT INTO Game (GameId, Date, HomeTeamId, AwayTeamId, VenueId, AwayTeamAwayScore, AwayTeamHomeScore, HomeTeamAwayScore, HomeTeamHomeScore) VALUES (10, '2020-11-19 12:00:00', 5, 1, 3, 5, 2, 5, 2);
--INSERT INTO Game (GameId, Date, HomeTeamId, AwayTeamId, VenueId, AwayTeamAwayScore, AwayTeamHomeScore, HomeTeamAwayScore, HomeTeamHomeScore) VALUES (11, '2020-11-09 19:00:00', 1, 6, 3, 7, 8, 7, 8);
--SET IDENTITY_INSERT Game OFF;

--SET IDENTITY_INSERT Player ON;
--INSERT INTO Player (PlayerId, FirstName, LastName, Email, Password, TeamId, IsCaptain, GameStatusId) VALUES (1, 'Colin', 'Murney', 'colin@gmail.com', 'abcd', 1, 1, 0);
--INSERT INTO Player (PlayerId, FirstName, LastName, Email, Password, TeamId, IsCaptain, GameStatusId) VALUES (3, 'Mark', 'Butyn', 'mark@gmail.com', 'abcd', 3, 1, 0);
--INSERT INTO Player (PlayerId, FirstName, LastName, Email, Password, TeamId, IsCaptain, GameStatusId) VALUES (6, 'Stew', 'Grant', 'stew@gmail.com', 'abcd', 3, 0, 0);
--INSERT INTO Player (PlayerId, FirstName, LastName, Email, Password, TeamId, IsCaptain, GameStatusId) VALUES (7, 'Stew', 'Grant', 'stew@gmail.com', 'abcd', 1, 0, 0);
--INSERT INTO Player (PlayerId, FirstName, LastName, Email, Password, TeamId, IsCaptain, GameStatusId) VALUES (17, 'Stew', 'Grant', 'stew@gmail.com', 'abcd', 1, 0, 0);
--INSERT INTO Player (PlayerId, FirstName, LastName, Email, Password, TeamId, IsCaptain, GameStatusId) VALUES (18, 'Stew', 'Grant', 'stew@gmail.com', 'abcd', 3 ,1, 0);
--SET IDENTITY_INSERT Player OFF;