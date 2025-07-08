using System;
using Emilia.DataBuildPipeline.Editor;
using Emilia.Kit.Editor;

namespace Emilia.BehaviorTree.Editor
{
    [BuildPipeline(typeof(BehaviorTreeBuildArgs)), BuildSequence(1000)]
    public class BehaviorTreeOutputEditor : IDataOutput
    {
        public void Output(IBuildContainer buildContainer, IBuildArgs buildArgs, Action onFinished)
        {
            BehaviorTreeBuildContainer container = buildContainer as BehaviorTreeBuildContainer;
            BehaviorTreeBuildArgs behaviorTreeBuildArgs = buildArgs as BehaviorTreeBuildArgs;
            
            behaviorTreeBuildArgs.behaviorTreeAsset.cache = container.asset;
            behaviorTreeBuildArgs.behaviorTreeAsset.cacheBindMap = container.bindMap;

            behaviorTreeBuildArgs.behaviorTreeAsset.SaveAll();

            onFinished.Invoke();
        }
    }
}