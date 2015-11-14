using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_CS2_117 : SimTemplate //earthenringfarseer
	{

//    kampfschrei:/ stellt 3 leben wieder her.
		public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
		{
            int heal =p.getMinionHeal(3, own.own);
            p.minionGetDamageOrHeal(target, -heal);
		}

	}
}