USE [TeamChat]
GO
/****** Object:  Table [dbo].[Login]    Script Date: 02.08.2023 19:33:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Login](
	[GUID] [int] IDENTITY(1,1) NOT NULL,
	[Account] [varchar](25) NOT NULL,
	[Password] [varchar](50) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LoginLog]    Script Date: 02.08.2023 19:33:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LoginLog](
	[GUID] [int] IDENTITY(1,1) NOT NULL,
	[Account] [varchar](100) NOT NULL,
	[time] [varchar](100) NOT NULL,
	[Type] [int] NOT NULL,
	[IpAddress] [varchar](4000) NOT NULL,
	[MacAddress] [varchar](4000) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TalkLog]    Script Date: 02.08.2023 19:33:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TalkLog](
	[GUID] [int] IDENTITY(1,1) NOT NULL,
	[time] [varchar](100) NOT NULL,
	[From] [varchar](4000) NOT NULL,
	[To] [varchar](4000) NOT NULL,
	[Message] [varchar](4000) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[LoginLog] ADD  DEFAULT (getdate()) FOR [time]
GO
ALTER TABLE [dbo].[LoginLog] ADD  DEFAULT ((0)) FOR [Type]
GO
ALTER TABLE [dbo].[LoginLog] ADD  DEFAULT ('') FOR [IpAddress]
GO
ALTER TABLE [dbo].[LoginLog] ADD  DEFAULT ('') FOR [MacAddress]
GO
ALTER TABLE [dbo].[TalkLog] ADD  DEFAULT (getdate()) FOR [time]
GO
ALTER TABLE [dbo].[TalkLog] ADD  DEFAULT ('') FOR [From]
GO
ALTER TABLE [dbo].[TalkLog] ADD  DEFAULT ('') FOR [To]
GO
ALTER TABLE [dbo].[TalkLog] ADD  DEFAULT ('') FOR [Message]
GO
