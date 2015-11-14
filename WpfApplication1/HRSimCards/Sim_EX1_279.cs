using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_279 : SimTemplate //pyroblast
	{

//    verursacht $10 schaden.

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int dmg = p.getSpellDamageDamage(10, ownplay);
            p.minionGetDamageOrHeal(target, dmg);
		}

	}
}