using System;
using Emilia.DataBuildPipeline.Editor;
using Emilia.Variables;

namespace Emilia.BehaviorTree.Editor
{
    [BuildPipeline(BehaviorTreeBuildPipeline.PipelineName), BuildSequence(1000)]
    public class BehaviorTreeBuildParameters : IDataBuild
    {
        public void Build(IBuildContainer buildContainer, Action onFinished)
        {
            BehaviorTreeBuildContainer container = buildContainer as BehaviorTreeBuildContainer;
            VariablesManage variablesManage = container.editorAsset.editorParametersManage.ToParametersManage();
            container.variablesManage = variablesManage;
            
            onFinished.Invoke();
        }
    }
}