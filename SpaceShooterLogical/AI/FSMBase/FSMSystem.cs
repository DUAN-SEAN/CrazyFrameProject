using CrazyEngine;
using System;
using System.Collections;
using System.Collections.Generic;


namespace FSMSystemSpace
{

    /// <summary>
    /// FSMSystem class represents the Finite State Machine class.
    ///  It has a List with the States the NPC has and methods to add,
    ///  delete a state, and to change the current state the Machine is on.
    /// </summary>
    public class FSMSystem
    {
        private List<FSMState> states;

        // The only way one can change the state of the FSM is by performing a transition
        // Don't change the CurrentState directly
        private StateID currentStateID;
        public StateID CurrentStateID => currentStateID;
        private FSMState currentState;
        public FSMState CurrentState => currentState;
        public StateID CurrentPopupID => PopStack.Count == 0 ? StateID.NullStateID : PopStack.Peek().ID;
        private Stack<FSMState> PopStack = new Stack<FSMState>();

        public FSMSystem()
        {
            states = new List<FSMState>();
        }


        public void TickState()
        {
            currentState.DoingSomthing();
        }

        /// <summary>
        /// This method places new states inside the FSM,
        /// or prints an ERROR message if the state was already inside the List.
        /// First state added is also the initial state.
        /// </summary>
        public void AddState(FSMState s)
        {
            // Check for Null reference before deleting
            if (s == null)
            {
               //LogUI.Log("FSM ERROR: Null reference is not allowed");
                return;
            }

            // First State inserted is also the Initial state,
            //   the state the machine is in when the simulation begins
            if (states.Count == 0)
            {
                states.Add(s);
                currentState = s;
                currentStateID = s.ID;
                currentState.DoBeforeEntering();
                return;
            }

            // Add the state to the List if it's not inside it
            foreach (FSMState state in states)
            {
                if (state.ID == s.ID)
                {
                   //LogUI.Log("FSM ERROR: Impossible to add state " + s.ID.ToString() +
                   //                " because state has already been added");
                    return;
                }
            }
            states.Add(s);
        }

        public void StartWithState(StateID stateId)
        {
            if (currentState != null)
            {
               //LogUI.Log("当前已有状态");
                return;
            }
            // Update the currentStateID and currentState		
            currentStateID = stateId;
            foreach (var state in states)
            {
                if (state.ID != currentStateID) continue;
                currentState = state;
                // Reset the state to its desired condition before it can reason or act
                currentState.DoBeforeEntering();
                break;
            }
        }
        /// <summary>
        /// This method delete a state from the FSM List if it exists, 
        ///   or prints an ERROR message if the state was not on the List.
        /// </summary>
        public void DeleteState(StateID id)
        {
            // Check for NullState before deleting
            if (id == StateID.NullStateID)
            {
               //LogUI.Log("FSM ERROR: NullStateID is not allowed for a real state");
                return;
            }

            // Search the List and delete the state if it's inside it
            foreach (FSMState state in states)
            {
                if (state.ID == id)
                {
                    states.Remove(state);
                    return;
                }
            }
           //LogUI.Log("FSM ERROR: Impossible to delete state " + id.ToString() +
           //                ". It was not on the list of states");
        }

        /// <summary>
        /// This method tries to change the state the FSM is in based on
        /// the current state and the transition passed. If current state
        ///  doesn't have a target state for the transition passed, 
        /// an ERROR message is printed.
        /// </summary>
        public void PerformTransition(Transition trans)
        {
            // Check for NullTransition before changing the current state
            if (trans == Transition.NullTransition)
            {
               //LogUI.Log("FSM ERROR: NullTransition is not allowed for a real transition");
                return;
            }

            // Check if the currentState has the transition passed as argument
            StateID id = currentState.GetOutputState(trans);
            if (id == StateID.NullStateID)
            {
               //LogUI.Log("FSM ERROR: State " + currentStateID.ToString() + " does not have a target state " +
               //                " for transition " + trans.ToString());
                return;
            }

            // Pop PopStack all elements
            for (var i = 0; i < PopStack.Count; i++)
            {
                PopStack.Pop().DoBeforeLeaving();
            }



            // Update the currentStateID and currentState		
            currentStateID = id;
            foreach (FSMState state in states)
            {
                if (state.ID == currentStateID)
                {
                    // Do the post processing of the state before setting the new one
                    currentState.DoBeforeLeaving();

                    currentState = state;

                    // Reset the state to its desired condition before it can reason or act
                    currentState.DoBeforeEntering();
                    break;
                }
            }

        } // PerformTransition()


