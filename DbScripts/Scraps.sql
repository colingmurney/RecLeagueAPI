USE RecLeagueV2;

--UPDATE Player
--SET Player.GameStatusId = GameStatus.GameStatusId
--FROM Player
--JOIN GameStatus ON Player.GameStatusId = GameStatus.GameStatusId
--WHERE GameStatusName = 'Absent';

--Make this into a stored procedure
UPDATE Player 
SET Player.GameStatusId = GameStatus.GameStatusId
FROM Player, GameStatus 
WHERE GameStatusName = 'Absent';

SELECT * FROM Player;
SELECT * FROM GameStatus;