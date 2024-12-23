using System;
using System.Collections.Generic;

namespace Emilia.BehaviorTree
{
    public partial class Clock
    {
        public class Timer
        {
            public ITimeInfo time;
            public Func<ITimeInfo> getTime;

            public int loop;
            public Action action;

            private static readonly Queue<Timer> pool = new Queue<Timer>();

            public static Timer Create()
            {
                if (pool.Count != 0) return pool.Dequeue();
                return new Timer();
            }

            public void Release()
            {
                time = null;
                loop = 0;
                action = null;
                pool.Enqueue(this);
            }
        }
    }
}