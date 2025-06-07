using Emilia.BehaviorTree;
using Emilia.BehaviorTree.Attributes;

namespace Emilia.AI.Task
{
    [NodeTag(AIDefine.NodeTag)]
    public abstract class AITaskAsset<T> : BaseTaskAsset<T> where T : BehaviorTree.Node, new() { }

    public abstract class AITask<T> : BaseTask<T> where T : NodeAsset { }
}