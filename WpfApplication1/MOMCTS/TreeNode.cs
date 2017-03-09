using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSim
{
    class TreeNode
    {
        public ParetoMCTSPlayer m_ParetoMCTSPlayer;
        public ParetoArchive pa;
        public double[] totValue;
        public double epsilon = 1e-6;
        public static Random r = new Random();
        Playfield state;
        public int nVisits;
        public RandomRoller RandomRoller;
        public ParetoTreePolicy paretoTreePolicy;
        public TreeNode parent;
        public TreeNode[] children;

        public TreeNode(Playfield state, TreeNode parent, RandomRoller RandomRoller, ParetoTreePolicy ParetoTreePolicy, ParetoMCTSPlayer a_ParetoMCTSPlayer)
        {
            this.m_ParetoMCTSPlayer = a_ParetoMCTSPlayer;
            this.parent = parent;
            totValue = new double[ParetoMCTSParameters.NUM_TARGETS];
            this.RandomRoller = RandomRoller;
            this.paretoTreePolicy = ParetoTreePolicy;
            // System.out.println("Made a TreeNode of depth " + depth() + ", arity " + children.Length);
        }

        public void mctsSearch(int its)
        {
        }

        public void backUp(double[] result)
        {
        }

        public void backUp(double[] result, bool flag, int childIndex)
        {
        }

        public TreeNode ParetoTreePolicy()
        {
            return null;
        }

        public TreeNode bestChild()
        {
            return null;
        }

        public int bestActionIndex()
        {
            int selected = -1;
            return selected;
        }

        public int bestActionIndex(double target0, double target1)
        {
            throw new Exception("Not implemented in SingleTreeNode!");
        }

        public void expand()
        {
        }

        public double[] rollOut()
        {
            return null;
        }


        int depth()
        {
            if (parent == null) return 0;
            else return 1 + parent.depth();
        }

        bool nonTerminal()
        {
            return children != null;
        }

        public bool isLeaf()
        {
            return children == null;
        }

        public int arity()
        {
            return children == null ? 0 : children.Length;
        }
    }
}
