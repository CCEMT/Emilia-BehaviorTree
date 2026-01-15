using System.Collections.Generic;
using Emilia.BehaviorTree;
using Emilia.BehaviorTree.Editor;
using UnityEngine;

namespace Emilia.AI.Editor
{
    [CreateAssetMenu(menuName = "Emilia/AI/EditorAIAsset", fileName = "EditorAIAsset")]
    public class EditorAIAsset : EditorBehaviorTreeAsset
    {
        public override string outputPath => "Assets/Example/Resource/Config";

        public override List<string[]> tags => new List<string[]>() {
            new[] {BaseDefine.NodeTag},
            new[] {UniversalDefine.NodeTag},
            new[] {AIDefine.NodeTag}
        };
    }
}