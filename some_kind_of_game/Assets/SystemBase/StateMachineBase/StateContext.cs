using UniRx;
using UnityEngine;

namespace SystemBase.StateMachineBase
{
    public class StateContext<T> : IStateContext<BaseState<T>, T>
    {
        public StateContext()
        {
            BevoreStateChange = new ReactiveCommand<BaseState<T>>();
            AfterStateChange = new ReactiveCommand<BaseState<T>>();
        }

        public ReactiveCommand<BaseState<T>> AfterStateChange { get; }

        public ReactiveCommand<BaseState<T>> BevoreStateChange { get; }

        public ReactiveProperty<BaseState<T>> CurrentState { get; private set; }

        public bool GoToState(BaseState<T> state)
        {
            if (!CurrentState.Value.ValidNextStates.Contains(state.GetType()) ||
                !CurrentState.Value.Exit())
            {
                return false;
            }

            BevoreStateChange.Execute(state);

            CurrentState.Value = state;
            CurrentState.Value.Enter(this);

            AfterStateChange.Execute(state);

            return true;
        }

        public void Start(BaseState<T> initialState)
        {
            CurrentState = new ReactiveProperty<BaseState<T>>(initialState);
            BevoreStateChange.Execute(CurrentState.Value);
            CurrentState.Value.Enter(this);
            AfterStateChange.Execute(initialState);
        }
    }
}