using Emilia.BehaviorTree.Attributes;
using Emilia.Kit.Editor;
using Emilia.Node.Editor;
using Sirenix.Utilities;

namespace Emilia.BehaviorTree.Editor
{
    public class BehaviorTreeCreateNodeHandle : CreateNodeHandle<NodeAsset>
    {
        protected object _userData;
        protected string _path;
        protected int _priority;

        public override object userData => _userData;
        public override string path => _path;
        public override int priority => _priority;

        public override void Initialize(object weakSmartValue)
        {
            base.Initialize(weakSmartValue);

            BehaviorTreeMenuAttribute menuAttribute = this.value.nodeType.GetCustomAttribute<BehaviorTreeMenuAttribute>();
            if (menuAttribute != null)
            {
                this._path = menuAttribute.path;
                this._priority = menuAttribute.priority;
            }

            _userData = ReflectUtility.CreateInstance(this.value.nodeType);
        }
    }
}