using System.Collections.Generic;
using System.Linq;
using Emilia.BehaviorTree.Attributes;
using Emilia.Kit;
using Emilia.Node.Editor;
using Emilia.Node.Universal.Editor;
using Sirenix.Utilities;

namespace Emilia.BehaviorTree.Editor
{
    [EditorHandle(typeof(EditorBehaviorTreeAsset))]
    public class BehaviorTreeCreateNodeMenuHandle : UniversalCreateNodeMenuHandle
    {
        private EditorBehaviorTreeAsset behaviorTreeAsset;

        public override void InitializeCache(EditorGraphView graphView, List<ICreateNodeHandle> createNodeHandles)
        {
            base.InitializeCache(graphView, createNodeHandles);
            behaviorTreeAsset = graphView.graphAsset as EditorBehaviorTreeAsset;

            FilterCreateNodeHandles(graphView);
            AddRoot(graphView);
        }

        private void AddRoot(EditorGraphView graphView)
        {
            CreateNodeHandle rootNodeHandle = new CreateNodeHandle();
            rootNodeHandle.editorNodeType = typeof(EditorRootNodeAsset);
            rootNodeHandle.path = "入口";

            graphView.createNodeMenu.createNodeHandleCacheList.Add(rootNodeHandle);
        }

        private void FilterCreateNodeHandles(EditorGraphView graphView)
        {
            int cacheAmount = graphView.createNodeMenu.createNodeHandleCacheList.Count;
            for (int i = cacheAmount - 1; i >= 0; i--)
            {
                ICreateNodeHandle createNodeHandle = graphView.createNodeMenu.createNodeHandleCacheList[i];

                object nodeData = createNodeHandle.nodeData;
                if (nodeData == null) continue;

                NodeTagAttribute[] nodeTagAttributes = nodeData.GetType().GetCustomAttributes<NodeTagAttribute>(true).ToArray();
                if (nodeTagAttributes.Any() == false)
                {
                    graphView.createNodeMenu.createNodeHandleCacheList.RemoveAt(i);
                    continue;
                }

                if (IsContain(nodeTagAttributes) == false) graphView.createNodeMenu.createNodeHandleCacheList.RemoveAt(i);
            }
        }

        private bool IsContain(NodeTagAttribute[] nodeTagAttributes)
        {
            foreach (NodeTagAttribute nodeTagAttribute in nodeTagAttributes)
            {
                foreach (string[] behaviorTreeTags in this.behaviorTreeAsset.tags)
                {
                    if (IsContainAllTags(nodeTagAttribute.tags, behaviorTreeTags) == false) continue;
                    return true;
                }
            }

            return false;
        }

        private bool IsContainAllTags(string[] tags, string[] targetTags)
        {
            for (var i = 0; i < targetTags.Length; i++)
            {
                var tag = targetTags[i];
                if (tags.Contains(tag) == false) return false;
            }

            return true;
        }
    }
}