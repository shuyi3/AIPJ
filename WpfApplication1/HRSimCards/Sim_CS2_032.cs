using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_CS2_032 : SimTemplate //flamestrike
	{

//    fügt allen feindlichen dienern $4 schaden zu.
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int dmg = p.getSpellDamageDamage(4, ownplay);
            p.allMinionOfASideGetDamage(!ownplay, dmg);
		}

	}
}