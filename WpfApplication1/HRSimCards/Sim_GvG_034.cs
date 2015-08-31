using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_034 : SimTemplate //* Mech-Bear-Cat
    {
        //    Whenever this minion takes damage, add a Spare Part card to your hand.
        public override void onMinionGotDmgTrigger(Playfield p, Minion m, bool ownDmgdmin)
        {
            if (m.anzGotDmg>=1)
            {
                for (int i = 0; i < m.anzGotDmg; i++)
                {
                    p.drawACard(CardDB.cardName.armorplating, m.own, true);
                }
                m.anzGotDmg = 0;
            }
        }
    }

}