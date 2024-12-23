using Emilia.BehaviorTree.Attributes;
using Sirenix.OdinInspector;

namespace Emilia.BehaviorTree
{
    [BehaviorTreeMenu("任务节点/等待直到停止")]
    public class WaitUntilStoppedAsset : BaseTaskAsset<WaitUntilStopped>
    {
        [LabelText("停止时返回是否成功")]
        public bool successWhenStopped;
    }

    public class WaitUntilStopped : BaseTask<WaitUntilStoppedAsset>
    {
        protected override void OnStop()
        {
            Finish(this.asset.successWhenStopped);
        }
    }
}