namespace HRSim
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;

    public class Movegenerator
    {
        //Silverfish sf;
        PenalityManager pen = PenalityManager.Instance;
        private double minionProbCut = 0.0;

        private static Movegenerator instance;

        public static Movegenerator Instance
        {
            get
            {
                return instance ?? (instance = new Movegenerator());
            }
        }

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

        public List<Action> GetNonPlaycardActions(Playfield p, bool isLethalCheck, bool usePenalityManager)
        {
            Player mPlayer = p.getCurrentPlayer(true);
            List<Action> ret = new List<Action>();

            List<Minion> trgts = p.getAttackTargets(p.isOwnTurn, isLethalCheck);
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
                    if (trgt != null && !trgt.isHero && trgt.own != p.isOwnTurn && trgt.targetProb < minionProbCut) continue;
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
                    if (trgt != null && !trgt.isHero && trgt.own != p.isOwnTurn && trgt.targetProb < minionProbCut) continue;
                    if (usePenalityManager) heroAttackPen = pen.getAttackWithHeroPenality(trgt, p, isLethalCheck);
                    if (heroAttackPen <= 499)
                    {
                        Action a = new Action(actionEnum.attackWithHero, null, mPlayer.ownHero, 0, trgt, heroAttackPen, 0, 0);
                        ret.Add(a);
                    }
                }
            }

            return ret;
        }

        public List<Action> GetActionsForCard(Playfield p, Handmanager.Handcard hc, bool isLethalCheck, bool usePenalityManager)
        {
            Player mPlayer = p.getCurrentPlayer(true);
            CardDB.Card c = hc.card;
            List<Minion> trgts = new List<Minion>();
            List<Action> ret = new List<Action>();

            int isChoice = (c.choice) ? 1 : 0;
            for (int i = 0 + 1 * isChoice; i < 1 + 2 * isChoice; i++)
            {
                if (isChoice == 1) c = getChooseCard(hc.card, i); // do all choice

                int manaCost = hc.getManaCost(p, p.isOwnTurn);
                if (mPlayer.mana >= manaCost) // if enough manna
                {
                    int cardplayPenality = 0;
                    int bestplace = p.getBestPlace(c, isLethalCheck, p.isOwnTurn);
                    trgts = c.getTargetsForCard(p, isLethalCheck, p.isOwnTurn);
                    foreach (Minion trgt in trgts)
                    {
                        if (trgt != null && !trgt.isHero && trgt.own != p.isOwnTurn && trgt.targetProb < minionProbCut) continue;
                        if (usePenalityManager) cardplayPenality = pen.getPlayCardPenality(hc.card, trgt, p, i, isLethalCheck);
                        if (cardplayPenality <= 499)
                        {
                            Action a = new Action(actionEnum.playcard, hc, null, bestplace, trgt, cardplayPenality, i, manaCost); //i is the choice

                            ret.Add(a);
                            if (trgt != null && trgt.own == p.isOwnTurn)
                            {
                                p.moveTrigger.hasOwnTargetMove = true;
                            }
                        }
                    }
                }
            }
            return ret;
        }

        public bool CanUseHeroPower(Playfield pf)
        {
            Player mPlayer = pf.getCurrentPlayer(true);
            return mPlayer.ownAbilityReady && mPlayer.mana >= 2;
        }

        public List<List<Action>> getMoveListByAction(Playfield p, bool isLethalCheck, bool usePenalityManager)
        {
            Player mPlayer = p.getCurrentPlayer(true);
            List<List<Action>> moveListByAction = new List<List<Action>>();
            List<Minion> trgts;

            foreach (Handmanager.Handcard hc in mPlayer.owncards)
            {
                if (!hc.canplayCard(p, p.isOwnTurn)) continue; // dont play the same card in one loop
                CardDB.Card c = hc.card;

                List<Action> actions = new List<Action>();
                int manaCost = hc.getManaCost(p, p.isOwnTurn);
                if (mPlayer.mana >= manaCost)
                {
                    int cardplayPenality = 0;
                    int bestplace = p.getBestPlace(c, isLethalCheck, p.isOwnTurn);
                    trgts = c.getTargetsForCard(p, isLethalCheck, p.isOwnTurn);

                    foreach (Minion trgt in trgts)
                    {
                        if (trgt != null && !trgt.isHero && trgt.own != p.isOwnTurn && trgt.targetProb < minionProbCut) continue;
                        if (usePenalityManager) cardplayPenality = pen.getPlayCardPenality(hc.card, trgt, p, 0, isLethalCheck);
                        if (cardplayPenality <= 499)
                        {
                            Action a = new Action(actionEnum.playcard, hc, null, bestplace, trgt, cardplayPenality, 0, manaCost); //i is the choice
                            actions.Add(a);
                            if (trgt != null && trgt.own == p.isOwnTurn)
                            {
                                p.moveTrigger.hasOwnTargetMove = true;
                            }
                        }
                    }
                    if (actions.Count > 0) moveListByAction.Add(actions);
                }
            }


            if (mPlayer.ownAbilityReady && mPlayer.mana >= 2) // if ready and enough manna TODO: TGT mana cost change
            {
                List<Action> actions = new List<Action>();
                int cardplayPenality = 0;
                int bestplace = mPlayer.ownMinions.Count + 1; //we can not manage it
                trgts = mPlayer.ownHeroAblility.card.getTargetsForCard(p, isLethalCheck, p.isOwnTurn);
                foreach (Minion trgt in trgts)
                {
                    if (trgt != null && !trgt.isHero && trgt.own != p.isOwnTurn && trgt.targetProb < minionProbCut) continue;
                    if (usePenalityManager) cardplayPenality = pen.getPlayCardPenality(mPlayer.ownHeroAblility.card, trgt, p, 0, isLethalCheck);
                    if (cardplayPenality <= 499)
                    {
                        Action a = new Action(actionEnum.useHeroPower, mPlayer.ownHeroAblility, null, bestplace, trgt, cardplayPenality, 0, 2);
                        actions.Add(a);
                    }
                }
                if (actions.Count > 0) moveListByAction.Add(actions);
            }

            return moveListByAction;
        }

        public void getMoveListForPlayfield(Playfield p, bool log, bool lethalCheck, double cardProbCut)
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
                p.moveList = new List<Action>(getMoveList(p, lethalCheck, true, true, 0.0));
                if (log)
                {
                    Helpfunctions.Instance.logg("######################start turn for player " + playerNumber);
                }
            }
            else if (p.moveTrigger.keepCard != -10000) //keep a card
            {
                p.moveList.RemoveAll(x => (x.card != null && x.card.entity == p.moveTrigger.keepCard));
                p.moveTrigger.Clear();
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
                    p.moveList = getMoveList(p, lethalCheck, true, true, cardProbCut);
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

        public List<Action> getMoveListSF(Playfield p, bool isLethalCheck, bool usePenalityManager, bool useCutingTargets, double cardProbCut)
        {
            bool own = p.isOwnTurn;
            Player mPlayer, ePlayer;
            if (own)
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }
            else
            {
                mPlayer = p.playerSecond;
                ePlayer = p.playerFirst;
            }

            //double cardProbCut = 0.0;
            //double minionProbCut = 1.0 / ePlayer.ownMinions.Count;

            //generates only own moves
            List<Action> ret = new List<Action>();
            List<Minion> trgts = new List<Minion>();

            if (mPlayer.ownHero.Hp <= 0)
            {
                return ret;
            }

            //play cards:
            List<CardDB.cardName> playedcards = new List<CardDB.cardName>();

            foreach (Handmanager.Handcard hc in mPlayer.owncards)
            {
                if (p.keepCardList.Contains(hc.entity))
                    continue;
                CardDB.Card c = hc.card;
                //implementation
                if (c.name == CardDB.cardName.armorplating)
                {
                    p.moveTrigger.hasOwnTargetMove = true;
                }
                //end of implementation
                if (playedcards.Contains(c.name) || !hc.canplayCard(p, own) || hc.playProb < cardProbCut) continue; // dont play the same card in one loop
                playedcards.Add(c.name);

                int isChoice = (c.choice) ? 1 : 0;
                for (int i = 0 + 1 * isChoice; i < 1 + 2 * isChoice; i++)
                {
                    if (isChoice == 1) c = getChooseCard(hc.card, i); // do all choice

                    int manaCost = hc.getManaCost(p, own);
                    if (mPlayer.mana >= manaCost) // if enough manna
                    {
                        int cardplayPenality = 0;
                        int bestplace = p.getBestPlace(c, isLethalCheck, own);
                        trgts = c.getTargetsForCard(p, isLethalCheck, own);
                        foreach (Minion trgt in trgts)
                        {
                            if (trgt != null && !trgt.isHero && trgt.own != own && trgt.targetProb < minionProbCut) continue;
                            if (usePenalityManager) cardplayPenality = pen.getPlayCardPenality(hc.card, trgt, p, i, isLethalCheck);
                            if (cardplayPenality <= 499)
                            {
                                Action a = new Action(actionEnum.playcard, hc, null, bestplace, trgt, cardplayPenality, i, manaCost); //i is the choice

                                ret.Add(a);
                                if (trgt != null && trgt.own == p.isOwnTurn)
                                {
                                    p.moveTrigger.hasOwnTargetMove = true;
                                }                                
                            }
                        }
                    }
                }


            }

            //get targets for Hero weapon and Minions  ###################################################################################

            trgts = p.getAttackTargets(own, isLethalCheck);
            if (!isLethalCheck) trgts = this.cutAttackList(trgts);

            // attack with minions
            List<Minion> attackingMinions = new List<Minion>(8);
            foreach (Minion m in mPlayer.ownMinions)
            {
                if (m.Ready && m.Angr >= 1 && !m.frozen) attackingMinions.Add(m); //* add non-attacing minions
            }
            attackingMinions = this.cutAttackList(attackingMinions);

            foreach (Minion m in attackingMinions)
            {
                int attackPenality = 0;
                foreach (Minion trgt in trgts)
                {
                    if (trgt != null && !trgt.isHero && trgt.own != own && trgt.targetProb < minionProbCut) continue;
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
                    if (trgt != null && !trgt.isHero && trgt.own != own && trgt.targetProb < minionProbCut) continue;
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
                    if (trgt != null && !trgt.isHero && trgt.own != own && trgt.targetProb < minionProbCut) continue;
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

        public List<Action> getNoneTargetMove(Playfield p, double cardProbCut)
        {
            bool own = p.isOwnTurn;
            bool isLethalCheck = false;
            bool usePenalityManager = true;
            Player mPlayer, ePlayer;
            if (own)
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }
            else
            {
                mPlayer = p.playerSecond;
                ePlayer = p.playerFirst;
            }

            List<CardDB.cardName> playedcards = new List<CardDB.cardName>();
            List<Minion> trgts = new List<Minion>();
            List<Action> ret = new List<Action>();

            int idx = 0;
            foreach (Handmanager.Handcard hc in mPlayer.owncards)
            {
                if (p.keepCardList.Contains(hc.entity))
                    continue;
                CardDB.Card c = hc.card;

                //implementation
                if (c.name == CardDB.cardName.armorplating)
                {
                    p.moveTrigger.hasOwnTargetMove = true;
                }
                //end of implementation

                if (playedcards.Contains(c.name) || !hc.canplayCard(p, own) || hc.playProb < cardProbCut) continue; // dont play the same card in one loop
                playedcards.Add(c.name);

                int isChoice = (c.choice) ? 1 : 0;
                for (int i = 0 + 1 * isChoice; i < 1 + 2 * isChoice; i++)
                {
                    if (isChoice == 1) c = getChooseCard(hc.card, i); // do all choice

                    int manaCost = hc.getManaCost(p, own);
                    if (mPlayer.mana >= manaCost) // if enough manna
                    {
                        int cardplayPenality = 0;
                        int bestplace = p.getBestPlace(c, isLethalCheck, own);

                        trgts = c.getTargetsForCard(p, isLethalCheck, own);
                        if (trgts.Count == 1 && trgts[0] == null)
                        {

                            if (c.name != CardDB.cardName.flamestrike)
                            {
                                if (usePenalityManager) cardplayPenality = pen.getPlayCardPenality(hc.card, null, p, i, isLethalCheck);
                                if (cardplayPenality <= 499)
                                {
                                    Action a = new Action(actionEnum.playcard, hc, null, bestplace, null, cardplayPenality, i, manaCost); //i is the choice
                                    ret.Add(a);
                                }
                            }
                        }
                    }
                }
            }

            isLethalCheck = true;
            trgts = p.getAttackTargets(own, isLethalCheck);

            // attack with minions
            List<Minion> attackingMinions = new List<Minion>(8);
            foreach (Minion m in mPlayer.ownMinions)
            {
                if (m.Ready && m.Angr >= 1 && !m.frozen) attackingMinions.Add(m); //* add non-attacing minions
            }

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
                        ret.Add(a);
                    }
                }
            }

            return ret;
        }

        public List<Action> getMoveForTarget(Playfield p, Minion target, double cardProbCut)
        {
            bool own = p.isOwnTurn;
            bool isLethalCheck = false;
            bool usePenalityManager = true;
            Player mPlayer, ePlayer;
            if (own)
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }
            else
            {
                mPlayer = p.playerSecond;
                ePlayer = p.playerFirst;
            }

            List<CardDB.cardName> playedcards = new List<CardDB.cardName>();
            List<Minion> trgts = new List<Minion>();
            List<int> trgtsEntity = new List<int>();
            List<Action> ret = new List<Action>();

            int idx = 0;
            foreach (Handmanager.Handcard hc in mPlayer.owncards)
            {
                if (p.keepCardList.Contains(hc.entity))
                    continue;
                CardDB.Card c = hc.card;

                //implementation
                if (c.name == CardDB.cardName.armorplating)
                {
                    p.moveTrigger.hasOwnTargetMove = true;
                }
                //end of implementation

                if (playedcards.Contains(c.name) || !hc.canplayCard(p, own) || hc.playProb < cardProbCut) continue; // dont play the same card in one loop
                playedcards.Add(c.name);

                int isChoice = (c.choice) ? 1 : 0;
                for (int i = 0 + 1 * isChoice; i < 1 + 2 * isChoice; i++)
                {
                    if (isChoice == 1) c = getChooseCard(hc.card, i); // do all choice

                    int manaCost = hc.getManaCost(p, own);
                    if (mPlayer.mana >= manaCost) // if enough manna
                    {
                        int cardplayPenality = 0;
                        int bestplace = p.getBestPlace(c, isLethalCheck, own);

                        trgtsEntity.Clear();
                        trgts = c.getTargetsForCard(p, isLethalCheck, own);

                        foreach (Minion m in trgts)
                        {
                            if (m != null)
                                trgtsEntity.Add(m.entitiyID);
                        }
                        if (c.name == CardDB.cardName.flamestrike || trgtsEntity.Contains(target.entitiyID))
                        {
                            if (usePenalityManager) cardplayPenality = pen.getPlayCardPenality(hc.card, target, p, i, isLethalCheck);
                            if (cardplayPenality <= 499)
                            {
                                Action a = new Action(actionEnum.playcard, hc, null, bestplace, target, cardplayPenality, i, manaCost); //i is the choice

                                ret.Add(a);
                                if (target != null && target.own == p.isOwnTurn)
                                {
                                    p.moveTrigger.hasOwnTargetMove = true;
                                }
                            }
                        }
                    }
                }
                idx++;
            }

            trgtsEntity.Clear();
            trgts = p.getAttackTargets(own, isLethalCheck);
            foreach (Minion m in trgts)
            {
                if (m != null)
                    trgtsEntity.Add(m.entitiyID);
            }
            if (trgtsEntity.Contains(target.entitiyID))
            {

                List<Minion> attackingMinions = new List<Minion>(8);
                foreach (Minion m in mPlayer.ownMinions)
                {
                    if (m.Ready && m.Angr >= 1 && !m.frozen) attackingMinions.Add(m); //* add non-attacing minions
                }

                foreach (Minion m in attackingMinions)
                {
                    int attackPenality = 0;

                    if (usePenalityManager) attackPenality = pen.getAttackWithMininonPenality(m, p, target, isLethalCheck);
                    if (attackPenality <= 499)
                    {
                        Action a = new Action(actionEnum.attackWithMinion, null, m, 0, target, attackPenality, 0, 0);
                        ret.Add(a);
                    }
                }

                if (mPlayer.ownHero.Ready && mPlayer.ownHero.Angr >= 1)
                {
                    int heroAttackPen = 0;
                    if (usePenalityManager) heroAttackPen = pen.getAttackWithHeroPenality(target, p, isLethalCheck);
                    if (heroAttackPen <= 499)
                    {
                        Action a = new Action(actionEnum.attackWithHero, null, mPlayer.ownHero, 0, target, heroAttackPen, 0, 0);
                        ret.Add(a);
                    }
                }
            }

            if (mPlayer.ownAbilityReady && mPlayer.mana >= 2) // if ready and enough manna TODO: TGT mana cost change
            {
                int cardplayPenality = 0;
                int bestplace = mPlayer.ownMinions.Count + 1; //we can not manage it
                trgts = mPlayer.ownHeroAblility.card.getTargetsForCard(p, isLethalCheck, own);

                trgtsEntity.Clear();
                foreach (Minion m in trgts)
                {
                    if (m != null)
                        trgtsEntity.Add(m.entitiyID);
                }

                if (trgtsEntity.Contains(target.entitiyID))
                {
                    if (usePenalityManager) cardplayPenality = pen.getPlayCardPenality(mPlayer.ownHeroAblility.card, target, p, 0, isLethalCheck);
                    if (cardplayPenality <= 499)
                    {
                        Action a = new Action(actionEnum.useHeroPower, mPlayer.ownHeroAblility, null, bestplace, target, cardplayPenality, 0, 2);
                        ret.Add(a);
                    }
                }
            }

            return ret;

        }

        public List<Action> getPlaycardMoveList(Playfield p, bool isLethalCheck, bool usePenalityManager, bool useCutingTargets, double cardProbCut)
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

            //double cardProbCut = 0.0;
            //double minionProbCut = 1.0 / ePlayer.ownMinions.Count;

            //generates only own moves
            List<Action> ret = new List<Action>();
            List<Minion> trgts = new List<Minion>();

            if (mPlayer.ownHero.Hp <= 0)
            {
                return ret;
            }

            //play cards:
            List<CardDB.cardName> playedcards = new List<CardDB.cardName>();

            foreach (Handmanager.Handcard hc in mPlayer.owncards)
            {
                if (p.keepCardList.Contains(hc.entity))
                    continue;
                CardDB.Card c = hc.card;
                //implementation
                if (c.name == CardDB.cardName.armorplating)
                {
                    p.moveTrigger.hasOwnTargetMove = true;
                }   
                //end of implementation
                if (playedcards.Contains(c.name) || !hc.canplayCard(p, own) || hc.playProb < cardProbCut) continue; // dont play the same card in one loop
                playedcards.Add(c.name);

                int isChoice = (c.choice) ? 1 : 0;
                for (int i = 0 + 1 * isChoice; i < 1 + 2 * isChoice; i++)
                {
                    if (isChoice == 1) c = getChooseCard(hc.card, i); // do all choice

                    int manaCost = hc.getManaCost(p, own);
                    if (mPlayer.mana >= manaCost) // if enough manna
                    {
                        int cardplayPenality = 0;
                        int bestplace = p.getBestPlace(c, isLethalCheck, own);
                        trgts = c.getTargetsForCard(p, isLethalCheck, own);
                        foreach (Minion trgt in trgts)                        
                        {
                            if (trgt != null && !trgt.isHero && trgt.own != own && trgt.targetProb < minionProbCut) continue;
                            if (usePenalityManager) cardplayPenality = pen.getPlayCardPenality(hc.card, trgt, p, i, isLethalCheck);
                            if (cardplayPenality <= 499)
                            {
                                Action a = new Action(actionEnum.playcard, hc, null, bestplace, trgt, cardplayPenality, i, manaCost); //i is the choice

                                ret.Add(a);
                                if (trgt != null && trgt.own == p.isOwnTurn) 
                                {
                                    p.moveTrigger.hasOwnTargetMove = true;
                                }
                            }
                        }              
                    }
                }
            }

            return ret;
        }

        public List<Action> getHeroPowerMoveList(Playfield p, bool isLethalCheck, bool usePenalityManager)
        {
            bool own = p.isOwnTurn;
            Player mPlayer = p.getCurrentPlayer(true);

            List<Action> ret = new List<Action>();
            List<Minion> trgts = new List<Minion>();

            if (mPlayer.ownHero.Hp <= 0)
            {
                return ret;
            }

            // use ability
            if (mPlayer.ownAbilityReady && mPlayer.mana >= 2) // if ready and enough manna TODO: TGT mana cost change
            {
                int cardplayPenality = 0;
                int bestplace = mPlayer.ownMinions.Count + 1; //we can not manage it
                trgts = mPlayer.ownHeroAblility.card.getTargetsForCard(p, isLethalCheck, own);
                foreach (Minion trgt in trgts)
                {
                    if (trgt != null && !trgt.isHero && trgt.own != own && trgt.targetProb < minionProbCut) continue;
                    if (usePenalityManager) cardplayPenality = pen.getPlayCardPenality(mPlayer.ownHeroAblility.card, trgt, p, 0, isLethalCheck);
                    if (cardplayPenality <= 499)
                    {
                        Action a = new Action(actionEnum.useHeroPower, mPlayer.ownHeroAblility, null, bestplace, trgt, cardplayPenality, 0, 2);
                        ret.Add(a);
                    }
                }
            }

            return ret;
        }


        public List<Action> getMoveList(Playfield p, bool isLethalCheck, bool usePenalityManager, bool useCutingTargets, double cardProbCut)
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

            //double cardProbCut = 0.0;
            //double minionProbCut = 1.0 / ePlayer.ownMinions.Count;

            //generates only own moves
            List<Action> ret = new List<Action>();
            List<Minion> trgts = new List<Minion>();

            if (mPlayer.ownHero.Hp <= 0)
            {
                return ret;
            }

            //play cards:
            List<CardDB.cardName> playedcards = new List<CardDB.cardName>();

            foreach (Handmanager.Handcard hc in mPlayer.owncards)
            {
                if (p.keepCardList.Contains(hc.entity))
                    continue;
                CardDB.Card c = hc.card;
                //implementation
                if (c.name == CardDB.cardName.thecoin)
                {
                    int debug = 1;
                }   
                if (c.name == CardDB.cardName.armorplating)
                {
                    p.moveTrigger.hasOwnTargetMove = true;
                }   
                //end of implementation
                if (playedcards.Contains(c.name) || !hc.canplayCard(p, own) || hc.playProb < cardProbCut) continue; // dont play the same card in one loop
                playedcards.Add(c.name);

                int isChoice = (c.choice) ? 1 : 0;
                for (int i = 0 + 1 * isChoice; i < 1 + 2 * isChoice; i++)
                {
                    if (isChoice == 1) c = getChooseCard(hc.card, i); // do all choice

                    int manaCost = hc.getManaCost(p, own);
                    if (mPlayer.mana >= manaCost) // if enough manna
                    {
                        int cardplayPenality = 0;
                        int bestplace = p.getBestPlace(c, isLethalCheck, own);
                        trgts = c.getTargetsForCard(p, isLethalCheck, own);

                        foreach (Minion trgt in trgts)                        
                        {
                            if (trgt != null && !trgt.isHero && trgt.own != own && trgt.targetProb < minionProbCut) continue;
                            if (usePenalityManager) cardplayPenality = pen.getPlayCardPenality(hc.card, trgt, p, i, isLethalCheck);
                            if (cardplayPenality <= 499)
                            {
                                Action a = new Action(actionEnum.playcard, hc, null, bestplace, trgt, cardplayPenality, i, manaCost); //i is the choice

                                ret.Add(a);
                                if (trgt != null && trgt.own == p.isOwnTurn) 
                                {
                                    p.moveTrigger.hasOwnTargetMove = true;
                                }
                            }
                        }              
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
                    if (trgt != null && !trgt.isHero && trgt.own != own && trgt.targetProb < minionProbCut) continue;
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
                    if (trgt != null && !trgt.isHero && trgt.own != own && trgt.targetProb < minionProbCut) continue;
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
                    if (trgt != null && !trgt.isHero && trgt.own != own && trgt.targetProb < minionProbCut) continue;
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
        public Action getRandomMove(Playfield p, bool isLethalCheck, bool usePenalityManager, bool useCutingTargets, bool own, double cardProbCut)
        {
            List<Action> actions = getMoveList(p, isLethalCheck, usePenalityManager, useCutingTargets, cardProbCut);
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