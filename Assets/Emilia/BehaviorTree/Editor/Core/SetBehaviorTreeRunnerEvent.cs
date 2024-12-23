using UnityEngine.UIElements;

namespace Emilia.BehaviorTree.Editor
{
    public class SetBehaviorTreeRunnerEvent : EventBase<SetBehaviorTreeRunnerEvent>
    {
        public EditorBehaviorTreeRunner runner;

        protected override void Init()
        {
            base.Init();
            this.runner = null;
        }

        public static SetBehaviorTreeRunnerEvent Create(EditorBehaviorTreeRunner runner)
        {
            SetBehaviorTreeRunnerEvent e = GetPooled();
            e.runner = runner;
            return e;
        }
    }
}