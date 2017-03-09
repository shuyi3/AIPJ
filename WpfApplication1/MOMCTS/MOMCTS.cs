using System;
using System.Collections.Generic;

namespace HRSim{

public class ParetoTreeNode {

    public ParetoMCTSPlayer m_player; //owner of this tree.
    public ParetoArchive pa;
    public SortedDictionary<int, List<Solution>> valueRoute;
    public static List<ParetoTreeNode> m_runList = new List<ParetoTreeNode>();
    public int childIndex;

    public double epsilon = 1e-6;
    //public static Random r = new Random();
    public Playfield state;
    public RandomRoller roller;
    public ParetoTreePolicy mTreePolicy;
    public ParetoTreeNode parent;
    public List<ParetoTreeNode> children;
    public double[] totValue;
    public int nVisits;
    public Random m_rnd;
    public bool[] m_prunedChildren;
    public int m_numIters;

    public bool isTerminal;
    public bool isExpanded;
    public bool isExhausted;
    public int numExpandedChildren;
    public int numExhaustedChildren;
    public int nodeNum;

    public ParetoTreeNode() { }

    public ParetoTreeNode(Playfield state, ParetoTreeNode parent, int childIndex, RandomRoller roller,
                      ParetoTreePolicy treePolicy, Random rnd, ParetoMCTSPlayer a_player)
    {
        this.m_player = a_player;
        this.state = state;
        this.parent = parent;
        this.m_rnd = rnd;
        //children = new ParetoTreeNode[ParetoMCTSParameters.NUM_ACTIONS];
        this.roller = roller;
        this.mTreePolicy = treePolicy;
        pa = new ParetoArchive();
        this.childIndex = childIndex;
        this.m_prunedChildren = new bool[ParetoMCTSParameters.NUM_ACTIONS];
        this.m_numIters = 0;
        //this.m_pi = pi;

        isTerminal = false;
        isExpanded = false;
        isExhausted = false;
        numExpandedChildren = 0;
        numExhaustedChildren = 0;
        nodeNum = GameManager.Instance.nodeCount;
        GameManager.Instance.nodeCount++;

        totValue = new double[ParetoMCTSParameters.NUM_TARGETS];
    }

    public void expand()
    {
        bool lethalCheck = false;
        //if (m_player.heuristicType == HeuristicType.LethalCheck)
            //lethalCheck = true;
        isExpanded = true;

        if (state.isOwnTurn != m_player.m_root.state.isOwnTurn)
            return;

        Movegenerator.Instance.getMoveListForPlayfield(state, false, lethalCheck);
        Playfield afterState = new Playfield(state);

        //afterState.printMoveList();
        //List<Action> testMoves = Movegenerator.Instance.getMoveList(afterState, lethalCheck, true, true);

        //if (afterState.moveList.Count != testMoves.Count)
        //{
        //    int debug = 1;
        //}


        //List<Action> testMoves = Movegenerator.Instance.getMoveList(afterState, lethalCheck, true, true);

        if (afterState.moveList.Count > 0)
        {
            children = new List<ParetoTreeNode>(afterState.moveList.Count + 1);

            int i = 0;
            ParetoTreeNode tn = null;

            //add endturn first
            Playfield nextState = new Playfield(afterState);
            //nextState.endTurn(false, false);
            //tn = new ParetoTreeNode(nextState, this, i, this.roller, this.mTreePolicy, this.m_rnd, this.m_player);
            //children.Add(tn);

            foreach (Action a in afterState.moveList)
            {
                //TODO: 暂时没有智慧祝福，收割机，大哥, 只有抽牌

                nextState = new Playfield(afterState);
                nextState.doAction(a);
                if (a.actionType == actionEnum.playcard && CardDB.Instance.UsefulNeedKeepDatabase.ContainsKey(a.card.card.name))
                {
                    Playfield keepState = new Playfield(afterState);
                    keepState.keepCardList.Add(a.card.entity);
                    keepState.moveTrigger.keepCard = a.card.entity;
                    tn = new ParetoTreeNode(keepState, this, i, this.roller, this.mTreePolicy, this.m_rnd, this.m_player);
                    children.Add(tn);
                    i++;
                }
                int cardDrawThisTurn = nextState.moveTrigger.newHandcardList.Count;
                tn = new ParetoTreeNode(nextState, this, i, this.roller, this.mTreePolicy, this.m_rnd, this.m_player);
                children.Add(tn);
                i++;
            }

            if (parent == null) //This is only for the root:
            {
                this.initValueRoute(i + 1);
            }
        }
        else
        {
            this.isTerminal = true;
            //this.isExhausted = true;
            //if (parent != null)
            //{
            //    parent.numExhaustedChildren++;
            //}
        }

    }

    public ParetoTreeNode bestChild() {
        return mTreePolicy.bestChild(this, m_player.getValueBounds());
    }

