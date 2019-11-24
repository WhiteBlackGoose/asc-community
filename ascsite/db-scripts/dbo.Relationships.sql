DROP TABLE [dbo].UserPostContributions;
CREATE TABLE [dbo].[UserPostContributions] (
	[Id]	INT NOT NULL PRIMARY KEY DEFAULT 0,
	[PostId]    INT,
    [UserId]    INT,
	[Type]   INT,
);