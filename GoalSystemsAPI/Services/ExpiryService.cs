using GoalSystemsAPI.Models;
using GoalSystemsAPI.Providers;
using GoalSystemsAPI.Providers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoalSystemsAPI.Services
{
    public interface IExpiryService
    {

    }

    public class ExpiryService : BackgroundService, IExpiryService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IServiceScopeFactory _scopeFactory;

        public ExpiryService(IServiceScopeFactory scopeFactory, IMessageRepository messageRepository)
        {
            _scopeFactory = scopeFactory;
            _messageRepository = messageRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<InventoryContext>();
                    var inventoryItems = from itm in context.InventoryItems
                                         where itm.Expired == false && itm.ExpiryDate <= DateTime.Now
                                         select itm;

                    foreach (var item in inventoryItems)
                    {
                        var notification = new Notification { Message = $"Item({item.Id}) {item.Name} expired on {item.ExpiryDate.ToShortDateString()}", Type = "expired" };

                        _messageRepository.Broadcast(notification);
                        item.Expired = true;
                        context.Entry(item).State = EntityState.Modified;
                    }

                    try
                    {
                        await context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw;
                    }
                }
                
                await Task.Delay(30 * 1000, stoppingToken);
            }
        }
    }
}
