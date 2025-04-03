using Emilia.DataBuildPipeline.Editor;

namespace Emilia.BehaviorTree.Editor
{
    [BuildPipeline(typeof(BehaviorTreeBuildArgs))]
    public class BehaviorTreeBuildPipeline : UniversalBuildPipeline
    {
        private BehaviorTreeBuildArgs behaviorTreeBuildArgs;

        protected override void RunInitialize()
        {
            base.RunInitialize();
            this.behaviorTreeBuildArgs = this.buildArgs as BehaviorTreeBuildArgs;
        }

        protected override IBuildContainer CreateContainer()
        {
            BehaviorTreeBuildContainer buildContainer = new BehaviorTreeBuildContainer();
            buildContainer.editorAsset = this.behaviorTreeBuildArgs.behaviorTreeAsset;
            return buildContainer;
        }
    }
}