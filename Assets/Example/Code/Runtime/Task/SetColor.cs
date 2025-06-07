using System;
using Emilia.BehaviorTree.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Emilia.AI.Task
{
    [BehaviorTreeMenu("任务节点/设置颜色"), Serializable]
    public class SetColorAsset : AITaskAsset<SetColor>
    {
        [LabelText("颜色")]
        public Color color;
    }

    public class SetColor : AITask<SetColorAsset>
    {
        private GameObject gameObject;
        private Renderer renderer;

        protected override void OnInit()
        {
            base.OnInit();
            gameObject = owner as GameObject;
            if (gameObject != null) renderer = gameObject.GetComponentInChildren<Renderer>();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            gameObject = null;
            renderer = null;
        }

        protected override void OnStart()
        {
            base.OnStart();

            if (this.renderer == null) renderer = gameObject.GetComponentInChildren<Renderer>();
            if (renderer != null) renderer.material.SetColor("_Color", asset.color);

            Finish(true);
        }
    }
}