using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_308 : SimTemplate //soulfire
	{

//    verursacht $4 schaden. werft eine zufällige karte ab.

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int dmg = p.getSpellDamageDamage(4, ownplay);
            p.minionGetDamageOrHeal(target, dmg);
            if (ownplay)
            {
                p.playerFirst.owncarddraw -= Math.Min(1, p.playerFirst.owncards.Count);
                p.playerFirst.owncards.RemoveRange(0, Math.Min(1, p.playerFirst.owncards.Count));
                p.triggerCardsChanged(true);
            }
            else
            {
                p.playerSecond.owncarddraw -= Math.Min(1, p.playerSecond.owncards.Count);
                p.playerSecond.owncards.RemoveRange(0, Math.Min(1, p.playerSecond.owncards.Count));
                p.triggerCardsChanged(true);
            }
		}

	}
}