using System;
using System.Collections.Generic;
using UnityEngine;

namespace Emilia.BehaviorTree
{
    public partial class Clock
    {
        public ITimeInfo time { get; private set; }

        private List<Action> updateEventList = new List<Action>();

        private List<Timer> timerList = new List<Timer>();

        private List<Action> removeUpdateEvent = new List<Action>();
        private List<Action> addUpdateEvent = new List<Action>();

        private List<Timer> removeTimerList = new List<Timer>();
        private List<Timer> addTimerList = new List<Timer>();

        public void Reset(ITimeInfo startTime)
        {
            time = startTime;

            updateEventList.Clear();
            timerList.Clear();

            removeUpdateEvent.Clear();
            addUpdateEvent.Clear();
            addTimerList.Clear();
        }

        public Timer AddTimer(Action action)
        {
            return AddTimer(() => null, 0, action);
        }

        public Timer AddTimer(ITimeInfo time, Action action)
        {
            return AddTimer(() => time, 0, action);
        }

        public Timer AddTimer(ITimeInfo time, int loop, Action action)
        {
            return AddTimer(() => time, loop, action);
        }

        public Timer AddTimer(Func<ITimeInfo> getTime, int loop, Action action)
        {
            Timer timer = Timer.Create();
            timer.time = getTime();
            timer.getTime = getTime;
            timer.loop = loop;
            timer.action = action;
            
            addTimerList.Add(timer);

            return timer;
        }

        public void RemoveTimer(Timer timer)
        {
            removeTimerList.Add(timer);
        }

        public void AddUpdateEvent(Action action)
        {
            if (updateEventList.Contains(action)) return;
            if (addUpdateEvent.Contains(action)) return;
            addUpdateEvent.Add(action);
        }

        public void RemoveUpdateEvent(Action action)
        {
            if (removeUpdateEvent.Contains(action)) return;
            removeUpdateEvent.Add(action);
        }

        public void Update(ITimeInfo deltaTime)
        {
            time.Add(deltaTime);

            int updateEventCount = updateEventList.Count;
            for (int i = 0; i < updateEventCount; i++)
            {
                Action action = updateEventList[i];
                if (removeUpdateEvent.Contains(action)) continue;
                action.Invoke();
            }

            int timerCount = timerList.Count;
            for (int i = 0; i < timerCount; i++)
            {
                Timer timer = timerList[i];
                if (removeTimerList.Contains(timer)) continue;

                if (timer.time != null)
                {
                    int compare = timer.time.CompareTo(time);
                    if (compare >= 0) continue;
                }

                timer.action.Invoke();

                if (timer.loop == 0) this.removeTimerList.Add(timer);
                else if (timer.loop > 0)
                {
                    timer.loop--;
                    timer.time.Add(timer.getTime.Invoke());
                }
            }

            int addUpdateEventCount = addUpdateEvent.Count;
            for (int i = 0; i < addUpdateEventCount; i++)
            {
                Action action = addUpdateEvent[i];
                updateEventList.Add(action);
            }

            int removeUpdateEventCount = removeUpdateEvent.Count;
            for (int i = 0; i < removeUpdateEventCount; i++)
            {
                Action action = removeUpdateEvent[i];
                updateEventList.Remove(action);
            }

            int addTimerListCount = addTimerList.Count;
            for (int i = 0; i < addTimerListCount; i++)
            {
                Timer timer = addTimerList[i];
                timerList.Add(timer);
            }

            int removeTimerListCount = removeTimerList.Count;
            for (int i = 0; i < removeTimerListCount; i++)
            {
                Timer timer = removeTimerList[i];

                timerList.Remove(timer);
                timer.Release();
            }

            removeUpdateEvent.Clear();
            addUpdateEvent.Clear();

            removeTimerList.Clear();
            addTimerList.Clear();
        }
    }
}