using System;
using Emilia.DataBuildPipeline.Editor;

namespace Emilia.BehaviorTree.Editor
{
    [BuildPipeline(BehaviorTreeBuildPipeline.PipelineName), BuildSequence(1000)]
    public class BehaviorTreeOutputEditor : IDataOutput
    {
        public void Output(IBuildContainer buildContainer, IBuildArgs buildArgs, Action onFinished)
        {
            BehaviorTreeBuildContainer container = buildContainer as BehaviorTreeBuildContainer;
            container.editorAsset.cache = container.asset;
            container.editorAsset.cacheBindMap = container.bindMap;

            onFinished.Invoke();
        }
    }
}