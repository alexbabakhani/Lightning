CREATE TABLE [dbo].[TableTicker]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
    [Symbol] CHAR(6) NULL, 
    [Description] VARCHAR(50) NULL
)
