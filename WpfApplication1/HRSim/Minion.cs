//using Triton.Game.Mapping;

namespace HRSim
{
    using System;
    using System.Collections.Generic;

    public class miniEnch
    {
        public CardDB.cardIDEnum CARDID = CardDB.cardIDEnum.None;
        public int creator = 0; // the minion
        public int controllerOfCreator = 0; // own or enemys buff?

        public miniEnch(CardDB.cardIDEnum id, int crtr, int controler)
        {
            this.CARDID = id;
            this.creator = crtr;
            this.controllerOfCreator = controler;
        }

    }

    public class Minion
    {

        //for policy
        public double targetProb = 1.0;

        //dont silence----------------------------
        public int anzGotDmg = 0;
        public bool isHero = false;
        public bool own;

        public CardDB.cardName name = CardDB.cardName.unknown;
        public TAG_CLASS cardClass = TAG_CLASS.INVALID;
        public Handmanager.Handcard handcard;
        public int entitiyID = -1;
        //public int id = -1;//delete this
        public int zonepos = 0;

        public bool playedThisTurn = false;
        public int numAttacksThisTurn = 0;
        public bool immuneWhileAttacking = false;

        public bool allreadyAttacked = false;

        //---------------------------------------
        public bool shadowmadnessed = false;//´can be silenced :D

        public int ancestralspirit = 0;
        public bool destroyOnOwnTurnStart = false; // depends on own!
        public bool destroyOnEnemyTurnStart = false; // depends on own!
        public bool destroyOnOwnTurnEnd = false; // depends on own!
        public bool destroyOnEnemyTurnEnd = false; // depends on own!

        public bool concedal = false;
        public int souloftheforest = 0;

        public int ownBlessingOfWisdom = 0;
        public int enemyBlessingOfWisdom = 0;
        public int spellpower = 0;

        public bool cantBeTargetedBySpellsOrHeroPowers = false;

        public int Hp = 0;
        public int maxHp = 0;
        public int armor = 0;

        public int Angr = 0;
        public int AdjacentAngr = 0;
        public int tempAttack = 0;

        public bool Ready = false;

        public bool taunt = false;
        public bool wounded = false;//hp red?

        public bool divineshild = false;
        public bool windfury = false;
        public bool frozen = false;
        public bool stealth = false;
        public bool immune = false;
        public bool exhausted = false;

        public int charge = 0;
        public bool poisonous = false;
        public bool cantLowerHPbelowONE = false;

        public bool silenced = false;
        public bool extraParam = false;

        public Minion()
        {
            this.handcard = new Handmanager.Handcard();
        }

        public Minion(Minion m)
        {
            this.targetProb = m.targetProb;
            //dont silence----------------------------
            this.anzGotDmg = m.anzGotDmg;
            this.isHero = m.isHero;
            this.own = m.own;

            this.name = m.name;
            this.cardClass = m.cardClass;
            this.handcard = m.handcard;//new?
            this.entitiyID = m.entitiyID;
            this.zonepos = m.zonepos;

            this.allreadyAttacked = m.allreadyAttacked;


            this.playedThisTurn = m.playedThisTurn;
            this.numAttacksThisTurn = m.numAttacksThisTurn;
            this.immuneWhileAttacking = m.immuneWhileAttacking;

            //---------------------------------------
            this.shadowmadnessed = m.shadowmadnessed;

            this.ancestralspirit = m.ancestralspirit;
            this.destroyOnOwnTurnStart = m.destroyOnOwnTurnStart; // depends on own!
            this.destroyOnEnemyTurnStart = m.destroyOnEnemyTurnStart; // depends on own!
            this.destroyOnOwnTurnEnd = m.destroyOnOwnTurnEnd; // depends on own!
            this.destroyOnEnemyTurnEnd = m.destroyOnEnemyTurnEnd; // depends on own!

            this.concedal = m.concedal;
            this.souloftheforest = m.souloftheforest;

            this.ownBlessingOfWisdom = m.ownBlessingOfWisdom;
            this.enemyBlessingOfWisdom = m.enemyBlessingOfWisdom;
            this.spellpower = m.spellpower;

            this.Hp = m.Hp;
            this.maxHp = m.maxHp;
            this.armor = m.armor;

            this.Angr = m.Angr;
            this.AdjacentAngr = m.AdjacentAngr;
            this.tempAttack = m.tempAttack;

            this.Ready = m.Ready;

            this.taunt = m.taunt;
            this.wounded = m.wounded;

            this.divineshild = m.divineshild;
            this.windfury = m.windfury;
            this.frozen = m.frozen;
            this.stealth = m.stealth;
            this.immune = m.immune;
            this.exhausted = m.exhausted;

            this.charge = m.charge;
            this.poisonous = m.poisonous;
            this.cantLowerHPbelowONE = m.cantLowerHPbelowONE;

            this.silenced = m.silenced;

            // Drew: fixed
            this.cantBeTargetedBySpellsOrHeroPowers = m.cantBeTargetedBySpellsOrHeroPowers;
        }

