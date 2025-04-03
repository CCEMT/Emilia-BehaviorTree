using System.Collections.Generic;
using Emilia.DataBuildPipeline.Editor;
using Emilia.Kit;
using Emilia.Node.Attributes;
using Emilia.Node.Universal.Editor;
using Emilia.Reflection.Editor;
using Emilia.Variables.Editor;
using UnityEditor;
using UnityEngine;

namespace Emilia.BehaviorTree.Editor
{
    public class BehaviorTreeToolbarView : ToolbarView
    {
        protected override void InitControls()
        {
            AddControl(new ButtonToolbarViewControl("参数", OnEditorParameter));

            if (EditorApplication.isPlaying)
            {
                AddControl(new ButtonToolbarViewControl("运行参数", OnEditorRuntimeParameter));

                AddControl(new DropdownButtonToolbarViewControl("运行实例", BuildRunnerMenu));
            }

            AddControl(new ButtonToolbarViewControl("保存", OnSave), ToolbarViewControlPosition.RightOrBottom);
        }

        protected virtual void OnEditorParameter()
        {
            EditorBehaviorTreeAsset behaviorTreeAsset = graphView.graphAsset as EditorBehaviorTreeAsset;

            EditorParametersManage editorParametersManage = behaviorTreeAsset.editorParametersManage;
            if (editorParametersManage == null)
            {
                editorParametersManage = behaviorTreeAsset.editorParametersManage = ScriptableObject.CreateInstance<EditorParametersManage>();
                EditorAssetKit.SaveAssetIntoObject(editorParametersManage, behaviorTreeAsset);
            }

            Selection.activeObject = editorParametersManage;
        }

        protected virtual void OnEditorRuntimeParameter()
        {
            GetBehaviorTreeRunnerEvent getBehaviorTreeRunnerEvent = GetBehaviorTreeRunnerEvent.GetPooled();
            getBehaviorTreeRunnerEvent.target = graphView;

            graphView.SendEvent_Internal(getBehaviorTreeRunnerEvent, DispatchMode_Internals.Immediate);
            BehaviorTreeRuntimeParameter behaviorTreeRuntimeParameter = new BehaviorTreeRuntimeParameter(getBehaviorTreeRunnerEvent.runner);
            EditorKit.SetSelection(behaviorTreeRuntimeParameter, "运行参数");
        }

        protected virtual OdinMenu BuildRunnerMenu()
        {
            EditorBehaviorTreeAsset behaviorTreeAsset = graphView.graphAsset as EditorBehaviorTreeAsset;

            OdinMenu odinMenu = new OdinMenu();
            odinMenu.defaultWidth = 300;

            if (EditorBehaviorTreeRunner.runnerByAssetId == null) return odinMenu;
            List<EditorBehaviorTreeRunner> runners = EditorBehaviorTreeRunner.runnerByAssetId.GetValueOrDefault(behaviorTreeAsset.id);
            if (runners == null) return odinMenu;
            int count = runners.Count;
            for (var i = 0; i < count; i++)
            {
                EditorBehaviorTreeRunner runner = runners[i];
                string itemName = runner.behaviorTree.owner.ToString();
                if (string.IsNullOrEmpty(runner.editorBehaviorTreeAsset.description) == false) itemName = $"{runner.editorBehaviorTreeAsset.description}({runner.editorBehaviorTreeAsset.name})";
                odinMenu.AddItem(itemName, () => {
                    SetBehaviorTreeRunnerEvent e = SetBehaviorTreeRunnerEvent.Create(runner);
                    e.target = graphView;

                    graphView.SendEvent(e);
                });
            }

            return odinMenu;
        }

        protected virtual void OnSave()
        {
            EditorBehaviorTreeAsset behaviorTreeAsset = graphView.graphAsset as EditorBehaviorTreeAsset;
            EditorBehaviorTreeAsset rootBehaviorTreeAsset = behaviorTreeAsset.GetRootAsset() as EditorBehaviorTreeAsset;

            EditorBehaviorTreeUtility.DataBuild(rootBehaviorTreeAsset, (report) => {
                if (report.result == BuildResult.Succeeded) graphView.window.ShowNotification(new GUIContent("保存成功"), 1.5f);
            });
        }
    }
}