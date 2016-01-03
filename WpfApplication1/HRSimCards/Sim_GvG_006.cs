using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_006 : SimTemplate //Mechwarper
    {

        //    Your Mechs cost (1) less.

        public override void onAuraStarts(Playfield p, Minion own)
        {
            if (own.own)
            {
                p.playerFirst.anzOwnMechwarper++;
                if (p.isOwnTurn) 
                {
                    p.moveTrigger.manaChanged = true;
                }
            }
            else
            {
                p.playerSecond.anzOwnMechwarper++;
                if (!p.isOwnTurn)
                {
                    p.moveTrigger.manaChanged = true;
                }
            }

        }

        public override void onAuraEnds(Playfield p, Minion own)
        {
            if (own.own)
            {
                p.playerFirst.anzOwnMechwarper--;
                if (p.isOwnTurn)
                {
                    p.moveTrigger.manaChanged = true;
                }
            }
            else
            {
                p.playerSecond.anzOwnMechwarper--;
                if (!p.isOwnTurn)
                {
                    p.moveTrigger.manaChanged = true;
                }
            }
        }


    }

}