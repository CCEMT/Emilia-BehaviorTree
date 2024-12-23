﻿using System.Collections.Generic;
using Emilia.DataBuildPipeline.Editor;
using Emilia.Kit;
using Emilia.Node.Attributes;
using Emilia.Node.Editor;
using Emilia.Node.Universal.Editor;
using Emilia.Reflection.Editor;
using UnityEditor;
using UnityEngine;

namespace Emilia.BehaviorTree.Editor
{
    public class BehaviorTreeToolbarView : ToolbarView
    {
        private EditorBehaviorTreeAsset behaviorTreeAsset;

        public override void Initialize(EditorGraphView graphView)
        {
            this.behaviorTreeAsset = graphView.graphAsset as EditorBehaviorTreeAsset;
            base.Initialize(graphView);
        }

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

        private void OnEditorParameter()
        {
            EditorParametersManage editorParametersManage = behaviorTreeAsset.editorParametersManage;
            if (editorParametersManage == null)
            {
                editorParametersManage = behaviorTreeAsset.editorParametersManage = ScriptableObject.CreateInstance<EditorParametersManage>();
                EditorAssetKit.SaveAssetIntoObject(editorParametersManage, this.behaviorTreeAsset);
            }

            Selection.activeObject = editorParametersManage;
        }

        private void OnEditorRuntimeParameter()
        {
            GetBehaviorTreeRunnerEvent getBehaviorTreeRunnerEvent = GetBehaviorTreeRunnerEvent.GetPooled();
            getBehaviorTreeRunnerEvent.target = graphView;

            graphView.SendEvent_Internal(getBehaviorTreeRunnerEvent, DispatchMode_Internals.Immediate);
            BehaviorTreeRuntimeParameter behaviorTreeRuntimeParameter = new BehaviorTreeRuntimeParameter(getBehaviorTreeRunnerEvent.runner);
            EditorKit.SetSelection(behaviorTreeRuntimeParameter, "运行参数");
        }

        private OdinMenu BuildRunnerMenu()
        {
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

        private void OnSave()
        {
            EditorBehaviorTreeUtility.DataBuild(this.behaviorTreeAsset, (report) => {
                if (report.result == BuildResult.Succeeded) graphView.window.ShowNotification(new GUIContent("保存成功"), 1.5f);
            });
        }
    }
}