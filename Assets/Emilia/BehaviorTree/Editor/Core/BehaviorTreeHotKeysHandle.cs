using System.Linq;
using Emilia.Kit;
using Emilia.Node.Editor;
using Emilia.Node.Universal.Editor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Emilia.BehaviorTree.Editor
{
    [EditorHandle(typeof(EditorBehaviorTreeAsset))]
    public class BehaviorTreeHotKeysHandle : UniversalGraphHotKeysHandle
    {
        public override void OnKeyDown(EditorGraphView graphView, KeyDownEvent evt)
        {
            base.OnKeyDown(graphView, evt);
            if (evt.ctrlKey && evt.keyCode == KeyCode.Q)
            {
                const float Interval = 50;

                GraphLayoutUtility.AlignmentType alignmentType = GraphLayoutUtility.AlignmentType.Vertical | GraphLayoutUtility.AlignmentType.Center;
                GraphLayoutUtility.Start(Interval, alignmentType, graphView.graphSelected.selected.OfType<IEditorNodeView>().ToList());
            }
        }
    }
}