USE [BSWeb]
GO
/****** Object:  Table [dbo].[USERS]    Script Date: 01/30/2018 10:52:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USERS](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserNo] [nvarchar](200) NOT NULL,
	[NAME] [nvarchar](200) NULL,
	[Role] [nvarchar](200) NULL,
	[Password] [nvarchar](500) NULL,
	[Sex] [int] NULL,
	[OrgID] [int] NULL,
	[IDCard] [nvarchar](50) NULL,
	[PhoneNumber] [nvarchar](50) NULL,
	[Address] [nvarchar](200) NULL,
	[Email] [nvarchar](50) NULL,
	[State] [int] NULL,
	[Operator] [nvarchar](50) NULL,
	[InDate] [datetime] NULL,
	[MGuid] [uniqueidentifier] NULL,
 CONSTRAINT [PK_USERS_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'流水号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'账号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS', @level2type=N'COLUMN',@level2name=N'UserNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS', @level2type=N'COLUMN',@level2name=N'NAME'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色:1管理员,2分发员,3投递员,4财务' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS', @level2type=N'COLUMN',@level2name=N'Role'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS', @level2type=N'COLUMN',@level2name=N'Password'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'性别,1男 ,0女' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS', @level2type=N'COLUMN',@level2name=N'Sex'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属机构ID,关联Org表流水号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS', @level2type=N'COLUMN',@level2name=N'OrgID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'证件号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS', @level2type=N'COLUMN',@level2name=N'IDCard'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手机号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS', @level2type=N'COLUMN',@level2name=N'PhoneNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS', @level2type=N'COLUMN',@level2name=N'Address'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'邮箱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS', @level2type=N'COLUMN',@level2name=N'Email'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态,0有效,1无效' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS', @level2type=N'COLUMN',@level2name=N'State'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS', @level2type=N'COLUMN',@level2name=N'Operator'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS', @level2type=N'COLUMN',@level2name=N'InDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'唯一标识,修改时用到,做并发验证' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'USERS', @level2type=N'COLUMN',@level2name=N'MGuid'
GO
/****** Object:  Table [dbo].[Role]    Script Date: 01/30/2018 10:52:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NULL,
	[Rights] [nvarchar](500) NULL,
	[Remark] [nvarchar](500) NULL,
	[State] [int] NULL,
	[NGuid] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Picture]    Script Date: 01/30/2018 10:52:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Picture](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](200) NULL,
	[FileName] [nvarchar](200) NULL,
	[FilePath] [nvarchar](200) NULL,
	[FileSize] [decimal](18, 2) NOT NULL,
	[FileExtension] [nvarchar](10) NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Picture] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Org]    Script Date: 01/30/2018 10:52:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Org](
	[OrgID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Phone] [nvarchar](50) NULL,
	[Address] [nvarchar](200) NULL,
	[Email] [nvarchar](50) NULL,
	[PostCode] [int] NULL,
	[ParentID] [int] NULL,
	[InDate] [date] NULL,
	[InUser] [nvarchar](50) NULL,
	[OrgCode] [nvarchar](50) NULL,
	[Level] [int] NULL,
 CONSTRAINT [PK_Org] PRIMARY KEY CLUSTERED 
(
	[OrgID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'机构流水号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Org', @level2type=N'COLUMN',@level2name=N'OrgID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'机构名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Org', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'电话' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Org', @level2type=N'COLUMN',@level2name=N'Phone'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Org', @level2type=N'COLUMN',@level2name=N'Address'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'邮箱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Org', @level2type=N'COLUMN',@level2name=N'Email'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'邮政编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Org', @level2type=N'COLUMN',@level2name=N'PostCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上级ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Org', @level2type=N'COLUMN',@level2name=N'ParentID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Org', @level2type=N'COLUMN',@level2name=N'InDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Org', @level2type=N'COLUMN',@level2name=N'InUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'机构代码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Org', @level2type=N'COLUMN',@level2name=N'OrgCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'机构级别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Org', @level2type=N'COLUMN',@level2name=N'Level'
GO
/****** Object:  Table [dbo].[OrderPeople]    Script Date: 01/30/2018 10:52:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderPeople](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Phone] [nvarchar](50) NULL,
	[Address] [nvarchar](200) NULL,
	[OrgID] [int] NULL,
	[Indate] [date] NULL,
	[InUser] [nvarchar](50) NULL,
	[UnitName] [nvarchar](50) NULL,
	[OrderNo] [nvarchar](50) NULL,
	[MGuid] [uniqueidentifier] NULL,
 CONSTRAINT [PK_OrderPeople] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 01/30/2018 10:52:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BKDH] [nvarchar](50) NOT NULL,
	[PersonID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[OrderNum] [int] NOT NULL,
	[OrderDate] [date] NULL,
	[PosterID] [int] NULL,
	[State] [int] NULL,
	[Indate] [date] NULL,
	[ModifyUser] [nvarchar](50) NULL,
	[ModifyDate] [date] NULL,
	[NGuid] [uniqueidentifier] NULL,
	[OrderMonths] [int] NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单流水号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'报刊代号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order', @level2type=N'COLUMN',@level2name=N'BKDH'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订户ID,关联OrderPeople表流水号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order', @level2type=N'COLUMN',@level2name=N'PersonID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户ID,新增人ID,关联Users表ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order', @level2type=N'COLUMN',@level2name=N'UserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订购数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order', @level2type=N'COLUMN',@level2name=N'OrderNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'起订日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order', @level2type=N'COLUMN',@level2name=N'OrderDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'投递员ID,关联users表流水号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order', @level2type=N'COLUMN',@level2name=N'PosterID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订购状态:0已缴清,1未缴清,-1退订未处理,-2退订已处理' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order', @level2type=N'COLUMN',@level2name=N'State'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'插入时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order', @level2type=N'COLUMN',@level2name=N'Indate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order', @level2type=N'COLUMN',@level2name=N'ModifyUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order', @level2type=N'COLUMN',@level2name=N'ModifyDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'唯一标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order', @level2type=N'COLUMN',@level2name=N'NGuid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订购月数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order', @level2type=N'COLUMN',@level2name=N'OrderMonths'
GO
/****** Object:  Table [dbo].[MenuClass]    Script Date: 01/30/2018 10:52:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MenuClass](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[ParentID] [int] NULL,
	[ParentName] [nvarchar](50) NULL,
	[Remark] [nvarchar](500) NULL,
	[Status] [int] NULL,
	[Sort] [int] NULL,
	[CreateTime] [datetime] NULL,
 CONSTRAINT [PK_MenuClass] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Menu]    Script Date: 01/30/2018 10:52:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Menu](
	[ID] [int] NOT NULL,
	[MenuName] [nvarchar](50) NULL,
	[ParentID] [int] NULL,
	[ParentName] [nvarchar](50) NULL,
	[Action] [nvarchar](50) NULL,
	[Controller] [nvarchar](50) NULL,
	[Remark] [nvarchar](500) NULL,
	[Icon] [nvarchar](20) NULL,
	[Status] [int] NULL,
	[Sort] [int] NULL,
	[CreateTime] [date] NULL,
 CONSTRAINT [PK_Menu] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Log]    Script Date: 01/30/2018 10:52:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Log](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NOT NULL,
	[Date] [date] NOT NULL,
	[NianJuanQi] [nvarchar](50) NULL,
	[type] [int] NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关联订购表主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Log', @level2type=N'COLUMN',@level2name=N'OrderID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订购分发时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Log', @level2type=N'COLUMN',@level2name=N'Date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分发的报纸年卷期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Log', @level2type=N'COLUMN',@level2name=N'NianJuanQi'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0代表已经被市分发过了,1表示被县分发过了' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Log', @level2type=N'COLUMN',@level2name=N'type'
GO
/****** Object:  Table [dbo].[DocType]    Script Date: 01/30/2018 10:52:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocType](
	[TypeID] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [nvarchar](50) NULL,
	[TypeDesc] [nvarchar](500) NULL,
	[NGuid] [uniqueidentifier] NULL,
 CONSTRAINT [PK_DocType] PRIMARY KEY CLUSTERED 
(
	[TypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Doc]    Script Date: 01/30/2018 10:52:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Doc](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TypeID] [int] NULL,
	[Name] [nvarchar](50) NULL,
	[ISSN] [nvarchar](50) NULL,
	[PublishArea] [nvarchar](50) NULL,
	[Publisher] [nvarchar](50) NULL,
	[PublishYear] [int] NULL,
	[C200F] [nvarchar](50) NULL,
	[Price] [numeric](16, 2) NULL,
	[PL] [nvarchar](50) NULL,
	[BKDH] [nvarchar](50) NULL,
	[AddPerson] [nvarchar](200) NULL,
	[AddDate] [date] NULL,
	[NGuid] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Doc] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cost]    Script Date: 01/30/2018 10:52:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cost](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NULL,
	[OrderUnitNO] [nvarchar](50) NULL,
	[Money] [decimal](18, 2) NULL,
	[MoneyPayed] [decimal](18, 2) NULL,
	[UpdateTime] [date] NULL,
	[UpdateUser] [int] NULL,
	[State] [int] NULL,
 CONSTRAINT [PK_Cost] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cost', @level2type=N'COLUMN',@level2name=N'OrderID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订户编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cost', @level2type=N'COLUMN',@level2name=N'OrderUnitNO'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应缴费金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cost', @level2type=N'COLUMN',@level2name=N'Money'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'已交费用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cost', @level2type=N'COLUMN',@level2name=N'MoneyPayed'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上次更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cost', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上次更新用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cost', @level2type=N'COLUMN',@level2name=N'UpdateUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0:已交清;1:未缴清;-1退订未处理;-2退订已处理' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cost', @level2type=N'COLUMN',@level2name=N'State'
GO
/****** Object:  Default [DF_Cost_UpdateTime]    Script Date: 01/30/2018 10:52:52 ******/
ALTER TABLE [dbo].[Cost] ADD  CONSTRAINT [DF_Cost_UpdateTime]  DEFAULT (getdate()) FOR [UpdateTime]
GO
/****** Object:  Default [DF_Doc_AddDate]    Script Date: 01/30/2018 10:52:52 ******/
ALTER TABLE [dbo].[Doc] ADD  CONSTRAINT [DF_Doc_AddDate]  DEFAULT (getdate()) FOR [AddDate]
GO
/****** Object:  Default [DF_Doc_NGuid]    Script Date: 01/30/2018 10:52:52 ******/
ALTER TABLE [dbo].[Doc] ADD  CONSTRAINT [DF_Doc_NGuid]  DEFAULT (newid()) FOR [NGuid]
GO
/****** Object:  Default [DF_DocType_NGuid]    Script Date: 01/30/2018 10:52:52 ******/
ALTER TABLE [dbo].[DocType] ADD  CONSTRAINT [DF_DocType_NGuid]  DEFAULT (newid()) FOR [NGuid]
GO
/****** Object:  Default [DF_Menu_CreateTime]    Script Date: 01/30/2018 10:52:52 ******/
ALTER TABLE [dbo].[Menu] ADD  CONSTRAINT [DF_Menu_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
/****** Object:  Default [DF_MenuClass_CreateTime]    Script Date: 01/30/2018 10:52:52 ******/
ALTER TABLE [dbo].[MenuClass] ADD  CONSTRAINT [DF_MenuClass_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
/****** Object:  Default [DF_Order_Indate]    Script Date: 01/30/2018 10:52:52 ******/
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_Indate]  DEFAULT (getdate()) FOR [Indate]
GO
/****** Object:  Default [DF_Order_NGuid]    Script Date: 01/30/2018 10:52:52 ******/
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_NGuid]  DEFAULT (newid()) FOR [NGuid]
GO
/****** Object:  Default [DF_OrderPeople_Indate]    Script Date: 01/30/2018 10:52:52 ******/
ALTER TABLE [dbo].[OrderPeople] ADD  CONSTRAINT [DF_OrderPeople_Indate]  DEFAULT (getdate()) FOR [Indate]
GO
/****** Object:  Default [DF_OrderPeople_MGuid]    Script Date: 01/30/2018 10:52:52 ******/
ALTER TABLE [dbo].[OrderPeople] ADD  CONSTRAINT [DF_OrderPeople_MGuid]  DEFAULT (newid()) FOR [MGuid]
GO
/****** Object:  Default [DF_Role_State]    Script Date: 01/30/2018 10:52:52 ******/
ALTER TABLE [dbo].[Role] ADD  CONSTRAINT [DF_Role_State]  DEFAULT ((0)) FOR [State]
GO
/****** Object:  Default [DF_Role_NGuid]    Script Date: 01/30/2018 10:52:52 ******/
ALTER TABLE [dbo].[Role] ADD  CONSTRAINT [DF_Role_NGuid]  DEFAULT (newid()) FOR [NGuid]
GO
/****** Object:  Default [DF_USERS_Sex]    Script Date: 01/30/2018 10:52:52 ******/
ALTER TABLE [dbo].[USERS] ADD  CONSTRAINT [DF_USERS_Sex]  DEFAULT ((1)) FOR [Sex]
GO
/****** Object:  Default [DF_USERS_Address]    Script Date: 01/30/2018 10:52:52 ******/
ALTER TABLE [dbo].[USERS] ADD  CONSTRAINT [DF_USERS_Address]  DEFAULT (newid()) FOR [Address]
GO
/****** Object:  Default [DF_USERS_State]    Script Date: 01/30/2018 10:52:52 ******/
ALTER TABLE [dbo].[USERS] ADD  CONSTRAINT [DF_USERS_State]  DEFAULT ((0)) FOR [State]
GO
/****** Object:  Default [DF_USERS_MGuid]    Script Date: 01/30/2018 10:52:52 ******/
ALTER TABLE [dbo].[USERS] ADD  CONSTRAINT [DF_USERS_MGuid]  DEFAULT (newid()) FOR [MGuid]
GO
