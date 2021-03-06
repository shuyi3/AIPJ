using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
	class Sim_EX1_557 : SimTemplate //natpagle
	{

//    zu beginn eures zuges besteht eine chance von 50%, dass ihr eine zusätzliche karte zieht.
        public override void onTurnStartTrigger(Playfield p, Minion triggerEffectMinion, bool turnStartOfOwner)
        {
            if (triggerEffectMinion.own == turnStartOfOwner)
            {
                if (GameManager.getRNG().NextDouble() < 0.5)
                    p.drawACard(p.getArandomCardFromDeck(turnStartOfOwner), turnStartOfOwner);
            }
        }

	}
}