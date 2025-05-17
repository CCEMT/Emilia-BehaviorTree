using System;

namespace Emilia.BehaviorTree
{
    public partial class Blackboard
    {
        private struct EventInfo : IEquatable<EventInfo>
        {
            public string key;
            public Action action;

            public EventInfo(string key, Action action)
            {
                this.key = key;
                this.action = action;
            }

            public bool Equals(EventInfo other) => this.key == other.key && Equals(this.action, other.action);

            public override bool Equals(object obj) => obj is EventInfo other && Equals(other);

            public override int GetHashCode() => HashCode.Combine(this.key, this.action);
        }
    }
}