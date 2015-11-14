using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_400 : SimTemplate //whirlwind
	{

//    fügt allen dienern $1 schaden zu.

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int dmg = p.getSpellDamageDamage(1, ownplay);
            p.allMinionsGetDamage(dmg);
		}

	}
}