        public Minion(HREngine.Bots.Minion m)
        {
            this.targetProb = 1.0;
            //dont silence----------------------------
            this.anzGotDmg = m.anzGotDmg;
            this.isHero = m.isHero;
            this.own = m.own;

            this.name = (CardDB.cardName)Enum.Parse(typeof(CardDB.cardName), m.name.ToString());
            this.cardClass = (TAG_CLASS)Enum.Parse(typeof(TAG_CLASS), m.cardClass.ToString());
            this.handcard = new Handmanager.Handcard(m.handcard);//new?
            this.entitiyID = m.entitiyID;
            this.zonepos = m.zonepos;

            this.allreadyAttacked = m.allreadyAttacked;


            this.playedThisTurn = m.playedThisTurn;
            this.numAttacksThisTurn = m.numAttacksThisTurn;
            this.immuneWhileAttacking = m.immuneWhileAttacking;

            //---------------------------------------
            this.shadowmadnessed = m.shadowmadnessed;

            this.ancestralspirit = m.ancestralspirit;
            this.destroyOnOwnTurnStart = m.destroyOnOwnTurnStart; // depends on own!
            this.destroyOnEnemyTurnStart = m.destroyOnEnemyTurnStart; // depends on own!
            this.destroyOnOwnTurnEnd = m.destroyOnOwnTurnEnd; // depends on own!
            this.destroyOnEnemyTurnEnd = m.destroyOnEnemyTurnEnd; // depends on own!

            this.concedal = m.concedal;
            this.souloftheforest = m.souloftheforest;

            this.ownBlessingOfWisdom = m.ownBlessingOfWisdom;
            this.enemyBlessingOfWisdom = m.enemyBlessingOfWisdom;
            this.spellpower = m.spellpower;

            this.Hp = m.Hp;
            this.maxHp = m.maxHp;
            this.armor = m.armor;

            this.Angr = m.Angr;
            this.AdjacentAngr = m.AdjacentAngr;
            this.tempAttack = m.tempAttack;

            this.Ready = m.Ready;

            this.taunt = m.taunt;
            this.wounded = m.wounded;

            this.divineshild = m.divineshild;
            this.windfury = m.windfury;
            this.frozen = m.frozen;
            this.stealth = m.stealth;
            this.immune = m.immune;
            this.exhausted = m.exhausted;

            this.charge = m.charge;
            this.poisonous = m.poisonous;
            this.cantLowerHPbelowONE = m.cantLowerHPbelowONE;

            this.silenced = m.silenced;

            // Drew: fixed
            this.cantBeTargetedBySpellsOrHeroPowers = m.cantBeTargetedBySpellsOrHeroPowers;
        }

