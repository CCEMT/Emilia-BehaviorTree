using System;

namespace Emilia.AI
{
    public class BehaviorTreeKeySelectorAttribute : Attribute
    {
        public string filePath;

        public BehaviorTreeKeySelectorAttribute(string filePath)
        {
            this.filePath = filePath;
        }
    }
}