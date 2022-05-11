using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;

namespace Infrastructure.Common
{
    public class NotificationService : INotificationService
    {
        public Task SendAsync(NotificationModel message)
        {
            return Task.CompletedTask;
        }
    }
}
