using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_392 : SimTemplate //battlerage
	{

//    zieht eine karte für jeden verletzten befreundeten charakter.

		public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
		{
            List<Minion> temp = (ownplay)? p.playerFirst.ownMinions : p.playerSecond.ownMinions;
            foreach (Minion mnn in temp )
            {
                if (mnn.wounded)
                {
                    p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay);
                }
            }
            if (ownplay && p.playerFirst.ownHero.Hp < 30) p.drawACard(p.getArandomCardFromDeck(true), true);
            if (!ownplay && p.playerSecond.ownHero.Hp < 30) p.drawACard(p.getArandomCardFromDeck(false), false);

		}

	}
}