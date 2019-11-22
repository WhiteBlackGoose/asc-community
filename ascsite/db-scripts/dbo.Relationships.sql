DROP TABLE [dbo].[Relationships];
CREATE TABLE [dbo].[Relationships] (
    [UserId]    INT NOT NULL PRIMARY KEY,
    [ProjectId] INT NOT NULL,
    [Type]      INT NOT NULL,
);

