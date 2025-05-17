using Emilia.BehaviorTree.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Emilia.BehaviorTree
{
    [BehaviorTreeMenu("装饰节点/随机节点")]
    public class RandomExecuteAsset : UniversalDecoratorAsset<RandomExecute>
    {
        [Range(0, 1), LabelText("概率")]
        public float probability = 0.5f;
    }

    public class RandomExecute : UniversalDecorator<RandomExecuteAsset>
    {
        protected override void OnStart()
        {
            if (Random.value < asset.probability) { decoratedNode.Start(); }
            else { Finish(false); }
        }

        protected override void OnStop()
        {
            decoratedNode.Stop();
        }

        protected override void OnChildStop(Node child, bool result)
        {
            Finish(result);
        }
    }
}