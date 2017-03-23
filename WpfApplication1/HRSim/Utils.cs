using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Python.Runtime;
using Newtonsoft.Json;

namespace HRSim
{
    public class Feature
    {
        public string name;
        public dynamic data;
        public Feature(string name, dynamic emptyData)
        {
            this.name = name;
            data = emptyData;
        }

        public PyList getPyList() 
        {
            PyList ret = new PyList();
            foreach (int e in data)
            {
                PythonUtils.AppendRecycle(ret, new PyInt(e));
            }
            return ret;
        }

        public void print()
        {
            Console.WriteLine(this.name);
            Console.WriteLine("[{0}]", string.Join(", ", data));
        }
    }

    public class StateFeature
    {
        public int numFeatures;
        public Feature[] featrueArray;
        //public int[] globalFeature;
        //public int[] ownMinionHpFeature;
        //public int[] ownMinionFeature;
        //public int[] ownCardFeature;
        //public int[] ownPlayedFeature;
        //public int[][] ownPlayableFeature;
        //public int[] enemyMinionHpFeature;
        //public int[] enemyMinionFeature;
        //public int[] enemyCardFeature;

        public StateFeature(int numFeatures) {
            this.numFeatures = numFeatures;
            featrueArray = new Feature[numFeatures];
        }

        public string[] getFeatureNames() {
            string[] ret = new string[numFeatures];
            for (int i = 0; i < numFeatures; i++) ret[i] = featrueArray[i].name;
            return ret;
        }

        public void print()
        {
            foreach (Feature ft in featrueArray)
            {
                ft.print();
            }
        }

        public PyList[] getPyData()
        {
            PyList[] ret = new PyList[numFeatures];
            int count = 0;
            foreach (Feature ft in featrueArray)
            {
                PyList ftList = ft.getPyList();
                ret[count] = ftList;
                count++;
            }
            return ret;
        }

        public PyList[] getPyFeatureData()
        {
            PyList[] ret = new PyList[9];

            PyList pyGlobal = new PyList();
            foreach (int f in featrueArray[0].data) PythonUtils.AppendRecycle(pyGlobal, new PyInt(f));

            PyList pyOwnMinionHp = new PyList();
            foreach (int f in featrueArray[1].data) PythonUtils.AppendRecycle(pyOwnMinionHp, new PyInt(f));

            PyList pyOwnMinion = new PyList();
            foreach (int f in featrueArray[2].data) PythonUtils.AppendRecycle(pyOwnMinion, new PyInt(f));

            PyList pyOwnCard = new PyList();
            foreach (int f in featrueArray[3].data) PythonUtils.AppendRecycle(pyOwnCard, new PyInt(f));

            PyList pyOwnPlayed = new PyList();
            foreach (int f in featrueArray[4].data) PythonUtils.AppendRecycle(pyOwnPlayed, new PyInt(f));

            PyList pyOwnPlayable = new PyList();
            foreach (int[] action in featrueArray[5].data)
            {
                PyList row = new PyList();
                foreach (int f in action) PythonUtils.AppendRecycle(row, new PyInt(f));
                PythonUtils.AppendRecycle(pyOwnPlayable, row);
            }

            PyList pyEnemyMinionHp = new PyList();
            foreach (int f in featrueArray[6].data) PythonUtils.AppendRecycle(pyEnemyMinionHp, new PyInt(f));

            PyList pyEnemyMinion = new PyList();
            foreach (int f in featrueArray[7].data) PythonUtils.AppendRecycle(pyEnemyMinion, new PyInt(f));

            PyList pyEnemyCard = new PyList();
            foreach (int f in featrueArray[8].data) PythonUtils.AppendRecycle(pyEnemyCard, new PyInt(f));

            ret[0] = pyGlobal;
            ret[1] = pyOwnMinionHp;
            ret[2] = pyOwnMinion;
            ret[3] = pyOwnCard;
            ret[4] = pyOwnPlayed;
            ret[5] = pyOwnPlayable;
            ret[6] = pyEnemyMinionHp;
            ret[7] = pyEnemyMinion;
            ret[8] = pyEnemyCard;

            return ret;
        }
    }

