CREATE TABLE [Entity].[Operation]
(
	[Id]		INT IDENTITY(1, 1) NOT NULL,
	[GroupId]	INT NULL,
	[Type]		INT NOT NULL DEFAULT 0,
	[Level]		INT NOT NULL DEFAULT 0,
	[Name]		NVARCHAR(255) NULL,
	[Order]		INT NOT NULL DEFAULT 0,
	[IsDeleted]	BIT NOT NULL DEFAULT 0,

	CONSTRAINT [PK_Operation] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Operation_Group] FOREIGN KEY ([GroupId]) REFERENCES [Entity].[Operation](Id)
)

GO

CREATE INDEX [IX_Operation_Group] ON [Entity].[Operation] ([GroupId])
