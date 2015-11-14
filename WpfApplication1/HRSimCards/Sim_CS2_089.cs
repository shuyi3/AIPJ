using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_CS2_089 : SimTemplate //holylight
	{

//    stellt #6 leben wieder her.
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int heal = p.getSpellHeal(6, ownplay);
            p.minionGetDamageOrHeal(target, -heal);
		}

	}
}