        public void setMinionTominion(Minion m)
        {
            //dont silence----------------------------
            this.anzGotDmg = m.anzGotDmg;
            this.isHero = m.isHero;
            this.own = m.own;

            this.name = m.name;
            this.cardClass = m.cardClass;
            this.handcard = m.handcard;//new?
            //this.entitiyID = m.entitiyID;
            this.zonepos = m.zonepos;


            this.allreadyAttacked = m.allreadyAttacked;

            this.playedThisTurn = m.playedThisTurn;
            this.numAttacksThisTurn = m.numAttacksThisTurn;
            this.immuneWhileAttacking = m.immuneWhileAttacking;

            //---------------------------------------
            this.shadowmadnessed = m.shadowmadnessed;

            this.ancestralspirit = m.ancestralspirit;
            this.destroyOnOwnTurnStart = m.destroyOnOwnTurnStart; // depends on own!
            this.destroyOnEnemyTurnStart = m.destroyOnEnemyTurnStart; // depends on own!
            this.destroyOnOwnTurnEnd = m.destroyOnOwnTurnEnd; // depends on own!
            this.destroyOnEnemyTurnEnd = m.destroyOnEnemyTurnEnd; // depends on own!

            this.concedal = m.concedal;
            this.souloftheforest = m.souloftheforest;

            this.ownBlessingOfWisdom = m.ownBlessingOfWisdom;
            this.enemyBlessingOfWisdom = m.enemyBlessingOfWisdom;
            this.spellpower = m.spellpower;

            this.Hp = (m.Hp > 0)? m.Hp: m.maxHp;
            this.maxHp = m.maxHp;
            this.armor = m.armor;

            this.Angr = m.Angr;
            this.AdjacentAngr = m.AdjacentAngr;
            this.tempAttack = m.tempAttack;

            this.Ready = m.Ready;

            this.taunt = m.taunt;
            this.wounded = m.wounded;

            this.divineshild = m.divineshild;
            this.windfury = m.windfury;
            this.frozen = m.frozen;
            this.stealth = m.stealth;
            this.immune = m.immune;
            this.exhausted = m.exhausted;

            this.charge = m.charge;
            this.poisonous = m.poisonous;
            this.cantLowerHPbelowONE = m.cantLowerHPbelowONE;

            this.silenced = m.silenced;

            // Drew: fixed
            this.cantBeTargetedBySpellsOrHeroPowers = m.cantBeTargetedBySpellsOrHeroPowers;
        }

        public int getRealAttack()
        {
            return this.Angr;
        }

        public float getMinionDefenseValue() 
        {
            return (this.Angr != 0) ? (float)Math.Sqrt(this.Angr) * this.Hp : 0.5f * this.Hp;
        }

        public float getMinionAttackValue()
        {
            return (this.Angr != 0) ? this.Angr * (float)Math.Sqrt(this.Hp) : 0.5f * (float)Math.Sqrt(this.Hp);
        }

