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
        private const int OutputVisibilitySyncFrameCount = 3;

        protected EditorBehaviorTreeNodeAsset behaviorTreeNodeAsset;
        private bool isOutputVisibilitySyncRegistered;
        private int remainingOutputVisibilitySyncFrames;
        private bool visibleByParent = true;

        public object openScriptObject => behaviorTreeNodeAsset.userData;

        public override bool expanded
        {
            get => base.expanded;
            set
            {
                base.expanded = value;
                RegisterOutputVisibilitySync();
            }
        }

        public override void Initialize(EditorGraphView graphView, EditorNodeAsset asset)
        {
            behaviorTreeNodeAsset = asset as EditorBehaviorTreeNodeAsset;
            base.Initialize(graphView, asset);
            RegisterOutputVisibilitySync();
        }

        protected override void InitializeNodeView()
        {
            base.InitializeNodeView();
            SetNodeColor(behaviorTreeNodeAsset.userData);
        }

        protected override void RebuildPortView()
        {
            base.RebuildPortView();
            EnsureBehaviorTreePortViewsCreated();
            ShowInputPorts();
            SetOutputPortsVisible(visibleByParent && expanded);
        }

        public override void CollectElements(HashSet<GraphElement> collectedElementSet, Func<GraphElement, bool> conditionFunc)
        {
            base.CollectElements(collectedElementSet, conditionFunc);
            if (this.behaviorTreeNodeAsset.isExpanded) return;

            HashSet<string> visitedNodeIds = new() {asset.id};
            CollectOutputNodeElements(asset.id, collectedElementSet, visitedNodeIds);
        }

        private void CollectOutputNodeElements(string nodeId, HashSet<GraphElement> collectedElementSet, HashSet<string> visitedNodeIds)
        {
            if (graphView?.graphAsset == null) return;

            IReadOnlyList<EditorEdgeAsset> edges = graphView.graphAsset.edges;
            int edgeCount = edges.Count;
            for (int i = 0; i < edgeCount; i++)
            {
                EditorEdgeAsset edge = edges[i];
                if (edge.outputNodeId != nodeId) continue;
                if (visitedNodeIds.Add(edge.inputNodeId) == false) continue;

                EditorNodeAsset outputNodeAsset = graphView.graphAsset.nodeMap.GetValueOrDefault(edge.inputNodeId);
                if (outputNodeAsset == null) continue;

                IEditorNodeView outputNodeView = graphView.graphElementCache.nodeViewById.GetValueOrDefault(outputNodeAsset.id);
                if (outputNodeView != null) collectedElementSet.Add(outputNodeView.element);

                CollectOutputNodeElements(outputNodeAsset.id, collectedElementSet, visitedNodeIds);
            }
        }

        public override void OnValueChanged(bool isSilent = false)
        {
            base.OnValueChanged(isSilent);
            RegisterOutputVisibilitySync();
        }

        public override void Dispose()
        {
            UnregisterOutputVisibilitySync();
            base.Dispose();
        }

        private void SyncOutputVisibility(bool visible, HashSet<string> visitedNodeIds = null)
        {
            ShowInputPorts();
            SetOutputPortsVisible(visible);

            if (IsGraphAssetNode() == false) return;

            if (visitedNodeIds == null) visitedNodeIds = new HashSet<string> {asset.id};
            SetOutputSubtreeVisible(asset.id, visible, visitedNodeIds);
        }

        private void ShowInputPorts()
        {
            ForceVisible(nodeTopContainer);
            ForceVisible(portNodeTopContainer);
            ForceVisible(verticalInputContainer);

            int portCount = portViews.Count;
            for (int i = 0; i < portCount; i++)
            {
                IEditorPortView port = portViews[i];
                if (port.portDirection != EditorPortDirection.Input) continue;

                ForceVisible(port.portElement);
            }
        }

        private void EnsureBehaviorTreePortViewsCreated()
        {
            List<EditorPortInfo> portInfos = CollectStaticPortAssets();
            portInfos.Sort((a, b) => a.order.CompareTo(b.order));

            int inputIndex = 0;
            int outputIndex = 0;
            int portInfoCount = portInfos.Count;
            for (int i = 0; i < portInfoCount; i++)
            {
                EditorPortInfo portInfo = portInfos[i];
                if (portInfo.direction == EditorPortDirection.Input)
                {
                    if (GetPortView(portInfo.id) == null) AddPortView(inputIndex, portInfo);
                    inputIndex++;
                    continue;
                }

                if (portInfo.direction == EditorPortDirection.Output)
                {
                    if (GetPortView(portInfo.id) == null) AddPortView(outputIndex, portInfo);
                    outputIndex++;
                }
            }
        }

        private static void ForceVisible(VisualElement element)
        {
            if (element == null) return;

            element.visible = true;
            element.style.display = DisplayStyle.Flex;
            element.style.visibility = Visibility.Visible;
            element.RemoveFromClassList("hidden");
        }

        private void SetOutputPortsVisible(bool visible)
        {
            if (visible)
            {
                ForceVisible(nodeBottomContainer);
                ForceVisible(portNodeBottomContainer);
                ForceVisible(verticalOutputContainer);
            }

            int portCount = portViews.Count;
            for (int i = 0; i < portCount; i++)
            {
                IEditorPortView port = portViews[i];
                if (port.portDirection == EditorPortDirection.Output)
                {
                    if (visible) ForceVisible(port.portElement);
                    else port.portElement.style.display = DisplayStyle.None;
                }
            }
        }

        private void SetOutputSubtreeVisible(string nodeId, bool visible, HashSet<string> visitedNodeIds)
        {
            IReadOnlyList<EditorEdgeAsset> edges = graphView.graphAsset.edges;
            int edgeCount = edges.Count;
            for (int i = 0; i < edgeCount; i++)
            {
                EditorEdgeAsset outputEdge = edges[i];
                if (outputEdge.outputNodeId != nodeId) continue;

                SetOutputEdgeVisible(outputEdge, visible);

                if (visitedNodeIds.Add(outputEdge.inputNodeId) == false) continue;

                EditorNodeAsset outputNode = graphView.graphAsset.nodeMap.GetValueOrDefault(outputEdge.inputNodeId);
                if (outputNode == null) continue;
                IEditorNodeView outputNodeView = graphView.graphElementCache.nodeViewById.GetValueOrDefault(outputNode.id);

                if (outputNodeView != null)
                {
                    outputNodeView.element.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;

                    if (outputNodeView is EditorBehaviorTreeNodeView behaviorTreeOutputNodeView)
                    {
                        behaviorTreeOutputNodeView.SetVisibleByParent(visible, visitedNodeIds);
                        continue;
                    }
                }

                SetOutputSubtreeVisible(outputNode.id, visible, visitedNodeIds);
            }
        }

        private void SetVisibleByParent(bool visible, HashSet<string> visitedNodeIds)
        {
            visibleByParent = visible;
            element.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
            ShowInputPorts();
            SyncOutputVisibility(visible && expanded, visitedNodeIds);
        }

        private void SetOutputEdgeVisible(EditorEdgeAsset edgeAsset, bool visible)
        {
            IEditorEdgeView outputEdgeView = graphView.graphElementCache.edgeViewById.GetValueOrDefault(edgeAsset.id);
            if (outputEdgeView == null) return;

            outputEdgeView.edgeElement.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void RegisterOutputVisibilitySync()
        {
            if (IsGraphAssetNode() == false) return;

            SyncOutputVisibility(visibleByParent && expanded);

            remainingOutputVisibilitySyncFrames = graphView.isInitialized ? OutputVisibilitySyncFrameCount : 0;
            if (isOutputVisibilitySyncRegistered) return;

            isOutputVisibilitySyncRegistered = true;
            graphView.onUpdate += SyncOutputVisibilityOnUpdate;
        }

        private void SyncOutputVisibilityOnUpdate()
        {
            if (IsGraphAssetNode() == false)
            {
                UnregisterOutputVisibilitySync();
                return;
            }

            SyncOutputVisibility(visibleByParent && expanded);

            if (graphView.isInitialized == false) return;
            if (visibleByParent && expanded == false) return;

            remainingOutputVisibilitySyncFrames--;
            if (remainingOutputVisibilitySyncFrames > 0) return;

            UnregisterOutputVisibilitySync();
        }

        private void UnregisterOutputVisibilitySync()
        {
            if (isOutputVisibilitySyncRegistered == false) return;

            graphView.onUpdate -= SyncOutputVisibilityOnUpdate;
            isOutputVisibilitySyncRegistered = false;
        }

        private bool IsGraphAssetNode()
        {
            if (graphView?.graphAsset == null) return false;
            if (asset == null) return false;

            return graphView.graphAsset.nodeMap.ContainsKey(asset.id);
        }
    }
}
