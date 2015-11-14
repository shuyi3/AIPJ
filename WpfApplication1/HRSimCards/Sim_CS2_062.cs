using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_CS2_062 : SimTemplate //hellfire
	{

//    fügt allen charakteren $3 schaden zu.
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int dmg = p.getSpellDamageDamage(3, ownplay);
            p.allCharsGetDamage(dmg);
		}

	}
}