    public static class Featurization
    {
        public static string[] minionCardArray = {"damagedgolem", "clockworkgnome", "boombot", "manawyrm", "cogmaster", "annoyotron", "mechwarper",
               "snowchugger", "harvestgolem", "spidertank", "tinkertowntechnician", "mechanicalyeti",
               "goblinblastmage", "loatheb", "archmageantonidas", "drboom", "unknown"};

        public static string[] spellCardArray = { "thecoin", "armorplating", "fireblast", "frostbolt", "fireball", "flamestrike" };

        public static string[] cardArray = {"damagedgolem", "clockworkgnome", "boombot", "manawyrm", "cogmaster", "annoyotron", "mechwarper",
               "snowchugger", "harvestgolem", "spidertank", "tinkertowntechnician", "mechanicalyeti",
               "goblinblastmage", "loatheb", "archmageantonidas", "drboom", "unknown",
                "thecoin", "armorplating", "fireblast", "frostbolt", "fireball", "flamestrike"};

        public static Dictionary<string, int> cardStrIdxDict = new Dictionary<string, int>()
                                                               {
                                                                {"damagedgolem", 0},                                                              
                                                                {"clockworkgnome", 1},
                                                                {"boombot", 2},
                                                                {"manawyrm", 3},
                                                                {"cogmaster", 4},
                                                                {"annoyotron", 5},
                                                                {"mechwarper", 6},
                                                                {"snowchugger", 7},
                                                                {"harvestgolem", 8},
                                                                {"spidertank", 9},
                                                                {"tinkertowntechnician", 10},
                                                                {"mechanicalyeti", 11},
                                                                {"goblinblastmage", 12},
                                                                {"loatheb", 13},
                                                                {"archmageantonidas", 14},
                                                                {"drboom", 15},
                                                                {"unknown", 16},
                                                                {"thecoin", 17},
                                                                {"armorplating", 18},
                                                                {"fireblast", 19},
                                                                {"frostbolt", 20},
                                                                {"fireball", 21},
                                                                {"flamestrike", 22}
                                                               };

        public static Dictionary<CardDB.cardName, int> cardIdxDict = new Dictionary<CardDB.cardName, int>()
                                                               {
                                                                {CardDB.cardName.damagedgolem, 0},                                                              
                                                                {CardDB.cardName.clockworkgnome, 1},
                                                                {CardDB.cardName.boombot, 2},
                                                                {CardDB.cardName.manawyrm, 3},
                                                                {CardDB.cardName.cogmaster, 4},
                                                                {CardDB.cardName.annoyotron, 5},
                                                                {CardDB.cardName.mechwarper, 6},
                                                                {CardDB.cardName.snowchugger, 7},
                                                                {CardDB.cardName.harvestgolem, 8},
                                                                {CardDB.cardName.spidertank, 9},
                                                                {CardDB.cardName.tinkertowntechnician, 10},
                                                                {CardDB.cardName.mechanicalyeti, 11},
                                                                {CardDB.cardName.goblinblastmage, 12},
                                                                {CardDB.cardName.loatheb, 13},
                                                                {CardDB.cardName.archmageantonidas, 14},
                                                                {CardDB.cardName.drboom, 15},
                                                                {CardDB.cardName.unknown, 16},
                                                                {CardDB.cardName.thecoin, 17},
                                                                {CardDB.cardName.armorplating, 18},
                                                                {CardDB.cardName.fireblast, 19},
                                                                {CardDB.cardName.frostbolt, 20},
                                                                {CardDB.cardName.fireball, 21},
                                                                {CardDB.cardName.flamestrike, 22}
                                                               };

