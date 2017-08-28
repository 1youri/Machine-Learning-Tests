using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.QLearning.Models
{
    public class StateActionTable
    {
        public StateActionTable()
        {
            QMatrix = new Dictionary<string, State>();
        }

        public Dictionary<string,State> QMatrix { get; set; }

        public State GetState(string state)
        {
            if(!QMatrix.ContainsKey(state))
                QMatrix.Add(state, new State(state));
            return QMatrix[state];
        }


        
    }
}
