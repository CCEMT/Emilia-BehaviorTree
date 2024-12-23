using Emilia.BehaviorTree.Attributes;

namespace Emilia.BehaviorTree
{
    [BehaviorTreeMenu("组合节点/顺序节点")]
    public class SequenceAsset : BaseCompositeAsset<Sequence> { }

    public class Sequence : BaseComposite<SequenceAsset>
    {
        private int currentIndex = -1;

        protected override void OnStart()
        {
            currentIndex = -1;
            ProcessChildren();
        }

        protected override void OnStop()
        {
            if (currentIndex != -1) children[currentIndex].Stop();
        }

        protected override void OnChildStop(Node child, bool result)
        {
            if (result) ProcessChildren();
            else Finish(false);
        }

        private void ProcessChildren()
        {
            currentIndex++;

            if (this.currentIndex >= children.Count) Finish(true);
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
    }
}