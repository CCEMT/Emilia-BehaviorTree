using Emilia.BehaviorTree.Attributes;
using Sirenix.OdinInspector;

namespace Emilia.BehaviorTree
{
    [BehaviorTreeMenu("任务节点/等待节点")]
    public class WaitAsset : UniversalTaskAsset<Wait>
    {
        [LabelText("等待时间(秒)")]
        public float seconds;
    }

    public class Wait : UniversalTask<WaitAsset>
    {
        private Clock.Timer timer;

        protected override void OnStart()
        {
            timer = tree.clock.AddTimer(new FloatTimeInfo(asset.seconds), OnTimerEnd);
        }

        protected override void OnStop()
        {
            if (timer != null) tree.clock.RemoveTimer(timer);
            Finish(false);
        }

        private void OnTimerEnd()
        {
            if (timer != null) tree.clock.RemoveTimer(timer);
            Finish(true);
        }
    }
}