using System;

namespace Emilia.BehaviorTree.Attributes
{
    /// <summary>
    /// 节点Tag
    /// </summary>
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