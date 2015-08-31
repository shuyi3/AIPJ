﻿namespace HREngine.Bots
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public class MiniSimulator
    {
        Silverfish sf;
        //#####################################################################################################################
        private int maxdeep = 6;
        private int maxwide = 10;
        private int totalboards = 50;
        private bool usePenalityManager = true;
        private bool useCutingTargets = true;
        private bool dontRecalc = true;
        private bool useLethalCheck = true;
        private bool useComparison = true;


        private bool printNormalstuff = false;

        List<Playfield> posmoves = new List<Playfield>(7000);
        List<Playfield> twoturnfields = new List<Playfield>(500);

        List<List<Playfield>> threadresults = new List<List<Playfield>>(64);
        private int dirtyTwoTurnSim = 256;

        public Action bestmove = null;
        public float bestmoveValue = 0;
        public Playfield bestboard;

        public Behavior botBase = null;
        private int calculated = 0;

        private bool simulateSecondTurn = false;
        private bool playaround = false;
        private int playaroundprob = 50;
        private int playaroundprob2 = 80;


        public MiniSimulator(Silverfish sf, int deep, int wide, int ttlboards)
        {
            this.sf = sf;
            this.bestboard = new Playfield(sf);
            this.maxdeep = deep;
            this.maxwide = wide;
            this.totalboards = ttlboards;
        }

        public void updateParams(int deep, int wide, int ttlboards)
        {
            this.maxdeep = deep;
            this.maxwide = wide;
            this.totalboards = ttlboards;
        }

        public void setPrintingstuff(bool sp)
        {
            this.printNormalstuff = sp;
        }

        public void setSecondTurnSimu(bool sts, int amount)
        {
            //this.simulateSecondTurn = sts;
            this.dirtyTwoTurnSim = amount;
        }

        public void setPlayAround(bool spa, int pprob, int pprob2)
        {
            this.playaround = spa;
            this.playaroundprob = pprob;
            this.playaroundprob2 = pprob2;
        }

        private void addToPosmoves(Playfield pf)
        {
            if (pf.ownHero.Hp <= 0) return;
            /*foreach (Playfield p in this.posmoves)
            {
                if (pf.isEqual(p, false)) return;
            }*/
            this.posmoves.Add(pf);

            //posmoves.Sort((a, b) => -(botBase.getPlayfieldValue(a)).CompareTo(botBase.getPlayfieldValue(b)));//want to keep the best
            //if (posmoves.Count > this.maxwide) posmoves.RemoveAt(this.maxwide);
            if (this.totalboards >= 1)
            {
                this.calculated++;
            }
        }

        private void startEnemyTurnSim(Playfield p, bool simulateTwoTurns, bool print)
        {
            if (p.guessingHeroHP >= 1)
            {
                //simulateEnemysTurn(simulateTwoTurns, playaround, print, pprob, pprob2);
                p.prepareNextTurn(p.isOwnTurn);

                sf.Ai.enemyTurnSim[0].simulateEnemysTurn(p, simulateTwoTurns, playaround, print, playaroundprob, playaroundprob2);

            }
            p.complete = true;
        }

        public float doallmoves(Playfield playf, bool isLethalCheck)
        {
            if (botBase == null) botBase = sf.Ai.botBase;
            bool test = false;
            this.posmoves.Clear();
            this.twoturnfields.Clear();
            this.addToPosmoves(playf);
            bool havedonesomething = true;
            List<Playfield> temp = new List<Playfield>();
            int deep = 0;
            //sf.helpfunctions.logg("NXTTRN" + playf.mana + " " + posmoves.Count);
            this.calculated = 0;
            while (havedonesomething)
            {
                if (this.printNormalstuff) sf.Helpfunctions.logg("ailoop");
                GC.Collect();
                temp.Clear();
                temp.AddRange(this.posmoves);
                havedonesomething = false;
                Playfield bestold = null;
                float bestoldval = -20000000;
                foreach (Playfield p in temp)
                {

                    if (p.complete || p.ownHero.Hp <= 0)
                    {
                        continue;
                    }

                    //gernerate actions and play them!
                    List<Action> actions = sf.Movegenerator.getMoveList(p, isLethalCheck, usePenalityManager, useCutingTargets);
                    
                    //sf.helpfunctions.ErrorLog(" ");
                    //sf.helpfunctions.ErrorLog(actions.Count + " Playfield: " + p.hashcode.ToString());
                    foreach (Action a in actions)
                    {
                       /* string aList = "";

                        if (a.actionType != null)
                        {
                            try
                            {
                                aList = aList + "type:" + a.actionType.ToString();
                            }
                            catch
                            {
                                aList = aList + "type:---";
                            }
                        }

                        if (a.card != null)
                        {
                            try
                            {
                                aList = aList + "  Cname:" + a.card.card.name.ToString();
                            }
                            catch
                            {
                                aList = aList + "  Cname:---";
                            }
                        }

                        if (a.own != null)
                        {
                            try
                            {
                                aList = aList + "  oname:" + a.own.name.ToString();
                            }
                            catch
                            {
                                aList = aList + "  oname:---";
                            }
                        }

                        if (a.place != null)
                        {
                            try
                            {
                                aList = aList + "  place:" + a.place.ToString();
                            }
                            catch
                            {
                                aList = aList + "  place:---";
                            }
                        }

                        if (a.penalty != null)
                        {
                            try
                            {
                                aList = aList + "  penalty:" + a.penalty.ToString();
                            }
                            catch
                            {
                                aList = aList + "  penalty:---";
                            }
                        }

                        sf.helpfunctions.ErrorLog(aList);*/
                            
                            //if (deep == 0 && a.actionType == actionEnum.attackWithMinion) sf.helpfunctions.ErrorLog("play " + a.own.entitiyID + " -> " + a.target.entitiyID);
                        havedonesomething = true;
                        Playfield pf = new Playfield(p);
                        pf.doAction(a);
                        addToPosmoves(pf);
                    }
                    //sf.helpfunctions.ErrorLog("deep " + deep + " len " + this.posmoves.Count);

                    // end the turn of the current board (only if its not a lethalcheck)
                    if (isLethalCheck)
                    {
                        p.complete = true;
                    }
                    else
                    {
                        //end turn of enemy
                        p.endTurn(this.simulateSecondTurn, this.playaround, false, this.playaroundprob, this.playaroundprob2);
                        //simulate the enemys response
                        this.startEnemyTurnSim(p, this.simulateSecondTurn, false);
                    }

                    //sort stupid stuff ouf

                    if (botBase.getPlayfieldValue(p) > bestoldval)
                    {
                        bestoldval = botBase.getPlayfieldValue(p);
                        bestold = p;
                    }
                    if (!test)
                    {
                        posmoves.Remove(p);
                    }

                    if (this.calculated > this.totalboards) break;
                }

                if (!test && bestoldval >= -10000 && bestold != null)
                {
                    this.posmoves.Add(bestold);
                }

                //sf.helpfunctions.loggonoff(true);
                if (this.printNormalstuff)
                {
                    int donec = 0;
                    foreach (Playfield p in posmoves)
                    {
                        if (p.complete) donec++;
                    }
                    sf.Helpfunctions.logg("deep " + deep + " len " + this.posmoves.Count + " dones " + donec);
                }

                if (!test)
                {
                    cuttingposibilities();
                }

                if (this.printNormalstuff)
                {
                    sf.Helpfunctions.logg("cut to len " + this.posmoves.Count);
                }
                //sf.helpfunctions.loggonoff(false);
                deep++;

                if (this.calculated > this.totalboards) break;
                if (deep >= this.maxdeep) break;//remove this?
            }

            foreach (Playfield p in posmoves)//temp
            {
                if (!p.complete)
                {
                    if (isLethalCheck)
                    {
                        p.complete = true;
                    }
                    else
                    {
                        p.endTurn(this.simulateSecondTurn, this.playaround, false, this.playaroundprob, this.playaroundprob2);
                        this.startEnemyTurnSim(p, this.simulateSecondTurn, false);
                    }
                }
            }

            // search the best play...........................................................

            //do dirtytwoturnsim first :D
            if (!isLethalCheck) doDirtyTwoTurnsim();

            if (!isLethalCheck) this.dirtyTwoTurnSim /= 2;

            // sf.helpfunctions.logg("find best ");
            if (posmoves.Count >= 1)
            {
                float bestval = int.MinValue;
                int bestanzactions = 1000;
                Playfield bestplay = posmoves[0];//temp[0]
                foreach (Playfield p in posmoves)//temp
                {
                    float val = botBase.getPlayfieldValue(p);
                    if (bestval <= val)
                    {
                        if (bestval == val && bestanzactions < p.playactions.Count) continue;
                        bestplay = p;
                        bestval = val;
                        bestanzactions = p.playactions.Count;
                    }

                }

                this.bestmove = bestplay.getNextAction();
                this.bestmoveValue = bestval;
                this.bestboard = new Playfield(bestplay);
                //sf.helpfunctions.logg("return");
                return bestval;
            }
            //sf.helpfunctions.logg("return");
            this.bestmove = null;
            this.bestmoveValue = -100000;
            this.bestboard = playf;
            return -10000;
        }

        public void doDirtyTwoTurnsim()
        {
            //return;
            if (this.dirtyTwoTurnSim == 0) return;
            this.posmoves.Clear();
            int thread = 0;
            //DateTime started = DateTime.Now;

            //set maxwide of enemyturnsimulator's to second step (this value is higher than the maxwide in first step) 
            foreach (EnemyTurnSimulator ets in sf.Ai.enemyTurnSim)
            {
                ets.setMaxwideSecondStep(true);
            }

            if (sf.Settings.numberOfThreads == 1)
            {
                foreach (Playfield p in this.twoturnfields)
                {

                    if (p.guessingHeroHP >= 1)
                    {
                        p.value = int.MinValue;
                        //simulateEnemysTurn(simulateTwoTurns, playaround, print, pprob, pprob2);
                        p.prepareNextTurn(p.isOwnTurn);
                        sf.Ai.enemyTurnSim[thread].simulateEnemysTurn(p, true, playaround, false, this.playaroundprob, this.playaroundprob2);
                    }
                    else
                    {
                        //p.value = -10000;
                    }
                    //sf.Ai.enemyTurnSim.simulateEnemysTurn(p, true, this.playaround, false, this.playaroundprob, this.playaroundprob2);
                    this.posmoves.Add(p);
                }
            }
            else
            {
                //multithreading!

                List<System.Threading.Thread> tasks = new List<System.Threading.Thread>(sf.Settings.numberOfThreads);
                for (int kl = 0; kl < sf.Settings.numberOfThreads; kl++)
                {
                    if (this.threadresults.Count > kl)
                    {
                        this.threadresults[kl].Clear();
                        continue;
                    }
                    this.threadresults.Add(new List<Playfield>());
                }


                int k = 0;
                for (k = 0; k < sf.Settings.numberOfThreads; k++)
                {
                    //System.Threading.Thread threadl = new System.Threading.Thread(() => this.Workthread(test, botBase, isLethalCheck, playfieldsTasklist[k], threadnumbers[k]));
                    System.Threading.Thread threadl = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(this.dirtyWorkthread));
                    //System.Threading.Tasks.Task tsk = new System.Threading.Tasks.Task(this.Workthread, (object)new threadobject(test, botBase, isLethalCheck, playfieldsTasklist[k], threadnumbers[k]));
                    int i = k;
                    threadl.Start((object)i);

                    tasks.Add(threadl);

                }

                System.Threading.Thread.Sleep(1);

                for (int j = 0; j < sf.Settings.numberOfThreads; j++)
                {
                    tasks[j].Join();
                    posmoves.AddRange(this.threadresults[j]);
                }


            }

            //just for debugging
            posmoves.Sort((a, b) => -(botBase.getPlayfieldValue(a)).CompareTo(botBase.getPlayfieldValue(b)));//want to keep the best

            //sf.helpfunctions.ErrorLog("time needed for parallel: " + (DateTime.Now - started).TotalSeconds);
        }

        //workthread for dirtyTwoTurnsim
        private void dirtyWorkthread(object to)
        {
            int threadnumber = (int)to;
            //sf.helpfunctions.ErrorLog("Hi, i'm no " + threadnumber);
            for (int i = 0; i < this.twoturnfields.Count; i++)
            {
                if (i % sf.Settings.numberOfThreads == threadnumber)
                {
                    //if(threadnumber ==0)sf.helpfunctions.ErrorLog("no " + threadnumber + " calculates " + i);
                    Playfield p = this.twoturnfields[i];
                    if (p.guessingHeroHP >= 1)
                    {
                        p.value = int.MinValue;
                        //simulateEnemysTurn(simulateTwoTurns, playaround, print, pprob, pprob2);
                        p.prepareNextTurn(p.isOwnTurn);
                        sf.Ai.enemyTurnSim[threadnumber].simulateEnemysTurn(p, true, playaround, false, this.playaroundprob, this.playaroundprob2);
                    }
                    else
                    {
                        //p.value = -10000;
                    }
                    //sf.Ai.enemyTurnSim.simulateEnemysTurn(p, true, this.playaround, false, this.playaroundprob, this.playaroundprob2);


                    this.threadresults[threadnumber].Add(p);

                }
            }

        }



        public void cuttingposibilities()
        {
            // take the x best values
            int takenumber = this.maxwide;
            List<Playfield> temp = new List<Playfield>();
            posmoves.Sort((a, b) => -(botBase.getPlayfieldValue(a)).CompareTo(botBase.getPlayfieldValue(b)));//want to keep the best

            if (this.useComparison)
            {
                int i = 0;
                int max = Math.Min(posmoves.Count, this.maxwide);

                Playfield p = null;
                Playfield pp = null;
                //foreach (Playfield p in posmoves)
                for (i = 0; i < max; i++)
                {
                    p = posmoves[i];
                    int hash = p.GetHashCode();
                    p.hashcode = hash;
                    bool found = false;
                    //foreach (Playfield pp in temp)
                    for (int j = 0; j < temp.Count; j++)
                    {
                        pp = temp[j];
                        if (pp.hashcode == p.hashcode)
                        {
                            if (pp.isEqualf(p))
                            {
                                found = true;
                                break;
                            }
                        }
                    }
                    if (!found) temp.Add(p);
                    //i++;
                    //if (i >= this.maxwide) break;

                }


            }
            else
            {
                temp.AddRange(posmoves);
            }
            posmoves.Clear();
            posmoves.AddRange(temp.GetRange(0, Math.Min(takenumber, temp.Count)));

            //twoturnfields!
            if (this.dirtyTwoTurnSim == 0) return;
            temp.Clear();
            temp.AddRange(this.twoturnfields);
            temp.AddRange(posmoves.GetRange(0, Math.Min(this.dirtyTwoTurnSim, posmoves.Count)));
            temp.Sort((a, b) => -(botBase.getPlayfieldValue(a)).CompareTo(botBase.getPlayfieldValue(b)));
            this.twoturnfields.Clear();

            if (this.useComparison)
            {
                int i = 0;
                int max = Math.Min(temp.Count, this.dirtyTwoTurnSim);

                Playfield p = null;
                Playfield pp = null;
                //foreach (Playfield p in posmoves)
                for (i = 0; i < max; i++)
                {
                    p = temp[i];
                    int hash = p.GetHashCode();
                    p.hashcode = hash;
                    bool found = false;
                    //foreach (Playfield pp in temp)
                    for (int j = 0; j < twoturnfields.Count; j++)
                    {
                        pp = twoturnfields[j];
                        if (pp.hashcode == p.hashcode)
                        {
                            if (pp.isEqualf(p))
                            {
                                found = true;
                                break;
                            }
                        }
                    }
                    if (!found) twoturnfields.Add(p);
                    //i++;
                    //if (i >= this.maxwide) break;

                }


            }






            //this.twoturnfields.AddRange(temp.GetRange(0, Math.Min(this.dirtyTwoTurnSim, temp.Count)));

            //sf.helpfunctions.ErrorLog(this.twoturnfields.Count + "");

            //posmoves.Clear();
            //posmoves.AddRange(Helpfunctions.TakeList(temp, takenumber));

        }

        public List<targett> cutAttackTargets(List<targett> oldlist, Playfield p, bool own)
        {
            List<targett> retvalues = new List<targett>();
            List<Minion> addedmins = new List<Minion>(8);

            bool priomins = false;
            List<targett> retvaluesPrio = new List<targett>();
            foreach (targett t in oldlist)
            {
                if ((own && t.target == 200) || (!own && t.target == 100))
                {
                    retvalues.Add(t);
                    continue;
                }
                if ((own && t.target >= 10 && t.target <= 19) || (!own && t.target >= 0 && t.target <= 9))
                {
                    Minion m = null;
                    if (own) m = p.enemyMinions[t.target - 10];
                    if (!own) m = p.ownMinions[t.target];
                    /*if (penman.priorityDatabase.ContainsKey(m.name))
                    {
                        //retvalues.Add(t);
                        retvaluesPrio.Add(t);
                        priomins = true;
                        //help.logg(m.name + " is added to targetlist");
                        continue;
                    }*/


                    bool goingtoadd = true;
                    List<Minion> temp = new List<Minion>(addedmins);
                    bool isSpecial = m.handcard.card.isSpecialMinion;
                    foreach (Minion mnn in temp)
                    {
                        // special minions are allowed to attack in silended and unsilenced state!
                        //help.logg(mnn.silenced + " " + m.silenced + " " + mnn.name + " " + m.name + " " + penman.specialMinions.ContainsKey(m.name));

                        bool otherisSpecial = mnn.handcard.card.isSpecialMinion;

                        if ((!isSpecial || (isSpecial && m.silenced)) && (!otherisSpecial || (otherisSpecial && mnn.silenced))) // both are not special, if they are the same, dont add
                        {
                            if (mnn.Angr == m.Angr && mnn.Hp == m.Hp && mnn.divineshild == m.divineshild && mnn.taunt == m.taunt && mnn.poisonous == m.poisonous) goingtoadd = false;
                            continue;
                        }

                        if (isSpecial == otherisSpecial && !m.silenced && !mnn.silenced) // same are special
                        {
                            if (m.name != mnn.name) // different name -> take it
                            {
                                continue;
                            }
                            // same name -> test whether they are equal
                            if (mnn.Angr == m.Angr && mnn.Hp == m.Hp && mnn.divineshild == m.divineshild && mnn.taunt == m.taunt && mnn.poisonous == m.poisonous) goingtoadd = false;
                            continue;
                        }

                    }

                    if (goingtoadd)
                    {
                        addedmins.Add(m);
                        retvalues.Add(t);
                        //help.logg(m.name + " " + m.id +" is added to targetlist");
                    }
                    else
                    {
                        //help.logg(m.name + " is not needed to attack");
                        continue;
                    }

                }
            }
            //help.logg("end targetcutting");
            if (priomins) return retvaluesPrio;

            return retvalues;
        }

        public void printPosmoves()
        {
            int i = 0;
            foreach (Playfield p in this.posmoves)
            {
                p.printBoard();
                i++;
                if (i >= 200) break;
            }
        }

    }

}