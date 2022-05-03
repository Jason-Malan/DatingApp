USE [DatingDB]
GO

CREATE TABLE [Photos]
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[PlatformUserId] INT NOT NULL,
	[PublicId] VARCHAR(MAX) NULL,
	[IsMain] INT NOT NULL,
	[Url] VARCHAR(500) NOT NULL,
	CONSTRAINT FK_Photos_Users_UserID FOREIGN KEY (PlatformUserId) REFERENCES dbo.PlatformUsers (Id)
)
GO

INSERT INTO dbo.Photos ([PlatformUserId], [IsMain], [Url])
VALUES	(6, 1, 'http://not-real.html')
GO

SELECT * FROM [Photos]
GO