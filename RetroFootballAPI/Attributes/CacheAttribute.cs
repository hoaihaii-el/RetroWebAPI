using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RetroFootballAPI.Repositories;
using RetroFootballAPI.ViewModels;
using System.Text;

namespace RetroFootballAPI.Attributes
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private int _timeToLiveSeconds;
        public CacheAttribute(int timeToLiveSecond) 
        {
            _timeToLiveSeconds = timeToLiveSecond;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // đọc config của Redis
            var cacheConfiguration = context.HttpContext
                .RequestServices.GetRequiredService<RedisConfiguration>();

            if (!cacheConfiguration.IsEnable)
            {
                // nếu cache không được bật => chạy vào action để lấy kết quả
                await next();
                return;
            }

            // get singleton service
            var cacheService = context.HttpContext
                .RequestServices.GetRequiredService<IRedisCacheManager>();

            // dữ liệu với key tương ứng
            var cacheKey = GenerateKeyFromRequest(context.HttpContext.Request);
            var cacheResponse = await cacheService.GetRedisCache(cacheKey);

            if (!string.IsNullOrEmpty(cacheResponse)) 
            {
                // đã có dữ liệu trong cache thì trả về luôn
                var contentResult = new ContentResult
                {
                    Content = cacheResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };

                context.Result = contentResult;
                return;
            }

            // cache chưa có dữ liệu thì set cache
            var executeResult = await next();
            if (executeResult.Result is OkObjectResult)
            {
                await cacheService.SetRedisCache(cacheKey, executeResult.Result, 
                    TimeSpan.FromSeconds(_timeToLiveSeconds));
            }
        }

        public string GenerateKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}--{value}");
            }

            return keyBuilder.ToString();
        }
    }
}
