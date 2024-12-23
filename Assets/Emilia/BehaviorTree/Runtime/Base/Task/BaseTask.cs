using Emilia.BehaviorTree.Attributes;

namespace Emilia.BehaviorTree
{
    [NodeTag(BaseDefine.NodeTag)]
    public abstract class BaseTaskAsset<T> : TaskAsset<T> where T : Node, new() { }

    public abstract class BaseTask<T> : Task<T> where T : NodeAsset { }
}