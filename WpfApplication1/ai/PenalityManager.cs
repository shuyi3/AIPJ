﻿namespace HREngine.Bots
{
    using System;
    using System.Collections.Generic;

    public class PenalityManager
    {
        //todo acolyteofpain
        //todo better aoe-penality

        ComboBreaker cb;


        Dictionary<CardDB.cardName, int> HealTargetDatabase = new Dictionary<CardDB.cardName, int>();
        Dictionary<CardDB.cardName, int> HealHeroDatabase = new Dictionary<CardDB.cardName, int>();
        Dictionary<CardDB.cardName, int> HealAllDatabase = new Dictionary<CardDB.cardName, int>();


        Dictionary<CardDB.cardName, int> DamageAllDatabase = new Dictionary<CardDB.cardName, int>();
        Dictionary<CardDB.cardName, int> DamageHeroDatabase = new Dictionary<CardDB.cardName, int>();
        Dictionary<CardDB.cardName, int> DamageRandomDatabase = new Dictionary<CardDB.cardName, int>();
        Dictionary<CardDB.cardName, int> DamageAllEnemysDatabase = new Dictionary<CardDB.cardName, int>();

        Dictionary<CardDB.cardName, int> enrageDatabase = new Dictionary<CardDB.cardName, int>();
        Dictionary<CardDB.cardName, int> silenceDatabase = new Dictionary<CardDB.cardName, int>();
        Dictionary<CardDB.cardName, int> OwnNeedSilenceDatabase = new Dictionary<CardDB.cardName, int>();

        Dictionary<CardDB.cardName, int> heroAttackBuffDatabase = new Dictionary<CardDB.cardName, int>();
        Dictionary<CardDB.cardName, int> attackBuffDatabase = new Dictionary<CardDB.cardName, int>();
        Dictionary<CardDB.cardName, int> healthBuffDatabase = new Dictionary<CardDB.cardName, int>();
        Dictionary<CardDB.cardName, int> tauntBuffDatabase = new Dictionary<CardDB.cardName, int>();

        Dictionary<CardDB.cardName, int> lethalHelpers = new Dictionary<CardDB.cardName, int>();
        
        Dictionary<CardDB.cardName, int> spellDependentDatabase = new Dictionary<CardDB.cardName, int>();

        Dictionary<CardDB.cardName, int> cardDiscardDatabase = new Dictionary<CardDB.cardName, int>();
        Dictionary<CardDB.cardName, int> destroyOwnDatabase = new Dictionary<CardDB.cardName, int>();
        Dictionary<CardDB.cardName, int> destroyDatabase = new Dictionary<CardDB.cardName, int>();
        Dictionary<CardDB.cardName, int> buffingMinionsDatabase = new Dictionary<CardDB.cardName, int>();
        Dictionary<CardDB.cardName, int> buffing1TurnDatabase = new Dictionary<CardDB.cardName, int>();
        Dictionary<CardDB.cardName, int> heroDamagingAoeDatabase = new Dictionary<CardDB.cardName, int>();
        Dictionary<CardDB.cardName, int> randomEffects = new Dictionary<CardDB.cardName, int>();

        Dictionary<CardDB.cardName, int> silenceTargets = new Dictionary<CardDB.cardName, int>();

        Dictionary<CardDB.cardName, int> returnHandDatabase = new Dictionary<CardDB.cardName, int>();

        Dictionary<CardDB.cardName, int> priorityDatabase = new Dictionary<CardDB.cardName, int>();
        Dictionary<CardDB.cardName, int> UsefulNeedKeepDatabase = new Dictionary<CardDB.cardName, int>();
        Dictionary<CardDB.cardName, CardDB.cardIDEnum> choose1database = new Dictionary<CardDB.cardName, CardDB.cardIDEnum>();
        Dictionary<CardDB.cardName, CardDB.cardIDEnum> choose2database = new Dictionary<CardDB.cardName, CardDB.cardIDEnum>();

        public Dictionary<CardDB.cardName, int> DamageTargetDatabase = new Dictionary<CardDB.cardName, int>();
        public Dictionary<CardDB.cardName, int> DamageTargetSpecialDatabase = new Dictionary<CardDB.cardName, int>();
        public Dictionary<CardDB.cardName, int> cardDrawBattleCryDatabase = new Dictionary<CardDB.cardName, int>();
        public Dictionary<CardDB.cardName, int> priorityTargets = new Dictionary<CardDB.cardName, int>();
        public Dictionary<CardDB.cardName, int> specialMinions = new Dictionary<CardDB.cardName, int>(); //minions with cardtext, but no battlecry


        private static PenalityManager instance;

        public static PenalityManager Instance
        {
            get
            {
                return instance ?? (instance = new PenalityManager());
            }
        }

        private PenalityManager()
        {
            setupHealDatabase();
            setupEnrageDatabase();
            setupDamageDatabase();
            setupPriorityList();
            setupsilenceDatabase();
            setupAttackBuff();
            setupHealthBuff();
            setupCardDrawBattlecry();
            setupDiscardCards();
            setupDestroyOwnCards();
            setupSpecialMins();
            setupEnemyTargetPriority();
            setupHeroDamagingAOE();
            setupBuffingMinions();
            setupRandomCards();
            setupLethalHelpMinions();
            setupSilenceTargets();
            setupUsefulNeedKeepDatabase();
            setupRelations();
            setupChooseDatabase();
        }

        public void setCombos()
        {
            this.cb = ComboBreaker.Instance;
        }

        public int getAttackWithMininonPenality(Minion m, Playfield p, Minion target, bool lethal)
        {
            int pen = 0;
            pen = getAttackSecretPenality(m, p, target);
            if (!lethal && m.name == CardDB.cardName.bloodimp) pen = 50;
            if (m.name == CardDB.cardName.leeroyjenkins)
            {
                if (!target.own)
                {
                    if (target.name == CardDB.cardName.whelp) return 500;
                }

            }
            return pen;
        }

        public int getAttackWithHeroPenality(Minion target, Playfield p, bool leathal)
        {
            int retval = 0;

            if (!leathal && p.ownWeaponName == CardDB.cardName.swordofjustice)
            {
                return 28;
            }

            if (p.ownWeaponDurability == 1 && p.ownWeaponName == CardDB.cardName.eaglehornbow)
            {
                foreach (Handmanager.Handcard hc in p.owncards)
                {
                    if (hc.card.name == CardDB.cardName.arcaneshot || hc.card.name == CardDB.cardName.killcommand) return -p.ownWeaponAttack - 1;
                }
                if (p.ownSecretsIDList.Count >= 1) return 20;

                foreach (Handmanager.Handcard hc in p.owncards)
                {
                    if (hc.card.Secret) return 20;
                }
            }

            //no penality, but a bonus, if he has weapon on hand!
            if (target.isHero && !target.own && p.ownWeaponName == CardDB.cardName.gorehowl && p.ownWeaponAttack >= 3)
            {
                return 10;
            }
            if (p.ownWeaponDurability >= 1)
            {
                bool hasweapon = false;
                foreach (Handmanager.Handcard c in p.owncards)
                {
                    if (c.card.type == CardDB.cardtype.WEAPON) hasweapon = true;
                }
                if (p.ownWeaponAttack == 1 && p.ownHeroName == HeroEnum.thief) hasweapon = true;
                if (hasweapon) retval = -p.ownWeaponAttack - 1; // so he doesnt "lose" the weapon in evaluation :D
            }
            if (p.ownWeaponAttack == 1 && p.ownHeroName == HeroEnum.thief) retval += -1;
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
                retval += cb.getPenalityForDestroyingCombo(card, p);
                retval += cb.getPlayValue(card.cardIDenum);
            }

            retval += playSecretPenality(card, p);
            retval += getPlayCardSecretPenality(card, p);

            //Helpfunctions.Instance.ErrorLog("retval " + retval);
            return retval;
        }

        private int getAttackBuffPenality(CardDB.Card card, Minion target, Playfield p, int choice, bool lethal)
        {
            CardDB.cardName name = card.name;
            if (name == CardDB.cardName.darkwispers && choice != 2) return 0;
            int pen = 0;
            //buff enemy?

            if (!lethal && (card.name == CardDB.cardName.savageroar || card.name == CardDB.cardName.bloodlust))
            {
                int targets = 0;
                foreach (Minion m in p.ownMinions)
                {
                    if (m.Ready) targets++;
                }
                if ((p.ownHero.Ready || p.ownHero.numAttacksThisTurn == 0) && card.name == CardDB.cardName.savageroar) targets++;

                if (targets <= 2)
                {
                    return 20;
                }
            }

            if (!this.attackBuffDatabase.ContainsKey(name)) return 0;
            if (target == null) return 60;
            if (!target.isHero && !target.own)
            {
                if (card.type == CardDB.cardtype.MOB && p.ownMinions.Count == 0) return 0;
                //allow it if you have biggamehunter
                foreach (Handmanager.Handcard hc in p.owncards)
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
                        foreach (Handmanager.Handcard hc in p.owncards)
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
            if (!target.isHero && target.own)
            {
                Minion m = target;
                if (!m.Ready)
                {
                    return 50;
                }
                if (m.Hp == 1 && !m.divineshild && !this.buffing1TurnDatabase.ContainsKey(name))
                {
                    return 10;
                }
            }

            if (card.name == CardDB.cardName.blessingofmight) return 6;
            return pen;
        }

        private int getHPBuffPenality(CardDB.Card card, Minion target, Playfield p, int choice)
        {


            CardDB.cardName name = card.name;
            if (name == CardDB.cardName.darkwispers && choice != 2) return 0;

            // Drew: We need a target for this card!
            if (name == CardDB.cardName.darkwispers && target == null) return 500;

            int pen = 0;

            //buff enemy?
            if (!this.healthBuffDatabase.ContainsKey(name)) return 0;

            // Drew: We add a null check here to avoid interfering with other logic with the previous conditional statement.
            if (target != null && !target.own && !this.tauntBuffDatabase.ContainsKey(name))
            {
                pen = 500;
            }

            return pen;
        }


        private int getTauntBuffPenality(CardDB.cardName name, Minion target, Playfield p, int choice)
        {
            int pen = 0;
            //buff enemy?
            if (!this.tauntBuffDatabase.ContainsKey(name)) return 0;
            if (name == CardDB.cardName.markofnature && choice != 2) return 0;
            if (name == CardDB.cardName.darkwispers && choice != 2) return 0;

            // Drew: We need a target for this card!
            if (name == CardDB.cardName.darkwispers && target == null) return 500;

            if (!target.isHero && !target.own)
            {
                //allow it if you have black knight
                foreach (Handmanager.Handcard hc in p.owncards)
                {
                    if (hc.card.name == CardDB.cardName.theblackknight) return 0;
                }

                // allow taunting if target is priority and others have taunt
                bool enemyhasTaunts = false;
                foreach (Minion mnn in p.enemyMinions)
                {
                    if (mnn.taunt)
                    {
                        enemyhasTaunts = true;
                        break;
                    }
                }
                if (enemyhasTaunts && this.priorityDatabase.ContainsKey(target.name) && !target.silenced && !target.taunt)
                {
                    return 0;
                }

                pen = 500;
            }

            return pen;
        }

        private int getSilencePenality(CardDB.cardName name, Minion target, Playfield p, int choice, bool lethal)
        {
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

            if (target.own)
            {
                if (this.silenceDatabase.ContainsKey(name))
                {
                    // no pen if own is enrage
                    if (!target.silenced && this.OwnNeedSilenceDatabase.ContainsKey(target.name))
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
            else if (!target.own)
            {
                if (this.silenceDatabase.ContainsKey(name))
                {
                    pen = 5;
                    if (lethal)
                    {
                        //during lethal we only silence taunt, or if its a mob (owl/spellbreaker) + we can give him charge
                        if (target.taunt || (name == CardDB.cardName.ironbeakowl && (p.ownMinions.Find(x => x.name == CardDB.cardName.tundrarhino) != null || p.ownMinions.Find(x => x.name == CardDB.cardName.warsongcommander) != null || p.owncards.Find(x => x.card.name == CardDB.cardName.charge) != null)) || (name == CardDB.cardName.spellbreaker && p.owncards.Find(x => x.card.name == CardDB.cardName.charge) != null)) return 0;

                        return 500;
                    }

                    if (!target.silenced && this.OwnNeedSilenceDatabase.ContainsKey(target.name))
                    {
                        if (target.taunt) pen += 15;
                        return 500;
                    }

                    if (!target.silenced)
                    {
                        if (this.priorityDatabase.ContainsKey(target.name)) return 0;
                        if (this.silenceTargets.ContainsKey(target.name)) return 0;
                        if (target.handcard.card.deathrattle) return 0;
                    }

                    if (target.Angr <= target.handcard.card.Attack && target.maxHp <= target.handcard.card.Health && !target.taunt && !target.windfury && !target.divineshild && !target.poisonous && !this.specialMinions.ContainsKey(name))
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
            CardDB.cardName name = card.name;
            int pen = 0;

            if (name == CardDB.cardName.shieldslam && p.ownHero.armor == 0) return 500;
            if (name == CardDB.cardName.savagery && p.ownHero.Angr == 0) return 500;
            if (name == CardDB.cardName.keeperofthegrove && choice != 1) return 0; // look at silence penality
            
            //aoe damage *************************************************************************************
            int aoeDamageType = 0;
            if (this.DamageAllEnemysDatabase.ContainsKey(name)) aoeDamageType = 1;
            else if (p.anzOwnAuchenaiSoulpriest >= 1 && HealAllDatabase.ContainsKey(name)) aoeDamageType = 2;
            else if (this.DamageAllDatabase.ContainsKey(name)) aoeDamageType = 3;
            if (aoeDamageType > 0)
            {
                if (p.enemyMinions.Count == 0) return 300;

                int aoeDamage = 0;
                if (aoeDamageType == 1) aoeDamage = (card.type == CardDB.cardtype.SPELL) ? p.getSpellDamageDamage(this.DamageAllEnemysDatabase[name]) : this.DamageAllEnemysDatabase[name];
                else if (aoeDamageType == 2) aoeDamage = (card.type == CardDB.cardtype.SPELL) ? p.getSpellDamageDamage(this.HealAllDatabase[name]) : this.HealAllDatabase[name];
                else if (aoeDamageType == 3)
                {
                    if (name == CardDB.cardName.revenge && p.ownHero.Hp <= 12) p.getSpellDamageDamage(3);
                    else aoeDamage = (card.type == CardDB.cardtype.SPELL) ? p.getSpellDamageDamage(this.DamageAllDatabase[name]) : this.DamageAllDatabase[name];
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
                foreach (Minion m in p.enemyMinions)
                {
                    if (aoeDamage >= m.Hp && (name == CardDB.cardName.demonwrath ? ((TAG_RACE)m.handcard.card.race == TAG_RACE.DEMON ? false : true) : true)) preventDamage += m.Angr;
                    else
                    {
                        survivedEnemyMinions++;
                        if (survivedEnemyMinionsAngr < m.Angr) survivedEnemyMinionsAngr = m.Angr;
                        if (!m.wounded && this.enrageDatabase.ContainsKey(name)) preventDamage -= this.enrageDatabase[name];
                        else if (m.name == CardDB.cardName.gurubashiberserker) preventDamage -= 3;
                        else if (m.name == CardDB.cardName.frothingberserker) frothingberserkerEnemy = true;
                        else if (m.name == CardDB.cardName.grimpatron) { preventDamage -= 3; grimpatronEnemy = true; }
                    }
                }
                int ownWoundedMinions = 0;
                foreach (Minion m in p.ownMinions)
                {
                    if (m.wounded) ownWoundedMinions++;
                    if (aoeDamage >= m.Hp && (name == CardDB.cardName.demonwrath ? ((TAG_RACE)m.handcard.card.race == TAG_RACE.DEMON ? false : true ) : true))
                    {
                        lostOwnDamage += m.Angr;
                        lostOwnMinions++;
                        if (!m.wounded && this.enrageDatabase.ContainsKey(name)) lostOwnDamage += this.enrageDatabase[name];
                        else if (m.name == CardDB.cardName.gurubashiberserker && m.Hp > 1) lostOwnDamage += 3;
                        else if (m.name == CardDB.cardName.frothingberserker) frothingberserkerOwn = true;
                        else if (m.name == CardDB.cardName.grimpatron) { lostOwnDamage += 3; grimpatronOwn = true; }
                    }
                }
                int hasExecute = 0;
                foreach (Handmanager.Handcard hc in p.owncards)
                {
                    if (hc.card.name == CardDB.cardName.execute) hasExecute++;
                }

                if (aoeDamageType == 1) //only enemy
                {
                    if (preventDamage > 5 || (p.enemyMinions.Count - survivedEnemyMinions) >= 4) return 0;
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
                    if (p.ownMinions.Count - lostOwnMinions - survivedEnemyMinions > 0)
                    {
                        if (preventDamage >= lostOwnDamage) return 0;
                        return (lostOwnDamage - preventDamage) * 2;
                    }
                    else
                    {
                        if (preventDamage >= lostOwnDamage * 2 + 1) return 0;
                        int MinionBalance = lostOwnMinions - (p.enemyMinions.Count - survivedEnemyMinions);
                        if (MinionBalance > 0 && preventDamage <= lostOwnDamage) return 80;
                        return 30;
                    }
                }
            }
            //END aoe damage **********************************************************************************

            if (target == null) return 0;

            if (target.own && target.isHero)
            {
                if (DamageTargetDatabase.ContainsKey(name) || DamageTargetSpecialDatabase.ContainsKey(name) || (p.anzOwnAuchenaiSoulpriest >= 1 && HealTargetDatabase.ContainsKey(name)))
                {
                    pen = 500;
                }
            }

            if (!lethal && !target.own && target.isHero)
            {
                if (name == CardDB.cardName.baneofdoom)
                {
                    pen = 500;
                }
            }

            if (target.own && !target.isHero)
            {
                if (DamageTargetDatabase.ContainsKey(name) || (p.anzOwnAuchenaiSoulpriest >= 1 && HealTargetDatabase.ContainsKey(name)))
                {
                    // no pen if own is enrage
                    Minion m = target;

                    //standard ones :D (mostly carddraw
                    if (enrageDatabase.ContainsKey(m.name) && !m.wounded && m.Ready)
                    {
                        return pen;
                    }

                    // no pen if we have battlerage for example
                    int dmg = this.DamageTargetDatabase.ContainsKey(name) ? this.DamageTargetDatabase[name] : this.HealTargetDatabase[name];

                    if (m.name == CardDB.cardName.madscientist && p.ownHeroStartClass == TAG_CLASS.HUNTER) return 500;
                    if (m.handcard.card.deathrattle) return 10;
                    if (m.Hp > dmg)
                    {
                        if (m.name == CardDB.cardName.acolyteofpain && p.owncards.Count <= 3) return 0;
                        if (m.name == CardDB.cardName.dragonegg && p.ownMinions.Count <= 6) return 5;
                        if (m.name == CardDB.cardName.impgangboss && p.ownMinions.Count <= 6) return 0;
                        if (m.name == CardDB.cardName.grimpatron && p.ownMinions.Count <= 6) return 0;
                        foreach (Handmanager.Handcard hc in p.owncards)
                        {
                            if (hc.card.name == CardDB.cardName.battlerage) return pen;
                            if (hc.card.name == CardDB.cardName.rampage) return pen;
                        }
                    }


                    pen = 500;
                }

                //special cards
                if (DamageTargetSpecialDatabase.ContainsKey(name))
                {
                    int dmg = DamageTargetSpecialDatabase[name];
                    Minion m = target;
                    if ((name == CardDB.cardName.crueltaskmaster || name == CardDB.cardName.innerrage) && m.Hp >= 2) return 0;
                    if ((name == CardDB.cardName.demonfire || name == CardDB.cardName.demonheart) && (TAG_RACE)m.handcard.card.race == TAG_RACE.DEMON) return 0;
                    if (name == CardDB.cardName.earthshock && m.Hp >= 2)
                    {
                        if ((!m.silenced && (m.name == CardDB.cardName.ancientwatcher || m.name == CardDB.cardName.ragnarosthefirelord)) || m.Angr < m.handcard.card.Attack || m.maxHp < m.handcard.card.Health || (m.frozen && !m.playedThisTurn && m.numAttacksThisTurn == 0))
                            return 0;
                        if (priorityDatabase.ContainsKey(m.name) && !m.silenced)
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
                        if (enrageDatabase.ContainsKey(m.name) && !m.wounded && m.Ready) // no pen if own is enrage
                        {
                            return pen;
                        }

                        foreach (Handmanager.Handcard hc in p.owncards) // no pen if we have battlerage for example
                        {
                            if (hc.card.name == CardDB.cardName.battlerage) return pen;
                            if (hc.card.name == CardDB.cardName.rampage) return pen;
                        }
                    }

                    pen = 500;
                }
            }
            if (!target.own && !target.isHero)
            {
                int realDamage = 0;
                if (DamageTargetSpecialDatabase.ContainsKey(name)) realDamage = (card.type == CardDB.cardtype.SPELL) ?  p.getSpellDamageDamage(this.DamageTargetSpecialDatabase[name]) : this.DamageTargetSpecialDatabase[name];
                else if (DamageTargetDatabase.ContainsKey(name)) realDamage = (card.type == CardDB.cardtype.SPELL) ? p.getSpellDamageDamage(this.DamageTargetDatabase[name]) : this.DamageTargetDatabase[name];
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
            ///Todo healpenality for aoe heal
            ///todo auchenai soulpriest
            if (p.anzOwnAuchenaiSoulpriest >= 1) return 0;
            if (name == CardDB.cardName.ancientoflore && choice != 2) return 0;
            int pen = 0;
            int heal = 0;
            /*if (HealHeroDatabase.ContainsKey(name))
            {
                heal = HealHeroDatabase[name];
                if (target == 200) pen = 500; // dont heal enemy
                if ((target == 100 || target == -1) && p.ownHeroHp + heal > 30) pen = p.ownHeroHp + heal - 30;
            }*/

            if (name == CardDB.cardName.treeoflife)
            {
                int mheal = 0;
                int wounded = 0;
                //int eheal = 0;
                if (p.ownHero.wounded) wounded++;
                foreach (Minion mi in p.ownMinions)
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
                foreach (Minion mi in p.ownMinions)
                {
                    mheal += Math.Min((mi.maxHp - mi.Hp), 4);
                    if (mi.wounded) wounded++;
                }
                //Console.WriteLine(mheal + " circle");
                if (mheal == 0) return 500;
                if (mheal <= 7 && wounded <= 2) return 20;
            }

            if (HealTargetDatabase.ContainsKey(name))
            {
                if (target == null) return 10;
                //Helpfunctions.Instance.ErrorLog("pencheck for " + name + " " + target.entitiyID + " " + target.isHero  + " " + target.own);
                heal = HealTargetDatabase[name];
                if (target.isHero && !target.own) return 510; // dont heal enemy
                //Helpfunctions.Instance.ErrorLog("pencheck for " + name + " " + target.entitiyID + " " + target.isHero + " " + target.own);
                if ((target.isHero && target.own) && p.ownHero.Hp == 30) return 150;
                if ((target.isHero && target.own) && p.ownHero.Hp + heal - 1 > 30) pen = p.ownHero.Hp + heal - 30;
                Minion m = new Minion();

                if (!target.isHero && target.own)
                {
                    m = target;
                    int wasted = 0;
                    if (m.Hp == m.maxHp) return 500;
                    if (m.Hp + heal - 1 > m.maxHp) wasted = m.Hp + heal - m.maxHp;
                    pen = wasted;

                    if (m.taunt && wasted <= 2 && m.Hp < m.maxHp) pen -= 5; // if we heal a taunt, its good :D

                    if (m.Hp + heal <= m.maxHp) pen = -1;
                }

                if (!target.isHero && !target.own)
                {
                    m = target;
                    if (m.Hp == m.maxHp) return 500;
                    // no penality if we heal enrage enemy
                    if (enrageDatabase.ContainsKey(m.name))
                    {
                        return pen;
                    }
                    // no penality if we have heal-trigger :D
                    int i = 0;
                    foreach (Minion mnn in p.ownMinions)
                    {
                        if (mnn.name == CardDB.cardName.northshirecleric) i++;
                        if (mnn.name == CardDB.cardName.lightwarden) i++;
                    }
                    foreach (Minion mnn in p.enemyMinions)
                    {
                        if (mnn.name == CardDB.cardName.northshirecleric) i--;
                        if (mnn.name == CardDB.cardName.lightwarden) i--;
                    }
                    if (i >= 1) return pen;

                    // no pen if we have slam

                    foreach (Handmanager.Handcard hc in p.owncards)
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
            // penality if carddraw is late or you have enough cards
            int pen = 0;
            if (!cardDrawBattleCryDatabase.ContainsKey(name)) return 0;
            if (name == CardDB.cardName.ancientoflore && choice != 1) return 0;
            if (name == CardDB.cardName.wrath && choice != 2) return 0;
            if (name == CardDB.cardName.nourish && choice != 2) return 0;
            if (name == CardDB.cardName.grovetender && choice != 2) return 0;
            if (name == CardDB.cardName.tracking) return 0;            

            int carddraw = cardDrawBattleCryDatabase[name];
            if (carddraw == 0)
            {
                switch(name)
                {
                    case CardDB.cardName.harrisonjones:
                        carddraw = p.enemyWeaponDurability;
                        if (carddraw == 0 && (p.enemyHeroStartClass != TAG_CLASS.DRUID && p.enemyHeroStartClass != TAG_CLASS.MAGE && p.enemyHeroStartClass != TAG_CLASS.WARLOCK && p.enemyHeroStartClass != TAG_CLASS.PRIEST)) return 5;
                        break;

                    case CardDB.cardName.divinefavor:
                        carddraw = p.enemyAnzCards - (p.owncards.Count);
                        if (carddraw <= 0) return 500;
                        break;

                    case CardDB.cardName.battlerage:
                        foreach (Minion mnn in p.ownMinions)
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
                        carddraw = (p.owncards.Count > 0) ? 0 : 1;
                        if (carddraw == 0) return 4;
                        break;

                    case CardDB.cardName.thoughtsteal:
                        carddraw = Math.Min(2, p.enemyDeckSize);
                        if (carddraw == 2) break;
                        if (carddraw == 1) pen +=4;
                        else
                        {
                            foreach (Minion mnn in p.ownMinions)
                            {
                                if (spellDependentDatabase.ContainsKey(mnn.name)) return 0;
                            }
                            return 500;
                        }
                        break;
                    
                    case CardDB.cardName.mindvision:
                        carddraw = Math.Min(1, p.enemyAnzCards);
                        if (carddraw != 1)
                        {
                            int scales = 0;
                            foreach (Minion mnn in p.ownMinions)
                            {
                                if (this.spellDependentDatabase.ContainsKey(mnn.name))
                                    if(mnn.name == CardDB.cardName.lorewalkercho) pen += 20; //if(spellDependentDatabase[mnn.name] == 0);
                                    else scales--;
                            }
                            if (scales == 0) return 500;
                            foreach (Minion mnn in p.enemyMinions)
                            {
                                if (this.spellDependentDatabase.ContainsKey(mnn.name) && this.spellDependentDatabase[name] <= 0) scales++;
                            }
                            return (12 + scales * 4 + pen);
                        }
                        break;
                        
                    case CardDB.cardName.echoofmedivh:
                        if (p.ownMinions.Count == 0) return 500;
                        return 0;
                        break;
                        
                    case CardDB.cardName.tinkertowntechnician:
                        foreach (Minion mnn in p.ownMinions)
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
                foreach (Handmanager.Handcard hc in p.owncards)
                {
                    if (hc.manacost <= minmana)
                    {
                        minmana = hc.manacost;
                    }
                    //if (hc.getManaCost(p) == p.ownMaxMana)
                    int manac = hc.getManaCost(p);
                    if (manac > p.ownMaxMana - 2 && manac <= p.ownMaxMana)
                    {
                        cardOnLimit = true;
                    }

                }

                if (Ai.Instance.botBase is BehaviorRush && p.ownMaxMana <= 3 && cardOnLimit) return 6; //RR penalization for drawing the 3 first turns if we have a card in hand that we won't be able to play in Rush


                if (p.owncards.Count + p.cardsPlayedThisTurn <= 5 && minmana > p.ownMaxMana) return 0;
                if (p.owncards.Count + p.cardsPlayedThisTurn > 5) return 25;
                return Math.Max(-carddraw + 2 * p.optionsPlayedThisTurn + p.ownMaxMana - p.mana, 0);
            }

            if (p.owncards.Count + carddraw > 10) return 15 * (p.owncards.Count + carddraw - 10);
            if (p.owncards.Count + p.cardsPlayedThisTurn > 5) return (5 * carddraw) + 1;

            pen += -carddraw + 2 * p.optionsPlayedThisTurn + p.ownMaxMana - p.mana;
            return pen;
        }

        private int getCardDrawofEffectMinions(CardDB.Card card, Playfield p)
        {
            int pen = 0;
            int carddraw = 0;
            if (card.type == CardDB.cardtype.SPELL)
            {
                foreach (Minion mnn in p.ownMinions)
                {
                    if (mnn.name == CardDB.cardName.gadgetzanauctioneer) carddraw++;
                }
            }

            if (card.type == CardDB.cardtype.MOB && (TAG_RACE)card.race == TAG_RACE.PET)
            {
                foreach (Minion mnn in p.ownMinions)
                {
                    if (mnn.name == CardDB.cardName.starvingbuzzard) carddraw++;
                }
            }

            if (carddraw == 0) return 0;

            if (p.owncards.Count >= 5) return 0;
            pen = -carddraw + p.ownMaxMana - p.mana + p.optionsPlayedThisTurn;

            return pen;
        }

        private int getRandomPenaltiy(CardDB.Card card, Playfield p, Minion target)
        {
            if (p.turnCounter >= 1)
            {
                return 0;
            }

            if (!this.randomEffects.ContainsKey(card.name) && !this.cardDrawBattleCryDatabase.ContainsKey(card.name))
            {
                return 0;
            }

            if (card.name == CardDB.cardName.brawl)
            {
                return 0;
            }

            if ((card.name == CardDB.cardName.cleave || card.name == CardDB.cardName.multishot)
                && p.enemyMinions.Count == 2)
            {
                return 0;
            }

            if ((card.name == CardDB.cardName.deadlyshot) && p.enemyMinions.Count == 1)
            {
                return 0;
            }

            if ((card.name == CardDB.cardName.arcanemissiles || card.name == CardDB.cardName.avengingwrath)
                && p.enemyMinions.Count == 0)
            {
                return 0;
            }

            int cards = 0;
            cards = this.randomEffects.ContainsKey(card.name) ? this.randomEffects[card.name] : this.cardDrawBattleCryDatabase[card.name];

            bool first = true;
            bool hasgadget = false;
            bool hasstarving = false;
            bool hasknife = false;
            bool hasFlamewaker = false;
            foreach (Minion mnn in p.ownMinions)
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

            foreach (Action a in p.playactions)
            {
                if (a.actionType == actionEnum.attackWithHero)
                {
                    first = false;
                    continue;
                }

                if (a.actionType == actionEnum.useHeroPower
                    && (p.ownHeroName != HeroEnum.shaman && p.ownHeroName != HeroEnum.warlock))
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

                    if (this.cardDrawBattleCryDatabase.ContainsKey(a.card.card.name))
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
                return cards + p.playactions.Count + 1;
            }

            return 0;
        }

        private int getCardDiscardPenality(CardDB.cardName name, Playfield p)
        {
            if (p.owncards.Count <= 1) return 0;
            if (p.ownMaxMana <= 3) return 0;
            int pen = 0;
            if (this.cardDiscardDatabase.ContainsKey(name))
            {
                int newmana = p.mana - cardDiscardDatabase[name];
                bool canplayanothercard = false;
                foreach (Handmanager.Handcard hc in p.owncards)
                {
                    if (this.cardDiscardDatabase.ContainsKey(hc.card.name)) continue;
                    if (hc.card.getManaCost(p, hc.manacost) <= newmana)
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
            if (!this.destroyOwnDatabase.ContainsKey(name)) return 0;
            int pen = 0;
            if ((name == CardDB.cardName.brawl || name == CardDB.cardName.deathwing || name == CardDB.cardName.twistingnether) && p.mobsplayedThisTurn >= 1) return 500;

            if (name == CardDB.cardName.brawl || name == CardDB.cardName.twistingnether)
            {
                if (name == CardDB.cardName.brawl && p.ownMinions.Count + p.enemyMinions.Count <= 1) return 500;
                int highminion = 0;
                int veryhighminion = 0;
                foreach (Minion m in p.enemyMinions)
                {
                    if (m.Angr >= 5 || m.Hp >= 5) highminion++;
                    if (m.Angr >= 8 || m.Hp >= 8) veryhighminion++;
                }

                if (highminion >= 2 || veryhighminion >= 1)
                {
                    return 0;
                }

                if (p.enemyMinions.Count <= 2 || p.enemyMinions.Count + 2 <= p.ownMinions.Count || p.ownMinions.Count >= 3)
                {
                    return 30;
                }
            }
            if (target == null) return 0;
            if (target.own && !target.isHero)
            {
                // dont destroy owns ;_; (except mins with deathrattle effects)

                Minion m = target;
                if (m.handcard.card.deathrattle) return 10;
                if (lethal && (name == CardDB.cardName.sacrificialpact || name == CardDB.cardName.siphonsoul))
                {
                    int beasts = 0;
                    foreach (Minion mm in p.ownMinions)
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
            if (!this.destroyDatabase.ContainsKey(name) || lethal) return 0;
            int pen = 0;
            if (target == null) return 0;
            if (target.own && !target.isHero)
            {
                Minion m = target;
                if (!m.handcard.card.deathrattle)
                {
                    pen = 500;
                }
            }
            if (!target.own && !target.isHero)
            {
                // dont destroy owns ;_; (except mins with deathrattle effects)

                Minion m = target;

                if (m.allreadyAttacked)
                {
                    return 50;
                }

                if (name == CardDB.cardName.shadowwordpain)
                {
                    if (this.specialMinions.ContainsKey(m.name) || m.Angr == 3 || m.Hp >= 4)
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

                if (name == CardDB.cardName.mindcontrol && (m.name == CardDB.cardName.direwolfalpha || m.name == CardDB.cardName.raidleader || m.name == CardDB.cardName.flametonguetotem) && p.enemyMinions.Count == 1)
                {
                    pen = 50;
                }

            }

            return pen;
        }

        private int getSpecialCardComboPenalitys(CardDB.Card card, Minion target, Playfield p, bool lethal, int choice)
        {
            CardDB.cardName name = card.name;

            if (lethal && card.type == CardDB.cardtype.MOB)
            {
                if (this.lethalHelpers.ContainsKey(name))
                {
                    return 0;
                }

                if (this.buffingMinionsDatabase.ContainsKey(name))
                {
                    if (name == CardDB.cardName.timberwolf || name == CardDB.cardName.houndmaster)
                    {
                        int beasts = 0;
                        foreach (Minion mm in p.ownMinions)
                        {
                            if ((TAG_RACE)mm.handcard.card.race == TAG_RACE.PET) beasts++;
                        }
                        if (beasts == 0) return 500;
                    }
                    if (name == CardDB.cardName.southseacaptain)
                    {
                        int beasts = 0;
                        foreach (Minion mm in p.ownMinions)
                        {
                            if ((TAG_RACE)mm.handcard.card.race == TAG_RACE.PIRATE) beasts++;
                        }
                        if (beasts == 0) return 500;
                    }
                    if (name == CardDB.cardName.murlocwarleader || name == CardDB.cardName.grimscaleoracle || name == CardDB.cardName.coldlightseer)
                    {
                        int beasts = 0;
                        foreach (Minion mm in p.ownMinions)
                        {
                            if ((TAG_RACE)mm.handcard.card.race == TAG_RACE.MURLOC) beasts++;
                        }
                        if (beasts == 0) return 500;
                    }
                }
                else
                {
                    if ((name == CardDB.cardName.rendblackhand && target != null) && !target.own)
                    {
                        if ((target.taunt && target.handcard.card.rarity == 5) || target.handcard.card.name == CardDB.cardName.malganis)
                        {
                            foreach (Handmanager.Handcard hc in p.owncards)
                            {
                                if ((TAG_RACE)hc.card.race == TAG_RACE.DRAGON) return 0;
                            }
                        }
                        return 500;
                    }

                    if (name == CardDB.cardName.theblackknight)
                    {
                        int beasts = 0;
                        foreach (Minion mm in p.enemyMinions)
                        {
                            if (mm.taunt) return 0;
                        }
                        return 500;
                    }
                    else
                    {
                        if ((this.HealTargetDatabase.ContainsKey(name) || this.HealHeroDatabase.ContainsKey(name) || this.HealAllDatabase.ContainsKey(name)))
                        {
                            int beasts = 0;
                            foreach (Minion mm in p.ownMinions)
                            {
                                if (mm.Ready && mm.handcard.card.name == CardDB.cardName.lightwarden) beasts++;
                            }
                            if (beasts == 0) return 500;
                        }
                        else
                        {
                            if (!(name == CardDB.cardName.nightblade || card.Charge || this.silenceDatabase.ContainsKey(name) || ((TAG_RACE)card.race == TAG_RACE.PET && p.ownMinions.Find(x => x.name == CardDB.cardName.tundrarhino) != null) || (p.ownMinions.Find(x => x.name == CardDB.cardName.warsongcommander) != null && card.Attack <= 3) || p.owncards.Find(x => x.card.name == CardDB.cardName.charge) != null))
                            {
                                return 500;
                            }
                        }
                    }
                }
            }

            //lethal end########################################################

            if (card.name == CardDB.cardName.unstableportal && p.owncards.Count <= 9) return -15;

            if (card.name == CardDB.cardName.daggermastery)
            {
                if (p.ownWeaponAttack >= 2 || p.ownWeaponDurability >= 2) return 5;
            }

            if (card.name == CardDB.cardName.upgrade)
            {
                if (p.ownWeaponDurability == 0)
                {
                    return 16;
                }
            }

            if (card.name == CardDB.cardName.baronrivendare)
            {
                foreach (Minion mnn in p.ownMinions)
                {
                    if (mnn.name == CardDB.cardName.deathlord || mnn.name == CardDB.cardName.zombiechow || mnn.name == CardDB.cardName.dancingswords) return 30;
                }
            }

            //rule for coin on early game
            if (p.ownMaxMana < 3 && card.name == CardDB.cardName.thecoin)
            {
                foreach (Handmanager.Handcard hc in p.owncards)
                {
                    if (hc.manacost <= p.ownMaxMana && hc.card.type == CardDB.cardtype.MOB) return 5;
                }

            }

            if (name == CardDB.cardName.flare && p.enemySecretCount >= 1 && p.playactions.Count == 0)
            {
                return -10;
            }

            //some effects, which are bad :D
            int pen = 0;
            if (name == CardDB.cardName.houndmaster)
            {
                if (target == null) return 50;
            }

            if ((card.name == CardDB.cardName.biggamehunter) && (target == null || target.own))
            {
                return 40;
            }
            if (name == CardDB.cardName.aldorpeacekeeper && target == null)
            {
                pen = 30;
            }

            if (name == CardDB.cardName.emergencycoolant && target != null && target.own)//dont freeze own minions
            {
                pen = 500;
            }

            if (name == CardDB.cardName.shatteredsuncleric && target == null) { pen = 10; }
            if (name == CardDB.cardName.argentprotector)
            {
                if (target == null) { pen = 20; }
                else
                {
                    if (!target.own) { return 500; }
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
                if (target.own)
                {
                    return 100;
                }
                if ((target.taunt && target.handcard.card.rarity == 5) || target.handcard.card.name == CardDB.cardName.malganis)
                {
                    foreach (Handmanager.Handcard hc in p.owncards)
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

                foreach (Minion mnn in p.enemyMinions)
                {
                    if (mnn.taunt && (target.Angr >= 3 || target.Hp >= 3)) return 0;
                }
                return 20;
            }

            if (name == CardDB.cardName.madbomber || name == CardDB.cardName.madderbomber)
            {
                pen = 0;
                foreach (Minion mnn in p.ownMinions)
                {
                    if (mnn.Ready & mnn.Hp < 3) pen += 5;
                }
                return pen;
            }

            //------------------------------------------------------------------------------------------------------
            Minion m = target;

            if (card.name == CardDB.cardName.reincarnate)
            {
                if (m.own)
                {
                    if (m.handcard.card.deathrattle || m.ancestralspirit >= 1 || m.souloftheforest >= 1 || m.enemyBlessingOfWisdom >= 1) return 0;
                    if (m.handcard.card.Charge && ((m.numAttacksThisTurn == 1 && !m.windfury) || (m.numAttacksThisTurn == 2 && m.windfury))) return 0;
                    if (m.wounded || m.Angr < m.handcard.card.Attack || (m.silenced && PenalityManager.instance.specialMinions.ContainsKey(m.name))) return 0;


                    bool hasOnMinionDiesMinion = false;
                    foreach (Minion mnn in p.ownMinions)
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

            if (p.ownHeroName == HeroEnum.shaman && p.ownAbilityReady == false)
            {
                return 20;
            }

            if (card.name == CardDB.cardName.flametonguetotem && p.ownMinions.Count == 0)
            {
                return 100;
            }

            if (card.name == CardDB.cardName.stampedingkodo)
            {
                bool found = false;
                foreach (Minion mi in p.enemyMinions)
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

            if ((name == CardDB.cardName.wildgrowth || name == CardDB.cardName.nourish) && p.ownMaxMana == 9 && !(p.ownHeroName == HeroEnum.thief && p.cardsPlayedThisTurn == 0))
            {
                return 500;
            }

            if (name == CardDB.cardName.ancestralspirit)
            {
                if (!target.own && !target.isHero)
                {
                    if (m.name == CardDB.cardName.deathlord || m.name == CardDB.cardName.zombiechow || m.name == CardDB.cardName.dancingswords) return 0;
                    return 500;
                }
                if (target.own && !target.isHero)
                {
                    if (this.specialMinions.ContainsKey(m.name)) return -5;
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
                if (p.enemyMinions.Count == 0)
                {
                    return 10;
                }
            }

            if (name == CardDB.cardName.betrayal && !target.own && !target.isHero)
            {
                if (m.Angr == 0) return 30;
                if (p.enemyMinions.Count == 1) return 30;
            }




            if (name == CardDB.cardName.bite)
            {
                if ((p.ownHero.numAttacksThisTurn == 0 || (p.ownHero.windfury && p.ownHero.numAttacksThisTurn == 1)) && !p.ownHero.frozen)
                {

                }
                else
                {
                    return 20;
                }
            }

            if (name == CardDB.cardName.deadlypoison)
            {
                return p.ownWeaponDurability * 2;
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
                if (!target.own && !target.isHero)
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
                if (target.own && !target.isHero && !m.Ready)
                {
                    return 500;
                }
            }

            if (name == CardDB.cardName.frothingberserker)
            {
                if (p.cardsPlayedThisTurn >= 1) pen = 5;
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
                    foreach (Minion mm in p.ownMinions)
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
                    if (!target.own && !target.isHero)
                    {
                        if (!m.taunt)
                        {
                            return 500;
                        }
                        else
                        {
                            // combo for killing with innerfire and biggamehunter
                            if (p.owncards.Find(x => x.card.name == CardDB.cardName.biggamehunter) != null && p.owncards.Find(x => x.card.name == CardDB.cardName.innerfire) != null && (m.Hp >= 4 || (p.owncards.Find(x => x.card.name == CardDB.cardName.divinespirit) != null && m.Hp >= 2)))
                            {
                                return 0;
                            }
                            return 500;
                        }
                    }
                }
                else
                {
                    if (!target.own && !target.isHero)
                    {

                        // combo for killing with innerfire and biggamehunter
                        if (p.owncards.Find(x => x.card.name == CardDB.cardName.biggamehunter) != null && p.owncards.Find(x => x.card.name == CardDB.cardName.innerfire) != null && m.Hp >= 4)
                        {
                            return 0;
                        }
                        return 500;
                    }

                }

                if (target.own && !target.isHero)
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
                if (this.UsefulNeedKeepDatabase.ContainsKey(name))
                {
                    penTmP = UsefulNeedKeepDatabase[name] - target.maxHp - p.ownMaxMana + target.handcard.card.cost;
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
                if (p.ownMaxMana < 6) return 50;
                if (p.ownMinions.Count == 7) return 500;
                if (p.ownMaxMana > 8) return 0;
                if (p.OwnLastDiedMinion == CardDB.cardIDEnum.None) return 6;
                return 0;
            }

            if (name == CardDB.cardName.lavashock)
            {
                if (p.ueberladung < 1) return 15;
                return (3 - 3 * p.ueberladung);
            }

            if (name == CardDB.cardName.enhanceomechano)
            {
                if (p.ownMinions.Count == 0 && p.ownMaxMana < 5) return 500;
                int penTmP = 2 * (p.mana - 4 - p.mobsplayedThisTurn); //for accurate calculation we need hc
                if (p.mobsplayedThisTurn < 1) penTmP += 30;
                return penTmP;
            }

            if (name == CardDB.cardName.knifejuggler && p.mobsplayedThisTurn >= 1)
            {
                return 20;
            }

            if (name == CardDB.cardName.flamewaker)
            {
                foreach (Action a in p.playactions)
                {
                    if (a.actionType == actionEnum.playcard && a.card.card.type == CardDB.cardtype.SPELL) return 30;
                }
            }

            if ((name == CardDB.cardName.polymorph || name == CardDB.cardName.hex))
            {
                if (target.own && !target.isHero)
                {
                    return 500;
                }

                if (!target.own && !target.isHero)
                {
                    if (target.allreadyAttacked) return 30;
                    Minion frog = target;
                    if (this.priorityTargets.ContainsKey(frog.name)) return 0;
                    if (frog.Angr >= 4 && frog.Hp >= 4) return 0;
                    return 30;
                }
            }


            if ((name == CardDB.cardName.defenderofargus || name == CardDB.cardName.sunfuryprotector) && p.ownMinions.Count == 1)
            {
                return 40;
            }
            if ((name == CardDB.cardName.defenderofargus || name == CardDB.cardName.sunfuryprotector) && p.ownMinions.Count == 0)
            {
                return 50;
            }

            if (name == CardDB.cardName.unleashthehounds)
            {
                if (p.enemyMinions.Count <= 1)
                {
                    return 20;
                }
            }

            if (name == CardDB.cardName.equality) // aoe penality
            {
                if (p.enemyMinions.Count <= 2 || (p.ownMinions.Count - p.enemyMinions.Count >= 1))
                {
                    return 20;
                }
            }

            if (name == CardDB.cardName.bloodsailraider && p.ownWeaponDurability == 0)
            {
                //if you have bloodsailraider and no weapon equiped, but own a weapon:
                foreach (Handmanager.Handcard hc in p.owncards)
                {
                    if (hc.card.type == CardDB.cardtype.WEAPON) return 10;
                }
            }

            if (name == CardDB.cardName.acidicswampooze)
            {
                if (p.enemyWeaponAttack >= 1) return 0;
                if (p.enemyHeroName == HeroEnum.warrior || p.enemyHeroName == HeroEnum.thief || p.enemyHeroName == HeroEnum.pala ) return 10;
                if (p.enemyHeroName == HeroEnum.shaman || p.enemyHeroName == HeroEnum.hunter) return 6;
            }

            if (name == CardDB.cardName.innerfire)
            {
                if (m.name == CardDB.cardName.lightspawn) pen = 500;
            }

            if (name == CardDB.cardName.huntersmark)
            {
                if (target.own && !target.isHero) pen = 500; // dont use on own minions
                if (!target.own && !target.isHero && (target.Hp <= 4) && target.Angr <= 4) // only use on strong minions
                {
                    pen = 20;
                }
            }


            if ((name == CardDB.cardName.aldorpeacekeeper || name == CardDB.cardName.humility))
            {
                if (target != null)
                {
                    if (target.own) pen = 500; // dont use on own minions
                    if (!target.own && target.Angr <= 3) // only use on strong minions
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



            if (name == CardDB.cardName.defiasringleader && p.cardsPlayedThisTurn == 0)
            { pen = 10; }
            if (name == CardDB.cardName.bloodknight)
            {
                int shilds = 0;
                foreach (Minion min in p.ownMinions)
                {
                    if (min.divineshild)
                    {
                        shilds++;
                    }
                }
                foreach (Minion min in p.enemyMinions)
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
                foreach (Minion min in p.ownMinions)
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
                foreach (Minion min in p.ownMinions)
                {
                    if (min.Ready)
                    { ready++; }
                }
                if (ready == 0)
                {
                    pen = 5;
                }
            }


            if (returnHandDatabase.ContainsKey(name))
            {
                if (name == CardDB.cardName.vanish)
                {
                    //dont vanish if we have minons on board wich are ready
                    bool haveready = false;
                    foreach (Minion mins in p.ownMinions)
                    {
                        if (mins.Ready) haveready = true;
                    }
                    if (haveready) pen += 10;
                }

                if (target.own && !target.isHero)
                {
                    Minion mnn = target;
                    if (mnn.Ready) pen += 10;
                }
            }

            return pen;
        }

        private int playSecretPenality(CardDB.Card card, Playfield p)
        {
            //penality if we play secret and have playable kirintormage
            int pen = 0;
            if (card.Secret)
            {
                foreach (Handmanager.Handcard hc in p.owncards)
                {
                    if (hc.card.name == CardDB.cardName.kirintormage && p.mana >= hc.getManaCost(p))
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
            int pen = 0;
            if (p.enemySecretCount == 0)
            {
                return 0;
            }

            if (c.name == CardDB.cardName.flare)
            {
                return 0;
            }

            int attackedbefore = 0;

            foreach (Minion mnn in p.ownMinions)
            {
                if (mnn.numAttacksThisTurn >= 1)
                {
                    attackedbefore++;
                }
            }

            if (c.name == CardDB.cardName.acidicswampooze
                && (p.enemyHeroStartClass == TAG_CLASS.WARRIOR || p.enemyHeroStartClass == TAG_CLASS.ROGUE || p.enemyHeroStartClass == TAG_CLASS.PALADIN))
            {
                if (p.enemyHeroStartClass == TAG_CLASS.ROGUE && p.enemyWeaponAttack <= 2)
                {
                    pen += 100;
                }
                else
                {
                    if (p.enemyWeaponAttack <= 1)
                    {
                        pen += 100;
                    }
                }
            }

            if (p.enemyHeroStartClass == TAG_CLASS.HUNTER)
            {
                if (c.type == CardDB.cardtype.MOB
                    && (attackedbefore == 0 || c.Health <= 4
                        || (p.enemyHero.Hp >= p.enemyHeroHpStarted && attackedbefore >= 1)))
                {
                    pen += 10;
                }
            }

            if (p.enemyHeroStartClass == TAG_CLASS.MAGE)
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
                    if ((!this.isOwnLowestInHand(m, p) && p.mobsplayedThisTurn == 0)
                        || (p.mobsplayedThisTurn == 0 && attackedbefore >= 1))
                    {
                        pen += 10;
                    }
                }

                if (c.type == CardDB.cardtype.SPELL && p.cardsPlayedThisTurn == p.mobsplayedThisTurn)
                {
                    pen += 10;
                }
            }

            if (p.enemyHeroStartClass == TAG_CLASS.PALADIN)
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
                    if ((!this.isOwnLowestInHand(m, p) && p.mobsplayedThisTurn == 0) || attackedbefore == 0)
                    {
                        pen += 10;
                    }
                }
            }

            return pen;
        }

        private int getAttackSecretPenality(Minion m, Playfield p, Minion target)
        {
            if (p.enemySecretCount == 0)
            {
                return 0;
            }

            int pen = 0;

            int attackedbefore = 0;

            foreach (Minion mnn in p.ownMinions)
            {
                if (mnn.numAttacksThisTurn >= 1) attackedbefore++;
            }

            if (p.enemyHeroStartClass == TAG_CLASS.HUNTER)
            {
                bool islow = isOwnLowest(m, p);
                if (attackedbefore == 0 && islow) pen -= 20;
                if (attackedbefore == 0 && !islow) pen += 10;

                if (target.isHero && !target.own && p.enemyMinions.Count >= 1)
                {
                    //penality if we doestn attacked before
                    if (hasMinionsWithLowHeal(p)) pen += 10; //penality if we doestn attacked minions before
                }
            }

            if (p.enemyHeroStartClass == TAG_CLASS.MAGE)
            {
                if (p.mobsplayedThisTurn == 0) pen += 10;

                bool islow = isOwnLowest(m, p);

                if (target.isHero && !target.own && !islow)
                {
                    pen += 10;
                }
                if (target.isHero && !target.own && islow && p.mobsplayedThisTurn >= 1)
                {
                    pen -= 20;
                }

            }

            if (p.enemyHeroStartClass == TAG_CLASS.PALADIN)
            {

                bool islow = isOwnLowest(m, p);

                if (!target.own && !target.isHero && attackedbefore == 0)
                {
                    if (!isEnemyLowest(target, p) || m.Hp <= 2) pen += 5;
                }

                if (target.isHero && !target.own && !islow)
                {
                    pen += 5;
                }

                if (target.isHero && !target.own && p.enemyMinions.Count >= 1 && attackedbefore == 0)
                {
                    pen += 5;
                }

            }


            return pen;
        }


        public CardDB.Card getChooseCard(CardDB.Card c, int choice)
        {
            if (choice == 1 && this.choose1database.ContainsKey(c.name))
            {
                c = CardDB.Instance.getCardDataFromID(this.choose1database[c.name]);
            }
            else if (choice == 2 && this.choose2database.ContainsKey(c.name))
            {
                c = CardDB.Instance.getCardDataFromID(this.choose2database[c.name]);
            }
            return c;
        }

        public int getValueOfUsefulNeedKeepPriority(CardDB.cardName name)
        {
            int ret = 0;
            if (this.UsefulNeedKeepDatabase.ContainsKey(name)) ret = UsefulNeedKeepDatabase[name];
            return ret;
        }


        private int getValueOfMinion(Minion m)
        {
            int ret = 0;
            ret += 2 * m.Angr + m.Hp;
            if (m.taunt) ret += 2;
            if (this.priorityDatabase.ContainsKey(m.name)) ret += 20 + priorityDatabase[m.name];
            return ret;
        }

        private bool isOwnLowest(Minion mnn, Playfield p)
        {
            bool ret = true;
            int val = getValueOfMinion(mnn);
            foreach (Minion m in p.ownMinions)
            {
                if (!m.Ready) continue;
                if (getValueOfMinion(m) < val) ret = false;
            }
            return ret;
        }

        private bool isOwnLowestInHand(Minion mnn, Playfield p)
        {
            bool ret = true;
            Minion m = new Minion();
            int val = getValueOfMinion(mnn);
            foreach (Handmanager.Handcard card in p.owncards)
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
            bool ret = true;
            List<Minion> litt = p.getAttackTargets(true, false);
            int val = getValueOfEnemyMinion(mnn);
            foreach (Minion m in p.enemyMinions)
            {
                if (litt.Find(x => x.entitiyID == m.entitiyID) == null) continue;
                if (getValueOfEnemyMinion(m) < val) ret = false;
            }
            return ret;
        }

        private bool hasMinionsWithLowHeal(Playfield p)
        {
            bool ret = false;
            foreach (Minion m in p.ownMinions)
            {
                if (m.Hp <= 2 && (m.Ready || this.priorityDatabase.ContainsKey(m.name))) ret = true;
            }
            return ret;
        }

        private void setupEnrageDatabase()
        {
            enrageDatabase.Add(CardDB.cardName.spitefulsmith, 2);
            enrageDatabase.Add(CardDB.cardName.angrychicken, 5);
            enrageDatabase.Add(CardDB.cardName.taurenwarrior, 3);
            enrageDatabase.Add(CardDB.cardName.amaniberserker, 3);
            enrageDatabase.Add(CardDB.cardName.ragingworgen, 2);
            enrageDatabase.Add(CardDB.cardName.grommashhellscream, 6);
            enrageDatabase.Add(CardDB.cardName.warbot, 1);
        }

        private void setupHealDatabase()
        {
            HealAllDatabase.Add(CardDB.cardName.holynova, 2);//to all own minions
            HealAllDatabase.Add(CardDB.cardName.circleofhealing, 4);//allminions
            HealAllDatabase.Add(CardDB.cardName.darkscalehealer, 2);//all friends
            HealAllDatabase.Add(CardDB.cardName.treeoflife, 1000);//all friends

            HealHeroDatabase.Add(CardDB.cardName.drainlife, 2);//tohero
            HealHeroDatabase.Add(CardDB.cardName.guardianofkings, 6);//tohero
            HealHeroDatabase.Add(CardDB.cardName.holyfire, 5);//tohero
            HealHeroDatabase.Add(CardDB.cardName.priestessofelune, 4);//tohero
            HealHeroDatabase.Add(CardDB.cardName.sacrificialpact, 5);//tohero
            HealHeroDatabase.Add(CardDB.cardName.siphonsoul, 3); //tohero
            HealHeroDatabase.Add(CardDB.cardName.sealoflight, 4); //tohero
            HealHeroDatabase.Add(CardDB.cardName.antiquehealbot, 8); //tohero

            HealTargetDatabase.Add(CardDB.cardName.lightofthenaaru, 3);
            HealTargetDatabase.Add(CardDB.cardName.ancestralhealing, 1000);
            HealTargetDatabase.Add(CardDB.cardName.ancientsecrets, 5);
            HealTargetDatabase.Add(CardDB.cardName.holylight, 6);
            HealTargetDatabase.Add(CardDB.cardName.earthenringfarseer, 3);
            HealTargetDatabase.Add(CardDB.cardName.healingtouch, 8);
            HealTargetDatabase.Add(CardDB.cardName.layonhands, 8);
            HealTargetDatabase.Add(CardDB.cardName.lesserheal, 2);
            HealTargetDatabase.Add(CardDB.cardName.voodoodoctor, 2);
            HealTargetDatabase.Add(CardDB.cardName.willofmukla, 8);
            HealTargetDatabase.Add(CardDB.cardName.ancientoflore, 5);
            //HealTargetDatabase.Add(CardDB.cardName.divinespirit, 2);
        }

        private void setupDamageDatabase()
        {
            DamageAllDatabase.Add(CardDB.cardName.abomination, 2);
            DamageAllDatabase.Add(CardDB.cardName.barongeddon, 2);
            DamageAllDatabase.Add(CardDB.cardName.damagereflector, 1);
            DamageAllDatabase.Add(CardDB.cardName.deathsbite, 1);
            DamageAllDatabase.Add(CardDB.cardName.dreadinfernal, 1);
            DamageAllDatabase.Add(CardDB.cardName.explosivesheep, 2);
            //DamageAllDatabase.Add(CardDB.cardName.flameleviathan, 2);
            DamageAllDatabase.Add(CardDB.cardName.hellfire, 3);
            DamageAllDatabase.Add(CardDB.cardName.lightbomb, 5);
            DamageAllDatabase.Add(CardDB.cardName.poisoncloud, 1);//todo 1 or 2
            DamageAllDatabase.Add(CardDB.cardName.scarletpurifier, 2);
            DamageAllDatabase.Add(CardDB.cardName.unstableghoul, 1);
            DamageAllDatabase.Add(CardDB.cardName.whirlwind, 1);
            DamageAllDatabase.Add(CardDB.cardName.wildpyromancer, 1);
            DamageAllDatabase.Add(CardDB.cardName.yseraawakens, 5);
            DamageAllDatabase.Add(CardDB.cardName.revenge, 1);
            DamageAllDatabase.Add(CardDB.cardName.demonwrath, 1);

            DamageAllEnemysDatabase.Add(CardDB.cardName.arcaneexplosion, 1);
            DamageAllEnemysDatabase.Add(CardDB.cardName.bladeflurry, 1);
            DamageAllEnemysDatabase.Add(CardDB.cardName.blizzard, 2);
            DamageAllEnemysDatabase.Add(CardDB.cardName.consecration, 2);
            DamageAllEnemysDatabase.Add(CardDB.cardName.fanofknives, 1);
            DamageAllEnemysDatabase.Add(CardDB.cardName.flamestrike, 4);
            DamageAllEnemysDatabase.Add(CardDB.cardName.holynova, 2);
            DamageAllEnemysDatabase.Add(CardDB.cardName.lightningstorm, 2);
            DamageAllEnemysDatabase.Add(CardDB.cardName.locustswarm, 3);
            DamageAllEnemysDatabase.Add(CardDB.cardName.shadowflame, 2);
            DamageAllEnemysDatabase.Add(CardDB.cardName.sporeburst, 1);
            DamageAllEnemysDatabase.Add(CardDB.cardName.starfall, 2);
            DamageAllEnemysDatabase.Add(CardDB.cardName.stomp, 2);
            DamageAllEnemysDatabase.Add(CardDB.cardName.swipe, 1);
            DamageAllEnemysDatabase.Add(CardDB.cardName.darkironskulker, 2);

            DamageHeroDatabase.Add(CardDB.cardName.headcrack, 2);
            DamageHeroDatabase.Add(CardDB.cardName.lepergnome, 2);
            DamageHeroDatabase.Add(CardDB.cardName.mindblast, 5);
            DamageHeroDatabase.Add(CardDB.cardName.nightblade, 3);
            DamageHeroDatabase.Add(CardDB.cardName.purecold, 8);
            DamageHeroDatabase.Add(CardDB.cardName.shadowbomber, 3);
            DamageHeroDatabase.Add(CardDB.cardName.sinisterstrike, 3);

            DamageRandomDatabase.Add(CardDB.cardName.arcanemissiles, 1);
            DamageRandomDatabase.Add(CardDB.cardName.avengingwrath, 1);
            DamageRandomDatabase.Add(CardDB.cardName.bomblobber, 4);
            DamageRandomDatabase.Add(CardDB.cardName.boombot, 1);
            DamageRandomDatabase.Add(CardDB.cardName.bouncingblade, 1);
            DamageRandomDatabase.Add(CardDB.cardName.cleave, 2);
            DamageRandomDatabase.Add(CardDB.cardName.demolisher, 2);
            DamageRandomDatabase.Add(CardDB.cardName.flamecannon, 4);
            DamageRandomDatabase.Add(CardDB.cardName.forkedlightning, 2);
            DamageRandomDatabase.Add(CardDB.cardName.goblinblastmage, 1);
            DamageRandomDatabase.Add(CardDB.cardName.knifejuggler, 1);
            DamageRandomDatabase.Add(CardDB.cardName.madbomber, 1);
            DamageRandomDatabase.Add(CardDB.cardName.madderbomber, 1);
            DamageRandomDatabase.Add(CardDB.cardName.multishot, 3);
            DamageRandomDatabase.Add(CardDB.cardName.ragnarosthefirelord, 8);
            DamageRandomDatabase.Add(CardDB.cardName.shadowboxer, 1);
            DamageRandomDatabase.Add(CardDB.cardName.shipscannon, 2);
            DamageRandomDatabase.Add(CardDB.cardName.flamewaker, 2);

            DamageTargetDatabase.Add(CardDB.cardName.arcaneshot, 2);
            DamageTargetDatabase.Add(CardDB.cardName.backstab, 2);
            DamageTargetDatabase.Add(CardDB.cardName.baneofdoom, 2);
            DamageTargetDatabase.Add(CardDB.cardName.barreltoss, 2);
            DamageTargetDatabase.Add(CardDB.cardName.betrayal, 2);
            DamageTargetDatabase.Add(CardDB.cardName.cobrashot, 3);
            DamageTargetDatabase.Add(CardDB.cardName.coneofcold, 1);
            DamageTargetDatabase.Add(CardDB.cardName.crackle, 3);
            DamageTargetDatabase.Add(CardDB.cardName.damage1, 1);
            DamageTargetDatabase.Add(CardDB.cardName.damage5, 5);
            DamageTargetDatabase.Add(CardDB.cardName.darkbomb, 3);
            DamageTargetDatabase.Add(CardDB.cardName.drainlife, 2);
            DamageTargetDatabase.Add(CardDB.cardName.elvenarcher, 1);
            DamageTargetDatabase.Add(CardDB.cardName.eviscerate, 2);
            DamageTargetDatabase.Add(CardDB.cardName.explosiveshot, 5);
            DamageTargetDatabase.Add(CardDB.cardName.felcannon, 2);
            DamageTargetDatabase.Add(CardDB.cardName.fireball, 6);
            DamageTargetDatabase.Add(CardDB.cardName.fireblast, 1);
            DamageTargetDatabase.Add(CardDB.cardName.fireelemental, 3);
            DamageTargetDatabase.Add(CardDB.cardName.frostbolt, 3);
            DamageTargetDatabase.Add(CardDB.cardName.frostshock, 1);
            DamageTargetDatabase.Add(CardDB.cardName.hoggersmash, 4);
            DamageTargetDatabase.Add(CardDB.cardName.holyfire, 5);
            DamageTargetDatabase.Add(CardDB.cardName.holysmite, 2);
            DamageTargetDatabase.Add(CardDB.cardName.icelance, 4);//only if iced
            DamageTargetDatabase.Add(CardDB.cardName.implosion, 2);
            DamageTargetDatabase.Add(CardDB.cardName.ironforgerifleman, 1);
            DamageTargetDatabase.Add(CardDB.cardName.keeperofthegrove, 2); // or silence
            DamageTargetDatabase.Add(CardDB.cardName.killcommand, 3);//or 5
            DamageTargetDatabase.Add(CardDB.cardName.lavaburst, 5);
            DamageTargetDatabase.Add(CardDB.cardName.lightningbolt, 3);
            DamageTargetDatabase.Add(CardDB.cardName.mindshatter, 3);
            DamageTargetDatabase.Add(CardDB.cardName.mindspike, 2);
            DamageTargetDatabase.Add(CardDB.cardName.moonfire, 1);
            DamageTargetDatabase.Add(CardDB.cardName.mortalcoil, 1);
            DamageTargetDatabase.Add(CardDB.cardName.mortalstrike, 4);
            DamageTargetDatabase.Add(CardDB.cardName.perditionsblade, 1);
            DamageTargetDatabase.Add(CardDB.cardName.pyroblast, 10);
            DamageTargetDatabase.Add(CardDB.cardName.shadowbolt, 4);
            DamageTargetDatabase.Add(CardDB.cardName.shadowform, 2);
            DamageTargetDatabase.Add(CardDB.cardName.shotgunblast, 1);
            DamageTargetDatabase.Add(CardDB.cardName.si7agent, 2);
            DamageTargetDatabase.Add(CardDB.cardName.starfall, 5);//2 to all enemy
            DamageTargetDatabase.Add(CardDB.cardName.starfire, 5);//draw a card
            DamageTargetDatabase.Add(CardDB.cardName.steadyshot, 2);//or 1 + card
            DamageTargetDatabase.Add(CardDB.cardName.stormpikecommando, 2);
            DamageTargetDatabase.Add(CardDB.cardName.swipe, 4);//1 to others
            DamageTargetDatabase.Add(CardDB.cardName.wrath, 1);//todo 3 or 1+card
            DamageTargetDatabase.Add(CardDB.cardName.dragonsbreath, 4);
            DamageTargetDatabase.Add(CardDB.cardName.lavashock, 2);
            DamageTargetDatabase.Add(CardDB.cardName.blackwingcorruptor, 3);//if dragon in hand

            DamageTargetSpecialDatabase.Add(CardDB.cardName.crueltaskmaster, 1); // gives 2 attack
            DamageTargetSpecialDatabase.Add(CardDB.cardName.deathbloom, 5);
            DamageTargetSpecialDatabase.Add(CardDB.cardName.demonfire, 2); // friendly demon get +2/+2
            DamageTargetSpecialDatabase.Add(CardDB.cardName.demonheart, 5);
            DamageTargetSpecialDatabase.Add(CardDB.cardName.earthshock, 1); //SILENCE /good for raggy etc or iced
            DamageTargetSpecialDatabase.Add(CardDB.cardName.hammerofwrath, 3); //draw a card
            DamageTargetSpecialDatabase.Add(CardDB.cardName.holywrath, 2);//draw a card
            DamageTargetSpecialDatabase.Add(CardDB.cardName.innerrage, 1); // gives 2 attack
            DamageTargetSpecialDatabase.Add(CardDB.cardName.roguesdoit, 4);//draw a card
            DamageTargetSpecialDatabase.Add(CardDB.cardName.savagery, 1);//dmg=herodamage
            DamageTargetSpecialDatabase.Add(CardDB.cardName.shieldslam, 1);//dmg=armor
            DamageTargetSpecialDatabase.Add(CardDB.cardName.shiv, 1);//draw a card
            DamageTargetSpecialDatabase.Add(CardDB.cardName.slam, 2);//draw card if it survives
            DamageTargetSpecialDatabase.Add(CardDB.cardName.soulfire, 4);//delete a card
            DamageTargetSpecialDatabase.Add(CardDB.cardName.quickshot, 3); //draw a card

        }

        private void setupsilenceDatabase()
        {
            this.silenceDatabase.Add(CardDB.cardName.dispel, 1);
            this.silenceDatabase.Add(CardDB.cardName.earthshock, 1);
            this.silenceDatabase.Add(CardDB.cardName.massdispel, 1);
            this.silenceDatabase.Add(CardDB.cardName.silence, 1);
            this.silenceDatabase.Add(CardDB.cardName.keeperofthegrove, 1);
            this.silenceDatabase.Add(CardDB.cardName.ironbeakowl, 1);
            this.silenceDatabase.Add(CardDB.cardName.spellbreaker, 1);

            this.silenceDatabase.Add(CardDB.cardName.wailingsoul, -2);//1=1 target, 2=all enemy, -2=all own

            //own need silence
            this.OwnNeedSilenceDatabase.Add(CardDB.cardName.ancientwatcher, 2);
            this.OwnNeedSilenceDatabase.Add(CardDB.cardName.zombiechow, 2);
            this.OwnNeedSilenceDatabase.Add(CardDB.cardName.animagolem, 1);
            this.OwnNeedSilenceDatabase.Add(CardDB.cardName.thebeast, 1);
            this.OwnNeedSilenceDatabase.Add(CardDB.cardName.deathcharger, 1);
            //this.OwnNeedSilenceDatabase.Add(CardDB.cardName.deathlord, 0);//if hp<3
            this.OwnNeedSilenceDatabase.Add(CardDB.cardName.dancingswords, 1);
            this.OwnNeedSilenceDatabase.Add(CardDB.cardName.spore, 3);
            //this.OwnNeedSilenceDatabase.Add(CardDB.cardName.barongeddon, 0); //2 damage to ALL other - if profitable
            this.OwnNeedSilenceDatabase.Add(CardDB.cardName.ragnarosthefirelord, 0);
            this.OwnNeedSilenceDatabase.Add(CardDB.cardName.mogortheogre, 1);
            this.OwnNeedSilenceDatabase.Add(CardDB.cardName.venturecomercenary, 1);
            this.OwnNeedSilenceDatabase.Add(CardDB.cardName.spectraltrainee, 1);
            this.OwnNeedSilenceDatabase.Add(CardDB.cardName.spectralwarrior, 1);
            this.OwnNeedSilenceDatabase.Add(CardDB.cardName.spectralrider, 1);
            this.OwnNeedSilenceDatabase.Add(CardDB.cardName.felreaver, 3);

        }


        private void setupPriorityList()
        {
            this.priorityDatabase.Add(CardDB.cardName.prophetvelen, 5);
            this.priorityDatabase.Add(CardDB.cardName.archmageantonidas, 5);
            this.priorityDatabase.Add(CardDB.cardName.flametonguetotem, 6);
            this.priorityDatabase.Add(CardDB.cardName.raidleader, 5);
            this.priorityDatabase.Add(CardDB.cardName.grimscaleoracle, 5);
            this.priorityDatabase.Add(CardDB.cardName.direwolfalpha, 6);
            this.priorityDatabase.Add(CardDB.cardName.murlocwarleader, 5);
            this.priorityDatabase.Add(CardDB.cardName.southseacaptain, 5);
            this.priorityDatabase.Add(CardDB.cardName.stormwindchampion, 5);
            this.priorityDatabase.Add(CardDB.cardName.timberwolf, 5);
            this.priorityDatabase.Add(CardDB.cardName.leokk, 5);
            this.priorityDatabase.Add(CardDB.cardName.northshirecleric, 5);
            this.priorityDatabase.Add(CardDB.cardName.sorcerersapprentice, 3);
            this.priorityDatabase.Add(CardDB.cardName.summoningportal, 5);
            this.priorityDatabase.Add(CardDB.cardName.pintsizedsummoner, 3);
            this.priorityDatabase.Add(CardDB.cardName.scavenginghyena, 5);
            this.priorityDatabase.Add(CardDB.cardName.manatidetotem, 5);
            this.priorityDatabase.Add(CardDB.cardName.mechwarper, 1);

            this.priorityDatabase.Add(CardDB.cardName.emperorthaurissan, 5);
            this.priorityDatabase.Add(CardDB.cardName.grimpatron, 5);
        }

        private void setupAttackBuff()
        {
            heroAttackBuffDatabase.Add(CardDB.cardName.bite, 4);
            heroAttackBuffDatabase.Add(CardDB.cardName.claw, 2);
            heroAttackBuffDatabase.Add(CardDB.cardName.heroicstrike, 2);

            this.attackBuffDatabase.Add(CardDB.cardName.abusivesergeant, 2);
            this.attackBuffDatabase.Add(CardDB.cardName.bananas, 1);
            this.attackBuffDatabase.Add(CardDB.cardName.bestialwrath, 2); // NEVER ON enemy MINION
            this.attackBuffDatabase.Add(CardDB.cardName.blessingofkings, 4);
            this.attackBuffDatabase.Add(CardDB.cardName.blessingofmight, 3);
            this.attackBuffDatabase.Add(CardDB.cardName.coldblood, 2);
            this.attackBuffDatabase.Add(CardDB.cardName.crueltaskmaster, 2);
            this.attackBuffDatabase.Add(CardDB.cardName.darkirondwarf, 2);
            this.attackBuffDatabase.Add(CardDB.cardName.innerrage, 2);
            this.attackBuffDatabase.Add(CardDB.cardName.markofnature, 4);//choice1 
            this.attackBuffDatabase.Add(CardDB.cardName.markofthewild, 2);
            this.attackBuffDatabase.Add(CardDB.cardName.nightmare, 5); //destroy minion on next turn
            this.attackBuffDatabase.Add(CardDB.cardName.rampage, 3);//only damaged minion 
            this.attackBuffDatabase.Add(CardDB.cardName.uproot, 5);
            this.attackBuffDatabase.Add(CardDB.cardName.velenschosen, 2);

            this.attackBuffDatabase.Add(CardDB.cardName.darkwispers, 5);//choice 2
            this.attackBuffDatabase.Add(CardDB.cardName.whirlingblades, 1);

        }

        private void setupHealthBuff()
        {

            //this.healthBuffDatabase.Add(CardDB.cardName.ancientofwar, 5);//choice2 is only buffing himself!
            this.healthBuffDatabase.Add(CardDB.cardName.bananas, 1);
            this.healthBuffDatabase.Add(CardDB.cardName.blessingofkings, 4);
            this.healthBuffDatabase.Add(CardDB.cardName.markofnature, 4);//choice2
            this.healthBuffDatabase.Add(CardDB.cardName.markofthewild, 2);
            this.healthBuffDatabase.Add(CardDB.cardName.nightmare, 5);
            this.healthBuffDatabase.Add(CardDB.cardName.powerwordshield, 2);
            this.healthBuffDatabase.Add(CardDB.cardName.rampage, 3);
            this.healthBuffDatabase.Add(CardDB.cardName.velenschosen, 4);
            this.healthBuffDatabase.Add(CardDB.cardName.darkwispers, 5);//choice2
            this.healthBuffDatabase.Add(CardDB.cardName.upgradedrepairbot, 4);
            this.healthBuffDatabase.Add(CardDB.cardName.armorplating, 1);
            //this.healthBuffDatabase.Add(CardDB.cardName.rooted, 5);

            this.tauntBuffDatabase.Add(CardDB.cardName.markofnature, 1);
            this.tauntBuffDatabase.Add(CardDB.cardName.markofthewild, 1);
            this.tauntBuffDatabase.Add(CardDB.cardName.darkwispers, 1);
            this.tauntBuffDatabase.Add(CardDB.cardName.rustyhorn, 1);
            this.tauntBuffDatabase.Add(CardDB.cardName.mutatinginjection, 1);
            this.tauntBuffDatabase.Add(CardDB.cardName.ancestralhealing, 1);
        }

        private void setupCardDrawBattlecry()
        {
            cardDrawBattleCryDatabase.Add(CardDB.cardName.echoofmedivh, 0);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.elitetaurenchieftain, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.flare, 1);
            //cardDrawBattleCryDatabase.Add(CardDB.cardName.ironjuggernaut, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.kingmukla, 2);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.mindpocalypse, 2);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.mindvision, 0);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.thoughtsteal, 0);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.tinkertowntechnician, 0); // If you have a Mech
            cardDrawBattleCryDatabase.Add(CardDB.cardName.toshley, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.tracking, 1); //NOT SUPPORTED YET
            cardDrawBattleCryDatabase.Add(CardDB.cardName.unstableportal, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.ancientoflore, 2);// choice =1
            cardDrawBattleCryDatabase.Add(CardDB.cardName.ancientteachings, 2);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.arcaneintellect, 2);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.azuredrake, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.battlerage, 0);//only if wounded own minions
            cardDrawBattleCryDatabase.Add(CardDB.cardName.callpet, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.coldlightoracle, 2);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.commandingshout, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.divinefavor, 0);//only if enemy has more cards than you
            cardDrawBattleCryDatabase.Add(CardDB.cardName.excessmana, 0);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.fanofknives, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.farsight, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.gnomishexperimenter, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.gnomishinventor, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.grovetender, 1); //choice = 2
            cardDrawBattleCryDatabase.Add(CardDB.cardName.hammerofwrath, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.harrisonjones, 0);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.holywrath, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.layonhands, 3);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.lifetap, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.massdispel, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.mortalcoil, 0);//only if kills
            cardDrawBattleCryDatabase.Add(CardDB.cardName.neptulon, 4);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.nourish, 3); //choice = 2
            cardDrawBattleCryDatabase.Add(CardDB.cardName.noviceengineer, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.powerwordshield, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.roguesdoit, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.shieldblock, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.shiv, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.slam, 0); //if survives
            cardDrawBattleCryDatabase.Add(CardDB.cardName.sprint, 4);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.starfire, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.wrath, 1); //choice=2
            cardDrawBattleCryDatabase.Add(CardDB.cardName.quickshot, 0);//only if your hand is empty
            cardDrawBattleCryDatabase.Add(CardDB.cardName.solemnvigil, 2);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.nefarian, 2);
        }

        
        private void setupUsefulNeedKeepDatabase()
        {
            UsefulNeedKeepDatabase.Add(CardDB.cardName.tradeprincegallywix, 5);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.ragnarosthefirelord, 5);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.kelthuzad, 18);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.mekgineerthermaplugg, 5);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.malganis, 13);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.gruul, 4);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.archmageantonidas, 7);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.troggzortheearthinator, 4);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.stormwindchampion, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.gazlowe, 6);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.weespellstopper, 11);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.violetteacher, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.siltfinspiritwalker, 5);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.siegeengine, 8);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.lightwell, 13);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.junkbot, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.impmaster, 5);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.illidanstormrage, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.felcannon, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.burlyrockjawtrogg, 5);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.summoningportal, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.natpagle, 2);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.leokk, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.jeeves, 0);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.hogger, 13);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.gadgetzanauctioneer, 9);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.frothingberserker, 9);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.floatingwatcher, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.emboldener3000, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.demolisher, 11);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.armorsmith, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.warsongcommander, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.vitalitytotem, 8);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.stonesplintertrogg, 8);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.southseacaptain, 11);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.shipscannon, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.shadowboxer, 11);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.repairbot, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.northshirecleric, 11);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.murlocwarleader, 11);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.mechwarper, 11);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.masterswordsmith, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.manawyrm, 9);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.manatidetotem, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.hobgoblin, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.flesheatingghoul, 9);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.flametonguetotem, 30);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.cobaltguardian, 8);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.alarmobot, 4);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.undertaker, 8);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.starvingbuzzard, 8);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.sorcerersapprentice, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.shadeofnaxxramas, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.secretkeeper, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.scavenginghyena, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.raidleader, 11);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.questingadventurer, 9);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.pintsizedsummoner, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.murloctidecaller, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.micromachine, 12);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.lightwarden, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.ironsensei, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.healingtotem, 9);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.direwolfalpha, 30);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.cultmaster, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.youngpriestess, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.timberwolf, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.homingchicken, 12);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.grimscaleoracle, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.bloodimp, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.illuminator, 2);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.faeriedragon, 7);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.prophetvelen, 5);

            UsefulNeedKeepDatabase.Add(CardDB.cardName.emperorthaurissan, 11);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.knifejuggler, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.flamewaker, 12);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.dragonkinsorcerer, 9);
        }

        private void setupDiscardCards()
        {
            cardDiscardDatabase.Add(CardDB.cardName.doomguard, 5);
            cardDiscardDatabase.Add(CardDB.cardName.soulfire, 0);
            cardDiscardDatabase.Add(CardDB.cardName.succubus, 2);
        }

        private void setupDestroyOwnCards()
        {
            this.destroyOwnDatabase.Add(CardDB.cardName.brawl, 0);
            this.destroyOwnDatabase.Add(CardDB.cardName.deathwing, 0);
            this.destroyOwnDatabase.Add(CardDB.cardName.twistingnether, 0);
            this.destroyOwnDatabase.Add(CardDB.cardName.naturalize, 0);//not own mins
            this.destroyOwnDatabase.Add(CardDB.cardName.shadowworddeath, 0);//not own mins
            this.destroyOwnDatabase.Add(CardDB.cardName.shadowwordpain, 0);//not own mins
            this.destroyOwnDatabase.Add(CardDB.cardName.siphonsoul, 0);//not own mins
            this.destroyOwnDatabase.Add(CardDB.cardName.biggamehunter, 0);//not own mins
            this.destroyOwnDatabase.Add(CardDB.cardName.hungrycrab, 0);//not own mins
            this.destroyOwnDatabase.Add(CardDB.cardName.sacrificialpact, 0);//not own mins

            this.destroyDatabase.Add(CardDB.cardName.assassinate, 0);//not own mins
            this.destroyDatabase.Add(CardDB.cardName.corruption, 0);//not own mins
            this.destroyDatabase.Add(CardDB.cardName.execute, 0);//not own mins
            this.destroyDatabase.Add(CardDB.cardName.naturalize, 0);//not own mins
            this.destroyDatabase.Add(CardDB.cardName.siphonsoul, 0);//not own mins
            this.destroyDatabase.Add(CardDB.cardName.mindcontrol, 0);//not own mins
            this.destroyDatabase.Add(CardDB.cardName.theblackknight, 0);//not own mins
            this.destroyDatabase.Add(CardDB.cardName.sabotage, 0);//not own mins
            this.destroyDatabase.Add(CardDB.cardName.crush, 0);//not own mins
            this.destroyDatabase.Add(CardDB.cardName.hemetnesingwary, 0);//not own mins
            this.destroyDatabase.Add(CardDB.cardName.deadlyshot, 0);
            this.destroyDatabase.Add(CardDB.cardName.shadowwordpain, 0);
            this.destroyDatabase.Add(CardDB.cardName.rendblackhand, 0);


        }

        private void setupReturnBackToHandCards()
        {
            returnHandDatabase.Add(CardDB.cardName.ancientbrewmaster, 0);
            returnHandDatabase.Add(CardDB.cardName.dream, 0);
            returnHandDatabase.Add(CardDB.cardName.kidnapper, 0);//if combo
            returnHandDatabase.Add(CardDB.cardName.shadowstep, 0);
            returnHandDatabase.Add(CardDB.cardName.vanish, 0);
            returnHandDatabase.Add(CardDB.cardName.youthfulbrewmaster, 0);
            returnHandDatabase.Add(CardDB.cardName.timerewinder, 0);
            returnHandDatabase.Add(CardDB.cardName.recycle, 0);
        }

        private void setupHeroDamagingAOE()
        {
            this.heroDamagingAoeDatabase.Add(CardDB.cardName.unknown, 0);
        }

        private void setupSpecialMins()
        {
            this.specialMinions.Add(CardDB.cardName.amaniberserker, 0);
            this.specialMinions.Add(CardDB.cardName.angrychicken, 0);
            this.specialMinions.Add(CardDB.cardName.abomination, 0);
            this.specialMinions.Add(CardDB.cardName.acolyteofpain, 0);
            this.specialMinions.Add(CardDB.cardName.alarmobot, 0);
            this.specialMinions.Add(CardDB.cardName.archmage, 0);
            this.specialMinions.Add(CardDB.cardName.archmageantonidas, 0);
            this.specialMinions.Add(CardDB.cardName.armorsmith, 0);
            this.specialMinions.Add(CardDB.cardName.auchenaisoulpriest, 0);
            this.specialMinions.Add(CardDB.cardName.azuredrake, 0);
            this.specialMinions.Add(CardDB.cardName.barongeddon, 0);
            this.specialMinions.Add(CardDB.cardName.bloodimp, 0);
            this.specialMinions.Add(CardDB.cardName.bloodmagethalnos, 0);
            this.specialMinions.Add(CardDB.cardName.cairnebloodhoof, 0);
            this.specialMinions.Add(CardDB.cardName.cultmaster, 0);
            this.specialMinions.Add(CardDB.cardName.dalaranmage, 0);
            this.specialMinions.Add(CardDB.cardName.demolisher, 0);
            this.specialMinions.Add(CardDB.cardName.direwolfalpha, 0);
            this.specialMinions.Add(CardDB.cardName.doomsayer, 0);
            this.specialMinions.Add(CardDB.cardName.emperorcobra, 0);
            this.specialMinions.Add(CardDB.cardName.etherealarcanist, 0);
            this.specialMinions.Add(CardDB.cardName.flametonguetotem, 0);
            this.specialMinions.Add(CardDB.cardName.flesheatingghoul, 0);
            this.specialMinions.Add(CardDB.cardName.gadgetzanauctioneer, 0);
            this.specialMinions.Add(CardDB.cardName.grimscaleoracle, 0);
            this.specialMinions.Add(CardDB.cardName.grommashhellscream, 0);
            this.specialMinions.Add(CardDB.cardName.gruul, 0);
            this.specialMinions.Add(CardDB.cardName.gurubashiberserker, 0);
            this.specialMinions.Add(CardDB.cardName.harvestgolem, 0);
            this.specialMinions.Add(CardDB.cardName.hogger, 0);
            this.specialMinions.Add(CardDB.cardName.illidanstormrage, 0);
            this.specialMinions.Add(CardDB.cardName.impmaster, 0);
            this.specialMinions.Add(CardDB.cardName.knifejuggler, 0);
            this.specialMinions.Add(CardDB.cardName.koboldgeomancer, 0);
            this.specialMinions.Add(CardDB.cardName.lepergnome, 0);
            this.specialMinions.Add(CardDB.cardName.lightspawn, 0);
            this.specialMinions.Add(CardDB.cardName.lightwarden, 0);
            this.specialMinions.Add(CardDB.cardName.lightwell, 0);
            this.specialMinions.Add(CardDB.cardName.loothoarder, 0);
            this.specialMinions.Add(CardDB.cardName.lorewalkercho, 0);
            this.specialMinions.Add(CardDB.cardName.malygos, 0);
            this.specialMinions.Add(CardDB.cardName.manaaddict, 0);
            this.specialMinions.Add(CardDB.cardName.manatidetotem, 0);
            this.specialMinions.Add(CardDB.cardName.manawraith, 0);
            this.specialMinions.Add(CardDB.cardName.manawyrm, 0);
            this.specialMinions.Add(CardDB.cardName.masterswordsmith, 0);
            this.specialMinions.Add(CardDB.cardName.murloctidecaller, 0);
            this.specialMinions.Add(CardDB.cardName.murlocwarleader, 0);
            this.specialMinions.Add(CardDB.cardName.natpagle, 0);
            this.specialMinions.Add(CardDB.cardName.northshirecleric, 0);
            this.specialMinions.Add(CardDB.cardName.ogremagi, 0);
            this.specialMinions.Add(CardDB.cardName.oldmurkeye, 0);
            this.specialMinions.Add(CardDB.cardName.patientassassin, 0);
            this.specialMinions.Add(CardDB.cardName.pintsizedsummoner, 0);
            this.specialMinions.Add(CardDB.cardName.prophetvelen, 0);
            this.specialMinions.Add(CardDB.cardName.questingadventurer, 0);
            this.specialMinions.Add(CardDB.cardName.ragingworgen, 0);
            this.specialMinions.Add(CardDB.cardName.raidleader, 0);
            this.specialMinions.Add(CardDB.cardName.savannahhighmane, 0);
            this.specialMinions.Add(CardDB.cardName.scavenginghyena, 0);
            this.specialMinions.Add(CardDB.cardName.secretkeeper, 0);
            this.specialMinions.Add(CardDB.cardName.sorcerersapprentice, 0);
            this.specialMinions.Add(CardDB.cardName.southseacaptain, 0);
            this.specialMinions.Add(CardDB.cardName.spitefulsmith, 0);
            this.specialMinions.Add(CardDB.cardName.starvingbuzzard, 0);
            this.specialMinions.Add(CardDB.cardName.stormwindchampion, 0);
            this.specialMinions.Add(CardDB.cardName.summoningportal, 0);
            this.specialMinions.Add(CardDB.cardName.sylvanaswindrunner, 0);
            this.specialMinions.Add(CardDB.cardName.taurenwarrior, 0);
            this.specialMinions.Add(CardDB.cardName.thebeast, 0);
            this.specialMinions.Add(CardDB.cardName.timberwolf, 0);
            this.specialMinions.Add(CardDB.cardName.tirionfordring, 0);
            this.specialMinions.Add(CardDB.cardName.tundrarhino, 0);
            this.specialMinions.Add(CardDB.cardName.unboundelemental, 0);
            //this.specialMinions.Add(CardDB.cardName.venturecomercenary, 0);
            this.specialMinions.Add(CardDB.cardName.violetteacher, 0);
            this.specialMinions.Add(CardDB.cardName.warsongcommander, 0);
            this.specialMinions.Add(CardDB.cardName.waterelemental, 0);

            this.specialMinions.Add(CardDB.cardName.grimpatron, 0);
            this.specialMinions.Add(CardDB.cardName.emperorthaurissan, 0);
            this.specialMinions.Add(CardDB.cardName.impgangboss, 0);
            this.specialMinions.Add(CardDB.cardName.axeflinger, 0);
            this.specialMinions.Add(CardDB.cardName.dragonegg, 0);

            // naxx cards
            this.specialMinions.Add(CardDB.cardName.baronrivendare, 0);
            this.specialMinions.Add(CardDB.cardName.undertaker, 0);
            this.specialMinions.Add(CardDB.cardName.dancingswords, 0);
            this.specialMinions.Add(CardDB.cardName.darkcultist, 0);
            this.specialMinions.Add(CardDB.cardName.deathlord, 0);
            this.specialMinions.Add(CardDB.cardName.feugen, 0);
            this.specialMinions.Add(CardDB.cardName.stalagg, 0);
            this.specialMinions.Add(CardDB.cardName.hauntedcreeper, 0);
            this.specialMinions.Add(CardDB.cardName.kelthuzad, 0);
            this.specialMinions.Add(CardDB.cardName.madscientist, 0);
            this.specialMinions.Add(CardDB.cardName.maexxna, 0);
            this.specialMinions.Add(CardDB.cardName.nerubarweblord, 0);
            this.specialMinions.Add(CardDB.cardName.shadeofnaxxramas, 0);
            this.specialMinions.Add(CardDB.cardName.unstableghoul, 0);
            this.specialMinions.Add(CardDB.cardName.voidcaller, 0);
            this.specialMinions.Add(CardDB.cardName.anubarambusher, 0);
            this.specialMinions.Add(CardDB.cardName.webspinner, 0);
            this.specialMinions.Add(CardDB.cardName.flamewaker, 0);
            this.specialMinions.Add(CardDB.cardName.chromaggus, 0);
            this.specialMinions.Add(CardDB.cardName.dragonkinsorcerer, 0);

        }

        private void setupBuffingMinions()
        {
            buffingMinionsDatabase.Add(CardDB.cardName.abusivesergeant, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.captaingreenskin, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.cenarius, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.coldlightseer, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.crueltaskmaster, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.darkirondwarf, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.defenderofargus, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.direwolfalpha, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.flametonguetotem, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.grimscaleoracle, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.houndmaster, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.leokk, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.murlocwarleader, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.raidleader, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.shatteredsuncleric, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.southseacaptain, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.spitefulsmith, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.stormwindchampion, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.templeenforcer, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.timberwolf, 0);

            buffing1TurnDatabase.Add(CardDB.cardName.abusivesergeant, 0);
            buffing1TurnDatabase.Add(CardDB.cardName.darkirondwarf, 0);

        }

        private void setupEnemyTargetPriority()
        {
            priorityTargets.Add(CardDB.cardName.angrychicken, 10);
            priorityTargets.Add(CardDB.cardName.lightwarden, 10);
            priorityTargets.Add(CardDB.cardName.secretkeeper, 10);
            priorityTargets.Add(CardDB.cardName.youngdragonhawk, 10);
            priorityTargets.Add(CardDB.cardName.bloodmagethalnos, 10);
            priorityTargets.Add(CardDB.cardName.direwolfalpha, 10);
            priorityTargets.Add(CardDB.cardName.doomsayer, 10);
            priorityTargets.Add(CardDB.cardName.knifejuggler, 10);
            priorityTargets.Add(CardDB.cardName.koboldgeomancer, 10);
            priorityTargets.Add(CardDB.cardName.manaaddict, 10);
            priorityTargets.Add(CardDB.cardName.masterswordsmith, 10);
            priorityTargets.Add(CardDB.cardName.natpagle, 10);
            priorityTargets.Add(CardDB.cardName.murloctidehunter, 10);
            priorityTargets.Add(CardDB.cardName.pintsizedsummoner, 10);
            priorityTargets.Add(CardDB.cardName.wildpyromancer, 10);
            priorityTargets.Add(CardDB.cardName.alarmobot, 10);
            priorityTargets.Add(CardDB.cardName.acolyteofpain, 10);
            priorityTargets.Add(CardDB.cardName.demolisher, 10);
            priorityTargets.Add(CardDB.cardName.flesheatingghoul, 10);
            priorityTargets.Add(CardDB.cardName.impmaster, 10);
            priorityTargets.Add(CardDB.cardName.questingadventurer, 10);
            priorityTargets.Add(CardDB.cardName.raidleader, 10);
            priorityTargets.Add(CardDB.cardName.thrallmarfarseer, 10);
            priorityTargets.Add(CardDB.cardName.cultmaster, 10);
            priorityTargets.Add(CardDB.cardName.leeroyjenkins, 10);
            priorityTargets.Add(CardDB.cardName.violetteacher, 10);
            priorityTargets.Add(CardDB.cardName.gadgetzanauctioneer, 10);
            priorityTargets.Add(CardDB.cardName.hogger, 10);
            priorityTargets.Add(CardDB.cardName.illidanstormrage, 10);
            priorityTargets.Add(CardDB.cardName.barongeddon, 10);
            priorityTargets.Add(CardDB.cardName.stormwindchampion, 10);
            priorityTargets.Add(CardDB.cardName.gurubashiberserker, 10);

            //BRM cards
            priorityTargets.Add(CardDB.cardName.grimpatron, 10);
            priorityTargets.Add(CardDB.cardName.emperorthaurissan, 10);
            priorityTargets.Add(CardDB.cardName.impgangboss, 10);
            priorityTargets.Add(CardDB.cardName.dragonegg, 10);
            priorityTargets.Add(CardDB.cardName.chromaggus, 10);
            priorityTargets.Add(CardDB.cardName.flamewaker, 10);
            priorityTargets.Add(CardDB.cardName.dragonkinsorcerer, 4);

            //warrior cards
            priorityTargets.Add(CardDB.cardName.frothingberserker, 10);
            priorityTargets.Add(CardDB.cardName.warsongcommander, 10);

            //warlock cards
            priorityTargets.Add(CardDB.cardName.summoningportal, 10);

            //shaman cards
            priorityTargets.Add(CardDB.cardName.dustdevil, 10);
            priorityTargets.Add(CardDB.cardName.wrathofairtotem, 1);
            priorityTargets.Add(CardDB.cardName.flametonguetotem, 10);
            priorityTargets.Add(CardDB.cardName.manatidetotem, 10);
            priorityTargets.Add(CardDB.cardName.unboundelemental, 10);

            //rogue cards

            //priest cards
            priorityTargets.Add(CardDB.cardName.northshirecleric, 10);
            priorityTargets.Add(CardDB.cardName.lightwell, 10);
            priorityTargets.Add(CardDB.cardName.auchenaisoulpriest, 10);
            priorityTargets.Add(CardDB.cardName.prophetvelen, 10);

            //paladin cards

            //mage cards
            priorityTargets.Add(CardDB.cardName.manawyrm, 10);
            priorityTargets.Add(CardDB.cardName.sorcerersapprentice, 10);
            priorityTargets.Add(CardDB.cardName.etherealarcanist, 10);
            priorityTargets.Add(CardDB.cardName.archmageantonidas, 10);

            //hunter cards
            priorityTargets.Add(CardDB.cardName.timberwolf, 10);
            priorityTargets.Add(CardDB.cardName.scavenginghyena, 10);
            priorityTargets.Add(CardDB.cardName.starvingbuzzard, 10);
            priorityTargets.Add(CardDB.cardName.leokk, 10);
            priorityTargets.Add(CardDB.cardName.tundrarhino, 10);

            //naxx cards
            priorityTargets.Add(CardDB.cardName.baronrivendare, 10);
            priorityTargets.Add(CardDB.cardName.kelthuzad, 10);
            priorityTargets.Add(CardDB.cardName.nerubarweblord, 10);
            priorityTargets.Add(CardDB.cardName.shadeofnaxxramas, 10);
            priorityTargets.Add(CardDB.cardName.undertaker, 10);

        }

        private void setupLethalHelpMinions()
        {
            lethalHelpers.Add(CardDB.cardName.auchenaisoulpriest, 0);
            //spellpower minions
            lethalHelpers.Add(CardDB.cardName.archmage, 0);
            lethalHelpers.Add(CardDB.cardName.dalaranmage, 0);
            lethalHelpers.Add(CardDB.cardName.koboldgeomancer, 0);
            lethalHelpers.Add(CardDB.cardName.ogremagi, 0);
            lethalHelpers.Add(CardDB.cardName.ancientmage, 0);
            lethalHelpers.Add(CardDB.cardName.azuredrake, 0);
            lethalHelpers.Add(CardDB.cardName.bloodmagethalnos, 0);
            lethalHelpers.Add(CardDB.cardName.malygos, 0);
            lethalHelpers.Add(CardDB.cardName.velenschosen, 0);
            lethalHelpers.Add(CardDB.cardName.sootspewer, 0);
            lethalHelpers.Add(CardDB.cardName.minimage, 0);
            //

        }
        
        private void setupRelations()
        {
            spellDependentDatabase.Add(CardDB.cardName.wildpyromancer, 1);
            spellDependentDatabase.Add(CardDB.cardName.lorewalkercho, 0);
            spellDependentDatabase.Add(CardDB.cardName.gazlowe, 1);
            spellDependentDatabase.Add(CardDB.cardName.archmageantonidas, 2);
            spellDependentDatabase.Add(CardDB.cardName.gadgetzanauctioneer, 2);
            spellDependentDatabase.Add(CardDB.cardName.manawyrm, 1);
            spellDependentDatabase.Add(CardDB.cardName.manaaddict, 1);
            spellDependentDatabase.Add(CardDB.cardName.violetteacher, 1);
            spellDependentDatabase.Add(CardDB.cardName.stonesplintertrogg, -1);
            spellDependentDatabase.Add(CardDB.cardName.burlyrockjawtrogg, -1);
            spellDependentDatabase.Add(CardDB.cardName.tradeprincegallywix, -2);
            spellDependentDatabase.Add(CardDB.cardName.troggzortheearthinator, -1);
            spellDependentDatabase.Add(CardDB.cardName.flamewaker, 1);
            spellDependentDatabase.Add(CardDB.cardName.chromaggus, 1);
        }

        private void setupSilenceTargets()
        {
            this.silenceTargets.Add(CardDB.cardName.abomination, 0);
            this.silenceTargets.Add(CardDB.cardName.acolyteofpain, 0);
            this.silenceTargets.Add(CardDB.cardName.archmageantonidas, 0);
            this.silenceTargets.Add(CardDB.cardName.armorsmith, 0);
            this.silenceTargets.Add(CardDB.cardName.auchenaisoulpriest, 0);
            this.silenceTargets.Add(CardDB.cardName.barongeddon, 0);
            this.silenceTargets.Add(CardDB.cardName.baronrivendare, 0);
            this.silenceTargets.Add(CardDB.cardName.bloodimp, 0);
            this.silenceTargets.Add(CardDB.cardName.bolvarfordragon, 0);
            this.silenceTargets.Add(CardDB.cardName.burlyrockjawtrogg, 0);
            this.silenceTargets.Add(CardDB.cardName.cobaltguardian, 0);
            this.silenceTargets.Add(CardDB.cardName.cultmaster, 0);
            this.silenceTargets.Add(CardDB.cardName.direwolfalpha, 0);
            this.silenceTargets.Add(CardDB.cardName.doomsayer, 0);
            this.silenceTargets.Add(CardDB.cardName.emboldener3000, 0);
            this.silenceTargets.Add(CardDB.cardName.emperorcobra, 0);
            this.silenceTargets.Add(CardDB.cardName.etherealarcanist, 0);
            this.silenceTargets.Add(CardDB.cardName.flametonguetotem, 0);
            this.silenceTargets.Add(CardDB.cardName.flesheatingghoul, 0);
            this.silenceTargets.Add(CardDB.cardName.floatingwatcher, 0);
            this.silenceTargets.Add(CardDB.cardName.foereaper4000, 0);
            this.silenceTargets.Add(CardDB.cardName.frothingberserker, 0);
            this.silenceTargets.Add(CardDB.cardName.gadgetzanauctioneer, 10);
            this.silenceTargets.Add(CardDB.cardName.gahzrilla, 0);
            this.silenceTargets.Add(CardDB.cardName.grimscaleoracle, 0);
            this.silenceTargets.Add(CardDB.cardName.grommashhellscream, 0);
            this.silenceTargets.Add(CardDB.cardName.gruul, 0);
            this.silenceTargets.Add(CardDB.cardName.gurubashiberserker, 0);
            this.silenceTargets.Add(CardDB.cardName.hobgoblin, 0);
            this.silenceTargets.Add(CardDB.cardName.hogger, 0);
            this.silenceTargets.Add(CardDB.cardName.homingchicken, 0);
            this.silenceTargets.Add(CardDB.cardName.illidanstormrage, 0);
            this.silenceTargets.Add(CardDB.cardName.impmaster, 0);
            this.silenceTargets.Add(CardDB.cardName.ironsensei, 0);
            this.silenceTargets.Add(CardDB.cardName.jeeves, 0);
            this.silenceTargets.Add(CardDB.cardName.junkbot, 0);
            this.silenceTargets.Add(CardDB.cardName.kelthuzad, 10);
            this.silenceTargets.Add(CardDB.cardName.knifejuggler, 0);
            this.silenceTargets.Add(CardDB.cardName.leokk, 0);
            this.silenceTargets.Add(CardDB.cardName.lightspawn, 0);
            this.silenceTargets.Add(CardDB.cardName.lightwarden, 0);
            this.silenceTargets.Add(CardDB.cardName.lightwell, 0);
            this.silenceTargets.Add(CardDB.cardName.lorewalkercho, 0);
            this.silenceTargets.Add(CardDB.cardName.maexxna, 0);
            this.silenceTargets.Add(CardDB.cardName.malganis, 0);
            this.silenceTargets.Add(CardDB.cardName.malygos, 0);
            this.silenceTargets.Add(CardDB.cardName.manaaddict, 0);
            this.silenceTargets.Add(CardDB.cardName.manatidetotem, 0);
            this.silenceTargets.Add(CardDB.cardName.manawraith, 0);
            this.silenceTargets.Add(CardDB.cardName.manawyrm, 0);
            this.silenceTargets.Add(CardDB.cardName.masterswordsmith, 0);
            this.silenceTargets.Add(CardDB.cardName.mekgineerthermaplugg, 0);
            this.silenceTargets.Add(CardDB.cardName.micromachine, 0);
            this.silenceTargets.Add(CardDB.cardName.mogortheogre, 0);
            this.silenceTargets.Add(CardDB.cardName.murloctidecaller, 0);
            this.silenceTargets.Add(CardDB.cardName.murlocwarleader, 0);
            this.silenceTargets.Add(CardDB.cardName.natpagle, 0);
            this.silenceTargets.Add(CardDB.cardName.nerubarweblord, 0);
            this.silenceTargets.Add(CardDB.cardName.northshirecleric, 0);
            this.silenceTargets.Add(CardDB.cardName.oldmurkeye, 0);
            this.silenceTargets.Add(CardDB.cardName.oneeyedcheat, 0);
            this.silenceTargets.Add(CardDB.cardName.prophetvelen, 0);
            this.silenceTargets.Add(CardDB.cardName.questingadventurer, 0);
            this.silenceTargets.Add(CardDB.cardName.ragingworgen, 0);
            this.silenceTargets.Add(CardDB.cardName.raidleader, 0);
            this.silenceTargets.Add(CardDB.cardName.scavenginghyena, 0);
            this.silenceTargets.Add(CardDB.cardName.secretkeeper, 0);
            this.silenceTargets.Add(CardDB.cardName.shadeofnaxxramas, 0);
            this.silenceTargets.Add(CardDB.cardName.shadowboxer, 0);
            this.silenceTargets.Add(CardDB.cardName.shipscannon, 0);
            this.silenceTargets.Add(CardDB.cardName.siegeengine, 0);
            this.silenceTargets.Add(CardDB.cardName.siltfinspiritwalker, 0);
            this.silenceTargets.Add(CardDB.cardName.sorcerersapprentice, 0);
            this.silenceTargets.Add(CardDB.cardName.southseacaptain, 0);
            this.silenceTargets.Add(CardDB.cardName.spitefulsmith, 0);
            this.silenceTargets.Add(CardDB.cardName.starvingbuzzard, 0);
            this.silenceTargets.Add(CardDB.cardName.steamwheedlesniper, 0);
            this.silenceTargets.Add(CardDB.cardName.stonesplintertrogg, 0);
            this.silenceTargets.Add(CardDB.cardName.stormwindchampion, 0);
            this.silenceTargets.Add(CardDB.cardName.summoningportal, 0);
            this.silenceTargets.Add(CardDB.cardName.timberwolf, 0);
            this.silenceTargets.Add(CardDB.cardName.tirionfordring, 0);
            this.silenceTargets.Add(CardDB.cardName.tradeprincegallywix, 0);
            this.silenceTargets.Add(CardDB.cardName.troggzortheearthinator, 0);
            this.silenceTargets.Add(CardDB.cardName.tundrarhino, 0);
            this.silenceTargets.Add(CardDB.cardName.unboundelemental, 0);
            this.silenceTargets.Add(CardDB.cardName.undertaker, 0);
            this.silenceTargets.Add(CardDB.cardName.v07tr0n, 0);
            this.silenceTargets.Add(CardDB.cardName.violetteacher, 0);
            this.silenceTargets.Add(CardDB.cardName.warsongcommander, 0);
            this.silenceTargets.Add(CardDB.cardName.youngpriestess, 0);
            this.silenceTargets.Add(CardDB.cardName.ysera, 0);

            this.silenceTargets.Add(CardDB.cardName.grimpatron, 0);
            this.silenceTargets.Add(CardDB.cardName.emperorthaurissan, 0);
            this.silenceTargets.Add(CardDB.cardName.impgangboss, 0);
            this.silenceTargets.Add(CardDB.cardName.axeflinger, 0);
            this.silenceTargets.Add(CardDB.cardName.dragonegg, 0);
            this.silenceTargets.Add(CardDB.cardName.flamewaker, 0);
            this.silenceTargets.Add(CardDB.cardName.chromaggus, 0);
            this.silenceTargets.Add(CardDB.cardName.dragonkinsorcerer, 0);

            //this.silenceTargets.Add(CardDB.cardName.bloodimp, 0);
            //this.specialMinions.Add(CardDB.cardName.unboundelemental, 0);
            //this.specialMinions.Add(CardDB.cardName.venturecomercenary, 0);
            //this.specialMinions.Add(CardDB.cardName.waterelemental, 0);
            //this.specialMinions.Add(CardDB.cardName.voidcaller, 0);
        }

        private void setupRandomCards()
        {
            this.randomEffects.Add(CardDB.cardName.deadlyshot, 1);
            this.randomEffects.Add(CardDB.cardName.multishot, 1);

            this.randomEffects.Add(CardDB.cardName.animalcompanion, 1);
            this.randomEffects.Add(CardDB.cardName.arcanemissiles, 3);
            this.randomEffects.Add(CardDB.cardName.goblinblastmage, 4);
            this.randomEffects.Add(CardDB.cardName.avengingwrath, 8);

            this.randomEffects.Add(CardDB.cardName.flamecannon, 1);

            //this.randomEffects.Add(CardDB.cardName.baneofdoom, 1);
            this.randomEffects.Add(CardDB.cardName.brawl, 1);
            this.randomEffects.Add(CardDB.cardName.captainsparrot, 1);
            this.randomEffects.Add(CardDB.cardName.cleave, 2);
            this.randomEffects.Add(CardDB.cardName.forkedlightning, 1);
            this.randomEffects.Add(CardDB.cardName.gelbinmekkatorque, 1);
            this.randomEffects.Add(CardDB.cardName.iammurloc, 3);
            this.randomEffects.Add(CardDB.cardName.lightningstorm, 1);
            this.randomEffects.Add(CardDB.cardName.madbomber, 3);
            this.randomEffects.Add(CardDB.cardName.mindgames, 1);
            this.randomEffects.Add(CardDB.cardName.mindcontroltech, 1);
            this.randomEffects.Add(CardDB.cardName.mindvision, 1);
            this.randomEffects.Add(CardDB.cardName.powerofthehorde, 1);
            this.randomEffects.Add(CardDB.cardName.sensedemons, 2);
            this.randomEffects.Add(CardDB.cardName.tinkmasteroverspark, 1);
            this.randomEffects.Add(CardDB.cardName.totemiccall, 1);
            this.randomEffects.Add(CardDB.cardName.elitetaurenchieftain, 1);
            this.randomEffects.Add(CardDB.cardName.lifetap, 1);

            this.randomEffects.Add(CardDB.cardName.unstableportal, 1);
            this.randomEffects.Add(CardDB.cardName.crackle, 1);
            this.randomEffects.Add(CardDB.cardName.bouncingblade, 3);
            this.randomEffects.Add(CardDB.cardName.coghammer, 1);
            this.randomEffects.Add(CardDB.cardName.madderbomber, 1);
            this.randomEffects.Add(CardDB.cardName.bomblobber, 1);
            this.randomEffects.Add(CardDB.cardName.enhanceomechano, 1);
        }
        

        private void setupChooseDatabase()
        {
            this.choose1database.Add(CardDB.cardName.wrath, CardDB.cardIDEnum.EX1_154a);
            this.choose1database.Add(CardDB.cardName.starfall, CardDB.cardIDEnum.NEW1_007b);
            this.choose1database.Add(CardDB.cardName.powerofthewild, CardDB.cardIDEnum.EX1_160b);
            this.choose1database.Add(CardDB.cardName.nourish, CardDB.cardIDEnum.EX1_164a);
            this.choose1database.Add(CardDB.cardName.markofnature, CardDB.cardIDEnum.EX1_155a);
            this.choose1database.Add(CardDB.cardName.keeperofthegrove, CardDB.cardIDEnum.EX1_166a);
            this.choose1database.Add(CardDB.cardName.grovetender, CardDB.cardIDEnum.GVG_032a);
            this.choose1database.Add(CardDB.cardName.druidoftheflame, CardDB.cardIDEnum.BRM_010t);
            this.choose1database.Add(CardDB.cardName.druidoftheclaw, CardDB.cardIDEnum.EX1_165t1);
            this.choose1database.Add(CardDB.cardName.darkwispers, CardDB.cardIDEnum.GVG_041b);
            this.choose1database.Add(CardDB.cardName.cenarius, CardDB.cardIDEnum.EX1_573a);
            this.choose1database.Add(CardDB.cardName.anodizedrobocub, CardDB.cardIDEnum.GVG_030a);
            this.choose1database.Add(CardDB.cardName.ancientofwar, CardDB.cardIDEnum.EX1_178a);
            this.choose1database.Add(CardDB.cardName.ancientoflore, CardDB.cardIDEnum.NEW1_008a);

            this.choose2database.Add(CardDB.cardName.wrath, CardDB.cardIDEnum.EX1_154b);
            this.choose2database.Add(CardDB.cardName.starfall, CardDB.cardIDEnum.NEW1_007a);
            this.choose2database.Add(CardDB.cardName.powerofthewild, CardDB.cardIDEnum.EX1_160a);
            this.choose2database.Add(CardDB.cardName.nourish, CardDB.cardIDEnum.EX1_164b);
            this.choose2database.Add(CardDB.cardName.markofnature, CardDB.cardIDEnum.EX1_155b);
            this.choose2database.Add(CardDB.cardName.keeperofthegrove, CardDB.cardIDEnum.EX1_166b);
            this.choose2database.Add(CardDB.cardName.grovetender, CardDB.cardIDEnum.GVG_032b);
            this.choose2database.Add(CardDB.cardName.druidoftheflame, CardDB.cardIDEnum.BRM_010t2);
            this.choose2database.Add(CardDB.cardName.druidoftheclaw, CardDB.cardIDEnum.EX1_165t2);
            this.choose2database.Add(CardDB.cardName.darkwispers, CardDB.cardIDEnum.GVG_041a);
            this.choose2database.Add(CardDB.cardName.cenarius, CardDB.cardIDEnum.EX1_573b);
            this.choose2database.Add(CardDB.cardName.anodizedrobocub, CardDB.cardIDEnum.GVG_030b);
            this.choose2database.Add(CardDB.cardName.ancientofwar, CardDB.cardIDEnum.EX1_178b);
            this.choose2database.Add(CardDB.cardName.ancientoflore, CardDB.cardIDEnum.NEW1_008b);
        }


    }

}