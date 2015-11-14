using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_EX1_508 : SimTemplate//Grimscale Oracle
    {
        public override void onAuraStarts(Playfield p, Minion own)
        {
            p.playerFirst.anzGrimscaleOracle++;
            foreach (Minion m in p.playerFirst.ownMinions)
            {
                if ((TAG_RACE)m.handcard.card.race == TAG_RACE.MURLOC && own.entitiyID != m.entitiyID) p.minionGetBuffed(m, 1, 0);
            }
            foreach (Minion m in p.playerSecond.ownMinions)
            {
                if ((TAG_RACE)m.handcard.card.race == TAG_RACE.MURLOC && own.entitiyID != m.entitiyID) p.minionGetBuffed(m, 1, 0);
            }
        }

        public override void onAuraEnds(Playfield p, Minion m)
        {
            p.playerFirst.anzGrimscaleOracle--;
            foreach (Minion mn in p.playerFirst.ownMinions)
            {
                if ((TAG_RACE)mn.handcard.card.race == TAG_RACE.MURLOC && mn.entitiyID != m.entitiyID) p.minionGetBuffed(m, -1, 0);
            }
            foreach (Minion mn in p.playerSecond.ownMinions)
            {
                if ((TAG_RACE)mn.handcard.card.race == TAG_RACE.MURLOC && mn.entitiyID != m.entitiyID) p.minionGetBuffed(m, -1, 0);
            }
        }
    }
}
