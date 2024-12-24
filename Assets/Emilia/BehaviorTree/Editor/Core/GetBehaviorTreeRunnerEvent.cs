using UnityEngine.UIElements;

namespace Emilia.BehaviorTree.Editor
{
    public class GetBehaviorTreeRunnerEvent : EventBase<GetBehaviorTreeRunnerEvent>
    {
        public EditorBehaviorTreeRunner runner;

        protected override void Init()
        {
            base.Init();
            this.runner = null;
        }
    }
}