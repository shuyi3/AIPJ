using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_DS1_185 : SimTemplate //arcaneshot
	{

//    verursacht $2 schaden.
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int dmg = p.getSpellDamageDamage(2, ownplay);
            p.minionGetDamageOrHeal(target, dmg);
		}

	}
}