using Emilia.BehaviorTree;
using UnityEngine;

namespace Emilia.AI
{
    public class AIMono : MonoBehaviour
    {
        public const string EditorPath = "Assets/Emilia/AI/Resource/Asset";

        public string fileName;

        private Clock clock;
        private IBehaviorTreeRunner _runner;

        public IBehaviorTreeRunner runner => _runner;

        private void OnEnable()
        {
            if (string.IsNullOrEmpty(fileName)) return;

            BehaviorTreeLoader behaviorTreeLoader = new BehaviorTreeLoader();
            behaviorTreeLoader.editorFilePath = EditorPath;

            clock = new Clock();
            clock.Reset(new FloatTimeInfo());

            this._runner = BehaviorTreeRunnerUtility.CreateRunner();
            this._runner.Init(this.fileName, behaviorTreeLoader, clock, gameObject);
            this._runner.Start();
        }

        private void FixedUpdate()
        {
            FloatTimeInfo timeInfo = new FloatTimeInfo(Time.fixedDeltaTime);
            clock.Update(timeInfo);
        }

        private void OnDisable()
        {
            this._runner?.Dispose();
            this._runner = null;
        }
    }
}