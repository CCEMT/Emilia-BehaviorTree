using Emilia.BehaviorTree.Attributes;

namespace Emilia.BehaviorTree
{
    [NodeTag(BaseDefine.NodeTag)]
    public abstract class BaseCompositeAsset<T> : CompositeAsset<T> where T : Node, new() { }

    public abstract class BaseComposite<T> : Composite<T> where T : NodeAsset { }
}