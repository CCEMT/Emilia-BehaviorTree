using System.Collections.Generic;
using Emilia.Node.Attributes;
using Emilia.Node.Editor;
using Emilia.Node.Universal.Editor;
using Sirenix.OdinInspector;
using UnityEngine;

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
        protected override bool TryGetDefaultNodeColor(out Color color)
        {
            color = CreateNodeColor(0.949f, 0.788f, 0.298f);
            return true;
        }

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
