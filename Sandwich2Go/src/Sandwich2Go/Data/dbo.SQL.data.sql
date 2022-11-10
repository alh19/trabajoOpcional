SET IDENTITY_INSERT [dbo].[Alergeno] ON
INSERT INTO [dbo].[Alergeno] ([id], [Name]) VALUES (1, N'Gluten')
INSERT INTO [dbo].[Alergeno] ([id], [Name]) VALUES (2, N'Soja')
INSERT INTO [dbo].[Alergeno] ([id], [Name]) VALUES (3, N'Lactosa')
SET IDENTITY_INSERT [dbo].[Alergeno] OFF


INSERT INTO [dbo].[AlergSandws] ([AlergenoId], [IngredienteId], [Id]) VALUES (1, 1, 1)
INSERT INTO [dbo].[AlergSandws] ([AlergenoId], [IngredienteId], [Id]) VALUES (2, 3, 2)
INSERT INTO [dbo].[AlergSandws] ([AlergenoId], [IngredienteId], [Id]) VALUES (3, 3, 3)



SET IDENTITY_INSERT [dbo].[Ingrediente] ON
INSERT INTO [dbo].[Ingrediente] ([Id], [Nombre], [PrecioUnitario], [Stock]) VALUES (1, N'Lechuga', 2, 6)
INSERT INTO [dbo].[Ingrediente] ([Id], [Nombre], [PrecioUnitario], [Stock]) VALUES (2, N'Tomate', 6, 8)
INSERT INTO [dbo].[Ingrediente] ([Id], [Nombre], [PrecioUnitario], [Stock]) VALUES (3, N'Queso', 3, 9)
INSERT INTO [dbo].[Ingrediente] ([Id], [Nombre], [PrecioUnitario], [Stock]) VALUES (4, N'Pan', 1, 6)
SET IDENTITY_INSERT [dbo].[Ingrediente] OFF

