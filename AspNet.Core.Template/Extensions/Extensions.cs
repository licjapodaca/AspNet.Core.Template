#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using System;

namespace AspNet.Core.Template.Extensions
{
	public static class Extensions
	{
		public static int CalculateAge(this DateTime theDateTime)
		{
			var age = DateTime.Today.Year - theDateTime.Year;

			if (theDateTime.AddYears(age) > DateTime.Today)
				age--;

			return age;
		}
	}
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
