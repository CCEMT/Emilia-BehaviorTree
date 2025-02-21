using System.Collections.Generic;
using Emilia.Node.Attributes;
using Emilia.Node.Editor;
using Sirenix.OdinInspector;

namespace Emilia.BehaviorTree.Editor
{
    [HideMonoScript]
    public class EditorRootNodeAsset : EditorBehaviorTreeNodeAsset
    {
        public override string title => "入口";
    }

    [EditorNode(typeof(EditorRootNodeAsset))]
    public class EditorRootNodeView : EditorBehaviorTreeNodeView
    {
        public override List<EditorPortInfo> CollectStaticPortAssets()
        {
            List<EditorPortInfo> portInfos = new List<EditorPortInfo>();

            EditorPortInfo output = new EditorPortInfo();
            output.id = OutputPortName;
            output.direction = EditorPortDirection.Output;
            output.orientation = EditorOrientation.Vertical;

            portInfos.Add(output);

            return portInfos;
        }
    }
}