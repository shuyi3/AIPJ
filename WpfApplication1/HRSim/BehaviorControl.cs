using System.Collections.Generic;
using System;

namespace HRSim
{

    public class BehaviorControl : Behavior
    {
        //PenalityManager penman;

        public BehaviorControl(){
            //penman = penman;
        }

        public override List<int> getPlayfieldFeature(Playfield p, bool own)
        {
            List<int> featureList = new List<int>();

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

            int hpboarder = 10;
            if (mPlayer.ownHeroName == HeroEnum.warlock && ePlayer.ownHeroName != HeroEnum.mage) hpboarder = 6;
            int aggroboarder = 11;

            if (p.isOwnTurn != own)
            {
                featureList.Add(-p.lastTurnEvaluatePenality);
            }
            else
            {
                featureList.Add(-p.evaluatePenality);
            }

            featureList.Add(mPlayer.owncards.Count * 5);

            featureList.Add(mPlayer.ownMaxMana);
            featureList.Add(-ePlayer.ownMaxMana);

            featureList.Add(mPlayer.ownMaxMana * 20 - ePlayer.ownMaxMana * 20);

            featureList.Add(-2 * ePlayer.spellpower);

            int hpFeature = mPlayer.ownHero.Hp + mPlayer.ownHero.armor > hpboarder ? 1 : 0;

            if (mPlayer.ownHero.Hp + mPlayer.ownHero.armor > hpboarder)
            {
                featureList.Add(mPlayer.ownHero.Hp + mPlayer.ownHero.armor);
            }
            else
            {
                featureList.Add(-2 * (hpboarder + 1 - mPlayer.ownHero.Hp - mPlayer.ownHero.armor) * (hpboarder + 1 - mPlayer.ownHero.Hp - mPlayer.ownHero.armor));
            }


            if (ePlayer.ownHero.Hp + ePlayer.ownHero.armor > aggroboarder)
            {
                featureList.Add(-ePlayer.ownHero.Hp - ePlayer.ownHero.armor);
            }
            else
            {
                featureList.Add(3 * (aggroboarder + 1 - ePlayer.ownHero.Hp - ePlayer.ownHero.armor));
            }

            //if (mPlayer.ownWeaponAttack >= 1)
            //{
            //    featureList.Add(mPlayer.ownWeaponAttack * mPlayer.ownWeaponDurability);
            //}

            //if (!ePlayer.ownHero.frozen)
            //{
            //    featureList.Add(ePlayer.ownWeaponDurability * ePlayer.ownWeaponAttack);
            //}
            //else
            //{
            //    if (ePlayer.ownWeaponDurability >= 1)
            //    {
            //        featureList.Add(12);
            //    }
            //}

            //RR card draw value depending on the turn and distance to lethal
            //RR if lethal is close, carddraw value is increased
            if (ePlayer.ownHero.armor + ePlayer.ownHero.Hp <= 10) //RR should be updated by lethal check
            {
                featureList.Add(mPlayer.owncarddraw * 100);
            }
            else
            {
                featureList.Add(0);
            }

            if (mPlayer.ownMaxMana < 4)
            {
                featureList.Add(mPlayer.owncarddraw * 2);
            }
            else
            {
                featureList.Add(mPlayer.owncarddraw * 5);
            }

            //retval += p.owncarddraw * 5;
            featureList.Add(-ePlayer.owncarddraw * 15);

            //int owntaunt = 0;
            int readycount = 0;
            int ownMinionsCount = 0;
            int ownMinionVal = 0;
            foreach (Minion m in mPlayer.ownMinions)
            {
                ownMinionVal += 5;
                ownMinionVal += m.Hp * 2;
                ownMinionVal += m.Angr * 2;
                ownMinionVal += m.handcard.card.rarity;
                if (!m.playedThisTurn && m.windfury) ownMinionVal += m.Angr;
                if (m.divineshild) ownMinionVal += 1;
                if (m.stealth) ownMinionVal += 1;
                if (m.handcard.card.isSpecialMinion)
                {
                    ownMinionVal += 1;
                    if (!m.taunt && m.stealth) ownMinionVal += 20;
                }
                else
                {
                    if (m.Angr <= 2 && m.Hp <= 2 && !m.divineshild) ownMinionVal -= 5;
                }
                //if (!m.taunt && m.stealth && penman.specialMinions.ContainsKey(m.name)) retval += 20;
                //if (m.poisonous) retval += 1;
                if (m.divineshild && m.taunt) ownMinionVal += 4;
                //if (m.taunt && m.handcard.card.name == CardDB.cardName.frog) owntaunt++;
                //if (m.handcard.card.isToken && m.Angr <= 2 && m.Hp <= 2) ownMinionVal -= 5;
                //if (!penman.specialMinions.ContainsKey(m.name) && m.Angr <= 2 && m.Hp <= 2) ownMinionVal -= 5;
                if (m.handcard.card.name == CardDB.cardName.direwolfalpha || m.handcard.card.name == CardDB.cardName.flametonguetotem || m.handcard.card.name == CardDB.cardName.stormwindchampion || m.handcard.card.name == CardDB.cardName.raidleader) ownMinionVal += 10;
                if (m.handcard.card.name == CardDB.cardName.bloodmagethalnos) ownMinionVal += 10;
                if (m.handcard.card.name == CardDB.cardName.nerubianegg)
                {
                    if (m.Angr >= 1) ownMinionVal += 2;
                    if ((!m.taunt && m.Angr == 0) && (m.divineshild || m.maxHp > 2)) ownMinionVal -= 10;
                }
                if (m.Ready) readycount++;
                if (m.Hp <= 4 && (m.Angr > 2 || m.Hp > 3)) ownMinionsCount++;
            }
            featureList.Add(ownMinionVal);

            /*if (ePlayer.ownMinions.Count >= 0)
            {
                int anz = ePlayer.ownMinions.Count;
                if (owntaunt == 0) retval -= 10 * anz;
                retval += owntaunt * 10 - 11 * anz;
            }*/


            bool useAbili = false;
            bool usecoin = false;
            List<Action> actions;

            if (p.isOwnTurn != own)
            {
                actions = mPlayer.lastTurnActions;
            }
            else
            {
                actions = mPlayer.playactions;
            }

            int actionVal = 0;
            foreach (Action a in actions)
            {
                if (a.actionType == actionEnum.attackWithHero && ePlayer.ownHero.Hp <= 15) actionVal++; //attackfacehp = 15
                if (a.actionType == actionEnum.useHeroPower) useAbili = true;
                if (mPlayer.ownHeroName == HeroEnum.warrior && a.actionType == actionEnum.attackWithHero && useAbili) actionVal -= 1;
                //if (a.actionType == actionEnum.useHeroPower && a.card.card.name == CardDB.cardName.lesserheal && (!a.target.own)) actionVal -= 5;
                if (a.actionType != actionEnum.playcard) continue;
                if ((a.card.card.name == CardDB.cardName.thecoin || a.card.card.name == CardDB.cardName.innervate)) usecoin = true;
                //save spell for all classes: (except for rouge if he has no combo)
                if (a.target == null) continue;
                if (mPlayer.ownHeroName != HeroEnum.thief && a.card.card.type == CardDB.cardtype.SPELL && (!a.target.own && a.target.isHero) && a.card.card.name != CardDB.cardName.shieldblock) actionVal -= 11;
                if (mPlayer.ownHeroName == HeroEnum.thief && a.card.card.type == CardDB.cardtype.SPELL && (a.target.isHero && !a.target.own)) actionVal -= 11;
            }
            if (usecoin && useAbili && mPlayer.ownMaxMana <= 2) actionVal -= 40;
            if (usecoin) actionVal -= 5 * mPlayer.manaTurnEnd;
            if (mPlayer.manaTurnEnd >= 2 && !useAbili)
            {
                actionVal -= 10;
                if (mPlayer.ownHeroName == HeroEnum.thief && (mPlayer.ownWeaponDurability >= 2 || mPlayer.ownWeaponAttack >= 2)) actionVal += 10;
            }
            //if (usecoin && p.mana >= 1) actionVal -= 20;
            featureList.Add(actionVal);

            int handMobFeature = 0;
            int mobsInHand = 0;
            int bigMobsInHand = 0;
            foreach (Handmanager.Handcard hc in mPlayer.owncards)
            {
                if (hc.card.type == CardDB.cardtype.MOB)
                {
                    mobsInHand++;
                    if (hc.card.Attack >= 3) bigMobsInHand++;
                }
            }

            if (ownMinionsCount - ePlayer.ownMinions.Count >= 4 && bigMobsInHand >= 1)
            {
                handMobFeature += bigMobsInHand * 25;
            }
            featureList.Add(handMobFeature);

            int enemyMinionFeature = 0;
            //bool hasTank = false;
            foreach (Minion m in ePlayer.ownMinions)
            {
                enemyMinionFeature += this.getEnemyMinionValue(m, p);
                //hasTank = hasTank || m.taunt;
            }
            featureList.Add(-enemyMinionFeature);

            int otherVal = 0;
            otherVal -= ePlayer.ownSecretsIDList.Count;
            otherVal -= p.lostDamage;//damage which was to high (like killing a 2/1 with an 3/3 -> => lostdamage =2
            otherVal -= p.lostWeaponDamage;

            //if (p.ownMinions.Count == 0) retval -= 20;
            //if (ePlayer.ownMinions.Count == 0) retval += 20;
            //if (ePlayer.ownHero.Hp <= 0) otherVal = 10000;
            //soulfire etc
            int deletecardsAtLast = 0;
            foreach (Action a in actions)
            {
                if (a.actionType != actionEnum.playcard) continue;
                if (a.card.card.name == CardDB.cardName.soulfire || a.card.card.name == CardDB.cardName.doomguard || a.card.card.name == CardDB.cardName.succubus) deletecardsAtLast = 1;
                if (deletecardsAtLast == 1 && !(a.card.card.name == CardDB.cardName.soulfire || a.card.card.name == CardDB.cardName.doomguard || a.card.card.name == CardDB.cardName.succubus)) otherVal -= 20;
            }
            if (ePlayer.ownHero.Hp >= 1 && p.guessingHeroHP <= 0)
            {
                if (p.turnCounter < 2) otherVal += mPlayer.owncarddraw * 100;
                otherVal -= 1000;
            }
            //if (mPlayer.ownHero.Hp <= 0) retval = -10000;
            featureList.Add(otherVal);

            return featureList;
        }

