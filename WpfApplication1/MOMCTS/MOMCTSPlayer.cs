using System;

namespace HRSim
{
    public enum HeuristicType
    {
        Boardvalue,
        LethalCheck
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
        //HeuristicMO m_heuristic;
        //PlayoutInfo m_playoutInfo;

        /**
         * Debug height map
         */
        //public int[][] m_heightMap;
        public int m_numCalls;
        public int m_numIters;


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

            if (heuristicType == HeuristicType.Boardvalue)
            {
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
            }
            else //lethal check
            {
                bounds = new double[1][];
                bounds[0] = new double[2];
                bounds[0][0] = 0;
                bounds[0][1] = 1f;
            }
        }

        public void init()
        {
            //m_root = new ParetoTreeNode(null, m_randomRoller, m_treePolicy, m_rnd, this, m_playoutInfo);
            m_root = new ParetoTreeNode(null,  null, -1, m_randomRoller, m_treePolicy, m_rnd, this);
            //m_heightMap = new int[m_heightMap.length][m_heightMap[0].length];
        }

        public int run(Playfield a_gameState, long a_timeDue, bool a)
        {
            m_root.state = a_gameState;
            m_root.m_numIters = 0;

            m_root.mctsSearch(a_timeDue);

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

        //public HeuristicMO getHeuristic() { return m_heuristic; }

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