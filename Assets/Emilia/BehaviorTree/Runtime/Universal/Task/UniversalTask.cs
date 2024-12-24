using Emilia.BehaviorTree.Attributes;

namespace Emilia.BehaviorTree
{
    [NodeTag(UniversalDefine.NodeTag)]
    public abstract class UniversalTaskAsset<T> : TaskAsset<T> where T : Node, new() { }

    public abstract class UniversalTask<T> : Task<T> where T : NodeAsset { }
}