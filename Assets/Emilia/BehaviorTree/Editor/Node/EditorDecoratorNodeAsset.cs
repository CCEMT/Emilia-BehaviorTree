using System.Collections.Generic;
using Emilia.Node.Attributes;
using Emilia.Node.Editor;
using Sirenix.OdinInspector;

namespace Emilia.BehaviorTree.Editor
{
    [HideMonoScript]
    public class EditorDecoratorNodeAsset : EditorBehaviorTreeNodeAsset { }

    [EditorNode(typeof(EditorDecoratorNodeAsset))]
    public class EditorDecoratorNodeView : EditorBehaviorTreeNodeView
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
            
            portInfos.Add(output);

            return portInfos;
        }
    }
}