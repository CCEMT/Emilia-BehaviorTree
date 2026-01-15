using System;
using System.IO;
using Emilia.DataBuildPipeline.Editor;
using Emilia.Kit;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;

namespace Emilia.BehaviorTree.Editor
{
    [BuildPipeline(typeof(BehaviorTreeBuildArgs)), BuildSequence(2000)]
    public class BehaviorTreeOutputFile : IDataOutput
    {
        public void Output(IBuildContainer buildContainer, IBuildArgs buildArgs, Action onFinished)
        {
            BehaviorTreeBuildContainer container = buildContainer as BehaviorTreeBuildContainer;
            BehaviorTreeBuildArgs args = buildArgs as BehaviorTreeBuildArgs;

            if (args.isGenerateFile == false || string.IsNullOrEmpty(args.outputPath))
            {
                onFinished.Invoke();
                return;
            }

            string dataPathNoAssets = Directory.GetParent(Application.dataPath).ToString();
            string path = $"{dataPathNoAssets}/{args.outputPath}/{args.behaviorTreeAsset.name}.bytes";

            System.Threading.Tasks.Task.Run(() => {
                try
                {
                    if (File.Exists(path)) File.Delete(path);
                    byte[] bytes = TagSerializationUtility.IgnoreTagSerializeValue(container.asset, DataFormat.Binary, SerializeTagDefine.DefaultIgnoreTag);
                    File.WriteAllBytes(path, bytes);
                    EditorApplication.delayCall += RefreshAssetDatabase;
                }
                catch (Exception e)
                {
                    EditorApplication.delayCall += () => Debug.LogError(e.ToUnityLogString());
                }
            });

            onFinished.Invoke();

            void RefreshAssetDatabase()
            {
                if (args.isSaveAsset) AssetDatabase.SaveAssets();
                if (args.isRefresh) AssetDatabase.Refresh();
                args.generateFileCallback?.Invoke();
            }
        }
    }
}