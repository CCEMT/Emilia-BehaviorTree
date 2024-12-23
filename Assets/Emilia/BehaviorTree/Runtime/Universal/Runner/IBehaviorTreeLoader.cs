using UnityEngine;

namespace Emilia.BehaviorTree
{
    public interface IBehaviorTreeLoader
    {
        string runtimeFilePath { get; }
        string editorFilePath { get; }
        
        Object LoadAsset(string path);
        BehaviorTreeAsset LoadBehaviorTreeAsset(byte[] bytes);
    }
}