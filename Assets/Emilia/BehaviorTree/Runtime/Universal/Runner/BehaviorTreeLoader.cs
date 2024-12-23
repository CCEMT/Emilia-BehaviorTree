using System;
using Object = UnityEngine.Object;

namespace Emilia.BehaviorTree
{
    public class BehaviorTreeLoader : IBehaviorTreeLoader
    {
        public string runtimeFilePath { get; set; }
        public string editorFilePath { get; set; }

        public Func<string, Object> onLoadAsset { get; set; }
        public Func<byte[], BehaviorTreeAsset> onLoadBehaviorTreeAsset { get; set; }

        public Object LoadAsset(string path)
        {
            return onLoadAsset?.Invoke(path);
        }

        public BehaviorTreeAsset LoadBehaviorTreeAsset(byte[] bytes)
        {
            return onLoadBehaviorTreeAsset?.Invoke(bytes);
        }
    }
}