        public void getDamageOrHeal(int dmg, Playfield p, bool isMinionAttack, bool dontCalcLostDmg)
        {
            if (this.Hp <= 0) return;

            if (this.immune && dmg > 0) return;

            if (this.isHero)
            {
                int copy = this.Hp;
                if (dmg < 0 || this.armor <= 0)
                {
                    //if (dmg < 0) return;

                    //heal

                    this.Hp = Math.Min(30, this.Hp - dmg);
                    if (copy < this.Hp)
                    {
                        p.tempTrigger.charsGotHealed++;
                    }
                    if (copy - this.Hp >= 1)
                    {
                        p.secretTrigger_HeroGotDmg(this.own, copy - this.Hp);
                    }
                }
                else
                {
                    if (this.armor > 0 && dmg > 0)
                    {

                        int rest = this.armor - dmg;
                        if (rest < 0)
                        {
                            this.Hp += rest;
                            p.secretTrigger_HeroGotDmg(this.own, rest);
                        }
                        this.armor = Math.Max(0, this.armor - dmg);

                    }
                }
                if (this.cantLowerHPbelowONE && this.Hp <= 0) this.Hp = 1;


                if (this.Hp < copy)
                {
                    this.anzGotDmg++;
                }
                return;
            }

            //its a Minion--------------------------------------------------------------


            int damage = dmg;
            int heal = 0;
            if (dmg < 0) heal = -dmg;

            bool woundedbefore = this.wounded;
            if (heal < 0) // heal was shifted in damage
            {
                damage = -1 * heal;
                heal = 0;
            }

            if (damage >= 1) this.allreadyAttacked = true;

            if (damage >= 1 && this.divineshild)
            {
                this.divineshild = false;
                if (!own && !dontCalcLostDmg && p.turnCounter == 0)
                {
                    if (isMinionAttack)
                    {
                        p.lostDamage += damage - 1;
                    }
                    else
                    {
                        p.lostDamage += (damage - 1) * (damage - 1);
                    }
                }
                return;
            }

            if (this.cantLowerHPbelowONE && damage >= 1 && damage >= this.Hp) damage = this.Hp - 1;

            if (!own && !dontCalcLostDmg && this.Hp < damage && p.turnCounter == 0)
            {
                if (isMinionAttack)
                {
                    p.lostDamage += (damage - this.Hp);
                }
                else
                {
                    p.lostDamage += (damage - this.Hp) * (damage - this.Hp);
                }
            }

            int hpcopy = this.Hp;

            if (damage >= 1)
            {
                this.Hp = this.Hp - damage;
            }

            if (heal >= 1)
            {
                if (own && !dontCalcLostDmg && heal <= 999 && this.Hp + heal > this.maxHp) p.lostHeal += this.Hp + heal - this.maxHp;

                this.Hp = this.Hp + Math.Min(heal, this.maxHp - this.Hp);
            }



            if (this.Hp > hpcopy)
            {
                //minionWasHealed
                p.tempTrigger.minionsGotHealed++;
                p.tempTrigger.charsGotHealed++;
            }

            if (this.Hp < hpcopy)
            {
                if (this.own)
                {
                    p.tempTrigger.ownMinionsGotDmg++;
                }
                else
                {
                    p.tempTrigger.enemyMinionsGotDmg++;
                }
                this.anzGotDmg++;
            }

            if (this.maxHp == this.Hp)
            {
                this.wounded = false;
            }
            else
            {
                this.wounded = true;
            }



            if (this.name == CardDB.cardName.lightspawn && !this.silenced)
            {
                this.Angr = this.Hp;
            }

            if (woundedbefore && !this.wounded)
            {
                this.handcard.card.sim_card.onEnrageStop(p, this);
            }

            if (!woundedbefore && this.wounded)
            {
                this.handcard.card.sim_card.onEnrageStart(p, this);
            }

            if (this.Hp <= 0)
            {
                this.minionDied(p);
            }



        }

        public void minionDied(Playfield p)
        {
            if (this.name == CardDB.cardName.stalagg)
            {
                p.stalaggDead = true;
            }
            else
            {
                if (this.name == CardDB.cardName.feugen) p.feugenDead = true;
            }

            

            if (own)
            {

                p.tempTrigger.ownMinionsDied++;
                if (this.handcard.card.race == 20)
                {
                    p.tempTrigger.ownBeastDied++;
                }
                if (this.handcard.card.race == 17)
                {
                    p.tempTrigger.ownMechanicDied++;
                }
                if (this.handcard.card.race == 14)
                {
                    p.tempTrigger.ownMurlocDied++;
                }
            }
            else
            {
                p.tempTrigger.enemyMinionsDied++;
                //if (this.taunt) p.anzEnemyTaunt--;
                if (this.handcard.card.race == 20)
                {
                    p.tempTrigger.enemyBeastDied++;
                }
                if (this.handcard.card.race == 17)
                {
                    p.tempTrigger.enemyMechanicDied++;
                }
                if (this.handcard.card.race == 14)
                {
                    p.tempTrigger.enemyMurlocDied++;
                }
            }

            if (p.diedMinions != null)
            {
                GraveYardItem gyi = new GraveYardItem(this.handcard.card.cardIDenum, this.entitiyID, this.own);
                p.diedMinions.Add(gyi);
                p.graveYard.Add(gyi);
            }

            //move trigger
            int maxMana = Math.Max(p.getCurrentPlayer(true).ownMaxMana, p.getCurrentPlayer(false).ownMaxMana);
            if (p.isOwnTurn == this.own)
            {
                p.moveTrigger.ownMinionDied += 1;
                float minionTurnVal = this.getTurnValue(maxMana);
                p.moveTrigger.moveReward -= minionTurnVal;
            }
            else
            {
                p.moveTrigger.enemyMinionDied += 1;
                float minionTurnVal = this.getTurnValue(maxMana);
                p.moveTrigger.moveReward += minionTurnVal;

                if (this.taunt) p.moveTrigger.tauntChanged = true;
            }
            if (this.handcard.card.name == CardDB.cardName.mechwarper && p.isOwnTurn == this.own)
            {
                p.moveTrigger.manaChanged = true; 
            }
            p.moveTrigger.minionDied = true;
            p.moveTrigger.minionDiedList.Add(this.entitiyID);
        }

