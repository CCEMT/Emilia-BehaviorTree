using Emilia.Kit;
using Emilia.Node.Editor;

namespace Emilia.BehaviorTree.Editor
{
    [EditorHandle(typeof(EditorBehaviorTreeAsset))]
    public class BehaviorTreeGraphPanelHandle : GraphPanelHandle
    {
        public override void LoadPanel(EditorGraphView graphView, GraphPanelSystem system)
        {
            base.LoadPanel(graphView, system);
            system.OpenDockPanel<BehaviorTreeToolbarView>(20, GraphDockPosition.Top);
        }
    }
}