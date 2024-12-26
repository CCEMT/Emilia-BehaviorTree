using System;

namespace Emilia.BehaviorTree.Attributes
{
    /// <summary>
    /// 菜单
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class BehaviorTreeMenuAttribute : Attribute
    {
        public string path;
        public int priority;

        public BehaviorTreeMenuAttribute(string path, int priority = 0)
        {
            this.path = path;
            this.priority = priority;
        }
    }
}