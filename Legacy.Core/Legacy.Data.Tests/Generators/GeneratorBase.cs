using System;
using System.Security.Cryptography;

namespace Legacy.Data.Tests.Generators
{
	public abstract class GeneratorBase : IDisposable
	{
		private RandomNumberGenerator _generator;
		
		protected GeneratorBase()
		{
			_generator = new RNGCryptoServiceProvider();
		}

		protected Random CreateRandom()
		{
			var buffer = new byte[4];
			_generator.GetBytes(buffer);
			return new Random(BitConverter.ToInt32(buffer, 0));
		}

		public void Dispose()
		{
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				_generator.Dispose();
				_generator = null;
			}
		}

		~GeneratorBase()
		{
			Dispose(false);

		}

	}
}