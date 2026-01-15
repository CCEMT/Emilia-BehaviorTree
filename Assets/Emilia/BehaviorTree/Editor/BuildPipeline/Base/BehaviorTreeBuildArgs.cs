using System;
using Emilia.DataBuildPipeline.Editor;

namespace Emilia.BehaviorTree.Editor
{
    public class BehaviorTreeBuildArgs : BuildArgs
    {
        public EditorBehaviorTreeAsset behaviorTreeAsset;
        public string outputPath;

        public bool isGenerateFile;
        public bool isSaveAsset = true;
        public bool isRefresh = true;
        public bool updateRunner = true;
        public Action generateFileCallback;

        public BehaviorTreeBuildArgs(EditorBehaviorTreeAsset behaviorTreeAsset, string outputPath, Action<BuildReport> onBuildComplete = null)
        {
            this.behaviorTreeAsset = behaviorTreeAsset;
            this.outputPath = outputPath;
            this.onBuildComplete = onBuildComplete;
            this.isGenerateFile = true;
            this.isSaveAsset = true;
            this.isRefresh = true;
        }
    }
}