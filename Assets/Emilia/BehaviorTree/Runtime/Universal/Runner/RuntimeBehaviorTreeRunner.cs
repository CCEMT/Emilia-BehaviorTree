using Emilia.Reference;
using UnityEngine;

namespace Emilia.BehaviorTree
{
    public class RuntimeBehaviorTreeRunner : IBehaviorTreeRunner, IReference
    {
        private BehaviorTree _behaviorTree;

        public int uid { get; private set; } = -1;
        public string fileName { get; private set; }
        public BehaviorTreeAsset asset => _behaviorTree.asset;
        public BehaviorTree behaviorTree => _behaviorTree;
        public bool isActive => behaviorTree.isActive;

        public void Init(string fileName, IBehaviorTreeLoader loader, Clock clock, object owner = null)
        {
            this.fileName = fileName;
            uid = BehaviorTreeRunnerUtility.GetId();

            string fullPath = $"{loader.runtimeFilePath}/{fileName}.bytes";
            TextAsset textAsset = loader.LoadAsset(fullPath) as TextAsset;
            BehaviorTreeAsset behaviorTreeAsset = loader.LoadBehaviorTreeAsset(textAsset.bytes);
            loader.ReleaseAsset(fullPath);

            this._behaviorTree = ReferencePool.Acquire<BehaviorTree>();
            this._behaviorTree.Init(uid, behaviorTreeAsset, clock, owner);
        }

        public void Start()
        {
            if (_behaviorTree == null) return;
            _behaviorTree.Start();
        }

        public void Stop()
        {
            if (_behaviorTree == null) return;
            if (_behaviorTree.isActive == false) return;
            _behaviorTree.Stop();
        }

        public void Dispose()
        {
            _behaviorTree?.Dispose();

            ReferencePool.Release(this);
        }

        void IReference.Clear()
        {
            if (uid != -1) BehaviorTreeRunnerUtility.RecycleId(uid);
            uid = -1;

            fileName = null;

            _behaviorTree = null;
        }
    }
}