namespace MyWebApplication.Util
{
    public class LoggerProvider
    {
        private static ServiceProvider _serviceProvider;

        // Initialize this method at startup
        public static void InitializeLogger(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static ILogger<T> Logger<T>()
        {
            if (_serviceProvider == null)
            {
                throw new InvalidOperationException("Please execute InitializeLogger method.");
            }
            return _serviceProvider.GetRequiredService<ILogger<T>>();
        }
    }
}
