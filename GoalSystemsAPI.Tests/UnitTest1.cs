using GoalSystemsAPI.Controllers;
using GoalSystemsAPI.Models;
using GoalSystemsAPI.Providers;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace GoalSystemsAPI.Tests
{
    public class Tests
    {
        private InventoryItemsController _controller;
        private MessageRepository _messageRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<InventoryContext>();
            
            options.UseSqlite();
            _messageRepository = new MessageRepository();
            _controller = new InventoryItemsController(new InventoryContext(options.Options), _messageRepository); 
        }

        [Test]
        public void ConstructionTest()
        {
            Assert.IsNotNull(_controller);
        }

        [Test]
        public async void AddItem()
        {
            var result = await _controller.AddInventoryItem(1);

            Assert.Equals(result, _controller.NoContent());
        }
    }
}