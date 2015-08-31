namespace HREngine.Bots
{
    using System.Collections.Generic;

    public class Movegenerator
    {
        PenalityManager pen = PenalityManager.Instance;

        private static Movegenerator instance;

        public static Movegenerator Instance
        {
            get
            {
                return instance ?? (instance = new Movegenerator());
            }
        }

        private Movegenerator()
        {
        }

        
        public List<Action> getMoveList(Playfield p, bool isLethalCheck, bool usePenalityManager, bool useCutingTargets)
        {
            //generates only own moves
            List<Action> ret = new List<Action>();
            List<Minion> trgts = new List<Minion>();

            if (p.complete || p.ownHero.Hp <= 0)
            {
                return ret;
            }

          //play cards:
            List<CardDB.cardName> playedcards = new List<CardDB.cardName>();

            foreach (Handmanager.Handcard hc in p.owncards)
            {
                CardDB.Card c = hc.card;
                if (playedcards.Contains(c.name) || !hc.canplayCard(p)) continue; // dont play the same card in one loop
                playedcards.Add(c.name);

                int isChoice = (c.choice) ? 1 : 0;
                for (int i = 0 + 1 * isChoice; i < 1 + 2 * isChoice; i++)
                {
                    if (isChoice == 1) c = PenalityManager.Instance.getChooseCard(hc.card, i); // do all choice

                    if (p.mana >= c.getManaCost(p, hc.manacost)) // if enough manna
                    {
                        int cardplayPenality = 0;
                        int bestplace = p.getBestPlace(c, isLethalCheck);
                        trgts = c.getTargetsForCard(p, isLethalCheck);
                        foreach (Minion trgt in trgts)
                        {
                            if (usePenalityManager) cardplayPenality = pen.getPlayCardPenality(hc.card, trgt, p, i, isLethalCheck);
                            if (cardplayPenality <= 499)
                            {
                                Action a = new Action(actionEnum.playcard, hc, null, bestplace, trgt, cardplayPenality, i); //i is the choice
                                ret.Add(a);
                            }
                        }
                    }
                }
            }

          //get targets for Hero weapon and Minions  ###################################################################################

            trgts = p.getAttackTargets(true, isLethalCheck);
            if (!isLethalCheck) trgts = this.cutAttackList(trgts);

          // attack with minions
            List<Minion> attackingMinions = new List<Minion>(8);
            foreach (Minion m in p.ownMinions)
            {
                if (m.Ready && m.Angr >= 1 && !m.frozen) attackingMinions.Add(m); //* add non-attacing minions
            }
            attackingMinions = this.cutAttackList(attackingMinions);

            foreach (Minion m in attackingMinions)
            {
                int attackPenality = 0;
                foreach (Minion trgt in trgts)
                {
                    if (usePenalityManager) attackPenality = pen.getAttackWithMininonPenality(m, p, trgt, isLethalCheck);
                    if (attackPenality <= 499)
                    {
                        Action a = new Action(actionEnum.attackWithMinion, null, m, 0, trgt, attackPenality, 0);
                        ret.Add(a);
                    }
                }
            }

          // attack with hero (weapon)
            if (p.ownHero.Ready && p.ownHero.Angr >= 1)
            {
                int heroAttackPen = 0;
                foreach (Minion trgt in trgts)
                {
                    if (usePenalityManager) heroAttackPen = pen.getAttackWithHeroPenality(trgt, p, isLethalCheck);
                    if (heroAttackPen <= 499)
                    {
                        Action a = new Action(actionEnum.attackWithHero, null, p.ownHero, 0, trgt, heroAttackPen, 0);
                        ret.Add(a);
                    }
                }
            }

           //#############################################################################################################

           // use ability
            if (p.ownAbilityReady && p.mana >= p.ownHeroAblility.card.getManaCost(p, 2)) // if ready and enough manna
            {
                int cardplayPenality = 0;
                int bestplace = p.ownMinions.Count + 1; //we can not manage it
                trgts = p.ownHeroAblility.card.getTargetsForCard(p, isLethalCheck);
                foreach (Minion trgt in trgts)
                {
                    if (usePenalityManager) cardplayPenality = pen.getPlayCardPenality(p.ownHeroAblility.card, trgt, p, 0, isLethalCheck);
                    if (cardplayPenality <= 499)
                    {
                        Action a = new Action(actionEnum.useHeroPower, p.ownHeroAblility, null, bestplace, trgt, cardplayPenality, 0);
                        ret.Add(a);
                    }
                }
            }

            return ret;
        }

        //turndeep = progress of current players turn
        public List<Action> getEnemyMoveList(Playfield p, bool isLethalCheck, bool usePenalityManager, bool useCutingTargets, int turndeep)
        {
            //generates only own moves

            List<Action> ret = new List<Action>();

            if (p.complete || p.ownHero.Hp <= 0)
            {
                return ret;
            }


            //if he can use ability use it on his turnstart or never!###########################################################################################
            if (turndeep == 0 && p.enemyAbilityReady && p.mana >= 2 && p.enemyHeroAblility.card.canplayCard(p, 0) && !p.loatheb)
            {
                int abilityPenality = 0;

                // if we have mage or priest, we have to target something####################################################
                if (p.enemyHeroName == HeroEnum.mage || p.enemyHeroName == HeroEnum.priest)
                {
                    List<Minion> trgts = p.enemyHeroAblility.card.getTargetsForCardEnemy(p);
                    foreach (Minion trgt in trgts)
                    {
                        if (trgt.isHero) continue;//dont target hero
                        Action a = new Action(actionEnum.useHeroPower, null, null, 0, trgt, abilityPenality, 0);
                        ret.Add(a);
                    }
                }
                else
                {
                    // the other classes dont have to target####################################################
                    Action a = new Action(actionEnum.useHeroPower, null, null, 0, null, abilityPenality, 0);
                    ret.Add(a);
                }
                return ret;
            }


            // attack with minions ###############################################################################################################

            List<Minion> playedMinions = new List<Minion>(8);
            bool attackordermatters = this.didAttackOrderMatters(p);

            foreach (Minion m in p.enemyMinions)
            {

                if (m.Ready && m.Angr >= 1 && !m.frozen)
                {
                    //BEGIN:cut (double/similar) attacking minions out#####################################
                    // DONT LET SIMMILAR MINIONS ATTACK IN ONE TURN (example 3 unlesh the hounds-hounds doesnt need to simulated hole)
                    if (attackordermatters)
                    {
                        List<Minion> tempoo = new List<Minion>(playedMinions);
                        bool dontattacked = true;
                        bool isSpecial = m.handcard.card.isSpecialMinion;
                        foreach (Minion mnn in tempoo)
                        {
                            // special minions are allowed to attack in silended and unsilenced state!
                            //help.logg(mnn.silenced + " " + m.silenced + " " + mnn.name + " " + m.name + " " + penman.specialMinions.ContainsKey(m.name));

                            bool otherisSpecial = mnn.handcard.card.isSpecialMinion;

                            if ((!isSpecial || (isSpecial && m.silenced)) && (!otherisSpecial || (otherisSpecial && mnn.silenced))) // both are not special, if they are the same, dont add
                            {
                                if (mnn.Angr == m.Angr && mnn.Hp == m.Hp && mnn.divineshild == m.divineshild && mnn.taunt == m.taunt && mnn.poisonous == m.poisonous) dontattacked = false;
                                continue;
                            }

                            if (isSpecial == otherisSpecial && !m.silenced && !mnn.silenced) // same are special
                            {
                                if (m.name != mnn.name) // different name -> take it
                                {
                                    continue;
                                }
                                // same name -> test whether they are equal
                                if (mnn.Angr == m.Angr && mnn.Hp == m.Hp && mnn.divineshild == m.divineshild && mnn.taunt == m.taunt && mnn.poisonous == m.poisonous) dontattacked = false;
                                continue;
                            }

                        }

                        if (dontattacked)
                        {
                            playedMinions.Add(m);
                        }
                        else
                        {
                            //help.logg(m.name + " doesnt need to attack!");
                            continue;
                        }
                    }
                    //END: cut (double/similar) attacking minions out#####################################

                    //help.logg(m.name + " is going to attack!");
                    List<Minion> trgts = p.getAttackTargets(false, isLethalCheck);

                    if (useCutingTargets) trgts = this.cutAttackList(trgts);

                    foreach (Minion trgt in trgts)
                    {
                        Action a = new Action(actionEnum.attackWithMinion, null, m, 0, trgt, this.pen.getAttackWithMininonPenality(m, p, trgt, false), 0);
                        ret.Add(a);
                    }


                    if ((!m.stealth) && trgts.Count == 1 && trgts[0].isHero)//only enemy hero is available als attack
                    {
                        break;
                    }
                    if (!attackordermatters) break;
                }


            }


            // attack with hero
            if (p.enemyHero.Ready && p.enemyHero.Angr >= 1)
            {
                List<Minion> trgts = p.getAttackTargets(false, isLethalCheck);

                if (useCutingTargets) trgts = this.cutAttackList(trgts);

                foreach (Minion trgt in trgts)
                {
                    //pf.attackWithWeapon(trgt.target, trgt.targetEntity, heroAttackPen);
                    Action a = new Action(actionEnum.attackWithHero, null, p.enemyHero, 0, trgt, 0, 0);
                    ret.Add(a);
                }
            }



            return ret;
        }


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


        public bool didAttackOrderMatters(Playfield p)
        {
            //return true;
            if (p.isOwnTurn)
            {
                if (p.enemySecretCount >= 1) return true;
                if (p.enemyHero.immune) return true;

            }
            else
            {
                if (p.ownHero.immune) return true;
            }
            List<Minion> enemym = (p.isOwnTurn) ? p.enemyMinions : p.ownMinions;
            List<Minion> ownm = (p.isOwnTurn) ? p.ownMinions : p.enemyMinions;

            int strongestAttack = 0;
            foreach (Minion m in enemym)
            {
                if (m.Angr > strongestAttack) strongestAttack = m.Angr;
                if (m.taunt) return true;
                if (m.name == CardDB.cardName.dancingswords || m.name == CardDB.cardName.deathlord) return true;
            }

            int haspets = 0;
            bool hashyena = false;
            bool hasJuggler = false;
            bool spawnminions = false;
            foreach (Minion m in ownm)
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