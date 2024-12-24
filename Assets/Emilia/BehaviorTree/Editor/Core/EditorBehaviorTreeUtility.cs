using System;
using Emilia.DataBuildPipeline.Editor;

namespace Emilia.BehaviorTree.Editor
{
    public static class EditorBehaviorTreeUtility
    {
        public static void DataBuild(EditorBehaviorTreeAsset asset, Action<BuildReport> onBuildComplete = null)
        {
            if (asset == null) return;

            string path = asset.outputPath;

            BehaviorTreeBuildArgs behaviorTreeBuildArgs = new BehaviorTreeBuildArgs(asset, path);
            behaviorTreeBuildArgs.onBuildComplete = onBuildComplete;

            DataBuildUtility.Build(behaviorTreeBuildArgs);
        }
    }
}