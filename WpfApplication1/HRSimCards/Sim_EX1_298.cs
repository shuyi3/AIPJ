using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_EX1_298 : SimTemplate //* Ragnaros the Firelord
    {
        // Can't Attack. At the end of your turn, deal 8 damage to a random enemy.

        public override void onTurnEndsTrigger(Playfield p, Minion triggerEffectMinion, bool turnEndOfOwner)
        {
            if (triggerEffectMinion.own == turnEndOfOwner)
            {
                Minion target = new Minion();
                if (turnEndOfOwner)
                {
                    if (p.playerSecond.ownMinions.Count > 0)
                    {
                        target = p.searchRandomMinion(p.playerSecond.ownMinions, Playfield.searchmode.searchLowestHP); //damage the lowest (pessimistic variant)
                        if (target == null) {
                            target = p.searchRandomMinion(p.playerSecond.ownMinions, Playfield.searchmode.searchLowestHP); //damage the lowest (pessimistic variant)
                        }
                    }
                    else
                    {
                        target = p.playerSecond.ownHero;
                    }
                }
                else
                {
                    if (p.playerFirst.ownMinions.Count > 0)
                    {
                        target = p.searchRandomMinion(p.playerFirst.ownMinions, Playfield.searchmode.searchHighestAttack); //damage the Highest (pessimistic variant)
                        if (target == null) {
                            target = p.searchRandomMinion(p.playerFirst.ownMinions, Playfield.searchmode.searchHighestAttack); //damage the Highest (pessimistic variant)
                        }
                    }
                    else
                    {
                        target = p.playerFirst.ownHero;
                    }
                }           
                p.minionGetDamageOrHeal(target, 8);
                triggerEffectMinion.stealth = false;
                //Implementation is this right?
                //p.doDmgTriggers();
            }
        }
    }
}