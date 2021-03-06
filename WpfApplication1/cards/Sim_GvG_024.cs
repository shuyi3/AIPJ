using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
    class Sim_GVG_024 : SimTemplate //* Cogmaster's Wrench
    {

        //    Has +2 Attack while you have a Mech.

        CardDB.Card w = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.GVG_024);
        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            p.equipWeapon(w, ownplay);

            List<Minion> temp = (ownplay) ? p.ownMinions : p.enemyMinions;
            bool hasmech = false;
            foreach (Minion m in temp)
            {
                //if we have allready a mechanical, we are buffed
                if ((TAG_RACE)m.handcard.card.race == TAG_RACE.MECHANICAL) hasmech=true;
            }
            if (hasmech)
            {
                if (ownplay)
                {
                    p.ownWeaponAttack += 2;
                    p.minionGetBuffed(p.ownHero, 2, 0);
                }
                else
                {
                    p.enemyWeaponAttack += 2;
                    p.minionGetBuffed(p.enemyHero, 2, 0);
                }
            }


        }

        public override void onMinionIsSummoned(Playfield p, Minion triggerEffectMinion, Minion summonedMinion)
        {
            if ((TAG_RACE)summonedMinion.handcard.card.race == TAG_RACE.MECHANICAL)
            {
                List<Minion> temp = (triggerEffectMinion.own) ? p.ownMinions : p.enemyMinions;

                foreach (Minion m in temp)
                {
                    //if we have allready a mechanical, we are buffed
                    if ((TAG_RACE)m.handcard.card.race == TAG_RACE.MECHANICAL) return; 
                }

                //we had no mechanical, but now!
                if (triggerEffectMinion.own)
                {
                    p.ownWeaponAttack += 2;
                    p.minionGetBuffed(p.ownHero, 2, 0);
                }
                else
                {
                    p.enemyWeaponAttack += 2;
                    p.minionGetBuffed(p.enemyHero, 2, 0);
                }
            }
        }
		
		public override void onMinionDiedTrigger(Playfield p, Minion m, Minion diedMinion)
        {
			int diedMinions = (m.own)? p.tempTrigger.ownMechanicDied : p.tempTrigger.enemyMechanicDied;
            if (diedMinions >= 1)
			{
				List<Minion> temp = (m.own) ? p.ownMinions : p.enemyMinions;
				bool hasmechanics = false;
                foreach (Minion mTmp in temp) //check if we have more mechanics, or debuff him
                {
                    if (mTmp.Hp >=1 && (TAG_RACE)mTmp.handcard.card.race == TAG_RACE.MECHANICAL) hasmechanics = true;
                }
				
                if (!hasmechanics)
                {
					if(m.own)
					{
						p.ownWeaponAttack -= 2;
						p.minionGetBuffed(p.ownHero, -2, 0);
					}
					else
					{
                        p.enemyWeaponAttack -= 2;
                        p.minionGetBuffed(p.enemyHero, -2, 0);
					}
                }
            }
        }
    }
}