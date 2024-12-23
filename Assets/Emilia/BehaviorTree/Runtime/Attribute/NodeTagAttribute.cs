using System;

namespace Emilia.BehaviorTree.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class NodeTagAttribute : Attribute
    {
        public string[] tags;

        public NodeTagAttribute(params string[] tags)
        {
            this.tags = tags;
        }
    }
}