namespace HRSim
{
    using System.Collections.Generic;
    using System;

    public class Movegenerator
    {
        //Silverfish sf;
        PenalityManager pen = PenalityManager.Instance;

        private static Movegenerator instance;

        public static Movegenerator Instance
        {
            get
            {
                return instance ?? (instance = new Movegenerator());
            }
        }

        //public Movegenerator(Silverfish sf)
        //{
        //    this.sf = sf;
        //    pen = sf.PenalityManager;
        //}
        public CardDB.Card getChooseCard(CardDB.Card c, int choice)
        {
            if (choice == 1 && CardDB.Instance.choose1database.ContainsKey(c.name))
            {
                c = CardDB.Instance.getCardDataFromID(CardDB.Instance.choose1database[c.name]);
            }
            else if (choice == 2 && CardDB.Instance.choose2database.ContainsKey(c.name))
            {
                c = CardDB.Instance.getCardDataFromID(CardDB.Instance.choose2database[c.name]);
            }
            return c;
        }

        public void getMoveListForPlayfield(Playfield p, bool log)
        {
            bool own = p.isOwnTurn;
            Player mPlayer;
            int playerNumber = 1;
            if (own)
            {
                mPlayer = p.playerFirst;
                playerNumber = 1;
            }
            else
            {
                mPlayer = p.playerSecond;
                playerNumber = 2;
            }

            Action playedAction;          

            if (p.moveList.Count == 0) //if starting, generate move
            {
                //GameManager.Instance.moveCount++;
                p.moveList = new List<Action>(getMoveList(p, false, true, true));
                if (log)
                {
                    Helpfunctions.Instance.logg("######################start turn for player " + playerNumber);
                }
            }
            else //non starting, generate move from existing moves
            {
                playedAction = mPlayer.playactions[mPlayer.playactions.Count - 1];

                if (log)
                {
                    Helpfunctions.Instance.logg("Action:------------------------------------");
                    playedAction.print();
                    p.printMoveList();
                }

                if (p.moveTrigger.handcardAdded || p.moveTrigger.tauntChanged || p.moveTrigger.manaChanged ||
                    p.moveTrigger.ownNewTarget || p.moveTrigger.enemyNewTarget)
                {
                    p.moveList = getMoveList(p, false, true, true);
                }
                else 
                {
                    if (p.moveTrigger.minionDied)
                    {
                        foreach (int entityID in p.moveTrigger.minionDiedList)
                        {
                            p.moveList.RemoveAll(x => ((x.target != null && x.target.entitiyID == entityID) || ((x.actionType == actionEnum.attackWithMinion) && x.own.entitiyID == entityID)));
                        }
                    }
                    //Helpfunctions.Instance.logg("movecount == " + GameManager.Instance.moveCount);
                    if (playedAction.actionType == actionEnum.playcard) //play a card
                    {
                        p.moveList.RemoveAll(x => (x.card != null && x.card.entity == playedAction.card.entity));
                    }
                    if (playedAction.actionType == actionEnum.attackWithMinion)//
                    {
                        if (playedAction.own.entitiyID == 1008 && playedAction.target.entitiyID == 1003)
                        {
                            int debug = 1;
                        }
                        if ((playedAction.own.windfury && playedAction.own.numAttacksThisTurn == 2) || !playedAction.own.windfury)
                        {
                            p.moveList.RemoveAll(x => (x.actionType == actionEnum.attackWithMinion && x.own.entitiyID == playedAction.own.entitiyID));
                        }
                    }
                    if (playedAction.actionType == actionEnum.useHeroPower)
                    {
                        p.moveList.RemoveAll(x => x.actionType == actionEnum.useHeroPower);
                    }

                    //mana
                    p.moveList.RemoveAll(x => x.manaCost > mPlayer.mana);
                }
            }

            foreach (int m in p.moveTrigger.minionDiedList)
            {
                if (m == 1008)
                {
                    int debug = 1;
                }
            }
            p.moveTrigger.Clear();

            if (log)
            {
                if (p.isOwnTurn)
                {
                    Helpfunctions.Instance.logg("player 1 Mana: " + p.playerFirst.mana + "/" + p.playerFirst.ownMaxMana);
                }
                else
                {
                    Helpfunctions.Instance.logg("player 2 Mana: " + p.playerSecond.mana + "/" + p.playerSecond.ownMaxMana);
                }
                p.printMoveList();
            }
        }
        
