namespace Emilia.BehaviorTree
{
    public struct FloatTimeInfo : ITimeInfo
    {
        public float time;

        public FloatTimeInfo(float time)
        {
            this.time = time;
        }

        public void Add(ITimeInfo other)
        {
            FloatTimeInfo otherTime = (FloatTimeInfo) other;
            this.time += otherTime.time;
        }

        public int CompareTo(ITimeInfo other)
        {
            FloatTimeInfo otherTime = (FloatTimeInfo) other;
            return time.CompareTo(otherTime.time);
        }
    }
}