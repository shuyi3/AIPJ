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
    public ParetoTreeNode[] children;
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
    //public PlayoutInfo m_pi;

    //public ParetoTreeNode()
    //{
    //    this(null, null, -1, null, null, null, null,null);
    //}

    //public ParetoTreeNode(Game state, Roller roller, ParetoTreePolicy treePolicy, Random rnd,
    //                      Player a_player, PlayoutInfo pi) {
    //    this(state, null, -1, roller, treePolicy, rnd, a_player,pi);
    //}

    //public ParetoTreeNode(Playfield state, RandomRoller roller, ParetoTreePolicy treePolicy, Random rnd,
    //                  ParetoMCTSPlayer a_player)
    //{
    //    this(state, null, -1, roller, treePolicy, rnd, a_player);
    //}

    //public ParetoTreeNode(Game state, ParetoTreeNode parent, int childIndex, Roller roller,
    //                      TreePolicy treePolicy, Random rnd, Player a_player, PlayoutInfo pi) {
    //    this.m_player = a_player;
    //    this.state = state;
    //    this.parent = parent;
    //    this.m_rnd = rnd;
    //    children = new ParetoTreeNode[ParetoMCTSParameters.NUM_ACTIONS];
    //    totValue = new double[ParetoMCTSParameters.NUM_TARGETS];
    //    this.roller = roller;
    //    this.treePolicy = treePolicy;
    //    pa = new ParetoArchive();
    //    this.childIndex = childIndex;
    //    this.m_prunedChildren = new bool[ParetoMCTSParameters.NUM_ACTIONS];
    //    this.m_numIters = 0;
    //    this.m_pi = pi;
        
    //    if(parent == null) //This is only for the root:
    //    {
    //        this.initValueRoute();
    //    }
    //}

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
        //if (parent == null) //This is only for the root:
        //{
        //    this.initValueRoute();
        //}
    }

    public static int NEXT_TICKS;
    public void mctsSearch(long a_timeDue) { //TODO: prune bad boards: 1. we will die next turn, 2. we will die this turn

        long remaining = a_timeDue - DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        NEXT_TICKS=0;
        int numIters = 500;
        double invIters = 0.0;

        //if(treePolicy is ParetoEGreedyTreePolicy)
        //{
        //    ((ParetoEGreedyTreePolicy) treePolicy).epsilon = 0.1;
        //    invIters = 0.1/numIters;
        //}

        for (int i = 0; i < numIters; i++) {
        //while(remaining > 10)   {
        //while(remaining > 0)   {
            m_runList.Clear();
            m_runList.Add(this); //root always in.

            //m_pi.reset(this.state);
            if (this.isExhausted)
            {
                //Helpfunctions.Instance.logg("exit at num: " + i);
                break; //if all exhausted, then return
            }
            ParetoTreeNode selected = treePolicy();
            AddPlayoutInfoTree();
            //double[] delta = selected.rollOut();
            Playfield endTurnState = selected.rollOut();
            //double[] delta = m_player.getHeuristic().value(endTurnState);
            double[] delta = null;
            switch (m_player.heuristicType)
            { 
                case HeuristicType.Boardvalue:
                    delta = new double[] { endTurnState.getBoardValue(), -endTurnState.getEnemyBoardValue(), endTurnState.getHandMinionValue() };
                    break;

                case HeuristicType.LethalCheck:
                    delta = new double[] { endTurnState.getLethalScore()};
                    break;
            
            }
            //double[] delta = new double[] {endTurnState.getBoardValue(), -endTurnState.getEnemyBoardValue(), endTurnState.getHandMinionValue()};
            //endTurnState.printBoard();         
            Solution deltaSol = new Solution(delta, endTurnState);
            selected.backUp(delta, deltaSol, true, selected.childIndex);

            m_numIters++;
            remaining = a_timeDue - DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            //if(treePolicy is ParetoEGreedyTreePolicy)
            //{
            //    ((ParetoEGreedyTreePolicy) treePolicy).epsilon -= invIters;
            //}
            if (m_player.heuristicType == HeuristicType.LethalCheck && (deltaSol.m_data[0] == 1.0 || deltaSol.m_data[0] == 0)) //we got lethal
            {
                int debug = 1;
                return;
            }
        }
        //Helpfunctions.Instance.logg("normal exit: 500");
    }

    public ParetoTreeNode treePolicy() {

        ParetoTreeNode cur = this;
        int depth = 0;

        while (!cur.isLeaf() && cur.state.getGameResult() == -1 && depth < ParetoMCTSParameters.ROLLOUT_DEPTH)
        {
            //if (!cur.isTerminal)
            //{
                cur = cur.bestChild();
                //Helpfunctions.Instance.logg("selected node: " + cur.nodeNum);
                depth++;
                m_runList.Insert(0, cur);
            //}
            //else
            //{
            //    m_runList.Insert(0, cur);
            //    break;
            //}
        }

        cur.expand();

        return cur;


        //    if (!cur.isExpanded || cur.notFullyExpanded())
        //    {
        //        ParetoTreeNode tn = cur.expand();
        //        if (tn != null) //Can happen: if all remaining nodes to be expanded must be pruned.
        //        {
        //            m_runList.Insert(0, tn);
        //            return tn;
        //        }

        //        if (cur.allChildrenPruned())
        //        {
        //            if (cur.parent != null)
        //            {
        //                cur.parent.m_prunedChildren[cur.childIndex] = true;
        //            }
        //            m_runList.RemoveAt(0);
        //            cur = cur.parent;

        //            depth--;

        //        }
        //        else
        //        {
        //            //Really, do nothing, next iteration we use UCB1
        //        }


        //    }
        //    else
        //    {
        //        cur = cur.bestChild();
        //        depth++;
        //        m_runList.Insert(0, cur);
        //    }
        //}

        ////try{
        //while (!isTerminal && cur.state.getGameResult() == -1 && depth < ParetoMCTSParameters.ROLLOUT_DEPTH)
        //{
        //    if (!cur.isExpanded || cur.notFullyExpanded())
        //    {
        //        ParetoTreeNode tn = cur.expand();
        //        if(tn != null) //Can happen: if all remaining nodes to be expanded must be pruned.
        //        {
        //            m_runList.Insert(0,tn);
        //            return tn;
        //        }

        //        if(cur.allChildrenPruned())
        //        {
        //            if(cur.parent != null)
        //            {
        //                cur.parent.m_prunedChildren[cur.childIndex] = true;
        //            }
        //            m_runList.RemoveAt(0);
        //            cur = cur.parent;

        //            depth--;

        //        }else
        //        {
        //            //Really, do nothing, next iteration we use UCB1
        //        }


        //    } else {
        //        cur = cur.bestChild();
        //        depth++;
        //        m_runList.Insert(0,cur);
        //    }
        //}
        //}catch(Exception e)
        //{
        //    Console.WriteLine(e);
        //}

        //return cur;
    }

    public void expand()
    {
        bool lethalCheck = false;
        //if (m_player.heuristicType == HeuristicType.LethalCheck)
            //lethalCheck = true;

        Playfield afterState = new Playfield(state);


        Movegenerator.Instance.getMoveListForPlayfield(afterState, false, lethalCheck);

        //afterState.printMoveList();
        //List<Action> testMoves = Movegenerator.Instance.getMoveList(afterState, lethalCheck, true, true);

        //if (afterState.moveList.Count != testMoves.Count)
        //{
        //    int debug = 1;
        //}

        isExpanded = true;

        //List<Action> testMoves = Movegenerator.Instance.getMoveList(afterState, lethalCheck, true, true);

        if (afterState.moveList.Count > 0)
        {
            children = new ParetoTreeNode[afterState.moveList.Count];

            int i = 0;

            foreach (Action a in afterState.moveList)
            {
                Playfield nextState = new Playfield(afterState);
                nextState.doAction(a);

                ParetoTreeNode tn = new ParetoTreeNode(nextState, this, i, this.roller, this.mTreePolicy, this.m_rnd, this.m_player);
                children[i] = tn;
                i++;
            }

            //Helpfunctions.Instance.logg("node expanded: " + this.nodeNum + ", children size: " + this.children.Length);
            //totValue = new double[afterState.moveList.Count];
            if (parent == null) //This is only for the root:
            {
                this.initValueRoute(afterState.moveList.Count);
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



    //public ParetoTreeNode expand() {
    //    // choose a random unused action and Add a new node for that


    //    ParetoTreeNode tn = null;

    //    //implementataion
    //        Playfield afterState = new Playfield(state);
    //        //Helpfunctions.Instance.logg("debug: size == " + afterState.moveList.Count);

    //        if (!isExpanded)
    //        {
    //            isExpanded = true;
    //            Movegenerator.Instance.getMoveListForPlayfield(afterState, false);
    //            if (afterState.moveList.Count > 0)
    //            {
    //                children = new ParetoTreeNode[afterState.moveList.Count];

    //                //totValue = new double[afterState.moveList.Count];
    //                if (parent == null) //This is only for the root:
    //                {
    //                    this.initValueRoute(afterState.moveList.Count);
    //                }
    //            }
    //        }
    //        else
    //        {
    //            Helpfunctions.Instance.logg("node: " + nodeNum + ", children exhasuted: " + numExhaustedChildren + "/" + children.Length);
    //            if (numExhaustedChildren >= m_prunedChildren.Length)
    //            {
    //                Helpfunctions.Instance.logg("node " + nodeNum + "is pruned");
    //                this.isExhausted = true;
    //                parent.numExhaustedChildren++; 
    //                return null;
    //            }
    //        }
    //        //else
    //        //{
    //        //    Movegenerator.Instance.getMoveListForPlayfield(afterState, false);
    //        //}


    //        if (afterState.moveList.Count == 0)
    //        { // no moves available
    //            //afterState.endTurn(false, false);
    //            //children[index] = new ParetoTreeNode(afterState, this, index, this.roller, this.mTreePolicy,
    //            //        this.m_rnd, this.m_player);
    //            this.isTerminal = true;
    //            this.isExhausted = true;
    //            return null;
    //        }

    //        //foreach (Action move in afterState.moveList)
    //        //{
    //        //    index++;
    //        //    Node afterNode = new Node(new Playfield(afterState), move, p.depth + 1);
    //        //    Playfield nextState = new Playfield(afterState);
    //        //    nextState.doAction(move);
    //        //    children[index] = new ParetoTreeNode(afterState, this, index, this.roller, this.mTreePolicy,
    //        //           this.m_rnd, this.m_player); 
    //        //}

    //        //int bestAction = m_rnd.Next(children.Length);

    //    //end of implementation

    //        while (tn == null)
    //        {
    //            List<int> validChildrenIndex = new List<int>();
    //            for (int i = 0; i < children.Length; i++)
    //            {
    //                if (!children[i].isExhausted)
    //                {
    //                    validChildrenIndex.Add(i);
    //                }
    //            }

    //            int bestAction = validChildrenIndex[m_rnd.Next(validChildrenIndex.Count)];
    //            Playfield nextState = new Playfield(afterState);
    //            nextState.doAction(afterState.moveList[bestAction]);

    //            tn = new ParetoTreeNode(nextState, this, bestAction, this.roller, this.mTreePolicy, this.m_rnd, this.m_player);
    //            children[bestAction] = tn;
    //            return tn;

    //        }
        
    //    //while(tn == null && prunedN < m_prunedChildren.Length)
    //    //{
    //    //    int bestAction = -1;
    //    //    double bestValue = -1;
    //    //    for (int i = 0; i < children.Length; i++) {
    //    //        double x = m_rnd.NextDouble();
    //    //        //if (x > bestValue && children[i] == null && !m_prunedChildren[i]) {
    //    //        if (x > bestValue && children[i] == null && !children[i].isExhausted) {
    //    //            bestAction = i;
    //    //            bestValue = x;
    //    //        }
    //    //    }

    //    //    if(bestValue==-1)
    //    //    {
    //    //        //No options (because of pruning!)
    //    //        return null;
    //    //    }

    //    //    Playfield nextState = new Playfield(afterState);
    //    //    nextState.doAction(afterState.moveList[bestAction]);
    //    //    //nextState.next(bestAction);
    //    //    //advance(nextState, bestAction);

    //    //    //if(m_player.getHeuristic().mustBePruned(nextState, state))
    //    //    if (false)
    //    //    {
    //    //        m_prunedChildren[bestAction] = true;
    //    //        prunedN++;
    //    //    }else{
    //    //        //tn = new ParetoTreeNode(nextState, this, bestAction, this.roller, this.mTreePolicy,
    //    //        //                        this.m_rnd, this.m_player, this.m_pi);
    //    //        tn = new ParetoTreeNode(nextState, this, bestAction, this.roller, this.mTreePolicy,
    //    //                this.m_rnd, this.m_player);
    //    //        children[bestAction] = tn;
    //    //        return tn;
    //    //    }

    //    //}

    //    //if(tn == null)
    //    //{
    //    //    //All children go pruned... prune myself?
    //    //    this.isExhausted = true;
    //    //    parent.numExhaustedChildren++;
    //    //    //if(parent.m_prunedChildren == null)
    //    //    //    Console.WriteLine("parent.m_prunedChildren is Null");
    //    //    //parent.m_prunedChildren[childIndex] = true;
    //    //}

    //    return tn;
    //}

    public ParetoTreeNode bestChild() {
        return mTreePolicy.bestChild(this, m_player.getValueBounds());
    }

    public Playfield rollOut()
    {
        //Playfield rollerState = new Playfield(state);
        //int thisDepth = this.depth();
        //int action = 0;
        // while (!rollerState.isTerminal() && action != -1) {


            bool lethalCheck = false;
            //if (m_player.heuristicType == HeuristicType.LethalCheck)
                //lethalCheck = true;

            Playfield startState = new Playfield(state);
            Action move = null;
            int turn = 0;

            int maxMana = startState.playerFirst.ownMaxMana;

            int score = startState.getGameResult();
            while (score == -1)
            {
                //List<Action> moves = Movegenerator.Instance.getMoveList(startState, false, false, true);
                //if (move != null)
                //{
                //GameManager.Instance.moveCount++;
                //if (GameManager.Instance.moveCount == 562)
                //{
                //    int debug = 1;
                //}
                    //var milliseconds = (DateTime.Now - DateTime.MinValue).TotalMilliseconds;
                Movegenerator.Instance.getMoveListForPlayfield(startState, false, lethalCheck);

                //List<Action> testMoves = Movegenerator.Instance.getMoveList(startState, lethalCheck, true, true);

                //if (startState.moveList.Count != testMoves.Count)
                //{
                //    int debug = 1;
                //}
                    //double time = (DateTime.Now - DateTime.MinValue).TotalMilliseconds - milliseconds;
                    //GameManager.Instance.myTimer += time;
                    //Helpfunctions.Instance.logg("my:" + time + " total:" + GameManager.Instance.myTimer);


                    //milliseconds = (DateTime.Now - DateTime.MinValue).TotalMilliseconds;
                    //List<Action> bruteForceMoves = Movegenerator.Instance.getMoveList(startState, false, true, true);
                    //time = (DateTime.Now - DateTime.MinValue).TotalMilliseconds - milliseconds;
                    //GameManager.Instance.sfTimer += time;
                    //Helpfunctions.Instance.logg("sf:" + time + " total:" + GameManager.Instance.sfTimer);

                    //if (bruteForceMoves.Count != startState.moveList.Count) {
                    //    startState.printBoard();
                    //    int debug = 1;
                    //    Helpfunctions.Instance.logg("BF Move List:------------------------------------");
                    //    foreach (Action action in bruteForceMoves)
                    //    {
                    //        action.print();
                    //    }
                    //    startState.printMoveList();
                    //}
                //}
                //Helpfunctions.Instance.logg("Count: " + startState.moveList.Count);
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
            }


        //TODO: last turn rollout
        //while (!finishRollout(rollerState,thisDepth,action)) {
        //    action = roller.roll(rollerState);
        //    //rollerState.next(action);
        //    advance(rollerState, action);
        //    m_player.getHeuristic().AddPlayoutInfo(action, rollerState);
        //    thisDepth++;
        //}
            int endMaxMana = startState.playerFirst.ownMaxMana;

            if (endMaxMana != maxMana)
            {
                int debug = 1;
            }
            startState.endTurn(false, false);
            return startState;
            //return m_player.getHeuristic().value(startState);
    }

    //public void advance(Playfield st, int action)
    //{
    //    bool gameOver = false;
    //    for(int singleAction = 0; !gameOver && singleAction < ParetoMCTSParameters.MACRO_ACTION_LENGTH; ++singleAction)
    //    {
    //        //((ParetoMCTSPlayer)m_player).m_heightMap[(int)st.getShip().s.x][(int)st.getShip().s.y]++;
    //        st.tick(action);
    //        NEXT_TICKS++;
    //        gameOver = st.isEnded();
    //    }
    //}


    //public bool finishRollout(Game rollerState, int depth, int action)
    //{
    //    if(depth >= ParetoMCTSParameters.ROLLOUT_DEPTH)      //rollout end condition.
    //        return true;

    //    if(action == -1)                           //end
    //        return true;

    //    if(rollerState.isEnded())               //end of game
    //    {
    //        //Console.WriteLine("End Reached!");
    //        return true;
    //    }

    //    return false;
    //}

    public void backUp(double[] result, Solution sol, bool Added, int cI) {

        /*nVisits++;
        Added = pa.Add(result);
        int comingFrom = cI;

        for(int i = 0; i < result.Length; ++i)
            totValue[i] += result[i];      */

        //for(ParetoTreeNode pn : m_runList)
        int comingFrom = -1;
        int numNodes = m_runList.Count;
        for(int i = 0; i < numNodes; ++i)
        {
            ParetoTreeNode pn = m_runList[i];
            //Helpfunctions.Instance.logg("node: " + pn.parent.nodeNum + ", children exhasuted: " + pn.parent.numExhaustedChildren + "/" + pn.parent.children.Length);

            if (pn.isTerminal || (pn.children != null && pn.numExhaustedChildren == pn.children.Length))
            {
                pn.isExhausted = true;
                if (pn.parent != null)
                {
                    pn.parent.numExhaustedChildren++;
                }
            }

            pn.nVisits++;

            if(Added)
                Added = pn.pa.add(sol);

            for(int j = 0; j < result.Length; ++j)
                pn.totValue[j] += result[j];

            if(i+1 < numNodes)
            {
                //ParetoTreeNode parentNode = m_runList.get(i+1);
                //parentNode.m_childCount[pn.childIndex]++; //for Nsa in one of the tree policies (see TransParetoTreePolicy).
                comingFrom = pn.childIndex;
            }
            else if(i+1 == numNodes)
            {
                if(pn.parent != null)
                    throw new Exception("This should be the root... and it's not.");

                if(Added)
                {
                    //Console.WriteLine("AddING (" + result[0] + "," + result[1] + ") to child " + comingFrom + " from " + pn.parent);
                    if(comingFrom != -1)
                    {
                        sol.m_through = comingFrom;
                        pn.valueRoute[comingFrom].Add(sol);
                    }
                }

            }

        }
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

    public int bestActionIndexEuqDistance(double[] targets) {
        bool verbose = false;
        int selected = -1;
        double[][] bounds = m_player.getValueBounds();
        double distance = Double.MaxValue;
        OrderedSolutionList myPA = pa.m_members;
        int numMembers =  myPA.size();
        if(verbose && numMembers>1)
            Console.WriteLine("Choosing among " + myPA.size() + " members.");

        for(int i = 0; i < numMembers; ++i)
        {
            double[] thisRes = myPA.get(i).m_data;

            double[] val = new double[targets.Length];
            for(int t = 0; t < targets.Length; ++t)
            {
                double v =  Utils.normalise(thisRes[t], bounds[t][0], bounds[t][1]);
                val[t] = v;
            }

            double thisDist = Utils.distanceEuq(val, targets);
            if(thisDist < distance)
            {
                distance = thisDist;
                selected = i;
            }
        }

        if(verbose && numMembers>1)
            Console.WriteLine("   Selected: " + selected);

        double[] selectedTarget = myPA.get(selected).m_data;
        //List<int> navSet = valueRoute.Keys.ToList();
        foreach (int key in valueRoute.Keys)
        {
            List<Solution> resFromThisChild = valueRoute[key];

            for(int i =0; i < resFromThisChild.Count; ++i)
            {
                double[] sol = resFromThisChild[i].m_data;

                if(sol.Length == 3 && sol[0] == selectedTarget[0] && sol[1] == selectedTarget[1] && sol[2] == selectedTarget[2])
                {
                    return key;
                }else if(sol.Length == 2 && sol[0] == selectedTarget[0] && sol[1] == selectedTarget[1])
                {
                    return key;
                }
            }
        }
        //throw new Exception("Unexpected selection: " + selected);
        return selected;

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
        for (int i=0; i<children.Length; i++) {

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
        for (int i=0; i<children.Length; i++) {

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


    public int bestActionIndexExpected() {
        int selected = -1;
        double bestValue = -Double.MaxValue;
        for (int i=0; i<children.Length; i++) {

            if(!this.m_prunedChildren[i])
            {
               // Console.WriteLine("Child " + i + ": " + sol[0] + ", " + sol[1] + ", nVis: " + children[i].nVisits);
                if (children[i] != null)
                {
                    double val = children[i].totValue[0] / children[i].nVisits;
                    if(val + m_rnd.NextDouble() * epsilon > bestValue){
                        bestValue = val;
                        selected = i;
                    }
                }
            }
        }
        if (selected == -1) throw new Exception("Unexpected selection!");

        //double sol[] = children[selected].pa.m_members.get(0).m_data;
        //Console.WriteLine("SELECTED: " + (int)bestValue + "," + sol[0] + "," + sol[1] + ": " + selected);

        return selected;
    }




    public void backUp(double[] result) {
        //Nothing to do.
    }

    public double getHV(bool a_normalized)
    {
        if(a_normalized)
            return pa.computeHV(m_player.getValueBounds());
        else
            return pa.computeHV();

    }


    int depth() {
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
        return children == null ? 0 : children.Length;
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