        public List<Action> getMoveList(Playfield p, bool isLethalCheck, bool usePenalityManager, bool useCutingTargets)
        {
            bool own = p.isOwnTurn;
            Player mPlayer, ePlayer;
            if (own)
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }
            else {
                mPlayer = p.playerSecond;
                ePlayer = p.playerFirst;
            }

            //generates only own moves
            List<Action> ret = new List<Action>();
            List<Minion> trgts = new List<Minion>();

            if (p.complete || mPlayer.ownHero.Hp <= 0)
            {
                return ret;
            }

          //play cards:
            List<CardDB.cardName> playedcards = new List<CardDB.cardName>();

            foreach (Handmanager.Handcard hc in mPlayer.owncards)
            {
                CardDB.Card c = hc.card;
                //implementation
                if (c.name == CardDB.cardName.armorplating)
                {
                    p.moveTrigger.hasOwnTargetMove = true;
                }   
                //end of implementation
                if (playedcards.Contains(c.name) || !hc.canplayCard(p, own)) continue; // dont play the same card in one loop
                //playedcards.Add(c.name);

                int isChoice = (c.choice) ? 1 : 0;
                for (int i = 0 + 1 * isChoice; i < 1 + 2 * isChoice; i++)
                {
                    if (isChoice == 1) c = getChooseCard(hc.card, i); // do all choice

                    int manaCost = hc.getManaCost(p, own);
                    if (mPlayer.mana >= manaCost) // if enough manna
                    {
                        int cardplayPenality = 0;
                        int bestplace = p.getBestPlace(c, isLethalCheck, own);
                        //if (c.cardIDenum == CardDB.cardIDEnum.NEW1_005) {
                        //    int toBreak = 1;
                        //}
                        trgts = c.getTargetsForCard(p, isLethalCheck, own);
                        float bestReward = Single.MinValue;

                        List<Tuple<Action, float>> tempRet = new List<Tuple<Action, float>>();
                        foreach (Minion trgt in trgts)                        
                        {
                            if (usePenalityManager) cardplayPenality = pen.getPlayCardPenality(hc.card, trgt, p, i, isLethalCheck);
                            if (cardplayPenality <= 499)
                            {
                                Action a = new Action(actionEnum.playcard, hc, null, bestplace, trgt, cardplayPenality, i, manaCost); //i is the choice

                                ret.Add(a);
                                if (trgt != null && trgt.own == p.isOwnTurn) 
                                {
                                    p.moveTrigger.hasOwnTargetMove = true;
                                }
                                //if (trgt != null)
                                //{
                                //    if (trgt.isHero && trgt.Hp <= 12)
                                //    {
                                //        ret.Add(a);
                                //        continue;
                                //    }
                                //    float reward = pen.getOffenseReward(a, p);
                                //    tempRet.Add(new Tuple<Action, float>(a, reward));
                                //}
                                //else 
                                //{
                                //    ret.Add(a);
                                //}
                            }
                        }

                        //tempRet.Sort((x, y) => y.Item2.CompareTo(x.Item2));
                        //if (tempRet.Count > 0)
                        //{
                        //    ret.Add(tempRet[0].Item1);
                        //    if (tempRet[0].Item1.target.own == p.isOwnTurn) 
                        //    {
                        //        p.moveTrigger.hasOwnTargetMove = true;
                        //    }
                        //}
                        //if (tempRet.Count > 1)
                        //{
                        //    ret.Add(tempRet[1].Item1);
                        //    if (tempRet[1].Item1.target.own == p.isOwnTurn)
                        //    {
                        //        p.moveTrigger.hasOwnTargetMove = true;
                        //    }
                        //}
                    }
                }


            }

          //get targets for Hero weapon and Minions  ###################################################################################

            trgts = p.getAttackTargets(own, isLethalCheck);
            //if (!isLethalCheck) trgts = this.cutAttackList(trgts);

          // attack with minions
            List<Minion> attackingMinions = new List<Minion>(8);
            foreach (Minion m in mPlayer.ownMinions)
            {
                if (m.Ready && m.Angr >= 1 && !m.frozen) attackingMinions.Add(m); //* add non-attacing minions
            }
            //attackingMinions = this.cutAttackList(attackingMinions);

            foreach (Minion m in attackingMinions)
            {
                int attackPenality = 0;
                foreach (Minion trgt in trgts)
                {
                    if (usePenalityManager) attackPenality = pen.getAttackWithMininonPenality(m, p, trgt, isLethalCheck);
                    if (attackPenality <= 499)
                    {
                        Action a = new Action(actionEnum.attackWithMinion, null, m, 0, trgt, attackPenality, 0, 0);
                        ret.Add(a);
                    }
                }
            }

