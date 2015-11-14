using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_CS2_114 : SimTemplate //cleave
	{

//    fügt zwei zufälligen feindlichen dienern $2 schaden zu.

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            //TODO delete new list
            int damage = p.getSpellDamageDamage(2, ownplay);
            List<Minion> temp2 = (ownplay) ? new List<Minion>(p.playerSecond.ownMinions) : new List<Minion>(p.playerFirst.ownMinions) ;
            temp2.Sort((a, b) => a.Hp.CompareTo(b.Hp));
            int i = 0;
            foreach (Minion enemy in temp2)
            {
                p.minionGetDamageOrHeal(enemy, damage);
                i++;
                if (i == 2) break;
            }


		}

	}
}