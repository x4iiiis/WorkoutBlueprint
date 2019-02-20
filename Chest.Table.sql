CREATE TABLE [dbo].[Table]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Exercise] NVARCHAR(50) NOT NULL, 
    [MuscleGroup] NVARCHAR(50) NOT NULL, 
    [SpecificTarget] NVARCHAR(50) NULL, 
    [IsCompound] BIT NOT NULL
)
