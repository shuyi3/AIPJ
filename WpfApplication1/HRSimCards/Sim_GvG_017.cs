using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_017 : SimTemplate //* Call Pet
    {

        //    Draw a card. If it's a Beast, it costs (4) less.
        //    TODO: draw a beast

        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            p.drawACard(p.getArandomCardFromDeck(ownplay), ownplay);
        }
    }
}