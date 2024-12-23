using Emilia.BehaviorTree.Attributes;
using Sirenix.OdinInspector;

namespace Emilia.BehaviorTree
{
    [BehaviorTreeMenu("装饰节点/循环节点")]
    public class LoopAsset : BaseDecoratorAsset<Loop>
    {
        [LabelText("循环次数")]
        public int loopCount = -1;
    }

    public class Loop : BaseDecorator<LoopAsset>
    {
        private int currentLoop;
        private Clock.Timer timer;

        protected override void OnStart()
        {
            if (this.asset.loopCount == 0) Finish(true);
            else
            {
                this.currentLoop = 0;
                this.decoratedNode.Start();
            }
        }

        protected override void OnStop()
        {
            if (timer != null) tree.clock.RemoveTimer(timer);

            if (decoratedNode.state == State.Active) decoratedNode.Stop();
            else Finish(false);
        }

        protected override void OnChildStop(Node child, bool result)
        {
            if (result == false) Finish(false);
            else
            {
                currentLoop++;
                bool isFinish = this.asset.loopCount > 0 && this.currentLoop >= this.asset.loopCount;

                if (state == State.Stop || isFinish) Finish(true);
                else timer = tree.clock.AddTimer(ReStart);
            }

        }

        protected void ReStart()
        {
            decoratedNode.Start();
        }
    }
}