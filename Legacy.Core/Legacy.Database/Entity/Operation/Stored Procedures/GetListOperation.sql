CREATE PROCEDURE [Entity].[GetListOperation]

	@startIndex		INT,
	@count			INT,
	@GroupId		INT = NULL,
	@Type			INT = NULL,
	@Name			NVARCHAR(MAX) = NULL,
	@IsDeleted		BIT = NULL,
	@Order			BIT = 0,
	@PropertySort	NVARCHAR(MAX) = NULL

AS
BEGIN

	SELECT COUNT(*) AS allCount
	FROM [Entity].[Operation]
	WHERE (@IsDeleted IS NULL OR [IsDeleted] = @IsDeleted)
		AND (@GroupId IS NULL OR ISNULL([GroupId], 0) = @GroupId)
		AND (@Type IS NULL OR ISNULL([Type], 0) = @Type)
		AND (@Name IS NULL OR [Name] LIKE @Name)

	SELECT *
	FROM [Entity].[Operation]
	WHERE (@IsDeleted IS NULL OR [IsDeleted] = @IsDeleted)
		AND (@GroupId IS NULL OR ISNULL([GroupId], 0) = @GroupId)
		AND (@Type IS NULL OR ISNULL([Type], 0) = @Type)
		AND (@Name IS NULL OR [Name] LIKE @Name)
	ORDER BY
	CASE WHEN @Order = 1 THEN
		CASE LOWER(@PropertySort)
			WHEN 'name'	THEN CAST([Name] AS sql_variant)
			WHEN 'type'	THEN CAST([Type] AS sql_variant)
						ELSE CAST([Name] AS sql_variant)
		END
	END DESC,
	CASE WHEN @Order = 0 THEN
		CASE LOWER(@PropertySort)
			WHEN 'name'	THEN CAST([Name] AS sql_variant)
			WHEN 'type'	THEN CAST([Type] AS sql_variant)
						ELSE CAST([Name] AS sql_variant)
		END
	END ASC
	OFFSET @startIndex ROWS
	FETCH NEXT @count ROWS ONLY


END