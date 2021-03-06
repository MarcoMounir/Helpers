USE [TestDB]
GO
/****** Object:  StoredProcedure [dbo].[sp_TestTableStoredProc]    Script Date: 3/31/2021 11:57:47 AM ******/
DROP PROCEDURE [dbo].[sp_TestTableStoredProc]
GO
/****** Object:  Table [dbo].[TestTable]    Script Date: 3/31/2021 11:57:47 AM ******/
DROP TABLE [dbo].[TestTable]
GO
/****** Object:  Table [dbo].[TestTable]    Script Date: 3/31/2021 11:57:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TestTable](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[MoneyColumn] [money] NOT NULL,
	[DecimalColumn] [decimal](18, 4) NOT NULL,
	[StringColumn] [nvarchar](100) NOT NULL,
	[CharColumn] [char](1) NOT NULL,
	[StringColumn2] [char](10) NOT NULL,
	[NullColumn] [int] NULL,
	[IntColumn] [int] NOT NULL,
	[EnumColumn] [tinyint] NOT NULL,
	[NullableIntColumn] [int] NULL,
	[NullableDecimalColumn] [money] NULL,
	[NullableEnumColumn] [tinyint] NULL,
	[NullableDateTimeColumn] [datetime] NULL,
	[DateTimeColumn] [datetime] NOT NULL,
	[BooleanColumn] [bit] NOT NULL,
	[NullableBooleanColumn] [bit] NULL,
 CONSTRAINT [PK_TestTable] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[TestTable] ON 

INSERT [dbo].[TestTable] ([Id], [MoneyColumn], [DecimalColumn], [StringColumn], [CharColumn], [StringColumn2], [NullColumn], [IntColumn], [EnumColumn], [NullableIntColumn], [NullableDecimalColumn], [NullableEnumColumn], [NullableDateTimeColumn], [DateTimeColumn], [BooleanColumn], [NullableBooleanColumn]) VALUES (1, 123.1230, CAST(123.4444 AS Decimal(18, 4)), N'Test String', N'T', N'Test Str  ', NULL, 99, 1, NULL, 23.1242, NULL, CAST(N'2021-03-30T16:24:19.040' AS DateTime), CAST(N'2021-03-30T16:21:24.333' AS DateTime), 0, NULL)
SET IDENTITY_INSERT [dbo].[TestTable] OFF
/****** Object:  StoredProcedure [dbo].[sp_TestTableStoredProc]    Script Date: 3/31/2021 11:57:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_TestTableStoredProc] 
@Id bigint
AS
BEGIN

if(@Id <> 0)
begin
	Select * from TestTable where Id = @Id
	end
else
begin
	Select * from TestTable
end
END
GO
