using Emilia.Reference;

namespace Emilia.BehaviorTree
{
    public abstract class TaskAsset : NodeAsset { }

    public abstract class Task : Node { }

    public abstract class TaskAsset<T> : TaskAsset where T : Node, new()
    {
        public override Node CreateNode()
        {
            return ReferencePool.Acquire<T>();
        }
    }

    public abstract class Task<T> : Task where T : NodeAsset
    {
        protected T asset;

        protected override void OnInit()
        {
            asset = nodeAsset as T;
        }
    }
}