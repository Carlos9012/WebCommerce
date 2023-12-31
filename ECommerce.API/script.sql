USE master;
GO

IF NOT EXISTS(SELECT name FROM sys.databases WHERE name = 'AVT_BD')
BEGIN
    CREATE DATABASE AVT_BD;
    ALTER DATABASE AVT_BD SET RECOVERY SIMPLE;
END
GO

USE AVT_BD;
GO

-- Cria��o das Tabelas
CREATE TABLE [dbo].[__EFMigrationsHistory](
    [MigrationId] [nvarchar](150) NOT NULL,
    [ProductVersion] [nvarchar](32) NOT NULL,
    CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED ([MigrationId] ASC)
);

CREATE TABLE [dbo].[CartItems](
    [CartItemId] [int] IDENTITY(1,1) NOT NULL,
    [CartId] [int] NOT NULL,
    [ProductId] [int] NOT NULL,
    CONSTRAINT [PK_CartItems] PRIMARY KEY CLUSTERED ([CartItemId] ASC)
);

CREATE TABLE [dbo].[Carts](
    [CartId] [int] IDENTITY(1,1) NOT NULL,
    [UserId] [int] NOT NULL,
    [Ordered] [nvarchar](10) NOT NULL,
    [OrderedOn] [nvarchar](max) NOT NULL,
    CONSTRAINT [PK_Carts] PRIMARY KEY CLUSTERED ([CartId] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];

CREATE TABLE [dbo].[Offers](
    [OfferId] [int] IDENTITY(1,1) NOT NULL,
    [Title] [nvarchar](max) NOT NULL,
    [Discount] [int] NOT NULL,
    CONSTRAINT [PK_Offers] PRIMARY KEY CLUSTERED ([OfferId] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];

CREATE TABLE [dbo].[Orders](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [UserId] [int] NOT NULL,
    [CartId] [int] NOT NULL,
    [PaymentId] [int] NOT NULL,
    [CreatedAt] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];

CREATE TABLE [dbo].[PaymentMethods](
    [PaymentMethodId] [int] IDENTITY(1,1) NOT NULL,
    [Type] [nvarchar](max) NULL,
    [Provider] [nvarchar](max) NULL,
    [Available] [nvarchar](50) NULL,
    [Reason] [nvarchar](max) NULL,
 CONSTRAINT [PK_PaymentMethods] PRIMARY KEY CLUSTERED ([PaymentMethodId] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];

CREATE TABLE [dbo].[Payments](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [UserId] [int] NOT NULL,
    [PaymentMethodId] [int] NOT NULL,
    [TotalAmount] [int] NOT NULL,
    [ShippingCharges] [int] NOT NULL,
    [AmountReduced] [int] NOT NULL,
    [AmountPaid] [int] NOT NULL,
    [CreatedAt] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Payments] PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];

CREATE TABLE [dbo].[ProductCategories](
    [CategoryId] [int] IDENTITY(1,1) NOT NULL,
    [Category] [nvarchar](50) NOT NULL,
    [SubCategory] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ProductCategories] PRIMARY KEY CLUSTERED ([CategoryId] ASC)
) ON [PRIMARY];

CREATE TABLE [dbo].[Products](
    [ProductId] [int] IDENTITY(1,1) NOT NULL,
    [Title] [nvarchar](max) NOT NULL,
    [description_] [nvarchar](max) NOT NULL,
    [CategoryId] [int] NOT NULL,
    [OfferId] [int] NOT NULL,
    [Price] [float] NOT NULL,
    [Quantity] [int] NOT NULL,
    [ImageName] [nvarchar](max) NOT NULL,
    [nota] [float] NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED ([ProductId] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];

CREATE TABLE [dbo].[Reviews](
    [ReviewId] [int] IDENTITY(1,1) NOT NULL,
    [UserId] [int] NOT NULL,
    [ProductId] [int] NOT NULL,
    [Review] [nvarchar](max) NOT NULL,
    [CreatedAt] [nvarchar](100) NOT NULL,
    [nota] [int] NULL,
 CONSTRAINT [PK_Reviews] PRIMARY KEY CLUSTERED ([ReviewId] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];

CREATE TABLE [dbo].[Users](
    [UserId] [int] IDENTITY(1,1) NOT NULL,
    [FirstName] [nvarchar](50) NOT NULL,
    [LastName] [nvarchar](50) NOT NULL,
    [Email] [nvarchar](100) NOT NULL,
    [Address] [nvarchar](100) NOT NULL,
    [Mobile] [nvarchar](15) NOT NULL,
    [Password] [nvarchar](50) NOT NULL,
    [CreatedAt] [nvarchar](max) NOT NULL,
    [ModifiedAt] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserId] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];

-- Adicionando Restri��es de Chave Estrangeira
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Carts] FOREIGN KEY([CartId])
REFERENCES [dbo].[Carts] ([CartId]);

ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Carts];

ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Payments] FOREIGN KEY([PaymentId])
REFERENCES [dbo].[Payments] ([Id]);

ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Payments];

ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId]);

ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Users];

ALTER TABLE [dbo].[Payments]  WITH CHECK ADD  CONSTRAINT [FK_Payments_PaymentMethods] FOREIGN KEY([PaymentMethodId])
REFERENCES [dbo].[PaymentMethods] ([PaymentMethodId]);

ALTER TABLE [dbo].[Payments] CHECK CONSTRAINT [FK_Payments_PaymentMethods];

ALTER TABLE [dbo].[Payments]  WITH CHECK ADD  CONSTRAINT [FK_Payments_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId]);

ALTER TABLE [dbo].[Payments] CHECK CONSTRAINT [FK_Payments_Users];

ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Product_Offers] FOREIGN KEY([OfferId])
REFERENCES [dbo].[Offers] ([OfferId]);

ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Product_Offers];

ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Product_ProductCategories] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[ProductCategories] ([CategoryId]);

ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Product_ProductCategories];

ALTER TABLE [dbo].[Reviews]  WITH CHECK ADD  CONSTRAINT [FK_Reviews_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId]);

ALTER TABLE [dbo].[Reviews] CHECK CONSTRAINT [FK_Reviews_Users];
