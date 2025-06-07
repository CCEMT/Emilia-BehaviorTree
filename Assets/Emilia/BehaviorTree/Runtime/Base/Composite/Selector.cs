using Emilia.BehaviorTree.Attributes;

namespace Emilia.BehaviorTree
{
    [BehaviorTreeMenu("组合节点/选择节点")]
    public class SelectorAsset : BaseCompositeAsset<Selector> { }

    public class Selector : BaseComposite<SelectorAsset>
    {
        private int currentIndex = -1;

        protected override void OnStart()
        {
            currentIndex = -1;
            ProcessChildren();
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
                else children[this.currentIndex].Start();
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

        protected override void OnDispose()
        {
            base.OnDispose();
            currentIndex = -1;
        }
    }
}