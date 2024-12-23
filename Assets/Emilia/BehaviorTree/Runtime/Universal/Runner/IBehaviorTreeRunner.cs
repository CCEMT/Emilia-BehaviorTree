namespace Emilia.BehaviorTree
{
    public interface IBehaviorTreeRunner
    {
        int uid { get; }

        BehaviorTreeAsset asset { get; }

        BehaviorTree behaviorTree { get; }

        bool isActive { get; }

        void Init(string fileName, IBehaviorTreeLoader loader, Clock clock, object owner = null);

        void Start();

        void Stop();

        void Dispose();
    }
}