using System.Linq;
using Emilia.BehaviorTree.Attributes;
using Emilia.Node.Editor;
using Sirenix.Utilities;

namespace Emilia.BehaviorTree.Editor
{
    public class BehaviorTreeCreateNodeMenuHandle : CreateNodeMenuHandle<EditorBehaviorTreeAsset>
    {
        private EditorBehaviorTreeAsset behaviorTreeAsset;

        public override void InitializeCache()
        {
            base.InitializeCache();

            behaviorTreeAsset = smartValue.graphAsset as EditorBehaviorTreeAsset;

            FilterCreateNodeHandles();

            AddRoot();
        }

        private void AddRoot()
        {
            CreateNodeHandle rootNodeHandle = new CreateNodeHandle();
            rootNodeHandle.editorNodeType = typeof(EditorRootNodeAsset);
            rootNodeHandle.path = "入口";

            smartValue.createNodeMenu.createNodeHandleCacheList.Add(rootNodeHandle);
        }

        private void FilterCreateNodeHandles()
        {
            int cacheAmount = smartValue.createNodeMenu.createNodeHandleCacheList.Count;
            for (int i = cacheAmount - 1; i >= 0; i--)
            {
                ICreateNodeHandle createNodeHandle = smartValue.createNodeMenu.createNodeHandleCacheList[i];

                object userData = createNodeHandle.userData;
                if (userData == null) continue;

                NodeTagAttribute[] nodeTagAttributes = userData.GetType().GetCustomAttributes<NodeTagAttribute>(true).ToArray();
                if (nodeTagAttributes.Any() == false)
                {
                    smartValue.createNodeMenu.createNodeHandleCacheList.RemoveAt(i);
                    continue;
                }

                if (IsContain(nodeTagAttributes) == false) smartValue.createNodeMenu.createNodeHandleCacheList.RemoveAt(i);
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