using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSim
{
    class GreedyPlayerMacro : PlayerAgent
    {
        private List<Playfield> posibleMoves;
        private List<Action> moveList;
        private Playfield board;
        private int maxWide = 15000;
        private Behavior bh;
        private bool playerSide;
        private bool useNNEval;
        private int macroMaxCount;

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

        public GreedyPlayerMacro(bool side, Playfield playfield, bool useNNEval)
        {
            this.board = new Playfield(playfield);
            posibleMoves = new List<Playfield>(maxWide);
            moveList = new List<Action>();
            bh = new BehaviorControl();
            this.playerSide = side;
            this.useNNEval = useNNEval;
            this.macroMaxCount = 0;
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

        private float getValue(Playfield p)
        {
            float score;
            //if (this.useNNEval)
            //{
            //    score = DNNEval.Instance.getNNEval(p, this.playerSide);
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
                int debug = 0;

                temp.Clear();
                temp.AddRange(posmoves);
                havedonesomething = false;
                int count = temp.Count;
                depth++;

                for (int i = 0; i < count; i++)
                {
                    GameManager.Instance.moveCount++;

                    Playfield pf = temp[i];
                    List<Action> actions = new List<Action>(Movegenerator.Instance.getNoneTargetMove(pf));

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
                        float score = getValue(pf);

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
                            Macro macro = new Macro(null, score, subPlayedActions, manaCost);
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
                    List<Action> actions = new List<Action>(Movegenerator.Instance.getMoveForTarget(pf, target));

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
                            float score = getValue(nextPf);
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
                                    Macro macro = new Macro(target, score, subPlayedActions, manaCost);
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


        private List<Action> getBestPlayfield(bool lethalCheck)
        {
            bool turn = this.board.isOwnTurn;

            List<Action> actionList = new List<Action>();

            List<Playfield> temp = new List<Playfield>(7000);
            bool havedonesomething = true;

            Playfield currentState = new Playfield(this.board);
            List<Playfield> posmoves = new List<Playfield>();
            posmoves.Add(currentState);
                            
            Playfield bestold = null;

            int boardcount = 0;
            int deep = 0;
            float bestoldval = float.MinValue;

            DNNEval.Instance.getNNEval(currentState, this.playerSide);

            while (havedonesomething)
            {
                int debug = 0;

                temp.Clear();
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

                    List<Macro> macroListAllMinion = getMacros(p);

                    //if (macroPfList.Count > this.macroMaxCount)
                    //{
                    //    Helpfunctions.Instance.logg("===========count:" + macroPfList.Count + "==================");
                    //    Helpfunctions.Instance.logg("===========no target==================");
                    //    p.printBoard();
                    //    this.macroMaxCount = macroPfList.Count;
                    //    if (this.macroMaxCount == 5)
                    //    {
                    //        foreach (Macro macro in macroPfList)
                    //        {
                    //            Helpfunctions.Instance.logg(" ====================Print macro cost: " + macro.manaCost);
                    //            foreach (Action action in macro.actionList)
                    //            {
                    //                action.print();
                    //            }
                    //        }
                    //    }
                    //}


                    foreach (Macro m in macroListAllMinion)
                    {
                        havedonesomething = true;
                        Playfield pf = new Playfield(p);
                        pf.doMacroAction(m);
                        posmoves.Add(pf);
                        boardcount++;
                    }
     

                    //foreach (Action a in actions)
                    //{
                    //    havedonesomething = true;
                    //    Playfield pf = new Playfield(p);
                    //    pf.doAction(a);
                    //    posmoves.Add(pf);
                    //    boardcount++;
                    //}

                    p.endTurn(false, false);
                    p.drawTurnStartCard();
                    float value;
                    if (lethalCheck)
                    {
                        Player ePlayer;
                        ePlayer = playerSide ? p.playerSecond : p.playerFirst;
                        if (ePlayer.ownHero.Hp < 0)
                        {
                            value = 1.0f;
                        }
                        else
                        {
                            value = 0.0f;
                        }
                    }
                    else
                    {
                        value = getValue(p);
                        if (debug == 1)
                        {
                            //Helpfunctions.Instance.logg("==============================");
                            //Helpfunctions.Instance.logg("coin value = " + bestoldval);
                            //p.printLastTurnActions();
                            //Helpfunctions.Instance.logg("==============================");
                        }
                    }
                    if (value > bestoldval) // want the best enemy-play-> worst for us
                    {
                        bestoldval = value;
                        bestold = p;
                        //GameManager.Instance.moveCount++;
                        //if (GameManager.Instance.moveCount == 17)
                        //{
                        //    debug = 1;
                        //}
                        Helpfunctions.Instance.logg("update value = " + bestoldval);
                        Helpfunctions.Instance.logg("=============macro start===========");
                        bestold.printLastTurnMacros();
                        Helpfunctions.Instance.logg("=============action start==========");
                        bestold.printLastTurnActions();
                        Helpfunctions.Instance.logg("=============action end============");

                    }
                    posmoves.Remove(p);

                    if (boardcount >= maxWide)
                    {
                        Helpfunctions.Instance.logg("reach maxwide");
                        break;
                    }
                }

                if (bestoldval <= 10000 && bestold != null)
                {
                    posmoves.Add(bestold);
                }

                deep++;
                if (boardcount >= maxWide)
                {
                    Helpfunctions.Instance.logg("reach maxwide");
                    break;
                }
            }

            if (lethalCheck)
            {
                if (bestoldval == 1.0f)
                    actionList.AddRange(bestold.getLastTurnAction());
                else
                    actionList.Add(null);
            }
            else
            {
                if (bestold != null)
                {
                    bestold.printLastTurnMacros();
                    actionList.AddRange(bestold.getLastTurnAction());
                    Helpfunctions.Instance.logg("value = " + bestoldval);
                }

                actionList.Add(null);
            }
           
            return actionList;
        }
    }
}
