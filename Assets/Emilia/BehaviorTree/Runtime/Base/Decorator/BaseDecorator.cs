using Emilia.BehaviorTree.Attributes;

namespace Emilia.BehaviorTree
{
    [NodeTag(BaseDefine.NodeTag)]
    public abstract class BaseDecoratorAsset<T> : DecoratorAsset<T> where T : Node, new() { }

    public abstract class BaseDecorator<T> : Decorator<T> where T : NodeAsset { }
}