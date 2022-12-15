SET IDENTITY_INSERT [dbo].[Ingrediente] ON
UPDATE [dbo].[Ingrediente] SET Stock=10 WHERE Id=1 
SET IDENTITY_INSERT [dbo].[Ingrediente] OFF


SET IDENTITY_INSERT [dbo].[Ingrediente] ON
UPDATE [dbo].[Ingrediente] SET Stock=100 WHERE Id=4
SET IDENTITY_INSERT [dbo].[Ingrediente] OFF