        public List<double> getMacroFeature(Playfield p, bool own)
        {
            bool afterTurnEnd = false;
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

            List<double> featureList = new List<double>();

            featureList.Add(p.getOwnBoardValue(afterTurnEnd));
            featureList.Add(p.getEnemyBoardValue(afterTurnEnd));
            featureList.Add(p.getOwnHandValue(afterTurnEnd));
            featureList.Add(p.getEnemyHandValue(afterTurnEnd));

            return featureList;

        }

        public override float getPlayfieldValue(Playfield p, bool own)
        {
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

            if (p.value >= -2000000) return sigmoid(p.value, mPlayer.ownMaxMana);
            int retval = 0;
            int hpboarder = 10;
            if (mPlayer.ownHeroName == HeroEnum.warlock && ePlayer.ownHeroName != HeroEnum.mage) hpboarder = 6;
            int aggroboarder = 11;

            if (p.isOwnTurn != own)
            {
                retval -= p.lastTurnEvaluatePenality;
            }
            else
            {
                retval -= p.evaluatePenality;
            }

            retval -= p.evaluatePenality;
            retval += mPlayer.owncards.Count * 5;

            retval += mPlayer.ownMaxMana;
            retval -= ePlayer.ownMaxMana;

            retval += mPlayer.ownMaxMana * 20 - ePlayer.ownMaxMana * 20;

            if (ePlayer.ownHeroName == HeroEnum.mage || ePlayer.ownHeroName == HeroEnum.druid) retval -= 2 * ePlayer.spellpower;

            if (mPlayer.ownHero.Hp + mPlayer.ownHero.armor > hpboarder)
            {
                retval += mPlayer.ownHero.Hp + mPlayer.ownHero.armor;
            }
            else
            {
                retval -= 2 * (hpboarder + 1 - mPlayer.ownHero.Hp - mPlayer.ownHero.armor) * (hpboarder + 1 - mPlayer.ownHero.Hp - mPlayer.ownHero.armor);
            }


            if (ePlayer.ownHero.Hp + ePlayer.ownHero.armor > aggroboarder)
            {
                retval += -ePlayer.ownHero.Hp - ePlayer.ownHero.armor;
            }
            else
            {
                retval += 3 * (aggroboarder + 1 - ePlayer.ownHero.Hp - ePlayer.ownHero.armor);
            }

            if (mPlayer.ownWeaponAttack >= 1)
            {
                retval += mPlayer.ownWeaponAttack * mPlayer.ownWeaponDurability;
            }

            if (!ePlayer.ownHero.frozen)
            {
                retval -= ePlayer.ownWeaponDurability * ePlayer.ownWeaponAttack;
            }
            else
            {
                if (ePlayer.ownWeaponDurability >= 1)
                {
                    retval += 12;
                }
            }

            //RR card draw value depending on the turn and distance to lethal
            //RR if lethal is close, carddraw value is increased
            if (ePlayer.ownHero.armor + ePlayer.ownHero.Hp <= 10) //RR should be updated by lethal check
            {
                retval += mPlayer.owncarddraw * 100;
            }
            if (mPlayer.ownMaxMana < 4)
            {
                retval += mPlayer.owncarddraw * 2;
            }
            else
            {
                retval += mPlayer.owncarddraw * 5;
            }

            //retval += p.owncarddraw * 5;
            retval -= ePlayer.owncarddraw * 15;

            //int owntaunt = 0;
            int readycount = 0;
            int ownMinionsCount = 0;
            foreach (Minion m in mPlayer.ownMinions)
            {
                retval += 5;
                retval += m.Hp * 2;
                retval += m.Angr * 2;
                retval += m.handcard.card.rarity;
                if (!m.playedThisTurn && m.windfury) retval += m.Angr;
                if (m.divineshild) retval += 1;
                if (m.stealth) retval += 1;
                if (m.handcard.card.isSpecialMinion)
                {
                    retval += 1;
                    if (!m.taunt && m.stealth) retval += 20;
                }
                else
                {
                    if (m.Angr <= 2 && m.Hp <= 2 && !m.divineshild) retval -= 5;
                }
                //if (!m.taunt && m.stealth && penman.specialMinions.ContainsKey(m.name)) retval += 20;
                //if (m.poisonous) retval += 1;
                if (m.divineshild && m.taunt) retval += 4;
                //if (m.taunt && m.handcard.card.name == CardDB.cardName.frog) owntaunt++;
                //if (m.handcard.card.isToken && m.Angr <= 2 && m.Hp <= 2) retval -= 5;
                //if (!penman.specialMinions.ContainsKey(m.name) && m.Angr <= 2 && m.Hp <= 2) retval -= 5;
                if (m.handcard.card.name == CardDB.cardName.direwolfalpha || m.handcard.card.name == CardDB.cardName.flametonguetotem || m.handcard.card.name == CardDB.cardName.stormwindchampion || m.handcard.card.name == CardDB.cardName.raidleader) retval += 10;
                if (m.handcard.card.name == CardDB.cardName.bloodmagethalnos) retval += 10;
                if (m.handcard.card.name == CardDB.cardName.nerubianegg)
                {
                    if (m.Angr >= 1) retval += 2;
                    if ((!m.taunt && m.Angr == 0) && (m.divineshild || m.maxHp > 2)) retval -= 10;
                }
                if (m.Ready) readycount++;
                if (m.Hp <= 4 && (m.Angr > 2 || m.Hp > 3)) ownMinionsCount++;
            }

            /*if (ePlayer.ownMinions.Count >= 0)
            {
                int anz = ePlayer.ownMinions.Count;
                if (owntaunt == 0) retval -= 10 * anz;
                retval += owntaunt * 10 - 11 * anz;
            }*/


            bool useAbili = false;
            bool usecoin = false;
            List<Action> actions;

            if (p.isOwnTurn != own)
            {
                actions = mPlayer.lastTurnActions;
            }
            else
            {
                actions = mPlayer.playactions;
            }

            foreach (Action a in actions)
            {
                if (a.actionType == actionEnum.attackWithHero && ePlayer.ownHero.Hp <= 15) retval++; //attackfacehp = 15
                if (a.actionType == actionEnum.useHeroPower) useAbili = true;
                if (mPlayer.ownHeroName == HeroEnum.warrior && a.actionType == actionEnum.attackWithHero && useAbili) retval -= 1;
                //if (a.actionType == actionEnum.useHeroPower && a.card.card.name == CardDB.cardName.lesserheal && (!a.target.own)) retval -= 5;
                if (a.actionType != actionEnum.playcard) continue;
                if ((a.card.card.name == CardDB.cardName.thecoin || a.card.card.name == CardDB.cardName.innervate)) usecoin = true;
                //save spell for all classes: (except for rouge if he has no combo)
                if (a.target == null) continue;
                if (mPlayer.ownHeroName != HeroEnum.thief && a.card.card.type == CardDB.cardtype.SPELL && (!a.target.own && a.target.isHero) && a.card.card.name != CardDB.cardName.shieldblock) retval -= 11;
                if (mPlayer.ownHeroName == HeroEnum.thief && a.card.card.type == CardDB.cardtype.SPELL && (a.target.isHero && !a.target.own)) retval -= 11;
            }
            if (usecoin && useAbili && mPlayer.ownMaxMana <= 2) retval -= 40;
            if (usecoin) retval -= 5 * mPlayer.manaTurnEnd;
            if (mPlayer.manaTurnEnd >= 2 && !useAbili)
            {
                retval -= 10;
                if (mPlayer.ownHeroName == HeroEnum.thief && (mPlayer.ownWeaponDurability >= 2 || mPlayer.ownWeaponAttack >= 2)) retval += 10;
            }
            //if (usecoin && p.mana >= 1) retval -= 20;

            int mobsInHand = 0;
            int bigMobsInHand = 0;
            foreach (Handmanager.Handcard hc in mPlayer.owncards)
            {
                if (hc.card.type == CardDB.cardtype.MOB)
                {
                    mobsInHand++;
                    if (hc.card.Attack >= 3) bigMobsInHand++;
                }
            }

            if (ownMinionsCount - ePlayer.ownMinions.Count >= 4 && bigMobsInHand >= 1)
            {
                retval += bigMobsInHand * 25;
            }


            //bool hasTank = false;
            foreach (Minion m in ePlayer.ownMinions)
            {
                retval -= this.getEnemyMinionValue(m, p);
                //hasTank = hasTank || m.taunt;
            }

            /*foreach (SecretItem si in ePlayer.ownSecretList)
            {
                if (readycount >= 1 && !hasTank && si.canbeTriggeredWithAttackingHero)
                {
                    retval -= 100;
                }
                if (readycount >= 1 && ePlayer.ownMinions.Count >= 1 && si.canbeTriggeredWithAttackingMinion)
                {
                    retval -= 100;
                }
                if (si.canbeTriggeredWithPlayingMinion && mobsInHand >= 1)
                {
                    retval -= 25;
                }
            }*/

            retval -= ePlayer.ownSecretsIDList.Count;
            retval -= p.lostDamage;//damage which was to high (like killing a 2/1 with an 3/3 -> => lostdamage =2
            retval -= p.lostWeaponDamage;

            //if (p.ownMinions.Count == 0) retval -= 20;
            //if (ePlayer.ownMinions.Count == 0) retval += 20;
            if (ePlayer.ownHero.Hp <= 0) retval = 10000;
            //soulfire etc
            int deletecardsAtLast = 0;
            foreach (Action a in actions)
            {
                if (a.actionType != actionEnum.playcard) continue;
                if (a.card.card.name == CardDB.cardName.soulfire || a.card.card.name == CardDB.cardName.doomguard || a.card.card.name == CardDB.cardName.succubus) deletecardsAtLast = 1;
                if (deletecardsAtLast == 1 && !(a.card.card.name == CardDB.cardName.soulfire || a.card.card.name == CardDB.cardName.doomguard || a.card.card.name == CardDB.cardName.succubus)) retval -= 20;
            }
            if (ePlayer.ownHero.Hp >= 1 && p.guessingHeroHP <= 0)
            {
                if (p.turnCounter < 2) retval += mPlayer.owncarddraw * 100;
                retval -= 1000;
            }
            if (mPlayer.ownHero.Hp <= 0) retval = -10000;

            p.value = retval;
            float val = sigmoid(retval, mPlayer.ownMaxMana);
            //Console.WriteLine("val:" + val);
            if (val < 0)
            {
                int debug = 1;
            }
            return val;
        }

