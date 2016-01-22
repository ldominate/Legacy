
CREATE PROCEDURE [Entity].[UpdateOperation]

	@Id			INT,
	@GroupId	INT = NULL,
	@Type		INT,
	@Name		NVARCHAR(MAX) = NULL

AS
BEGIN

	UPDATE [Entity].[Operation] SET [GroupId] = @GroupId, [Type] = @Type, [Name] = @Name
	WHERE [Id] = @Id

END