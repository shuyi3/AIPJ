using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
{
    class Sim_GVG_082 : SimTemplate //Clockwork Gnome
    {

        //   Deathrattle: Add a Spare Part card to your hand.

        public override void onDeathrattle(Playfield p, Minion m)
        {
            p.drawACard(CardDB.cardName.armorplating, m.own, true);
        }

    }

}