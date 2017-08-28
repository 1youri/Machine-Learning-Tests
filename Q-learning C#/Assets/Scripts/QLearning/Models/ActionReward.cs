using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.QLearning.Models
{
    public class ActionReward
    {
        public ActionReward(string actionName, double reward)
        {
            ActionName = actionName;
            Reward = reward;
        }

        public string ActionName { get; set; }
        public double Reward { get; set; }
    }
}
