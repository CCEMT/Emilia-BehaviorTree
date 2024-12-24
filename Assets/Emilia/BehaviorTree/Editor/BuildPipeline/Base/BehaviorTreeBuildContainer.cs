using System.Collections.Generic;
using Emilia.DataBuildPipeline.Editor;
using Emilia.Variables;

namespace Emilia.BehaviorTree.Editor
{
    public class BehaviorTreeBuildContainer : BuildContainer
    {
        public EditorBehaviorTreeAsset editorAsset { get; set; }

        public Dictionary<int, string> bindMap { get; set; } = new Dictionary<int, string>();
        public VariablesManage variablesManage { get; set; }
        public List<NodeAsset> nodeAssets { get; set; }
        public int entryNodeId { get; set; }

        public BehaviorTreeAsset asset { get; set; }
    }
}