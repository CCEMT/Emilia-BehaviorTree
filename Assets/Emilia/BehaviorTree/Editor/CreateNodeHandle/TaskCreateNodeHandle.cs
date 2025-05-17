using Emilia.BehaviorTree.Attributes;
using Emilia.Kit;
using Emilia.Kit.Editor;
using Emilia.Node.Editor;
using Sirenix.Utilities;

namespace Emilia.BehaviorTree.Editor
{
    [EditorHandle(typeof(TaskAsset))]
    public class TaskCreateNodeHandle : CreateNodeHandle
    {
        public override void Initialize(object arg)
        {
            base.Initialize(arg);

            CreateNodeHandleContext context = (CreateNodeHandleContext) arg;
            editorNodeType = typeof(EditorTaskNodeAsset);

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