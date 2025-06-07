using System;
using Emilia.BehaviorTree.Attributes;
using Emilia.Node.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Emilia.AI.Task
{
    [BehaviorTreeMenu("任务节点/移动"), Serializable]
    public class MoveAsset : AITaskAsset<Move>
    {
        [LabelText("位置"), VariableKeySelector]
        public string key;

        [LabelText("速度")]
        public float speed = 0.5f;
    }

    public class Move : AITask<MoveAsset>
    {
        private GameObject gameObject;

        protected override void OnStart()
        {
            base.OnStart();

            if (this.gameObject == null) this.gameObject = owner as GameObject;

            UpdatePosition();

            clock.AddUpdateEvent(UpdatePosition);
        }

        private void UpdatePosition()
        {
            Vector3 localPosition = blackboard.Get<Vector3>(asset.key);
            if (gameObject != null) gameObject.transform.localPosition += localPosition * this.asset.speed * Time.deltaTime;
        }

        protected override void OnStop()
        {
            base.OnStop();
            clock.RemoveUpdateEvent(UpdatePosition);
            Finish(true);
        }
    }
}