using System;
using Emilia.DataBuildPipeline.Editor;
using Emilia.Kit.Editor;
using UnityEditor;

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

            behaviorTreeBuildArgs.behaviorTreeAsset.SetDirtyAll();
            if (behaviorTreeBuildArgs.isSaveAsset) AssetDatabase.SaveAssets();

            onFinished.Invoke();
        }
    }
}