          // attack with hero (weapon)
            if (mPlayer.ownHero.Ready && mPlayer.ownHero.Angr >= 1)
            {
                int heroAttackPen = 0;
                foreach (Minion trgt in trgts)
                {
                    if (usePenalityManager) heroAttackPen = pen.getAttackWithHeroPenality(trgt, p, isLethalCheck);
                    if (heroAttackPen <= 499)
                    {
                        Action a = new Action(actionEnum.attackWithHero, null, mPlayer.ownHero, 0, trgt, heroAttackPen, 0, 0);
                        ret.Add(a);
                    }
                }
            }

           //#############################################################################################################

           // use ability
            if (mPlayer.ownAbilityReady && mPlayer.mana >= 2) // if ready and enough manna TODO: TGT mana cost change
            {
                int cardplayPenality = 0;
                int bestplace = mPlayer.ownMinions.Count + 1; //we can not manage it
                trgts = mPlayer.ownHeroAblility.card.getTargetsForCard(p, isLethalCheck, own);
                foreach (Minion trgt in trgts)
                {
                    if (usePenalityManager) cardplayPenality = pen.getPlayCardPenality(mPlayer.ownHeroAblility.card, trgt, p, 0, isLethalCheck);
                    if (cardplayPenality <= 499)
                    {
                        Action a = new Action(actionEnum.useHeroPower, mPlayer.ownHeroAblility, null, bestplace, trgt, cardplayPenality, 0, 2);
                        if (trgt.own == p.isOwnTurn)
                        {
                            //Helpfunctions.Instance.logg("ping on own minion");
                        }
                        if ((trgt.entitiyID == 0 && p.isOwnTurn) || (trgt.entitiyID == 1 && !p.isOwnTurn))
                        {
                            Helpfunctions.Instance.logg("ping on own hero");
                        }
                        ret.Add(a);
                    }
                }
            }

            return ret;
        }

        //implememtation
        public Action getRandomMove(Playfield p, bool isLethalCheck, bool usePenalityManager, bool useCutingTargets, bool own)
        {
            List<Action> actions = getMoveList(p, isLethalCheck, usePenalityManager, useCutingTargets);
            if (actions.Count > 0)
            {
                return actions[GameManager.getRNG().Next(actions.Count)];
            }
            return null;
        }

        //turndeep = progress of current players turn
        //public List<Action> getEnemyMoveList(Playfield p, bool isLethalCheck, bool usePenalityManager, bool useCutingTargets, int turndeep)
        //{
        //    //generates only own moves

        //    List<Action> ret = new List<Action>();

        //    if (p.complete || p.ownHero.Hp <= 0)
        //    {
        //        return ret;
        //    }


        //    //if he can use ability use it on his turnstart or never!###########################################################################################
        //    if (turndeep == 0 && p.enemyAbilityReady && p.mana >= 2 && p.enemyHeroAblility.card.canplayCard(p, 0) && !p.loatheb)
        //    {
        //        int abilityPenality = 0;

        //        // if we have mage or priest, we have to target something####################################################
        //        if (p.enemyHeroName == HeroEnum.mage || p.enemyHeroName == HeroEnum.priest)
        //        {
        //            List<Minion> trgts = p.enemyHeroAblility.card.getTargetsForCardEnemy(p);
        //            foreach (Minion trgt in trgts)
        //            {
        //                if (trgt.isHero) continue;//dont target hero
        //                Action a = new Action(actionEnum.useHeroPower, null, null, 0, trgt, abilityPenality, 0);
        //                //if (trgt.own == true) {
        //                //    sf.helpfunctions.logg("ping on own minion");
        //                //}
        //                ret.Add(a);
        //            }
        //        }
        //        else
        //        {
        //            // the other classes dont have to target####################################################
        //            Action a = new Action(actionEnum.useHeroPower, null, null, 0, null, abilityPenality, 0);
        //            ret.Add(a);
        //        }
        //        return ret;
        //    }


        //    // attack with minions ###############################################################################################################

        //    List<Minion> playedMinions = new List<Minion>(8);
        //    bool attackordermatters = this.didAttackOrderMatters(p);

        //    foreach (Minion m in p.enemyMinions)
        //    {

