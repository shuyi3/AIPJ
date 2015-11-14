using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_Mekka2 : SimTemplate //repairbot
    {

        //    stellt am ende eures zuges bei einem verletzten charakter 6 leben wieder her.

        public override void onTurnEndsTrigger(Playfield p, Minion triggerEffectMinion, bool turnEndOfOwner)
        {
            if (turnEndOfOwner == triggerEffectMinion.own)
            {

                Minion tm = null;
                int hl = p.getMinionHeal(6, triggerEffectMinion.own);
                int heal = 0;
                foreach (Minion m in p.playerFirst.ownMinions)
                {
                    if (m.maxHp - m.Hp > heal)
                    {
                        tm = m;
                        heal = m.maxHp - m.Hp;
                    }
                }
                foreach (Minion m in p.playerSecond.ownMinions)
                {
                    if (m.maxHp - m.Hp > heal)
                    {
                        tm = m;
                        heal = m.maxHp - m.Hp;
                    }
                }
                if (heal >= 1)
                {
                    p.minionGetDamageOrHeal(tm, -hl);
                }
                else
                {
                    p.minionGetDamageOrHeal(p.playerFirst.ownHero.Hp < 30 ? p.playerFirst.ownHero : p.playerSecond.ownHero, -hl);
                }

            }
        }

    }
}