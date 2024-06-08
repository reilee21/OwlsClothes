using Microsoft.EntityFrameworkCore;
using Owls.Models;
using System.Drawing;

namespace Owls.Repositories.ColorRepos
{
	public class ColorRepos : IColorRepos
	{
		private readonly OwlStoreContext _storeContext;

		public ColorRepos(OwlStoreContext storeContext)
		{
			_storeContext = storeContext;
		}

		public async Task<Models.Color> GetColor(int id)
		{
			var color = await _storeContext.Colors.FirstOrDefaultAsync(p => p.ColorId.Equals(id));

			return color;
		}

		public async Task<IEnumerable<Models.Color>> GetColors()
		{
			var colors = await _storeContext.Colors.ToListAsync();
			return colors;
		}

	}
}
