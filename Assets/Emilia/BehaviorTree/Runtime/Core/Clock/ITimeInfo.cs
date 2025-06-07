namespace Emilia.BehaviorTree
{
    public interface ITimeInfo
    {
        void Add(ITimeInfo other);
        int CompareTo(ITimeInfo other);

        ITimeInfo GetDefault();
    }
}