using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSim
{
    class GreedyPlayer : PlayerAgent
    {
        private List<Playfield> posibleMoves;
        private List<Action> moveList;
        private Playfield board;
        private int maxWide = 500;
        private Behavior bh;
        private bool playerSide;
        private bool useNNEval;
        private int macroMaxCount;
        private int debug = 0;
        private int turnPlayed = 0;
        private int avgBoardCount = 0;
        private bool currentRandom = false;
        private double randomCut = 1.0;
        //class Node
        //{
        //    public List<Node> children;
        //    public Action move;
        //    public Playfield state;

        //    public Node(Action move, Playfield state)
        //    {
        //        this.state = state;
        //        this.move = move;
        //    }
        //}
        public bool isDNNEval()
        { 
            return this.useNNEval;
        }

        public override bool isRandomMove()
        {
            return currentRandom;
        }

        public string getAvgBCString()
        {
            return "Use DNN:" + this.useNNEval + ", avgBoardCount = " + this.avgBoardCount;
        }

        public GreedyPlayer(bool side, Playfield playfield, bool useNNEval)
        {
            this.board = new Playfield(playfield);
            posibleMoves = new List<Playfield>(maxWide);
            moveList = new List<Action>();
            bh = new BehaviorControl();
            this.playerSide = side;
            this.useNNEval = useNNEval;
            this.macroMaxCount = 0;
            if (this.useNNEval) maxWide = 50;
        }

        public override void updateState(Playfield playfield)
        {
            this.board = new Playfield(playfield);
        }


        public override Action getMove()
        {
            if (moveList.Count == 0)
            {
                moveList = getBestPlayfield(true);
                if (moveList[0] == null)
                {
                    moveList = getBestPlayfield(false);
                }
            }

            Action moveToPlay = moveList[0];
            moveList.RemoveAt(0);
            return moveToPlay;
        }

        public override float getStateValue(Playfield p)
        {
            //if (this.useNNEval)
                //return getDNNValue(p);
            //else
                return getSfValue(p);
        }

        private float getDNNValue(Playfield p)
        {
            dynamic encoded_feature = DNNEval.Instance.parsePlayfieldCNN(p, !this.playerSide);
            float stateValue = DNNEval.Instance.PredictValue(encoded_feature);
            return 1.0f - stateValue;
        }

        private float getSfValue(Playfield p)
        {
            float score;

            //if (this.useNNEval)
            //{
            //    List<int> endTurnFeature = bh.getPlayfieldFeature(p, this.playerSide);
            //    dynamic hand_feature = LogitEval.Instance.getHandFeature(p, this.playerSide);
            //    score = LogitEval.Instance.predict(endTurnFeature, hand_feature);
            //}
            //else
            //{
                score = bh.getPlayfieldValue(p, this.playerSide);
            //}

            return score;
        }

        //private bool doLethalCheck()
        //{ 
            
        //}

        public List<Macro> getMacroActionWOTarget(Playfield p)
        {
            List<Macro> macroActionList = new List<Macro>();
            List<Action> macroAction = new List<Action>();
            Playfield currentPf = new Playfield(p);

            List<Playfield> posmoves = new List<Playfield>();
            List<Playfield> temp = new List<Playfield>(7000);
            List<Playfield> macroPfList = new List<Playfield>();
            posmoves.Add(currentPf);
            bool havedonesomething = true;

            List<Macro>[] manaDiffMacroList = new List<Macro>[11];

            for (int i = 0; i < 11; i++)
            {
                manaDiffMacroList[i] = new List<Macro>();
            }
            List<HashSet<int>> actionSetList = new List<HashSet<int>>();

            int depth = -1;
            int startMana = p.getCurrentPlayer(true).mana;
            while (havedonesomething)
            {
                temp.Clear();
                temp.AddRange(posmoves);
                havedonesomething = false;
                int count = temp.Count;
                depth++;

                for (int i = 0; i < count; i++)
                {
                    GameManager.Instance.moveCount++;

                    Playfield pf = temp[i];
                    List<Action> actions = new List<Action>(Movegenerator.Instance.getNoneTargetMove(pf, 0.0));

                    foreach (Action a in actions)
                    {
                        havedonesomething = true;
                        Playfield nextPf = new Playfield(pf);
                        nextPf.doAction(a);
                        posmoves.Add(nextPf);             
                    }

                    List<Action> playedActions = pf.getActions();
                    List<Action> subPlayedActions = playedActions.GetRange(playedActions.Count - depth, depth);

                    if (subPlayedActions.Count > 0)
                    {


                        int curMana = pf.getCurrentPlayer(true).mana;
                        int manaCost = Math.Max(0, startMana - curMana);
                        pf.endTurn(false, false);
                        float sfScore = getSfValue(pf);

                        HashSet<int> actionSet = new HashSet<int>();
                        foreach (Action subAction in subPlayedActions)
                        {
                            if (subAction.actionType == actionEnum.useHeroPower)
                            {
                                actionSet.Add(-1000); //-1000 for heroPower
                            }
                            else if (subAction.actionType == actionEnum.playcard)
                            {
                                actionSet.Add(subAction.card.entity);
                            }
                            else
                            {
                                actionSet.Add(subAction.own.entitiyID);
                            }
                        }

                        bool exist = false;
                        foreach (HashSet<int> set in actionSetList)
                        {
                            if (set.SetEquals(actionSet))
                            {
                                exist = true;
                                break;
                            }
                        }

                        if (!exist)
                        {
                            List<Macro> macroList = manaDiffMacroList[manaCost];
                            Macro macro = new Macro(null, sfScore, subPlayedActions, manaCost);
                            actionSetList.Add(actionSet);
                            macroList.Add(macro);
                        }
                    }

                    posmoves.Remove(pf);
                }
            }

            for (int i = 0; i < 11; i++)
            {
                if (manaDiffMacroList[i].Count > 0)
                {
                    manaDiffMacroList[i] = manaDiffMacroList[i].OrderByDescending(x => x.score).Take(2).ToList();
                    macroActionList.AddRange(manaDiffMacroList[i]);
                }

            }

            return macroActionList;
        }


        public List<Macro> getMacroAction(Playfield p, Minion target)
        {
            List<Macro> macroActionList = new List<Macro>();
            List<Action> macroAction = new List<Action>();
            Playfield currentPf = new Playfield(p);

            List<Playfield> posmoves = new List<Playfield>();
            List<Playfield> temp = new List<Playfield>(7000);
            List<Playfield> macroPfList = new List<Playfield>();
            posmoves.Add(currentPf);
            bool havedonesomething = true;

            List<Macro>[] manaDiffMacroList = new  List<Macro>[11];

            for (int i = 0; i < 11; i++)
            { 
                manaDiffMacroList[i] = new List<Macro>();
            }
            List<HashSet<int>> actionSetList = new List<HashSet<int>>();

            int depth = 0;
            int startMana = p.getCurrentPlayer(true).mana;
            while (havedonesomething)
            {
                int debug = 0;

                temp.Clear();
                temp.AddRange(posmoves);
                havedonesomething = false;
                int count = temp.Count;
                depth++;

                for (int i = 0; i < count; i++)
                {

                    Playfield pf = temp[i];
                    List<Action> actions = new List<Action>(Movegenerator.Instance.getMoveForTarget(pf, target, 0.0));

                    foreach (Action a in actions)
                    {
                        havedonesomething = true;
                        Playfield nextPf = new Playfield(pf);
                        nextPf.doAction(a);

                        List<Minion> enemyMinions = nextPf.getCurrentPlayer(false).ownMinions;
                        List<int> minionEntity = new List<int>();
                        foreach (Minion m in enemyMinions)
                        {
                            minionEntity.Add(m.entitiyID);
                        }

                        if (minionEntity.Contains(target.entitiyID))
                        {
                            posmoves.Add(nextPf);
                        }
                        else
                        {
                            float sfScore = getSfValue(nextPf);
                            List<Action> playedActions = nextPf.getActions();
                            List<Action> subPlayedActions = playedActions.GetRange(playedActions.Count - depth, depth);

                            if (subPlayedActions.Count > 0)
                            {

                                int manaCost = Math.Max(0, startMana - nextPf.getCurrentPlayer(true).mana);

                                HashSet<int> actionSet = new HashSet<int>();
                                foreach (Action subAction in subPlayedActions)
                                {
                                    if (subAction.actionType == actionEnum.useHeroPower)
                                    {
                                        actionSet.Add(-1000); //-1000 for heroPower
                                    }
                                    else if (subAction.actionType == actionEnum.playcard)
                                    {
                                        actionSet.Add(subAction.card.entity);
                                    }
                                    else
                                    {
                                        actionSet.Add(subAction.own.entitiyID);
                                    }
                                }

                                bool exist = false;
                                foreach (HashSet<int> set in actionSetList)
                                { 
                                    if (set.SetEquals(actionSet))
                                    {
                                        exist = true;
                                        break;
                                    }
                                }

                                if (!exist)
                                {
                                    List<Macro> macroList = manaDiffMacroList[manaCost];
                                    Macro macro = new Macro(target, sfScore, subPlayedActions, manaCost);
                                    actionSetList.Add(actionSet);
                                    macroList.Add(macro);
                                }
                            }

                        }
                    }

                    posmoves.Remove(pf);
                }
            }

            for (int i = 0; i < 11; i++)
            {
                if (manaDiffMacroList[i].Count > 0)
                {
                    manaDiffMacroList[i] = manaDiffMacroList[i].OrderByDescending(x => x.score).Take(2).ToList();
                    macroActionList.AddRange(manaDiffMacroList[i]);
                }

            } 

            return macroActionList;
        }

        public List<Macro> getMacros(Playfield p)
        {
            List<Macro> macroPfList = null;
            List<Macro> macroListAllMinion = new List<Macro>();
             
            List<Minion> tempMinions = p.getCurrentPlayer(false).ownMinions;
            foreach (Minion m in tempMinions)
            {

                macroPfList = getMacroAction(p, m);
                macroListAllMinion.AddRange(macroPfList);
            }

            macroPfList = getMacroActionWOTarget(p);
            macroListAllMinion.AddRange(macroPfList);

            return macroListAllMinion;
        }

        public Playfield getBestCandidate(List<Playfield> candidateStates, bool lethalCheck)
        {
            float bestVal = float.MinValue;
            Playfield bestState = null;

            if (lethalCheck)
            {
                foreach (Playfield p in candidateStates)
                {
                    Player ePlayer = playerSide ? p.playerSecond : p.playerFirst;
                    if (ePlayer.ownHero.Hp < 0)
                    {
                        return p;
                    }
                }
                return null;
            }

            //if (!this.useNNEval)
            //{
                foreach (Playfield p in candidateStates)
                {
                    float value = getStateValue(p);
                    if (getStateValue(p) > bestVal)
                    {
                        bestVal = value;
                        bestState = p;
                    }
                }
            //}
            //else
            //{
            //    foreach (Playfield p in candidateStates)
            //    {
            //        float value = getDNNValue(p);
            //        //p.printLastTurnActions();
            //        //Helpfunctions.Instance.logg("DNN VAL = " + value);
            //        //float sfVal = getSfValue(p);
            //        //Helpfunctions.Instance.logg("SF VAL = " + sfVal);
            //        if (value > bestVal)
            //        {
            //            bestVal = value;
            //            bestState = p;
            //        }
            //    }
            //    //int idx = LogitEval.Instance.getBestCandidate(candidateStates, this.playerSide);
            //    //bestState = candidateStates[idx];
            //}
            return bestState;
        }

        private List<Action> getBestPlayfield(bool lethalCheck)
        {
            this.currentRandom = (GameManager.getRNG().NextDouble() > this.randomCut) ? true : false;
            this.currentRandom = false;
            this.turnPlayed += 1;

            bool turn = this.board.isOwnTurn;

            List<Action> actionList = new List<Action>();

            List<Playfield> temp = new List<Playfield>(7000);
            bool havedonesomething = true;

            Playfield currentState = new Playfield(this.board);
            double probCut = this.useNNEval ? 1.0 : 0.0;
            //if (this.playerSide == false)
            //{
            //    for (int i = 0; i < currentState.getCurrentPlayer(true).owncards.Count; i++)
            //    {
            //        Console.WriteLine("idx:" + i);
            //        Console.WriteLine(currentState.getCurrentPlayer(true).owncards[i].card.name);
            //    }

            //    string s = Console.ReadLine();
            //    if (s.Length != 0)
            //    {
            //        string[] sList = s.Split(',');
            //        foreach (string idxStr in sList)
            //        {
            //            int idx = int.Parse(idxStr);
            //            currentState.getCurrentPlayer(true).owncards[idx].playProb = 0.0;
            //        }
            //    }
            //}
            
            List<Playfield> posmoves = new List<Playfield>();
            posmoves.Add(currentState);

            if (this.useNNEval)
            {
                DNNEval.Instance.getNNActionPolicy(currentState, this.playerSide);
                Player mPlayer = currentState.getCurrentPlayer(true);

                foreach (Handmanager.Handcard hc in mPlayer.owncards)
                {
                    Console.WriteLine(hc.card.name + ":" + hc.playProb);
                }
            }
                            
            int boardcount = 0;
            int deep = 0;
            //float bestoldval = float.MinValue;

            Dictionary<int, Playfield> seenStates = new Dictionary<int, Playfield>();
            List<int> levelCount = new List<int>();
            //List<Playfield> candidateStates = new List<Playfield>();
            Dictionary<int, List<Playfield>> candidateStates = new Dictionary<int, List<Playfield>>();

            while (havedonesomething)
            {
                debug = 0;

                temp.Clear();
                levelCount.Add(posmoves.Count);
                temp.AddRange(posmoves);
                havedonesomething = false;

                int count = temp.Count;
                for (int i = 0; i < count; i++)
                {
                    Playfield p = temp[i];
                    if (turn != p.isOwnTurn)
                    {
                        continue;
                    }

                    List<Action> actions = new List<Action>(Movegenerator.Instance.getMoveList(p, false, true, true, probCut));

                    foreach (Action a in actions)
                    {
                        havedonesomething = true;
                        Playfield pf = new Playfield(p);
                        pf.doAction(a);
                        if (!seenStates.ContainsKey(pf.GetHashCode()))
                        {
                            seenStates[pf.GetHashCode()] = pf;
                            posmoves.Add(pf);
                        }
                        else
                        {
                            continue;
                        }

                    }

                    int actionHash = BoardHash.Instance.getActionHash(p);

                    bool followPolicy = true;
                    if (this.useNNEval)
                    {
                        int violated = 0;
                        for (int j = 0; j < p.getCurrentPlayer(true).owncards.Count; j++)
                        {
                            Handmanager.Handcard hc = p.getCurrentPlayer(true).owncards[j];
                            //Helpfunctions.Instance.logg(hc.card.name + ":" + hc.playProb);
                            //Helpfunctions.Instance.logg("Entity:" + hc.entity);
                            violated = hc.entity;
                            if (p.getCurrentPlayer(true).owncards[j].playProb > 0)
                            {
                                followPolicy = false; 
                                break;
                            }
                        }
                        //if (p.getCurrentPlayer(true).playactions.Count > 0 && violated != 1041)
                        //{
                        //    int ddbug = 1;
                        //}
                    }

                    p.endTurn(false, false);
                    p.drawTurnStartCard();
                    //float value;
                    //if (lethalCheck)
                    //{
                    //    Player ePlayer;
                    //    ePlayer = playerSide ? p.playerSecond : p.playerFirst;
                    //    if (ePlayer.ownHero.Hp < 0)
                    //    {
                    //        value = 1.0f;
                    //    }
                    //    else
                    //    {
                    //        value = 0.0f;
                    //    }
                    //}
                    //else
                    //{
                    //    value = getValue(p);
                    //}
                    //if (value > bestoldval) // want the best enemy-play-> worst for us
                    //{
                    //    bestoldval = value;
                    //    bestold = p;                    
                    //}     
                   
                    if (followPolicy)
                    {
                        if (!candidateStates.ContainsKey(actionHash))
                        {
                            candidateStates[actionHash] = new List<Playfield>();
                        }
                        float stateValue = getStateValue(p);
                        p.value = stateValue;
                        candidateStates[actionHash].Add(p);
                        boardcount++;

                        //candidateStates.Add(p);
                        posmoves.Remove(p);

                        if (boardcount >= maxWide)
                        {
                            break;
                        }
                    }
                }

                //if (bestoldval <= 10000 && bestold != null)
                //{
                //    posmoves.Add(bestold);
                //}

                deep++;
                if (boardcount >= maxWide)
                {
                    Helpfunctions.Instance.logg("reach maxwide");
                    break;
                }
            }

           
            List<Playfield> statesList = new List<Playfield>();

            Helpfunctions.Instance.logg("Diff card play policy: " + candidateStates.Count);
            foreach (List<Playfield> value in candidateStates.Values)
            {
                if (value.Count > 0)
                {
                    //Helpfunctions.Instance.logg("Actions:" + lethalCheck);
                    //value[0].printLastTurnActions();
                    //Helpfunctions.Instance.logg("Has " + value.Count + " children");
                    //if (value.Count > 100)
                    //{
                    //    this.board.printBoard();
                    //    debug = 1;
                    //}
                    //Helpfunctions.Instance.logg("Count: " + value.Count);
                    //Helpfunctions.Instance.logg("Top 10: ");

                    var mySortedList = value.OrderByDescending(l => l.value)
                         .Take(10);
                    //foreach (Playfield pf in mySortedList)
                    //{
                    //    Helpfunctions.Instance.logg("state value:" + pf.value);
                    //    pf.printLastTurnActions();
                    //}
                    statesList.AddRange(value);
                }
            }

            if (statesList.Count > 10000)
            {
                debug = 1;
            }
            //Playfield bestState = getBestCandidate(candidateStates, lethalCheck);
            Playfield bestState = null;
            if (!lethalCheck && this.currentRandom)
            {
                bestState = statesList[GameManager.getRNG().Next(statesList.Count)];
            }
            else
            { 
                bestState = getBestCandidate(statesList, lethalCheck);
            }

            if (lethalCheck)
            {
                if (bestState != null)
                    actionList.AddRange(bestState.getLastTurnAction());
                else
                    actionList.Add(null);
            }
            else
            {
                if (bestState != null)
                {
                    bestState.printLastTurnMacros();
                    actionList.AddRange(bestState.getLastTurnAction());
                    //Helpfunctions.Instance.logg("value = " + bestoldval);
                }

                actionList.Add(null);
            }

            this.avgBoardCount = (this.avgBoardCount + boardcount) / this.turnPlayed;
            Helpfunctions.Instance.logg("Use DNN:" + this.useNNEval + ", BC = " + boardcount);
            Helpfunctions.Instance.logg("Use DNN:" + this.useNNEval + ", avgBoardCount = " + this.avgBoardCount);
            return actionList;
        }
    }
}
