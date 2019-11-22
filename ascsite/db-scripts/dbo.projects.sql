DROP TABLE [dbo].[projects];
CREATE TABLE [dbo].[projects] (
    [Id]           INT  NOT NULL PRIMARY KEY,
    [Name]         NTEXT NULL,
    [Announcement] NTEXT NULL,
    [Body]         NTEXT NULL,
);