        public static StateFeature normalFeaturization(StateKeyInfo stKeyInfo)
        {
            PlayerKeyInfo mPlayer = stKeyInfo.attackPlayer;
            PlayerKeyInfo ePlayer = stKeyInfo.defensePlayer;

            StateFeature ret = new StateFeature(9);

            ret.featrueArray[0] = new Feature("Global", new int[4]); //ownhp, enemyhp, ownmana, enemymana;
            ret.featrueArray[0].data[0] = mPlayer.heroInfo.Hp;
            ret.featrueArray[0].data[1] = mPlayer.maxMana;
            ret.featrueArray[0].data[2] = ePlayer.heroInfo.Hp;
            ret.featrueArray[0].data[3] = ePlayer.maxMana;

            ret.featrueArray[1] = new Feature("OwnMinionHp", new int[minionCardArray.Length]);
            ret.featrueArray[2] = new Feature("OwnMinion", new int[minionCardArray.Length]);

            foreach (PlayerKeyInfo.MinionKeyInfo m in mPlayer.minionJsonList)
            {
                int idx = cardStrIdxDict[m.name];
                ret.featrueArray[1].data[idx] = Math.Max(ret.featrueArray[1].data[idx], m.Hp);
                ret.featrueArray[2].data[idx]++;
            }

            ret.featrueArray[3] = new Feature("OwnCardFeature", new int[cardArray.Length]);
            foreach (PlayerKeyInfo.CardKeyInfo hc in mPlayer.handcardJsonList)
            {
                int idx = cardStrIdxDict[hc.cardName];
                ret.featrueArray[3].data[idx]++;
            }

            ret.featrueArray[4] = new Feature("OwnPlayedFeature", new int[10]);
            ret.featrueArray[5] = new Feature("OwnPlayableFeature", new int[10][]);

            for (int i = 0; i < 10; i++) ret.featrueArray[5].data[i] = new int[cardArray.Length];
            List<List<PlayerKeyInfo.CardKeyInfo>> playableList = new List<List<PlayerKeyInfo.CardKeyInfo>>();
            playableList.Add(mPlayer.handcardJsonList);
            if (mPlayer.handcardChange != null) playableList.AddRange(mPlayer.handcardChange);
            int playedCount = 0;
            int playedCardCount = 0;
            foreach (PlayerKeyInfo.ActionKeyInfo actionKeyInfo in mPlayer.playedActionJsonList)
            {
                if (actionKeyInfo.actionType == actionEnum.playcard || actionKeyInfo.actionType == actionEnum.useHeroPower)
                {                    
                    List<PlayerKeyInfo.CardKeyInfo> playable = playableList[playedCount];
                    foreach (PlayerKeyInfo.CardKeyInfo hc in playable)
                    {
                        if (hc.playable)
                        {
                            ret.featrueArray[5].data[playedCount][cardStrIdxDict[hc.cardName]] = 1;
                        }
                        if (mPlayer.canPlayHeroPower[playedCount] == 1)
                        {
                            ret.featrueArray[5].data[playedCount][cardStrIdxDict["fireblast"]] = 1;
                        }
                    }
                    int playedActionIdx;
                    if (actionKeyInfo.actionType == actionEnum.playcard)
                    {
                        playedActionIdx = cardStrIdxDict[mPlayer.playedCardJsonList[playedCardCount].cardName];
                        playedCardCount++;
                    }
                    else
                    {
                        playedActionIdx = cardStrIdxDict["fireblast"];
                    }
                    ret.featrueArray[4].data[playedCount] = playedActionIdx;
                    playedCount++;
                }
            }

            ret.featrueArray[6] = new Feature("EnemyMinionHp", new int[minionCardArray.Length]);
            ret.featrueArray[7] = new Feature("EnemyMinion", new int[minionCardArray.Length]);

            foreach (PlayerKeyInfo.MinionKeyInfo m in ePlayer.minionJsonList)
            {
                int idx = cardStrIdxDict[m.name];
                ret.featrueArray[6].data[idx]++;
                ret.featrueArray[7].data[idx] = Math.Max(ret.featrueArray[7].data[idx], m.Hp);
            }

            ret.featrueArray[8] = new Feature("EnemyCardFeature", new int[cardArray.Length]);

            foreach (PlayerKeyInfo.CardKeyInfo hc in ePlayer.handcardJsonList)
            {
                int idx = cardStrIdxDict[hc.cardName];
                ret.featrueArray[8].data[idx]++;
            }

            return ret;
        }

