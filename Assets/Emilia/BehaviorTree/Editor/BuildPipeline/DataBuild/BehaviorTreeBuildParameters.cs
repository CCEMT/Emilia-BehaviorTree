using System;
using Emilia.DataBuildPipeline.Editor;
using Emilia.Variables;

namespace Emilia.BehaviorTree.Editor
{
    [BuildPipeline(typeof(BehaviorTreeBuildArgs)), BuildSequence(1000)]
    public class BehaviorTreeBuildParameters : IDataBuild
    {
        public void Build(IBuildContainer buildContainer, IBuildArgs buildArgs, Action onFinished)
        {
            BehaviorTreeBuildContainer container = buildContainer as BehaviorTreeBuildContainer;
            BehaviorTreeBuildArgs behaviorTreeBuildArgs = buildArgs as BehaviorTreeBuildArgs;

            if (behaviorTreeBuildArgs.behaviorTreeAsset.editorParametersManage == null) container.variablesManage = new VariablesManager();
            else
            {
                VariablesManager rootVariablesManage = behaviorTreeBuildArgs.behaviorTreeAsset.editorParametersManage.ToParametersManage();
                container.variablesManage = rootVariablesManage;
            }

            onFinished.Invoke();
        }
    }
}