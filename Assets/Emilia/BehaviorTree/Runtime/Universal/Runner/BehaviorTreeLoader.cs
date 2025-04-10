using System;
using Object = UnityEngine.Object;

namespace Emilia.BehaviorTree
{
    public class BehaviorTreeLoader : IBehaviorTreeLoader
    {
        public string runtimeFilePath { get; set; }
        public string editorFilePath { get; set; }

        public Func<string, Object> onLoadAsset { get; set; }
        public Action<string> onReleaseAsset { get; set; }
        public Func<byte[], BehaviorTreeAsset> onLoadBehaviorTreeAsset { get; set; }

        public Object LoadAsset(string path) => onLoadAsset?.Invoke(path);
        public void ReleaseAsset(string path) => onReleaseAsset?.Invoke(path);
        public BehaviorTreeAsset LoadBehaviorTreeAsset(byte[] bytes) => onLoadBehaviorTreeAsset?.Invoke(bytes);
    }
}