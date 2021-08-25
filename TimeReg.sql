USE [TimeReg]
GO
/****** Object:  Table [dbo].[AccessRights]    Script Date: 28-Jun-18 22:59:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[Employees]    Script Date: 28-Jun-18 22:59:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employees](
	[EmployeeID] [int] IDENTITY(1,1) NOT NULL,
	[Netname] [nvarchar](25) NOT NULL,
	[FullName] [nvarchar](50) NOT NULL,
	[SuperUser] [bit] NOT NULL,
	[Number] [int] NOT NULL,
	[Department] [int] NOT NULL,
 CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED 
(
	[EmployeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Main]    Script Date: 28-Jun-18 22:59:26 ******/
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
/****** Object:  Table [dbo].[MyProjects]    Script Date: 28-Jun-18 22:59:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MyProjects](
	[EmployeeID] [int] NOT NULL,
	[ProjectID] [int] NOT NULL,
 CONSTRAINT [Unique_MyProjects] UNIQUE NONCLUSTERED 
(
	[EmployeeID] ASC,
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Projects]    Script Date: 28-Jun-18 22:59:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Projects](
	[ProjectID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Parent] [int] NOT NULL,
	[ProjectNo] [nvarchar](50) NULL,
	[Closed] [bit] NOT NULL,
	[PSONumber] [int] NOT NULL,
	[Category] [nvarchar](10) NOT NULL,
	[ManagerID] [int] NULL,
	[Grouptag] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Projects] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Employees] ADD  CONSTRAINT [DF_Employees_Number]  DEFAULT ((0)) FOR [Number]
GO
ALTER TABLE [dbo].[Employees] ADD  CONSTRAINT [DF_Employees_Department]  DEFAULT ((0)) FOR [Department]
GO
ALTER TABLE [dbo].[Projects] ADD  CONSTRAINT [DF_Projects_Closed]  DEFAULT ((0)) FOR [Closed]
GO
ALTER TABLE [dbo].[Projects] ADD  CONSTRAINT [DF_Projects_PSONumber]  DEFAULT ((0)) FOR [PSONumber]
GO
ALTER TABLE [dbo].[Projects] ADD  CONSTRAINT [DF_Projects_Category]  DEFAULT (N'NON') FOR [Category]
GO
ALTER TABLE [dbo].[Projects] ADD  DEFAULT ('') FOR [Grouptag]
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
ALTER TABLE [dbo].[MyProjects]  WITH NOCHECK ADD  CONSTRAINT [FK_MyProjects_Employees] FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[Employees] ([EmployeeID])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[MyProjects] NOCHECK CONSTRAINT [FK_MyProjects_Employees]
GO
ALTER TABLE [dbo].[MyProjects]  WITH CHECK ADD  CONSTRAINT [FK_MyProjects_Projects] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[Projects] ([ProjectID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MyProjects] CHECK CONSTRAINT [FK_MyProjects_Projects]
GO
ALTER TABLE [dbo].[Projects]  WITH CHECK ADD  CONSTRAINT [FK_Projects_Employees] FOREIGN KEY([ManagerID])
REFERENCES [dbo].[Employees] ([EmployeeID])
ON UPDATE SET NULL
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Projects] CHECK CONSTRAINT [FK_Projects_Employees]
GO

INSERT INTO [dbo].[Projects] 
(Name, Parent, ProjectNo, Category, ManagerID, Grouptag)
VALUES ('Vacation',-1,NULL,'OUT',NULL,'Vacation')
GO
INSERT INTO [dbo].[Projects] 
(Name, Parent, ProjectNo, Category, ManagerID, Grouptag)
VALUES ('Sickness',-1,NULL,'OUT',NULL,'Sickness')
GO