using System;
using System.Collections.Generic;
using Emilia.Variables;

namespace Emilia.BehaviorTree
{
    public partial class Blackboard
    {
        private Clock clock;
        private VariablesManage _variablesManage;

        private Dictionary<string, List<EventInfo>> events = new Dictionary<string, List<EventInfo>>();

        private List<EventInfo> addEvents = new List<EventInfo>();
        private List<EventInfo> removeEvents = new List<EventInfo>();

        private List<string> fireEvents = new List<string>();

        public VariablesManage variablesManage => this._variablesManage;

        public Blackboard(Clock clock, VariablesManage variablesManage)
        {
            this.clock = clock;
            this._variablesManage = variablesManage;
        }

        public void Set<T>(string key, T value)
        {
            bool isSet = this._variablesManage.SetValue(key, value);
            if (isSet == false) return;
            this.fireEvents.Add(key);
            this.clock.AddTimer(Fire);
        }

        public T Get<T>(string key)
        {
            return this._variablesManage.GetValue<T>(key);
        }

        public Variable GetVariable(string key)
        {
            return this._variablesManage.GetThisValue(key);
        }

        private void Fire()
        {
            if (fireEvents.Count == 0) return;

            int fireCount = fireEvents.Count;
            for (int i = 0; i < fireCount; i++)
            {
                string key = fireEvents[i];

                List<EventInfo> eventInfos = this.events.GetValueOrDefault(key);
                if (eventInfos == null) continue;

                int eventCount = eventInfos.Count;
                for (int j = 0; j < eventCount; j++)
                {
                    EventInfo eventInfo = eventInfos[j];
                    eventInfo.action.Invoke();
                }
            }

            fireEvents.Clear();

            int addCount = addEvents.Count;
            for (int i = 0; i < addCount; i++)
            {
                EventInfo eventInfo = addEvents[i];
                if (this.events.TryGetValue(eventInfo.key, out List<EventInfo> eventInfos) == false)
                {
                    eventInfos = new List<EventInfo>();
                    events[eventInfo.key] = eventInfos;
                }

                eventInfos.Add(eventInfo);
            }

            int removeCount = removeEvents.Count;
            for (int i = 0; i < removeCount; i++)
            {
                EventInfo eventInfo = removeEvents[i];
                if (this.events.TryGetValue(eventInfo.key, out List<EventInfo> eventInfos) == false) continue;
                eventInfos.Remove(eventInfo);
            }

            addEvents.Clear();
            removeEvents.Clear();
        }

        public void Subscribe(string key, Action action)
        {
            EventInfo eventInfo = new EventInfo(key, action);
            if (this.addEvents.Contains(eventInfo)) return;
            addEvents.Add(eventInfo);
        }

        public void Unsubscribe(string key, Action action)
        {
            EventInfo eventInfo = new EventInfo(key, action);
            if (this.removeEvents.Contains(eventInfo)) return;
            removeEvents.Add(eventInfo);
        }
    }
}