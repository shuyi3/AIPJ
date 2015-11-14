using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_094 : SimTemplate //Jeeves
    {

        //   At the end of each player's turn, that player draws until they have 3 cards.

        public override void onTurnEndsTrigger(Playfield p, Minion triggerEffectMinion, bool turnEndOfOwner)
        {
            if (triggerEffectMinion.own == turnEndOfOwner)
            {
                Player mPlayer;
                if (turnEndOfOwner)
                {
                    mPlayer = p.playerFirst;
                }
                else
                {
                    mPlayer = p.playerSecond;
                }

                int cardstodraw = 0;
                if (mPlayer.owncards.Count <= 2)
                {
                    cardstodraw = 3 - mPlayer.owncards.Count;
                }

                for (int i = 0; i < cardstodraw; i++)
                {
                    p.drawACard(p.getArandomCardFromDeck(turnEndOfOwner), turnEndOfOwner);
                }
            }
        }
    }
}