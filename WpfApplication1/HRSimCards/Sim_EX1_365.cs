using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_365 : SimTemplate //holywrath
	{
        // todo ask the posibility manager!
//    zieht eine karte und verursacht schaden, der ihren kosten entspricht.
		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay);

            int dmg = p.getSpellDamageDamage(3, ownplay);
            p.minionGetDamageOrHeal(target, dmg);
		}

	}
}