using Emilia.Node.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Emilia.BehaviorTree.Editor
{
    [HideMonoScript]
    public class EditorRandomSequenceNodeAsset : EditorCompositeNodeAsset { }

    [EditorNode(typeof(EditorRandomSequenceNodeAsset))]
    public class EditorRandomSequenceNodeView : EditorCompositeNodeView
    {
        protected override bool TryGetDefaultNodeColor(out Color color)
        {
            color = CreateNodeColor(0.337f, 0.800f, 0.949f);
            return true;
        }
    }
}