        public static double getNonspellMinionInteraction(Playfield pf, Handmanager.Handcard hc, List<Minion> minions)
        {
            double ret = 0;
            if (minions.Count == 0) return 0;
            foreach (Minion m in minions)
            {
                //worst we can do
                int ownHp = hc.card.Health;
                int ownAttack = hc.card.Attack;
                int ownResult = ownHp - m.Angr;
                int enemyResult = m.Hp - ownAttack;
                int result = 0;
                if (ownResult <= 0)
                {
                    result = (enemyResult > 0) ? 0 : 1;
                }
                else
                {
                    result = (enemyResult > 0) ? 1 : 2;
                }
                ret += result * m.getDefMinionValue();
            }
            return ret;
        }

        public static int calcScaledEffect(double effect, double totalEffect)
        {
            int e = (int)(effect / totalEffect * 10);
            if (e <= 1) return e+1;
            if (e <= 3) return 3;
            if (e <= 6) return 4;
            return 5;
        }

        public static double getHeroPowerMinionInteraction(Playfield pf, List<Minion> minions) {
            double ret = 0;
            if (minions.Count == 0) return ret;
            foreach (Minion m in minions)
            {
                if (1 >= m.Hp)
                {
                    ret = Math.Max(ret, 2 * m.getDefMinionValue() * minions.Count);
                }
            }
            return ret;
        }

        public static double getSpellMinionInteraction(Playfield pf, Handmanager.Handcard hc, List<Minion> minions)
        {
            double ret = 0;
            if (minions.Count == 0) return ret;
            if (CardDB.Instance.DamageTargetDatabase.ContainsKey(hc.card.name))
            { //target
                int damage = CardDB.Instance.DamageTargetDatabase[hc.card.name];
                foreach (Minion m in minions)
                {
                    //best can do
                    if (damage >= m.Hp)
                    {
                        ret = Math.Max(ret, 2 * m.getDefMinionValue() * minions.Count);
                    }
                }
            }
            else if (CardDB.Instance.DamageAllDatabase.ContainsKey(hc.card.name))
            { //aoe
                int aoeDamage = CardDB.Instance.DamageTargetDatabase[hc.card.name];
                foreach (Minion m in minions)
                {
                    if (aoeDamage >= m.Hp)
                    {
                        ret += 2 * m.getDefMinionValue();
                    }
                }
            }
            else //buff
            {
                //int hpBuff = 1; //TODO: remove hardcode
                //ret = 0;
            }

            return ret;
        }

