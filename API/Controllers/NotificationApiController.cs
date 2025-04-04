using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using Repositories.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationApiController : ControllerBase
    {
        private readonly INotificationInterface _not;

        public NotificationApiController(INotificationInterface notificationInterface)
        {
            _not = notificationInterface;
        }
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddNotification([FromForm] t_notification notification)
        {
            var status = await _not.Add(notification);
            if (status == 1)
            {
                return Ok(new { success = true, message = "notification created" });
            }
            else
            {
                return BadRequest(new { success = false, message = "notification not error created" });
            }
        }

        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List()
        {
            var notifications = await _not.GetAll();
            return Ok(notifications);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _not.Delete(id);
            return Ok(new { success = result > 0 });
        }
    }
}