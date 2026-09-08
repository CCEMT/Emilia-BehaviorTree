using Emilia.Node.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Emilia.BehaviorTree.Editor
{
    [HideMonoScript]
    public class EditorParallelNodeAsset : EditorCompositeNodeAsset { }

    [EditorNode(typeof(EditorParallelNodeAsset))]
    public class EditorParallelNodeView : EditorCompositeNodeView
    {
        protected override bool TryGetDefaultNodeColor(out Color color)
        {
            color = CreateNodeColor(0.608f, 0.318f, 0.878f);
            return true;
        }
    }
}
