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
            private bool isInPool;

            public static Timer Create()
            {
                while (pool.Count != 0)
                {
                    Timer timer = pool.Dequeue();
                    if (timer.isInPool == false) continue;

                    timer.isInPool = false;
                    return timer;
                }

                return new Timer();
            }

            public void Release()
            {
                if (isInPool) return;

                time = null;
                getTime = null;
                loop = 0;
                action = null;

                isInPool = true;
                pool.Enqueue(this);
            }
        }
    }
}
