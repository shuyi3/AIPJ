using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_238 : SimTemplate //lightningbolt
	{

//    verursacht $3 schaden. überladung:/ (1)

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int dmg = p.getSpellDamageDamage(3, ownplay);
            p.minionGetDamageOrHeal(target, dmg);
            if (ownplay) p.ueberladung++;
		}

	}
}