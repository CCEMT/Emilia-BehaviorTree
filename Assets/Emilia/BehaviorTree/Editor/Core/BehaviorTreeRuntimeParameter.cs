using System;
using System.Collections.Generic;
using Emilia.Variables;
using Emilia.Variables.Editor;
using Sirenix.OdinInspector;

namespace Emilia.BehaviorTree.Editor
{
    [Serializable]
    public class BehaviorTreeRuntimeParameter
    {
        private EditorBehaviorTreeRunner runner;

        private Dictionary<string, Variable> _runtimeUserVariables = new Dictionary<string, Variable>();

        [LabelText("参数"), ShowInInspector]
        public Dictionary<string, Variable> runtimeUserVariables
        {
            get
            {
                _runtimeUserVariables.Clear();
                if (this.runner == null) return this._runtimeUserVariables;

                foreach (var variablePair in this.runner.behaviorTree.blackboard.variablesManage.variableMap)
                {
                    EditorParameter editorParameter = this.runner.editorBehaviorTreeAsset.editorParametersManage.parameters.Find((x) => x.key == variablePair.Key);
                    if (editorParameter == null) continue;
                    _runtimeUserVariables[editorParameter.description] = variablePair.Value;
                }

                return this._runtimeUserVariables;
            }

            set { }
        }

        public BehaviorTreeRuntimeParameter(EditorBehaviorTreeRunner runner)
        {
            this.runner = runner;
        }
    }
}