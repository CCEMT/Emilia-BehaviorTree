using System;
using Emilia.DataBuildPipeline.Editor;

namespace Emilia.BehaviorTree.Editor
{
    [BuildPipeline(typeof(BehaviorTreeBuildArgs)), BuildSequence(3000)]
    public class BehaviorTreeBuildAsset : IDataBuild
    {
        public void Build(IBuildContainer buildContainer, Action onFinished)
        {
            BehaviorTreeBuildContainer container = buildContainer as BehaviorTreeBuildContainer;

            string id = container.editorAsset.id;
            string description = container.editorAsset.description;
            BehaviorTreeAsset asset = new BehaviorTreeAsset(id, description, container.entryNodeId, container.nodeAssets, container.variablesManage);
            container.asset = asset;

            onFinished.Invoke();
        }
    }
}