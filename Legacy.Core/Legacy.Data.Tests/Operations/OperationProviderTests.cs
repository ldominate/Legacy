using System;
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

		/// <summary></summary>
		/// <param name="parent">С Id из БД</param>
		/// <param name="length">Длинна случайной последовательности</param>
		/// <returns></returns>
		protected IEnumerable<Operation> SetTestListOperations(Operation parent = null, int length = 9)
		{
			var i = 0;
			foreach (var operation in Container.Resolve<OperationGenerator>().GenerateCollection(length))
			{
				operation.Order = i++;
				if (parent != null)
				{
					operation.GroupId = parent.Id;
					operation.Type = OperationType.Item;
				}
				else
				{
					operation.Type = OperationType.Group;
				}
				var result = Provider.Add(operation);

				Assert.IsNotNull(result);
				Assert.IsTrue(result.Success, result.ErrorMessage);

				yield return operation;
			}
		}

		protected IEnumerable<Operation> SetTreeListTestOperation(int lenRoot = 5)
		{
			var rnd = new Random();

			return SetTestListOperations(null, lenRoot).Select(r =>
			{
				r.Operations = SetTestListOperations(r, rnd.Next(3, 9)).ToArray();
				return r;
			});
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
		public void LogicDeleteShouldBeResultSuccess()
		{
			foreach (var operation in SetTestListOperations().Reverse())
			{
				var result = Provider.LogicDelete(operation.Id);

				Assert.IsNotNull(result);
				Assert.IsTrue(result.Success, result.ErrorMessage);
			}
		}

		[TestMethod]
		public void LogicDeleteGetShouldBeIsDeleted()
		{
			var operations = SetTestListOperations().Reverse().Select(o => { Provider.LogicDelete(o.Id); return o; }).ToArray();

			foreach (var operation in operations)
			{
				var result = Provider.GetById(operation.Id);

				Assert.IsNotNull(result);
				Assert.IsTrue(result.Success, result.ErrorMessage);
				Assert.IsNotNull(result.Result);
				Assert.IsTrue(result.Result.IsDeleted);
			}
		}

		[TestMethod]
		public void LogicRecoveryShouldBeResultSuccess()
		{
			var operations = SetTestListOperations().Reverse().Select(o => { Provider.LogicDelete(o.Id); return o; }).ToArray();

			foreach (var operation in operations.OrderBy(o => o.Name))
			{
				var result = Provider.LogicRecovery(operation.Id);

				Assert.IsNotNull(result);
				Assert.IsTrue(result.Success, result.ErrorMessage);
			}
		}

		[TestMethod]
		public void LogicRecoveryGetShouldBeIsDeleted()
		{
			var operations = SetTestListOperations().Reverse().Select(o => { Provider.LogicDelete(o.Id); return o; }).Select(o => { Provider.LogicRecovery(o.Id); return o; }).ToArray();

			foreach (var operation in operations.OrderBy(o => o.Name))
			{
				var result = Provider.GetById(operation.Id);

				Assert.IsNotNull(result);
				Assert.IsTrue(result.Success, result.ErrorMessage);
				Assert.IsNotNull(result.Result);
				Assert.IsFalse(result.Result.IsDeleted);
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

			var root = operationGenerator.Generate();

			Provider.Add(root);

			root.Operations = SetTestListOperations(root).ToArray();

			var maxOrder = root.Operations.Max(o => o.Order);

			foreach (var operation in root.Operations.OrderBy(o => o.Name))
			{
				if (operation.GroupId.HasValue)
				{
					var result = Provider.MaxOrder(operation.GroupId.Value);

					Assert.IsNotNull(result);
					Assert.IsTrue(result.Success, result.ErrorMessage);
					Assert.IsTrue(result.Result <= maxOrder, "MaxOrder:{0}", result.Result);

					TestContext.WriteLine("Id:\t{0}\tOrder:\t{1}\tMaxOrder:\t{2}", operation.Id, operation.Order, result.Result);
				}
				else
				{
					TestContext.WriteLine("GroupId is null. Id: {0}", operation.Id);
				}
			}
		}

		[TestMethod]
		public void GetListShouldBeResultSuccess()
		{
			var tree = SetTreeListTestOperation();

			var result = Provider.GetList(new OperationListRequest {GroupId = tree.ElementAt(2).Id});

			Assert.IsNotNull(result);
			Assert.IsTrue(result.Success, result.ErrorMessage);
			Assert.IsNotNull(result.Result);
			Assert.IsTrue(result.Result.Any());
		}

		[TestMethod]
		public void GetListByDefaultRequestShouldBeResultSuccess()
		{
			var tree = SetTreeListTestOperation().ToArray();

			var result = Provider.GetList(new OperationListRequest());

			Assert.IsNotNull(result);
			Assert.IsTrue(result.Success, result.ErrorMessage);
			Assert.IsNotNull(result.Result);
			Assert.IsTrue(result.Result.Any());
			Assert.IsTrue(tree.Length == result.Result.Count(), "Length sequences not equal, src:{0} dst:{1}", tree.Length, result.Result.Count());

			foreach (var operation in tree.OrderBy(o => o.Name))
			{
				var resultDb = result.Result.FirstOrDefault(o => o.Id == operation.Id);

				Assert.IsNotNull(resultDb);

				var comparisonResult = CreateCompareLogic().Compare(operation, resultDb);
				Assert.IsTrue(comparisonResult.AreEqual, comparisonResult.DifferencesString);
			}
		}
	}
}