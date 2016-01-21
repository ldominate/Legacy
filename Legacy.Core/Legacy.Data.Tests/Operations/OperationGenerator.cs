using System.Collections.Generic;
using Legacy.Domain.Operations;
using Ploeh.AutoFixture;
using StringGenerator = Legacy.Data.Tests.Generators.StringGenerator;

namespace Legacy.Data.Tests.Operations
{
	public class OperationGenerator
	{
		private readonly Fixture _fixture;
		private readonly StringGenerator _stringGenerator;

		public OperationGenerator(Fixture fixture, StringGenerator stringGenerator)
		{
			_fixture = fixture;
			_stringGenerator = stringGenerator;
		}

		public Operation Generate()
		{
			return _fixture.Build<Operation>()
				.With(o => o.Name, _stringGenerator.GenerateCyrillicWords().Trim())
				.Without(o => o.Id)
				.Without(o => o.IsDeleted)
				.Without(o => o.GroupId)
				.Without(o => o.Operations)
				.Create();
		}

		public IEnumerable<Operation> GenerateCollection(int length = 9)
		{
			for (var i = 0; i < length; i++)
			{
				yield return Generate();
			}
		}
	}
}