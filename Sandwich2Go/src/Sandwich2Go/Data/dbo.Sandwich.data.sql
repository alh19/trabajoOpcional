SET IDENTITY_INSERT [dbo].[Sandwich] ON
INSERT INTO [dbo].[Sandwich] ([Id], [SandwichName], [Precio], [Desc], [Discriminator], [Cantidad]) VALUES (1, N'Mixto', 3, N'saojdjioa', N'Sandwich', NULL)
INSERT INTO [dbo].[Sandwich] ([Id], [SandwichName], [Precio], [Desc], [Discriminator], [Cantidad]) VALUES (2, N'Cubano', 4, N'iadfsiasd', N'Sandwich', NULL)
INSERT INTO [dbo].[Sandwich] ([Id], [SandwichName], [Precio], [Desc], [Discriminator], [Cantidad]) VALUES (3, N'Submarino', 5, N'doijqoid', N'Sandwich', NULL)
INSERT INTO [dbo].[Sandwich] ([Id], [SandwichName], [Precio], [Desc], [Discriminator], [Cantidad]) VALUES (4, N'Inglés', 5, N'sfooijdf', N'Sandwich', NULL)
INSERT INTO [dbo].[Sandwich] ([Id], [SandwichName], [Precio], [Desc], [Discriminator], [Cantidad]) VALUES (5, N'Mixto sin Gúten', 4, N'sfioasdi', N'Sandwich', NULL)
SET IDENTITY_INSERT [dbo].[Sandwich] OFF
