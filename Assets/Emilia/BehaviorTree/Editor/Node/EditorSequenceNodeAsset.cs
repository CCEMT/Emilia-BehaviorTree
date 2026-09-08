using Emilia.Node.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Emilia.BehaviorTree.Editor
{
    [HideMonoScript]
    public class EditorSequenceNodeAsset : EditorCompositeNodeAsset { }

    [EditorNode(typeof(EditorSequenceNodeAsset))]
    public class EditorSequenceNodeView : EditorCompositeNodeView
    {
        protected override bool TryGetDefaultNodeColor(out Color color)
        {
            color = CreateNodeColor(0.184f, 0.502f, 0.929f);
            return true;
        }
    }
}
