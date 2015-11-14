using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_241 : SimTemplate //lavaburst
	{

//    verursacht $5 schaden. überladung:/ (2)

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            int dmg = p.getSpellDamageDamage(5, ownplay);
            p.minionGetDamageOrHeal(target, dmg);
            if (ownplay) p.ueberladung += 2;
		}

	}
}