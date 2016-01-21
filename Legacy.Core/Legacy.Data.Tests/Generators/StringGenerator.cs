using System;
using System.Collections.Generic;
using Legacy.Domain.Common;

namespace Legacy.Data.Tests.Generators
{
	[Flags]
	public enum StringGeneratorSettings
	{
		Default = 0,

		Latin = 1,

		Cyrillic = 2,

		Caps = 4,

		Number = 8,

		Space = 16,

		RandomLength = 32
	}


	public class StringGenerator : GeneratorBase
	{
		readonly Range<char> _latinLowerRange = new Range<char>('a', 'z');
		readonly Range<char> _latinCapsRange = new Range<char>('A', 'Z');

		readonly Range<char> _cyrillicLowerRange = new Range<char>('а', 'я');
		readonly Range<char> _cyrillicCapsRange = new Range<char>('А', 'Я');

		private const int DefaultLength = 16;

		readonly Range<int> _defaultLenghtRange = new Range<int>(1, 255);

		public string GenerateNumeric(int min = 0, int max = 100)
		{
			return CreateRandom().Next(min, max).ToString();
		}

		public string Generate()
		{
			return Generate(StringGeneratorSettings.Latin, new Range<int>(_defaultLenghtRange.From, DefaultLength));
		}

		public string Generate(int length)
		{
			return Generate(StringGeneratorSettings.Latin, new Range<int>(_defaultLenghtRange.From, length));
		}

		public string GenerateCyrillicString(int length = 128)
		{
			return Generate(StringGeneratorSettings.Cyrillic | StringGeneratorSettings.RandomLength, new Range<int>(_defaultLenghtRange.From, length));
		}

		public string GenerateLatinString(int length = 128)
		{
			return Generate(StringGeneratorSettings.Latin | StringGeneratorSettings.RandomLength, new Range<int>(_defaultLenghtRange.From, length));
		}

		public string GenerateCyrillicWords(int length = 255)
		{
			return Generate(StringGeneratorSettings.Cyrillic | StringGeneratorSettings.Caps | StringGeneratorSettings.Space | StringGeneratorSettings.RandomLength, new Range<int>(_defaultLenghtRange.From, length));
		}

		public string GenerateLatinWords(int length = 255)
		{
			return Generate(StringGeneratorSettings.Latin | StringGeneratorSettings.Caps | StringGeneratorSettings.Space | StringGeneratorSettings.RandomLength, new Range<int>(_defaultLenghtRange.From, length));
		}

		public string Generate(StringGeneratorSettings settings)
		{
			return Generate(settings, _defaultLenghtRange);
		}

		public string Generate(StringGeneratorSettings settings, int length)
		{
			return Generate(settings, new Range<int>(_defaultLenghtRange.From, length));
		}

		public string Generate(StringGeneratorSettings settings, Range<int> rangeLength)
		{
			var random = CreateRandom();

			var result = string.Empty;

			var actualLength = new Range<int>(1, rangeLength.To);

			if (settings.HasFlag(StringGeneratorSettings.RandomLength))
			{
				//actualLength.From = random.Next(rangeLength.From, rangeLength.To - 1);

				actualLength.To = random.Next(rangeLength.From, rangeLength.To);
			}
			var actions = new List<StringGeneratorSettings>();

			if (settings.HasFlag(StringGeneratorSettings.Latin))
			{
				actions.Add(StringGeneratorSettings.Latin);
			}
			if (settings.HasFlag(StringGeneratorSettings.Cyrillic))
			{
				actions.Add(StringGeneratorSettings.Cyrillic);
			}
			if (settings.HasFlag(StringGeneratorSettings.Number))
			{
				actions.Add(StringGeneratorSettings.Number);
			}
			if (settings.HasFlag(StringGeneratorSettings.Space))
			{
				actions.Add(StringGeneratorSettings.Space);
			}
			var actionsCount = actions.Count;

			for (var i = actualLength.From; i <= actualLength.To; i++)
			{
				switch (actions[random.Next(0, actionsCount)])
				{
					case StringGeneratorSettings.Latin:
						var latinRange = (settings.HasFlag(StringGeneratorSettings.Caps) && random.Next(0, 9) > 5) ? _latinCapsRange : _latinLowerRange;

						result += (char)random.Next(latinRange.From, latinRange.To);
						break;
					case StringGeneratorSettings.Cyrillic:
						var cyrillicRange = (settings.HasFlag(StringGeneratorSettings.Caps) && random.Next(0, 9) > 5) ? _cyrillicCapsRange : _cyrillicLowerRange;

						result += (char)random.Next(cyrillicRange.From, cyrillicRange.To);
						break;
					case StringGeneratorSettings.Number:
						result += random.Next(0, 9).ToString();
						break;
					case StringGeneratorSettings.Space:
						var aRange = (settings.HasFlag(StringGeneratorSettings.Cyrillic)) ? _cyrillicLowerRange : _latinLowerRange;
						result += (result.Length <= 0 || i == actualLength.To - 1) ? (char)random.Next(aRange.From, aRange.To) : ' ';
						break;
					default:
						result += (char)random.Next(_latinLowerRange.From, _latinLowerRange.To);
						break;
				}
			}
			return result;
		}

		public string Generate(char min, char max, int len)
		{
			var result = string.Empty;
			for (var i = 0; i < len; i++)
				result += (char)CreateRandom().Next(min, max);
			return result;
		}
	}
}