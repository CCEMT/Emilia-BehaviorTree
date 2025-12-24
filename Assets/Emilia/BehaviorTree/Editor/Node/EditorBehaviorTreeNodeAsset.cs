using System;
using System.Collections.Generic;
using Emilia.BehaviorTree.Attributes;
using Emilia.Node.Attributes;
using Emilia.Node.Editor;
using Emilia.Node.Universal.Editor;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Emilia.BehaviorTree.Editor
{
    [HideMonoScript]
    public class EditorBehaviorTreeNodeAsset : UniversalNodeAsset
    {
        [ShowInInspector, HideLabel, HideReferenceObjectPicker, ShowIf(nameof(userData))]
        public object displayData
        {
            get => userData;
            set => userData = value;
        }

        protected override string defaultDisplayName
        {
            get
            {
                if (userData == null) return base.defaultDisplayName;
                Type nodeAssetType = userData.GetType();
                BehaviorTreeMenuAttribute createNodeMenu = nodeAssetType.GetCustomAttribute<BehaviorTreeMenuAttribute>();
                if (createNodeMenu == null) return base.defaultDisplayName;
                OperateMenuUtility.PathToNameAndCategory(createNodeMenu.path, out string titleText, out string _);
                return titleText;
            }
        }
    }

    [EditorNode(typeof(EditorBehaviorTreeNodeAsset))]
    public class EditorBehaviorTreeNodeView : UniversalEditorNodeView, ISpecifyOpenScriptObject
    {
        public const string InputPortName = "Input";
        public const string OutputPortName = "Output";

        protected EditorBehaviorTreeNodeAsset behaviorTreeNodeAsset;

        public object openScriptObject => behaviorTreeNodeAsset.userData;
        
        public override bool expanded
        {
            get=>base.expanded;
            set
            {
                base.expanded = value;
                SetOutputNodeVisible(value);
            }
        }

        public override void Initialize(EditorGraphView graphView, EditorNodeAsset asset)
        {
            behaviorTreeNodeAsset = asset as EditorBehaviorTreeNodeAsset;
            base.Initialize(graphView, asset);
        }

        protected override void InitializeNodeView()
        {
            base.InitializeNodeView();
            SetNodeColor(behaviorTreeNodeAsset.userData);
        }

        public override void CollectElements(HashSet<GraphElement> collectedElementSet, Func<GraphElement, bool> conditionFunc)
        {
            base.CollectElements(collectedElementSet, conditionFunc);
            if (this.behaviorTreeNodeAsset.isExpanded) return;
            List<EditorNodeAsset> allOutputNodeAsset = asset.GetLogicalOutputNodes();
            int outputNodeCount = allOutputNodeAsset.Count;
            for (int i = 0; i < outputNodeCount; i++)
            {
                EditorNodeAsset outputNodeAsset = allOutputNodeAsset[i];
                IEditorNodeView outputNodeView = graphView.graphElementCache.nodeViewById.GetValueOrDefault(outputNodeAsset.id);
                if (outputNodeView == null) continue;
                collectedElementSet.Add(outputNodeView.element);
            }
        }



        private void SetOutputNodeVisible(bool visible)
        {
            int portCount = portViews.Count;
            for (int i = 0; i < portCount; i++)
            {
                IEditorPortView port = portViews[i];
                if (port.portDirection == EditorPortDirection.Output) port.portElement.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
            }

            List<EditorEdgeAsset> outputEdges = graphView.graphAsset.GetOutputEdges(asset);
            int outputEdgeCount = outputEdges.Count;
            for (int i = 0; i < outputEdgeCount; i++)
            {
                EditorEdgeAsset outputEdge = outputEdges[i];
                IEditorEdgeView outputEdgeView = graphView.graphElementCache.edgeViewById.GetValueOrDefault(outputEdge.id);
                if (outputEdgeView == null) continue;

                outputEdgeView.edgeElement.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
            }

            List<EditorNodeAsset> outputNodes = asset.GetLogicalOutputNodes();
            int outputNodeCount = outputNodes.Count;
            for (int i = 0; i < outputNodeCount; i++)
            {
                EditorNodeAsset outputNode = outputNodes[i];

                IEditorNodeView outputNodeView = graphView.graphElementCache.nodeViewById.GetValueOrDefault(outputNode.id);
                if (outputNodeView == null) continue;

                outputNodeView.element.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;

                UniversalEditorNodeView universalOutputNodeView = outputNodeView as UniversalEditorNodeView;
                if (universalOutputNodeView != null) universalOutputNodeView.expanded = visible;
            }
        }
    }
}