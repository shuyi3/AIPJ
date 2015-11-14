using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_NEW1_008b : SimTemplate //ancientsecrets
	{

//    stellt 5 leben wieder her.
		public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
		{
            int heal = p.getMinionHeal(5, own.own);
            p.minionGetDamageOrHeal(target, -heal);
		}

	}
}