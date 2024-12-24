using System.Reflection;

namespace Emilia.BehaviorTree
{
    public static class BehaviorTreeDebugUtility
    {
        public static void Ping(Node node, string message)
        {
#if UNITY_EDITOR
            Assembly.Load("Emilia.BehaviorTree.Editor").GetType("Emilia.BehaviorTree.Editor.EditorBehaviorTreeDebugUtility").GetMethod("Ping").Invoke(null, new object[] {node, message});
#endif
        }
    }
}