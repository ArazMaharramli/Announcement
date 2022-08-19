using System;
using Common;

namespace Infrastructure.Common
{
    public class MachineDateTime : IDateTimeService
    {
        public DateTime Now => DateTime.UtcNow;

        public int CurrentYear => DateTime.UtcNow.Year;
    }
}
