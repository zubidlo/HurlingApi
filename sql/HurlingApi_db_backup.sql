USE [master]
GO
/****** Object:  Database [HurlingApi_db]    Script Date: 21/03/2015 22:41:10 ******/
CREATE DATABASE [HurlingApi_db]
GO
ALTER DATABASE [HurlingApi_db] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [HurlingApi_db].[dbo].[sp_fulltext_database] @action = 'disable'
end
GO
ALTER DATABASE [HurlingApi_db] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [HurlingApi_db] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [HurlingApi_db] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [HurlingApi_db] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [HurlingApi_db] SET ARITHABORT OFF 
GO
ALTER DATABASE [HurlingApi_db] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [HurlingApi_db] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [HurlingApi_db] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [HurlingApi_db] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [HurlingApi_db] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [HurlingApi_db] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [HurlingApi_db] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [HurlingApi_db] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [HurlingApi_db] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [HurlingApi_db] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [HurlingApi_db] SET  DISABLE_BROKER 
GO
ALTER DATABASE [HurlingApi_db] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [HurlingApi_db] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [HurlingApi_db] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [HurlingApi_db] SET ALLOW_SNAPSHOT_ISOLATION ON 
GO
ALTER DATABASE [HurlingApi_db] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [HurlingApi_db] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [HurlingApi_db] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [HurlingApi_db] SET RECOVERY FULL 
GO
ALTER DATABASE [HurlingApi_db] SET  MULTI_USER 
GO
ALTER DATABASE [HurlingApi_db] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [HurlingApi_db] SET DB_CHAINING OFF 
GO
USE [HurlingApi_db]
GO
/****** Object:  Table [dbo].[Leagues]    Script Date: 21/03/2015 22:41:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Leagues](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[NextFixtures] [datetime] NOT NULL,
	[Week] [tinyint] NOT NULL,
 CONSTRAINT [PK_Leagues] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Messages]    Script Date: 21/03/2015 22:41:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Messages](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Text] [nvarchar](max) NOT NULL,
	[UserId] [int] NOT NULL,
	[Created] [datetime] NOT NULL,
 CONSTRAINT [PK_Messages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Players]    Script Date: 21/03/2015 22:41:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Players](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](max) NOT NULL,
	[LastName] [nvarchar](max) NOT NULL,
	[GaaTeam] [nvarchar](max) NOT NULL,
	[LastWeekPoints] [decimal](18, 0) NOT NULL,
	[OverallPoints] [decimal](18, 0) NOT NULL,
	[Price] [decimal](18, 0) NOT NULL,
	[Rating] [tinyint] NOT NULL,
	[Injured] [bit] NOT NULL,
	[PositionId] [int] NOT NULL,
 CONSTRAINT [PK_Players] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Positions]    Script Date: 21/03/2015 22:41:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Positions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Positions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[TeamPlayer]    Script Date: 21/03/2015 22:41:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TeamPlayer](
	[Teams_Id] [int] NOT NULL,
	[Players_Id] [int] NOT NULL,
 CONSTRAINT [PK_TeamPlayer] PRIMARY KEY CLUSTERED 
(
	[Teams_Id] ASC,
	[Players_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Teams]    Script Date: 21/03/2015 22:41:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Teams](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[OverAllPoints] [decimal](18, 0) NOT NULL,
	[LastWeekPoints] [decimal](18, 0) NOT NULL,
	[Budget] [decimal](18, 0) NOT NULL,
	[LeagueId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_Teams] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Users]    Script Date: 21/03/2015 22:41:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Username] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET IDENTITY_INSERT [dbo].[Leagues] ON 

INSERT [dbo].[Leagues] ([Id], [Name], [NextFixtures], [Week]) VALUES (1, N'The League', CAST(N'2014-05-23 14:25:10.000' AS DateTime), 0)
SET IDENTITY_INSERT [dbo].[Leagues] OFF
SET IDENTITY_INSERT [dbo].[Messages] ON 

INSERT [dbo].[Messages] ([Id], [Text], [UserId], [Created]) VALUES (24, N'Martin there are termites in this forum.', 2, CAST(N'2015-01-29 20:31:48.307' AS DateTime))
INSERT [dbo].[Messages] ([Id], [Text], [UserId], [Created]) VALUES (25, N'Shut the fuck up Michael', 2, CAST(N'2015-01-29 20:31:59.647' AS DateTime))
INSERT [dbo].[Messages] ([Id], [Text], [UserId], [Created]) VALUES (36, N'Enter Message Here
', 2, CAST(N'2015-01-30 18:04:17.897' AS DateTime))
INSERT [dbo].[Messages] ([Id], [Text], [UserId], [Created]) VALUES (41, N'Enter your message here...
', 2, CAST(N'2015-01-31 01:33:20.827' AS DateTime))
INSERT [dbo].[Messages] ([Id], [Text], [UserId], [Created]) VALUES (42, N'Hi There', 2, CAST(N'2015-01-31 01:35:39.227' AS DateTime))
INSERT [dbo].[Messages] ([Id], [Text], [UserId], [Created]) VALUES (45, N'Enter Message Here', 2, CAST(N'2015-01-31 14:47:52.257' AS DateTime))
INSERT [dbo].[Messages] ([Id], [Text], [UserId], [Created]) VALUES (46, N'Martin there are termites in this forum.', 2, CAST(N'2015-01-31 14:50:20.143' AS DateTime))
INSERT [dbo].[Messages] ([Id], [Text], [UserId], [Created]) VALUES (47, N'Message from David', 3, CAST(N'2015-01-31 15:48:40.097' AS DateTime))
INSERT [dbo].[Messages] ([Id], [Text], [UserId], [Created]) VALUES (48, N'Hi there, testing. ', 3, CAST(N'2015-01-31 17:03:59.587' AS DateTime))
INSERT [dbo].[Messages] ([Id], [Text], [UserId], [Created]) VALUES (49, N'mICHAEL', 2, CAST(N'2015-01-31 17:04:23.587' AS DateTime))
INSERT [dbo].[Messages] ([Id], [Text], [UserId], [Created]) VALUES (50, N'Jam Tarts', 2, CAST(N'2015-01-31 20:05:16.557' AS DateTime))
INSERT [dbo].[Messages] ([Id], [Text], [UserId], [Created]) VALUES (51, N'Tester', 3, CAST(N'2015-02-01 13:05:25.150' AS DateTime))
INSERT [dbo].[Messages] ([Id], [Text], [UserId], [Created]) VALUES (52, N'Hhi you are all gay', 3, CAST(N'2015-02-01 17:10:16.313' AS DateTime))
INSERT [dbo].[Messages] ([Id], [Text], [UserId], [Created]) VALUES (53, N'I hate CSS bullshit', 3, CAST(N'2015-02-01 21:54:45.933' AS DateTime))
INSERT [dbo].[Messages] ([Id], [Text], [UserId], [Created]) VALUES (54, N'Hey', 2, CAST(N'2015-02-02 12:06:19.900' AS DateTime))
INSERT [dbo].[Messages] ([Id], [Text], [UserId], [Created]) VALUES (55, N'ppooioij', 2, CAST(N'2015-02-02 17:00:28.857' AS DateTime))
INSERT [dbo].[Messages] ([Id], [Text], [UserId], [Created]) VALUES (56, N'DFGDFGFGFG', 2, CAST(N'2015-02-02 19:31:31.003' AS DateTime))
INSERT [dbo].[Messages] ([Id], [Text], [UserId], [Created]) VALUES (57, N'Message  ', 3, CAST(N'2015-02-02 19:48:01.943' AS DateTime))
INSERT [dbo].[Messages] ([Id], [Text], [UserId], [Created]) VALUES (58, N'Hey there termites', 3, CAST(N'2015-02-03 18:25:23.393' AS DateTime))
INSERT [dbo].[Messages] ([Id], [Text], [UserId], [Created]) VALUES (59, N'ngfhfg', 3, CAST(N'2015-02-04 19:40:34.887' AS DateTime))
INSERT [dbo].[Messages] ([Id], [Text], [UserId], [Created]) VALUES (60, N'New Message', 3, CAST(N'2015-02-04 21:06:41.013' AS DateTime))
INSERT [dbo].[Messages] ([Id], [Text], [UserId], [Created]) VALUES (61, N'Hello Martin', 1, CAST(N'2015-02-05 12:01:38.450' AS DateTime))
INSERT [dbo].[Messages] ([Id], [Text], [UserId], [Created]) VALUES (62, N'dsfsdsds', 1, CAST(N'2015-02-09 13:36:58.110' AS DateTime))
INSERT [dbo].[Messages] ([Id], [Text], [UserId], [Created]) VALUES (63, N'A+', 3, CAST(N'2015-03-19 18:31:07.370' AS DateTime))
SET IDENTITY_INSERT [dbo].[Messages] OFF
SET IDENTITY_INSERT [dbo].[Players] ON 

INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (1, N'Steven ', N'Chester', N'Dublin', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(10000 AS Decimal(18, 0)), 4, 0, 1)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (2, N'Anthony ', N'Nash', N'Cork', CAST(1 AS Decimal(18, 0)), CAST(30 AS Decimal(18, 0)), CAST(8000 AS Decimal(18, 0)), 3, 0, 1)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (3, N'John', N'Gallagher', N'Laois', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(5000 AS Decimal(18, 0)), 2, 0, 1)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (4, N'Aidan', N'Deveanay', N'Sligo ', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(4000 AS Decimal(18, 0)), 2, 0, 1)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (5, N'Mark ', N'Fanning', N'Wexford', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(7000 AS Decimal(18, 0)), 3, 0, 1)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (6, N'Peter', N'Kelly', N'Dublin', CAST(1 AS Decimal(18, 0)), CAST(3 AS Decimal(18, 0)), CAST(9000 AS Decimal(18, 0)), 4, 0, 2)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (7, N'Shane', N'Durkin', N'Dublin', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(10000 AS Decimal(18, 0)), 2, 0, 2)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (8, N'Matthew ', N'O Hanlon', N'Wexford', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(4000 AS Decimal(18, 0)), 2, 0, 2)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (9, N'Andrew', N'Shore', N'Wexford', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(5000 AS Decimal(18, 0)), 3, 0, 2)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (10, N'Paul', N'Crily', N'Laois', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(2000 AS Decimal(18, 0)), 1, 0, 2)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (11, N'David', N'Rath', N'Laois', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(3000 AS Decimal(18, 0)), 2, 0, 2)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (12, N'Johnny ', N'Martyr', N'Sligo', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(6000 AS Decimal(18, 0)), 3, 0, 2)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (13, N'Charlie', N'Harrison', N'Sligo', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(9000 AS Decimal(18, 0)), 4, 0, 2)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (14, N'Mark', N'Ellis', N'Cork', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(9000 AS Decimal(18, 0)), 4, 0, 2)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (15, N'Christopher', N'Joyce', N'Cork', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(7000 AS Decimal(18, 0)), 3, 0, 2)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (18, N'Darren', N'Garry', N'Kildare', CAST(4 AS Decimal(18, 0)), CAST(9 AS Decimal(18, 0)), CAST(5000 AS Decimal(18, 0)), 2, 0, 3)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (19, N'Paul ', N'Jones', N'Dublin', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(5000 AS Decimal(18, 0)), 4, 0, 4)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (20, N'Dan ', N'Jones', N'Cork', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(6000 AS Decimal(18, 0)), 2, 0, 5)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (21, N'Kirk', N'Jones', N'Laois', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(3000 AS Decimal(18, 0)), 2, 0, 6)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (22, N'Harry', N'Jones', N'Dublin', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(8000 AS Decimal(18, 0)), 4, 0, 7)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (23, N'Karl', N'Jones', N'Kildare', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(7000 AS Decimal(18, 0)), 3, 0, 8)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (25, N'Chris', N'Kerr', N'Antrim', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(8000 AS Decimal(18, 0)), 3, 0, 1)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (26, N'Tony', N'Scullion', N'Antrim', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(8000 AS Decimal(18, 0)), 3, 0, 2)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (28, N'Martin', N'Johnston', N'Antrim', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(5000 AS Decimal(18, 0)), 2, 0, 3)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (29, N'Justin', N'Crozier', N'Antrim', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(5000 AS Decimal(18, 0)), 2, 0, 3)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (30, N'Richard ', N'Johnston', N'Antrim', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(5000 AS Decimal(18, 0)), 2, 0, 4)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (31, N'Anthony', N'Healy', N'Antrim', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(4000 AS Decimal(18, 0)), 2, 0, 3)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (32, N'Michael', N'McCann', N'Antrim', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(9000 AS Decimal(18, 0)), 4, 0, 5)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (33, N'Sean', N'McVeigh', N'Antrim', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(8000 AS Decimal(18, 0)), 3, 0, 5)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (34, N'Patrick', N'McBride', N'Antrim', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(8000 AS Decimal(18, 0)), 4, 0, 5)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (35, N'Thomas', N'McCann', N'Antrim', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(9000 AS Decimal(18, 0)), 5, 0, 6)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (36, N'Paddy', N'Cunningham', N'Antrim', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(7000 AS Decimal(18, 0)), 3, 0, 6)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (37, N'Kevin', N'Niblock', N'Antrim', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(7000 AS Decimal(18, 0)), 3, 0, 7)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (38, N'Aodhan', N'Gallagher', N'Antrim', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(6000 AS Decimal(18, 0)), 3, 0, 7)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (39, N'James', N'Loghry', N'Antrim', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(6000 AS Decimal(18, 0)), 3, 0, 7)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (40, N'Kevin', N'Brady', N'Antrim', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(7000 AS Decimal(18, 0)), 3, 0, 7)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (41, N'Brendan', N'Herron', N'Antrim', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(5000 AS Decimal(18, 0)), 2, 0, 8)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (42, N'Ryan', N'Murray', N'Antrim', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(8000 AS Decimal(18, 0)), 4, 0, 8)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (43, N'Michael', N'Pollack', N'Antrim', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(7000 AS Decimal(18, 0)), 3, 0, 8)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (44, N'John', N'Carron', N'Antrim', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(8000 AS Decimal(18, 0)), 4, 0, 5)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (45, N'Sean', N'Finch', N'Antrim', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(6000 AS Decimal(18, 0)), 3, 0, 5)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (46, N'Deaglan ', N'O''Hagan', N'Antrim', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(5000 AS Decimal(18, 0)), 2, 0, 3)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (47, N'Paul', N'Doherty', N'Antrim', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(5000 AS Decimal(18, 0)), 2, 0, 3)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (48, N'Sean', N'Kelly', N'Antrim', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(5000 AS Decimal(18, 0)), 2, 0, 3)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (49, N'Owen', N'Gallagher', N'Antrim', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(6000 AS Decimal(18, 0)), 3, 0, 4)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (50, N'Niall', N'Delegary', N'Antrim', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(5000 AS Decimal(18, 0)), 2, 0, 4)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (51, N'Andy', N'McClean', N'Antrim', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(5000 AS Decimal(18, 0)), 2, 0, 2)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (52, N'Martin', N'Armstrong', N'Antrim', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(7000 AS Decimal(18, 0)), 4, 0, 7)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (53, N'John', N'Finnucane', N'Antrim', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(5000 AS Decimal(18, 0)), 2, 0, 2)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (54, N'Gary', N'McGuire', N'Dublin', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(7000 AS Decimal(18, 0)), 3, 0, 1)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (55, N'Michael', N'Carton', N'Dublin', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(7000 AS Decimal(18, 0)), 4, 0, 2)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (56, N'Niall', N'Corcran', N'Dublin', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(7000 AS Decimal(18, 0)), 4, 0, 2)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (57, N'Thomas', N'Brady', N'Dublin', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(6000 AS Decimal(18, 0)), 3, 0, 3)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (58, N'Oisin', N'Gough', N'Dublin', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(7000 AS Decimal(18, 0)), 3, 0, 3)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (59, N'Simon', N'Lambert', N'Dublin', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(6000 AS Decimal(18, 0)), 3, 0, 4)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (61, N'Maurice', N'O''Brien', N'Dublin', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(5000 AS Decimal(18, 0)), 2, 0, 4)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (62, N'Alan', N'MCrabbe', N'Dublin', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(11000 AS Decimal(18, 0)), 4, 0, 5)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (63, N'Joey', N'Boland', N'Dublin', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(10000 AS Decimal(18, 0)), 5, 0, 5)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (64, N'John', N'McCaffery', N'Dublin', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(6000 AS Decimal(18, 0)), 3, 0, 5)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (65, N'Peter', N'Kelly', N'Dublin', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(6000 AS Decimal(18, 0)), 3, 0, 3)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (66, N'Shane', N'Durkin', N'Dublin', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(7000 AS Decimal(18, 0)), 4, 0, 4)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (67, N'Paul', N'Schutte', N'Dublin', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(5000 AS Decimal(18, 0)), 2, 0, 4)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (68, N'Simon', N'Timlin', N'Dublin', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(6000 AS Decimal(18, 0)), 3, 0, 2)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (69, N'Martin', N'Quilty', N'Dublin', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(8000 AS Decimal(18, 0)), 3, 0, 5)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (70, N'Colm', N'Cronin', N'Dublin', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(9000 AS Decimal(18, 0)), 4, 0, 5)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (71, N'David', N'O''Callaghan', N'Dublin', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(8000 AS Decimal(18, 0)), 4, 0, 6)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (72, N'David', N'Treacy', N'Dublin', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(7000 AS Decimal(18, 0)), 3, 0, 7)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (73, N'Conor', N'McCormack', N'Dublin', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(7000 AS Decimal(18, 0)), 3, 0, 8)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (74, N'Danny', N'Sutcliffe', N'Dublin', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(8000 AS Decimal(18, 0)), 4, 0, 6)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (75, N'Paul', N'Ryan', N'Dublin', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(7000 AS Decimal(18, 0)), 3, 0, 7)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (76, N'Kevin', N'Byrne', N'Dublin', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(7000 AS Decimal(18, 0)), 3, 0, 8)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (77, N'Paul', N'Winters', N'Dublin', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(6000 AS Decimal(18, 0)), 3, 0, 8)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (78, N'Eamon', N'Dillon', N'Dublin', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(5000 AS Decimal(18, 0)), 2, 0, 8)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (79, N'Kevin', N'O''Boyle', N'Antrim', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(10000 AS Decimal(18, 0)), 4, 0, 2)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (80, N'Niall', N'McMorrow', N'Dublin', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(7000 AS Decimal(18, 0)), 3, 0, 8)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (81, N'Ian', N'O''Reagan', N'Waterford', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(6000 AS Decimal(18, 0)), 4, 0, 1)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (82, N'Sean', N'O''Keefe', N'Waterford', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(7000 AS Decimal(18, 0)), 3, 0, 1)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (83, N'Darragh', N'Fives', N'Waterford', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(9000 AS Decimal(18, 0)), 4, 0, 2)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (84, N'Shane', N'Fives', N'Waterford', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(7000 AS Decimal(18, 0)), 3, 0, 2)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (85, N'Liam', N'Lawlor', N'Waterford', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(7000 AS Decimal(18, 0)), 3, 0, 3)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (86, N'Noel', N'Connors', N'Waterford', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(8000 AS Decimal(18, 0)), 4, 0, 2)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (87, N'Jamie', N'Nagle', N'Waterford', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(7000 AS Decimal(18, 0)), 3, 0, 4)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (88, N'Michael', N'Walsh', N'Waterford', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(6000 AS Decimal(18, 0)), 3, 0, 4)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (89, N'Kevin', N'Moran', N'Waterford', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(8000 AS Decimal(18, 0)), 4, 0, 5)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (90, N'Richie', N'Foley', N'Waterford', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(8000 AS Decimal(18, 0)), 4, 0, 5)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (91, N'Jake', N'Dillon', N'Waterford', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(6000 AS Decimal(18, 0)), 3, 0, 6)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (92, N'Seamus', N'Prendergast', N'Waterford', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(7000 AS Decimal(18, 0)), 4, 0, 8)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (93, N'Shane', N'O''Sullivan', N'Waterford', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(7000 AS Decimal(18, 0)), 4, 0, 6)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (94, N'Brian', N'O''Sullivan', N'Waterford', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(6000 AS Decimal(18, 0)), 3, 0, 6)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (95, N'Maurice', N'Shanahan', N'Waterford', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(9000 AS Decimal(18, 0)), 4, 0, 8)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (96, N'Jamie', N'Barron', N'Waterford', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(7000 AS Decimal(18, 0)), 3, 0, 7)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (97, N'Eoin', N'Kelly', N'Waterford', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(10000 AS Decimal(18, 0)), 5, 0, 8)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (98, N'John', N'Mullane', N'Waterford', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(9000 AS Decimal(18, 0)), 5, 0, 6)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (99, N'Eoin', N'McGrath', N'Waterford', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(9000 AS Decimal(18, 0)), 5, 0, 7)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (100, N'Stephen', N'Molumphy', N'Waterford', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(8000 AS Decimal(18, 0)), 4, 0, 5)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (101, N'Paudie', N'Prendergast', N'Waterford', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(6000 AS Decimal(18, 0)), 3, 0, 3)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (102, N'Eddie', N'Barrett', N'Waterford', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(7000 AS Decimal(18, 0)), 3, 0, 4)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (103, N'Gavin', N'O''Brien', N'Waterford', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(7000 AS Decimal(18, 0)), 3, 0, 6)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (104, N'Thomas', N'Ryan', N'Waterford', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(5000 AS Decimal(18, 0)), 2, 0, 6)
GO
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (105, N'Declan', N'Prendergast', N'Waterford', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(6000 AS Decimal(18, 0)), 3, 0, 2)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (106, N'Shane', N'Casey', N'Waterford', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(8000 AS Decimal(18, 0)), 4, 0, 4)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (107, N'Henry', N'Shefflin', N'Kilkenny', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(13000 AS Decimal(18, 0)), 5, 0, 8)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (108, N'Richie', N'Power', N'Kilkenny', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(12000 AS Decimal(18, 0)), 5, 0, 7)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (109, N'David', N'Herity', N'Kilkenny', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(9000 AS Decimal(18, 0)), 4, 0, 1)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (110, N'Eoin', N'Murphy', N'Kilkenny', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(7000 AS Decimal(18, 0)), 3, 0, 1)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (111, N'JJ', N'Delaney', N'Kilkenny', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(14000 AS Decimal(18, 0)), 5, 0, 3)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (112, N'Tommy', N'Walsh', N'Kilkenny', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(10000 AS Decimal(18, 0)), 5, 0, 4)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (113, N'Jackie', N'Tyrell', N'Kilkenny', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(9000 AS Decimal(18, 0)), 5, 0, 3)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (114, N'Noel', N'Hickey', N'Kilkenny', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(9000 AS Decimal(18, 0)), 5, 0, 3)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (115, N'Brian', N'Hogan', N'Kilkenny', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(10000 AS Decimal(18, 0)), 5, 0, 4)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (116, N'Paddy', N'Hogan', N'Kilkenny', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(8000 AS Decimal(18, 0)), 4, 0, 4)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (117, N'Richie', N'Doyle', N'Kilkenny', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(7000 AS Decimal(18, 0)), 3, 0, 3)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (118, N'Cillian', N'Buckley', N'Kilkenny', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(9000 AS Decimal(18, 0)), 5, 0, 2)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (119, N'Michael', N'Rice', N'Kilkenny', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(10000 AS Decimal(18, 0)), 5, 0, 5)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (120, N'Michael', N'Fennelley', N'Kilkenny', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(10000 AS Decimal(18, 0)), 5, 0, 5)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (121, N'TJ', N'Reid', N'Kilkenny', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(10000 AS Decimal(18, 0)), 5, 0, 7)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (122, N'Eoin', N'Larkin', N'Kilkenny', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(10000 AS Decimal(18, 0)), 5, 0, 6)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (123, N'Walter', N'Walsh', N'Kilkenny', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(9000 AS Decimal(18, 0)), 5, 0, 8)
INSERT [dbo].[Players] ([Id], [FirstName], [LastName], [GaaTeam], [LastWeekPoints], [OverallPoints], [Price], [Rating], [Injured], [PositionId]) VALUES (124, N'Walter', N'Fennely', N'Kilkenny', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(8000 AS Decimal(18, 0)), 4, 0, 7)
SET IDENTITY_INSERT [dbo].[Players] OFF
SET IDENTITY_INSERT [dbo].[Positions] ON 

INSERT [dbo].[Positions] ([Id], [Name]) VALUES (1, N'Goalkeeper')
INSERT [dbo].[Positions] ([Id], [Name]) VALUES (2, N'Corner-Back')
INSERT [dbo].[Positions] ([Id], [Name]) VALUES (3, N'Full-Back')
INSERT [dbo].[Positions] ([Id], [Name]) VALUES (4, N'Half-Back')
INSERT [dbo].[Positions] ([Id], [Name]) VALUES (5, N'Midfielder')
INSERT [dbo].[Positions] ([Id], [Name]) VALUES (6, N'Half-Forward')
INSERT [dbo].[Positions] ([Id], [Name]) VALUES (7, N'Corner-Forward')
INSERT [dbo].[Positions] ([Id], [Name]) VALUES (8, N'Full-Forward')
SET IDENTITY_INSERT [dbo].[Positions] OFF
INSERT [dbo].[TeamPlayer] ([Teams_Id], [Players_Id]) VALUES (1, 1)
INSERT [dbo].[TeamPlayer] ([Teams_Id], [Players_Id]) VALUES (3, 2)
INSERT [dbo].[TeamPlayer] ([Teams_Id], [Players_Id]) VALUES (2, 3)
INSERT [dbo].[TeamPlayer] ([Teams_Id], [Players_Id]) VALUES (3, 6)
INSERT [dbo].[TeamPlayer] ([Teams_Id], [Players_Id]) VALUES (1, 8)
INSERT [dbo].[TeamPlayer] ([Teams_Id], [Players_Id]) VALUES (2, 14)
INSERT [dbo].[TeamPlayer] ([Teams_Id], [Players_Id]) VALUES (3, 18)
INSERT [dbo].[TeamPlayer] ([Teams_Id], [Players_Id]) VALUES (3, 19)
INSERT [dbo].[TeamPlayer] ([Teams_Id], [Players_Id]) VALUES (3, 20)
INSERT [dbo].[TeamPlayer] ([Teams_Id], [Players_Id]) VALUES (3, 21)
INSERT [dbo].[TeamPlayer] ([Teams_Id], [Players_Id]) VALUES (3, 22)
INSERT [dbo].[TeamPlayer] ([Teams_Id], [Players_Id]) VALUES (3, 23)
SET IDENTITY_INSERT [dbo].[Teams] ON 

INSERT [dbo].[Teams] ([Id], [Name], [OverAllPoints], [LastWeekPoints], [Budget], [LeagueId], [UserId]) VALUES (1, N'Middleagers', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(30000 AS Decimal(18, 0)), 1, 1)
INSERT [dbo].[Teams] ([Id], [Name], [OverAllPoints], [LastWeekPoints], [Budget], [LeagueId], [UserId]) VALUES (2, N'Dubliners', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(100000 AS Decimal(18, 0)), 1, 2)
INSERT [dbo].[Teams] ([Id], [Name], [OverAllPoints], [LastWeekPoints], [Budget], [LeagueId], [UserId]) VALUES (3, N'Apples', CAST(5 AS Decimal(18, 0)), CAST(40 AS Decimal(18, 0)), CAST(71000 AS Decimal(18, 0)), 1, 3)
SET IDENTITY_INSERT [dbo].[Teams] OFF
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Id], [Email], [Username], [Password]) VALUES (1, N'zubidlo@gmail.com', N'zubidlo', N'password123')
INSERT [dbo].[Users] ([Id], [Email], [Username], [Password]) VALUES (2, N'michaelcjames10@gmail.com', N'michael', N'password')
INSERT [dbo].[Users] ([Id], [Email], [Username], [Password]) VALUES (3, N'hellodavidkelly@gmail.com', N'david', N'password')
INSERT [dbo].[Users] ([Id], [Email], [Username], [Password]) VALUES (7, N'zubidlo@gmail.com', N'Administrator', N'Password1')
INSERT [dbo].[Users] ([Id], [Email], [Username], [Password]) VALUES (8, N'p@g.com', N'Paul', N'password')
INSERT [dbo].[Users] ([Id], [Email], [Username], [Password]) VALUES (9, N'b@g.com', N'Billy', N'password')
INSERT [dbo].[Users] ([Id], [Email], [Username], [Password]) VALUES (10, N'm@gmail.com', N'digweed', N'password')
SET IDENTITY_INSERT [dbo].[Users] OFF
/****** Object:  Index [IX_FK_UserMessage]    Script Date: 21/03/2015 22:41:11 ******/
CREATE NONCLUSTERED INDEX [IX_FK_UserMessage] ON [dbo].[Messages]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Index [IX_FK_PositionPlayer]    Script Date: 21/03/2015 22:41:11 ******/
CREATE NONCLUSTERED INDEX [IX_FK_PositionPlayer] ON [dbo].[Players]
(
	[PositionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Index [IX_FK_TeamPlayer_Player]    Script Date: 21/03/2015 22:41:11 ******/
CREATE NONCLUSTERED INDEX [IX_FK_TeamPlayer_Player] ON [dbo].[TeamPlayer]
(
	[Players_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Index [IX_FK_TeamLeague]    Script Date: 21/03/2015 22:41:11 ******/
CREATE NONCLUSTERED INDEX [IX_FK_TeamLeague] ON [dbo].[Teams]
(
	[LeagueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Index [IX_FK_UserTeam]    Script Date: 21/03/2015 22:41:11 ******/
CREATE NONCLUSTERED INDEX [IX_FK_UserTeam] ON [dbo].[Teams]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
ALTER TABLE [dbo].[Messages]  WITH CHECK ADD  CONSTRAINT [FK_UserMessage] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Messages] CHECK CONSTRAINT [FK_UserMessage]
GO
ALTER TABLE [dbo].[Players]  WITH CHECK ADD  CONSTRAINT [FK_PositionPlayer] FOREIGN KEY([PositionId])
REFERENCES [dbo].[Positions] ([Id])
GO
ALTER TABLE [dbo].[Players] CHECK CONSTRAINT [FK_PositionPlayer]
GO
ALTER TABLE [dbo].[TeamPlayer]  WITH CHECK ADD  CONSTRAINT [FK_TeamPlayer_Player] FOREIGN KEY([Players_Id])
REFERENCES [dbo].[Players] ([Id])
GO
ALTER TABLE [dbo].[TeamPlayer] CHECK CONSTRAINT [FK_TeamPlayer_Player]
GO
ALTER TABLE [dbo].[TeamPlayer]  WITH CHECK ADD  CONSTRAINT [FK_TeamPlayer_Team] FOREIGN KEY([Teams_Id])
REFERENCES [dbo].[Teams] ([Id])
GO
ALTER TABLE [dbo].[TeamPlayer] CHECK CONSTRAINT [FK_TeamPlayer_Team]
GO
ALTER TABLE [dbo].[Teams]  WITH CHECK ADD  CONSTRAINT [FK_TeamLeague] FOREIGN KEY([LeagueId])
REFERENCES [dbo].[Leagues] ([Id])
GO
ALTER TABLE [dbo].[Teams] CHECK CONSTRAINT [FK_TeamLeague]
GO
ALTER TABLE [dbo].[Teams]  WITH CHECK ADD  CONSTRAINT [FK_UserTeam] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Teams] CHECK CONSTRAINT [FK_UserTeam]
GO
USE [master]
GO
ALTER DATABASE [HurlingApi_db] SET  READ_WRITE 
GO
