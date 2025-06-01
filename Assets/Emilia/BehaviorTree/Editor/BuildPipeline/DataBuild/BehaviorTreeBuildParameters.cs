using System;
using Emilia.DataBuildPipeline.Editor;
using Emilia.Variables;

namespace Emilia.BehaviorTree.Editor
{
    [BuildPipeline(typeof(BehaviorTreeBuildArgs)), BuildSequence(1000)]
    public class BehaviorTreeBuildParameters : IDataBuild
    {
        public void Build(IBuildContainer buildContainer, Action onFinished)
        {
            BehaviorTreeBuildContainer container = buildContainer as BehaviorTreeBuildContainer;

            if (container.editorAsset.editorParametersManage == null) container.variablesManage = new VariablesManage();
            else
            {
                VariablesManage rootVariablesManage = container.editorAsset.editorParametersManage.ToParametersManage();
                container.variablesManage = rootVariablesManage;
            }

            onFinished.Invoke();
        }
    }
}