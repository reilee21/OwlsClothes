using Owls.Models;

namespace Owls.Repositories.ColorRepos
{
	public interface IColorRepos
	{
		Task<Color> GetColor(int id);
		Task<IEnumerable<Color>> GetColors();

	
	}
}
