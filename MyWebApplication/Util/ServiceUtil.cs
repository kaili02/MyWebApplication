namespace MyWebApplication.Util
{
    public static class ServiceUtil
    {
        private static ServiceProvider _serviceProvider;

        // Initialize this method at startup
        public static void Initialize(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static ILogger<T> Logger<T>()
        {
            if (_serviceProvider == null)
            {
                throw new InvalidOperationException("Please execute Initialize method.");
            }
            return _serviceProvider.GetRequiredService<ILogger<T>>();
        }
        public static T? GetService<T>()
        {
            if (_serviceProvider == null)
            {
                throw new InvalidOperationException("Please execute Initialize method.");
            }
            return _serviceProvider.GetService<T>();
        }
        public static T GetRequiredService<T>()
        {
            if (_serviceProvider == null)
            {
                throw new InvalidOperationException("Please execute Initialize method.");
            }
            return _serviceProvider.GetRequiredService<T>();
        }
    }
}
