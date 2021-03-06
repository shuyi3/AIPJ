using System;
using System.Collections.Generic;
using System.Text;

namespace SilverfishAi
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
                    if (p.enemyMinions.Count > 0)
                    {
                        target = p.searchRandomMinion(p.enemyMinions, Playfield.searchmode.searchLowestHP); //damage the lowest (pessimistic variant)
                    }
                    else
                    {
                        target = p.enemyHero;
                    }
                }
                else
                {
                    if (p.ownMinions.Count > 0)
                    {
                        target = p.searchRandomMinion(p.ownMinions, Playfield.searchmode.searchHighestAttack); //damage the Highest (pessimistic variant)
                    }
                    else
                    {
                        target = p.ownHero;
                    }
                }
                p.minionGetDamageOrHeal(target, 8);
                triggerEffectMinion.stealth = false;
            }
        }
    }
}