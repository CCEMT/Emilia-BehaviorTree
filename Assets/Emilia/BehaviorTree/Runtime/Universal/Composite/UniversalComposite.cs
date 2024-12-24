using Emilia.BehaviorTree.Attributes;

namespace Emilia.BehaviorTree
{
    [NodeTag(UniversalDefine.NodeTag)]
    public abstract class UniversalCompositeAsset<T> : CompositeAsset<T> where T : Node, new() { }

    public abstract class UniversalComposite<T> : Composite<T> where T : NodeAsset { }
}