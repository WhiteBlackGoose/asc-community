DROP TABLE [dbo].UserPostContributions;
CREATE TABLE [dbo].[UserPostContributions] (
	[Id]	INT NOT NULL PRIMARY KEY IDENTITY,
	[PostId]    INT,
    [UserId]    INT,
	[Type]   INT,
);