DROP TABLE IF EXISTS Speakers
GO

CREATE TABLE Speakers
(
Id int NOT NULL PRIMARY KEY,
Name varchar(255) NOT NULL,
IsActive bit NOT NULL DEFAULT 1
)
GO

INSERT INTO Speakers ( Id, Name, IsActive ) VALUES ( 1, 'Adam Cogan', 1 )
GO

INSERT INTO Speakers ( Id, Name, IsActive ) VALUES ( 2, 'Adam Dymitruk', 1 )
GO

INSERT INTO Speakers ( Id, Name, IsActive ) VALUES ( 3, 'Alex Dunn', 1 )
GO

INSERT INTO Speakers ( Id, Name, IsActive ) VALUES ( 4, 'Alexander Arvidsson', 0 )
GO


DROP PROCEDURE IF EXISTS SetSpeakerStatus
GO

CREATE PROCEDURE SetSpeakerStatus
	@Id int,
	@IsActive bit
AS

	UPDATE Speakers SET IsActive = @IsActive WHERE Id = @Id;

GO

DROP PROCEDURE IF EXISTS GetActiveSpeakers
GO


CREATE PROCEDURE GetActiveSpeakers
AS
	SELECT * FROM Speakers WHERE IsActive = 1;

GO

DROP PROCEDURE IF EXISTS GetSpeakers
GO


CREATE PROCEDURE GetSpeakers
AS
	SELECT * FROM Speakers;

GO