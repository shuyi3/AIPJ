namespace SilverfishAi
{
    using System;
    using System.Collections.Generic;

    public class Ai
    {
        Silverfish sf;
        private int maxdeep = 12;
        public int maxwide = 3000;
        //public int playaroundprob = 40;
        public int playaroundprob2 = 80;


        private bool usePenalityManager = true;
        private bool useCutingTargets = true;
        private bool dontRecalc = true;
        private bool useLethalCheck = true;
        private bool useComparison = true;


        public int lethalMissing = 30; //RR

        public MiniSimulator mainTurnSimulator;

        public List<EnemyTurnSimulator> enemyTurnSim = new List<EnemyTurnSimulator>();
        public List<MiniSimulatorNextTurn> nextTurnSimulator = new List<MiniSimulatorNextTurn>();
        public List<EnemyTurnSimulator> enemySecondTurnSim = new List<EnemyTurnSimulator>();

        public string currentCalculatedBoard = "1";

        PenalityManager penman;

        List<Playfield> posmoves = new List<Playfield>(7000);

        Hrtprozis hp;
        Handmanager hm;
        Helpfunctions help;

        public Action bestmove = null;
        public float bestmoveValue = 0;
        public Playfield nextMoveGuess;
        public Behavior botBase = null;

        public List<Action> bestActions = new List<Action>();

        public bool secondturnsim = false;
        //public int secondTurnAmount = 256;
        public bool playaround = false;

        //private static Ai instance;

        //public static Ai Instance
        //{
        //    get
        //    {
        //        return instance ?? (instance = new Ai());
        //    }
        //}

        public Ai(Silverfish sf)
        {
            this.sf = sf;
            this.nextMoveGuess = new Playfield(sf);
            PenalityManager penman = sf.PenalityManager;
            hp = sf.Hrtprozis;
            hm = sf.Handmanager;
            help = sf.Helpfunctions;

            this.nextMoveGuess = new Playfield (sf) { mana = -100 };

            this.mainTurnSimulator = new MiniSimulator(sf, maxdeep, maxwide, 0); // 0 for unlimited
            this.mainTurnSimulator.setPrintingstuff(true);

            /*this.nextTurnSimulator = new MiniSimulatorNextTurn();
            this.enemyTurnSim = new EnemyTurnSimulator();
            this.enemySecondTurnSim = new EnemyTurnSimulator();*/

            for (int i = 0; i < sf.Settings.numberOfThreads; i++)
            {
                this.nextTurnSimulator.Add(new MiniSimulatorNextTurn(sf));
                this.enemyTurnSim.Add(new EnemyTurnSimulator(sf));
                this.enemySecondTurnSim.Add(new EnemyTurnSimulator(sf));

                this.nextTurnSimulator[i].thread = i;
                this.enemyTurnSim[i].thread = i;
                this.enemySecondTurnSim[i].thread = i;
            }

        }

        public void setMaxWide(int mw)
        {
            this.maxwide = mw;
            if (maxwide <= 100) this.maxwide = 100;
            this.mainTurnSimulator.updateParams(maxdeep, maxwide, 0);
        }

        public void setTwoTurnSimulation(bool stts, int amount)
        {
            this.mainTurnSimulator.setSecondTurnSimu(stts, amount);
            this.secondturnsim = stts;
            sf.Settings.secondTurnAmount = amount;
        }

        public void updateTwoTurnSim()
        {
            this.mainTurnSimulator.setSecondTurnSimu(sf.Settings.simulateEnemysTurn, sf.Settings.secondTurnAmount);
        }

        public void setPlayAround()
        {
            this.mainTurnSimulator.setPlayAround(sf.Settings.playarround, sf.Settings.playaroundprob, sf.Settings.playaroundprob2);
        }

        private void doallmoves(bool test, bool isLethalCheck)
        {
            //set maxwide to the value for the first-turn-sim.
            foreach (EnemyTurnSimulator ets in enemyTurnSim)
            {
                ets.setMaxwideFirstStep(true);
            }

            foreach (EnemyTurnSimulator ets in enemySecondTurnSim)
            {
                ets.setMaxwideFirstStep(false);
            }

            if (isLethalCheck) this.posmoves[0].enemySecretList.Clear();
            this.mainTurnSimulator.doallmoves(this.posmoves[0], isLethalCheck);
            ////HRSim.Helpfunctions.Instance.logTime("do all moves");
            Playfield bestplay = this.mainTurnSimulator.bestboard;
            float bestval = this.mainTurnSimulator.bestmoveValue;

            help.loggonoff(true);
            help.logg("-------------------------------------");
            help.logg("value of best board " + bestval);

            if (isLethalCheck)
            {
                this.lethalMissing = bestplay.enemyHero.armor + bestplay.enemyHero.Hp;//RR
                help.logg("missing dmg to lethal " + this.lethalMissing);
            }
            else
            {
                this.lethalMissing = 130;
            }

            this.bestActions.Clear();
            this.bestmove = null;
            foreach (Action a in bestplay.playactions)
            {
                Action aa = new Action(a);
                if (aa.actionType == actionEnum.playcard && aa.card.entity == 1020)
                {
                    int debug1 = 1;
                } 
                this.bestActions.Add(aa);
                a.print(sf.Helpfunctions);
            }
            //this.bestActions.Add(new Action(actionEnum.endturn, null, null, 0, null, 0, 0));

            if (this.bestActions.Count >= 1)
            {
                this.bestmove = this.bestActions[0];
                //debug checking best moves
                //Playfield pp = new Playfield(sf);
                //Handmanager.Handcard ha = null;
                //if (bestmove.actionType == actionEnum.playcard)
                //{
                //    if (bestmove.card.entity == 1020)
                //    {
                //        int debug1 = 1;
                //    }
                //    if (bestmove.card != null)
                //    {
                //        foreach (Handmanager.Handcard hh in pp.owncards)
                //        {
                //            if (hh.entity == bestmove.card.entity)
                //            {
                //                ha = hh;
                //                break;
                //            }
                //        }
                //        if (bestmove.actionType == actionEnum.useHeroPower)
                //        {
                //            ha = pp.isOwnTurn ? pp.ownHeroAblility : pp.enemyHeroAblility;
                //        }
                //    }
                //    if (ha == null)
                //    {
                //        //there is a bug
                //        int debug = 1;
                //    }
                //} 
                this.bestActions.RemoveAt(0);
            }
            this.bestmoveValue = bestval;

            if (bestmove != null && bestmove.actionType != actionEnum.endturn) // save the guessed move, so we doesnt need to recalc!
            {
                this.nextMoveGuess = new Playfield(sf);

                this.nextMoveGuess.doAction(bestmove);
            }
            else
            {
                nextMoveGuess.mana = -100;
            }
            ////HRSim.Helpfunctions.Instance.logTime("do all actions");
        }

        public void setBestMoves(List<Action> alist, float value)
        {
            help.logg("set best action-----------------------------------");
            this.bestActions.Clear();
            this.bestmove = null;

            foreach (Action a in alist)
            {
                help.logg("-a-");
                //Action aa = new Action(a);
                //if (aa.actionType == actionEnum.playcard && aa.card.entity == 1020)
                //{
                //    int debug1 = 1;
                //}
                //this.bestActions.Add(aa);
                this.bestActions[this.bestActions.Count - 1].print(sf.Helpfunctions);
            }
            //this.bestActions.Add(new Action(actionEnum.endturn, null, null, 0, null, 0, 0));

            if (this.bestActions.Count >= 1)
            {
                this.bestmove = this.bestActions[0];
                this.bestActions.RemoveAt(0);
            }

            this.nextMoveGuess = new Playfield(sf);
            //only debug:
            //this.nextMoveGuess.printBoardDebug();

            if (bestmove != null && bestmove.actionType != actionEnum.endturn) // save the guessed move, so we doesnt need to recalc!
            {
                help.logg("nmgsim-");
                try
                {
                    this.nextMoveGuess.doAction(bestmove);
                    this.bestmove = this.nextMoveGuess.playactions[this.nextMoveGuess.playactions.Count - 1];
                }
                catch (Exception ex)
                {
                    help.logg("Message ---");
                    help.logg("Message ---" + ex.Message);
                    help.logg("Source ---" + ex.Source);
                    help.logg("StackTrace ---" + ex.StackTrace);
                    help.logg("TargetSite ---\n{0}" + ex.TargetSite);

                }
                help.logg("nmgsime-");


            }
            else
            {
                nextMoveGuess.mana = -100;
            }

            //if (bestmove != null && bestmove.actionType == actionEnum.playcard)
            //{
            //    Playfield ppTest = new Playfield(sf);
            //    if (bestmove.card.entity == 1020)
            //    {
            //        int debug1 = 1;
            //    }
            //    bool isCardValid = false;
            //    foreach (Handmanager.Handcard hh in ppTest.owncards)
            //    {
            //        if (hh.entity == bestmove.card.entity)
            //        {
            //            isCardValid = true;
            //            break;
            //        }
            //    }
            //    if (!isCardValid)
            //    {
            //        int debug = 1;
            //    }
            //}
        }

        public void doNextCalcedMove()
        {
            help.logg("noRecalcNeeded!!!-----------------------------------");
            //this.bestboard.printActions();

            this.bestmove = null;
            if (this.bestActions.Count >= 1)
            {
                this.bestmove = this.bestActions[0];
                this.bestActions.RemoveAt(0);
            }
            if (this.nextMoveGuess == null) this.nextMoveGuess = new Playfield(sf);
            //this.nextMoveGuess.printBoardDebug();

            if (bestmove != null && bestmove.actionType != actionEnum.endturn)  // save the guessed move, so we doesnt need to recalc!
            {
                //this.nextMoveGuess = new Playfield();
                help.logg("nmgsim-");
                try
                {
                    this.nextMoveGuess.doAction(bestmove);
                    this.bestmove = this.nextMoveGuess.playactions[this.nextMoveGuess.playactions.Count - 1];
                }
                catch (Exception ex)
                {
                    help.logg("Message ---");
                    help.logg("Message ---" + ex.Message);
                    help.logg("Source ---" + ex.Source);
                    help.logg("StackTrace ---" + ex.StackTrace);
                    help.logg("TargetSite ---\n{0}" + ex.TargetSite);

                }
                help.logg("nmgsime-");

            }
            else
            {
                //help.logg("nd trn");
                nextMoveGuess.mana = -100;
            }
            //if (bestmove != null && bestmove.actionType == actionEnum.playcard)
            //{
            //    Playfield ppTest = new Playfield(sf);
            //    if (bestmove.card.entity == 1020)
            //    {
            //        int debug1 = 1;
            //    }
            //    bool isCardValid = false;
            //    foreach (Handmanager.Handcard hh in ppTest.owncards)
            //    {
            //        if (hh.entity == bestmove.card.entity)
            //        {
            //            isCardValid = true;
            //            break;
            //        }
            //    }
            //    if (!isCardValid)
            //    {
            //        int debug = 1;
            //    }
            //}
        }

        public void dosomethingclever(Behavior bbase)
        {
            //return;
            //turncheck
            //help.moveMouse(950,750);
            //help.Screenshot();
                     
            this.botBase = bbase;
            hp.updatePositions();

            posmoves.Clear();
            posmoves.Add(new Playfield(sf));
            posmoves[0].sEnemTurn = sf.Settings.simulateEnemysTurn;
            /* foreach (var item in this.posmoves[0].owncards)
             {
                 help.logg("card " + item.handcard.card.name + " is playable :" + item.handcard.card.canplayCard(posmoves[0]) + " cost/mana: " + item.handcard.card.cost + "/" + posmoves[0].mana);
             }
             */
            //help.logg("is hero ready?" + posmoves[0].ownHeroReady);
            ////HRSim.Helpfunctions.Instance.logTime("do sth to update");

            help.loggonoff(false);
            //do we need to recalc?
            help.logg("recalc-check###########");
            //foreach (Action a in this.bestActions)
            //{
            //    a.print(help);
            //}
            if (this.dontRecalc && posmoves[0].isEqual(this.nextMoveGuess, true))
            {
                //foreach (Action a in this.bestActions)
                //{
                //    a.print(help);
                //} 
                doNextCalcedMove();
            }
            else
            {
                help.logg("Leathal-check###########");
                bestmoveValue = -1000000;
                DateTime strt = DateTime.Now;
                if (useLethalCheck)
                {
                    strt = DateTime.Now;
                    //HRSim.Helpfunctions.Instance.startTimer();
                    doallmoves(false, true);
                    help.logg("calculated " + (DateTime.Now - strt).TotalSeconds);
                }

                if (bestmoveValue < 10000)
                {
                    posmoves.Clear();
                    posmoves.Add(new Playfield(sf));
                    posmoves[0].sEnemTurn = sf.Settings.simulateEnemysTurn;
                    help.logg("no lethal, do something random######");
                    strt = DateTime.Now;
                    //HRSim.Helpfunctions.Instance.startTimer();
                    doallmoves(false, false);
                    help.logg("calculated " + (DateTime.Now - strt).TotalSeconds);

                }
            }

            //help.logging(true);

        }

        //public void autoTester(bool printstuff, string data = "")
        //{
        //    help.logg("simulating board ");

        //    BoardTester bt = new BoardTester(data);
        //    if (!bt.datareaded) return;
        //    hp.printHero();
        //    hp.printOwnMinions();
        //    hp.printEnemyMinions();
        //    hm.printcards();
        //    //calculate the stuff
        //    posmoves.Clear();
        //    posmoves.Add(new Playfield());
        //    posmoves[0].sEnemTurn = sf.settings.simulateEnemysTurn;
        //    help.logg("Debug: before the foreach");
        //    foreach (Playfield p in this.posmoves)
        //    {
        //        help.logg("Debug: in the foreach");
        //        //p.printBoard();
        //    }
        //    help.logg("ownminionscount " + posmoves[0].ownMinions.Count);
        //    help.logg("owncardscount " + posmoves[0].owncards.Count);

        //    foreach (var item in this.posmoves[0].owncards)
        //    {
        //        help.logg("card " + item.card.name + " is playable :" + item.canplayCard(posmoves[0]) + " cost/mana: " + item.manacost + "/" + posmoves[0].mana);
        //    }
        //    help.logg("ability " + posmoves[0].ownHeroAblility.card.name + " is playable :" + posmoves[0].ownHeroAblility.card.canplayCard(posmoves[0], 2) + " cost/mana: " + posmoves[0].ownHeroAblility.card.getManaCost(posmoves[0], 2) + "/" + posmoves[0].mana);

        //    // lethalcheck + normal
        //    DateTime strt = DateTime.Now;
        //    doallmoves(false, true);
        //    help.logg("calculated 0 " + (DateTime.Now - strt).TotalSeconds);
        //    double timeneeded = 0;
        //    if (bestmoveValue < 10000)
        //    {
        //        posmoves.Clear();
        //        posmoves.Add(new Playfield());
        //        posmoves[0].sEnemTurn = sf.settings.simulateEnemysTurn;
        //        strt = DateTime.Now;
        //        doallmoves(false, false);
        //        timeneeded = (DateTime.Now - strt).TotalSeconds;
        //        help.logg("calculated 1 " + (DateTime.Now - strt).TotalSeconds);
        //    }

        //    if (printstuff)
        //    {
        //        this.mainTurnSimulator.printPosmoves();
        //        help.logg("printPosmoves ended");
        //        simmulateWholeTurn();
        //        help.logg("calculated 2 " + timeneeded);
        //    }
        //}

        public void simmulateWholeTurn()
        {
            help.ErrorLog("########################################################################################################");
            help.ErrorLog("simulate best board");
            help.ErrorLog("########################################################################################################");
            //this.bestboard.printActions();

            Playfield tempbestboard = new Playfield(sf);

            tempbestboard.printBoard();

            if (bestmove != null && bestmove.actionType != actionEnum.endturn)  // save the guessed move, so we doesnt need to recalc!
            {
                bestmove.print(sf.Helpfunctions);

                tempbestboard.doAction(bestmove);

            }
            else
            {
                tempbestboard.mana = -100;
            }
            help.logg("-------------");
            tempbestboard.printBoard();

            foreach (Action bestmovee in this.bestActions)
            {

                help.logg("stepp");


                if (bestmovee != null && bestmove.actionType != actionEnum.endturn)  // save the guessed move, so we doesnt need to recalc!
                {
                    bestmovee.print(sf.Helpfunctions);

                    tempbestboard.doAction(bestmovee);

                }
                else
                {
                    tempbestboard.mana = -100;
                }
                help.logg("-------------");
                tempbestboard.printBoard();
            }

            //help.logg("AFTER ENEMY TURN:" );
            tempbestboard.sEnemTurn = true;
            tempbestboard.endTurn(false, this.playaround, false, sf.Settings.playaroundprob, sf.Settings.playaroundprob2);
            help.logg("ENEMY TURN:-----------------------------");
            tempbestboard.value = int.MinValue;
            tempbestboard.prepareNextTurn(tempbestboard.isOwnTurn);
            sf.Ai.enemyTurnSim[0].simulateEnemysTurn(tempbestboard, true, playaround, true, sf.Settings.playaroundprob, sf.Settings.playaroundprob2);
        }

        public void simmulateWholeTurnandPrint()
        {
            help.ErrorLog("###################################");
            help.ErrorLog("what would silverfish do?---------");
            help.ErrorLog("###################################");
            if (this.bestmoveValue >= 10000) help.ErrorLog("DETECTED LETHAL ###################################");
            //this.bestboard.printActions();

            Playfield tempbestboard = new Playfield(sf);

            if (bestmove != null && bestmove.actionType != actionEnum.endturn)  // save the guessed move, so we doesnt need to recalc!
            {

                tempbestboard.doAction(bestmove);
                tempbestboard.printActionforDummies(tempbestboard.playactions[tempbestboard.playactions.Count - 1]);

                if (this.bestActions.Count == 0)
                {
                    help.ErrorLog("end turn");
                }
            }
            else
            {
                tempbestboard.mana = -100;
                help.ErrorLog("end turn");
            }


            foreach (Action bestmovee in this.bestActions)
            {

                if (bestmovee != null && bestmove.actionType != actionEnum.endturn)  // save the guessed move, so we doesnt need to recalc!
                {
                    //bestmovee.print();
                    tempbestboard.doAction(bestmovee);
                    tempbestboard.printActionforDummies(tempbestboard.playactions[tempbestboard.playactions.Count - 1]);

                }
                else
                {
                    tempbestboard.mana = -100;
                    help.ErrorLog("end turn");
                }
            }
        }

        public void updateEntitiy(int old, int newone)
        {
            help.logg("entityupdate! " + old + " to " + newone);
            if (this.nextMoveGuess != null)
            {
                foreach (Minion m in this.nextMoveGuess.ownMinions)
                {
                    if (m.entitiyID == old) m.entitiyID = newone;
                }
                foreach (Minion m in this.nextMoveGuess.enemyMinions)
                {
                    if (m.entitiyID == old) m.entitiyID = newone;
                }
            }
            foreach (Action a in this.bestActions)
            {
                if (a.own != null && a.own.entitiyID == old) a.own.entitiyID = newone;
                if (a.target != null && a.target.entitiyID == old) a.target.entitiyID = newone;
                if (a.card != null && a.card.entity == old) a.card.entity = newone;
            }

        }

    }


}