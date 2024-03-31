using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace MyWebApplication.Services
{
    public class MyBackgroundService : BackendJob
    {
        private static bool _shouldRestart = true;
        private readonly ILogger<MyBackgroundService> _logger;


        private static IServiceProvider _serviceProvider;
        private static CancellationTokenSource _stoppingCts2;
        private static CancellationTokenSource _stoppingCts;
        private static CancellationToken _stoppingToken;
        private static CancellationToken _cancellationToken;


        private bool _isServiceRunning = true;
        private static readonly object _lockObject = new object();

        public MyBackgroundService(ILogger<MyBackgroundService> logger)
        {
            this._logger = logger;
            _logger.LogInformation("MyBackgroundService init.");
        }
        public static void SetServiceCollection(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            base.StartAsync(cancellationToken);

            // Otherwise it's running
            return Task.CompletedTask;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            MyBackgroundService._stoppingToken = stoppingToken;
            _stoppingCts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
            _stoppingCts2 = CancellationTokenSource.CreateLinkedTokenSource(_stoppingCts.Token);


            return DoWorkAsync(_stoppingCts2.Token);

        }

        public Task DoWorkAsync(CancellationToken cancellationToken)
        {
            return Task.Run(async () => {
                     while (!cancellationToken.IsCancellationRequested && _shouldRestart)
                    {
                        _logger.LogInformation("Background service is running... {}, {}", this.GetHashCode(), DateTime.Now);
                        // Simulate work
                        await Task.Delay(2000);

                        // Check if restart is requested
                        if (!_shouldRestart)
                        {
                            return; // Exit the loop if restart is not requested
                        }
                    }               
                });

        }

        public static void TestCancelToke()
        {

            //_shouldRestart = false;
            try { 
                _stoppingCts.Cancel();
                Console.WriteLine("stoppingToken status---------" + _stoppingToken.IsCancellationRequested);
                Console.WriteLine("_stoppingCts Token status---------" + _stoppingCts.Token.IsCancellationRequested);
                Console.WriteLine("_stoppingCts2.Token status---------" + _stoppingCts2.Token.IsCancellationRequested);

            } catch (Exception ex)
            {
                Console.WriteLine("stoppingToken status2---------" + _stoppingToken.IsCancellationRequested);
                Console.WriteLine(ex.Message +"---------" + ex.StackTrace);
            }
        }

        public void RestartService()
        {
            _shouldRestart = true;
            StartAsync(default).GetAwaiter().GetResult(); // Manually restart the service
        }

        public string StartService()
        {
            lock (_lockObject)
            {
                if (_cancellationToken.IsCancellationRequested)
                {
                    return "CancellationToken Is CancellationRequested";
                }
                if (!_isServiceRunning)
                {
                    
                    StartAsync(_cancellationToken).GetAwaiter().GetResult();
                    _isServiceRunning = true;
                    _logger.LogInformation("Background service started... {}, {}", this.GetHashCode(), DateTime.Now);
                    return "Service started.";

                }
                return "Service is already running.";
                
            }
        }


        public string StopService()
        {
            lock (_lockObject)
            {
                if (_cancellationToken.IsCancellationRequested)
                {
                    return "CancellationToken Is CancellationRequested";
                }
                if (_isServiceRunning && !_cancellationToken.IsCancellationRequested)
                {
                    StopAsync(default).GetAwaiter().GetResult();
                    _isServiceRunning = false;
                    _logger.LogInformation("Background service stopped... {}, {}", this.GetHashCode(), DateTime.Now);
                    return "Service stopped.";
           
                }

                return "Service is not running.";

            }
        }

        public string StartService2() {
            // 尝试获取锁，如果获取不到立即返回
            bool lockAcquired = Monitor.TryEnter(_lockObject);
            try
            {
                if (!lockAcquired)
                {
                    // 没有获取到锁，执行其他操作或返回
                    return "Failed to acquire lock, returning...";
                }
                // 获取到了锁，执行临界区代码
                if (_cancellationToken.IsCancellationRequested)
                {
                    return "CancellationToken Is CancellationRequested";
                }
                if (!_isServiceRunning)
                {
                    
                    StartAsync(_cancellationToken).GetAwaiter().GetResult();
                    _isServiceRunning = true;
                    _logger.LogInformation("Background service started... {}, {}", this.GetHashCode(), DateTime.Now);
                    return "Service started.";

                }
                return "Service is already running.";

            }
            finally
            {
                if (lockAcquired)
                {
                    // 释放锁
                    Monitor.Exit(_lockObject);
                }
            }
        }

    }

}
