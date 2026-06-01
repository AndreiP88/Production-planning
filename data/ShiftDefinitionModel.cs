using System;

namespace data
{
    public class ShiftDefinitionModel
    {
        public ulong Id { get; set; }
        public int ShiftNumber { get; set; }
        public string Name { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
