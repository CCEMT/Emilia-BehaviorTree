using Emilia.Node.Editor;

namespace Emilia.BehaviorTree.Editor
{
    public class BehaviorTreeGraphPanelHandle : GraphPanelHandle<EditorBehaviorTreeAsset>
    {
        public override void LoadPanel(GraphPanelSystem system)
        {
            system.OpenDockPanel<BehaviorTreeToolbarView>(20, GraphDockPosition.Top);
        }
    }
}