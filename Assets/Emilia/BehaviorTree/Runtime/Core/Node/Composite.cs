using Emilia.Reference;

namespace Emilia.BehaviorTree
{
    public abstract class CompositeAsset : NodeAsset { }

    public abstract class Composite : Node
    {
        protected override void OnFinish(bool result)
        {
            int count = children.Count;
            for (int i = 0; i < count; i++)
            {
                Node child = children[i];
                child.ParentCompositeStop(this);
            }
        }

        public abstract void StopLowerPriorityChildrenForChild(Node abortForChild, bool immediateRestart);
    }

    public abstract class CompositeAsset<T> : CompositeAsset where T : Node, new()
    {
        public override Node CreateNode()
        {
            return ReferencePool.Acquire<T>();
        }
    }

    public abstract class Composite<T> : Composite where T : NodeAsset
    {
        protected T asset;

        protected override void OnInit()
        {
            asset = nodeAsset as T;
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            asset = null;
        }
    }
}