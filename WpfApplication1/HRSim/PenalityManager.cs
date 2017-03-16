namespace HRSim
{
    using System;
    using System.Collections.Generic;

    public class PenalityManager
    {
        //todo acolyteofpain
        //todo better aoe-penality

        //ComboBreaker cb;


        private static PenalityManager instance;

        public static PenalityManager Instance
        {
            get
            {
                return instance ?? (instance = new PenalityManager());
            }
        }

        //public void setCombos()
        //{
        //    this.cb = sf.Combobreaker;
        //}

        public float getOffenseReward(Action a, Playfield p) 
        {
            int reward = 0;
            if (CardDB.Instance.DamageTargetDatabase.ContainsKey(a.card.card.name))
            {
                int damage = CardDB.Instance.DamageTargetDatabase[a.card.card.name];
                int damagedone = p.getSpellDamageDamage(damage, p.isOwnTurn);

                if (a.target.Hp < damagedone)
                {
                    reward = a.target.Hp + a.target.Angr;
                }
                else
                {
                    reward = damagedone;
                }
            }
            return (float)reward / a.card.getManaCost(p, p.isOwnTurn);
        }

        public float getPlaycardActionReward(Action a, Playfield p)
        {
            float reward = 0f;

            if (a.card.card.type == CardDB.cardtype.SPELL)
            {
                if (CardDB.Instance.DamageTargetDatabase.ContainsKey(a.card.card.name))
                    reward += getOffenseReward(a, p);
            }

            return reward;
        
        }

        public float getAttackReward(Action a, Playfield p)
        {
            float reward = 0f;


            return reward;

        }

        public int getAttackWithMininonPenality(Minion m, Playfield p, Minion target, bool lethal)
        {
            bool isTargetOwn = false; if (target != null) isTargetOwn = target.own == p.isOwnTurn;

            int pen = 0;
            pen = getAttackSecretPenality(m, p, target);
            if (!lethal && m.name == CardDB.cardName.bloodimp) pen = 50;
            if (m.name == CardDB.cardName.leeroyjenkins)
            {
                if (!isTargetOwn)
                {
                    if (target.name == CardDB.cardName.whelp) return 500;
                }

            }
            return pen;
        }

        public int getAttackWithHeroPenality(Minion target, Playfield p, bool leathal)
        {
            int retval = 0;
                        bool isTargetOwn = false; if (target != null) isTargetOwn = target.own == p.isOwnTurn;

            Player mPlayer, ePlayer;

            if (p.isOwnTurn)
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }
            else 
            {
                mPlayer = p.playerSecond;
                ePlayer = p.playerFirst;
            }

            if (!leathal && mPlayer.ownWeaponName == CardDB.cardName.swordofjustice)
            {
                return 28;
            }

            if (mPlayer.ownWeaponDurability == 1 && mPlayer.ownWeaponName == CardDB.cardName.eaglehornbow)
            {
                foreach (Handmanager.Handcard hc in mPlayer.owncards)
                {
                    if (hc.card.name == CardDB.cardName.arcaneshot || hc.card.name == CardDB.cardName.killcommand) return -mPlayer.ownWeaponAttack - 1;
                }
                if (mPlayer.ownSecretsIDList.Count >= 1) return 20;

                foreach (Handmanager.Handcard hc in mPlayer.owncards)
                {
                    if (hc.card.Secret) return 20;
                }
            }

            //no penality, but a bonus, if he has weapon on hand!
            if (target.isHero && !isTargetOwn && mPlayer.ownWeaponName == CardDB.cardName.gorehowl && mPlayer.ownWeaponAttack >= 3)
            {
                return 10;
            }
            if (mPlayer.ownWeaponDurability >= 1)
            {
                bool hasweapon = false;
                foreach (Handmanager.Handcard c in mPlayer.owncards)
                {
                    if (c.card.type == CardDB.cardtype.WEAPON) hasweapon = true;
                }
                if (mPlayer.ownWeaponAttack == 1 && mPlayer.ownHeroName == HeroEnum.thief) hasweapon = true;
                if (hasweapon) retval = -mPlayer.ownWeaponAttack - 1; // so he doesnt "lose" the weapon in evaluation :D
            }
            if (mPlayer.ownWeaponAttack == 1 && mPlayer.ownHeroName == HeroEnum.thief) retval += -1;
            return retval;
        }

        public int getPlayCardPenality(CardDB.Card card, Minion target, Playfield p, int choice, bool lethal)
        {
            int retval = 0;
            CardDB.cardName name = card.name;
            //there is no reason to buff HP of minon (because it is not healed)

            int abuff = getAttackBuffPenality(card, target, p, choice, lethal);
            int tbuff = getTauntBuffPenality(name, target, p, choice);
            if (name == CardDB.cardName.markofthewild && ((abuff >= 500 && tbuff == 0) || (abuff == 0 && tbuff >= 500)))
            {
                retval = 0;
            }
            else
            {
                retval += abuff + tbuff;
            }
            retval += getHPBuffPenality(card, target, p, choice);
            retval += getSilencePenality(name, target, p, choice, lethal);
            retval += getDamagePenality(card, target, p, choice, lethal);
            retval += getHealPenality(name, target, p, choice, lethal);
            //if(retval < 500) 
            retval += getCardDrawPenality(name, target, p, choice, lethal);
            retval += getCardDrawofEffectMinions(card, p);
            retval += getCardDiscardPenality(name, p);
            retval += getDestroyOwnPenality(name, target, p, lethal);

            retval += getDestroyPenality(name, target, p, lethal);
            retval += getSpecialCardComboPenalitys(card, target, p, lethal, choice);
            retval += getRandomPenaltiy(card, p, target);
            if (!lethal)
            {
                //retval += cb.getPenalityForDestroyingCombo(card, p);
                //retval += cb.getPlayValue(card.cardIDenum);
            }

            retval += playSecretPenality(card, p);
            retval += getPlayCardSecretPenality(card, p);

            //Helpfunctions.Instance.ErrorLog("retval " + retval);
            return retval;
        }

        private int getAttackBuffPenality(CardDB.Card card, Minion target, Playfield p, int choice, bool lethal)
        {

                        bool isTargetOwn = false; if (target != null) isTargetOwn = target.own == p.isOwnTurn;

            Player mPlayer, ePlayer;

            if (p.isOwnTurn)
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }
            else
            {
                mPlayer = p.playerSecond;
                ePlayer = p.playerFirst;
            }

            CardDB.cardName name = card.name;
            if (name == CardDB.cardName.darkwispers && choice != 2) return 0;
            int pen = 0;
            //buff enemy?

            if (!lethal && (card.name == CardDB.cardName.savageroar || card.name == CardDB.cardName.bloodlust))
            {
                int targets = 0;
                foreach (Minion m in mPlayer.ownMinions)
                {
                    if (m.Ready) targets++;
                }
                if ((mPlayer.ownHero.Ready || mPlayer.ownHero.numAttacksThisTurn == 0) && card.name == CardDB.cardName.savageroar) targets++;

                if (targets <= 2)
                {
                    return 20;
                }
            }

            if (!CardDB.Instance.attackBuffDatabase.ContainsKey(name)) return 0;
            if (target == null) return 60;
            if (!target.isHero && !isTargetOwn)
            {
                if (card.type == CardDB.cardtype.MOB && mPlayer.ownMinions.Count == 0) return 0;
                //allow it if you have biggamehunter
                foreach (Handmanager.Handcard hc in mPlayer.owncards)
                {
                    if (hc.card.name == CardDB.cardName.biggamehunter) return 5;
                    if (hc.card.name == CardDB.cardName.shadowworddeath) return 5;
                }
                if (card.name == CardDB.cardName.crueltaskmaster || card.name == CardDB.cardName.innerrage)
                {
                    Minion m = target;

                    if (m.Hp == 1)
                    {
                        return 0;
                    }

                    if (!m.wounded && (m.Angr >= 4 || m.Hp >= 5))
                    {
                        foreach (Handmanager.Handcard hc in mPlayer.owncards)
                        {
                            if (hc.card.name == CardDB.cardName.execute) return 0;
                        }
                    }
                    pen = 30;
                }
                else
                {
                    pen = 500;
                }
            }
            if (!target.isHero && isTargetOwn)
            {
                Minion m = target;
                if (!m.Ready)
                {
                    return 50;
                }
                if (m.Hp == 1 && !m.divineshild && !CardDB.Instance.buffing1TurnDatabase.ContainsKey(name))
                {
                    return 10;
                }
            }

            if (card.name == CardDB.cardName.blessingofmight) return 6;
            return pen;
        }

        private int getHPBuffPenality(CardDB.Card card, Minion target, Playfield p, int choice)
        {
            bool isTargetOwn = false; if (target != null) isTargetOwn = target.own == p.isOwnTurn;

            CardDB.cardName name = card.name;
            if (name == CardDB.cardName.darkwispers && choice != 2) return 0;

            // Drew: We need a target for this card!
            if (name == CardDB.cardName.darkwispers && target == null) return 500;

            int pen = 0;

            //buff enemy?
            if (!CardDB.Instance.healthBuffDatabase.ContainsKey(name)) return 0;

            // Drew: We add a null check here to avoid interfering with other logic with the previous conditional statement.
            if (target != null && !isTargetOwn && !CardDB.Instance.tauntBuffDatabase.ContainsKey(name))
            {
                pen = 500;
            }

            return pen;
        }


        private int getTauntBuffPenality(CardDB.cardName name, Minion target, Playfield p, int choice)
        {
                        bool isTargetOwn = false; if (target != null) isTargetOwn = target.own == p.isOwnTurn;

            Player mPlayer, ePlayer;

            if (p.isOwnTurn)
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }
            else
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }

            int pen = 0;
            //buff enemy?
            if (!CardDB.Instance.tauntBuffDatabase.ContainsKey(name)) return 0;
            if (name == CardDB.cardName.markofnature && choice != 2) return 0;
            if (name == CardDB.cardName.darkwispers && choice != 2) return 0;

            // Drew: We need a target for this card!
            if (name == CardDB.cardName.darkwispers && target == null) return 500;

            if (!target.isHero && !isTargetOwn)
            {
                //allow it if you have black knight
                foreach (Handmanager.Handcard hc in mPlayer.owncards)
                {
                    if (hc.card.name == CardDB.cardName.theblackknight) return 0;
                }

                // allow taunting if target is priority and others have taunt
                bool enemyhasTaunts = false;
                foreach (Minion mnn in ePlayer.ownMinions)
                {
                    if (mnn.taunt)
                    {
                        enemyhasTaunts = true;
                        break;
                    }
                }
                if (enemyhasTaunts && CardDB.Instance.priorityDatabase.ContainsKey(target.name) && !target.silenced && !target.taunt)
                {
                    return 0;
                }

                pen = 500;
            }

            return pen;
        }

        private int getSilencePenality(CardDB.cardName name, Minion target, Playfield p, int choice, bool lethal)
        {
                        bool isTargetOwn = false; if (target != null) isTargetOwn = target.own == p.isOwnTurn;

            Player mPlayer, ePlayer;

            if (p.isOwnTurn)
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }
            else
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }

            int pen = 0;
            if (name == CardDB.cardName.keeperofthegrove && choice != 2) return 0; // look at damage penality in this case

            if (target == null)
            {
                if (name == CardDB.cardName.ironbeakowl || name == CardDB.cardName.spellbreaker || name == CardDB.cardName.keeperofthegrove)
                {
                    return 20;
                }
                return 0;
            }

            if (isTargetOwn)
            {
                if (CardDB.Instance.silenceDatabase.ContainsKey(name))
                {
                    // no pen if own is enrage
                    if (!target.silenced && CardDB.Instance.OwnNeedSilenceDatabase.ContainsKey(target.name))
                    {
                        return -5;
                    }
                    // no pen if own is enrage
                    if (target.Angr < target.handcard.card.Attack || target.maxHp < target.handcard.card.Health || (target.frozen && !target.playedThisTurn && target.numAttacksThisTurn == 0))
                    {
                        return 0;
                    }
                    pen += 500;
                }
            }
            else if (!isTargetOwn)
            {
                if (CardDB.Instance.silenceDatabase.ContainsKey(name))
                {
                    pen = 5;
                    if (lethal)
                    {
                        //during lethal we only silence taunt, or if its a mob (owl/spellbreaker) + we can give him charge
                        if (target.taunt || (name == CardDB.cardName.ironbeakowl && (mPlayer.ownMinions.Find(x => x.name == CardDB.cardName.tundrarhino) != null || mPlayer.ownMinions.Find(x => x.name == CardDB.cardName.warsongcommander) != null || mPlayer.owncards.Find(x => x.card.name == CardDB.cardName.charge) != null)) || (name == CardDB.cardName.spellbreaker && mPlayer.owncards.Find(x => x.card.name == CardDB.cardName.charge) != null)) return 0;

                        return 500;
                    }

                    if (!target.silenced && CardDB.Instance.OwnNeedSilenceDatabase.ContainsKey(target.name))
                    {
                        if (target.taunt) pen += 15;
                        return 500;
                    }

                    if (!target.silenced)
                    {
                        if (CardDB.Instance.priorityDatabase.ContainsKey(target.name)) return 0;
                        if (CardDB.Instance.silenceTargets.ContainsKey(target.name)) return 0;
                        if (target.handcard.card.deathrattle) return 0;
                    }

                    if (target.Angr <= target.handcard.card.Attack && target.maxHp <= target.handcard.card.Health && !target.taunt && !target.windfury && !target.divineshild && !target.poisonous && !CardDB.Instance.specialMinions.ContainsKey(name))
                    {
                        if (name == CardDB.cardName.keeperofthegrove) return 500;
                        return 30;
                    }

                    if (target.Angr > target.handcard.card.Attack || target.maxHp > target.handcard.card.Health)
                    {
                        return 0;
                    }

                    return pen;
                }
            }

            return pen;

        }

        private int getDamagePenality(CardDB.Card card, Minion target, Playfield p, int choice, bool lethal)
        {
                        bool isTargetOwn = false; if (target != null) isTargetOwn = target.own == p.isOwnTurn;

            Player mPlayer, ePlayer;

            if (p.isOwnTurn)
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }
            else
            {
                mPlayer = p.playerSecond;
                ePlayer = p.playerFirst;
            }

            CardDB.cardName name = card.name;
            int pen = 0;

            if (name == CardDB.cardName.shieldslam && mPlayer.ownHero.armor == 0) return 500;
            if (name == CardDB.cardName.savagery && mPlayer.ownHero.Angr == 0) return 500;
            if (name == CardDB.cardName.keeperofthegrove && choice != 1) return 0; // look at silence penality
            
            //aoe damage *************************************************************************************
            int aoeDamageType = 0;
            if (CardDB.Instance.DamageAllEnemysDatabase.ContainsKey(name)) aoeDamageType = 1;
            else if (mPlayer.anzOwnAuchenaiSoulpriest >= 1 && CardDB.Instance.HealAllDatabase.ContainsKey(name)) aoeDamageType = 2;
            else if (CardDB.Instance.DamageAllDatabase.ContainsKey(name)) aoeDamageType = 3;
            if (aoeDamageType > 0)
            {
                if (ePlayer.ownMinions.Count == 0) return 300;

                int aoeDamage = 0;
                if (aoeDamageType == 1) aoeDamage = (card.type == CardDB.cardtype.SPELL) ? p.getSpellDamageDamage(CardDB.Instance.DamageAllEnemysDatabase[name], p.isOwnTurn) : CardDB.Instance.DamageAllEnemysDatabase[name];
                else if (aoeDamageType == 2) aoeDamage = (card.type == CardDB.cardtype.SPELL) ? p.getSpellDamageDamage(CardDB.Instance.HealAllDatabase[name], p.isOwnTurn) : CardDB.Instance.HealAllDatabase[name];
                else if (aoeDamageType == 3)
                {
                    if (name == CardDB.cardName.revenge && mPlayer.ownHero.Hp <= 12) p.getSpellDamageDamage(3, p.isOwnTurn);
                    else aoeDamage = (card.type == CardDB.cardtype.SPELL) ? p.getSpellDamageDamage(CardDB.Instance.DamageAllDatabase[name], p.isOwnTurn) : CardDB.Instance.DamageAllDatabase[name];
                }
                
                int preventDamage = 0;
                int lostOwnDamage = 0;
                int lostOwnMinions = 0;
                int survivedEnemyMinions = 0;
                int survivedEnemyMinionsAngr = 0;
                bool frothingberserkerEnemy = false;
                bool frothingberserkerOwn = false;
                bool grimpatronEnemy = false;
                bool grimpatronOwn = false;
                foreach (Minion m in ePlayer.ownMinions)
                {
                    if (aoeDamage >= m.Hp && (name == CardDB.cardName.demonwrath ? ((TAG_RACE)m.handcard.card.race == TAG_RACE.DEMON ? false : true) : true)) preventDamage += m.Angr;
                    else
                    {
                        survivedEnemyMinions++;
                        if (survivedEnemyMinionsAngr < m.Angr) survivedEnemyMinionsAngr = m.Angr;
                        if (!m.wounded && CardDB.Instance.enrageDatabase.ContainsKey(name)) preventDamage -= CardDB.Instance.enrageDatabase[name];
                        else if (m.name == CardDB.cardName.gurubashiberserker) preventDamage -= 3;
                        else if (m.name == CardDB.cardName.frothingberserker) frothingberserkerEnemy = true;
                        else if (m.name == CardDB.cardName.grimpatron) { preventDamage -= 3; grimpatronEnemy = true; }
                    }
                }
                int ownWoundedMinions = 0;
                foreach (Minion m in mPlayer.ownMinions)
                {
                    if (m.wounded) ownWoundedMinions++;
                    if (aoeDamage >= m.Hp && (name == CardDB.cardName.demonwrath ? ((TAG_RACE)m.handcard.card.race == TAG_RACE.DEMON ? false : true ) : true))
                    {
                        lostOwnDamage += m.Angr;
                        lostOwnMinions++;
                        if (!m.wounded && CardDB.Instance.enrageDatabase.ContainsKey(name)) lostOwnDamage += CardDB.Instance.enrageDatabase[name];
                        else if (m.name == CardDB.cardName.gurubashiberserker && m.Hp > 1) lostOwnDamage += 3;
                        else if (m.name == CardDB.cardName.frothingberserker) frothingberserkerOwn = true;
                        else if (m.name == CardDB.cardName.grimpatron) { lostOwnDamage += 3; grimpatronOwn = true; }
                    }
                }
                int hasExecute = 0;
                foreach (Handmanager.Handcard hc in mPlayer.owncards)
                {
                    if (hc.card.name == CardDB.cardName.execute) hasExecute++;
                }

                if (aoeDamageType == 1) //only enemy
                {
                    if (preventDamage > 5 || (ePlayer.ownMinions.Count - survivedEnemyMinions) >= 4) return 0;
                    else if (name == CardDB.cardName.holynova && preventDamage >= 0 && ownWoundedMinions > 2) return 20;

                    if (hasExecute > 0)
                    {
                        if (survivedEnemyMinions <= hasExecute) return 0;
                        preventDamage += survivedEnemyMinionsAngr;
                        if (preventDamage > 6) preventDamage = 6;
                    }
                    
                    return (6 - preventDamage) * 20;
                }
                else
                {
                    if (mPlayer.ownMinions.Count - lostOwnMinions - survivedEnemyMinions > 0)
                    {
                        if (preventDamage >= lostOwnDamage) return 0;
                        return (lostOwnDamage - preventDamage) * 2;
                    }
                    else
                    {
                        if (preventDamage >= lostOwnDamage * 2 + 1) return 0;
                        int MinionBalance = lostOwnMinions - (ePlayer.ownMinions.Count - survivedEnemyMinions);
                        if (MinionBalance > 0 && preventDamage <= lostOwnDamage) return 80;
                        return 30;
                    }
                }
            }
            //END aoe damage **********************************************************************************

            if (target == null) return 0;

            if (isTargetOwn && target.isHero)
            {
                if (CardDB.Instance.DamageTargetDatabase.ContainsKey(name) || CardDB.Instance.DamageTargetSpecialDatabase.ContainsKey(name) || (mPlayer.anzOwnAuchenaiSoulpriest >= 1 && CardDB.Instance.HealTargetDatabase.ContainsKey(name)))
                {
                    pen = 500;
                }
            }

            if (!lethal && !isTargetOwn && target.isHero)
            {
                if (name == CardDB.cardName.baneofdoom)
                {
                    pen = 500;
                }
            }

            if (isTargetOwn && !target.isHero)
            {
                if (CardDB.Instance.DamageTargetDatabase.ContainsKey(name) || (mPlayer.anzOwnAuchenaiSoulpriest >= 1 && CardDB.Instance.HealTargetDatabase.ContainsKey(name)))
                {
                    // no pen if own is enrage
                    Minion m = target;

                    //standard ones :D (mostly carddraw
                    if (CardDB.Instance.enrageDatabase.ContainsKey(m.name) && !m.wounded && m.Ready)
                    {
                        return pen;
                    }

                    // no pen if we have battlerage for example
                    int dmg = CardDB.Instance.DamageTargetDatabase.ContainsKey(name) ? CardDB.Instance.DamageTargetDatabase[name] : CardDB.Instance.HealTargetDatabase[name];

                    if (m.name == CardDB.cardName.madscientist && mPlayer.ownHeroStartClass == TAG_CLASS.HUNTER) return 500;
                    if (m.handcard.card.deathrattle) return 10;
                    if (m.Hp > dmg)
                    {
                        if (m.name == CardDB.cardName.acolyteofpain && mPlayer.owncards.Count <= 3) return 0;
                        if (m.name == CardDB.cardName.dragonegg && mPlayer.ownMinions.Count <= 6) return 5;
                        if (m.name == CardDB.cardName.impgangboss && mPlayer.ownMinions.Count <= 6) return 0;
                        if (m.name == CardDB.cardName.grimpatron && mPlayer.ownMinions.Count <= 6) return 0;
                        foreach (Handmanager.Handcard hc in mPlayer.owncards)
                        {
                            if (hc.card.name == CardDB.cardName.battlerage) return pen;
                            if (hc.card.name == CardDB.cardName.rampage) return pen;
                        }
                    }


                    pen = 500;
                }

                //special cards
                if (CardDB.Instance.DamageTargetSpecialDatabase.ContainsKey(name))
                {
                    int dmg = CardDB.Instance.DamageTargetSpecialDatabase[name];
                    Minion m = target;
                    if ((name == CardDB.cardName.crueltaskmaster || name == CardDB.cardName.innerrage) && m.Hp >= 2) return 0;
                    if ((name == CardDB.cardName.demonfire || name == CardDB.cardName.demonheart) && (TAG_RACE)m.handcard.card.race == TAG_RACE.DEMON) return 0;
                    if (name == CardDB.cardName.earthshock && m.Hp >= 2)
                    {
                        if ((!m.silenced && (m.name == CardDB.cardName.ancientwatcher || m.name == CardDB.cardName.ragnarosthefirelord)) || m.Angr < m.handcard.card.Attack || m.maxHp < m.handcard.card.Health || (m.frozen && !m.playedThisTurn && m.numAttacksThisTurn == 0))
                            return 0;
                        if (CardDB.Instance.priorityDatabase.ContainsKey(m.name) && !m.silenced)
                        {
                            return 500;
                        }
                    }
                    if (name == CardDB.cardName.earthshock)//dont silence other own minions
                    {
                        return 500;
                    }

                    if (m.Hp > dmg)
                    {
                        if (CardDB.Instance.enrageDatabase.ContainsKey(m.name) && !m.wounded && m.Ready) // no pen if own is enrage
                        {
                            return pen;
                        }

                        foreach (Handmanager.Handcard hc in mPlayer.owncards) // no pen if we have battlerage for example
                        {
                            if (hc.card.name == CardDB.cardName.battlerage) return pen;
                            if (hc.card.name == CardDB.cardName.rampage) return pen;
                        }
                    }

                    pen = 500;
                }
            }
            if (!isTargetOwn && !target.isHero)
            {
                int realDamage = 0;
                if (CardDB.Instance.DamageTargetSpecialDatabase.ContainsKey(name)) realDamage = (card.type == CardDB.cardtype.SPELL) ?  p.getSpellDamageDamage(CardDB.Instance.DamageTargetSpecialDatabase[name], p.isOwnTurn) : CardDB.Instance.DamageTargetSpecialDatabase[name];
                else if (CardDB.Instance.DamageTargetDatabase.ContainsKey(name)) realDamage = (card.type == CardDB.cardtype.SPELL) ? p.getSpellDamageDamage(CardDB.Instance.DamageTargetDatabase[name], p.isOwnTurn) : CardDB.Instance.DamageTargetDatabase[name];
                if (realDamage > 0)
                {
                    if (name == CardDB.cardName.soulfire && target.maxHp <= realDamage - 2) pen = 10;
                    if (name == CardDB.cardName.baneofdoom && target.Hp > realDamage) pen = 10;
                    if (name == CardDB.cardName.shieldslam && (target.Hp <= 4 || target.Angr <= 4)) pen = 20;
                }
                if (realDamage == 0) realDamage = card.Attack;
                if (target.name == CardDB.cardName.grimpatron && realDamage < target.Hp) return 500;
            }

            return pen;
        }

        private int getHealPenality(CardDB.cardName name, Minion target, Playfield p, int choice, bool lethal)
        {
                        bool isTargetOwn = false; if (target != null) isTargetOwn = target.own == p.isOwnTurn;

            Player mPlayer, ePlayer;

            if (p.isOwnTurn)
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }
            else
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }

            ///Todo healpenality for aoe heal
            ///todo auchenai soulpriest
            if (mPlayer.anzOwnAuchenaiSoulpriest >= 1) return 0;
            if (name == CardDB.cardName.ancientoflore && choice != 2) return 0;
            int pen = 0;
            int heal = 0;
            /*if (HealHeroDatabase.ContainsKey(name))
            {
                heal = HealHeroDatabase[name];
                if (target == 200) pen = 500; // dont heal enemy
                if ((target == 100 || target == -1) && mPlayer.ownHeroHp + heal > 30) pen = mPlayer.ownHeroHp + heal - 30;
            }*/

            if (name == CardDB.cardName.treeoflife)
            {
                int mheal = 0;
                int wounded = 0;
                //int eheal = 0;
                if (mPlayer.ownHero.wounded) wounded++;
                foreach (Minion mi in mPlayer.ownMinions)
                {
                    mheal += Math.Min((mi.maxHp - mi.Hp), 4);
                    if (mi.wounded) wounded++;
                }
                //Console.WriteLine(mheal + " circle");
                if (mheal == 0) return 500;
                if (mheal <= 7 && wounded <= 2) return 20;
            }

            if (name == CardDB.cardName.circleofhealing)
            {
                int mheal = 0;
                int wounded = 0;
                //int eheal = 0;
                foreach (Minion mi in mPlayer.ownMinions)
                {
                    mheal += Math.Min((mi.maxHp - mi.Hp), 4);
                    if (mi.wounded) wounded++;
                }
                //Console.WriteLine(mheal + " circle");
                if (mheal == 0) return 500;
                if (mheal <= 7 && wounded <= 2) return 20;
            }

            if (CardDB.Instance.HealTargetDatabase.ContainsKey(name))
            {
                if (target == null) return 10;
                //Helpfunctions.Instance.ErrorLog("pencheck for " + name + " " + target.entitiyID + " " + target.isHero  + " " + target.own);
                heal = CardDB.Instance.HealTargetDatabase[name];
                if (target.isHero && !isTargetOwn) return 510; // dont heal enemy
                //Helpfunctions.Instance.ErrorLog("pencheck for " + name + " " + target.entitiyID + " " + target.isHero + " " + target.own);
                if ((target.isHero && isTargetOwn) && mPlayer.ownHero.Hp == 30) return 150;
                if ((target.isHero && isTargetOwn) && mPlayer.ownHero.Hp + heal - 1 > 30) pen = mPlayer.ownHero.Hp + heal - 30;
                Minion m = new Minion();

                if (!target.isHero && isTargetOwn)
                {
                    m = target;
                    int wasted = 0;
                    if (m.Hp == m.maxHp) return 500;
                    if (m.Hp + heal - 1 > m.maxHp) wasted = m.Hp + heal - m.maxHp;
                    pen = wasted;

                    if (m.taunt && wasted <= 2 && m.Hp < m.maxHp) pen -= 5; // if we heal a taunt, its good :D

                    if (m.Hp + heal <= m.maxHp) pen = -1;
                }

                if (!target.isHero && !isTargetOwn)
                {
                    m = target;
                    if (m.Hp == m.maxHp) return 500;
                    // no penality if we heal enrage enemy
                    if (CardDB.Instance.enrageDatabase.ContainsKey(m.name))
                    {
                        return pen;
                    }
                    // no penality if we have heal-trigger :D
                    int i = 0;
                    foreach (Minion mnn in mPlayer.ownMinions)
                    {
                        if (mnn.name == CardDB.cardName.northshirecleric) i++;
                        if (mnn.name == CardDB.cardName.lightwarden) i++;
                    }
                    foreach (Minion mnn in ePlayer.ownMinions)
                    {
                        if (mnn.name == CardDB.cardName.northshirecleric) i--;
                        if (mnn.name == CardDB.cardName.lightwarden) i--;
                    }
                    if (i >= 1) return pen;

                    // no pen if we have slam

                    foreach (Handmanager.Handcard hc in mPlayer.owncards)
                    {
                        if (hc.card.name == CardDB.cardName.slam && m.Hp < 2) return pen;
                        if (hc.card.name == CardDB.cardName.backstab) return pen;
                    }

                    pen = 500;
                }


            }

            return pen;
        }

        private int getCardDrawPenality(CardDB.cardName name, Minion target, Playfield p, int choice, bool lethal)
        {
            Player mPlayer, ePlayer; //know mplayer, but not eplayer
            List<CardDB.Card> mDeck, eDeck;
            if (p.isOwnTurn)
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
                mDeck = p.homeDeck;
                eDeck = p.awayDeck;
            }
            else
            {
                ePlayer = p.playerFirst;
                mPlayer = p.playerSecond;
                eDeck = p.homeDeck;
                mDeck = p.awayDeck;
            }

            // penality if carddraw is late or you have enough cards
            int pen = 0;
            if (!CardDB.Instance.cardDrawBattleCryDatabase.ContainsKey(name)) return 0;
            if (name == CardDB.cardName.ancientoflore && choice != 1) return 0;
            if (name == CardDB.cardName.wrath && choice != 2) return 0;
            if (name == CardDB.cardName.nourish && choice != 2) return 0;
            if (name == CardDB.cardName.grovetender && choice != 2) return 0;
            if (name == CardDB.cardName.tracking) return 0;

            int carddraw = CardDB.Instance.cardDrawBattleCryDatabase[name];
            if (carddraw == 0)
            {
                switch(name)
                {
                    case CardDB.cardName.harrisonjones:
                        carddraw = ePlayer.ownWeaponDurability;
                        if (carddraw == 0 && (ePlayer.ownHeroStartClass != TAG_CLASS.DRUID && ePlayer.ownHeroStartClass != TAG_CLASS.MAGE && ePlayer.ownHeroStartClass != TAG_CLASS.WARLOCK && ePlayer.ownHeroStartClass != TAG_CLASS.PRIEST)) return 5;
                        break;

                    case CardDB.cardName.divinefavor:
                        carddraw = ePlayer.owncards.Count - (mPlayer.owncards.Count);
                        if (carddraw <= 0) return 500;
                        break;

                    case CardDB.cardName.battlerage:
                        foreach (Minion mnn in mPlayer.ownMinions)
                        {
                            if (mnn.wounded) carddraw++;
                        }
                        if (carddraw == 0) return 500;
                        break;

                    case CardDB.cardName.slam:
                        if (target != null && target.Hp >= 3) carddraw = 1;
                        if (carddraw == 0) return 4;
                        break;

                    case CardDB.cardName.mortalcoil:
                        if (target != null && target.Hp == 1) carddraw = 1;
                        if (carddraw == 0) return 15;
                        break;

                    case CardDB.cardName.quickshot:
                        carddraw = (mPlayer.owncards.Count > 0) ? 0 : 1;
                        if (carddraw == 0) return 4;
                        break;

                    case CardDB.cardName.thoughtsteal:
                        carddraw = Math.Min(2, eDeck.Count);
                        if (carddraw == 2) break;
                        if (carddraw == 1) pen +=4;
                        else
                        {
                            foreach (Minion mnn in mPlayer.ownMinions)
                            {
                                if (CardDB.Instance.spellDependentDatabase.ContainsKey(mnn.name)) return 0;
                            }
                            return 500;
                        }
                        break;
                    
                    case CardDB.cardName.mindvision:
                        carddraw = Math.Min(1, ePlayer.owncards.Count);
                        if (carddraw != 1)
                        {
                            int scales = 0;
                            foreach (Minion mnn in mPlayer.ownMinions)
                            {
                                if (CardDB.Instance.spellDependentDatabase.ContainsKey(mnn.name))
                                    if(mnn.name == CardDB.cardName.lorewalkercho) pen += 20; //if(spellDependentDatabase[mnn.name] == 0);
                                    else scales--;
                            }
                            if (scales == 0) return 500;
                            foreach (Minion mnn in ePlayer.ownMinions)
                            {
                                if (CardDB.Instance.spellDependentDatabase.ContainsKey(mnn.name) && CardDB.Instance.spellDependentDatabase[name] <= 0) scales++;
                            }
                            return (12 + scales * 4 + pen);
                        }
                        break;
                        
                    case CardDB.cardName.echoofmedivh:
                        if (mPlayer.ownMinions.Count == 0) return 500;
                        return 0;
                        break;
                        
                    case CardDB.cardName.tinkertowntechnician:
                        foreach (Minion mnn in mPlayer.ownMinions)
                        {
                            if ((TAG_RACE)mnn.handcard.card.race != TAG_RACE.MECHANICAL) pen += 4;
                        }
                        break;
                    default:
                        break;
                }
            }
            
            if (name == CardDB.cardName.farsight || name == CardDB.cardName.callpet) pen -= 10;
            //if (name == CardDB.cardName.flare) // handled in private int getSpecialCardComboPenalitys


            if (name == CardDB.cardName.lifetap)
            {
                if (lethal) return 500; //RR no benefit for lethal check
                int minmana = 10;
                bool cardOnLimit = false;
                foreach (Handmanager.Handcard hc in mPlayer.owncards)
                {
                    if (hc.manacost <= minmana)
                    {
                        minmana = hc.manacost;
                    }
                    //if (hc.getManaCost(p) == mPlayer.ownMaxMana)
                    int manac = hc.getManaCost(p, p.isOwnTurn);
                    if (manac > mPlayer.ownMaxMana - 2 && manac <= mPlayer.ownMaxMana)
                    {
                        cardOnLimit = true;
                    }

                }

                //if (sf.Ai.botBase is BehaviorRush && mPlayer.ownMaxMana <= 3 && cardOnLimit) return 6; //RR penalization for drawing the 3 first turns if we have a card in hand that we won't be able to play in Rush


                if (mPlayer.owncards.Count + mPlayer.cardsPlayedThisTurn <= 5 && minmana > mPlayer.ownMaxMana) return 0;
                if (mPlayer.owncards.Count + mPlayer.cardsPlayedThisTurn > 5) return 25;
                return Math.Max(-carddraw + 2 * mPlayer.optionsPlayedThisTurn + mPlayer.ownMaxMana - mPlayer.mana, 0);
            }

            if (mPlayer.owncards.Count + carddraw > 10) return 15 * (mPlayer.owncards.Count + carddraw - 10);
            if (mPlayer.owncards.Count + mPlayer.cardsPlayedThisTurn > 5) return (5 * carddraw) + 1;

            pen += -carddraw + 2 * mPlayer.optionsPlayedThisTurn + mPlayer.ownMaxMana - mPlayer.mana;
            return pen;
        }

        private int getCardDrawofEffectMinions(CardDB.Card card, Playfield p)
        {
            Player mPlayer, ePlayer;

            if (p.isOwnTurn)
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }
            else
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }

            int pen = 0;
            int carddraw = 0;
            if (card.type == CardDB.cardtype.SPELL)
            {
                foreach (Minion mnn in mPlayer.ownMinions)
                {
                    if (mnn.name == CardDB.cardName.gadgetzanauctioneer) carddraw++;
                }
            }

            if (card.type == CardDB.cardtype.MOB && (TAG_RACE)card.race == TAG_RACE.PET)
            {
                foreach (Minion mnn in mPlayer.ownMinions)
                {
                    if (mnn.name == CardDB.cardName.starvingbuzzard) carddraw++;
                }
            }

            if (carddraw == 0) return 0;

            if (mPlayer.owncards.Count >= 5) return 0;
            pen = -carddraw + mPlayer.ownMaxMana - mPlayer.mana + mPlayer.optionsPlayedThisTurn;

            return pen;
        }

        private int getRandomPenaltiy(CardDB.Card card, Playfield p, Minion target)
        {
            Player mPlayer, ePlayer;

            if (p.isOwnTurn)
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }
            else
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }

            if (p.turnCounter >= 1)
            {
                return 0;
            }

            if (!CardDB.Instance.randomEffects.ContainsKey(card.name) && !CardDB.Instance.cardDrawBattleCryDatabase.ContainsKey(card.name))
            {
                return 0;
            }

            if (card.name == CardDB.cardName.brawl)
            {
                return 0;
            }

            if ((card.name == CardDB.cardName.cleave || card.name == CardDB.cardName.multishot)
                && ePlayer.ownMinions.Count == 2)
            {
                return 0;
            }

            if ((card.name == CardDB.cardName.deadlyshot) && ePlayer.ownMinions.Count == 1)
            {
                return 0;
            }

            if ((card.name == CardDB.cardName.arcanemissiles || card.name == CardDB.cardName.avengingwrath)
                && ePlayer.ownMinions.Count == 0)
            {
                return 0;
            }

            int cards = 0;
            cards = CardDB.Instance.randomEffects.ContainsKey(card.name) ? CardDB.Instance.randomEffects[card.name] : CardDB.Instance.cardDrawBattleCryDatabase[card.name];

            bool first = true;
            bool hasgadget = false;
            bool hasstarving = false;
            bool hasknife = false;
            bool hasFlamewaker = false;
            foreach (Minion mnn in mPlayer.ownMinions)
            {
                if (mnn.name == CardDB.cardName.gadgetzanauctioneer)
                {
                    hasgadget = true;
                }

                if (mnn.name == CardDB.cardName.starvingbuzzard)
                {
                    hasstarving = true;
                }

                if (mnn.name == CardDB.cardName.knifejuggler)
                {
                    hasknife = true;
                }

                if (mnn.name == CardDB.cardName.flamewaker)
                {
                    hasFlamewaker = true;
                }
            }

            foreach (Action a in mPlayer.playactions)
            {
                if (a.actionType == actionEnum.attackWithHero)
                {
                    first = false;
                    continue;
                }

                if (a.actionType == actionEnum.useHeroPower
                    && (mPlayer.ownHeroName != HeroEnum.shaman && mPlayer.ownHeroName != HeroEnum.warlock))
                {
                    first = false;
                    continue;
                }

                if (a.actionType == actionEnum.attackWithMinion)
                {
                    first = false;
                    continue;
                }

                if (a.actionType == actionEnum.playcard)
                {
                    if (card.name == CardDB.cardName.knifejuggler && card.type == CardDB.cardtype.MOB)
                    {
                        continue;
                    }

                    if (CardDB.Instance.cardDrawBattleCryDatabase.ContainsKey(a.card.card.name))
                    {
                        continue;
                    }

                    if (hasgadget && card.type == CardDB.cardtype.SPELL)
                    {
                        continue;
                    }

                    if (hasFlamewaker && card.type == CardDB.cardtype.SPELL)
                    {
                        continue;
                    }

                    if (hasstarving && (TAG_RACE)card.race == TAG_RACE.PET)
                    {
                        continue;
                    }

                    if (hasknife && card.type == CardDB.cardtype.MOB)
                    {
                        continue;
                    }

                    first = false;
                }
            }

            if (first == false)
            {
                return cards + mPlayer.playactions.Count + 1;
            }

            return 0;
        }

        private int getCardDiscardPenality(CardDB.cardName name, Playfield p)
        {
            Player mPlayer, ePlayer;

            if (p.isOwnTurn)
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }
            else
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }

            if (mPlayer.owncards.Count <= 1) return 0;
            if (mPlayer.ownMaxMana <= 3) return 0;
            int pen = 0;
            if (CardDB.Instance.cardDiscardDatabase.ContainsKey(name))
            {
                int newmana = mPlayer.mana - CardDB.Instance.cardDiscardDatabase[name];
                bool canplayanothercard = false;
                foreach (Handmanager.Handcard hc in mPlayer.owncards)
                {
                    if (CardDB.Instance.cardDiscardDatabase.ContainsKey(hc.card.name)) continue;
                    if (hc.getManaCost(p, p.isOwnTurn) <= newmana)
                    {
                        canplayanothercard = true;
                    }
                }
                if (canplayanothercard) pen += 20;

            }

            return pen;
        }

        private int getDestroyOwnPenality(CardDB.cardName name, Minion target, Playfield p, bool lethal)
        {
                        bool isTargetOwn = false; if (target != null) isTargetOwn = target.own == p.isOwnTurn;

            Player mPlayer, ePlayer;

            if (p.isOwnTurn)
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }
            else
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }

            if (!CardDB.Instance.destroyOwnDatabase.ContainsKey(name)) return 0;
            int pen = 0;
            if ((name == CardDB.cardName.brawl || name == CardDB.cardName.deathwing || name == CardDB.cardName.twistingnether) && mPlayer.mobsplayedThisTurn >= 1) return 500;

            if (name == CardDB.cardName.brawl || name == CardDB.cardName.twistingnether)
            {
                if (name == CardDB.cardName.brawl && mPlayer.ownMinions.Count + ePlayer.ownMinions.Count <= 1) return 500;
                int highminion = 0;
                int veryhighminion = 0;
                foreach (Minion m in ePlayer.ownMinions)
                {
                    if (m.Angr >= 5 || m.Hp >= 5) highminion++;
                    if (m.Angr >= 8 || m.Hp >= 8) veryhighminion++;
                }

                if (highminion >= 2 || veryhighminion >= 1)
                {
                    return 0;
                }

                if (ePlayer.ownMinions.Count <= 2 || ePlayer.ownMinions.Count + 2 <= mPlayer.ownMinions.Count || mPlayer.ownMinions.Count >= 3)
                {
                    return 30;
                }
            }
            if (target == null) return 0;
            if (isTargetOwn && !target.isHero)
            {
                // dont destroy owns ;_; (except mins with deathrattle effects)

                Minion m = target;
                if (m.handcard.card.deathrattle) return 10;
                if (lethal && (name == CardDB.cardName.sacrificialpact || name == CardDB.cardName.siphonsoul))
                {
                    int beasts = 0;
                    foreach (Minion mm in mPlayer.ownMinions)
                    {
                        if (mm.Ready && mm.handcard.card.name == CardDB.cardName.lightwarden && target.entitiyID != mm.entitiyID) beasts++;
                    }
                    if (beasts == 0) return 500;
                }
                else
                {

                    return 500;
                }
            }

            return pen;
        }

        private int getDestroyPenality(CardDB.cardName name, Minion target, Playfield p, bool lethal)
        {
                        bool isTargetOwn = false; if (target != null) isTargetOwn = target.own == p.isOwnTurn;

            Player mPlayer, ePlayer;

            if (p.isOwnTurn)
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }
            else
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }

            if (!CardDB.Instance.destroyDatabase.ContainsKey(name) || lethal) return 0;
            int pen = 0;
            if (target == null) return 0;
            if (isTargetOwn && !target.isHero)
            {
                Minion m = target;
                if (!m.handcard.card.deathrattle)
                {
                    pen = 500;
                }
            }
            if (!isTargetOwn && !target.isHero)
            {
                // dont destroy owns ;_; (except mins with deathrattle effects)

                Minion m = target;

                if (m.allreadyAttacked)
                {
                    return 50;
                }

                if (name == CardDB.cardName.shadowwordpain)
                {
                    if (CardDB.Instance.specialMinions.ContainsKey(m.name) || m.Angr == 3 || m.Hp >= 4)
                    {
                        return 0;
                    }

                    if (m.Angr == 2) return 5;

                    return 10;
                }

                if (m.Angr >= 4 || m.Hp >= 5)
                {
                    pen = 0; // so we dont destroy cheap ones :D
                }
                else
                {
                    pen = 30;
                }

                if (name == CardDB.cardName.mindcontrol && (m.name == CardDB.cardName.direwolfalpha || m.name == CardDB.cardName.raidleader || m.name == CardDB.cardName.flametonguetotem) && ePlayer.ownMinions.Count == 1)
                {
                    pen = 50;
                }

            }

            return pen;
        }

        private int getSpecialCardComboPenalitys(CardDB.Card card, Minion target, Playfield p, bool lethal, int choice)
        {
                        bool isTargetOwn = false; if (target != null) isTargetOwn = target.own == p.isOwnTurn;

            Player mPlayer, ePlayer;

            if (p.isOwnTurn)
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }
            else
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }

            CardDB.cardName name = card.name;

            if (lethal && card.type == CardDB.cardtype.MOB)
            {
                if (CardDB.Instance.lethalHelpers.ContainsKey(name))
                {
                    return 0;
                }

                if (CardDB.Instance.buffingMinionsDatabase.ContainsKey(name))
                {
                    if (name == CardDB.cardName.timberwolf || name == CardDB.cardName.houndmaster)
                    {
                        int beasts = 0;
                        foreach (Minion mm in mPlayer.ownMinions)
                        {
                            if ((TAG_RACE)mm.handcard.card.race == TAG_RACE.PET) beasts++;
                        }
                        if (beasts == 0) return 500;
                    }
                    if (name == CardDB.cardName.southseacaptain)
                    {
                        int beasts = 0;
                        foreach (Minion mm in mPlayer.ownMinions)
                        {
                            if ((TAG_RACE)mm.handcard.card.race == TAG_RACE.PIRATE) beasts++;
                        }
                        if (beasts == 0) return 500;
                    }
                    if (name == CardDB.cardName.murlocwarleader || name == CardDB.cardName.grimscaleoracle || name == CardDB.cardName.coldlightseer)
                    {
                        int beasts = 0;
                        foreach (Minion mm in mPlayer.ownMinions)
                        {
                            if ((TAG_RACE)mm.handcard.card.race == TAG_RACE.MURLOC) beasts++;
                        }
                        if (beasts == 0) return 500;
                    }
                }
                else
                {
                    if ((name == CardDB.cardName.rendblackhand && target != null) && !isTargetOwn)
                    {
                        if ((target.taunt && target.handcard.card.rarity == 5) || target.handcard.card.name == CardDB.cardName.malganis)
                        {
                            foreach (Handmanager.Handcard hc in mPlayer.owncards)
                            {
                                if ((TAG_RACE)hc.card.race == TAG_RACE.DRAGON) return 0;
                            }
                        }
                        return 500;
                    }

                    if (name == CardDB.cardName.theblackknight)
                    {
                        int beasts = 0;
                        foreach (Minion mm in ePlayer.ownMinions)
                        {
                            if (mm.taunt) return 0;
                        }
                        return 500;
                    }
                    else
                    {
                        if ((CardDB.Instance.HealTargetDatabase.ContainsKey(name) || CardDB.Instance.HealHeroDatabase.ContainsKey(name) || CardDB.Instance.HealAllDatabase.ContainsKey(name)))
                        {
                            int beasts = 0;
                            foreach (Minion mm in mPlayer.ownMinions)
                            {
                                if (mm.Ready && mm.handcard.card.name == CardDB.cardName.lightwarden) beasts++;
                            }
                            if (beasts == 0) return 500;
                        }
                        else
                        {
                            if (!(name == CardDB.cardName.nightblade || card.Charge || CardDB.Instance.silenceDatabase.ContainsKey(name) || ((TAG_RACE)card.race == TAG_RACE.PET && mPlayer.ownMinions.Find(x => x.name == CardDB.cardName.tundrarhino) != null) || (mPlayer.ownMinions.Find(x => x.name == CardDB.cardName.warsongcommander) != null && card.Attack <= 3) || mPlayer.owncards.Find(x => x.card.name == CardDB.cardName.charge) != null))
                            {
                                return 500;
                            }
                        }
                    }
                }
            }

            //lethal end########################################################

            if (card.name == CardDB.cardName.unstableportal && mPlayer.owncards.Count <= 9) return -15;

            if (card.name == CardDB.cardName.daggermastery)
            {
                if (mPlayer.ownWeaponAttack >= 2 || mPlayer.ownWeaponDurability >= 2) return 5;
            }

            if (card.name == CardDB.cardName.upgrade)
            {
                if (mPlayer.ownWeaponDurability == 0)
                {
                    return 16;
                }
            }

            if (card.name == CardDB.cardName.baronrivendare)
            {
                foreach (Minion mnn in mPlayer.ownMinions)
                {
                    if (mnn.name == CardDB.cardName.deathlord || mnn.name == CardDB.cardName.zombiechow || mnn.name == CardDB.cardName.dancingswords) return 30;
                }
            }

            //rule for coin on early game
            if (mPlayer.ownMaxMana < 3 && card.name == CardDB.cardName.thecoin)
            {
                foreach (Handmanager.Handcard hc in mPlayer.owncards)
                {
                    if (hc.manacost <= mPlayer.ownMaxMana && hc.card.type == CardDB.cardtype.MOB) return 5;
                }

            }

            if (name == CardDB.cardName.flare && ePlayer.ownSecretsIDList.Count >= 1 && mPlayer.playactions.Count == 0)
            {
                return -10;
            }

            //some effects, which are bad :D
            int pen = 0;
            if (name == CardDB.cardName.houndmaster)
            {
                if (target == null) return 50;
            }

            if ((card.name == CardDB.cardName.biggamehunter) && (target == null || isTargetOwn))
            {
                return 40;
            }
            if (name == CardDB.cardName.aldorpeacekeeper && target == null)
            {
                pen = 30;
            }

            if (name == CardDB.cardName.emergencycoolant && target != null && isTargetOwn)//dont freeze own minions
            {
                pen = 500;
            }

            if (name == CardDB.cardName.shatteredsuncleric && target == null) { pen = 10; }
            if (name == CardDB.cardName.argentprotector)
            {
                if (target == null) { pen = 20; }
                else
                {
                    if (!isTargetOwn) { return 500; }
                    if (!target.Ready && !target.handcard.card.isSpecialMinion) { pen = 10; }
                    if (!target.Ready && !target.handcard.card.isSpecialMinion && target.Angr <= 2 && target.Hp <= 2) { pen = 15; }
                }

            }

            if (name == CardDB.cardName.facelessmanipulator)
            {
                if (target == null)
                {
                    return 50;
                }
                if (target.Angr >= 5 || target.handcard.card.cost >= 5 || (target.handcard.card.rarity == 5 || target.handcard.card.cost >= 3))
                {
                    return 0;
                }
                return 49;
            }

            if (name == CardDB.cardName.rendblackhand)
            {
                if (target == null)
                {
                    return 50;
                }
                if (isTargetOwn)
                {
                    return 100;
                }
                if ((target.taunt && target.handcard.card.rarity == 5) || target.handcard.card.name == CardDB.cardName.malganis)
                {
                    foreach (Handmanager.Handcard hc in mPlayer.owncards)
                    {
                        if ((TAG_RACE)hc.card.race == TAG_RACE.DRAGON) return 0;
                    }
                }
                return 500;
            }

            if (name == CardDB.cardName.theblackknight)
            {
                if (target == null)
                {
                    return 50;
                }

                foreach (Minion mnn in ePlayer.ownMinions)
                {
                    if (mnn.taunt && (target.Angr >= 3 || target.Hp >= 3)) return 0;
                }
                return 20;
            }

            if (name == CardDB.cardName.madbomber || name == CardDB.cardName.madderbomber)
            {
                pen = 0;
                foreach (Minion mnn in mPlayer.ownMinions)
                {
                    if (mnn.Ready & mnn.Hp < 3) pen += 5;
                }
                return pen;
            }

            //implementation
            if (name == CardDB.cardName.kidnapper && target == null)
            {
                return 15;
            }

            //------------------------------------------------------------------------------------------------------
            Minion m = target;

            if (card.name == CardDB.cardName.reincarnate)
            {
                if (m.own)
                {
                    if (m.handcard.card.deathrattle || m.ancestralspirit >= 1 || m.souloftheforest >= 1 || m.enemyBlessingOfWisdom >= 1) return 0;
                    if (m.handcard.card.Charge && ((m.numAttacksThisTurn == 1 && !m.windfury) || (m.numAttacksThisTurn == 2 && m.windfury))) return 0;
                    if (m.wounded || m.Angr < m.handcard.card.Attack || (m.silenced && CardDB.Instance.specialMinions.ContainsKey(m.name))) return 0;


                    bool hasOnMinionDiesMinion = false;
                    foreach (Minion mnn in mPlayer.ownMinions)
                    {
                        if (mnn.name == CardDB.cardName.scavenginghyena && m.handcard.card.race == 20) hasOnMinionDiesMinion = true;
                        if (mnn.name == CardDB.cardName.flesheatingghoul || mnn.name == CardDB.cardName.cultmaster) hasOnMinionDiesMinion = true;
                    }
                    if (hasOnMinionDiesMinion) return 0;

                    return 500;
                }
                else
                {
                    if (m.name == CardDB.cardName.nerubianegg && m.Angr <= 4 && !m.taunt) return 500;
                    if (m.taunt && !m.handcard.card.tank) return 0;
                    if (m.enemyBlessingOfWisdom >= 1) return 0;
                    if (m.Angr > m.handcard.card.Attack || m.Hp > m.handcard.card.Health) return 0;
                    if (m.name == CardDB.cardName.abomination || m.name == CardDB.cardName.zombiechow || m.name == CardDB.cardName.unstableghoul || m.name == CardDB.cardName.dancingswords) return 0;
                    return 500;

                }

            }

            if (mPlayer.ownHeroName == HeroEnum.shaman && mPlayer.ownAbilityReady == false)
            {
                return 20;
            }

            if (card.name == CardDB.cardName.flametonguetotem && mPlayer.ownMinions.Count == 0)
            {
                return 100;
            }

            if (card.name == CardDB.cardName.stampedingkodo)
            {
                bool found = false;
                foreach (Minion mi in ePlayer.ownMinions)
                {
                    if (mi.Angr <= 2) found = true;
                }
                if (!found) return 20;
            }

            if (name == CardDB.cardName.windfury)
            {
                if (!m.own) return 500;
                if (m.own && !m.Ready) return 500;
            }

            if ((name == CardDB.cardName.wildgrowth || name == CardDB.cardName.nourish) && mPlayer.ownMaxMana == 9 && !(mPlayer.ownHeroName == HeroEnum.thief && mPlayer.cardsPlayedThisTurn == 0))
            {
                return 500;
            }

            if (name == CardDB.cardName.ancestralspirit)
            {
                if (!isTargetOwn && !target.isHero)
                {
                    if (m.name == CardDB.cardName.deathlord || m.name == CardDB.cardName.zombiechow || m.name == CardDB.cardName.dancingswords) return 0;
                    return 500;
                }
                if (isTargetOwn && !target.isHero)
                {
                    if (CardDB.Instance.specialMinions.ContainsKey(m.name)) return -5;
                    return 0;
                }

            }

            if (name == CardDB.cardName.sap || name == CardDB.cardName.dream || name == CardDB.cardName.kidnapper)
            {
                if (!m.own && (m.name == CardDB.cardName.theblackknight || name == CardDB.cardName.rendblackhand))
                {
                    return 50;
                }
            }

            if (name == CardDB.cardName.sylvanaswindrunner)
            {
                if (ePlayer.ownMinions.Count == 0)
                {
                    return 10;
                }
            }

            if (name == CardDB.cardName.betrayal && !isTargetOwn && !target.isHero)
            {
                if (m.Angr == 0) return 30;
                if (ePlayer.ownMinions.Count == 1) return 30;
            }




            if (name == CardDB.cardName.bite)
            {
                if ((mPlayer.ownHero.numAttacksThisTurn == 0 || (mPlayer.ownHero.windfury && mPlayer.ownHero.numAttacksThisTurn == 1)) && !mPlayer.ownHero.frozen)
                {

                }
                else
                {
                    return 20;
                }
            }

            if (name == CardDB.cardName.deadlypoison)
            {
                return mPlayer.ownWeaponDurability * 2;
            }

            if (name == CardDB.cardName.coldblood)
            {
                if (lethal) return 0;
                return 25;
            }

            if (name == CardDB.cardName.bloodmagethalnos)
            {
                return 10;
            }

            if (name == CardDB.cardName.frostbolt)
            {
                if (!isTargetOwn && !target.isHero)
                {
                    if (m.handcard.card.cost <= 2)
                        return 15;
                }
                return 15;
            }

            if (!lethal && choice == 1 && name == CardDB.cardName.druidoftheclaw)
            {
                return 20;
            }


            if (name == CardDB.cardName.poweroverwhelming)
            {
                if (isTargetOwn && !target.isHero && !m.Ready)
                {
                    return 500;
                }
            }

            if (name == CardDB.cardName.frothingberserker)
            {
                if (mPlayer.cardsPlayedThisTurn >= 1) pen = 5;
            }

            if (name == CardDB.cardName.handofprotection)
            {
                if (m.Hp == 1) pen = 15;
            }

            if (lethal)
            {
                if (name == CardDB.cardName.corruption)
                {
                    int beasts = 0;
                    foreach (Minion mm in mPlayer.ownMinions)
                    {
                        if (mm.Ready && (mm.handcard.card.name == CardDB.cardName.questingadventurer || mm.handcard.card.name == CardDB.cardName.archmageantonidas || mm.handcard.card.name == CardDB.cardName.manaaddict || mm.handcard.card.name == CardDB.cardName.manawyrm || mm.handcard.card.name == CardDB.cardName.wildpyromancer)) beasts++;
                    }
                    if (beasts == 0) return 500;
                }
            }

            if (name == CardDB.cardName.divinespirit)
            {
                if (lethal)
                {
                    if (!isTargetOwn && !target.isHero)
                    {
                        if (!m.taunt)
                        {
                            return 500;
                        }
                        else
                        {
                            // combo for killing with innerfire and biggamehunter
                            if (mPlayer.owncards.Find(x => x.card.name == CardDB.cardName.biggamehunter) != null && mPlayer.owncards.Find(x => x.card.name == CardDB.cardName.innerfire) != null && (m.Hp >= 4 || (mPlayer.owncards.Find(x => x.card.name == CardDB.cardName.divinespirit) != null && m.Hp >= 2)))
                            {
                                return 0;
                            }
                            return 500;
                        }
                    }
                }
                else
                {
                    if (!isTargetOwn && !target.isHero)
                    {

                        // combo for killing with innerfire and biggamehunter
                        if (mPlayer.owncards.Find(x => x.card.name == CardDB.cardName.biggamehunter) != null && mPlayer.owncards.Find(x => x.card.name == CardDB.cardName.innerfire) != null && m.Hp >= 4)
                        {
                            return 0;
                        }
                        return 500;
                    }

                }

                if (isTargetOwn && !target.isHero)
                {

                    if (m.Hp >= 4)
                    {
                        return 0;
                    }
                    return 15;
                }

            }

            if (name == CardDB.cardName.gangup)
            {
                int penTmP = 0;
                if (CardDB.Instance.UsefulNeedKeepDatabase.ContainsKey(name))
                {
                    penTmP = CardDB.Instance.UsefulNeedKeepDatabase[name] - target.maxHp - mPlayer.ownMaxMana + target.handcard.card.cost;
                    penTmP = -1 * Math.Min(penTmP, 0);
                }
                else
                {
                    penTmP = 40;
                }
                return penTmP;
            }

            if (name == CardDB.cardName.resurrect)
            {
                if (mPlayer.ownMaxMana < 6) return 50;
                if (mPlayer.ownMinions.Count == 7) return 500;
                if (mPlayer.ownMaxMana > 8) return 0;
                if (mPlayer.ownLastDiedMinion == CardDB.cardIDEnum.None) return 6;
                return 0;
            }

            if (name == CardDB.cardName.lavashock)
            {
                if (p.ueberladung < 1) return 15;
                return (3 - 3 * p.ueberladung);
            }

            if (name == CardDB.cardName.enhanceomechano)
            {
                if (mPlayer.ownMinions.Count == 0 && mPlayer.ownMaxMana < 5) return 500;
                int penTmP = 2 * (mPlayer.mana - 4 - mPlayer.mobsplayedThisTurn); //for accurate calculation we need hc
                if (mPlayer.mobsplayedThisTurn < 1) penTmP += 30;
                return penTmP;
            }

            if (name == CardDB.cardName.knifejuggler && mPlayer.mobsplayedThisTurn >= 1)
            {
                return 20;
            }

            if (name == CardDB.cardName.flamewaker)
            {
                foreach (Action a in mPlayer.playactions)
                {
                    if (a.actionType == actionEnum.playcard && a.card.card.type == CardDB.cardtype.SPELL) return 30;
                }
            }

            if ((name == CardDB.cardName.polymorph || name == CardDB.cardName.hex))
            {
                if (isTargetOwn && !target.isHero)
                {
                    return 500;
                }

                if (!isTargetOwn && !target.isHero)
                {
                    if (target.allreadyAttacked) return 30;
                    Minion frog = target;
                    if (CardDB.Instance.priorityTargets.ContainsKey(frog.name)) return 0;
                    if (frog.Angr >= 4 && frog.Hp >= 4) return 0;
                    return 30;
                }
            }


            if ((name == CardDB.cardName.defenderofargus || name == CardDB.cardName.sunfuryprotector) && mPlayer.ownMinions.Count == 1)
            {
                return 40;
            }
            if ((name == CardDB.cardName.defenderofargus || name == CardDB.cardName.sunfuryprotector) && mPlayer.ownMinions.Count == 0)
            {
                return 50;
            }

            if (name == CardDB.cardName.unleashthehounds)
            {
                if (ePlayer.ownMinions.Count <= 1)
                {
                    return 20;
                }
            }

            if (name == CardDB.cardName.equality) // aoe penality
            {
                if (ePlayer.ownMinions.Count <= 2 || (mPlayer.ownMinions.Count - ePlayer.ownMinions.Count >= 1))
                {
                    return 20;
                }
            }

            if (name == CardDB.cardName.bloodsailraider && mPlayer.ownWeaponDurability == 0)
            {
                //if you have bloodsailraider and no weapon equiped, but own a weapon:
                foreach (Handmanager.Handcard hc in mPlayer.owncards)
                {
                    if (hc.card.type == CardDB.cardtype.WEAPON) return 10;
                }
            }

            if (name == CardDB.cardName.acidicswampooze)
            {
                if (ePlayer.ownWeaponAttack >= 1) return 0;
                if (ePlayer.ownHeroName == HeroEnum.warrior || ePlayer.ownHeroName == HeroEnum.thief || ePlayer.ownHeroName == HeroEnum.pala ) return 10;
                if (ePlayer.ownHeroName == HeroEnum.shaman || ePlayer.ownHeroName == HeroEnum.hunter) return 6;
            }

            if (name == CardDB.cardName.innerfire)
            {
                if (m.name == CardDB.cardName.lightspawn) pen = 500;
            }

            if (name == CardDB.cardName.huntersmark)
            {
                if (isTargetOwn && !target.isHero) pen = 500; // dont use on own minions
                if (!isTargetOwn && !target.isHero && (target.Hp <= 4) && target.Angr <= 4) // only use on strong minions
                {
                    pen = 20;
                }
            }


            if ((name == CardDB.cardName.aldorpeacekeeper || name == CardDB.cardName.humility))
            {
                if (target != null)
                {
                    if (isTargetOwn) pen = 500; // dont use on own minions
                    if (!isTargetOwn && target.Angr <= 3) // only use on strong minions
                    {
                        pen = 30;
                    }
                    if (m.name == CardDB.cardName.lightspawn) pen = 500;
                }
                else
                {
                    pen = 50;
                }
            }



            if (name == CardDB.cardName.defiasringleader && mPlayer.cardsPlayedThisTurn == 0)
            { pen = 10; }
            if (name == CardDB.cardName.bloodknight)
            {
                int shilds = 0;
                foreach (Minion min in mPlayer.ownMinions)
                {
                    if (min.divineshild)
                    {
                        shilds++;
                    }
                }
                foreach (Minion min in ePlayer.ownMinions)
                {
                    if (min.divineshild)
                    {
                        shilds++;
                    }
                }
                if (shilds == 0)
                {
                    pen = 10;
                }
            }
            if (name == CardDB.cardName.direwolfalpha)
            {
                int ready = 0;
                foreach (Minion min in mPlayer.ownMinions)
                {
                    if (min.Ready)
                    { ready++; }
                }
                if (ready == 0)
                { pen = 5; }
            }
            if (name == CardDB.cardName.abusivesergeant)
            {
                int ready = 0;
                foreach (Minion min in mPlayer.ownMinions)
                {
                    if (min.Ready)
                    { ready++; }
                }
                if (ready == 0)
                {
                    pen = 5;
                }
            }


            if (CardDB.Instance.returnHandDatabase.ContainsKey(name))
            {
                if (name == CardDB.cardName.vanish)
                {
                    //dont vanish if we have minons on board wich are ready
                    bool haveready = false;
                    foreach (Minion mins in mPlayer.ownMinions)
                    {
                        if (mins.Ready) haveready = true;
                    }
                    if (haveready) pen += 10;
                }

                if (isTargetOwn && !target.isHero)
                {
                    Minion mnn = target;
                    if (mnn.Ready) pen += 10;
                }
            }

            return pen;
        }

        private int playSecretPenality(CardDB.Card card, Playfield p)
        {
            Player mPlayer, ePlayer;

            if (p.isOwnTurn)
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }
            else
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }

            //penality if we play secret and have playable kirintormage
            int pen = 0;
            if (card.Secret)
            {
                foreach (Handmanager.Handcard hc in mPlayer.owncards)
                {
                    if (hc.card.name == CardDB.cardName.kirintormage && mPlayer.mana >= hc.getManaCost(p, p.isOwnTurn))
                    {
                        pen = 500;
                    }
                }
            }

            return pen;
        }

        ///secret strategys pala
        /// -Attack lowest enemy. If you can’t, use noncombat means to kill it. 
        /// -attack with something able to withstand 2 damage. 
        /// -Then play something that had low health to begin with to dodge Repentance. 
        /// 
        ///secret strategys hunter
        /// - kill enemys with your minions with 2 or less heal.
        ///  - Use the smallest minion available for the first attack 
        ///  - Then smack them in the face with whatever’s left. 
        ///  - If nothing triggered until then, it’s a Snipe, so throw something in front of it that won’t die or is expendable.
        /// 
        ///secret strategys mage
        /// - Play a small minion to trigger Mirror Entity.
        /// Then attack the mage directly with the smallest minion on your side. 
        /// If nothing triggered by that point, it’s either Spellbender or Counterspell, so hold your spells until you can (and have to!) deal with either. 

        private int getPlayCardSecretPenality(CardDB.Card c, Playfield p)
        {
            Player mPlayer, ePlayer;

            if (p.isOwnTurn)
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }
            else
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }

            int pen = 0;
            if (ePlayer.ownSecretsIDList.Count == 0)
            {
                return 0;
            }

            if (c.name == CardDB.cardName.flare)
            {
                return 0;
            }

            int attackedbefore = 0;

            foreach (Minion mnn in mPlayer.ownMinions)
            {
                if (mnn.numAttacksThisTurn >= 1)
                {
                    attackedbefore++;
                }
            }

            if (c.name == CardDB.cardName.acidicswampooze
                && (ePlayer.ownHeroStartClass == TAG_CLASS.WARRIOR || ePlayer.ownHeroStartClass == TAG_CLASS.ROGUE || ePlayer.ownHeroStartClass == TAG_CLASS.PALADIN))
            {
                if (ePlayer.ownHeroStartClass == TAG_CLASS.ROGUE && ePlayer.ownWeaponAttack <= 2)
                {
                    pen += 100;
                }
                else
                {
                    if (ePlayer.ownWeaponAttack <= 1)
                    {
                        pen += 100;
                    }
                }
            }

            if (ePlayer.ownHeroStartClass == TAG_CLASS.HUNTER)
            {
                if (c.type == CardDB.cardtype.MOB
                    && (attackedbefore == 0 || c.Health <= 4
                        || (ePlayer.ownHero.Hp >= ePlayer.ownHeroHpStarted && attackedbefore >= 1)))
                {
                    pen += 10;
                }
            }

            if (ePlayer.ownHeroStartClass == TAG_CLASS.MAGE)
            {
                if (c.type == CardDB.cardtype.MOB)
                {
                    Minion m = new Minion
                    {
                        Hp = c.Health,
                        maxHp = c.Health,
                        Angr = c.Attack,
                        taunt = c.tank,
                        name = c.name
                    };

                    // play first the small minion:
                    if ((!this.isOwnLowestInHand(m, p) && mPlayer.mobsplayedThisTurn == 0)
                        || (mPlayer.mobsplayedThisTurn == 0 && attackedbefore >= 1))
                    {
                        pen += 10;
                    }
                }

                if (c.type == CardDB.cardtype.SPELL && mPlayer.cardsPlayedThisTurn == mPlayer.mobsplayedThisTurn)
                {
                    pen += 10;
                }
            }

            if (ePlayer.ownHeroStartClass == TAG_CLASS.PALADIN)
            {
                if (c.type == CardDB.cardtype.MOB)
                {
                    Minion m = new Minion
                    {
                        Hp = c.Health,
                        maxHp = c.Health,
                        Angr = c.Attack,
                        taunt = c.tank,
                        name = c.name
                    };
                    if ((!this.isOwnLowestInHand(m, p) && mPlayer.mobsplayedThisTurn == 0) || attackedbefore == 0)
                    {
                        pen += 10;
                    }
                }
            }

            return pen;
        }

        private int getAttackSecretPenality(Minion m, Playfield p, Minion target)
        {
            Player mPlayer, ePlayer;

                        bool isTargetOwn = false; if (target != null) isTargetOwn = target.own == p.isOwnTurn;

            if (p.isOwnTurn)
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }
            else
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }

            if (ePlayer.ownSecretsIDList.Count == 0)
            {
                return 0;
            }

            int pen = 0;

            int attackedbefore = 0;

            foreach (Minion mnn in mPlayer.ownMinions)
            {
                if (mnn.numAttacksThisTurn >= 1) attackedbefore++;
            }

            if (ePlayer.ownHeroStartClass == TAG_CLASS.HUNTER)
            {
                bool islow = isOwnLowest(m, p);
                if (attackedbefore == 0 && islow) pen -= 20;
                if (attackedbefore == 0 && !islow) pen += 10;

                if (target.isHero && !isTargetOwn && ePlayer.ownMinions.Count >= 1)
                {
                    //penality if we doestn attacked before
                    if (hasMinionsWithLowHeal(p)) pen += 10; //penality if we doestn attacked minions before
                }
            }

            if (ePlayer.ownHeroStartClass == TAG_CLASS.MAGE)
            {
                if (mPlayer.mobsplayedThisTurn == 0) pen += 10;

                bool islow = isOwnLowest(m, p);

                if (target.isHero && !isTargetOwn && !islow)
                {
                    pen += 10;
                }
                if (target.isHero && !isTargetOwn && islow && mPlayer.mobsplayedThisTurn >= 1)
                {
                    pen -= 20;
                }

            }

            if (ePlayer.ownHeroStartClass == TAG_CLASS.PALADIN)
            {

                bool islow = isOwnLowest(m, p);

                if (!isTargetOwn && !target.isHero && attackedbefore == 0)
                {
                    if (!isEnemyLowest(target, p) || m.Hp <= 2) pen += 5;
                }

                if (target.isHero && !isTargetOwn && !islow)
                {
                    pen += 5;
                }

                if (target.isHero && !isTargetOwn && ePlayer.ownMinions.Count >= 1 && attackedbefore == 0)
                {
                    pen += 5;
                }

            }


            return pen;
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

        public int getValueOfUsefulNeedKeepPriority(CardDB.cardName name)
        {
            int ret = 0;
            if (CardDB.Instance.UsefulNeedKeepDatabase.ContainsKey(name)) ret = CardDB.Instance.UsefulNeedKeepDatabase[name];
            return ret;
        }


        private int getValueOfMinion(Minion m)
        {
            int ret = 0;
            ret += 2 * m.Angr + m.Hp;
            if (m.taunt) ret += 2;
            if (CardDB.Instance.priorityDatabase.ContainsKey(m.name)) ret += 20 + CardDB.Instance.priorityDatabase[m.name];
            return ret;
        }

        private bool isOwnLowest(Minion mnn, Playfield p)
        {

            Player mPlayer, ePlayer;

            if (p.isOwnTurn)
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }
            else
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }

            bool ret = true;
            int val = getValueOfMinion(mnn);
            foreach (Minion m in mPlayer.ownMinions)
            {
                if (!m.Ready) continue;
                if (getValueOfMinion(m) < val) ret = false;
            }
            return ret;
        }

        private bool isOwnLowestInHand(Minion mnn, Playfield p)
        {

            Player mPlayer, ePlayer;

            if (p.isOwnTurn)
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }
            else
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }

            bool ret = true;
            Minion m = new Minion();
            int val = getValueOfMinion(mnn);
            foreach (Handmanager.Handcard card in mPlayer.owncards)
            {
                if (card.card.type != CardDB.cardtype.MOB) continue;
                CardDB.Card c = card.card;
                m.Hp = c.Health;
                m.maxHp = c.Health;
                m.Angr = c.Attack;
                m.taunt = c.tank;
                m.name = c.name;
                if (getValueOfMinion(m) < val) ret = false;
            }
            return ret;
        }

        private int getValueOfEnemyMinion(Minion m)
        {
            int ret = 0;
            ret += m.Hp;
            if (m.taunt) ret -= 2;
            return ret;
        }

        private bool isEnemyLowest(Minion mnn, Playfield p)
        {

            Player mPlayer, ePlayer;

            if (p.isOwnTurn)
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }
            else
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }

            bool ret = true;
            List<Minion> litt = p.getAttackTargets(true, false);
            int val = getValueOfEnemyMinion(mnn);
            foreach (Minion m in ePlayer.ownMinions)
            {
                if (litt.Find(x => x.entitiyID == m.entitiyID) == null) continue;
                if (getValueOfEnemyMinion(m) < val) ret = false;
            }
            return ret;
        }

        private bool hasMinionsWithLowHeal(Playfield p)
        {

            Player mPlayer, ePlayer;

            if (p.isOwnTurn)
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }
            else
            {
                mPlayer = p.playerFirst;
                ePlayer = p.playerSecond;
            }

            bool ret = false;
            foreach (Minion m in mPlayer.ownMinions)
            {
                if (m.Hp <= 2 && (m.Ready || CardDB.Instance.priorityDatabase.ContainsKey(m.name))) ret = true;
            }
            return ret;
        }

   


    }

}