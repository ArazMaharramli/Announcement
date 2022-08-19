using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Common.Models;

namespace Application.Common.Interfaces
{
    public interface IEventBusService
    {
        void AddEvent(IntegrationEvent @event);
        void RemoveEvent(IntegrationEvent @event);
        void Clear();

        Task PublishAsync();
    }
}
