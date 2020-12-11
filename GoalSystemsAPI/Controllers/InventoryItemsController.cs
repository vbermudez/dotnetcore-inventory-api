using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using GoalSystemsAPI.Models;
using System.Collections.Concurrent;
using System.Threading;
using System.Text.Json;
using GoalSystemsAPI.Providers;
using GoalSystemsAPI.Providers.Models;
using Microsoft.Extensions.Logging;

namespace GoalSystemsAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class InventoryItemsController : ControllerBase
    {
        private readonly InventoryContext _context;
        private readonly IMessageRepository _messageRepository;
        //private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public InventoryItemsController(InventoryContext context, IMessageRepository messageRepository)
        {
            _context = context;
            _messageRepository = messageRepository;
        }

        // GET: api/v1/InventoryItems
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<InventoryItem>>> GetInventoryItems()
        {
            return await _context.InventoryItems.ToListAsync();
        }

        // GET: api/v1/InventoryItems/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<InventoryItem>> GetInventoryItem(long id)
        {
            var inventoryItem = await _context.InventoryItems.FindAsync(id);

            if (inventoryItem == null)
            {
                return NotFound();
            }

            return inventoryItem;
        }

        // PUT: api/v1/InventoryItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutInventoryItem(long id, InventoryItem inventoryItem)
        {
            if (id != inventoryItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(inventoryItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // PUT: api/v1/InventoryItems/add/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("add/{id}")]
        [Authorize]
        public async Task<IActionResult> AddInventoryItem(long id)
        {
            var inventoryItem = await _context.InventoryItems.FindAsync(id);

            inventoryItem.Units += 1;
            _context.Entry(inventoryItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // PUT: api/v1/InventoryItems/extract/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("extract/{id}")]
        [Authorize]
        public async Task<IActionResult> ExtractInventoryItem(long id)
        {
            var inventoryItem = await _context.InventoryItems.FindAsync(id);
            var notification = new Notification { Message = $"Item({inventoryItem.Id}) {inventoryItem.Name} extracted", Type = "extracted" };

            inventoryItem.Units -= 1;
            _context.Entry(inventoryItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _messageRepository.Broadcast(notification);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/v1/InventoryItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<InventoryItem>> PostInventoryItem(InventoryItem inventoryItem)
        {
            _context.InventoryItems.Add(inventoryItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInventoryItem", new { id = inventoryItem.Id }, inventoryItem);
        }

        // DELETE: api/v1/InventoryItems/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<InventoryItem>> DeleteInventoryItem(long id)
        {
            var inventoryItem = await _context.InventoryItems.FindAsync(id);
            if (inventoryItem == null)
            {
                return NotFound();
            }

            _context.InventoryItems.Remove(inventoryItem);
            await _context.SaveChangesAsync();

            return inventoryItem;
        }

        // GET: api/v1/events
        [Produces("text/event-stream")]
        [HttpGet("events")]
        public async Task SubscribeEvents(CancellationToken cancellationToken)
        {
            SetServerSentEventHeaders();

            var data = new { Message = "connected", Type = "connection" };
            //var jsonConnection = JsonSerializer.Serialize(data, _jsonSerializerOptions);

            await Response.WriteAsync($"event: {data.Type}\n", cancellationToken);
            await Response.WriteAsync($"data: {data.Message}\n\n", cancellationToken);
            await Response.Body.FlushAsync(cancellationToken);

            async void OnNotification(object? sender, Notification notification)
            {
                try
                {
                    // idea: https://stackoverflow.com/a/58565850/80527
                    // var json = JsonSerializer.Serialize(notification.Message, _jsonSerializerOptions);
                    await Response.WriteAsync($"event: {notification.Type}\n", cancellationToken);
                    await Response.WriteAsync($"data: {notification.Message}\n\n", cancellationToken);
                    await Response.Body.FlushAsync(cancellationToken);
                }
                catch (Exception)
                {
                    Console.WriteLine("Not able to send the notification");
                }
            }

            _messageRepository.NotificationEvent += OnNotification;

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    // Spin until something break or stop...
                    await Task.Delay(1000, cancellationToken);
                }
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("Client disconnected");
            }
            finally
            {
                _messageRepository.NotificationEvent -= OnNotification;
            }
        }

        [HttpPost("broadcast")]
        public Task Broadcast([FromBody] Notification notification)
        {
            _messageRepository.Broadcast(notification);

            return Task.CompletedTask;
        }

        private bool InventoryItemExists(long id)
        {
            return _context.InventoryItems.Any(e => e.Id == id);
        }

        private void SetServerSentEventHeaders()
        {
            Response.StatusCode = 200;
            Response.Headers.Add("Content-Type", "text/event-stream");
            Response.Headers.Add("Cache-Control", "no-cache");
            Response.Headers.Add("Connection", "keep-alive");
        }
    }
}
