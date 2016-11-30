using System;

namespace DataObjects
{
    [Serializable]
    public class TodoItem
    {
        public Guid Key { get; set; }
        public string Description { get; set; }
        public bool IsComplete { get; set; }

        public DateTime TimeCreated { get; set; }
    }
}