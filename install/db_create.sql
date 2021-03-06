USE [master]
GO
/****** Object:  Database [AMQP_Exchange]    Script Date: 08/04/2016 18:22:07 ******/
CREATE DATABASE [AMQP_Exchange]
GO
USE [AMQP_Exchange]
GO
/****** Object:  User [NT AUTHORITY\NETWORK SERVICE]    Script Date: 08/04/2016 18:22:07 ******/
CREATE USER [NT AUTHORITY\NETWORK SERVICE] FOR LOGIN [NT AUTHORITY\NETWORK SERVICE] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[Outbound]    Script Date: 08/04/2016 18:22:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Outbound](
	[Message_Id] [int] IDENTITY(1,1) NOT NULL,
	[QueueId] [int] NOT NULL,
	[DateWritten] [datetime] NULL,
	[DateSent] [datetime] NULL,
	[Message] [nvarchar](max) NULL,
	[ErrorFlag] [bit] NOT NULL,
	[AppMsgId] [nvarchar](36) NULL,
 CONSTRAINT [PK_Outbound] PRIMARY KEY CLUSTERED 
(
	[Message_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Log]    Script Date: 08/04/2016 18:22:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Log](
	[Log_Id] [int] IDENTITY(1,1) NOT NULL,
	[Timestamp] [datetime] NOT NULL,
	[Source] [nvarchar](200) NULL,
	[IsError] [bit] NOT NULL,
	[HostId] [int] NULL,
	[QueueId] [int] NULL,
	[Inbound_Id] [int] NULL,
	[Outbound_Id] [int] NULL,
	[Message] [nvarchar](max) NULL,
	[Details] [nvarchar](max) NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[Log_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Inbound]    Script Date: 08/04/2016 18:22:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Inbound](
	[Message_Id] [int] IDENTITY(1,1) NOT NULL,
	[QueueId] [int] NOT NULL,
	[DateReceived] [datetime] NOT NULL,
	[DateRead] [datetime] NULL,
	[Message] [nvarchar](max) NULL,
 CONSTRAINT [PK_Inbound] PRIMARY KEY CLUSTERED 
(
	[Message_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Hosts]    Script Date: 08/04/2016 18:22:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hosts](
	[Host_Id] [int] IDENTITY(1,1) NOT NULL,
	[Host] [nvarchar](200) NOT NULL,
	[Port] [int] NULL,
	[VirtualHost] [nvarchar](200) NULL,
	[Username] [nvarchar](200) NULL,
	[Password] [nvarchar](200) NULL,
	[SslEnabled] [bit] NOT NULL,
	[PrefetchCount] [int] NULL,
 CONSTRAINT [PK_Hosts] PRIMARY KEY CLUSTERED 
(
	[Host_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Queues]    Script Date: 08/04/2016 18:22:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Queues](
	[Queue_Id] [int] IDENTITY(1,1) NOT NULL,
	[Host_Id] [int] NOT NULL,
	[Direction] [nchar](10) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[SenderPollInterval] [int] NULL,
	[Base64Data] [bit] NOT NULL,
	[Codepage] [int] NULL,
	[Exchange] [nvarchar](1000) NULL,
 CONSTRAINT [PK_Queues] PRIMARY KEY CLUSTERED 
(
	[Queue_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Default [DF_Outbound_ErrorFlag]    Script Date: 08/04/2016 18:22:09 ******/
ALTER TABLE [dbo].[Outbound] ADD  CONSTRAINT [DF_Outbound_ErrorFlag]  DEFAULT ((0)) FOR [ErrorFlag]
GO
/****** Object:  Default [DF_Log_IsError]    Script Date: 08/04/2016 18:22:09 ******/
ALTER TABLE [dbo].[Log] ADD  CONSTRAINT [DF_Log_IsError]  DEFAULT ((0)) FOR [IsError]
GO
/****** Object:  Default [DF_Hosts_2_SslEnabled]    Script Date: 08/04/2016 18:22:09 ******/
ALTER TABLE [dbo].[Hosts] ADD  CONSTRAINT [DF_Hosts_2_SslEnabled]  DEFAULT ((0)) FOR [SslEnabled]
GO
/****** Object:  Default [DF_Queues_Base64Data]    Script Date: 08/04/2016 18:22:09 ******/
ALTER TABLE [dbo].[Queues] ADD  CONSTRAINT [DF_Queues_Base64Data]  DEFAULT ((0)) FOR [Base64Data]
GO
/****** Object:  ForeignKey [FK_Queues_Hosts]    Script Date: 08/04/2016 18:22:09 ******/
ALTER TABLE [dbo].[Queues]  WITH CHECK ADD  CONSTRAINT [FK_Queues_Hosts] FOREIGN KEY([Host_Id])
REFERENCES [dbo].[Hosts] ([Host_Id])
GO
ALTER TABLE [dbo].[Queues] CHECK CONSTRAINT [FK_Queues_Hosts]
GO
