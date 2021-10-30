using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MygaClient
{
    public class ThreadManager : MonoBehaviour
    {
        public static ThreadManager instance;
        private static readonly List<Action> executeOnMainThread = new List<Action>();
        private static readonly List<Action> executeCopiedOnMainThread = new List<Action>();
        private static readonly List<Action> completedActions = new List<Action>();
        private static List<Action> allActions = new List<Action>();

        public static void SetAllActions(List<Action> actions)
        {
            allActions = actions;
        }

        private static bool actionToExecuteOnMainThread = false;

        private void Awake()
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            UpdateMain();
            UpdateMainOldActions();
        }

        /// <summary>Sets an action to be executed on the main thread.</summary>
        /// <param name="_action">The action to be executed on the main thread.</param>
        public static void ExecuteOnMainThread(Action _action)
        {
            if (_action == null)
            {
                Debug.Log("No action to execute on main thread!");
                return;
            }

            lock (executeOnMainThread)
            {
                executeOnMainThread.Add(_action);
                actionToExecuteOnMainThread = true;
            }
        }

        /// <summary>Executes all code meant to run on the main thread. NOTE: Call this ONLY from the main thread.</summary>
        public static void UpdateMain()
        {
            if (actionToExecuteOnMainThread)
            {
                executeCopiedOnMainThread.Clear();
                lock (executeOnMainThread)
                {
                    executeCopiedOnMainThread.AddRange(executeOnMainThread);
                    executeOnMainThread.Clear();
                    actionToExecuteOnMainThread = false;
                }

                for (int i = 0; i < executeCopiedOnMainThread.Count; i++)
                {
                    completedActions.Add(executeCopiedOnMainThread[i]);
                    executeCopiedOnMainThread[i]();
                }
            }
        }

        public static void UpdateMainOldActions()
        {
            List<Action> updateActions = allActions.Except(completedActions).ToList();

            if (updateActions.Count > 0)
            {
                for (int i = 0; i < updateActions.Count; i++)
                {
                    updateActions[i]();
                    completedActions.Add(updateActions[i]);
                }
            }
        }
    }
}