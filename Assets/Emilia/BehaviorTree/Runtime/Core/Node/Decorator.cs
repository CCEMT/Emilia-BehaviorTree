﻿using Emilia.Reference;

namespace Emilia.BehaviorTree
{
    public abstract class DecoratorAsset : NodeAsset { }

    public abstract class Decorator : Node
    {
        private Node _decoratedNode;

        public Node decoratedNode => this._decoratedNode;

        protected override void OnInit()
        {
            this._decoratedNode = children[0];
        }

        protected override void OnParentCompositeStop(Composite composite)
        {
            base.OnParentCompositeStop(composite);
            decoratedNode.ParentCompositeStop(composite);
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            _decoratedNode = null;
        }
    }

    public abstract class DecoratorAsset<T> : DecoratorAsset where T : Node, new()
    {
        public override Node CreateNode() => ReferencePool.Acquire<T>();
    }

    public abstract class Decorator<T> : Decorator where T : NodeAsset
    {
        protected T asset;

        protected override void OnInit()
        {
            base.OnInit();
            asset = nodeAsset as T;
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            asset = null;
        }
    }
}