using System.Reflection;
using Emilia.BehaviorTree.Attributes;
using Emilia.Kit;
using Emilia.Kit.Editor;
using Emilia.Node.Editor;

namespace Emilia.BehaviorTree.Editor
{
    [EditorHandle(typeof(NodeAsset))]
    public class BehaviorTreeCreateNodeHandle : CreateNodeHandle
    {
        public override void Initialize(object arg)
        {
            base.Initialize(arg);

            CreateNodeHandleContext context = (CreateNodeHandleContext) arg;
            BehaviorTreeMenuAttribute menuAttribute = context.nodeType.GetCustomAttribute<BehaviorTreeMenuAttribute>();
            if (menuAttribute != null)
            {
                path = menuAttribute.path;
                priority = menuAttribute.priority;
            }

            nodeData = ReflectUtility.CreateInstance(context.nodeType);
        }
    }
}