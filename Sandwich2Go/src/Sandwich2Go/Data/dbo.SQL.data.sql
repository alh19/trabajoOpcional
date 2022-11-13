SET IDENTITY_INSERT [dbo].[Alergeno] ON
INSERT INTO [dbo].[Alergeno] ([id], [Name]) VALUES (1, N'Gluten')
INSERT INTO [dbo].[Alergeno] ([id], [Name]) VALUES (2, N'Soja')
INSERT INTO [dbo].[Alergeno] ([id], [Name]) VALUES (3, N'Lactosa')
SET IDENTITY_INSERT [dbo].[Alergeno] OFF


SET IDENTITY_INSERT [dbo].[Ingrediente] ON
INSERT INTO [dbo].[Ingrediente] ([Id], [Nombre], [PrecioUnitario], [Stock]) VALUES (1, N'Lechuga', 2, 6)
INSERT INTO [dbo].[Ingrediente] ([Id], [Nombre], [PrecioUnitario], [Stock]) VALUES (2, N'Tomate', 6, 8)
INSERT INTO [dbo].[Ingrediente] ([Id], [Nombre], [PrecioUnitario], [Stock]) VALUES (3, N'Queso', 3, 9)
INSERT INTO [dbo].[Ingrediente] ([Id], [Nombre], [PrecioUnitario], [Stock]) VALUES (4, N'Pan', 1, 6)
SET IDENTITY_INSERT [dbo].[Ingrediente] OFF

SET IDENTITY_INSERT [dbo].[Proveedor] ON
INSERT INTO [dbo].[Proveedor] ([Id], [Cif], [Nombre], [Direccion]) VALUES (1, N'11111a', N'Alberto', N'Calle1')
INSERT INTO [dbo].[Proveedor] ([Id], [Cif], [Nombre], [Direccion]) VALUES (2, N'22222b', N'Maria', N'Calle2')
INSERT INTO [dbo].[Proveedor] ([Id], [Cif], [Nombre], [Direccion]) VALUES (3, N'33333c', N'Luis', N'Calle3')
INSERT INTO [dbo].[Proveedor] ([Id], [Cif], [Nombre], [Direccion]) VALUES (4, N'44444d', N'Sara', N'Calle1')
INSERT INTO [dbo].[Proveedor] ([Id], [Cif], [Nombre], [Direccion]) VALUES (5, N'55555e', N'Pepe', N'Calle4')
INSERT INTO [dbo].[Proveedor] ([Id], [Cif], [Nombre], [Direccion]) VALUES (6, N'66666f', N'Ana', N'Calle5')
SET IDENTITY_INSERT [dbo].[Proveedor] OFF

SET IDENTITY_INSERT [dbo].[AlergSandws] ON
INSERT INTO [dbo].[AlergSandws] ([AlergenoId], [IngredienteId], [Id]) VALUES (1, 1, 1)
INSERT INTO [dbo].[AlergSandws] ([AlergenoId], [IngredienteId], [Id]) VALUES (2, 3, 2)
INSERT INTO [dbo].[AlergSandws] ([AlergenoId], [IngredienteId], [Id]) VALUES (3, 3, 3)
SET IDENTITY_INSERT [dbo].[AlergSandws] OFF

SET IDENTITY_INSERT [dbo].[IngrProv] ON
INSERT INTO [dbo].[IngrProv] ([Id], [IngredienteId], [ProveedorId]) VALUES (1, 1, 1)
INSERT INTO [dbo].[IngrProv] ([Id], [IngredienteId], [ProveedorId]) VALUES (2, 2, 1)
INSERT INTO [dbo].[IngrProv] ([Id], [IngredienteId], [ProveedorId]) VALUES (3, 5, 1)
SET IDENTITY_INSERT [dbo].[IngrProv] OFF
