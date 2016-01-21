using System.Collections.Generic;
using System.Linq;
using Autofac;
using KellermanSoftware.CompareNetObjects;
using Legacy.Domain.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Legacy.Data.Tests.Operations
{
	[TestClass]
	public class OperationProviderTests : TestBase
	{
		protected virtual ICompareLogic CreateCompareLogic()
		{
			return new CompareLogic(new ComparisonConfig
			{
				TreatStringEmptyAndNullTheSame = true,
				IgnoreCollectionOrder = true,
				MembersToIgnore = new List<string> { "Operations" },
				MaxDifferences = 10,
				ShowBreadcrumb = true
			});
		}

		[TestMethod]
		public void AddShouldBeResultSuccess()
		{
			var provider = Container.Resolve<IOperationProvider>();

			foreach (var operation in Container.Resolve<OperationGenerator>().GenerateCollection())
			{
				var result = provider.Add(operation);

				Assert.IsNotNull(result);
				Assert.IsTrue(result.Success, result.ErrorMessage);
			}
		}

		[TestMethod]
		public void AddShouldBeReturnId()
		{
			var provider = Container.Resolve<IOperationProvider>();

			foreach (var operation in Container.Resolve<OperationGenerator>().GenerateCollection())
			{
				var result = provider.Add(operation);

				Assert.IsNotNull(result);
				Assert.IsTrue(result.Success, result.ErrorMessage);
				Assert.IsTrue(result.Result > 0);
			}
		}

		[TestMethod]
		public void AddGetOperationShouldBeEqual()
		{
			var provider = Container.Resolve<IOperationProvider>();

			var operations = Container.Resolve<OperationGenerator>().GenerateCollection().ToArray();

			foreach (var operation in operations)
			{
				var result = provider.Add(operation);

				Assert.IsNotNull(result);
				Assert.IsTrue(result.Success, result.ErrorMessage);
			}

			foreach (var operation in operations.Reverse())
			{
				var result = provider.GetById(operation.Id);

				Assert.IsNotNull(result);
				Assert.IsTrue(result.Success, result.ErrorMessage);

				var comparisonResult = CreateCompareLogic().Compare(operation, result.Result);
				Assert.IsTrue(comparisonResult.AreEqual, comparisonResult.DifferencesString);
			}
		}
	}
}