using Sirenix.OdinInspector;

namespace Emilia.BehaviorTree
{
    public enum Stops
    {
        [LabelText("不停止")]
        None,

        [LabelText("停止自身")]
        Self,

        [LabelText("停止低优先级节点")]
        LowerPriority,

        [LabelText("停止自身节点和低优先级节点")]
        Both,

        [LabelText("重新启动")]
        ImmediateRestart,

        [LabelText("停止低优先级并重新启动")]
        LowerPriorityImmediateRestart
    }
}