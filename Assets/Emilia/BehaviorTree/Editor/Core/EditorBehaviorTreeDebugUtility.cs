using System.Collections.Generic;

namespace Emilia.BehaviorTree.Editor
{
    public struct EditorBehaviorTreeDebugPingMessage
    {
        public int nodeId;
        public string text;
    }

    public static class EditorBehaviorTreeDebugUtility
    {
        public static void Ping(Node node, string message)
        {
            EditorBehaviorTreeDebugPingMessage pingMessage = new EditorBehaviorTreeDebugPingMessage();
            pingMessage.nodeId = node.nodeAsset.id;
            pingMessage.text = message;

            int uid = node.tree.uid;

            if (EditorBehaviorTreeRunner.nodeMessage.ContainsKey(node.tree.uid) == false) EditorBehaviorTreeRunner.nodeMessage[uid] = new Queue<EditorBehaviorTreeDebugPingMessage>();
            EditorBehaviorTreeRunner.nodeMessage[uid].Enqueue(pingMessage);
        }
    }
}