using System.Collections.Generic;
using Emilia.Node.Attributes;
using Emilia.Node.Editor;
using Emilia.Node.Universal.Editor;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Emilia.BehaviorTree.Editor
{
    [HideMonoScript]
    public class EditorTaskNodeAsset : EditorBehaviorTreeNodeAsset { }

    [EditorNode(typeof(EditorTaskNodeAsset))]
    public class EditorTaskNodeView : EditorBehaviorTreeNodeView
    {
        protected override bool TryGetDefaultNodeColor(out Color color)
        {
            color = CreateNodeColor(0.510f, 0.510f, 0.510f);
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

            return portInfos;
        }
    }
}
