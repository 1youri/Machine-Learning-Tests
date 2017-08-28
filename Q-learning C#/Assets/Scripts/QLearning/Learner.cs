using System;
using System.Collections.Generic;
using Assets.Scripts.QLearning.Models;
using ActionReward = Assets.Scripts.QLearning.Models.ActionReward;

namespace Assets.Scripts.QLearning
{
    public class Learner
    {
        private static Learner instance;
        public static Learner Instance
        {
            get
            {
                if (instance != null)
                    return instance;
                else return null;
            }
        }

        public List<string> ActionNames;
        public StateActionTable QTable;
        private double discount;
        public double gamma;
        public double gammaDecay;
        public double alpha = 0;
        private int maxIterations;
        private int current_iteration;
        private int time = 1;
        private string currentState;
        private Random rnd;
        private string startState;

        public Learner(List<string> actionNames, string startState, int maxIter = 5000, double discount = 0.3, double gamma = 0.8, double gammaDecay = 0.95)
        {
            instance = this;
            ActionNames = actionNames;
            QTable = new StateActionTable();
            rnd = new Random();
            maxIterations = maxIter;
            currentState = startState;

            this.discount = discount;
            this.gamma = gamma;
            this.gammaDecay = gammaDecay;
            this.startState = startState;
        }

        public string MakeDecision()
        {
            string bestAction = QTable.GetState(currentState).BestAction().ActionName;

            if (rnd.NextDouble() <= gamma)
                bestAction = ActionNames[rnd.Next(0, ActionNames.Count - 1)];

            return bestAction;
        }

        public bool Learn(string chosenAction, string newState, double reward)
        {
            current_iteration += 1;
            if (current_iteration > maxIterations)
            {
                
                EndEpisode();
                return true;
            }


            State prevState = QTable.GetState(currentState);
            ActionReward bestActionNextState = QTable.GetState(newState).BestAction();

            prevState.IncreaseReward_Q(chosenAction, alpha, reward + discount * bestActionNextState.Reward);


            time += 1;
            alpha = Math.Pow(time, -0.1);
            currentState = newState;
            return false;
        }

        public void EndEpisode()
        {
            gamma *= gammaDecay;
            time = 1;
            currentState = startState;
            current_iteration = 0;
        }
    }
}
