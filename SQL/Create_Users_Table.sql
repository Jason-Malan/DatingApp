USE [DatingDB]
GO

CREATE TABLE [Users] (
	Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	UserName VARCHAR(200) NOT NULL
);

INSERT INTO [Users] (UserName)
VALUES ('Jason')