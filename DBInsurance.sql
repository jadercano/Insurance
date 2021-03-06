/****** Object:  User [USR_INSURANCE]    Script Date: 8/7/2019 3:02:45 AM ******/
CREATE USER [USR_INSURANCE] WITH DEFAULT_SCHEMA=[dbo]
GO
sys.sp_addrolemember @rolename = N'db_owner', @membername = N'USR_INSURANCE'
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 8/7/2019 3:02:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[CustomerId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerInsurance]    Script Date: 8/7/2019 3:02:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerInsurance](
	[CustomerId] [uniqueidentifier] NOT NULL,
	[InsuranceId] [uniqueidentifier] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[Status] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_CustomerInsurance] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC,
	[InsuranceId] ASC,
	[StartDate] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Insurance]    Script Date: 8/7/2019 3:02:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Insurance](
	[InsuranceId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[CoverageType] [nvarchar](20) NOT NULL,
	[Coverage] [numeric](5, 2) NOT NULL,
	[Cost] [float] NOT NULL,
	[RiskType] [nvarchar](20) NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_Insurance] PRIMARY KEY CLUSTERED 
(
	[InsuranceId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[CustomerInsurance]  WITH CHECK ADD  CONSTRAINT [FK_CustomerInsurance_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([CustomerId])
GO
ALTER TABLE [dbo].[CustomerInsurance] CHECK CONSTRAINT [FK_CustomerInsurance_Customer]
GO
ALTER TABLE [dbo].[CustomerInsurance]  WITH CHECK ADD  CONSTRAINT [FK_CustomerInsurance_Insurance] FOREIGN KEY([InsuranceId])
REFERENCES [dbo].[Insurance] ([InsuranceId])
GO
ALTER TABLE [dbo].[CustomerInsurance] CHECK CONSTRAINT [FK_CustomerInsurance_Insurance]
GO
