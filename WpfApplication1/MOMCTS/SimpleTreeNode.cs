using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSim
{
public class SimpleTreeNode {

    public ParetoMCTSPlayer m_ParetoMCTSPlayer;
    public ParetoArchive pa;
    public double[] totValue;
    public double epsilon = 1e-6;
    public static Random r = new Random();
    Playfield state;
    public int nVisits;
    public RandomRoller RandomRoller;
    public ParetoTreePolicy paretoTreePolicy;
    public SimpleTreeNode parent;
    public SimpleTreeNode[] children;


    public SimpleTreeNode(Playfield state, RandomRoller RandomRoller, ParetoTreePolicy ParetoTreePolicy, ParetoMCTSPlayer a_ParetoMCTSPlayer)
    {
        this(state, null, RandomRoller, ParetoTreePolicy, a_ParetoMCTSPlayer);
        this.RandomRoller = RandomRoller;
        this.paretoTreePolicy = ParetoTreePolicy;
        pa = new ParetoArchive();
    }

    public SimpleTreeNode(Playfield state, SimpleTreeNode parent, RandomRoller RandomRoller, ParetoTreePolicy ParetoTreePolicy, ParetoMCTSPlayer a_ParetoMCTSPlayer)
    {
        this.m_ParetoMCTSPlayer = a_ParetoMCTSPlayer;
        this.parent = parent;
        children = new SimpleTreeNode[ParetoMCTSController.NUM_ACTIONS];
        totValue = new double[ParetoMCTSParameters.NUM_TARGETS];
        this.RandomRoller = RandomRoller;
        this.paretoTreePolicy = ParetoTreePolicy;
        // System.out.println("Made a TreeNode of depth " + depth() + ", arity " + children.Length);
    }

    public void mctsSearch(int its) {
        for (int i = 0; i < its; i++) {
            SimpleTreeNode selected = ParetoTreePolicy();
            double[] delta = selected.rollOut();
            pa.add(new Solution(delta));  //Add the result of the new tree walk to the pareto front (it checks for dominance)
            selected.backUp(delta);
        }
    }

    public void backUp(double[] result) {
        nVisits++;
        for(int i = 0; i < result.Length; ++i)
            totValue[i] += result[i];

        if (parent != null) parent.backUp(result);
    }

    public void backUp(double[] result, bool flag, int childIndex) {
        //Nothing to do
    }

    public SimpleTreeNode ParetoTreePolicy() {


        SimpleTreeNode cur = this;
        while (cur.nonTerminal() && !cur.state.isEnded()) {

            if (cur.notFullyExpanded()) {
                return cur.expand();
            } else {
                cur = cur.bestChild();
            }
            // System.out.println("cur = " + cur);
        }
        return cur;
    }

    public SimpleTreeNode bestChild() {
        return paretoTreePolicy.bestChild(this, m_ParetoMCTSPlayer.getHeuristic().getValueBounds());
    }

    public int bestActionIndex() {
        int selected = -1;
        double bestValue = Double.MinValue;
        for (int i=0; i<children.Length; i++) {
            if (children[i] != null && children[i].nVisits + r.NextDouble() * epsilon > bestValue) {
                bestValue = children[i].nVisits;
                selected = i;
            }
        }
        if (selected == -1) throw new Exception("Unexpected selection!");
        return selected;
    }

    public int bestActionIndex(double target0, double target1) {
        throw new Exception("Not implemented in SingleTreeNode!");
    }

    public SimpleTreeNode expand() {
        // choose a random unused action and add a new node for that
        int bestAction = -1;
        double bestValue = -1;
        for (int i = 0; i < children.Length; i++) {
            double x = r.NextDouble();
            if (x > bestValue && children[i] == null) {
                bestAction = i;
                bestValue = x;
            }
        }
        Playfield nextState = new Playfield(state);
        nextState.tick(bestAction);
        SimpleTreeNode tn = new SimpleTreeNode(nextState, this, this.RandomRoller, this.paretoTreePolicy, this.m_ParetoMCTSPlayer);
        children[bestAction] = tn;
        return tn;
    }

    public double[] rollOut()
    {
        Playfield RandomRollerState = new Playfield(state);
        int action = 0;
        while (!RandomRollerState.isEnded() && action != -1) {
            action = RandomRoller.roll(RandomRollerState);
            if(action >= 0)
            {
                RandomRollerState.tick(action);
            }
        }
        return m_ParetoMCTSPlayer.getHeuristic().value(RandomRollerState);
    }


    int depth() {
        if (parent == null) return 0;
        else return 1 + parent.depth();
    }

    bool nonTerminal() {
        return children != null;
    }

    public bool notFullyExpanded() {
        foreach (SimpleTreeNode tn in children) {
            if (tn == null) {
                return true;
            }
        }
        return false;
    }

    public bool isLeaf() {
        return children == null;
    }

    public int arity() {
        return children == null ? 0 : children.Length;
    }


}

}
