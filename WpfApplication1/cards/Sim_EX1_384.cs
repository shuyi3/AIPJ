using System;
using System.Collections.Generic;
using System.Text;

namespace HREngine.Bots
{
    class Sim_EX1_384 : SimTemplate //avengingwrath
    {

        //    verursacht $8 schaden, der zufällig auf feindliche charaktere verteilt wird.
        //todo for enemy
        public override void onCardPlay(Playfield p, bool ownplay, Minion target, int choice)
        {
            // optimistic

            int i = 0;
            List<Minion> temp = (ownplay) ? p.enemyMinions : p.ownMinions;
            int times = (ownplay) ? p.getSpellDamageDamage(8) : p.getEnemySpellDamageDamage(8);

            if ((ownplay && p.enemyHero.Hp <= times) || (!ownplay && p.ownHero.Hp <= times))
            {
                if (ownplay) p.minionGetDamageOrHeal(p.enemyHero, p.enemyHero.Hp - 1);
                else p.minionGetDamageOrHeal(p.ownHero, p.ownHero.Hp - 1);
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
                            p.minionGetDamageOrHeal(ownplay ? p.enemyHero : p.ownHero, 1);
                        }

                    }
                    else
                    {
                        p.minionGetDamageOrHeal(ownplay ? p.enemyHero : p.ownHero, 1);
                    }

                    i++;
                }
            }
        }

    }

}