        //        if (m.Ready && m.Angr >= 1 && !m.frozen)
        //        {
        //            //BEGIN:cut (double/similar) attacking minions out#####################################
        //            // DONT LET SIMMILAR MINIONS ATTACK IN ONE TURN (example 3 unlesh the hounds-hounds doesnt need to simulated hole)
        //            if (attackordermatters)
        //            {
        //                List<Minion> tempoo = new List<Minion>(playedMinions);
        //                bool dontattacked = true;
        //                bool isSpecial = m.handcard.card.isSpecialMinion;
        //                foreach (Minion mnn in tempoo)
        //                {
        //                    // special minions are allowed to attack in silended and unsilenced state!
        //                    //help.logg(mnn.silenced + " " + m.silenced + " " + mnn.name + " " + m.name + " " + penman.specialMinions.ContainsKey(m.name));

        //                    bool otherisSpecial = mnn.handcard.card.isSpecialMinion;

        //                    if ((!isSpecial || (isSpecial && m.silenced)) && (!otherisSpecial || (otherisSpecial && mnn.silenced))) // both are not special, if they are the same, dont add
        //                    {
        //                        if (mnn.Angr == m.Angr && mnn.Hp == m.Hp && mnn.divineshild == m.divineshild && mnn.taunt == m.taunt && mnn.poisonous == m.poisonous) dontattacked = false;
        //                        continue;
        //                    }

        //                    if (isSpecial == otherisSpecial && !m.silenced && !mnn.silenced) // same are special
        //                    {
        //                        if (m.name != mnn.name) // different name -> take it
        //                        {
        //                            continue;
        //                        }
        //                        // same name -> test whether they are equal
        //                        if (mnn.Angr == m.Angr && mnn.Hp == m.Hp && mnn.divineshild == m.divineshild && mnn.taunt == m.taunt && mnn.poisonous == m.poisonous) dontattacked = false;
        //                        continue;
        //                    }

        //                }

        //                if (dontattacked)
        //                {
        //                    playedMinions.Add(m);
        //                }
        //                else
        //                {
        //                    //help.logg(m.name + " doesnt need to attack!");
        //                    continue;
        //                }
        //            }
        //            //END: cut (double/similar) attacking minions out#####################################

        //            //help.logg(m.name + " is going to attack!");
        //            List<Minion> trgts = p.getAttackTargets(false, isLethalCheck);

        //            if (useCutingTargets) trgts = this.cutAttackList(trgts);

        //            foreach (Minion trgt in trgts)
        //            {
        //                Action a = new Action(actionEnum.attackWithMinion, null, m, 0, trgt, this.pen.getAttackWithMininonPenality(m, p, trgt, false), 0);
        //                ret.Add(a);
        //            }


        //            if ((!m.stealth) && trgts.Count == 1 && trgts[0].isHero)//only enemy hero is available als attack
        //            {
        //                break;
        //            }
        //            if (!attackordermatters) break;
        //        }


        //    }


        //    // attack with hero
        //    if (p.enemyHero.Ready && p.enemyHero.Angr >= 1)
        //    {
        //        List<Minion> trgts = p.getAttackTargets(false, isLethalCheck);

        //        if (useCutingTargets) trgts = this.cutAttackList(trgts);

        //        foreach (Minion trgt in trgts)
        //        {
        //            //pf.attackWithWeapon(trgt.target, trgt.targetEntity, heroAttackPen);
        //            Action a = new Action(actionEnum.attackWithHero, null, p.enemyHero, 0, trgt, 0, 0);
        //            ret.Add(a);
        //        }
        //    }



        //    return ret;
        //}


        public List<Minion> cutAttackList(List<Minion> oldlist)
        {
            List<Minion> retvalues = new List<Minion>(oldlist.Count);
            List<Minion> addedmins = new List<Minion>(oldlist.Count);

            foreach (Minion m in oldlist)
            {
                if (m.isHero)
                {
                    retvalues.Add(m);
                    continue;
                }

                bool goingtoadd = true;
                bool isSpecial = m.handcard.card.isSpecialMinion;
                foreach (Minion mnn in addedmins)
                {
                    //help.logg(mnn.silenced + " " + m.silenced + " " + mnn.name + " " + m.name + " " + penman.specialMinions.ContainsKey(m.name));

                    bool otherisSpecial = mnn.handcard.card.isSpecialMinion;
			        bool onlySpecial = isSpecial && otherisSpecial && !m.silenced && !mnn.silenced;
			        bool onlyNotSpecial =(!isSpecial || (isSpecial && m.silenced)) && (!otherisSpecial || (otherisSpecial && mnn.silenced));
			
			        if(onlySpecial && (m.name != mnn.name)) continue; // different name -> take it
			        if ((onlySpecial || onlyNotSpecial) && (mnn.Angr == m.Angr && mnn.Hp == m.Hp && mnn.divineshild == m.divineshild && mnn.taunt == m.taunt && mnn.poisonous == m.poisonous && m.handcard.card.isToken == mnn.handcard.card.isToken && mnn.handcard.card.race == m.handcard.card.race))
                    {
				        goingtoadd = false;
				        break;
                    }
                }

                if (goingtoadd)
                {
                    addedmins.Add(m);
                    retvalues.Add(m);
                    //help.logg(m.name + " " + m.id +" is added to targetlist");
                }
                else
                {
                    //help.logg(m.name + " is not needed to attack");
                    continue;
                }
            }
            //help.logg("end targetcutting");

            return retvalues;
        }


