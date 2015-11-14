using System;
using System.Collections.Generic;
using System.Text;

namespace HRSim
{
    class Sim_GVG_004 : SimTemplate //* Goblin Blastmage
    {

        //    Battlecry: If you have a Mech, deal 4 damage randomly split among all enemies.

        public override void  getBattlecryEffect(Playfield p, Minion own, Minion target, int choice)
        {
            // optimistic
            bool ownplay = own.own;
            List<Minion> temp1 = (ownplay) ? p.playerFirst.ownMinions : p.playerSecond.ownMinions;
            bool haveAMech = false;
            foreach (Minion m in temp1)
            {
                if ((TAG_RACE)m.handcard.card.race == TAG_RACE.MECHANICAL)
				{
					haveAMech = true;
					break;
				}
            }
            if (!haveAMech) return;

            int i = 0;
            List<Minion> temp = (ownplay) ? p.playerSecond.ownMinions : p.playerFirst.ownMinions;
            int times = 4;

            if ((ownplay && p.playerSecond.ownHero.Hp <= times) || (!ownplay && p.playerFirst.ownHero.Hp <= times))
            {
                if (ownplay) p.minionGetDamageOrHeal(p.playerSecond.ownHero, p.playerSecond.ownHero.Hp - 1);
                else p.minionGetDamageOrHeal(p.playerFirst.ownHero, p.playerFirst.ownHero.Hp - 1);
            }
            else
            {
                while (i < times)
                {
                    if (temp.Count >= 1)
                    {
                        //search Minion with lowest hp
                        Minion enemy = temp[0];
                        int minhp = 10000;
                        bool found = false;
                        foreach (Minion m in temp)
                        {
                            if (m.name == CardDB.cardName.nerubianegg && enemy.Hp >= 2) continue; //dont attack nerubianegg!

                            if (m.Hp >= 2 && minhp > m.Hp)
                            {
                                enemy = m;
                                minhp = m.Hp;
                                found = true;
                            }
                        }

                        if (found)
                        {
                            p.minionGetDamageOrHeal(enemy, 1);
                        }
                        else
                        {
                            p.minionGetDamageOrHeal(ownplay ? p.playerSecond.ownHero : p.playerFirst.ownHero, 1);
                        }

                    }
                    else
                    {
                        p.minionGetDamageOrHeal(ownplay ? p.playerSecond.ownHero : p.playerFirst.ownHero, 1);
                    }

                    i++;
                }
            }
        }


    }

}