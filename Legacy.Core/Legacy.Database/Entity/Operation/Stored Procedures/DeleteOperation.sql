CREATE PROCEDURE [Entity].[DeleteOperation]

	@Id INT

AS
BEGIN

	WITH operation_hierarchy (Id, GroupId)
	AS
	(
			SELECT o.Id, o.GroupId FROM [Entity].[Operation] o
			WHERE o.Id = @Id
		UNION ALL
			SELECT o.Id, o.GroupId FROM [Entity].[Operation] o
			INNER JOIN operation_hierarchy AS oh ON o.GroupId = oh.Id
	)
	DELETE FROM [Entity].[Operation] WHERE [Id] IN(SELECT Id FROM operation_hierarchy)

END