        public bool didAttackOrderMatters(Playfield p, bool own)
        {
            Player mPlayer, ePlayer;

            if (own) {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }else{
                mPlayer = p.playerSecond;
                ePlayer = p.playerFirst;
            }

            //return true;
            if (p.isOwnTurn)
            {
                if (ePlayer.ownSecretsIDList.Count >= 1) return true;
                if (ePlayer.ownHero.immune) return true;
            }            

            int strongestAttack = 0;
            foreach (Minion m in ePlayer.ownMinions)
            {
                if (m.Angr > strongestAttack) strongestAttack = m.Angr;
                if (m.taunt) return true;
                if (m.name == CardDB.cardName.dancingswords || m.name == CardDB.cardName.deathlord) return true;
            }

            int haspets = 0;
            bool hashyena = false;
            bool hasJuggler = false;
            bool spawnminions = false;
            foreach (Minion m in mPlayer.ownMinions)
            {
                if (m.name == CardDB.cardName.cultmaster) return true;
                if (m.name == CardDB.cardName.knifejuggler) hasJuggler = true;
                if (m.Ready && m.Angr >= 1)
                {
                    if (m.AdjacentAngr >= 1) return true;//wolphalfa or flametongue is in play
                    if (m.name == CardDB.cardName.northshirecleric) return true;
                    if (m.name == CardDB.cardName.armorsmith) return true;
                    if (m.name == CardDB.cardName.loothoarder) return true;
                    //if (m.name == CardDB.cardName.madscientist) return true; // dont change the tactic
                    if (m.name == CardDB.cardName.sylvanaswindrunner) return true;
                    if (m.name == CardDB.cardName.darkcultist) return true;
                    if (m.ownBlessingOfWisdom >= 1) return true;
                    if (m.name == CardDB.cardName.acolyteofpain) return true;
                    if (m.name == CardDB.cardName.frothingberserker) return true;
                    if (m.name == CardDB.cardName.flesheatingghoul) return true;
                    if (m.name == CardDB.cardName.bloodmagethalnos) return true;
                    if (m.name == CardDB.cardName.webspinner) return true;
                    if (m.name == CardDB.cardName.tirionfordring) return true;
                    if (m.name == CardDB.cardName.baronrivendare) return true;


                    //if (m.name == CardDB.cardName.manawraith) return true;
                    //buffing minions (attack with them last)
                    if (m.name == CardDB.cardName.raidleader || m.name == CardDB.cardName.stormwindchampion || m.name == CardDB.cardName.timberwolf || m.name == CardDB.cardName.southseacaptain || m.name == CardDB.cardName.murlocwarleader || m.name == CardDB.cardName.grimscaleoracle || m.name == CardDB.cardName.leokk) return true;


                    if (m.name == CardDB.cardName.scavenginghyena) hashyena = true;
                    if (m.handcard.card.race == 20) haspets++;
                    if (m.name == CardDB.cardName.harvestgolem || m.name == CardDB.cardName.hauntedcreeper || m.souloftheforest >= 1 || m.ancestralspirit >= 1 || m.name == CardDB.cardName.nerubianegg || m.name == CardDB.cardName.savannahhighmane || m.name == CardDB.cardName.sludgebelcher || m.name == CardDB.cardName.cairnebloodhoof || m.name == CardDB.cardName.feugen || m.name == CardDB.cardName.stalagg || m.name == CardDB.cardName.thebeast) spawnminions = true;

                }
            }

            if (haspets >= 1 && hashyena) return true;
            if (hasJuggler && spawnminions) return true;




            return false;
        }
    }

}