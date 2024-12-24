using System.Collections.Generic;
using Emilia.BehaviorTree.Attributes;
using Sirenix.OdinInspector;

namespace Emilia.BehaviorTree
{
    public enum ParallelPolicy
    {
        [LabelText("一个（成功/失败）就返回（成功/失败）")]
        One,

        [LabelText("全部（成功/失败）才返回（成功/失败）")]
        All,
    }

    [BehaviorTreeMenu("组合节点/并行节点")]
    public class ParallelAsset : BaseCompositeAsset<Parallel>
    {
        [LabelText("成功策略")]
        public ParallelPolicy successPolicy;

        [LabelText("失败策略")]
        public ParallelPolicy failurePolicy;
    }

    public class Parallel : BaseComposite<ParallelAsset>
    {
        private int runningCount = 0;
        private int succeededCount = 0;
        private int failedCount = 0;

        private bool successState;
        private bool childrenAborted;

        private Dictionary<Node, bool> childrenResults = new Dictionary<Node, bool>();

        protected override void OnStart()
        {
            childrenResults.Clear();

            int count = children.Count;
            for (int i = 0; i < count; i++)
            {
                Node child = children[i];
                child.Start();
            }
        }

        protected override void OnStop()
        {
            int count = children.Count;
            for (int i = 0; i < count; i++)
            {
                Node child = children[i];
                if (child.state != State.Active) continue;
                child.Stop();
            }
        }

        protected override void OnChildStop(Node child, bool result)
        {
            runningCount--;

            if (result) succeededCount++;
            else failedCount++;

            this.childrenResults[child] = result;

            bool allChildrenStarted = runningCount + succeededCount + failedCount == children.Count;
            if (allChildrenStarted == false) return;

            if (this.runningCount == 0)
            {
                if (this.childrenAborted == false)
                {
                    if (this.asset.failurePolicy == ParallelPolicy.One && this.failedCount > 0)
                    {
                        this.successState = false;
                    }
                    else if (this.asset.successPolicy == ParallelPolicy.One && this.succeededCount > 0)
                    {
                        this.successState = true;
                    }
                    else if (this.asset.successPolicy == ParallelPolicy.All && this.succeededCount == children.Count)
                    {
                        this.successState = true;
                    }
                    else
                    {
                        this.successState = false;
                    }
                }

                Finish(this.successState);
            }
            else if (this.childrenAborted == false)
            {
                if (this.asset.failurePolicy == ParallelPolicy.One && this.failedCount > 0)
                {
                    this.successState = false;
                    this.childrenAborted = true;
                }
                else if (this.asset.successPolicy == ParallelPolicy.One && this.succeededCount > 0)
                {
                    this.successState = true;
                    this.childrenAborted = true;
                }

                if (this.childrenAborted == false) return;

                int count = this.children.Count;
                for (var i = 0; i < count; i++)
                {
                    Node currentChild = this.children[i];
                    if (currentChild.state != State.Active) continue;
                    currentChild.Stop();
                }
            }
        }

        public override void StopLowerPriorityChildrenForChild(Node abortForChild, bool immediateRestart)
        {
            if (immediateRestart == false) return;

            if (childrenResults[abortForChild]) succeededCount--;
            else failedCount--;

            runningCount++;

            abortForChild.Start();
        }
    }
}