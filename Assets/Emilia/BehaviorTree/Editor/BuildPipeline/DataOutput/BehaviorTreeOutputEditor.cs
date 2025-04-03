using System;
using Emilia.DataBuildPipeline.Editor;
using Emilia.Node.Editor;

namespace Emilia.BehaviorTree.Editor
{
    [BuildPipeline(typeof(BehaviorTreeBuildArgs)), BuildSequence(1000)]
    public class BehaviorTreeOutputEditor : IDataOutput
    {
        public void Output(IBuildContainer buildContainer, IBuildArgs buildArgs, Action onFinished)
        {
            BehaviorTreeBuildContainer container = buildContainer as BehaviorTreeBuildContainer;
            container.editorAsset.cache = container.asset;
            container.editorAsset.cacheBindMap = container.bindMap;

            container.editorAsset.Save();

            onFinished.Invoke();
        }
    }
}