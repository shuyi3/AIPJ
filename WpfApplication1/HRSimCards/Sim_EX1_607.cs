using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_607 : SimTemplate //innerrage
	{

//    fügt einem diener $1 schaden zu. der diener erhält +2 angriff.

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int dmg = p.getSpellDamageDamage(1, ownplay);
            p.minionGetDamageOrHeal(target, dmg);
            p.minionGetTempBuff(target, 2,0);
		}

	}
}