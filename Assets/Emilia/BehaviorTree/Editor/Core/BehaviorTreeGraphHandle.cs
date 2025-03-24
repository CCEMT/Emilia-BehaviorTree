using System.Collections.Generic;
using System.Linq;
using Emilia.Node.Editor;
using UnityEditor;
using UnityEngine;

namespace Emilia.BehaviorTree.Editor
{
    public class BehaviorTreeGraphHandle : GraphHandle<EditorBehaviorTreeAsset>
    {
        private EditorBehaviorTreeAsset editorBehaviorTreeAsset;
        private EditorBehaviorTreeRunner debugRunner;
        private List<int> runningNodes = new List<int>();

        public override void Initialize(object weakSmartValue)
        {
            base.Initialize(weakSmartValue);
            editorBehaviorTreeAsset = this.smartValue.graphAsset as EditorBehaviorTreeAsset;
            this.smartValue.RegisterCallback<GetBehaviorTreeRunnerEvent>(OnGetBehaviorTreeRunner);
            this.smartValue.RegisterCallback<SetBehaviorTreeRunnerEvent>(OnSetBehaviorTreeRunner);
        }

        public override void OnUpdate()
        {
            if (EditorApplication.isPlaying == false)
            {
                ClearRunner();
                return;
            }

            if (this.debugRunner == null)
            {
                List<EditorBehaviorTreeRunner> runners = EditorBehaviorTreeRunner.runnerByAssetId.GetValueOrDefault(smartValue.graphAsset.id);
                if (runners != null && runners.Count == 1)
                {
                    this.debugRunner = runners.FirstOrDefault();
                    if (EditorBehaviorTreeRunner.nodeMessage.TryGetValue(this.debugRunner.uid, out var queue)) queue.Clear();
                }
            }
            else
            {
                if (EditorBehaviorTreeRunner.runnerByAssetId.ContainsKey(smartValue.graphAsset.id) == false) ClearRunner();
                else if (EditorBehaviorTreeRunner.runnerByAssetId[smartValue.graphAsset.id].Contains(this.debugRunner) == false) ClearRunner();
            }

            DrawDebug();
        }

        void OnGetBehaviorTreeRunner(GetBehaviorTreeRunnerEvent getBehaviorTreeRunnerEvent)
        {
            getBehaviorTreeRunnerEvent.runner = debugRunner;
        }

        void OnSetBehaviorTreeRunner(SetBehaviorTreeRunnerEvent setBehaviorTreeRunnerEvent)
        {
            ClearRunner();
            debugRunner = setBehaviorTreeRunnerEvent.runner;

            if (EditorBehaviorTreeRunner.nodeMessage.TryGetValue(this.debugRunner.uid, out var queue)) queue.Clear();
        }

        private void DrawDebug()
        {
            if (this.debugRunner == null)
            {
                ClearRunner();
                return;
            }

            BehaviorTree behaviorTree = null;

            Queue<BehaviorTree> queue = new Queue<BehaviorTree>();

            queue.Enqueue(this.debugRunner.behaviorTree);

            while (queue.Count > 0)
            {
                BehaviorTree machine = queue.Dequeue();

                if (machine.asset.id == smartValue.graphAsset.id)
                {
                    behaviorTree = machine;
                    break;
                }

                int childrenCount = machine.children.Count;
                for (var i = 0; i < childrenCount; i++)
                {
                    BehaviorTree child = machine.children[i];
                    queue.Enqueue(child);
                }
            }

            if (behaviorTree == null)
            {
                ClearRunner();
                return;
            }

            if (behaviorTree.isActive == false)
            {
                ClearRunner();
                return;
            }

            SetState(behaviorTree);
            ShowMessage();
        }

        private void SetState(BehaviorTree behaviorTree)
        {
            Queue<Node> nodeQueue = new Queue<Node>();
            nodeQueue.Enqueue(behaviorTree.entryNode);

            while (nodeQueue.Count > 0)
            {
                Node node = nodeQueue.Dequeue();

                if (node.state == Node.State.Active)
                {
                    if (this.runningNodes.Contains(node.id) == false)
                    {
                        string editorNodeId = editorBehaviorTreeAsset.cacheBindMap.GetValueOrDefault(node.id);
                        EditorBehaviorTreeNodeView nodeView = smartValue.graphElementCache.nodeViewById.GetValueOrDefault(editorNodeId) as EditorBehaviorTreeNodeView;
                        if (nodeView == null) continue;
                        nodeView.SetFocus(Color.green);
                        runningNodes.Add(node.id);
                    }
                }
                else
                {
                    if (this.runningNodes.Contains(node.id))
                    {
                        string editorNodeId = editorBehaviorTreeAsset.cacheBindMap.GetValueOrDefault(node.id);
                        EditorBehaviorTreeNodeView nodeView = smartValue.graphElementCache.nodeViewById.GetValueOrDefault(editorNodeId) as EditorBehaviorTreeNodeView;
                        if (nodeView == null) continue;
                        nodeView.ClearFocus();
                        runningNodes.Remove(node.id);
                    }
                }

                int childrenCount = node.children.Count;
                for (var i = 0; i < childrenCount; i++)
                {
                    Node child = node.children[i];
                    nodeQueue.Enqueue(child);
                }
            }
        }

        private void ShowMessage()
        {
            int uid = this.debugRunner.uid;

            if (EditorBehaviorTreeRunner.nodeMessage.TryGetValue(uid, out Queue<EditorBehaviorTreeDebugPingMessage> messages) == false) return;

            while (messages.Count > 0)
            {
                EditorBehaviorTreeDebugPingMessage message = messages.Dequeue();

                EditorBehaviorTreeNodeView nodeView = GetEditorBehaviorTreeNodeView(message.nodeId);
                if (nodeView == null) return;
                nodeView.Tips(message.text);

                if (runningNodes.Contains(message.nodeId) == false) nodeView.SetFocus(Color.green, 1500);
            }
        }

        private EditorBehaviorTreeNodeView GetEditorBehaviorTreeNodeView(int nodeId)
        {
            string editorNodeId = editorBehaviorTreeAsset.cacheBindMap.GetValueOrDefault(nodeId);
            if (string.IsNullOrEmpty(editorNodeId)) return null;

            IEditorNodeView nodeView = smartValue.graphElementCache.nodeViewById.GetValueOrDefault(editorNodeId);
            if (nodeView == null) return null;

            return nodeView as EditorBehaviorTreeNodeView;
        }

        private void ClearRunner()
        {
            if (this.debugRunner == null) return;

            int count = runningNodes.Count;
            for (int i = 0; i < count; i++)
            {
                string editorNodeId = editorBehaviorTreeAsset.cacheBindMap.GetValueOrDefault(runningNodes[i]);
                EditorBehaviorTreeNodeView nodeView = smartValue.graphElementCache.nodeViewById.GetValueOrDefault(editorNodeId) as EditorBehaviorTreeNodeView;
                if (nodeView == null) continue;
                nodeView.ClearFocus();
            }

            runningNodes.Clear();
            this.debugRunner = null;
        }
    }
}