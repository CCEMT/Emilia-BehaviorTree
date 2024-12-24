using Emilia.BehaviorTree.Attributes;

namespace Emilia.BehaviorTree
{
    [BehaviorTreeMenu("装饰节点/反转节点")]
    public class InverterAsset : BaseDecoratorAsset<Inverter> { }

    public class Inverter : BaseDecorator<InverterAsset>
    {
        protected override void OnStart()
        {
            decoratedNode.Start();
        }

        protected override void OnFinish(bool result)
        {
            decoratedNode.Stop();
        }

        protected override void OnChildStop(Node child, bool result)
        {
            Finish(! result);
        }
    }
}