        public static StateFeature interactionFeaturization(Playfield pf)
        {
            Player mPlayer = pf.getCurrentPlayer(true);
            Player ePlayer = pf.getCurrentPlayer(false);

            StateFeature ret = new StateFeature(6);
            int globalFeatureSize = 6;
            ret.featrueArray[0] = new Feature("Global", new int[globalFeatureSize]); //ownhp, enemyhp, ownmana, enemymana;
            ret.featrueArray[0].data[0] = mPlayer.ownHero.Hp;
            ret.featrueArray[0].data[1] = mPlayer.ownMaxMana;
            ret.featrueArray[0].data[2] = ePlayer.ownHero.Hp;
            ret.featrueArray[0].data[3] = ePlayer.ownMaxMana;

            int enemyBoardHp = 0;
            int ownBoardAttack = 0;
            foreach (Minion m in ePlayer.ownMinions)
            {
                enemyBoardHp += m.Hp;
            }

            foreach (Minion m in mPlayer.ownMinions)
            {
                ownBoardAttack += m.Angr;
            }
            int atkAdv = enemyBoardHp <= ownBoardAttack ? 1 : 0;
            int numAdv = mPlayer.ownMinions.Count >= ePlayer.ownMinions.Count ? 1 : 0;
            ret.featrueArray[0].data[4] = atkAdv;
            ret.featrueArray[0].data[5] = numAdv;

            ret.featrueArray[1] = new Feature("OwnCardFeature", new int[cardArray.Length]);
            ret.featrueArray[2] = new Feature("OwnPlayableFeature", new int[cardArray.Length]);
            ret.featrueArray[3] = new Feature("OwnPlayedFeature", new int[cardArray.Length]);
            ret.featrueArray[4] = new Feature("EffectFeature", new int[cardArray.Length]);
            ret.featrueArray[5] = new Feature("CanPlayAfter", new int[cardArray.Length]);

            double totalEffect = 0;
            foreach (Minion m in ePlayer.ownMinions)
            {
                totalEffect += 2 * m.getDefMinionValue();
            }

            if (mPlayer.ownAbilityReady && mPlayer.mana >= 2)
            {
                int idx = cardIdxDict[CardDB.cardName.fireblast];
                ret.featrueArray[2].data[idx] = 1;

                double effect = getHeroPowerMinionInteraction(pf, ePlayer.ownMinions);
                int scaledEffect = (effect != 0) ? calcScaledEffect(effect, totalEffect) : 0;
                ret.featrueArray[4].data[idx] = scaledEffect;
            }

            foreach (Handmanager.Handcard hc in mPlayer.owncards)
            {
                int idx = cardIdxDict[hc.card.name]; 
                ret.featrueArray[1].data[idx]++;
                if (ret.featrueArray[1].data[idx] > 6) {
                    int debug = 1;
                }
                if (hc.canplayCard(pf, pf.isOwnTurn)){
                    ret.featrueArray[2].data[idx] = 1;

                    if (hc.card.type == CardDB.cardtype.SPELL)
                    {
                        double effect = getSpellMinionInteraction(pf, hc, ePlayer.ownMinions);
                        int scaledEffect = (effect != 0) ? calcScaledEffect(effect, totalEffect) : 0;
                        ret.featrueArray[4].data[idx] = scaledEffect;
                    }
                    else
                    {
                        double effect = getNonspellMinionInteraction(pf, hc, ePlayer.ownMinions);
                        int scaledEffect = (effect != 0) ? calcScaledEffect(effect, totalEffect) : 0;
                        ret.featrueArray[4].data[idx] = scaledEffect;
                    }
                }           
            }

            foreach (Handmanager.Handcard hc in mPlayer.owncards){         
                int idx = cardIdxDict[hc.card.name]; 
                if (ret.featrueArray[2].data[idx] == 1){
                    //mana after play
                    int temp = mPlayer.mana;
                    int manaAfterPlay = mPlayer.mana - hc.getManaCost(pf, pf.isOwnTurn);
                    mPlayer.mana = manaAfterPlay;
                    int canPlayAfter = 0;
                    foreach (Handmanager.Handcard hc1 in mPlayer.owncards)
                    {
                        if (hc1.canplayCard(pf, pf.isOwnTurn)){
                            int idx1 = cardIdxDict[hc1.card.name]; 
                            canPlayAfter = Math.Max(canPlayAfter, ret.featrueArray[4].data[idx1]);
                        }
                    }
                    mPlayer.mana = temp;
                    ret.featrueArray[5].data[idx] = canPlayAfter;
                }
            }

            foreach (Action action in mPlayer.playactions)
            {
                if (action.actionType == actionEnum.playcard)
                {
                    int idx = cardIdxDict[action.card.card.name]; 
                    ret.featrueArray[3].data[idx]++;
                }
                else if (action.actionType == actionEnum.useHeroPower)
                {
                    int idx = cardIdxDict[CardDB.cardName.fireblast]; 
                    ret.featrueArray[3].data[idx]++;
                }
            }

            return ret;
        }

    }


    public static class PythonUtils
    {
        public static void AppendRecycle(PyList list, PyObject obj)
        {
            list.Append(obj);
            obj.Dispose();
        }
    }

    public static class MathUtils
    {
        public static void SoftMax(List<double> probList)
        {
            if (probList.Count == 0) return;

            double cMax = Double.MinValue;
            foreach (double v in probList)
            {
                cMax = Math.Max(v, cMax);
            }

            double[] expArray = new double[probList.Count];
            for (int i = 0; i < probList.Count; i++)
            {
                probList[i] = Math.Exp(probList[i] - cMax);
            }

            double expSum = 0.0;
            foreach (double v in probList)
            {
                expSum += v;
            }

            for (int i = 0; i < probList.Count; i++)
            {
                probList[i] /= expSum;
            }
        }
    }
}
