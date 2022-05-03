USE [DatingDB]
GO

CREATE TABLE [PlatformUsers] (
	[Id] INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[UserName] VARCHAR(200) NOT NULL,
	[PasswordHash] VARBINARY(MAX) NOT NULL,
	[PasswordSalt] VARBINARY(MAX) NOT NULL,
	[DateOfBirth] DATETIME,
	[KnownAs] VARCHAR(100),
	[Created] DATETIME DEFAULT GETDATE(),
	[LastActive] DATETIME DEFAULT GETDATE(),
	[Gender] VARCHAR(7),
	[Introduction] VARCHAR(MAX),
	[LookingFor] VARCHAR(750),
	[Interests] VARCHAR(1000),
	[City] VARCHAR(200),
	[Country] VARCHAR(200)
);
GO

SELECT * FROM [PlatformUsers]
GO