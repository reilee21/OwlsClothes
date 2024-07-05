using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Owls.Models;

namespace Owls.Repositories.ManageRepos
{
    public class ManageRepos : IManageRepos
    {
        private readonly OwlStoreContext context;
        private readonly UserManager<OwlsUser> userManager;

        public ManageRepos(OwlStoreContext context, UserManager<OwlsUser> us)
        {
            this.context = context;
            this.userManager = us;
        }


        public async Task<double> GetRevenueByTime(DateTime from, DateTime to)
        {
            to = to.AddDays(1);

            var ttorder = await context.Orders.Where(o => o.CreateAt.Date >= from.Date && o.CreateAt.Date < to.Date).Select(o => o.Total).ToListAsync();
            double rs = 0;
            if (ttorder.Any())
            {
                rs = ttorder.Sum();
            }
            return rs;

        }


        public async Task<int> GetTotalCustomer()
        {
            var users = await userManager.Users.ToListAsync();
            int rs = 0;
            foreach (var u in users)
            {
                var role = await userManager.GetRolesAsync(u);
                if (role.Count == 0) rs++;
            }
            return rs;
        }

        public int GetTotalOrder()
        {
            return context.Orders.Count();
        }
    }
}
