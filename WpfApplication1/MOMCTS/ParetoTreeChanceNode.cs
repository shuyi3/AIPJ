using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSim
{
    public class ParetoTreeChanceNode : ParetoTreeNode
    {
        public Action chanceAction;
        public Dictionary<ActionResult, Tuple<double, int>> drawCardResultMap;
        public int childrenCount;
        public int lastChildIndex;
        public double HVvalue;

        public ParetoTreeChanceNode(Playfield state, ParetoTreeNode parent, int childIndex, RandomRoller roller,
                          ParetoTreePolicy treePolicy, Random rnd, ParetoMCTSPlayer a_player, Action chanceAction, int childrenSize) :
            this(state, parent, childIndex, roller,
                       treePolicy, rnd, a_player, chanceAction)
        {
            this.children = new List<ParetoTreeNode>(childrenSize);
        }

        public ParetoTreeChanceNode(Playfield state, ParetoTreeNode parent, int childIndex, RandomRoller roller,
                          ParetoTreePolicy treePolicy, Random rnd, ParetoMCTSPlayer a_player, Action chanceAction)
        {
            this.m_player = a_player;
            this.state = state;
            this.parent = parent;
            this.m_rnd = rnd;
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
            this.chanceAction = chanceAction;
            this.lastChildIndex = 0;
            HVvalue = 0;

        }

        //public override void expand()
        //{
        //    bool lethalCheck = false;

        //    Playfield afterState = new Playfield(state);

        //    Movegenerator.Instance.getMoveListForPlayfield(afterState, false, lethalCheck);

        //    isExpanded = true;

        //    //List<Action> testMoves = Movegenerator.Instance.getMoveList(afterState, lethalCheck, true, true);

        //    if (afterState.moveList.Count > 0)
        //    {
        //        children = new ParetoTreeNode[afterState.moveList.Count];

        //        int i = 0;
        //        ParetoTreeNode tn = null;

        //        foreach (Action a in afterState.moveList)
        //        {
        //            //TODO: 暂时没有智慧祝福，收割机，大哥, 只有抽牌

        //            Playfield nextState = new Playfield(afterState);
        //            int cardDraw = nextState.getNumCardDraw();
        //            nextState.doAction(a);
        //            if (nextState.getNumCardDraw() != cardDraw) //it's a chance node
        //            {
        //                tn = new ParetoTreeChanceNode(new Playfield(nextState), this, i, this.roller, this.mTreePolicy, this.m_rnd, this.m_player, a);
        //            }

        //            tn = new ParetoTreeNode(nextState, this, i, this.roller, this.mTreePolicy, this.m_rnd, this.m_player);
        //            children[i] = tn;
        //            i++;
        //        }

        //        //Helpfunctions.Instance.logg("node expanded: " + this.nodeNum + ", children size: " + this.children.Length);
        //        //totValue = new double[afterState.moveList.Count];
        //        if (parent == null) //This is only for the root:
        //        {
        //            this.initValueRoute(afterState.moveList.Count);
        //        }
        //    }
        //    else
        //    {
        //        this.isTerminal = true;
        //        //this.isExhausted = true;
        //        //if (parent != null)
        //        //{
        //        //    parent.numExhaustedChildren++;
        //        //}
        //    }

        //}

        public ParetoTreeNode doChanceAction() //only if it's a chance node
        {
            ParetoTreeNode tn;
            Playfield nextState = new Playfield(this.parent.state);
            nextState.doAction(chanceAction);
            ActionResult ar = new ActionResult(nextState.moveTrigger.newHandcardList.ToArray());
            if (this.drawCardResultMap == null) //init all the things
            {
                this.drawCardResultMap = new Dictionary<ActionResult, Tuple<double, int>>();
                this.childrenCount = 1;
                lastChildIndex = 0;
                this.drawCardResultMap.Add(ar, new Tuple<double, int>(0, lastChildIndex));
                tn = new ParetoTreeNode(nextState, this, lastChildIndex, this.roller, this.mTreePolicy, this.m_rnd, this.m_player);
                this.children[lastChildIndex] = tn;
            }
            else if (!this.drawCardResultMap.ContainsKey(ar))
            {
                this.childrenCount++;
                lastChildIndex = this.childrenCount - 1;
                this.drawCardResultMap.Add(ar, new Tuple<double, int>(0, lastChildIndex));
                tn = new ParetoTreeNode(nextState, this, lastChildIndex, this.roller, this.mTreePolicy, this.m_rnd, this.m_player);
                this.children[lastChildIndex] = tn;
            }
            else
            {
                lastChildIndex = this.drawCardResultMap[ar].Item2;
                tn = this.children[lastChildIndex];
            }
            return tn;
        }

        //public ParetoTreeNode bestChild()
        //{
        //    return mTreePolicy.bestChild(this, m_player.getValueBounds());
        //}

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
                    Added = pn.pa.add(sol);

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

        public int bestActionIndexEuqDistance(double[] targets)
        {
            bool verbose = false;
            int selected = -1;
            double[][] bounds = m_player.getValueBounds();
            double distance = Double.MaxValue;
            OrderedSolutionList myPA = pa.m_members;
            int numMembers = myPA.size();
            if (verbose && numMembers > 1)
                Console.WriteLine("Choosing among " + myPA.size() + " members.");

            for (int i = 0; i < numMembers; ++i)
            {
                double[] thisRes = myPA.get(i).m_data;

                double[] val = new double[targets.Length];
                for (int t = 0; t < targets.Length; ++t)
                {
                    double v = Utils.normalise(thisRes[t], bounds[t][0], bounds[t][1]);
                    val[t] = v;
                }

                double thisDist = Utils.distanceEuq(val, targets);
                if (thisDist < distance)
                {
                    distance = thisDist;
                    selected = i;
                }
            }

            if (verbose && numMembers > 1)
                Console.WriteLine("   Selected: " + selected);

            double[] selectedTarget = myPA.get(selected).m_data;
            //List<int> navSet = valueRoute.Keys.ToList();
            foreach (int key in valueRoute.Keys)
            {
                List<Solution> resFromThisChild = valueRoute[key];

                for (int i = 0; i < resFromThisChild.Count; ++i)
                {
                    double[] sol = resFromThisChild[i].m_data;

                    if (sol.Length == 3 && sol[0] == selectedTarget[0] && sol[1] == selectedTarget[1] && sol[2] == selectedTarget[2])
                    {
                        return key;
                    }
                    else if (sol.Length == 2 && sol[0] == selectedTarget[0] && sol[1] == selectedTarget[1])
                    {
                        return key;
                    }
                }
            }
            //throw new Exception("Unexpected selection: " + selected);
            return selected;

        }


        public int bestActionIndex(double[] targets)
        {
            bool verbose = false;
            int selected = -1;
            double[][] bounds = m_player.getValueBounds();
            double bestValue = -Double.MaxValue;
            OrderedSolutionList myPA = pa.m_members;
            int numMembers = myPA.size();
            if (verbose && numMembers > 1)
                Console.WriteLine("Choosing among " + myPA.size() + " members.");
            for (int i = 0; i < numMembers; ++i)
            {
                double[] thisRes = myPA.get(i).m_data;
                /*
                double val0 = Utils.normalise(thisRes[0], bounds[0][0], bounds[0][1]);
                double val1 = Utils.normalise(thisRes[1], bounds[1][0], bounds[1][1]);
                double val2 = Utils.normalise(thisRes[2], bounds[2][0], bounds[2][1]);
                double val = targets[0] * val0 + targets[1] * val1 + targets[2] * val2;*/

                double val = 0.0;
                for (int t = 0; t < targets.Length; ++t)
                {
                    double v = Utils.normalise(thisRes[t], bounds[t][0], bounds[t][1]);
                    val += v * targets[t];
                }


                if (verbose && numMembers > 1)
                {
                    if (thisRes.Length == 3)
                        Console.WriteLine("   [%.4f, %.4f, %.4f] => %.4f, from %d\n", thisRes[0], thisRes[1], thisRes[2], val, myPA.get(i).m_through);
                    if (thisRes.Length == 2)
                        Console.WriteLine("   [%.4f, %.4f] => %.4f, from %d\n", thisRes[0], thisRes[1], val, myPA.get(i).m_through);
                }
                //Console.WriteLine("Element in PA " + i + ": " + val);

                if (val > bestValue)
                {
                    bestValue = val;
                    selected = i;
                }

            }

            if (verbose && numMembers > 1)
                Console.WriteLine("   Selected: " + selected);

            if (selected == -1)
            {
                //Console.WriteLine(" ********************* SELECTED -1, myPA.Count(): " + myPA.Count() + " ***************");
                return 0;
            }

            double[] selectedTarget = myPA.get(selected).m_data;
            //List<int> navSet = valueRoute.Keys.ToList();
            foreach (int key in valueRoute.Keys)
            {
                List<Solution> resFromThisChild = valueRoute[key];

                for (int i = 0; i < resFromThisChild.Count; ++i)
                {
                    double[] sol = resFromThisChild[i].m_data;
                    //Console.WriteLine("PA point " + key + ":" + i + ": " + sol[0] + ", " + sol[1] + ", nVis: " + children[key].nVisits);

                    if (sol.Length == 3 && sol[0] == selectedTarget[0] && sol[1] == selectedTarget[1] && sol[2] == selectedTarget[2])
                    //if(sol[0] == selectedTarget[0] && sol[1] == selectedTarget[1])
                    {
                        //Console.WriteLine("SELECTED-3: " + children[key].nVisits + "," + sol[0] + "," + sol[1] + ": " + key);
                        return key;
                    }
                    else if (sol.Length == 2 && sol[0] == selectedTarget[0] && sol[1] == selectedTarget[1])
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

        public int bestActionIndex()
        {
            int selected = -1;
            double bestValue = -Double.MaxValue;
            for (int i = 0; i < children.Count; i++)
            {

                if (!this.m_prunedChildren[i])
                {
                    if (children[i] != null && children[i].nVisits + m_rnd.NextDouble() * epsilon > bestValue)
                    {
                        bestValue = children[i].nVisits;
                        selected = i;
                    }
                }
            }
            if (selected == -1) throw new Exception("Unexpected selection!");
            return selected;
        }

        public int bestActionIndexValue()
        {
            int selected = -1;
            double bestValue = -Double.MaxValue;
            for (int i = 0; i < children.Count; i++)
            {

                if (!this.m_prunedChildren[i])
                {
                    if (children[i] != null && children[i].totValue[0] + m_rnd.NextDouble() * epsilon > bestValue)
                    {
                        bestValue = children[i].totValue[0];
                        selected = i;
                    }
                }
            }
            if (selected == -1) throw new Exception("Unexpected selection!");
            return selected;
        }


        public int bestActionIndexExpected()
        {
            int selected = -1;
            double bestValue = -Double.MaxValue;
            for (int i = 0; i < children.Count; i++)
            {

                if (!this.m_prunedChildren[i])
                {
                    // Console.WriteLine("Child " + i + ": " + sol[0] + ", " + sol[1] + ", nVis: " + children[i].nVisits);
                    if (children[i] != null)
                    {
                        double val = children[i].totValue[0] / children[i].nVisits;
                        if (val + m_rnd.NextDouble() * epsilon > bestValue)
                        {
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




        public void backUp(double[] result)
        {
            //Nothing to do.
        }

        public double getHV(bool a_normalized)
        {
            if (a_normalized)
                return pa.computeHV(m_player.getValueBounds());
            else
                return pa.computeHV();

        }


        public int depth()
        {
            if (parent == null) return 0;
            else return 1 + parent.depth();
        }

        bool nonTerminal()
        {
            return children != null;
        }

        public bool notFullyExpanded()
        {
            int i = 0;
            bool allPruned = true;
            foreach (ParetoTreeNode tn in children)
            {
                allPruned &= m_prunedChildren[i];
                if (tn == null && !m_prunedChildren[i])
                {
                    return true;
                }
                ++i;
            }

            if (allPruned)
                return true;

            return false;
        }

        public bool allChildrenPruned()
        {
            for (int i = 0; i < m_prunedChildren.Length; ++i)
            {
                if (!m_prunedChildren[i])
                {
                    return false;
                }
            }
            return true;
        }

        public bool isLeaf()
        {
            return children == null;
        }

        public int arity()
        {
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
