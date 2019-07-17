using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FSMSystemSpace
{
    /// <summary>
    /// Place the labels for the Transitions in this enum.
    /// Don't change the first label, NullTransition as FSMSystem class uses it.
    /// </summary>
    public enum Transition
    {
        NullTransition = 0, // Use this transition to represent a non-existing transition in your system
        ATTACK = 1,
        FOLLOW = 2,
        ALERT = 3
    }

    /// <summary>
    /// Place the labels for the States in this enum.
    /// Don't change the first label, NullTransition as FSMSystem class uses it.
    /// </summary>
    public enum StateID
    {
        NullStateID = 0, // Use this ID to represent a non-existing State in your system	
        FOLLOWSTATE = 1,
        ATTACKSTATE = 2,
        ALERTSTATE = 3
    }

    public enum Popup
    {
        NullPopup = 0,
    }

    public interface IFSMState
    {
        /// <summary>
        /// This method is used to set up the State condition before entering it.
        /// It is called automatically by the FSMSystem class before assigning it
        /// to the current state.
        /// </summary>
        void DoBeforeEntering();

        /// <summary>
        /// This method is used to make anything necessary, as reseting variables
        /// before the FSMSystem changes to another one. It is called automatically
        /// by the FSMSystem before changing to a new state.
        /// </summary>
        void DoBeforeLeaving();

        /// <summary>
        /// 这个方法在当前状态被不停调用。
        /// </summary>
        void DoingSomthing();
    }

    /// <summary>
    /// This class represents the States in the Finite State System.
    /// Each state has a Dictionary with pairs (transition-state) showing
    /// which state the FSM should be if a transition is fired while this state
    /// is the current state.
    /// Method Reason is used to determine which transition should be fired .
    /// Method Act has the code to perform the actions the NPC is supposed do if it's on this state.
    /// </summary>
    public abstract class FSMState : IFSMState
    {
        protected Dictionary<Transition, StateID> map = new Dictionary<Transition, StateID>();
        protected Dictionary<Popup,StateID> map_pop = new Dictionary<Popup, StateID>();
        protected StateID stateID;
        public StateID ID { get { return stateID; } }
        protected string context;

        public string Context
        {
            set => context = value;
        }


        public void AddTransition(Transition trans, StateID id)
        {
            // Check if anyone of the args is invalid
            if (trans == Transition.NullTransition)
            {
               LogUI.Log("FSMState ERROR: NullTransition is not allowed for a real transition");
                return;
            }

            if (id == StateID.NullStateID)
            {
               LogUI.Log("FSMState ERROR: NullStateID is not allowed for a real ID");
                return;
            }

            // Since this is a Deterministic FSM,
            //   check if the current transition was already inside the map
            if (map.ContainsKey(trans))
            {
               LogUI.Log("FSMState ERROR: State " + stateID.ToString() + " already has transition " + trans.ToString() +
                               "Impossible to assign to another state");
                return;
            }

            map.Add(trans, id);
        }

        public void AddPopup(Popup pop, StateID id)
        {
            // Check if anyone of the args is invalid
            if (pop == Popup.NullPopup)
            {
               LogUI.Log("FSMState ERROR: NullPopup is not allowed for a real Popup");
                return;
            }

            if (id == StateID.NullStateID)
            {
               LogUI.Log("FSMState ERROR: NullStateID is not allowed for a real ID");
                return;
            }

            // Since this is a Deterministic FSM,
            //   check if the current transition was already inside the map
            if (map_pop.ContainsKey(pop))
            {
               LogUI.Log("FSMState ERROR: State " + stateID.ToString() + " already has popup " + pop.ToString() +
                           "Impossible to assign to another state");
                return;
            }

            map_pop.Add(pop, id);
        }

        /// <summary>
        /// This method deletes a pair transition-state from this state's map.
        /// If the transition was not inside the state's map, an ERROR message is printed.
        /// </summary>
        public void DeleteTransition(Transition trans)
        {
            // Check for NullTransition
            if (trans == Transition.NullTransition)
            {
               LogUI.Log("FSMState ERROR: NullTransition is not allowed");
                return;
            }

            // Check if the pair is inside the map before deleting
            if (map.ContainsKey(trans))
            {
                map.Remove(trans);
                return;
            }
           LogUI.Log("FSMState ERROR: Transition " + trans.ToString() + " passed to " + stateID.ToString() +
                           " was not on the state's transition list");
        }

        /// <summary>
        /// This method returns the new state the FSM should be if
        ///    this state receives a transition and 
        /// </summary>
        public StateID GetOutputState(Transition trans)
        {
            // Check if the map has this transition
            if (map.ContainsKey(trans))
            {
                return map[trans];
            }
            return StateID.NullStateID;
        }

        /// <summary>
        /// 这个方法通过pop条件获取需要蹦出的状态ID
        /// </summary>
        /// <param name="pop">状态条件</param>
        /// <returns>返回ID</returns>
        public StateID GetOutputState(Popup pop)
        {
            if (map_pop.ContainsKey(pop))
            {
                return map_pop[pop];
            }

            return StateID.NullStateID;
        }


        /// <summary>
        /// This method is used to set up the State condition before entering it.
        /// It is called automatically by the FSMSystem class before assigning it
        /// to the current state.
        /// </summary>
        public virtual void DoBeforeEntering() { }

        //这个方法可以以某个对象为前提进行开始
        public virtual void DoBeforeEntering<T>(T t) { }
        //这个方法可以以某个对象为前提进行开始
        public virtual void DoBeforeEntering<T1>(T1 t1,Vector2 t2)  { }
        /// <summary>
        /// This method is used to make anything necessary, as reseting variables
        /// before the FSMSystem changes to another one. It is called automatically
        /// by the FSMSystem before changing to a new state.
        /// </summary>
        public virtual void DoBeforeLeaving() { }

         /// <summary>
        /// 这个方法在当前状态被不停调用。
        /// </summary>
        public virtual void DoingSomthing() { }

      
    } // class FSMState

}