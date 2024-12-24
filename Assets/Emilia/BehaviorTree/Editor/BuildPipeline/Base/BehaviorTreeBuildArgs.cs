using System;
using Emilia.DataBuildPipeline.Editor;

namespace Emilia.BehaviorTree.Editor
{
    public class BehaviorTreeBuildArgs : BuildArgs
    {
        public EditorBehaviorTreeAsset behaviorTreeAsset;
        public string outputPath;
        public bool updateRunner = true;

        public BehaviorTreeBuildArgs(EditorBehaviorTreeAsset behaviorTreeAsset, string outputPath, Action<BuildReport> onBuildComplete = null)
        {
            pipelineName = BehaviorTreeBuildPipeline.PipelineName;

            this.behaviorTreeAsset = behaviorTreeAsset;
            this.outputPath = outputPath;
            this.onBuildComplete = onBuildComplete;
        }
    }
}