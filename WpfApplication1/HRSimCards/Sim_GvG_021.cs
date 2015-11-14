using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_021 : SimTemplate //Mal'Ganis
    {

        //    Your other Demons have +2/+2.Your hero is Immune;

        public override void onAuraStarts(Playfield p, Minion own)
        {
            if (own.own)
            {
                p.playerFirst.anzOwnMalGanis++;
                p.playerFirst.ownHero.immune = true;
                foreach (Minion m in p.playerFirst.ownMinions)
                {
                    if (own.entitiyID != m.entitiyID && (TAG_RACE)m.handcard.card.race == TAG_RACE.DEMON) p.minionGetBuffed(m, 2, 2);
                }
            }
            else
            {
                p.playerSecond.anzOwnMalGanis++;
                p.playerSecond.ownHero.immune = true;
                foreach (Minion m in p.playerSecond.ownMinions)
                {
                    if (own.entitiyID != m.entitiyID && (TAG_RACE)m.handcard.card.race == TAG_RACE.DEMON) p.minionGetBuffed(m, 2, 2);
                }
            }

        }

        public override void onAuraEnds(Playfield p, Minion own)
        {
            if (own.own)
            {
                p.playerFirst.anzOwnMalGanis--;
                p.playerFirst.ownHero.immune = false;
                foreach (Minion m in p.playerFirst.ownMinions)
                {
                    if (own.entitiyID != m.entitiyID && (TAG_RACE)m.handcard.card.race == TAG_RACE.DEMON) p.minionGetBuffed(m, -2, -2);
                }
            }
            else
            {
                p.playerSecond.anzOwnMalGanis--;
                p.playerSecond.ownHero.immune = false;
                foreach (Minion m in p.playerSecond.ownMinions)
                {
                    if (own.entitiyID != m.entitiyID && (TAG_RACE)m.handcard.card.race == TAG_RACE.DEMON) p.minionGetBuffed(m, -2, -2);
                }
            }
        }


    }

}