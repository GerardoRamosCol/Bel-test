USE [master]
GO
/****** Object:  Database [AppLog3]    Script Date: 8/24/2018 3:09:22 a. m. ******/
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'AppLog3')
BEGIN
CREATE DATABASE [AppLog3]
END
GO
USE [AppLog3]
GO
/****** Object:  Table [dbo].[LOG]    Script Date: 8/24/2018 3:09:22 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LOG]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[LOG](
	[ID_LOG] [int] IDENTITY(1,1) NOT NULL,
	[TYPE] [int] NULL,
	[MESSAGE] [varchar](max) NULL,
 CONSTRAINT [PK_LOG] PRIMARY KEY CLUSTERED 
(
	[ID_LOG] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  StoredProcedure [dbo].[UP_INSERT_LOG]    Script Date: 8/24/2018 3:09:22 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UP_INSERT_LOG]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[UP_INSERT_LOG] AS' 
END
GO
ALTER PROCEDURE [dbo].[UP_INSERT_LOG]
(
	@TYPE INT,
	@MESSAGE VARCHAR(MAX)
)
AS
BEGIN
	INSERT INTO LOG
	([TYPE],[MESSAGE])
	VALUES
	(@TYPE, @MESSAGE)

END

GO

USE [master]
GO
ALTER DATABASE [AppLog3] SET  READ_WRITE 
GO
