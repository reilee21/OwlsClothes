namespace Owls.Repositories.ManageRepos
{
    public interface IManageRepos
    {
        Task<double> GetRevenueByTime(DateTime from, DateTime to);
        int GetTotalOrder();
        Task<int> GetTotalCustomer();

    }
}
