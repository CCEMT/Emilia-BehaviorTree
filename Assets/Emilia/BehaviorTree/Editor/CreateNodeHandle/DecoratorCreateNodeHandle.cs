using System;
using Emilia.BehaviorTree.Attributes;
using Emilia.Kit.Editor;
using Emilia.Node.Editor;
using Sirenix.Utilities;

namespace Emilia.BehaviorTree.Editor
{
    public class DecoratorCreateNodeHandle : CreateNodeHandle<DecoratorAsset>
    {
        protected object _nodeData;
        protected string _path;
        protected int _priority;

        public override object nodeData => this._nodeData;
        public override string path => _path;
        public override int priority => _priority;
        public override Type editorNodeType => typeof(EditorDecoratorNodeAsset);

        public override void Initialize(object weakSmartValue)
        {
            base.Initialize(weakSmartValue);

            BehaviorTreeMenuAttribute menuAttribute = this.value.nodeType.GetCustomAttribute<BehaviorTreeMenuAttribute>();
            if (menuAttribute != null)
            {
                this._path = menuAttribute.path;
                this._priority = menuAttribute.priority;
            }

            this._nodeData = ReflectUtility.CreateInstance(this.value.nodeType);
        }
    }
}