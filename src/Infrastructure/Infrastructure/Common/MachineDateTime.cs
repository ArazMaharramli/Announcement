using System;
using Common;

namespace Infrastructure.Common
{
    public class MachineDateTime : IDateTimeService
    {
        public DateTime Now => DateTime.Now;

        public int CurrentYear => DateTime.Now.Year;
    }
}
