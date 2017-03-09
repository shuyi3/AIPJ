using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace HRSim
{
    public enum HeuristicType
    {
        Boardvalue,
        LethalCheck,
        DrawCard
    }

    public class ParetoMCTSPlayer
    {

        public HeuristicType heuristicType;
        ParetoTreePolicy m_treePolicy;
        public ParetoTreeNode m_root;
        Random m_rnd;
        double[] m_targetWeights;
        public ParetoArchive m_globalPA;
        RandomRoller m_randomRoller;
        public double[][] bounds;
        public int initNewCardCount;
        //HeuristicMO m_heuristic;
        //PlayoutInfo m_playoutInfo;

        /**
         * Debug height map
         */
        //public int[][] m_heightMap;
        public int m_numCalls;
        public int m_numIters;

        public List<ParetoTreeNode> m_runList;


        //public ParetoMCTSPlayer(ParetoTreePolicy a_treePolicy, HeuristicMO a_h, Random a_rnd, Game a_game, PlayoutInfo pInfo)
        //{
        //    m_playoutInfo = pInfo;
        //    //m_heightMap = new int[a_game.getMap().getMapWidth()][a_game.getMap().getMapHeight()];
        //    m_heuristic = a_h;
        //    m_heuristic.setPlayoutInfo(m_playoutInfo);
        //    m_treePolicy = a_treePolicy;
        //    this.m_rnd = a_rnd;
        //    this.m_targetWeights = ParetoMCTSParameters.targetWeights;
        //    m_globalPA = new ParetoArchive();
        //    m_randomRoller = new RandomRoller(RandomRoller.RANDOM_ROLLOUT, this.m_rnd, ParetoMCTSParameters.NUM_ACTIONS);
        //    m_root = new ParetoTreeNode(null, m_randomRoller, m_treePolicy, m_rnd, this, m_playoutInfo);
        //    this.m_numCalls = 0;
        //    this.m_numIters = 0;
        //}

        public ParetoMCTSPlayer(ParetoTreePolicy a_treePolicy, Random a_rnd, Playfield a_game, HeuristicType _ht)
        {
            //m_playoutInfo = pInfo;
            ////m_heightMap = new int[a_game.getMap().getMapWidth()][a_game.getMap().getMapHeight()];
            //m_heuristic = a_h;
            //m_heuristic.setPlayoutInfo(m_playoutInfo);
            m_treePolicy = a_treePolicy;
            this.m_rnd = a_rnd;
            this.m_targetWeights = ParetoMCTSParameters.targetWeights;
            m_globalPA = new ParetoArchive();
            m_randomRoller = new RandomRoller(RandomRoller.RANDOM_ROLLOUT, this.m_rnd, ParetoMCTSParameters.NUM_ACTIONS);
            //m_root = new ParetoTreeNode(null, m_randomRoller, m_treePolicy, m_rnd, this, m_playoutInfo);
            m_root = new ParetoTreeNode(a_game, null, -1, m_randomRoller, m_treePolicy, m_rnd, this);
            this.m_numCalls = 0;
            this.m_numIters = 0;

            m_runList = new List<ParetoTreeNode>();
            heuristicType = _ht;
          
            ////bounds[0][0] = -Math.Sqrt(8) * 8 * 8;
            //bounds[0][0] = 0;
            //bounds[0][1] = Math.Sqrt(8) * 8 * 8 * 2;
            ////bounds[1][0] = -Math.Sqrt(8) * 8 * 8;
            //bounds[1][0] = 0;
            //bounds[1][1] = Math.Sqrt(8) * 8 * 8;
            ////bounds[2][0] = -10 * 10;
            ////bounds[2][1] = 10 * 10;
            ////bounds[2][0] = -Math.Sqrt(8) * 8 * 10;
            //bounds[2][0] = 0;
            //bounds[2][1] = Math.Sqrt(8) * 8 * 10 * 2;

            switch (this.heuristicType)
            {
                case HeuristicType.Boardvalue:
                    bounds = new double[3][];
                    bounds[0] = new double[2];
                    bounds[1] = new double[2];
                    bounds[2] = new double[2];
                    //bounds[0][0] = -Math.Sqrt(8) * 8 * 8;
                    bounds[0][0] = 0;
                    bounds[0][1] = 1f;
                    //bounds[1][0] = -Math.Sqrt(8) * 8 * 8;
                    bounds[1][0] = 0;
                    bounds[1][1] = 1f;
                    //bounds[2][0] = -10 * 10;
                    //bounds[2][1] = 10 * 10;
                    //bounds[2][0] = -Math.Sqrt(8) * 8 * 10;
                    bounds[2][0] = 0;
                    bounds[2][1] = 1f;
                    break;

                case HeuristicType.LethalCheck:
                    bounds = new double[1][];
                    bounds[0] = new double[2];
                    bounds[0][0] = 0;
                    bounds[0][1] = 1f;
                    break;

                case HeuristicType.DrawCard:
                    bounds = new double[2][];
                    bounds[0] = new double[2];
                    bounds[1] = new double[2];
                    bounds[0][0] = 0;
                    bounds[0][1] = 1f;
                    bounds[1][0] = 0;
                    bounds[1][1] = 1f;
                    initNewCardCount = m_root.state.moveTrigger.newHandcardList.Count;
                    break;
            }       
        }

        public void init()
        {
            //m_root = new ParetoTreeNode(null, m_randomRoller, m_treePolicy, m_rnd, this, m_playoutInfo);
            m_root = new ParetoTreeNode(null,  null, -1, m_randomRoller, m_treePolicy, m_rnd, this);
            //m_heightMap = new int[m_heightMap.length][m_heightMap[0].length];
        }

        public int run(Playfield a_gameState, int a_timeDue, bool a)
        {
            m_root.state = a_gameState;
            m_root.m_numIters = 0;

            //m_root.mctsSearch(a_timeDue);

            mctsSearch(a_timeDue, m_root);

            int nextAction = 0;
            if (a)
            {
                if (m_targetWeights.Length == 1)
                    nextAction = m_root.bestActionIndexValue();
                else
                {
                    /*System.out.print("(");
                    for(int i = 0; i < m_root.children.length; ++i)
                    {
                        if(!m_root.m_prunedChildren[i])
                        {
                            System.out.print(i + ":" + m_root.children[i].nVisits + ",");
                        }
                    }
                    System.out.println(")");        */
                    //double[] targetWeights = m_heuristic.getTargetWeights();
                    double[] targetWeights = ParetoMCTSParameters.targetWeights;
                    //System.out.println(targetWeights[0] + "," + targetWeights[1] + "," + targetWeights[2]);
                    nextAction = m_root.bestActionIndex(targetWeights);
                }

            }

            this.m_numCalls++;
            this.m_numIters += m_root.m_numIters;
            return nextAction;
        }


        public void mctsSearch(int a_timeDue, ParetoTreeNode root)
        { //TODO: prune bad boards: 1. we will die next turn, 2. we will die this turn

            //long remaining = a_timeDue - DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            int numIters = a_timeDue;
            double invIters = 0.0;

            for (int i = 0; i < numIters; i++)
            {            
                m_runList.Clear();
                m_runList.Add(root); //root always in.

                if (root.isExhausted)
                {
                    //Helpfunctions.Instance.logg("exit at num: " + i);
                    break; //if all exhausted, then return
                }
                ParetoTreeNode selected = treePolicy(root);
                //double[] delta = selected.rollOut();
                Playfield endTurnState = rollOut(selected);
                //double[] delta = m_player.getHeuristic().value(endTurnState);
                if (endTurnState.isOwnTurn == m_root.state.isOwnTurn)
                {
                    int debug = 1;
                }
                //Debug.Assert(endTurnState.isOwnTurn != m_root.state.isOwnTurn);
                double[] delta = getSolutionVector(endTurnState);     
                Solution deltaSol = new Solution(delta, endTurnState);
                backUp(delta, deltaSol, true, selected.childIndex);

                m_numIters++;
                //remaining = a_timeDue - DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

                //if(treePolicy is ParetoEGreedyTreePolicy)
                //{
                //    ((ParetoEGreedyTreePolicy) treePolicy).epsilon -= invIters;
                //}
                if (this.heuristicType == HeuristicType.LethalCheck && (deltaSol.m_data[0] == 1.0 || deltaSol.m_data[0] == 0)) //we got lethal
                {
                    return;
                }
            }

            //if (this.heuristicType == HeuristicType.DrawCard) //we got lethal
            //{
            //    Helpfunctions.Instance.logg("drawcard size = " + root.pa.m_members.size());
            //    int debug = 1;
            //}
            //Helpfunctions.Instance.logg("normal exit: 500");
        }

        public Playfield rollOut(ParetoTreeNode tn)
        {
            switch (this.heuristicType)
            {
                case HeuristicType.Boardvalue:
                    return tn.rollOut();

                case HeuristicType.LethalCheck:
                    return tn.rollOut();

                case HeuristicType.DrawCard:
                    return tn.chanceRollOut();
            }
            return null;
        }

        public double[] getSolutionVector(Playfield state)
        {
            double[] delta = null;
            switch (this.heuristicType)
            {
                case HeuristicType.Boardvalue:
                    //delta = new double[] { state.getBoardValue(), -state.getEnemyBoardValue(), state.getHandValue() };
                    delta = new double[] { state.getBoardValue(true), state.getHandValue(true), state.getHeroValue()};
                    break;

                case HeuristicType.LethalCheck:
                    delta = new double[] { state.getLethalScore() };
                    break;

                case HeuristicType.DrawCard:
                    delta = state.getDrawCardScore();
                    break;
            }
            return delta;
        }

        public ParetoTreeNode treePolicy(ParetoTreeNode root)
        {

            ParetoTreeNode cur = root;
            int depth = 0;

            while (keepTreePolicy(cur, depth))
            {                

                //ParetoTreeChanceNode chanceCur = cur as ParetoTreeChanceNode;
                //if (chanceCur != null) {
                //    cur = chanceCur.doChanceAction(); //roll a dice
                //}
                //else
                //{
                cur = cur.bestChild();
                depth++;
                //}
                m_runList.Insert(0, cur);
            }

            if (isExpandable(cur, depth))
                cur.expand();

            return cur;
        }

        public bool isExpandable(ParetoTreeNode pn, int depth)
        {
            switch (this.heuristicType)
            {
                case HeuristicType.Boardvalue:
                    return !pn.isExpanded && pn.state.getGameResult() == -1;

                case HeuristicType.LethalCheck:
                    return !pn.isExpanded && pn.state.getGameResult() == -1;

                case HeuristicType.DrawCard:
                    return !pn.isExpanded && (depth == 0 || (depth > 0 && pn.state.moveTrigger.newHandcardList.Count == 0));
                    //return !pn.isExpanded && pn.state.getGameResult() == -1 && pn.state.moveTrigger.newHandcardList.Count == 0;
            }
            return true;
        }

        public bool keepTreePolicy(ParetoTreeNode node, int depth)
        {
            switch (this.heuristicType)
            {
                case HeuristicType.Boardvalue:
                    return !node.isLeaf() && !node.isExhausted && node.state.getGameResult() == -1 && depth < ParetoMCTSParameters.ROLLOUT_DEPTH;

                case HeuristicType.LethalCheck:
                    return !node.isLeaf() && !node.isExhausted && node.state.getGameResult() == -1 && depth < ParetoMCTSParameters.ROLLOUT_DEPTH;

                case HeuristicType.DrawCard:
                    return !node.isLeaf() && !node.isExhausted && node.state.getGameResult() == -1 &&
                        depth < ParetoMCTSParameters.ROLLOUT_DEPTH && (node.state.moveTrigger.newHandcardList.Count != 0 && depth > 0);                   
            }
            return false;
        }

        public void backUp(double[] result, Solution sol, bool Added, int cI)
        {

            /*nVisits++;
            Added = pa.Add(result);
            int comingFrom = cI;

            for(int i = 0; i < result.Length; ++i)
                totValue[i] += result[i];      */

            //for(ParetoTreeNode pn : m_runList)
            int comingFrom = -1;
            int numNodes = m_runList.Count;
            for (int i = 0; i < numNodes; ++i)
            {
                ParetoTreeNode pn = m_runList[i];
                //Helpfunctions.Instance.logg("node: " + pn.parent.nodeNum + ", children exhasuted: " + pn.parent.numExhaustedChildren + "/" + pn.parent.children.Length);

                if (pn.isTerminal || (pn.children != null && pn.numExhaustedChildren == pn.children.Count))
                {
                    pn.isExhausted = true;
                    if (pn.parent != null)
                    {
                        pn.parent.numExhaustedChildren++;
                    }
                }

                pn.nVisits++;

                if (Added)
                {
                    Added = pn.pa.add(sol);
                    //ParetoTreeChanceNode paretoChanceNode = pn.parent as ParetoTreeChanceNode;
                    //if (paretoChanceNode != null)
                    //{
                    //    paretoChanceNode.HVvalue += pn.getHV(false);
                    //}
                }

                for (int j = 0; j < result.Length; ++j)
                    pn.totValue[j] += result[j];

                if (i + 1 < numNodes)
                {
                    //ParetoTreeNode parentNode = m_runList.get(i+1);
                    //parentNode.m_childCount[pn.childIndex]++; //for Nsa in one of the tree policies (see TransParetoTreePolicy).
                    comingFrom = pn.childIndex;
                }
                else if (i + 1 == numNodes)
                {
                    if (pn.parent != null)
                        throw new Exception("This should be the root... and it's not.");

                    if (Added)
                    {
                        //Console.WriteLine("AddING (" + result[0] + "," + result[1] + ") to child " + comingFrom + " from " + pn.parent);
                        if (comingFrom != -1)
                        {
                            sol.m_through = comingFrom;
                            pn.valueRoute[comingFrom].Add(sol);
                        }
                    }

                }

            }
        }



        public double getHV(bool a_normalized)
        {
            return m_root.getHV(a_normalized);
        }

        public void reset() { }

        public double[][] getValueBounds()
        {
            return bounds;
        }
    }

}