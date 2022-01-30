using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class Pool
    {
        public List<Agent> Agents { get; set; }
        public Dictionary<Agent, Agent> Solution { get; set; }

        public bool Empty { get { return Agents.Count == 0; } }

        public  Pool(List<Agent> agents)
        {
            Agents = agents;
            Solution = new Dictionary<Agent, Agent>();
        }

        public void Update()
        {
            Agents.RemoveAll(i => i.PreferenceList.Count == 0);
        }

        public void Match(Agent a1, Agent a2)
        {
            Solution.Add(a1, a2);
            Solution.Add(a2, a1);

            Agents.Remove(a1);
            Agents.Remove(a2);
        }

        public void MatchCycle(List<Agent> coreCycle)
        {
            if (coreCycle.Count >= 2)
            {
                Solution.Add(coreCycle.Last(), coreCycle.First());

                for (int i = 0; i < coreCycle.Count -1; i++)
                    Solution.Add(coreCycle[i], coreCycle[i + 1]);
            }
        }

    }
}
