namespace btl_web_coffeeshop.Utilities
{
    public static class HttpRequestExtensions
    {
        public static bool IsAjax(this HttpRequest request)
        {
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }

}
