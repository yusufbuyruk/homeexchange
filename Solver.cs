using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace ConsoleApp3
{
    struct Solution
    {
        public int beforeLocal, afterLocal, beforeGlobal, afterGlobal;
        public int localSize, globalSize;
        public double improvementLocal, improvementGlobal;
    }

    struct Match
    {
        public Agent a1, a2;
    }

    class Solver
    {
        public List<Agent> agents;
        public List<Match> matchList;

        public Solver()
        {
            List<Solution> solutions = new List<Solution>();

            int i = 88;
            int k = 4;

            //for (int i = 0; i < 80; i++)
            //{
            Console.Clear();
            Console.WriteLine(i);

            LoadFile("distance_matrix_generated.txt", k, i);
            Solution s1 = StableRoommate();
            solutions.Add(s1);
            Print(s1);
            Console.ReadKey();

            //LoadFile("distance_matrix_generated.txt", k, i);
            //Solution s2 = PreferenceRank();
            //solutions.Add(s2);
            //Print(s2);

            //LoadFile("distance_matrix_generated.txt", k, i);
            //Solution s3 = TopTradingCycles();
            //solutions.Add(s3);
            //Print(s3);
            //}
            Console.Clear();
            Console.WriteLine("Average Matching Agents {0}", solutions.Average(item => item.localSize));
            Console.WriteLine("Average Improvements Local {0}", solutions.Average(item => item.improvementLocal));
            Console.WriteLine("Average Improvements Global {0}", solutions.Average(item => item.improvementGlobal));

            Console.ReadKey();

        }

        public void LoadFile(string fileName, int size, int seed)
        {
            List<Agent> agentsLoaded  = new List<Agent>();
            agents = new List<Agent>();
            matchList = new List<Match>();
            Random rnd = new Random(seed);

            using (StreamReader sr = new StreamReader(fileName))
            {
                for (int i = 0; i < 1260; i++)
                {
                    string line = sr.ReadLine();
                    string[] tokens = line.Split(',');

                    Agent agent = new Agent();
                    agent.ID = i;
                    agent.L1 = Convert.ToDouble(tokens[0].Replace('.', ','));
                    agent.L2 = Convert.ToDouble(tokens[1].Replace('.', ','));

                    line = sr.ReadLine();
                    tokens = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

                    foreach (var token in tokens)
                        agent.Values.Add(Convert.ToInt32(token));

                    agentsLoaded.Add(agent);
                }
            }

            // TOURNAMENT SELECTION
            for (int college_index = 0; college_index < 1000; college_index++)
            {
                    Agent currentBest = agentsLoaded[rnd.Next(0, agentsLoaded.Count)];

                    for (int k = 1; k < size; k++) // TOURNAMENT SIZE K = 4
                    {
                        Agent agent = agentsLoaded[rnd.Next(0, agentsLoaded.Count)];
                        if (agent.Values[college_index] < currentBest.Values[college_index])
                            currentBest = agent;
                    }

                    currentBest.Index = college_index;
                    agentsLoaded.Remove(currentBest);
                    agents.Add(currentBest);
  

            }

            // MUTUAL TEST
            foreach (var a1 in agents)
                foreach (var a2 in agents)
                    a1.Test(a2);
        }


        public Solution TopTradingCycles()
        {

            List<Agent> matchPool = new List<Agent>(agents);
            matchList.Clear();

            while (true)
            {
                matchPool.RemoveAll(i => i.PreferenceList.Count == 0);

                if (matchPool.Count == 0) break;

                List<Agent> ttcPool = new List<Agent>();
                Agent ttcAgent = matchPool[0];

                while (true)
                {
                    if (ttcPool.Contains(ttcAgent))
                    {
                        while (ttcPool.IndexOf(ttcAgent) > 0)
                            ttcPool.RemoveAt(0);
                        break;
                    }
                    else
                    {
                        ttcPool.Add(ttcAgent);
                        ttcAgent = ttcAgent.FirstChoice;
                    }
                }

                foreach (var agent in ttcPool)
                {
                    Match m = new Match{ a1 = agent, a2 = agent.FirstChoice};
                    matchList.Add(m);

                    matchPool.Remove(agent);
                }

                foreach (var poolAgent in matchPool)
                    foreach (var cycleAgent in ttcPool)
                        poolAgent.PreferenceList.Remove(cycleAgent);
            }

            return SetSolution();
        }

        public Solution PreferenceRank()
        {

            List<Agent> matchPool = new List<Agent>(agents);
            matchList.Clear();

            while (true)
            {
                matchPool.RemoveAll(i => i.PreferenceList.Count == 0);

                if (matchPool.Count == 0)
                    break;

                Agent bestRank = matchPool.OrderBy(i => i.Rank).ToList()[0];
                Agent firstChoice = bestRank.FirstChoice;

                Match m1 = new Match { a1 = bestRank, a2 = firstChoice };
                Match m2 = new Match { a1 = firstChoice, a2 = bestRank };
                matchList.Add(m1);
                matchList.Add(m2);

                matchPool.Remove(bestRank);
                matchPool.Remove(firstChoice);

                foreach (var agent in matchPool)
                {
                    agent.PreferenceList.Remove(bestRank);
                    agent.PreferenceList.Remove(firstChoice);
                }
            }

            return SetSolution();
        }

        public Solution StableRoommate()
        {
            List<Agent> matchPool = new List<Agent>(agents);
            matchList.Clear();

            Console.WriteLine("startOK");

            // STEP 1
            bool flag = true;

            while (flag)
            {
                flag = false;

                // CLEAN EMPTY
                matchPool.RemoveAll(i => i.PreferenceList.Count == 0);

                // REQUEST
                foreach (var agent in matchPool)
                    agent.FirstChoice.Best = agent;

                // CLEAN FIRST // CLEAN BEFORE
                foreach (var agent in matchPool
                    )
                    if (agent.FirstChoice.Best != agent)
                    {
                        flag = true;
                        agent.PreferenceList.Remove(agent.FirstChoice);
                    }

                // CLEAN LAST // CLEAN AFTER
                foreach (var agent in matchPool)
                {
                    if (agent.Best != null)
                    {
                        int best = agent.PreferenceList.IndexOf(agent.Best);
                        while (agent.PreferenceList.Count > best + 1)
                            agent.Clean(agent.PreferenceList[best + 1]);
                    }
                }
            }

            Console.WriteLine("step1ok");


            // STEP 2
            List<Agent> topList = new List<Agent>();
            List<Agent> bottomList = new List<Agent>();

            Agent topItem;
            Agent bottomItem;

            foreach (var agent in matchPool)
            {
                while (agent.PreferenceList.Count > 1)
                {
                    topList.Clear();
                    bottomList.Clear();

                    topItem = agent;
                    

                    while (true)
                    {
                        topList.Add(topItem);
                        bottomItem = topItem.SecondChoice;
                        bottomList.Add(bottomItem);
                        topItem = bottomItem.LastChoice;

                        if (topList.Contains(topItem))
                        {
                            while (topList.IndexOf(topItem) > 0)
                            {
                                topList.RemoveAt(0);
                                bottomList.RemoveAt(0);
                            }

                            topList.Add(topItem);
                            break;
                        }
                    }

                    

                    for (int i = 0; i < bottomList.Count; i++)
                    {
                        bottomList[i].Clean(topList[i + 1]);
                    }
                }
            }

            foreach (var agent in matchPool)
            {
                if (agent.PreferenceList.Count > 0)
                {
                    Match m = new Match { a1 = agent, a2 = agent.FirstChoice };
                    matchList.Add(m);
                }
            }

            return SetSolution();
        }

        public Solution SetSolution()
        {
            Solution solution;
            solution.beforeLocal = 0;
            solution.afterLocal = 0;
            solution.localSize = matchList.Count;

            foreach (var match in matchList)
            {
                Agent a1 = match.a1;
                Agent a2 = match.a2;

                int onceki_sure = a1.Values[a1.Index];
                int sonraki_sure = a2.Values[a1.Index];
                int fark = onceki_sure - sonraki_sure;

                solution.beforeLocal += onceki_sure;
                solution.afterLocal += sonraki_sure;
            }
            solution.improvementLocal = (1 - ((double)solution.afterLocal / (double)solution.beforeLocal)) * 100;




            solution.beforeGlobal = 0;
            solution.afterGlobal = 0;
            solution.globalSize = agents.Count;

            foreach (var agent in agents)
            {
                int before;
                int after;

                before = agent.Values[agent.Index];
                after = agent.Values[agent.Index];


                foreach (var match in matchList)
                    if (match.a1 == agent)
                        after = match.a2.Values[agent.Index];

                solution.beforeGlobal += before;
                solution.afterGlobal += after;
            }
            solution.improvementGlobal = (1 - (solution.afterGlobal / (double)solution.beforeGlobal)) * 100;

            return solution;

        }

        public void Print(Solution s)
        {
            StringBuilder report = new StringBuilder();

            report.AppendLine();
            report.AppendLine(String.Format("{0} Agents are matched.", s.localSize));
            report.AppendLine(String.Format("Matching Ratio {0:0.0}", s.localSize / (double)s.globalSize * 100));

            report.AppendLine();
            report.AppendLine(String.Format("Among Matching Agents ({0})", s.localSize));
            report.AppendLine(String.Format("Improvement {0:0.00}", s.improvementLocal));
            report.AppendLine(String.Format("Average Travel Time Before Match  {0}", TimeFromSeconds(s.beforeLocal / s.localSize)));
            report.AppendLine(String.Format("Average Travel Time After Match   {0}", TimeFromSeconds(s.afterLocal / s.localSize)));

            report.AppendLine();

            report.AppendLine(String.Format("In Whole System ({0})", s.globalSize));

            report.AppendLine(String.Format("Improvement {0:0.00}", s.improvementGlobal));
            report.AppendLine(String.Format("Average Travel Time Before Match  {0}", TimeFromSeconds(s.beforeGlobal / s.globalSize)));
            report.AppendLine(String.Format("Average Travel Time After Match   {0}", TimeFromSeconds(s.afterGlobal / s.globalSize)));


            Console.WriteLine(report.ToString());
        }

        public static string TimeFromSeconds(double sec)
        {
            TimeSpan ts = TimeSpan.FromSeconds(sec);
            return String.Format("{0:hh\\:mm\\:ss}", ts);
        }

    }
}
