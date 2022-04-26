using System.Text.RegularExpressions;

public static class StringExtensions
{
	public static string FirstLetterToUpperCaseOrConvertNullToEmptyString(this string s)
	{
		if (string.IsNullOrEmpty(s))
			return string.Empty;

		char[] a = s.ToCharArray();
		a[0] = char.ToUpper(a[0]);
		return new string(a);
	}

	public static string FirstWordInString(this string s)
	{
		return Regex.Replace(s.Split()[0], @"[^0-9a-zA-Z\ ]+", "");
	}
}