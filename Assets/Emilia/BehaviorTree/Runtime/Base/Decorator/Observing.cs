using Sirenix.OdinInspector;
using UnityEngine;

namespace Emilia.BehaviorTree
{
    public interface IObservingAsset
    {
        Stops stops { get; }
    }

    public interface IObserving { }

    public abstract class ObservingAsset<T> : BaseDecoratorAsset<T>, IObservingAsset where T : Node, new()
    {
        [LabelText("停止策略"), SerializeField]
        private Stops _stops;

        public Stops stops => this._stops;
    }

    public abstract class Observing<T> : BaseDecorator<T> where T : NodeAsset
    {
        private IObservingAsset observingAsset;
        private bool isObserving;

        protected override void OnInit()
        {
            base.OnInit();
            observingAsset = (IObservingAsset) this.nodeAsset;
        }

        protected override void OnStart()
        {
            if (this.observingAsset.stops != Stops.None)
            {
                if (this.isObserving == false)
                {
                    this.isObserving = true;
                    StartObserving();
                }
            }

            if (IsConditionMet() == false) Finish(false);
            else decoratedNode.Start();
        }

        protected override void OnStop()
        {
            decoratedNode.Stop();
        }

        protected override void OnChildStop(Node child, bool result)
        {
            if (this.observingAsset.stops == Stops.None || this.observingAsset.stops == Stops.Self)
            {
                if (this.isObserving)
                {
                    this.isObserving = false;
                    StopObserving();
                }
            }

            Finish(result);
        }

        protected override void OnParentCompositeStop(Composite composite)
        {
            base.OnParentCompositeStop(composite);
            if (this.isObserving == false) return;
            this.isObserving = false;

            StopObserving();
        }

        protected void Evaluate()
        {
            bool isConditionMet = IsConditionMet();
            Stops stops = observingAsset.stops;

            if (state == State.Active && isConditionMet == false)
            {
                if (stops == Stops.Self || stops == Stops.Both || stops == Stops.ImmediateRestart) Stop();
            }
            else if (state != State.Active && isConditionMet)
            {
                if (stops != Stops.LowerPriority && stops != Stops.Both && stops != Stops.ImmediateRestart && stops != Stops.LowerPriorityImmediateRestart) return;

                Node parentNode = parent;
                Node childNode = this;

                Composite composite = parentNode as Composite;

                while (parentNode != null && composite == null)
                {
                    childNode = parentNode;
                    parentNode = parentNode.parent;

                    composite = parentNode as Composite;
                }

                bool isStopLowerPriorityChildrenForChild = stops == Stops.ImmediateRestart || stops == Stops.LowerPriorityImmediateRestart;

                if (isStopLowerPriorityChildrenForChild)
                {
                    if (this.isObserving)
                    {
                        this.isObserving = false;
                        StopObserving();
                    }
                }

                composite.StopLowerPriorityChildrenForChild(childNode, isStopLowerPriorityChildrenForChild);
            }
        }

        protected abstract void StartObserving();

        protected abstract void StopObserving();

        protected abstract bool IsConditionMet();
    }
}