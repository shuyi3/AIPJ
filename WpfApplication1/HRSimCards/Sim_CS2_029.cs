using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_CS2_029 : SimTemplate //fireball
	{

//    verursacht $6 schaden.
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int dmg = p.getSpellDamageDamage(6, ownplay);
            p.minionGetDamageOrHeal(target, dmg);
		}

	}
}