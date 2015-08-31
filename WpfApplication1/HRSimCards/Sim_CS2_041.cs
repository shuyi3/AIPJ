using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_CS2_041 : SimTemplate //* ancestralhealing
	{

//    Restore a minion to full Health and give it Taunt.
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            target.taunt = true;
            p.minionGetDamageOrHeal(target, -1000);
		}

	}
}