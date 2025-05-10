namespace TrendyolApp.Server.Services
{
    public interface IQueueService
    {
       Task<string?> GetDiscountCodeAsync();
    }
}
