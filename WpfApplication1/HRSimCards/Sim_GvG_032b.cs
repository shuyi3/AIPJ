using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_032b : SimTemplate //Grove Tender
    {

        //    Give each player a Mana Crystal.

        public override void getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
        {

            p.drawACard(p.getArandomCardFromDeck(true), true);
            p.drawACard(p.getArandomCardFromDeck(false), false);
           
        }


    }

}