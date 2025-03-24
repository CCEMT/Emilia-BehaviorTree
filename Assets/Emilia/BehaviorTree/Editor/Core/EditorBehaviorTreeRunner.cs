using System.Collections.Generic;
using UnityEditor;

namespace Emilia.BehaviorTree.Editor
{
    public class EditorBehaviorTreeRunner : IBehaviorTreeRunner
    {
        private static Dictionary<string, List<EditorBehaviorTreeRunner>> _runnerByAssetId = new Dictionary<string, List<EditorBehaviorTreeRunner>>();
        private static Dictionary<int, EditorBehaviorTreeRunner> _runnerByUid = new Dictionary<int, EditorBehaviorTreeRunner>();

        public static readonly Dictionary<int, Queue<EditorBehaviorTreeDebugPingMessage>> nodeMessage = new Dictionary<int, Queue<EditorBehaviorTreeDebugPingMessage>>();
        public static IReadOnlyDictionary<string, List<EditorBehaviorTreeRunner>> runnerByAssetId => _runnerByAssetId;
        public static IReadOnlyDictionary<int, EditorBehaviorTreeRunner> runnerByUid => _runnerByUid;

        private IBehaviorTreeLoader behaviorTreeLoader;
        private EditorBehaviorTreeAsset _editorBehaviorTreeAsset;

        private BehaviorTree _behaviorTree;

        public int uid { get; private set; }
        public BehaviorTreeAsset asset => _behaviorTree.asset;
        public BehaviorTree behaviorTree => _behaviorTree;
        public bool isActive => _behaviorTree.isActive;

        public EditorBehaviorTreeAsset editorBehaviorTreeAsset => _editorBehaviorTreeAsset;

        public void Init(string fileName, IBehaviorTreeLoader loader, Clock clock, object owner = null)
        {
            uid = BehaviorTreeRunnerUtility.GetId();
            this.behaviorTreeLoader = loader;

            string fullPath = $"{loader.editorFilePath}/{fileName}.asset";
            EditorBehaviorTreeAsset loadAsset = AssetDatabase.LoadAssetAtPath<EditorBehaviorTreeAsset>(fullPath);
            this._editorBehaviorTreeAsset = loadAsset;

            this._behaviorTree = new BehaviorTree();
            this._behaviorTree.Init(uid, loadAsset.cache, clock, owner);

            if (_runnerByAssetId.ContainsKey(loadAsset.id) == false) _runnerByAssetId[loadAsset.id] = new List<EditorBehaviorTreeRunner>();
            _runnerByAssetId[loadAsset.id].Add(this);

            _runnerByUid[uid] = this;
        }

        public void Reload(BehaviorTreeAsset behaviorTreeAsset)
        {
            bool isStart = this.isActive;

            Clock clock = this._behaviorTree.clock;
            object owner = this._behaviorTree.owner;

            if (isStart) this.Stop();
            if (this._behaviorTree != null) this._behaviorTree.Dispose();

            this._behaviorTree = new BehaviorTree();
            this._behaviorTree.Init(uid, behaviorTreeAsset, clock, owner);
            if (isStart) this.Start();
        }

        public void Start()
        {
            if (this._behaviorTree == null) return;
            this._behaviorTree.Start();
        }

        public void Stop()
        {
            if (this._behaviorTree == null) return;
            this._behaviorTree.Stop();
        }

        public void Dispose()
        {
            if (_runnerByUid.ContainsKey(uid)) _runnerByUid.Remove(uid);
            if (nodeMessage.ContainsKey(uid)) nodeMessage.Remove(uid);

            BehaviorTreeRunnerUtility.RecycleId(uid);
            uid = -1;

            if (this._behaviorTree == null) return;

            if (_runnerByAssetId.ContainsKey(this._editorBehaviorTreeAsset.id)) _runnerByAssetId[this._editorBehaviorTreeAsset.id].Remove(this);

            this._behaviorTree.Dispose();
            _behaviorTree = null;
        }
    }
}