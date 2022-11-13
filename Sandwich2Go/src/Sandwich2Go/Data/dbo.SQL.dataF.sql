﻿SET IDENTITY_INSERT [dbo].[Alergeno] ON
INSERT INTO [dbo].[Alergeno] ([id], [Name]) VALUES (1, N'Huevo')
INSERT INTO [dbo].[Alergeno] ([id], [Name]) VALUES (2, N'Leche')
INSERT INTO [dbo].[Alergeno] ([id], [Name]) VALUES (3, N'Glúten')
SET IDENTITY_INSERT [dbo].[Alergeno] OFF

SET IDENTITY_INSERT [dbo].[Ingrediente] ON
INSERT INTO [dbo].[Ingrediente] ([Id], [Nombre], [PrecioUnitario], [Stock]) VALUES (1, N'Queso', 1, 10)
INSERT INTO [dbo].[Ingrediente] ([Id], [Nombre], [PrecioUnitario], [Stock]) VALUES (2, N'Pepinillo', 2, 0)
INSERT INTO [dbo].[Ingrediente] ([Id], [Nombre], [PrecioUnitario], [Stock]) VALUES (3, N'Pan', 1, 100)
INSERT INTO [dbo].[Ingrediente] ([Id], [Nombre], [PrecioUnitario], [Stock]) VALUES (4, N'Huevo', 1, 100)
INSERT INTO [dbo].[Ingrediente] ([Id], [Nombre], [PrecioUnitario], [Stock]) VALUES (5, N'Jamon', 1, 100)
INSERT INTO [dbo].[Ingrediente] ([Id], [Nombre], [PrecioUnitario], [Stock]) VALUES (6, N'Pan sin Glúten', 2, 100)
SET IDENTITY_INSERT [dbo].[Ingrediente] OFF

SET IDENTITY_INSERT [dbo].[Sandwich] ON
INSERT INTO [dbo].[Sandwich] ([Id], [SandwichName], [Precio], [Desc], [Discriminator], [Cantidad]) VALUES (1, N'Mixto', 3, N'saojdjioa', N'Sandwich', NULL)
INSERT INTO [dbo].[Sandwich] ([Id], [SandwichName], [Precio], [Desc], [Discriminator], [Cantidad]) VALUES (2, N'Cubano', 4, N'iadfsiasd', N'Sandwich', NULL)
INSERT INTO [dbo].[Sandwich] ([Id], [SandwichName], [Precio], [Desc], [Discriminator], [Cantidad]) VALUES (3, N'Submarino', 5, N'doijqoid', N'Sandwich', NULL)
INSERT INTO [dbo].[Sandwich] ([Id], [SandwichName], [Precio], [Desc], [Discriminator], [Cantidad]) VALUES (4, N'Inglés', 5, N'sfooijdf', N'Sandwich', NULL)
INSERT INTO [dbo].[Sandwich] ([Id], [SandwichName], [Precio], [Desc], [Discriminator], [Cantidad]) VALUES (5, N'Mixto sin Gúten', 4, N'sfioasdi', N'Sandwich', NULL)
SET IDENTITY_INSERT [dbo].[Sandwich] OFF

SET IDENTITY_INSERT [dbo].[Proveedor] ON
INSERT INTO [dbo].[Proveedor] ([Id], [Cif], [Nombre], [Direccion]) VALUES (1, N'11111a', N'Alberto', N'Calle1')
INSERT INTO [dbo].[Proveedor] ([Id], [Cif], [Nombre], [Direccion]) VALUES (2, N'22222b', N'Maria', N'Calle2')
INSERT INTO [dbo].[Proveedor] ([Id], [Cif], [Nombre], [Direccion]) VALUES (3, N'33333c', N'Luis', N'Calle3')
INSERT INTO [dbo].[Proveedor] ([Id], [Cif], [Nombre], [Direccion]) VALUES (4, N'44444d', N'Sara', N'Calle1')
INSERT INTO [dbo].[Proveedor] ([Id], [Cif], [Nombre], [Direccion]) VALUES (5, N'55555e', N'Pepe', N'Calle4')
INSERT INTO [dbo].[Proveedor] ([Id], [Cif], [Nombre], [Direccion]) VALUES (6, N'66666f', N'Ana', N'Calle5')
SET IDENTITY_INSERT [dbo].[Proveedor] OFF

INSERT INTO [dbo].[AlergSandws] ([AlergenoId], [IngredienteId], [Id]) VALUES (2, 1, 2)
INSERT INTO [dbo].[AlergSandws] ([AlergenoId], [IngredienteId], [Id]) VALUES (3, 3, 3)
INSERT INTO [dbo].[AlergSandws] ([AlergenoId], [IngredienteId], [Id]) VALUES (1, 4, 1)

