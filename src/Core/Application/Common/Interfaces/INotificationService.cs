using System.Threading.Tasks;
using Application.Common.Models;

namespace Application.Common.Interfaces
{
    public interface INotificationService
    {
        Task SendAsync(NotificationModel message);
    }
}
