namespace RetroFootballAPI.Repositories
{
    public interface IRedisCacheManager
    {
        Task SetRedisCache(string key, object response, TimeSpan timeOut);
        Task<string> GetRedisCache(string key);
    }
}
