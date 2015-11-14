using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_CS1_130 : SimTemplate //holysmite
	{

//    verursacht $2 schaden.
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int dmg = p.getSpellDamageDamage(2, ownplay);
            p.minionGetDamageOrHeal(target, dmg);
		}

	}
}