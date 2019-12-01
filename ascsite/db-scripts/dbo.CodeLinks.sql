use [asc]
drop table [dbo].[CodeLinks];
CREATE TABLE [dbo].[CodeLinks] (
    [Id]           INT  NOT NULL PRIMARY KEY IDENTITY,
    [Code]		   NText NULL
);