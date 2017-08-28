using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.QLearning;

namespace Assets.Scripts.QLearning.Models
{
    public class State
    {
        public string StateName { get; set; }
        public List<ActionReward> ActionRewards { get; set; }

        public State(string stateName)
        {
            StateName = stateName;
            ActionRewards = new List<ActionReward>();

            foreach (var action in Learner.Instance.ActionNames)
            {
                ActionRewards.Add(new ActionReward(action, 0.1f));
            }
        }

        public ActionReward BestAction()
        {
            ActionReward bestAction = null;
            foreach (var actionReward in ActionRewards)
                if (bestAction == null || actionReward.Reward > bestAction.Reward)
                    bestAction = actionReward;

            return bestAction;
        }

        public ActionReward FindAction(string action)
        {
            return ActionRewards.Find(x => x.ActionName == action);
        }

        public void IncreaseReward_Q(string actionName, double alpha, double inc)
        {
            ActionReward action = FindAction(actionName);
            action.Reward *= 1 - alpha;
            action.Reward += alpha * inc;
        }
    }
}
