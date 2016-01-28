using System;
using System.Collections.Generic; 

namespace HRSim{
    public class ParetoTreePolicy {

    public double K;

    public ParetoTreePolicy(double a_kValue)
    {
        K = a_kValue;
    }

    public ParetoTreeNode bestChild(ParetoTreeNode node, double[][] bounds) {

        if (node.numExpandedChildren < node.children.Length) 
        {
            List<int> childrenToSelect = new List<int>();
            int j = 0;
            foreach (ParetoTreeNode child in node.children)
            {
                if (child.nVisits == 0)
                {
                    childrenToSelect.Add(j);
                }
                j++;
            }
            node.numExpandedChildren++;
            return node.children[childrenToSelect[node.m_rnd.Next(childrenToSelect.Count)]];
        }

        ParetoTreeNode selected = null;
        double bestValue = - Double.MaxValue;
        int i = 0;
        foreach (ParetoTreeNode child in node.children) {

            //If it is not prunned.
            if (!child.isExhausted)
            {
                double hvVal = child.getHV(false);
                double childValue =  hvVal;// / (child.nVisits + node.epsilon);

                if(hvVal < 0)
                    Console.WriteLine("Negative HyperVolume: " + hvVal);

                double uctValue = childValue +
                        K * Math.Sqrt(Math.Log(node.nVisits + 1) / (child.nVisits + node.epsilon)) +
                        node.m_rnd.NextDouble() * node.epsilon;

                // small random numbers: break ties in unexpanded nodes
                if (uctValue > bestValue) {
                    selected = child;
                    bestValue = uctValue;
                }
            }
            ++i;
        }
        if (selected == null)          {
            node.children[0].getHV(false);
            throw new Exception("Warning! returning null: " + bestValue + " : " + node.children.Length);
        }

        return selected;
    }

    //public SimpleTreeNode bestChild(SimpleTreeNode node, double[][] bounds) {
    //    return null;  //N/A
    //}

}

}