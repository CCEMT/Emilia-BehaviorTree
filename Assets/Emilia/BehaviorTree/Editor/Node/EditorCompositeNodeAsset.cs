using System.Collections.Generic;
using Emilia.Node.Attributes;
using Emilia.Node.Editor;
using Emilia.Node.Universal.Editor;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Emilia.BehaviorTree.Editor
{
    [HideMonoScript]
    public class EditorCompositeNodeAsset : EditorBehaviorTreeNodeAsset { }

    [HideMonoScript]
    public class EditorSequenceNodeAsset : EditorCompositeNodeAsset { }

    [HideMonoScript]
    public class EditorSelectorNodeAsset : EditorCompositeNodeAsset { }

    [HideMonoScript]
    public class EditorParallelNodeAsset : EditorCompositeNodeAsset { }

    [HideMonoScript]
    public class EditorRandomSequenceNodeAsset : EditorCompositeNodeAsset { }

    [HideMonoScript]
    public class EditorRandomSelectorNodeAsset : EditorCompositeNodeAsset { }

    [EditorNode(typeof(EditorCompositeNodeAsset))]
    public class EditorCompositeNodeView : EditorBehaviorTreeNodeView
    {
        protected override bool TryGetDefaultNodeColor(out Color color)
        {
            color = CreateNodeColor(0.306f, 0.804f, 0.769f);
            return true;
        }

        public override List<EditorPortInfo> CollectStaticPortAssets()
        {
            List<EditorPortInfo> portInfos = new List<EditorPortInfo>();

            EditorPortInfo input = new EditorPortInfo();
            input.id = InputPortName;
            input.direction = EditorPortDirection.Input;
            input.orientation = EditorOrientation.Vertical;

            portInfos.Add(input);

            EditorPortInfo output = new EditorPortInfo();
            output.id = OutputPortName;
            output.direction = EditorPortDirection.Output;
            output.orientation = EditorOrientation.Vertical;
            output.canMultiConnect = true;

            portInfos.Add(output);

            return portInfos;
        }
    }

    [EditorNode(typeof(EditorSequenceNodeAsset))]
    public class EditorSequenceNodeView : EditorCompositeNodeView
    {
        protected override bool TryGetDefaultNodeColor(out Color color)
        {
            color = CreateNodeColor(0.184f, 0.502f, 0.929f);
            return true;
        }
    }

    [EditorNode(typeof(EditorSelectorNodeAsset))]
    public class EditorSelectorNodeView : EditorCompositeNodeView
    {
        protected override bool TryGetDefaultNodeColor(out Color color)
        {
            color = CreateNodeColor(0.851f, 0.310f, 0.439f);
            return true;
        }
    }

    [EditorNode(typeof(EditorParallelNodeAsset))]
    public class EditorParallelNodeView : EditorCompositeNodeView
    {
        protected override bool TryGetDefaultNodeColor(out Color color)
        {
            color = CreateNodeColor(0.608f, 0.318f, 0.878f);
            return true;
        }
    }

    [EditorNode(typeof(EditorRandomSequenceNodeAsset))]
    public class EditorRandomSequenceNodeView : EditorCompositeNodeView
    {
        protected override bool TryGetDefaultNodeColor(out Color color)
        {
            color = CreateNodeColor(0.337f, 0.800f, 0.949f);
            return true;
        }
    }

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
