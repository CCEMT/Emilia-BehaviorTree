using System.Collections.Generic;
using Emilia.DataBuildPipeline.Editor;
using Emilia.Variables;

namespace Emilia.BehaviorTree.Editor
{
    [BuildPipeline(typeof(BehaviorTreeBuildArgs))]
    public class BehaviorTreeBuildContainer : BuildContainer
    {
        public Dictionary<int, string> bindMap { get; set; } = new Dictionary<int, string>();
        public VariablesManager variablesManage { get; set; }
        public List<NodeAsset> nodeAssets { get; set; }
        public int entryNodeId { get; set; }

        public BehaviorTreeAsset asset { get; set; }
    }
}