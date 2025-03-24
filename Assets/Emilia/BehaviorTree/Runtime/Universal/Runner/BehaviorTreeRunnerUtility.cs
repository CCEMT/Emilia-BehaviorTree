#if !UNITY_EDITOR
using Emilia.Reference;
#endif

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Emilia.BehaviorTree
{
    public static class BehaviorTreeRunnerUtility
    {
        private static int maxId = 0;
        private static Queue<int> idPool = new Queue<int>();

        public static int GetId()
        {
            if (idPool.Count == 0) return ++maxId;
            return idPool.Dequeue();
        }

        public static void RecycleId(int id)
        {
            idPool.Enqueue(id);
        }

        public static IBehaviorTreeRunner CreateRunner()
        {
#if UNITY_EDITOR
            Type type = Assembly.Load("Emilia.BehaviorTree.Editor").GetType("Emilia.BehaviorTree.Editor.EditorBehaviorTreeRunner");
            return Activator.CreateInstance(type) as IBehaviorTreeRunner;
#else
            return ReferencePool.Acquire<RuntimeBehaviorTreeRunner>();
#endif
        }
    }
}