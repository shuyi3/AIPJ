using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_DS1_055 : SimTemplate //darkscalehealer
	{

//    kampfschrei:/ stellt bei allen befreundeten charakteren 2 leben wieder her.
		public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
		{
            int heal = p.getMinionHeal(2, own.own);
            p.allCharsOfASideGetDamage(own.own, -heal);
		}


	}
}