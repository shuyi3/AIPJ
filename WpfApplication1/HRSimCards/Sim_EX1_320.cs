using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_320 : SimTemplate //baneofdoom
	{

//    fügt einem charakter $2 schaden zu. beschwört einen zufälligen dämon, wenn der schaden tödlich ist.
        CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.CS2_059);//bloodimp
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{


            int dmg = p.getSpellDamageDamage(2, ownplay);

            bool summondemon = false;

            if (!target.isHero && dmg >= target.Hp && !target.divineshild && !target.immune)
            {
                summondemon = true;
            }

            p.minionGetDamageOrHeal(target, dmg);

            if (summondemon)
            {
                int posi = (ownplay) ? p.playerFirst.ownMinions.Count : p.playerSecond.ownMinions.Count;
                
                p.callKid(kid, posi, ownplay);
            }

		}

	}
}