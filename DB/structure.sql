USE [master]
GO
/****** Object:  Database [NanoBlockExplorer]    Script Date: 9/29/2016 11:27:08 PM ******/
CREATE DATABASE [NanoBlockExplorer]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'NanoBlockExplorer', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\NanoBlockExplorer.mdf' , SIZE = 39936KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'NanoBlockExplorer_log', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\NanoBlockExplorer_log.ldf' , SIZE = 15040KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [NanoBlockExplorer] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [NanoBlockExplorer].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [NanoBlockExplorer] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [NanoBlockExplorer] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [NanoBlockExplorer] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [NanoBlockExplorer] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [NanoBlockExplorer] SET ARITHABORT OFF 
GO
ALTER DATABASE [NanoBlockExplorer] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [NanoBlockExplorer] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [NanoBlockExplorer] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [NanoBlockExplorer] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [NanoBlockExplorer] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [NanoBlockExplorer] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [NanoBlockExplorer] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [NanoBlockExplorer] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [NanoBlockExplorer] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [NanoBlockExplorer] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [NanoBlockExplorer] SET  DISABLE_BROKER 
GO
ALTER DATABASE [NanoBlockExplorer] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [NanoBlockExplorer] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [NanoBlockExplorer] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [NanoBlockExplorer] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [NanoBlockExplorer] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [NanoBlockExplorer] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [NanoBlockExplorer] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [NanoBlockExplorer] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [NanoBlockExplorer] SET  MULTI_USER 
GO
ALTER DATABASE [NanoBlockExplorer] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [NanoBlockExplorer] SET DB_CHAINING OFF 
GO
ALTER DATABASE [NanoBlockExplorer] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [NanoBlockExplorer] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [NanoBlockExplorer]
GO
/****** Object:  Table [dbo].[Block]    Script Date: 9/29/2016 11:27:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Block](
	[Hash] [varchar](64) NOT NULL,
	[PreviousBlockHash] [varchar](64) NULL,
	[MerkelRootHash] [varchar](64) NOT NULL,
	[Version] [int] NOT NULL,
	[Time] [int] NOT NULL,
	[Bits] [varchar](50) NOT NULL,
	[Nounce] [bigint] NOT NULL,
	[Size] [int] NOT NULL,
	[Height] [int] NOT NULL,
	[Difficulty] [decimal](18, 8) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_Block_1] PRIMARY KEY CLUSTERED 
(
	[Hash] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Block_Node]    Script Date: 9/29/2016 11:27:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Block_Node](
	[BlockHash] [varchar](64) NOT NULL,
	[NodeId] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_Block_Node] PRIMARY KEY CLUSTERED 
(
	[BlockHash] ASC,
	[NodeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BlockTransaction]    Script Date: 9/29/2016 11:27:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BlockTransaction](
	[BlockHash] [varchar](64) NOT NULL,
	[TxId] [varchar](64) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_BlockTransaction] PRIMARY KEY CLUSTERED 
(
	[BlockHash] ASC,
	[TxId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Node]    Script Date: 9/29/2016 11:27:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Node](
	[Id] [int] NOT NULL,
	[Name] [varchar](250) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_Node] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Transaction]    Script Date: 9/29/2016 11:27:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Transaction](
	[TxId] [varchar](64) NOT NULL,
	[Size] [int] NOT NULL,
	[Version] [bigint] NOT NULL,
	[LockTime] [bigint] NOT NULL,
	[Time] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED 
(
	[TxId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TransactionInput]    Script Date: 9/29/2016 11:27:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TransactionInput](
	[TxId] [varchar](64) NOT NULL,
	[InputTxId] [varchar](64) NULL,
	[InputVOut] [int] NULL,
	[CreatedOn] [datetime] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TransactionOutput]    Script Date: 9/29/2016 11:27:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TransactionOutput](
	[TxId] [varchar](64) NOT NULL,
	[VOut] [int] NOT NULL,
	[Address] [varchar](64) NULL,
	[Satoshi] [bigint] NOT NULL,
	[Script] [varchar](max) NULL,
	[Type] [varchar](50) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_TransactionOutput] PRIMARY KEY CLUSTERED 
(
	[TxId] ASC,
	[VOut] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Transaction]    Script Date: 9/29/2016 11:27:08 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Transaction] ON [dbo].[Transaction]
(
	[TxId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_TransactionInput]    Script Date: 9/29/2016 11:27:08 PM ******/
