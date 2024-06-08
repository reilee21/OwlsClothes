using Owls.Models;

namespace Owls.Helper
{
	public static class Money
	{ 
		public static string Cast(int? money)
		{
			return string.Format("{0:N0}₫", money);
		}
	}
}
