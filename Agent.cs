using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp3
{
    class Agent
    {
        public double L1 { get; set; } // latitude
        public double L2 { get; set; } // longitude

        public string L1L2
        {
            get
            {
                string l1 = String.Format("{0:0000}", L1).Replace(',', '.');
                string l2 = String.Format("{0:0000}", L2).Replace(',', '.');
                return String.Format("[{0}:{1}]", l1, l2);
            }
        }

        public List<int> Values;

        public int ID { get; set; }
        public int Index { get; set; }

        public int Rank
        {
            get
            { return PreferenceList[0].PreferenceList.IndexOf(this); }
        }



        public List<Agent> PreferenceList;


        public Agent()
        {
            Values = new List<int>();
            PreferenceList = new List<Agent>();
        }



        public void Test(Agent agent)
        {
            double additionalSeconds = 600; // en az beş dakika

            if (agent.Values[Index] + additionalSeconds < Values[Index]
                && Values[agent.Index] + additionalSeconds < agent.Values[agent.Index])
            {
                PreferenceList.Add(agent);
                PreferenceList = PreferenceList.OrderBy(i => i.Values[Index]).ToList();
            }
        }


        public Agent best;
        public Agent Best
        {
            get { return best; }
            set
            {
                if (best == null)
                    best = value;

                else if (PreferenceList.IndexOf(best) > PreferenceList.IndexOf(value))
                    best = value;

            }
        }

        public Agent FirstChoice
        {
            get { return PreferenceList[0]; }
        }

        public Agent SecondChoice
        {
            get { return PreferenceList[1]; }
        }

        public Agent LastChoice
        {
            get { return PreferenceList[PreferenceList.Count - 1]; }
        }

        public void Clean(Agent agent)
        {
            PreferenceList.Remove(agent);
            agent.PreferenceList.Remove(this);
        }

    }

}
