using System;
using System.Collections.Generic;
using Emilia.Reference;
using UnityEngine;

namespace Emilia.BehaviorTree
{
    [Serializable]
    public abstract class NodeAsset
    {
        [SerializeField, HideInInspector]
        public readonly int id;

        [SerializeField, HideInInspector]
        private List<int> _childrenIds = new List<int>();

        public IReadOnlyList<int> childrenIds => _childrenIds;

        protected NodeAsset() { }
        public abstract Node CreateNode();
    }

    public abstract class Node : IReference
    {
        public enum State : byte
        {
            Inactive,
            Active,
            Stop,
        }

        private Node[] _children;

        public NodeAsset nodeAsset { get; private set; }
        public BehaviorTree tree { get; private set; }
        public Clock clock => tree.clock;
        public Blackboard blackboard => tree.blackboard;
        public object owner => tree.owner;

        public int id => nodeAsset.id;
        public Node parent { get; private set; }

        public IReadOnlyList<Node> children => _children;

        public State state { get; private set; }

        public void Init(BehaviorTree behaviorTree, NodeAsset nodeAsset)
        {
            tree = behaviorTree;
            this.nodeAsset = nodeAsset;

            _children = new Node[nodeAsset.childrenIds.Count];
            int childrenIdsCount = nodeAsset.childrenIds.Count;
            for (int i = 0; i < childrenIdsCount; i++)
            {
                int childrenId = nodeAsset.childrenIds[i];

                Node childrenNode = tree.nodeMap.GetValueOrDefault(childrenId);
                childrenNode.parent = this;

                _children[i] = childrenNode;
            }

            OnInit();
        }

        public void Start()
        {
            state = State.Active;
            OnStart();

            BehaviorTreeDebugUtility.Ping(this, "Start");
        }

        public void Stop()
        {
            state = State.Stop;
            OnStop();
        }

        public void ChildStop(Node child, bool result)
        {
            OnChildStop(child, result);
        }

        public void ParentCompositeStop(Composite composite)
        {
            OnParentCompositeStop(composite);
        }

        protected void Finish(bool result)
        {
            OnFinish(result);

            state = State.Inactive;

            if (parent != null) parent.ChildStop(this, result);
            else if (tree.entryNode == this) tree.ChildStop();

            BehaviorTreeDebugUtility.Ping(this, nameof(Finish) + ":" + result);
        }

        protected virtual void OnInit() { }

        protected virtual void OnStart() { }

        protected virtual void OnStop() { }

        protected virtual void OnChildStop(Node child, bool result) { }

        protected virtual void OnParentCompositeStop(Composite composite) { }

        protected virtual void OnFinish(bool result) { }

        public void Dispose()
        {
            ReferencePool.Release(this);
        }

        public void Clear()
        {
            state = State.Inactive;

            _children = null;
            nodeAsset = null;
            tree = null;

        }
    }
}