using System;
using System.Collections.Generic;
using Emilia.Reference;
using Emilia.Variables;
using UnityEngine;

namespace Emilia.BehaviorTree
{
    [Serializable]
    public class BehaviorTreeAsset
    {
        [SerializeField]
        private string _id;

        [SerializeField]
        private string _description;

        [SerializeField]
        private int _entryId;

        [SerializeField]
        private List<NodeAsset> _nodes;

        [SerializeField]
        private VariablesManager _variablesManage;

        [SerializeField]
        private object _userData;

        public string id => this._id;
        public string description => this._description;
        public int entryId => this._entryId;
        public IReadOnlyList<NodeAsset> nodes => this._nodes;
        public VariablesManager variablesManage => this._variablesManage;

        public object userData
        {
            get => this._userData;
            internal set => this._userData = value;
        }

        public BehaviorTreeAsset(string id, string description, int entryId, List<NodeAsset> nodes, VariablesManager variablesManage)
        {
            this._id = id;
            this._description = description;
            this._entryId = entryId;
            this._nodes = nodes;
            this._variablesManage = variablesManage;
        }
    }

    public class BehaviorTree : IReference
    {
        private bool isInit = false;
        private List<Node> _nodes = new List<Node>();
        private Dictionary<int, Node> _nodeMap = new Dictionary<int, Node>();

        public int uid { get; private set; }
        public BehaviorTreeAsset asset { get; private set; }
        public BehaviorTree parent { get; private set; }
        public Blackboard blackboard { get; private set; }
        public Node entryNode { get; private set; }
        public object owner { get; private set; }
        public Clock clock { get; private set; }
        public bool isActive { get; private set; }

        public List<BehaviorTree> children { get; private set; } = new List<BehaviorTree>();
        public IReadOnlyList<Node> nodes => this._nodes;
        public IReadOnlyDictionary<int, Node> nodeMap => this._nodeMap;

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(int uid, BehaviorTreeAsset asset, Clock clock, object owner = default, BehaviorTree parent = default)
        {
            this.uid = uid;
            this.asset = asset;
            this.owner = owner;

            this.clock = clock;
            blackboard = new Blackboard(clock, asset.variablesManage);

            this.parent = parent;

            int nodeCount = asset.nodes.Count;
            for (int i = 0; i < nodeCount; i++)
            {
                NodeAsset nodeAsset = asset.nodes[i];
                Node node = nodeAsset.CreateNode();
                node.Init(this, nodeAsset);

                this._nodes.Add(node);
                this._nodeMap[node.id] = node;
            }

            entryNode = this._nodeMap[asset.entryId];

            this.isInit = true;
        }

        /// <summary>
        /// 开始
        /// </summary>
        public void Start()
        {
            if (this.isInit == false) return;
            isActive = true;
            entryNode.Start();
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            isActive = false;
            entryNode.Stop();
        }

        /// <summary>
        /// 子对象停止
        /// </summary>
        public void ChildStop()
        {
            if (isActive) clock.AddTimer(Start);
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            int nodeCount = this._nodes.Count;
            for (int i = 0; i < nodeCount; i++)
            {
                Node node = this._nodes[i];
                node.Dispose();
            }

            ReferencePool.Release(this);
        }

        void IReference.Clear()
        {
            this._nodes.Clear();
            this._nodeMap.Clear();

            uid = -1;
            asset = null;
            blackboard = null;
            entryNode = null;
            owner = null;

            isActive = false;
            this.isInit = false;
        }
    }
}