        public float sigmoid(float value, int maxMana)
        { 
            double k;
            if (maxMana < 4)
            {
                k = -0.02;
            }
            else if (maxMana < 7)
            {
                k = -0.015;
            }
            else
            {
                k = -0.01;
            }

            float squashedVal = 1.0f / (float)(1.0 + Math.Exp(k * value));
            return squashedVal;
        }

        public override int getEnemyMinionValue(Minion m, Playfield p)
        {
            int retval = 5;
            retval += m.Hp * 2;
            if (!m.frozen && !((m.name == CardDB.cardName.ancientwatcher || m.name == CardDB.cardName.ragnarosthefirelord) && !m.silenced))
            {
                retval += m.Angr * 2;
                if (m.windfury) retval += m.Angr * 2;
                if (m.Angr >= 4) retval += 10;
                if (m.Angr >= 7) retval += 50;
            }

            if (m.Angr == 0) retval -= 7;

            retval += m.handcard.card.rarity;
            if (m.taunt) retval += 5;
            if (m.divineshild) retval += m.Angr;
            if (m.divineshild && m.taunt) retval += 5;
            if (m.stealth) retval += 1;

            if (m.poisonous) retval += 4;

            if (m.handcard.card.targetPriority >= 1 && !m.silenced)
            {
                retval += m.handcard.card.targetPriority;
            }
            if (m.name == CardDB.cardName.nerubianegg && m.Angr <= 3 && !m.taunt) retval = 0;
            return retval;
        }

    }

}