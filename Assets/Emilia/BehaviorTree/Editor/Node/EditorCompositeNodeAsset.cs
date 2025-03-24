using System.Collections.Generic;
using Emilia.Node.Attributes;
using Emilia.Node.Editor;
using Sirenix.OdinInspector;

namespace Emilia.BehaviorTree.Editor
{
    [HideMonoScript]
    public class EditorCompositeNodeAsset : EditorBehaviorTreeNodeAsset { }

    [EditorNode(typeof(EditorCompositeNodeAsset))]
    public class EditorCompositeNodeView : EditorBehaviorTreeNodeView
    {
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
}