CREATE NONCLUSTERED INDEX [IX_TransactionInput] ON [dbo].[TransactionInput]
(
	[TxId] ASC,
	[InputTxId] ASC,
	[InputVOut] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Block] ADD  CONSTRAINT [DF_Block_CreatedOn]  DEFAULT (getutcdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Block_Node] ADD  CONSTRAINT [DF_Block_Node_CreatedOn]  DEFAULT (getutcdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Node] ADD  CONSTRAINT [DF_Node_CreatedOn]  DEFAULT (getutcdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Transaction] ADD  CONSTRAINT [DF_Transaction_CreatedOn]  DEFAULT (getutcdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[TransactionInput] ADD  CONSTRAINT [DF_TransactionInput_TxId]  DEFAULT (getutcdate()) FOR [TxId]
GO
ALTER TABLE [dbo].[Block]  WITH CHECK ADD  CONSTRAINT [FK_Block_Block] FOREIGN KEY([PreviousBlockHash])
REFERENCES [dbo].[Block] ([Hash])
GO
ALTER TABLE [dbo].[Block] CHECK CONSTRAINT [FK_Block_Block]
GO
ALTER TABLE [dbo].[Block_Node]  WITH CHECK ADD  CONSTRAINT [FK_Block_Node_Block] FOREIGN KEY([BlockHash])
REFERENCES [dbo].[Block] ([Hash])
GO
ALTER TABLE [dbo].[Block_Node] CHECK CONSTRAINT [FK_Block_Node_Block]
GO
ALTER TABLE [dbo].[Block_Node]  WITH CHECK ADD  CONSTRAINT [FK_Block_Node_Node] FOREIGN KEY([NodeId])
REFERENCES [dbo].[Node] ([Id])
GO
ALTER TABLE [dbo].[Block_Node] CHECK CONSTRAINT [FK_Block_Node_Node]
GO
ALTER TABLE [dbo].[BlockTransaction]  WITH CHECK ADD  CONSTRAINT [FK_BlockTransaction_Block] FOREIGN KEY([BlockHash])
REFERENCES [dbo].[Block] ([Hash])
GO
ALTER TABLE [dbo].[BlockTransaction] CHECK CONSTRAINT [FK_BlockTransaction_Block]
GO
ALTER TABLE [dbo].[BlockTransaction]  WITH CHECK ADD  CONSTRAINT [FK_BlockTransaction_Transaction] FOREIGN KEY([TxId])
REFERENCES [dbo].[Transaction] ([TxId])
GO
ALTER TABLE [dbo].[BlockTransaction] CHECK CONSTRAINT [FK_BlockTransaction_Transaction]
GO
ALTER TABLE [dbo].[TransactionInput]  WITH CHECK ADD  CONSTRAINT [FK_TransactionInput_TransactionOutput] FOREIGN KEY([InputTxId])
REFERENCES [dbo].[Transaction] ([TxId])
GO
ALTER TABLE [dbo].[TransactionInput] CHECK CONSTRAINT [FK_TransactionInput_TransactionOutput]
GO
ALTER TABLE [dbo].[TransactionInput]  WITH CHECK ADD  CONSTRAINT [FK_TransactionInput_TransactionOutput1] FOREIGN KEY([InputTxId], [InputVOut])
REFERENCES [dbo].[TransactionOutput] ([TxId], [VOut])
GO
ALTER TABLE [dbo].[TransactionInput] CHECK CONSTRAINT [FK_TransactionInput_TransactionOutput1]
GO
ALTER TABLE [dbo].[TransactionOutput]  WITH CHECK ADD  CONSTRAINT [FK_TransactionOutput_Transaction] FOREIGN KEY([TxId])
REFERENCES [dbo].[Transaction] ([TxId])
GO
ALTER TABLE [dbo].[TransactionOutput] CHECK CONSTRAINT [FK_TransactionOutput_Transaction]
GO
USE [master]
GO
ALTER DATABASE [NanoBlockExplorer] SET  READ_WRITE 
GO
