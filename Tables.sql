USE [master]
GO
/****** Object:  Database [TimeReg]    Script Date: 17-03-2018 15:32:11 ******/
CREATE DATABASE [TimeReg]
GO
ALTER DATABASE [TimeReg] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TimeReg].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [TimeReg] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [TimeReg] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [TimeReg] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [TimeReg] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [TimeReg] SET ARITHABORT OFF 
GO
ALTER DATABASE [TimeReg] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [TimeReg] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [TimeReg] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [TimeReg] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [TimeReg] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [TimeReg] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [TimeReg] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [TimeReg] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [TimeReg] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [TimeReg] SET  DISABLE_BROKER 
GO
ALTER DATABASE [TimeReg] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [TimeReg] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [TimeReg] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [TimeReg] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [TimeReg] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [TimeReg] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [TimeReg] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [TimeReg] SET RECOVERY FULL 
GO
ALTER DATABASE [TimeReg] SET  MULTI_USER 
GO
ALTER DATABASE [TimeReg] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [TimeReg] SET DB_CHAINING OFF 
GO
ALTER DATABASE [TimeReg] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [TimeReg] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [TimeReg]
GO
/****** Object:  User [TimeRegUser]    Script Date: 17-03-2018 15:32:12 ******/
CREATE USER [TimeRegUser] FOR LOGIN [TimeRegUser] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [TimeRegReader]    Script Date: 17-03-2018 15:32:12 ******/
CREATE USER [TimeRegReader] FOR LOGIN [TimeRegReader] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_datareader] ADD MEMBER [TimeRegUser]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [TimeRegUser]
GO
ALTER ROLE [db_datareader] ADD MEMBER [TimeRegReader]
GO
/****** Object:  Table [dbo].[AccessRights]    Script Date: 17-03-2018 15:32:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AccessRights](
	[Netname] [varchar](50) NOT NULL,
	[Roles] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Netname] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dates]    Script Date: 17-03-2018 15:32:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dates](
	[aDate] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[aDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Employees]    Script Date: 17-03-2018 15:32:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employees](
	[EmployeeID] [int] IDENTITY(1,1) NOT NULL,
	[Netname] [nvarchar](25) NOT NULL,
	[FullName] [nvarchar](50) NOT NULL,
	[SuperUser] [bit] NOT NULL,
	[Number] [int] NOT NULL CONSTRAINT [DF_Employees_Number]  DEFAULT ((0)),
	[Department] [int] NOT NULL CONSTRAINT [DF_Employees_Department]  DEFAULT ((0)),
 CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED 
(
	[EmployeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Forecasts]    Script Date: 17-03-2018 15:32:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Forecasts](
	[TaskID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [int] NOT NULL,
	[EmployeeID] [int] NOT NULL,
	[TaskDescr] [varchar](200) NOT NULL,
	[CalMonth] [int] NOT NULL,
	[Effort] [real] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TaskID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [NEW_CELL] UNIQUE NONCLUSTERED 
(
	[ProjectID] ASC,
	[EmployeeID] ASC,
	[TaskDescr] ASC,
	[CalMonth] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Main]    Script Date: 17-03-2018 15:32:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Main](
	[MainID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeID] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[ProjectID] [int] NOT NULL,
	[Hours] [real] NOT NULL,
	[Comment] [nvarchar](200) NULL,
	[InOracle] [datetime] NULL,
 CONSTRAINT [PK_Main] PRIMARY KEY CLUSTERED 
(
	[MainID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [No_Dupl] UNIQUE NONCLUSTERED 
(
	[ProjectID] ASC,
	[EmployeeID] ASC,
	[Date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MyProjects]    Script Date: 17-03-2018 15:32:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MyProjects](
	[EmployeeID] [int] NOT NULL,
	[ProjectID] [int] NOT NULL,
 CONSTRAINT [PK_MyProjects] PRIMARY KEY CLUSTERED 
(
	[EmployeeID] ASC,
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Projects]    Script Date: 17-03-2018 15:32:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Projects](
	[ProjectID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Parent] [int] NOT NULL,
	[ProjectNo] [nvarchar](50) NULL,
	[Closed] [bit] NOT NULL CONSTRAINT [DF_Projects_Closed]  DEFAULT ((0)),
	[PSONumber] [int] NOT NULL CONSTRAINT [DF_Projects_PSONumber]  DEFAULT ((0)),
	[Category] [nvarchar](10) NOT NULL CONSTRAINT [DF_Projects_Category]  DEFAULT (N'NON'),
	[ManagerID] [int] NULL,
	[Grouptag] [varchar](50) NOT NULL DEFAULT (''),
 CONSTRAINT [PK_Projects] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Query]    Script Date: 17-03-2018 15:32:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Query](
	[Name] [nvarchar](50) NOT NULL,
	[Project] [nvarchar](50) NOT NULL,
	[Tothours] [float] NULL
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[ForecastDates]    Script Date: 17-03-2018 15:32:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ForecastDates] AS
SELECT TaskID, ProjectID, EmployeeID, TaskDescr, Effort, 
DATEFROMPARTS((CalMonth/100),(CALMONTH%100),1) as TheDate 
FROM     dbo.Forecasts

GO
/****** Object:  View [dbo].[TotalPerEmployeePerProject]    Script Date: 17-03-2018 15:32:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[TotalPerEmployeePerProject]
AS
SELECT emps.FullName AS Name, projects.Name AS Project, ROUND(SUM(main.Hours), 0) AS Tothours
FROM  dbo.Main AS main INNER JOIN
               dbo.Projects AS projects ON main.ProjectID = projects.ProjectID INNER JOIN
               dbo.Employees AS emps ON main.EmployeeID = emps.EmployeeID
WHERE (main.Date <= '2015-12-31') AND (main.Date >= '2014-12-31')
GROUP BY projects.Name, emps.FullName

GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Employees]    Script Date: 17-03-2018 15:32:13 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Employees] ON [dbo].[Employees]
(
	[Netname] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_MyProjects]    Script Date: 17-03-2018 15:32:13 ******/
CREATE NONCLUSTERED INDEX [IX_MyProjects] ON [dbo].[MyProjects]
(
	[EmployeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Forecasts]  WITH CHECK ADD FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[Employees] ([EmployeeID])
GO
ALTER TABLE [dbo].[Forecasts]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [dbo].[Projects] ([ProjectID])
GO
ALTER TABLE [dbo].[Main]  WITH CHECK ADD  CONSTRAINT [FK_Main_Employees] FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[Employees] ([EmployeeID])
GO
ALTER TABLE [dbo].[Main] CHECK CONSTRAINT [FK_Main_Employees]
GO
ALTER TABLE [dbo].[Main]  WITH CHECK ADD  CONSTRAINT [FK_Main_Projects] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[Projects] ([ProjectID])
GO
ALTER TABLE [dbo].[Main] CHECK CONSTRAINT [FK_Main_Projects]
GO
ALTER TABLE [dbo].[Projects]  WITH CHECK ADD  CONSTRAINT [FK_Projects_Employees] FOREIGN KEY([ManagerID])
REFERENCES [dbo].[Employees] ([EmployeeID])
ON UPDATE SET NULL
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Projects] CHECK CONSTRAINT [FK_Projects_Employees]
GO
GO
ALTER DATABASE [TimeReg] SET  READ_WRITE 
GO
