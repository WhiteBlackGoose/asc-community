DROP TABLE [dbo].[Users];
CREATE TABLE [dbo].[Users] (
    [Id]   INT  NOT NULL PRIMARY KEY DEFAULT 0,
    [Name] TEXT NOT NULL,
);

insert into Users values(0, 'MomoDev');
insert into Users values(1, 'WhiteBlackGoose');