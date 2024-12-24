using System;
using System.Collections.Generic;
using Emilia.Node.Universal.Editor;
using Sirenix.Serialization;

namespace Emilia.BehaviorTree.Editor
{
    [NodeToRuntime(typeof(NodeAsset), typeof(EditorBehaviorTreeNodeAsset))]
    public abstract class EditorBehaviorTreeAsset : EditorUniversalGraphAsset
    {
        [NonSerialized, OdinSerialize]
        public BehaviorTreeAsset cache;

        [NonSerialized, OdinSerialize]
        public Dictionary<int, string> cacheBindMap = new Dictionary<int, string>();

        public abstract string outputPath { get; }
        public abstract List<string[]> tags { get; }
    }
}