drop table [dbo].[Posts];
CREATE TABLE [dbo].[Posts] (
    [Id]           INT  NOT NULL PRIMARY KEY IDENTITY,
    [Name]         NTEXT NULL,
    [Announcement] NTEXT NULL,
    [Body]         NTEXT NULL,
	[Type]         INT NULL
);