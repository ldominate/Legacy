using System.Collections.Generic;
using System.Linq;
using Autofac;
using KellermanSoftware.CompareNetObjects;
using Legacy.Data.Tests.Generators;
using Legacy.Domain.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Legacy.Data.Tests.Operations
{
	[TestClass]
	public class OperationProviderTests : TestBase
	{
		protected IOperationProvider Provider;

		public TestContext TestContext { get; set; }

		[TestInitialize]
		public void Start()
		{
			Provider = Container.Resolve<IOperationProvider>();
		}

		protected Operation Modify(Operation operation)
		{
			operation.Name = Container.Resolve<StringGenerator>().GenerateCyrillicWords();

			return operation;
		}

		protected IEnumerable<Operation> SetTestListOperations()
		{
			foreach (var operation in Container.Resolve<OperationGenerator>().GenerateCollection())
			{
				var result = Provider.Add(operation);

				Assert.IsNotNull(result);
				Assert.IsTrue(result.Success, result.ErrorMessage);

				yield return operation;
			}
		}

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
			foreach (var operation in Container.Resolve<OperationGenerator>().GenerateCollection())
			{
				var result = Provider.Add(operation);

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
				var result = Provider.Add(operation);

				Assert.IsNotNull(result);
				Assert.IsTrue(result.Success, result.ErrorMessage);
				Assert.IsTrue(result.Result > 0);
			}
		}

		[TestMethod]
		public void AddGetShouldBeEqual()
		{
			foreach (var operation in SetTestListOperations().Reverse())
			{
				var result = Provider.GetById(operation.Id);

				Assert.IsNotNull(result);
				Assert.IsTrue(result.Success, result.ErrorMessage);

				var comparisonResult = CreateCompareLogic().Compare(operation, result.Result);
				Assert.IsTrue(comparisonResult.AreEqual, comparisonResult.DifferencesString);
			}
		}

		[TestMethod]
		public void UpdateShouldBeResultSuccess()
		{
			foreach (var operation in SetTestListOperations().Reverse())
			{
				var result = Provider.Update(Modify(operation));

				Assert.IsNotNull(result);
				Assert.IsTrue(result.Success, result.ErrorMessage);
			}
		}

		[TestMethod]
		public void UpdateGetShouldBeEqual()
		{
			var operations = SetTestListOperations().ToArray();

			foreach (var operation in operations.Reverse())
			{
				Provider.Update(Modify(operation));
			}
			foreach (var operation in operations.OrderBy(o => o.Name))
			{
				var result = Provider.GetById(operation.Id);

				Assert.IsNotNull(result);
				Assert.IsTrue(result.Success, result.ErrorMessage);

				var comparisonResult = CreateCompareLogic().Compare(operation, result.Result);
				Assert.IsTrue(comparisonResult.AreEqual, comparisonResult.DifferencesString);
			}
		}

		[TestMethod]
		public void ForceDeleteShouldBeResultSuccess()
		{
			foreach (var operation in SetTestListOperations().Reverse())
			{
				var result = Provider.ForceDelete(operation.Id);

				Assert.IsNotNull(result);
				Assert.IsTrue(result.Success, result.ErrorMessage);
			}
		}

		[TestMethod]
		public void ForceDeleteShouldBeRealDelete()
		{
			var operations = SetTestListOperations().ToArray();

			foreach (var operation in operations.Reverse())
			{
				Provider.ForceDelete(operation.Id);
			}
			foreach (var operation in operations.OrderBy(o => o.Name))
			{
				var result = Provider.GetById(operation.Id);

				Assert.IsNotNull(result);
				Assert.IsTrue(result.Success, result.ErrorMessage);
				Assert.IsNull(result.Result);
			}
		}

		[TestMethod]
		public void MaxOrderShouldBeResultSuccess()
		{
			var operationGenerator = Container.Resolve<OperationGenerator>();

			var i = 0;
			var root = operationGenerator.Generate();
			root.Operations = operationGenerator.GenerateCollection().Select(o => { o.Order = i++; return o; }).ToArray();

			Provider.Add(root);

			root.SetParentByChilds();

			foreach (var result in root.Operations.Reverse().Select(operation => Provider.Add(operation)))
			{
				Assert.IsNotNull(result);
				Assert.IsTrue(result.Success, result.ErrorMessage);
			}

			foreach (var operation in root.Operations.OrderBy(o => o.Name))
			{
				if (operation.GroupId.HasValue)
				{
					var result = Provider.MaxOrder(operation.GroupId.Value);

					Assert.IsNotNull(result);
					Assert.IsTrue(result.Success, result.ErrorMessage);
					Assert.IsTrue(result.Result <= i, "MaxOrder:{0}", result.Result);

					TestContext.WriteLine("Id:\t{0}\tOrder:\t{1}\tMaxOrder:\t{2}", operation.Id, operation.Order, result.Result);
				}
				else
				{
					TestContext.WriteLine("GroupId is null. Id: {0}", operation.Id);
				}
			}
		}
	}
}