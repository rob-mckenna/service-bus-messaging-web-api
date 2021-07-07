using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace service_bus_messaging_web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public PersonController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync([FromBody] Person patient)
        {
            IQueueClient queueClient = new QueueClient(_configuration["QueueConnectionString"], _configuration["QueueName"]);
            var orderJSON = JsonConvert.SerializeObject(patient);
            var orderMessage = new Message(Encoding.UTF8.GetBytes(orderJSON))
            {
                MessageId = Guid.NewGuid().ToString(),
                ContentType = "application/json"
            };
            await queueClient.SendAsync(orderMessage).ConfigureAwait(false);

            return Ok("Create order message has been successfully pushed to queue");
        }
    }
}
