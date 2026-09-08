using Emilia.Node.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Emilia.BehaviorTree.Editor
{
    [HideMonoScript]
    public class EditorSelectorNodeAsset : EditorCompositeNodeAsset { }

    [EditorNode(typeof(EditorSelectorNodeAsset))]
    public class EditorSelectorNodeView : EditorCompositeNodeView
    {
        protected override bool TryGetDefaultNodeColor(out Color color)
        {
            color = CreateNodeColor(0.851f, 0.310f, 0.439f);
            return true;
        }
    }
}
