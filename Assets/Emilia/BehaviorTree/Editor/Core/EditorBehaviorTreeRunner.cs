using System;
using System.Collections.Generic;
using Emilia.Kit;
using UnityEditor;
using UnityEngine;

namespace Emilia.BehaviorTree.Editor
{
    public class EditorBehaviorTreeRunner : IBehaviorTreeRunner
    {
        private static Dictionary<string, List<EditorBehaviorTreeRunner>> _runnerByAssetId = new Dictionary<string, List<EditorBehaviorTreeRunner>>();
        private static Dictionary<int, EditorBehaviorTreeRunner> _runnerByUid = new Dictionary<int, EditorBehaviorTreeRunner>();

        public static readonly Dictionary<int, Queue<EditorBehaviorTreeDebugPingMessage>> nodeMessage = new Dictionary<int, Queue<EditorBehaviorTreeDebugPingMessage>>();
        public static IReadOnlyDictionary<string, List<EditorBehaviorTreeRunner>> runnerByAssetId => _runnerByAssetId;
        public static IReadOnlyDictionary<int, EditorBehaviorTreeRunner> runnerByUid => _runnerByUid;

        private BehaviorTree _behaviorTree;
        private object owner;

        public int uid { get; private set; }
        public string fileName { get; private set; }
        public BehaviorTreeAsset asset => _behaviorTree.asset;
        public BehaviorTree behaviorTree => _behaviorTree;
        public bool isActive => _behaviorTree.isActive;

        public void Init(string fileName, IBehaviorTreeLoader loader, Clock clock, object owner = null)
        {
            try
            {
                this.fileName = fileName;

                string fullPath = $"{loader.editorFilePath}/{fileName}.asset";
                EditorBehaviorTreeAsset loadAsset = AssetDatabase.LoadAssetAtPath<EditorBehaviorTreeAsset>(fullPath);

                Init(loadAsset.cache, clock, owner);
            }
            catch (Exception e)
            {
                Debug.LogError($"{owner} Init 时出现错误：\n{e.ToUnityLogString()}");
                _behaviorTree?.Dispose();
            }
        }

        public void Init(BehaviorTreeAsset behaviorTreeAsset, Clock clock, object owner = null)
        {
            uid = BehaviorTreeRunnerUtility.GetId();
            this.owner = owner;

            this._behaviorTree = new BehaviorTree();
            this._behaviorTree.Init(uid, behaviorTreeAsset, clock, owner);

            if (_runnerByAssetId.ContainsKey(behaviorTreeAsset.id) == false) _runnerByAssetId[behaviorTreeAsset.id] = new List<EditorBehaviorTreeRunner>();
            _runnerByAssetId[behaviorTreeAsset.id].Add(this);

            _runnerByUid[uid] = this;
        }

        public void Reload(BehaviorTreeAsset behaviorTreeAsset)
        {
            bool isStart = this.isActive;

            Clock clock = this._behaviorTree.clock;
            object newOwner = this._behaviorTree.owner;

            if (isStart) this.Stop();
            if (this._behaviorTree != null) this._behaviorTree.Dispose();

            this._behaviorTree = new BehaviorTree();
            this._behaviorTree.Init(uid, behaviorTreeAsset, clock, newOwner);
            if (isStart) this.Start();
        }

        public void Start()
        {
            if (this._behaviorTree == null) return;

            try { this._behaviorTree.Start(); }
            catch (Exception e)
            {
                Debug.LogError($"{owner} Start 时出现错误：\n{e.ToUnityLogString()}");
                _behaviorTree.Dispose();
            }
        }

        public void Stop()
        {
            if (this._behaviorTree == null) return;

            try { this._behaviorTree.Stop(); }
            catch (Exception e)
            {
                Debug.LogError($"{owner} Stop 时出现错误：\n{e.ToUnityLogString()}");
                _behaviorTree.Dispose();
            }
        }

        public void Dispose()
        {
            try
            {
                if (_runnerByUid.ContainsKey(uid)) _runnerByUid.Remove(uid);
                if (nodeMessage.ContainsKey(uid)) nodeMessage.Remove(uid);

                if (uid != -1) BehaviorTreeRunnerUtility.RecycleId(uid);
                uid = -1;

                fileName = null;

                if (this._behaviorTree == null) return;

                if (_runnerByAssetId.ContainsKey(asset.id)) _runnerByAssetId[asset.id].Remove(this);

                if (behaviorTree.isActive) this._behaviorTree.Dispose();
                _behaviorTree = null;

                owner = null;
            }
            catch (Exception e) { Debug.LogError($"{owner} Dispose 时出现错误：\n{e.ToUnityLogString()}"); }
        }
    }
}