    public Playfield rollOut()
    {
            if (state.isOwnTurn != m_player.m_root.state.isOwnTurn)
                return state;

            bool lethalCheck = false;
            //if (m_player.heuristicType == HeuristicType.LethalCheck)
                //lethalCheck = true;

            Playfield startState = new Playfield(state);
            Action move = null;

            int score = startState.getGameResult();
            int depth = 0;
            while (score == -1)
            {
                if (depth != 0) Movegenerator.Instance.getMoveListForPlayfield(startState, false, lethalCheck);

                if (startState.moveList.Count == 0)
                {
                    //this.isTerminal = true;
                    break;
                }
                else
                {
                    move = startState.moveList[GameManager.getRNG().Next(startState.moveList.Count)];
                    startState.doAction(move);
                }
                score = startState.getGameResult();
                depth++;
            }

            startState.endTurn(false, false);
            return startState;
    }

    public Playfield chanceRollOut()
    {

        Playfield startState = new Playfield(state);
        Action move = null;

        int score = startState.getGameResult();
        int depth = 0;
        while (startState.moveTrigger.newHandcardList.Count == 0 && score == -1) //exit loop if it's chance move or end of game
        {
            if (depth != 0) Movegenerator.Instance.getMoveListForPlayfield(startState, false, false);

            if (startState.moveList.Count == 0)
            {
                break;
            }
            else
            {
                move = startState.moveList[GameManager.getRNG().Next(startState.moveList.Count)];
                startState.doAction(move);
            }
            score = startState.getGameResult();
            depth++;
        }

        if (startState.moveTrigger.newHandcardList.Count > 0)
        {
            int debug = 1;
        }

        return startState;
    }

    public void AddPlayoutInfoTree()
    {
        //int numNodes = m_runList.Count;
        //for(int i = 0; i < numNodes; ++i)
        //{
        //    ParetoTreeNode pn = m_runList[i];
        //    if(pn.childIndex > -1)
        //        m_player.getHeuristic().AddPlayoutInfo(pn.childIndex, pn.state);
        //}
    }

    public void printStats()
    {
        Helpfunctions.Instance.logg("************** Root archive **************");
        this.pa.printArchive();
        //if (children != null) for (int i = 0; i < children.Length; ++i)
        //{
        //    ParetoTreeNode pnCh = children[i];
        //    Helpfunctions.Instance.logg("*********** Child " + i + " **************");
        //    pnCh.pa.printArchive();
        //    Helpfunctions.Instance.logg("***************************************");
        //}
        Helpfunctions.Instance.logg("********************************************");

    }



    public int bestActionIndex(double[] targets) {
        bool verbose = false;
        int selected = -1;
        double[][] bounds = m_player.getValueBounds();
        double bestValue = -Double.MaxValue;
        OrderedSolutionList myPA = pa.m_members;
        int numMembers =  myPA.size();
        if(verbose && numMembers>1)
            Console.WriteLine("Choosing among " + myPA.size() + " members.");
        for(int i = 0; i < numMembers; ++i)
        {
            double[] thisRes = myPA.get(i).m_data;
            /*
            double val0 = Utils.normalise(thisRes[0], bounds[0][0], bounds[0][1]);
            double val1 = Utils.normalise(thisRes[1], bounds[1][0], bounds[1][1]);
            double val2 = Utils.normalise(thisRes[2], bounds[2][0], bounds[2][1]);
            double val = targets[0] * val0 + targets[1] * val1 + targets[2] * val2;*/

            double val = 0.0;
            for(int t = 0; t < targets.Length; ++t)
            {
                double v =  Utils.normalise(thisRes[t], bounds[t][0], bounds[t][1]);
                val += v*targets[t];
            }


            if(verbose && numMembers>1)
            {
                if(thisRes.Length==3)
                    Console.WriteLine("   [%.4f, %.4f, %.4f] => %.4f, from %d\n", thisRes[0], thisRes[1], thisRes[2], val, myPA.get(i).m_through);
                if(thisRes.Length==2)
                    Console.WriteLine("   [%.4f, %.4f] => %.4f, from %d\n", thisRes[0], thisRes[1], val, myPA.get(i).m_through);
            }
            //Console.WriteLine("Element in PA " + i + ": " + val);

            if(val > bestValue) {
                bestValue = val;
                selected = i;
            }

        }

        if(verbose && numMembers>1)
            Console.WriteLine("   Selected: " + selected);

        if(selected == -1)
        {
            //Console.WriteLine(" ********************* SELECTED -1, myPA.Count(): " + myPA.Count() + " ***************");
            return 0;
        }

        double[] selectedTarget = myPA.get(selected).m_data;
        //List<int> navSet = valueRoute.Keys.ToList();
        foreach(int key in valueRoute.Keys)
        {
            List<Solution> resFromThisChild = valueRoute[key];
            
            for(int i =0; i < resFromThisChild.Count; ++i)
            {
                double[] sol = resFromThisChild[i].m_data;
                //Console.WriteLine("PA point " + key + ":" + i + ": " + sol[0] + ", " + sol[1] + ", nVis: " + children[key].nVisits);

                if(sol.Length == 3 && sol[0] == selectedTarget[0] && sol[1] == selectedTarget[1] && sol[2] == selectedTarget[2])
                //if(sol[0] == selectedTarget[0] && sol[1] == selectedTarget[1])
                {
                    //Console.WriteLine("SELECTED-3: " + children[key].nVisits + "," + sol[0] + "," + sol[1] + ": " + key);
                    return key;
                }else if(sol.Length == 2 && sol[0] == selectedTarget[0] && sol[1] == selectedTarget[1])
                {
                    //Console.WriteLine("SELECTED-2: " + children[key].nVisits + "," + sol[0] + "," + sol[1] + ": " + key);
                    return key;
                }
            }
        }


        //If we get down here, we've done something wrong.
       /* pa.printArchive();

        Console.WriteLine("Looking for: " + selectedTarget[0] + "," + selectedTarget[1]);
        for(Integer key : navSet)
        {
            LinkedList<double[]> resFromThisChild = valueRoute.get(key);

            for(int i =0; i < resFromThisChild.Count(); ++i)
            {
                double[] sol = resFromThisChild.get(i);
                Console.WriteLine(key + ": " + sol[0] + "," + sol[1]);
                if(sol[0] == selectedTarget[0] && sol[1] == selectedTarget[1])
                    Console.WriteLine("FOUND!");
            }
        }
                  */
        //throw new Exception("Unexpected selection: " + selected);
        return selected;
    }

