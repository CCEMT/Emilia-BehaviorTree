using System;
using Emilia.BehaviorTree.Attributes;
using Emilia.Node.Attributes;
using Emilia.Variables;
using Sirenix.OdinInspector;

namespace Emilia.BehaviorTree
{
    [BehaviorTreeMenu("装饰节点/黑板条件"), Serializable]
    public class BlackboardConditionAsset : ObservingAsset<BlackboardCondition>
    {
        [LabelText("比较值"), VariableKeySelector]
        public string leftKey;

        [LabelText("比较操作符")]
        public VariableCompareOperator compareOperator;

        [HideLabel, HorizontalGroup(20)]
        public bool useDefine = true;

        [HorizontalGroup, VariableTypeFilter(nameof(leftKey)), ShowIf(nameof(useDefine))]
        public Variable rightDefineValue = new VariableObject();

        [LabelText("比较值"), VariableKeySelector, HorizontalGroup, HideIf(nameof(useDefine))]
        public string rightKey;
    }

    public class BlackboardCondition : Observing<BlackboardConditionAsset>
    {
        protected override void StartObserving()
        {
            tree.blackboard.Subscribe(this.asset.leftKey, Evaluate);
        }

        protected override void StopObserving()
        {
            tree.blackboard.Unsubscribe(this.asset.leftKey, Evaluate);
        }

        protected override bool IsConditionMet()
        {
            Variable leftValue = tree.blackboard.GetVariable(this.asset.leftKey);
            Variable rightValue = this.asset.useDefine ? this.asset.rightDefineValue : tree.blackboard.GetVariable(this.asset.rightKey);
            if (leftValue == null || rightValue == null) return false;
            return VariableUtility.Compare(leftValue, rightValue, this.asset.compareOperator);
        }
    }
}