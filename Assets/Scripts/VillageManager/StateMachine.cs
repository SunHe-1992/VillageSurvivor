using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunHeTBS
{
    public class StateMachine
    {
      

        public class State
        {
            public PawnState state;
            public string name;
            public System.Action OnFrame;
            public System.Action OnEnter;
            public System.Action OnExit;
            public float elpsedTime;
            public BuildingEffectType targetBuildingType = BuildingEffectType.Default;
            public void StateOnEnter()
            {
                this.elpsedTime = 0;
            }
            public override string ToString()
            {
                return name;
            }

        }
        public Dictionary<string, State> states = new Dictionary<string, State>();

        public State currentState { get; private set; }

        State initialState;

        public State CreateState(string _name)
        {
            var st = new State()
            {
                name = _name
            };
            if (states.Count == 0)
                initialState = st;

            states[_name] = st;

            return st;
        }
        public void Update()
        {
            if (states.Count == 0 || initialState == null)
            {
                Debug.LogError("state machine with no states!");
                return;
            }
            if (currentState == null)
            {
                TransitionTo(initialState);
            }
            else
            {
                currentState.OnFrame.Invoke();
                currentState.elpsedTime += Time.deltaTime;
            }
        }

        public void TransitionTo(State st)
        {
            if (st != null)
            {
                if (currentState != st)
                {
                    if (currentState != null)
                    {
                        currentState.OnExit?.Invoke();
                        Debug.Log($"transitioning from {currentState.ToString()} to {st.ToString()}");
                    }
                    currentState = st;
                    st.OnEnter?.Invoke();
                    st.StateOnEnter();
                }
                else //stay at the same state, do nothing.
                {
                }
            }
            else
            {
                Debug.LogError("try to transnit to null state");
            }
        }


        public void TransitionTo(string name)
        {
            if (states.ContainsKey(name) == false)
            {
                Debug.LogError($"can not find this state {name}");
                return;
            }

            if (currentState != null)
            {
                currentState.OnExit?.Invoke();
                Debug.Log($"transitioning from {currentState.ToString()} to {states[name].ToString()}");
            }
            currentState = states[name];
            currentState.OnEnter?.Invoke();


        }
    }

}