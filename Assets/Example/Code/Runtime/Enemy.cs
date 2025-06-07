using UnityEngine;

namespace Emilia.AI
{
    public class Enemy : MonoBehaviour
    {
        [BehaviorTreeKeySelector(nameof(path))]
        public string playerDistanceKey;

        [BehaviorTreeKeySelector(nameof(path))]
        public string playerPositionKey;

        public AIMono aiMono;

        public string path => $"{AIMono.EditorPath}/{this.aiMono.fileName}.asset";

        private void Update()
        {
            if (this.aiMono.runner.isActive == false) return;
            Vector3 playerLocalPos = this.transform.InverseTransformPoint(GameObject.FindGameObjectWithTag("Player").transform.position);
            aiMono.runner.behaviorTree.blackboard.Set(playerPositionKey, playerLocalPos);
            aiMono.runner.behaviorTree.blackboard.Set(playerDistanceKey, playerLocalPos.magnitude);
        }
    }
}