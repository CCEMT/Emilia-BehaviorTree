using System;
using System.Collections.Generic;
using System.Linq;
using Emilia.DataBuildPipeline.Editor;
using Emilia.Kit.Editor;
using Emilia.Node.Editor;
using Sirenix.Serialization;

namespace Emilia.BehaviorTree.Editor
{
    [BuildPipeline(BehaviorTreeBuildPipeline.PipelineName), BuildSequence(2000)]
    public class BehaviorTreeBuildNode : IDataBuild
    {
        public void Build(IBuildContainer buildContainer, Action onFinished)
        {
            BehaviorTreeBuildContainer container = buildContainer as BehaviorTreeBuildContainer;

            List<EditorBehaviorTreeNodeAsset> allNodes = container.editorAsset.nodes.OfType<EditorBehaviorTreeNodeAsset>().ToList();

            Dictionary<int, EditorBehaviorTreeNodeAsset> nodeByRuntimeIdMap = new Dictionary<int, EditorBehaviorTreeNodeAsset>();
            Dictionary<string, NodeAsset> nodeByEditorIdMap = new Dictionary<string, NodeAsset>();

            List<NodeAsset> nodeAssets = new List<NodeAsset>();

            Queue<EditorBehaviorTreeNodeAsset> nodeQueue = new Queue<EditorBehaviorTreeNodeAsset>();
            EditorRootNodeAsset rootNode = allNodes.OfType<EditorRootNodeAsset>().FirstOrDefault();
            EditorBehaviorTreeNodeAsset rootOutputNode = container.editorAsset.GetOutputNodes(rootNode).FirstOrDefault() as EditorBehaviorTreeNodeAsset;
            nodeQueue.Enqueue(rootOutputNode);

            int id = 0;
            while (nodeQueue.Count > 0)
            {
                EditorBehaviorTreeNodeAsset editorNode = nodeQueue.Dequeue();
                if (editorNode.userData == null) continue;

                NodeAsset node = SerializationUtility.CreateCopy(editorNode.userData) as NodeAsset;

                id++;
                int timeId = int.Parse(DateTime.Now.ToString("yyMMdd") + id);
                ReflectUtility.SetValue(typeof(NodeAsset), node, nameof(NodeAsset.id), timeId);

                nodeByRuntimeIdMap[node.id] = editorNode;
                nodeByEditorIdMap[editorNode.id] = node;
                nodeAssets.Add(node);

                List<EditorNodeAsset> outputNodes = container.editorAsset.GetOutputNodes(editorNode);
                outputNodes.Sort((a, b) => a.position.x.CompareTo(b.position.x));

                int outputNodeCount = outputNodes.Count;
                for (int i = 0; i < outputNodeCount; i++)
                {
                    EditorNodeAsset outputNode = outputNodes[i];
                    EditorBehaviorTreeNodeAsset outputNodeAsset = outputNode as EditorBehaviorTreeNodeAsset;
                    nodeQueue.Enqueue(outputNodeAsset);
                }

                container.bindMap[node.id] = editorNode.id;
            }

            List<NodeAsset> behaviorTreeNodeAssets = new List<NodeAsset>();

            int nodeAssetsCount = nodeAssets.Count;
            for (int i = nodeAssetsCount - 1; i >= 0; i--)
            {
                NodeAsset node = nodeAssets[i];
                EditorBehaviorTreeNodeAsset editorNode = nodeByRuntimeIdMap.GetValueOrDefault(node.id);

                List<int> children = node.childrenIds as List<int>;
                children.Clear();

                List<EditorNodeAsset> outputNodes = container.editorAsset.GetOutputNodes(editorNode);
                outputNodes.Sort((a, b) => a.position.x.CompareTo(b.position.x));

                int outputNodeCount = outputNodes.Count;
                for (int j = 0; j < outputNodeCount; j++)
                {
                    EditorNodeAsset outputNode = outputNodes[j];
                    NodeAsset outputNodeAsset = nodeByEditorIdMap.GetValueOrDefault(outputNode.id);
                    children.Add(outputNodeAsset.id);
                }

                behaviorTreeNodeAssets.Add(node);
            }

            container.nodeAssets = behaviorTreeNodeAssets;

            NodeAsset rootOutputNodeAsset = nodeByEditorIdMap.GetValueOrDefault(rootOutputNode.id);
            container.entryNodeId = rootOutputNodeAsset.id;

            onFinished.Invoke();
        }
    }
}