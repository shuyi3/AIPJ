using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_EX1_341 : SimTemplate//lightwell
    {

        // <deDE>Stellt zu Beginn Eures Zuges bei einem verletzten befreundeten Charakter 3 Leben wieder her.</deDE>
        public override void onTurnStartTrigger(Playfield p, Minion triggerEffectMinion, bool turnStartOfOwner)
        {
            if (turnStartOfOwner == triggerEffectMinion.own)
            {
                int heal = p.getMinionHeal(3, turnStartOfOwner);
                List<Minion> temp = (turnStartOfOwner) ? p.playerFirst.ownMinions : p.playerSecond.ownMinions;
                if (temp.Count >= 1)
                {
                    bool healed = false;
                    foreach (Minion mins in temp)
                    {
                        if (mins.wounded)
                        {
                            p.minionGetDamageOrHeal(mins, -heal);
                            healed = true;
                            break;
                        }
                    }

                    if (!healed)
                    {
                        p.minionGetDamageOrHeal(turnStartOfOwner ? p.playerFirst.ownHero : p.playerSecond.ownHero, -heal);
                    }
                }
                else
                {
                    p.minionGetDamageOrHeal(turnStartOfOwner ? p.playerFirst.ownHero : p.playerSecond.ownHero, -heal);
                }

            }
        }
    }
}
