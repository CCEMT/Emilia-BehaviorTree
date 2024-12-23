using System;
using System.Collections.Generic;
using Emilia.DataBuildPipeline.Editor;
using UnityEditor;

namespace Emilia.BehaviorTree.Editor
{
    [BuildPipeline(BehaviorTreeBuildPipeline.PipelineName), BuildSequence(3000)]
    public class BehaviorTreeOutputRunner : IDataOutput
    {
        public void Output(IBuildContainer buildContainer, IBuildArgs buildArgs, Action onFinished)
        {
            BehaviorTreeBuildContainer container = buildContainer as BehaviorTreeBuildContainer;
            BehaviorTreeBuildArgs args = buildArgs as BehaviorTreeBuildArgs;

            if (args.updateRunner == false || EditorApplication.isPlaying == false)
            {
                onFinished.Invoke();
                return;
            }

            List<EditorBehaviorTreeRunner> runners = EditorBehaviorTreeRunner.runnerByAssetId.GetValueOrDefault(container.editorAsset.id);
            if (runners == null)
            {
                onFinished.Invoke();
                return;
            }

            int runnerCount = runners.Count;
            for (int i = 0; i < runnerCount; i++)
            {
                EditorBehaviorTreeRunner runner = runners[i];
                if (runner == null) continue;
                if (runner.isActive == false) continue;
                runner.Reload(container.asset);
            }

            onFinished.Invoke();
        }
    }
}