drop table [dbo].[ProjectTiles];
CREATE TABLE [dbo].[ProjectTiles] (
    [Id]           INT  NOT NULL PRIMARY KEY IDENTITY,
    [Name]         NTEXT NOT NULL,
    [ProjectLink]  NTEXT NULL,
	[ImagePath]    NTEXT NULL
);