using System;
using Emilia.BehaviorTree.Attributes;

namespace Emilia.BehaviorTree
{
    [BehaviorTreeMenu("组合节点/随机选择节点")]
    public class RandomSelectorAsset : UniversalCompositeAsset<RandomSelector> { }

    public class RandomSelector : UniversalComposite<RandomSelectorAsset>
    {
        private static Random random = new Random();

        private int currentIndex = -1;
        private int[] randomizedOrder;

        protected override void OnInit()
        {
            base.OnInit();
            randomizedOrder = new int[children.Count];
            for (int i = 0; i < children.Count; i++) { this.randomizedOrder[i] = i; }
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            this.randomizedOrder = null;
            this.currentIndex = -1;
        }

        protected override void OnStart()
        {
            this.currentIndex = -1;

            int n = this.randomizedOrder.Length;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                int value = this.randomizedOrder[k];
                this.randomizedOrder[k] = this.randomizedOrder[n];
                this.randomizedOrder[n] = value;
            }

            ProcessChildren();
        }

        protected override void OnStop()
        {
            if (this.currentIndex != -1) children[randomizedOrder[currentIndex]].Stop();
        }

        protected override void OnChildStop(Node child, bool result)
        {
            if (result) Finish(true);
            else ProcessChildren();
        }

        private void ProcessChildren()
        {
            currentIndex++;

            if (this.currentIndex >= children.Count) Finish(false);
            else
            {
                if (state == State.Stop) Finish(false);
                else children[this.randomizedOrder[this.currentIndex]].Start();
            }
        }

        public override void StopLowerPriorityChildrenForChild(Node abortForChild, bool immediateRestart)
        {
            int indexForChild = 0;
            bool found = false;

            int count = children.Count;
            for (var i = 0; i < count; i++)
            {
                Node currentChild = children[i];

                if (currentChild == abortForChild) found = true;
                else if (found == false) indexForChild++;
                else if (currentChild.state == State.Active)
                {
                    if (immediateRestart) this.currentIndex = indexForChild - 1;
                    else this.currentIndex = children.Count;
                    currentChild.Stop();
                    break;
                }
            }
        }
    }
}