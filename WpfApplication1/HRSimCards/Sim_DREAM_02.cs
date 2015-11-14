using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_DREAM_02 : SimTemplate //yseraawakens
	{

//    fügt allen charakteren mit ausnahme von ysera $5 schaden zu.

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            int dmg = p.getSpellDamageDamage(5, ownplay);
            foreach (Minion m in p.playerFirst.ownMinions)
            {
                if (m.name != CardDB.cardName.ysera) p.minionGetDamageOrHeal(m, dmg);
            }
            foreach (Minion m in p.playerSecond.ownMinions)
            {
                if (m.name != CardDB.cardName.ysera) p.minionGetDamageOrHeal(m, dmg);
            }
            p.minionGetDamageOrHeal(p.playerFirst.ownHero, dmg);
            p.minionGetDamageOrHeal(p.playerSecond.ownHero, dmg);

		}

	}
}