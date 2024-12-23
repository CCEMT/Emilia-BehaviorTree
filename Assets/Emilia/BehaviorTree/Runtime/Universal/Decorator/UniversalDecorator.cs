using Emilia.BehaviorTree.Attributes;

namespace Emilia.BehaviorTree
{
    [NodeTag(UniversalDefine.NodeTag)]
    public abstract class UniversalDecoratorAsset<T> : DecoratorAsset<T> where T : Node, new() { }

    public abstract class UniversalDecorator<T> : Decorator<T> where T : NodeAsset { }
}