using System;
using Emilia.DataBuildPipeline.Editor;

namespace Emilia.BehaviorTree.Editor
{
    [BuildPipeline(typeof(BehaviorTreeBuildArgs)), BuildSequence(3000)]
    public class BehaviorTreeBuildAsset : IDataBuild
    {
        public void Build(IBuildContainer buildContainer, IBuildArgs buildArgs, Action onFinished)
        {
            BehaviorTreeBuildContainer container = buildContainer as BehaviorTreeBuildContainer;
            BehaviorTreeBuildArgs behaviorTreeBuildArgs = buildArgs as BehaviorTreeBuildArgs;

            string id = behaviorTreeBuildArgs.behaviorTreeAsset.id;
            string description = behaviorTreeBuildArgs.behaviorTreeAsset.description;
            BehaviorTreeAsset asset = new BehaviorTreeAsset(id, description, container.entryNodeId, container.nodeAssets, container.variablesManage);
            container.asset = asset;

            onFinished.Invoke();
        }
    }
}