        /// <summary>
        /// This method tries to change the state the FSM is in based on
        /// the current state and the transition passed. If current state
        ///  doesn't have a target state for the transition passed, 
        /// an ERROR message is printed.
        /// </summary>
        public void PerformTransition<T>(Transition trans,T t)
        {
            // Check for NullTransition before changing the current state
            if (trans == Transition.NullTransition)
            {
               //LogUI.Log("FSM ERROR: NullTransition is not allowed for a real transition");
                return;
            }

            // Check if the currentState has the transition passed as argument
            StateID id = currentState.GetOutputState(trans);
            if (id == StateID.NullStateID)
            {
               //LogUI.Log("FSM ERROR: State " + currentStateID.ToString() + " does not have a target state " +
               //                " for transition " + trans.ToString());
                return;
            }

            // Pop PopStack all elements
            for (var i = 0; i < PopStack.Count; i++)
            {
                PopStack.Pop().DoBeforeLeaving();
            }



            // Update the currentStateID and currentState       
            currentStateID = id;
            foreach (FSMState state in states)
            {
                if (state.ID == currentStateID)
                {
                    // Do the post processing of the state before setting the new one
                    currentState.DoBeforeLeaving();

                    currentState = state;

                    // Reset the state to its desired condition before it can reason or act
                    currentState.DoBeforeEntering(t);
                    break;
                }
            }

        } // PerformTransition()
        /// <summary>
        /// This method tries to change the state the FSM is in based on
        /// the current state and the transition passed. If current state
        ///  doesn't have a target state for the transition passed, 
        /// an ERROR message is printed.
        /// </summary>
        public void PerformTransition<T1>(Transition trans, T1 t1,Vector2 t2)
        {
            // Check for NullTransition before changing the current state
            if (trans == Transition.NullTransition)
            {
                //LogUI.Log("FSM ERROR: NullTransition is not allowed for a real transition");
                return;
            }

            // Check if the currentState has the transition passed as argument
            StateID id = currentState.GetOutputState(trans);
            if (id == StateID.NullStateID)
            {
                //LogUI.Log("FSM ERROR: State " + currentStateID.ToString() + " does not have a target state " +
                //                " for transition " + trans.ToString());
                return;
            }

            // Pop PopStack all elements
            for (var i = 0; i < PopStack.Count; i++)
            {
                PopStack.Pop().DoBeforeLeaving();
            }



            // Update the currentStateID and currentState       
            currentStateID = id;
            foreach (FSMState state in states)
            {
                if (state.ID == currentStateID)
                {
                    // Do the post processing of the state before setting the new one
                    currentState.DoBeforeLeaving();

                    currentState = state;

                    // Reset the state to its desired condition before it can reason or act
                    currentState.DoBeforeEntering(t1,t2);
                    break;
                }
            }

        } // PerformTransition()
        public void PerformPopup(Popup pop)
        {
            // Check for NullTransition before changing the current state
            if (pop == Popup.NullPopup)
            {
               //LogUI.Log("FSM ERROR: NullPopup is not allowed for a real popup");
                return;
            }

            // Check if the currentState has the transition passed as argument
            StateID id = currentState.GetOutputState(pop);
            if (id == StateID.NullStateID)
            {
               //LogUI.Log("FSM ERROR: State " + currentStateID.ToString() + " does not have a target state " +
               //            " for popup " + pop.ToString());
                return;
            }


            foreach (FSMState state in states)
            {
                if (state.ID == id)
                {
                    state.DoBeforeEntering();
                    PopStack.Push(state);
                    break;
                }
            }
        }//PerformPopup

        public void PerformPopup(Popup pop, string text)
        {
            // Check for NullTransition before changing the current state
            if (pop == Popup.NullPopup)
            {
               //LogUI.Log("FSM ERROR: NullPopup is not allowed for a real popup");
                return;
            }

            // Check if the currentState has the transition passed as argument
            StateID id = currentState.GetOutputState(pop);
            if (id == StateID.NullStateID)
            {
               //LogUI.Log("FSM ERROR: State " + currentStateID.ToString() + " does not have a target state " +
               //            " for popup " + pop.ToString());
                return;
            }


            foreach (FSMState state in states)
            {
                if (state.ID == id)
                {
                    state.Context = text;
                    state.DoBeforeEntering();
                    PopStack.Push(state);
                    break;
                }
            }
        }//PerformPopup

        public void PerformPopDone()
        {
            if (PopStack.Count == 0)
            {
               //LogUI.Log("FSM ERROR: PopStack is no elements");
            }
            var popState = PopStack.Pop();
            popState.DoBeforeLeaving();
        }


    } //class FSMSystem

}
