using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_011 : SimTemplate //voodoodoctor
	{

//    kampfschrei:/ stellt 2 leben wieder her.
		public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
		{
            int heal = p.getMinionHeal(2, own.own);
            p.minionGetDamageOrHeal(target, -heal);
		}


	}
}