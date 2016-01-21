namespace Legacy.Domain.Results
{
	/// <summary>Данные о результате исполнения операции</summary>
	public class ExecuteStatus
	{
		public ExecuteStatus()
		{
			Success = true;
		}

		public ExecuteStatus(string errorMessage)
		{
			ErrorMessage = errorMessage;
		}

		/// <summary>Успешность исполнения</summary>
		public bool Success { get; set; }

		/// <summary>Данные об ошибке исполнения</summary>
		public string ErrorMessage { get; set; }
	}

	/// <summary>Расширение класса результата исполнения с данными</summary>
	/// <typeparam name="TResult">Тип данных исполнения</typeparam>
	public class ExecuteStatus<TResult> : ExecuteStatus
	{
		public ExecuteStatus(TResult result)
		{
			Result = result;
		}

		public ExecuteStatus(TResult result, string errorMessage) : base(errorMessage) 
		{
			Result = result;
		}

		/// <summary>Данные исполнения</summary>
		public TResult Result { get; set; }
	}
}