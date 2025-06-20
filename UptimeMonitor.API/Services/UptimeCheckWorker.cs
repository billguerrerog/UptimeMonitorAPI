﻿using Microsoft.EntityFrameworkCore;
using UptimeMonitor.API.Data;
using UptimeMonitor.API.Entities;
using UptimeMonitor.API.Interfaces;

namespace UptimeMonitor.API.Services
{
    public class UptimeCheckWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<UptimeCheckWorker> _logger;

        public UptimeCheckWorker(IServiceScopeFactory scopeFactory, ILogger<UptimeCheckWorker> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("UptimeCheckWorker started.");

            var limaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");

            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, limaTimeZone);
                var checks = await db.UptimeChecks.ToListAsync(stoppingToken);

                foreach (var check in checks)
                {
                    var lastEvent = await db.UptimeEvents
                        .Where(e => e.UptimeCheckId == check.Id)
                        .OrderByDescending(e => e.StartTime)
                        .FirstOrDefaultAsync(stoppingToken);

                    var shouldRun = lastEvent == null ||
                                    now >= lastEvent.StartTime.AddMinutes(check.CheckInterval);

                    if (!shouldRun) continue;

                    var start = now;
                    var result = await SimulateCheckAsync(check);
                    var end = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, limaTimeZone);

                    var newEvent = new UptimeEvent
                    {
                        UptimeCheckId = check.Id,
                        StartTime = start,
                        EndTime = end,
                        IsUp = result.IsUp,
                        Note = result.Note
                    };

                    db.UptimeEvents.Add(newEvent);

                    if (!result.IsUp)
                    {
                        var pastDowns = await db.UptimeEvents
                            .Where(e => e.UptimeCheckId == check.Id && !e.IsUp)
                            .OrderByDescending(e => e.StartTime)
                            .Take(check.DownAlertResend)
                            .ToListAsync();

                        var firstDown = pastDowns.LastOrDefault();

                        if (firstDown != null && now >= firstDown.StartTime.AddMinutes(check.DownAlertDelay))
                        {
                            var subject = $"[ALERT] UptimeCheck '{check.Name}' is DOWN";
                            var body = check.DownAlertMessage ?? "The service is currently unavailable.";

                            try
                            {
                                await emailService.SendAsync(check.AlertEmail, subject, body);
                                _logger.LogInformation($"Alert email sent to {check.AlertEmail} for check '{check.Name}'.");
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, $"Failed to send alert email for check '{check.Name}'.");
                            }
                        }
                    }
                }

                await db.SaveChangesAsync(stoppingToken);

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task<(bool IsUp, string Note)> SimulateCheckAsync(UptimeCheck check)
        {
            await Task.Delay(200);

            var random = new Random();
            bool isUp = random.NextDouble() < 0.8;

            return isUp
                ? (true, "Simulated OK")
                : (false, "Simulated DOWN - mock failure");
        }
    }
}