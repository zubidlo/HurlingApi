
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/24/2015 21:30:17
-- Generated from EDMX file: C:\Users\martin\Documents\Visual Studio 2013\Projects\HurlingApi\HurlingApi\Models\HurlingModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [HurlingApi_db];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UserMessage]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Messages] DROP CONSTRAINT [FK_UserMessage];
GO
IF OBJECT_ID(N'[dbo].[FK_PositionPlayer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Players] DROP CONSTRAINT [FK_PositionPlayer];
GO
IF OBJECT_ID(N'[dbo].[FK_TeamTeamPlayer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TeamPlayers] DROP CONSTRAINT [FK_TeamTeamPlayer];
GO
IF OBJECT_ID(N'[dbo].[FK_PlayerTeamPlayer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TeamPlayers] DROP CONSTRAINT [FK_PlayerTeamPlayer];
GO
IF OBJECT_ID(N'[dbo].[FK_TeamLeague]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Teams] DROP CONSTRAINT [FK_TeamLeague];
GO
IF OBJECT_ID(N'[dbo].[FK_UserTeam]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_UserTeam];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Positions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Positions];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[Leagues]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Leagues];
GO
IF OBJECT_ID(N'[dbo].[Teams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Teams];
GO
IF OBJECT_ID(N'[dbo].[Players]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Players];
GO
IF OBJECT_ID(N'[dbo].[Messages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Messages];
GO
IF OBJECT_ID(N'[dbo].[TeamPlayers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TeamPlayers];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Positions'
CREATE TABLE [dbo].[Positions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [Username] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Leagues'
CREATE TABLE [dbo].[Leagues] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [NextFixtures] datetime  NOT NULL,
    [Week] tinyint  NOT NULL
);
GO

-- Creating table 'Teams'
CREATE TABLE [dbo].[Teams] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [OverAllPoints] decimal(18,0)  NOT NULL,
    [LastWeekPoints] decimal(18,0)  NOT NULL,
    [Budget] decimal(18,0)  NOT NULL,
    [LeagueId] int  NOT NULL,
    [UserId] int  NOT NULL
);
GO

-- Creating table 'Players'
CREATE TABLE [dbo].[Players] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [GaaTeam] nvarchar(max)  NOT NULL,
    [LastWeekPoints] decimal(18,0)  NOT NULL,
    [OverallPoints] decimal(18,0)  NOT NULL,
    [Price] decimal(18,0)  NOT NULL,
    [Rating] tinyint  NOT NULL,
    [Injured] bit  NOT NULL,
    [PositionId] int  NOT NULL
);
GO

-- Creating table 'Messages'
CREATE TABLE [dbo].[Messages] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Text] nvarchar(max)  NOT NULL,
    [UserId] int  NOT NULL,
    [Created] datetimeoffset  NULL
);
GO

-- Creating table 'TeamPlayers'
CREATE TABLE [dbo].[TeamPlayers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TeamId] int  NULL,
    [PlayerId] int  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Positions'
ALTER TABLE [dbo].[Positions]
ADD CONSTRAINT [PK_Positions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Leagues'
ALTER TABLE [dbo].[Leagues]
ADD CONSTRAINT [PK_Leagues]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Teams'
ALTER TABLE [dbo].[Teams]
ADD CONSTRAINT [PK_Teams]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Players'
ALTER TABLE [dbo].[Players]
ADD CONSTRAINT [PK_Players]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Messages'
ALTER TABLE [dbo].[Messages]
ADD CONSTRAINT [PK_Messages]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TeamPlayers'
ALTER TABLE [dbo].[TeamPlayers]
ADD CONSTRAINT [PK_TeamPlayers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UserId] in table 'Messages'
ALTER TABLE [dbo].[Messages]
ADD CONSTRAINT [FK_UserMessage]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserMessage'
CREATE INDEX [IX_FK_UserMessage]
ON [dbo].[Messages]
    ([UserId]);
GO

-- Creating foreign key on [PositionId] in table 'Players'
ALTER TABLE [dbo].[Players]
ADD CONSTRAINT [FK_PositionPlayer]
    FOREIGN KEY ([PositionId])
    REFERENCES [dbo].[Positions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PositionPlayer'
CREATE INDEX [IX_FK_PositionPlayer]
ON [dbo].[Players]
    ([PositionId]);
GO

-- Creating foreign key on [TeamId] in table 'TeamPlayers'
ALTER TABLE [dbo].[TeamPlayers]
ADD CONSTRAINT [FK_TeamTeamPlayer]
    FOREIGN KEY ([TeamId])
    REFERENCES [dbo].[Teams]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TeamTeamPlayer'
CREATE INDEX [IX_FK_TeamTeamPlayer]
ON [dbo].[TeamPlayers]
    ([TeamId]);
GO

-- Creating foreign key on [PlayerId] in table 'TeamPlayers'
ALTER TABLE [dbo].[TeamPlayers]
ADD CONSTRAINT [FK_PlayerTeamPlayer]
    FOREIGN KEY ([PlayerId])
    REFERENCES [dbo].[Players]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PlayerTeamPlayer'
CREATE INDEX [IX_FK_PlayerTeamPlayer]
ON [dbo].[TeamPlayers]
    ([PlayerId]);
GO

-- Creating foreign key on [LeagueId] in table 'Teams'
ALTER TABLE [dbo].[Teams]
ADD CONSTRAINT [FK_TeamLeague]
    FOREIGN KEY ([LeagueId])
    REFERENCES [dbo].[Leagues]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TeamLeague'
CREATE INDEX [IX_FK_TeamLeague]
ON [dbo].[Teams]
    ([LeagueId]);
GO

-- Creating foreign key on [UserId] in table 'Teams'
ALTER TABLE [dbo].[Teams]
ADD CONSTRAINT [FK_UserTeam]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserTeam'
CREATE INDEX [IX_FK_UserTeam]
ON [dbo].[Teams]
    ([UserId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------