        public void updateReadyness()
        {
            Ready = false;
            //default test (minion must be unfrozen!)
            if (isHero)
            {
                if (!frozen && ((charge >= 1 && playedThisTurn) || !playedThisTurn) && (numAttacksThisTurn == 0 || (numAttacksThisTurn == 1 && windfury))) Ready = true;
                return;
            }

            if (!silenced && (name == CardDB.cardName.ragnarosthefirelord || name == CardDB.cardName.ancientwatcher)) return;

            if (!frozen && ((charge >= 1 && playedThisTurn) || !playedThisTurn || shadowmadnessed) && (numAttacksThisTurn == 0 || (numAttacksThisTurn == 1 && windfury) || ( !silenced && this.name == CardDB.cardName.v07tr0n && numAttacksThisTurn <=3 )) ) Ready = true;

        }

        public void becomeSilence(Playfield p)
        {

            if (own) p.playerFirst.spellpower -= spellpower;
            else
            {
                p.playerSecond.spellpower -= spellpower;
                //if (this.taunt) p.anzEnemyTaunt--;
            }
            spellpower = 0;

            p.minionGetOrEraseAllAreaBuffs(this, false);
            //buffs
            ancestralspirit = 0;
            destroyOnOwnTurnStart = false;
            destroyOnEnemyTurnStart = false;
            destroyOnOwnTurnEnd = false;
            destroyOnEnemyTurnEnd = false;
            concedal = false;
            souloftheforest = 0;
            ownBlessingOfWisdom = 0;
            enemyBlessingOfWisdom = 0;

            cantBeTargetedBySpellsOrHeroPowers = false;

            charge = 0;
            taunt = false;
            divineshild = false;
            windfury = false;
            frozen = false;
            stealth = false;
            immune = false;
            poisonous = false;
            cantLowerHPbelowONE = false;
            

            //delete enrage (if minion is silenced the first time)
            if (wounded && handcard.card.Enrage && !silenced)
            {
                handcard.card.sim_card.onEnrageStop(p, this);
            }

            //reset attack
            Angr = handcard.card.Attack;
            tempAttack = 0;//we dont toutch the adjacent buffs!


            //reset hp and heal it
            if (maxHp < handcard.card.Health)//minion has lower maxHp as his card -> heal his hp
            {
                Hp += handcard.card.Health - maxHp; //heal minion
            }
            maxHp = handcard.card.Health;
            if (Hp > maxHp) Hp = maxHp;

            if (!silenced)//minion WAS not silenced, deactivate his aura
            {
                handcard.card.sim_card.onAuraEnds(p, this);
            }

            silenced = true;
            this.updateReadyness();
            p.minionGetOrEraseAllAreaBuffs(this, true);
            if (own)
            {
                p.tempTrigger.ownMinionsChanged = true;
            }
            else
            {
                p.tempTrigger.enemyMininsChanged = true;
            }
            if (this.shadowmadnessed)
            {
                this.shadowmadnessed = false;
                p.shadowmadnessed--;
                p.minionGetControlled(this, !own, false);
            }
        }

        public float getOffMinionValue()
        {
            float minionValue = (this.Angr != 0) ? (float)Math.Sqrt(this.Angr) * this.Hp : 0.5f * this.Hp;
            if (this.silenced) return minionValue;

            if (this.handcard.card.deathrattle)
                minionValue+= 0.5f;

            if (CardDB.Instance.priorityDatabase.ContainsKey(this.name)) minionValue += (float)(20 + CardDB.Instance.priorityDatabase[this.name]) / 20;

            //if (this.divineshild || this.stealth || this.taunt || this.handcard.card.deathrattle || this.handcard.card.isSpecialMinion)
            if (this.divineshild || this.stealth || this.handcard.card.deathrattle || this.handcard.card.isSpecialMinion)
            {
                minionValue += 1;
            }
            return minionValue;
        }

