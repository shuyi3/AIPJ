using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_CS2_007 : SimTemplate //healingtouch
	{

//    stellt #8 leben wieder her.

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int heal = p.getSpellHeal(8, ownplay);
            p.minionGetDamageOrHeal(target, -heal);
            
		}

	}
}