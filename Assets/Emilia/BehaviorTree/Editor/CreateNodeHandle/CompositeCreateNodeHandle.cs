using System;
using System.Reflection;
using Emilia.BehaviorTree.Attributes;
using Emilia.Kit;
using Emilia.Kit.Editor;
using Emilia.Node.Editor;

namespace Emilia.BehaviorTree.Editor
{
    [EditorHandle(typeof(CompositeAsset))]
    public class CompositeCreateNodeHandle : CreateNodeHandle
    {
        public override void Initialize(object arg)
        {
            base.Initialize(arg);

            CreateNodeHandleContext context = (CreateNodeHandleContext) arg;
            editorNodeType = GetEditorNodeType(context.nodeType);

            BehaviorTreeMenuAttribute menuAttribute = context.nodeType.GetCustomAttribute<BehaviorTreeMenuAttribute>();
            if (menuAttribute != null)
            {
                path = menuAttribute.path;
                priority = menuAttribute.priority;
            }

            nodeData = ReflectUtility.CreateInstance(context.nodeType);
        }

        private static Type GetEditorNodeType(Type nodeType)
        {
            if (nodeType == typeof(SequenceAsset)) return typeof(EditorSequenceNodeAsset);
            if (nodeType == typeof(SelectorAsset)) return typeof(EditorSelectorNodeAsset);
            if (nodeType == typeof(ParallelAsset)) return typeof(EditorParallelNodeAsset);
            if (nodeType == typeof(RandomSequenceAsset)) return typeof(EditorRandomSequenceNodeAsset);
            if (nodeType == typeof(RandomSelectorAsset)) return typeof(EditorRandomSelectorNodeAsset);

            return typeof(EditorCompositeNodeAsset);
        }
    }
}