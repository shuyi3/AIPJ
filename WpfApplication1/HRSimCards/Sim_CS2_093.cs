using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_CS2_093 : SimTemplate //consecration
	{

//    fügt allen feinden $2 schaden zu.
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int dmg = p.getSpellDamageDamage(2, ownplay);
            p.allCharsOfASideGetDamage(!ownplay, dmg);
		}

	}
}