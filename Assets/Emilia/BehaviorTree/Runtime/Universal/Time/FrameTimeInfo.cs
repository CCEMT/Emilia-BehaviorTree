namespace Emilia.BehaviorTree
{
    public struct FrameTimeInfo : ITimeInfo
    {
        public static FrameTimeInfo One => new FrameTimeInfo(1);

        public int frame;

        public FrameTimeInfo(int frame)
        {
            this.frame = frame;
        }

        public void Add(ITimeInfo other)
        {
            FrameTimeInfo otherTime = (FrameTimeInfo) other;
            this.frame += otherTime.frame;
        }

        public int CompareTo(ITimeInfo other)
        {
            FrameTimeInfo otherTime = (FrameTimeInfo) other;
            return frame.CompareTo(otherTime.frame);
        }
    }
}