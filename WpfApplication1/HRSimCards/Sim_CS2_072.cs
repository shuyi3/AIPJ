using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_CS2_072 : SimTemplate //backstab
	{

//    fügt einem unverletzten diener $2 schaden zu.
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int dmg = p.getSpellDamageDamage(2, ownplay);
            p.minionGetDamageOrHeal(target, dmg);
		}

	}
}