        public float getDefMinionValue()
        {
            float minionValue = (this.Angr != 0) ? this.Angr * (float)Math.Sqrt(this.Hp) : 0.25f * (float)Math.Sqrt(this.Hp);
            if (this.silenced) return minionValue;

            switch (this.name)
            {
                case CardDB.cardName.archmageantonidas:
                    minionValue *= 1.2f;
                    break;
            }

            if (this.handcard.card.deathrattle)
                minionValue+= 0.5f;

            if (CardDB.Instance.priorityDatabase.ContainsKey(this.name)) minionValue += (float)(20 + CardDB.Instance.priorityDatabase[this.name]) / 20;

            //if (this.divineshild || this.stealth || this.taunt || this.handcard.card.deathrattle || this.handcard.card.isSpecialMinion)
            if (this.divineshild || this.stealth || this.handcard.card.deathrattle || this.handcard.card.isSpecialMinion)
            {
                minionValue++;
            }

            return minionValue;
        }

        public void loadEnchantments(List<miniEnch> enchants, int ownPlayerControler)
        {
            foreach (miniEnch me in enchants)
            {
                // deathrattles reborns and destoyings----------------------------------------------
                if (me.CARDID == CardDB.cardIDEnum.CS2_038e) //ancestral spirit
                {
                    this.ancestralspirit++;
                }
                if (me.CARDID == CardDB.cardIDEnum.EX1_584e) //ancient mage
                {
                    this.spellpower++;
                }
                if (me.CARDID == CardDB.cardIDEnum.GVG_010b) //Velen's Chosen (+2+4, +spellpower)
                {
                    this.spellpower++;
                }
                if (me.CARDID == CardDB.cardIDEnum.EX1_158e) //soul of the forest
                {
                    this.souloftheforest++;
                }

                if (me.CARDID == CardDB.cardIDEnum.EX1_128e) //conceal
                {
                    this.concedal = true;
                }
                if (me.CARDID == CardDB.cardIDEnum.PART_004e) //conceal
                {
                    this.concedal = true;
                }

                if (me.CARDID == CardDB.cardIDEnum.CS2_063e) //corruption
                {
                    if (me.controllerOfCreator == ownPlayerControler)
                    {
                        this.destroyOnOwnTurnStart = true;
                    }
                    else
                    {
                        this.destroyOnEnemyTurnStart = true;
                    }
                }

                if (me.CARDID == CardDB.cardIDEnum.EX1_363e || me.CARDID == CardDB.cardIDEnum.EX1_363e2) //corruption
                {
                    if (me.controllerOfCreator == ownPlayerControler)
                    {
                        this.ownBlessingOfWisdom++;
                    }
                    else
                    {
                        this.enemyBlessingOfWisdom++;
                    }
                }

                if (me.CARDID == CardDB.cardIDEnum.DREAM_05e) //nightmare
                {
                    if (me.controllerOfCreator == ownPlayerControler)
                    {
                        this.destroyOnOwnTurnStart = true;
                    }
                    else
                    {
                        this.destroyOnEnemyTurnStart = true;
                    }
                }

                if (me.CARDID == CardDB.cardIDEnum.EX1_316e) //overwhelmingpower
                {
                    if (me.controllerOfCreator == ownPlayerControler)
                    {
                        this.destroyOnOwnTurnEnd = true;
                    }
                    else
                    {
                        this.destroyOnEnemyTurnEnd = true;
                    }
                }

                if (me.CARDID == CardDB.cardIDEnum.NEW1_036e) //commanding shout
                {
                    this.cantLowerHPbelowONE = true;
                }
                if (me.CARDID == CardDB.cardIDEnum.NEW1_036e2) //commanding shout
                {
                    this.cantLowerHPbelowONE = true;
                }

                if (me.CARDID == CardDB.cardIDEnum.EX1_334e) //Dark Command
                {
                    this.shadowmadnessed = true;
                }

                if (me.CARDID == CardDB.cardIDEnum.FP1_030e) //Necrotic Aura
                {
                    //todo Eure Zauber kosten in diesem Zug (5) mehr.
                }
                if (me.CARDID == CardDB.cardIDEnum.NEW1_029t) //death to millhouse!
                {
                    // todo spells cost (0) this turn!
                }
                if (me.CARDID == CardDB.cardIDEnum.EX1_612o) //Power of the Kirin Tor
                {
                    // todo Your next Secret costs (0).
                }
                if (me.CARDID == CardDB.cardIDEnum.EX1_084e) //warsongcommander
                {
                    this.charge++;
                }
                if (me.CARDID == CardDB.cardIDEnum.DS1_178e) //rhino
                {
                    this.charge++;
                }
                if (me.CARDID == CardDB.cardIDEnum.CS2_103e2)// sturmangriff    +2 angriff und ansturm/.
                {
                    this.charge++;
                }

                //ancientbuffs-------------------------------------------------
                if (me.CARDID == CardDB.cardIDEnum.EX1_565o) //flametongue
                {
                    this.AdjacentAngr += 2;
                }
                if (me.CARDID == CardDB.cardIDEnum.EX1_162o) //dire wolf alpha
                {
                    this.AdjacentAngr += 1;
                }
                //tempbuffs-------------------------------------------------

                if (me.CARDID == CardDB.cardIDEnum.CS2_105e) //heldenhafter stoss
                {
                    this.tempAttack += 4;
                }
                if (me.CARDID == CardDB.cardIDEnum.EX1_570e) //bite
                {
                    this.tempAttack += 4;
                }
                if (me.CARDID == CardDB.cardIDEnum.CS2_083e) //sharpened
                {
                    this.tempAttack += 1;
                }
                if (me.CARDID == CardDB.cardIDEnum.EX1_046e) //tempered
                {
                    this.tempAttack += 2;
                }
                if (me.CARDID == CardDB.cardIDEnum.CS2_188o) //inspiring
                {
                    this.tempAttack += 2;
                }
                if (me.CARDID == CardDB.cardIDEnum.CS2_045e) //rockbiter
                {
                    this.tempAttack += 3;
                }
                if (me.CARDID == CardDB.cardIDEnum.CS2_046e) //bloodlust
                {
                    this.tempAttack += 3;
                }

                if (me.CARDID == CardDB.cardIDEnum.CS2_011o) //Savage Roar
                {
                    this.tempAttack += 2;
                }
                if (me.CARDID == CardDB.cardIDEnum.CS2_017o) //Claws
                {
                    this.tempAttack += 1;
                }
                if (me.CARDID == CardDB.cardIDEnum.EX1_549o) //bestial wrath
                {
                    this.tempAttack += 2;
                    this.immune = true;
                }
                if (me.CARDID == CardDB.cardIDEnum.CS2_005o) //Claw
                {
                    this.tempAttack += 2;
                }
                if (me.CARDID == CardDB.cardIDEnum.GVG_011a) //Shrink Ray
                {
                    this.tempAttack -= 2; //todo might not be correct
                }
                if (me.CARDID == CardDB.cardIDEnum.GVG_057a) //Seal of Light
                {
                    this.tempAttack += 2;
                }




            }
        }

        public float getTurnValue(int mana)
        {
            return (float)this.handcard.card.cost / mana * 0.25f;
        }

        public int getHpValue()
        { 
            /* hp_idx = np.select(
                [hp <= 1,
                 1 < hp <= 2,
                 2 < hp <= 4,
                 4 < hp <= 6,
                 6 < hp],
                [0, 1, 2, 3, 4]).astype(int).tolist()
            */
            if (this.Hp <= 1) return 0;
            if (this.Hp <= 2) return 1;
            if (this.Hp <= 4) return 2;
            if (this.Hp <= 6) return 3;
            return 4;
        }

        public int getHeroHpValue()
        {
            /* 
                        ownHeroHp <= 6,
                        6 < ownHeroHp <= 12,
                        12 < ownHeroHp <= 15,
                        15 < ownHeroHp <= 20,
                        20 < ownHeroHp
            */
            if (this.Hp <= 6) return 0;
            if (this.Hp <= 12) return 1;
            if (this.Hp <= 15) return 2;
            if (this.Hp <= 20) return 3;
            return 4;
        }

    }

}