    public int bestActionIndex() {
        int selected = -1;
        double bestValue = -Double.MaxValue;
        for (int i=0; i<children.Count; i++) {

            if(!this.m_prunedChildren[i])
            {
                if (children[i] != null && children[i].nVisits + m_rnd.NextDouble() * epsilon > bestValue) {
                    bestValue = children[i].nVisits;
                    selected = i;
                }
            }
        }
        if (selected == -1) throw new Exception("Unexpected selection!");
        return selected;
    }

    public int bestActionIndexValue() {
        int selected = -1;
        double bestValue = -Double.MaxValue;
        for (int i=0; i<children.Count; i++) {

            if(!this.m_prunedChildren[i])
            {
                if (children[i] != null && children[i].totValue[0] + m_rnd.NextDouble() * epsilon > bestValue) {
                    bestValue = children[i].totValue[0];
                    selected = i;
                }
            }
        }
        if (selected == -1) throw new Exception("Unexpected selection!");
        return selected;
    }


    //public int bestActionIndexExpected() {
    //    int selected = -1;
    //    double bestValue = -Double.MaxValue;
    //    for (int i=0; i<children.Length; i++) {

    //        if(!this.m_prunedChildren[i])
    //        {
    //           // Console.WriteLine("Child " + i + ": " + sol[0] + ", " + sol[1] + ", nVis: " + children[i].nVisits);
    //            if (children[i] != null)
    //            {
    //                double val = children[i].totValue[0] / children[i].nVisits;
    //                if(val + m_rnd.NextDouble() * epsilon > bestValue){
    //                    bestValue = val;
    //                    selected = i;
    //                }
    //            }
    //        }
    //    }
    //    if (selected == -1) throw new Exception("Unexpected selection!");

    //    //double sol[] = children[selected].pa.m_members.get(0).m_data;
    //    //Console.WriteLine("SELECTED: " + (int)bestValue + "," + sol[0] + "," + sol[1] + ": " + selected);

    //    return selected;
    //}


    public double getHV(bool a_normalized)
    {
        if(a_normalized)
            return pa.computeHV(m_player.getValueBounds());
        else
            return pa.computeHV();

    }


    public int depth() {
        if (parent == null) return 0;
        else return 1 + parent.depth();
    }

    bool nonTerminal() {
        return children != null;
    }

    public bool notFullyExpanded() {
        int i = 0;
        bool allPruned = true;
        foreach (ParetoTreeNode tn in children) {
            allPruned &= m_prunedChildren[i];
            if (tn == null && !m_prunedChildren[i]) {
                return true;
            }
            ++i;
        }

        if(allPruned)
            return true;

        return false;
    }

    public bool allChildrenPruned()
    {
        for (int i = 0; i < m_prunedChildren.Length; ++i) {
            if (!m_prunedChildren[i]) {
                return false;
            }
        }
        return true;
    }

    public bool isLeaf() {
        return children == null;
    }

    public int arity() {
        return children == null ? 0 : children.Count;
    }

    //public void initValueRoute()
    //{
    //    this.valueRoute = new SortedDictionary<int, List<Solution>>();
    //    for(int i = 0; i < ParetoMCTSController.NUM_ACTIONS; ++i)
    //    {
    //        this.valueRoute.Add(i, new List<Solution>());
    //    }
    //}

    public void initValueRoute(int count)
    {
        this.valueRoute = new SortedDictionary<int, List<Solution>>();
        for (int i = 0; i < count; ++i)
        {
            this.valueRoute.Add(i, new List<Solution>());
        }
    }

}

}