INSERT INTO [dbo].[IngredienteSandwich] ([IngredienteId], [SandwichId], [Id], [Cantidad]) VALUES (1, 1, 1, 1)
INSERT INTO [dbo].[IngredienteSandwich] ([IngredienteId], [SandwichId], [Id], [Cantidad]) VALUES (1, 2, 2, 1)
INSERT INTO [dbo].[IngredienteSandwich] ([IngredienteId], [SandwichId], [Id], [Cantidad]) VALUES (1, 3, 3, 1)
INSERT INTO [dbo].[IngredienteSandwich] ([IngredienteId], [SandwichId], [Id], [Cantidad]) VALUES (1, 4, 4, 1)
INSERT INTO [dbo].[IngredienteSandwich] ([IngredienteId], [SandwichId], [Id], [Cantidad]) VALUES (1, 5, 5, 1)
INSERT INTO [dbo].[IngredienteSandwich] ([IngredienteId], [SandwichId], [Id], [Cantidad]) VALUES (2, 3, 6, 1)
INSERT INTO [dbo].[IngredienteSandwich] ([IngredienteId], [SandwichId], [Id], [Cantidad]) VALUES (3, 1, 7, 1)
INSERT INTO [dbo].[IngredienteSandwich] ([IngredienteId], [SandwichId], [Id], [Cantidad]) VALUES (3, 2, 8, 1)
INSERT INTO [dbo].[IngredienteSandwich] ([IngredienteId], [SandwichId], [Id], [Cantidad]) VALUES (3, 3, 9, 1)
INSERT INTO [dbo].[IngredienteSandwich] ([IngredienteId], [SandwichId], [Id], [Cantidad]) VALUES (3, 4, 10, 1)
INSERT INTO [dbo].[IngredienteSandwich] ([IngredienteId], [SandwichId], [Id], [Cantidad]) VALUES (4, 2, 11, 1)
INSERT INTO [dbo].[IngredienteSandwich] ([IngredienteId], [SandwichId], [Id], [Cantidad]) VALUES (4, 3, 12, 1)
INSERT INTO [dbo].[IngredienteSandwich] ([IngredienteId], [SandwichId], [Id], [Cantidad]) VALUES (4, 4, 13, 1)
INSERT INTO [dbo].[IngredienteSandwich] ([IngredienteId], [SandwichId], [Id], [Cantidad]) VALUES (5, 1, 14, 1)
INSERT INTO [dbo].[IngredienteSandwich] ([IngredienteId], [SandwichId], [Id], [Cantidad]) VALUES (5, 2, 15, 1)
INSERT INTO [dbo].[IngredienteSandwich] ([IngredienteId], [SandwichId], [Id], [Cantidad]) VALUES (5, 3, 16, 1)
INSERT INTO [dbo].[IngredienteSandwich] ([IngredienteId], [SandwichId], [Id], [Cantidad]) VALUES (5, 4, 17, 1)
INSERT INTO [dbo].[IngredienteSandwich] ([IngredienteId], [SandwichId], [Id], [Cantidad]) VALUES (5, 5, 18, 1)
INSERT INTO [dbo].[IngredienteSandwich] ([IngredienteId], [SandwichId], [Id], [Cantidad]) VALUES (6, 5, 19, 1)

INSERT INTO [dbo].[IngrProv] ([Id], [IngredienteId], [ProveedorId]) VALUES (1, 1, 1)
INSERT INTO [dbo].[IngrProv] ([Id], [IngredienteId], [ProveedorId]) VALUES (2, 2, 1)
INSERT INTO [dbo].[IngrProv] ([Id], [IngredienteId], [ProveedorId]) VALUES (3, 5, 1)
INSERT INTO [dbo].[IngrProv] ([Id], [IngredienteId], [ProveedorId]) VALUES (4, 6, 2)
INSERT INTO [dbo].[IngrProv] ([Id], [IngredienteId], [ProveedorId]) VALUES (5, 5, 2)
INSERT INTO [dbo].[IngrProv] ([Id], [IngredienteId], [ProveedorId]) VALUES (6, 3, 2)
INSERT INTO [dbo].[IngrProv] ([Id], [IngredienteId], [ProveedorId]) VALUES (7, 1, 3)
INSERT INTO [dbo].[IngrProv] ([Id], [IngredienteId], [ProveedorId]) VALUES (8, 2, 3)
INSERT INTO [dbo].[IngrProv] ([Id], [IngredienteId], [ProveedorId]) VALUES (9, 3, 3)
INSERT INTO [dbo].[IngrProv] ([Id], [IngredienteId], [ProveedorId]) VALUES (10, 4, 3)
INSERT INTO [dbo].[IngrProv] ([Id], [IngredienteId], [ProveedorId]) VALUES (11, 5, 3)
INSERT INTO [dbo].[IngrProv] ([Id], [IngredienteId], [ProveedorId]) VALUES (12, 6, 3)
INSERT INTO [dbo].[IngrProv] ([Id], [IngredienteId], [ProveedorId]) VALUES (13, 2, 4)
INSERT INTO [dbo].[IngrProv] ([Id], [IngredienteId], [ProveedorId]) VALUES (14, 6, 4)
INSERT INTO [dbo].[IngrProv] ([Id], [IngredienteId], [ProveedorId]) VALUES (15, 4, 5)
INSERT INTO [dbo].[IngrProv] ([Id], [IngredienteId], [ProveedorId]) VALUES (16, 5, 5)