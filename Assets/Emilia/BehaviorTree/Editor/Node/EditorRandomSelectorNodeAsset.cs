using Emilia.Node.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Emilia.BehaviorTree.Editor
{
    [HideMonoScript]
    public class EditorRandomSelectorNodeAsset : EditorCompositeNodeAsset { }

    [EditorNode(typeof(EditorRandomSelectorNodeAsset))]
    public class EditorRandomSelectorNodeView : EditorCompositeNodeView
    {
        protected override bool TryGetDefaultNodeColor(out Color color)
        {
            color = CreateNodeColor(0.922f, 0.341f, 0.341f);
            return true;
        }
    }
}
