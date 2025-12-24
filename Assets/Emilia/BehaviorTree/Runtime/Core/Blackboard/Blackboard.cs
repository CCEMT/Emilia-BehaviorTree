using Emilia.Variables;

namespace Emilia.BehaviorTree
{
    public class Blackboard : VariablesEventManager
    {
        private Clock clock;

        public Blackboard(Clock clock, VariablesManager variablesManage) : base(variablesManage)
        {
            this.clock = clock;
        }

        protected override void OnSet<T>(string key, T value)
        {
            base.OnSet(key, value);
            this.clock.AddTimer(Tick);
        }
    }
}