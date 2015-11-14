using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_BRM_011 : SimTemplate //* Lava Shock
	{
		// Deal 2 damage. Unlock your Overloaded Mana Crystals.
		
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int dmg = p.getSpellDamageDamage(2, ownplay);
            p.minionGetDamageOrHeal(target, dmg);
			if (ownplay) p.ueberladung = 0;
		}
	}
}