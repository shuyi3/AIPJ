using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Python.Runtime;

namespace HRSim
{


    public class FeatureConst {

        public Dictionary<int, int> heroHpDict;

        public Dictionary<int, int> countDict;

        public int heroPowerIdx;

        public string[] minionCardArray;
        public string[] spellCardArray;

        public string[] cardArray;

        public Dictionary<string, int> cardStrIdxDict;

        public Dictionary<CardDB.cardName, int> cardIdxDict;

        public PyInt[] pyIntMap;

        private static FeatureConst instance;

        private FeatureConst()
        {
            Init();
        }

        public static FeatureConst Instance
        {
            get
            {
                return instance ?? (instance = new FeatureConst());
            }
        }

        public void Init()
        {
            heroHpDict = new Dictionary<int, int>()
                                                {
                                                {4, 0},                                                              
                                                {3, 1},
                                                {2, 2},
                                                {14, 3},
                                                {13, 4},
                                                {24, 5},
                                                {12, 6},
                                                {23, 7},
                                                {34, 8},
                                                {1, 9},
                                                {44, 10},
                                                {33, 11},
                                                {22, 12},
                                                {11, 13},
                                                {0, 14},
                                                {10, 15},
                                                {43, 16},
                                                {32, 17},
                                                {21, 18},
                                                {42, 19},
                                                {31, 20},
                                                {41, 21},
                                                {20, 22},
                                                {30, 23},
                                                {40, 24}
                                                };

        countDict = new Dictionary<int, int>()
                                                {
                                                {2, 0},                                                              
                                                {1, 1},
                                                {12, 2},
                                                {0, 3},
                                                {11, 4},
                                                {22, 5},
                                                {21, 6},
                                                {10, 7},
                                                {20, 8}
                                                };

        heroPowerIdx = 19;

        minionCardArray = new string[] {"damagedgolem", "clockworkgnome", "boombot", "manawyrm", "cogmaster", "annoyotron", "mechwarper",
               "snowchugger", "harvestgolem", "spidertank", "tinkertowntechnician", "mechanicalyeti",
               "goblinblastmage", "loatheb", "archmageantonidas", "drboom", "unknown"};

        spellCardArray = new string[] { "thecoin", "armorplating", "fireblast", "frostbolt", "fireball", "flamestrike" };

        cardArray = new string[] {"damagedgolem", "clockworkgnome", "boombot", "manawyrm", "cogmaster", "annoyotron", "mechwarper",
               "snowchugger", "harvestgolem", "spidertank", "tinkertowntechnician", "mechanicalyeti",
               "goblinblastmage", "loatheb", "archmageantonidas", "drboom", "unknown",
                "thecoin", "armorplating", "fireblast", "frostbolt", "fireball", "flamestrike"};

        cardStrIdxDict = new Dictionary<string, int>()
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

        cardIdxDict = new Dictionary<CardDB.cardName, int>()
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

            pyIntMap = new PyInt[40];
            for (int i = 0; i < 40; i++)
            {
                pyIntMap[i] = new PyInt(i);
            }

        }

        
    }

    public class NumpyStateFeature
    {
        public dynamic boardFeature;
        public dynamic handFeature;
        public dynamic playFeature;
        public dynamic globalFeature;
    }

    public class BinaryFeature
    {
        public dynamic data;
        public int dim;

        public virtual void FillIdxArr(PyList idxArr, int offset)
        { 
        }

        public virtual string EncodeToString()
        {
            return null;
        }

        public virtual string EncodeToStringFlatten()
        {
            return null;
        }
    }

    public class OneDBinaryFeature: BinaryFeature
    {

        public OneDBinaryFeature(int d1)
        {
            data = new int[d1];
        }

        public override string EncodeToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] != 0)
                {
                    if (sb.Length != 0) sb.Append(","); 
                    sb.Append(i);
                }
            }

            return sb.ToString();
        }

        public override string EncodeToStringFlatten()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] != 0)
                {
                    if (sb.Length != 0) sb.Append(",");
                    sb.Append(i);
                }
            }

            return sb.ToString();
        }
    }

    public class TwoDBinaryFeature : BinaryFeature
    {
        public TwoDBinaryFeature(int d1)
        {
            data = new int[d1];
        }

        public override string EncodeToString()
        {
            StringBuilder d1Sb = new StringBuilder();
            StringBuilder d2Sb = new StringBuilder();
            
            for (int i = 0; i < data.Length; i++)
            {
                if (i != 0){
                    d1Sb.Append(",");
                    d2Sb.Append(",");
                }

                d1Sb.Append(data[i]);
                d2Sb.Append(i);
            }

            string ret = d1Sb.ToString() + "." + d2Sb.ToString();
            return ret;
        }

        public override string EncodeToStringFlatten()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                if (i != 0)
                {
                    sb.Append(",");
                }

                sb.Append(data[i] * data.Length + i);
            }

            return sb.ToString();
        }

        public override void FillIdxArr(PyList idxArr, int offset)
        {
            PyList Dim1 = new PyList();
            PyList Dim2 = new PyList();
            for (int i = 0; i < data.Length; i++)
            {
                Dim1.Append(FeatureConst.Instance.pyIntMap[data[i] + offset]);
                Dim2.Append(FeatureConst.Instance.pyIntMap[i]);
            }
            PythonUtils.AppendRecycle(idxArr, Dim1);
            PythonUtils.AppendRecycle(idxArr, Dim2);
        }
    }

    public class ThreeDBinaryFeature : BinaryFeature
    {
        public ThreeDBinaryFeature(int d1, int d2)
        {
            data = new int[d1][];
            for (int i = 0; i < d1; i++) data[i] = new int[d2];
        }

        public override string EncodeToString()
        {
            StringBuilder d1Sb = new StringBuilder();
            StringBuilder d2Sb = new StringBuilder();
            StringBuilder d3Sb = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < data[i].Length; j++)
                {
                    if (i != 0 || j != 0)
                    {
                        d1Sb.Append(",");
                        d2Sb.Append(",");
                        d3Sb.Append(",");
                    }
                    d1Sb.Append(data[i][j]);
                    d2Sb.Append(i);
                    d3Sb.Append(j);
                }
            }

            string ret = d1Sb.ToString() + "." + d2Sb.ToString() + "." + d3Sb.ToString();
            return ret;

        }

        public override string EncodeToStringFlatten()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < data[i].Length; j++)
                {
                    if (i != 0 || j != 0)
                    {
                        sb.Append(",");
                    }
                    sb.Append(data[i][j] * data.Length * data[i].Length + i * data[i].Length + j);
                }
            }

            return sb.ToString();

        }


        public override void FillIdxArr(PyList idxArr, int offset)
        {
            PyList Dim1 = new PyList();
            PyList Dim2 = new PyList();
            PyList Dim3 = new PyList();

            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < data[i].Length; j++)
                {
                    //npArray[data[i][j] + offset][i].itemset(j, 1);
                    Dim1.Append(FeatureConst.Instance.pyIntMap[data[i][j] + offset]);
                    Dim2.Append(FeatureConst.Instance.pyIntMap[i]);
                    Dim3.Append(FeatureConst.Instance.pyIntMap[j]);
                }
            }

            PythonUtils.AppendRecycle(idxArr, Dim1);
            PythonUtils.AppendRecycle(idxArr, Dim2);
            PythonUtils.AppendRecycle(idxArr, Dim3);
        }
    }

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
                ret.Append(FeatureConst.Instance.pyIntMap[e]);
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
        public StateFeature(int numFeatures)
        {
            this.numFeatures = numFeatures;
            featrueArray = new Feature[numFeatures];
        }

        public string[] getFeatureNames()
        {
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
            foreach (int f in featrueArray[0].data) pyGlobal.Append(FeatureConst.Instance.pyIntMap[f]);

            PyList pyOwnMinionHp = new PyList();
            foreach (int f in featrueArray[1].data) pyOwnMinionHp.Append(FeatureConst.Instance.pyIntMap[f]);

            PyList pyOwnMinion = new PyList();
            foreach (int f in featrueArray[2].data) pyOwnMinion.Append(FeatureConst.Instance.pyIntMap[f]);

            PyList pyOwnCard = new PyList();
            foreach (int f in featrueArray[3].data) pyOwnCard.Append(FeatureConst.Instance.pyIntMap[f]);

            PyList pyOwnPlayed = new PyList();
            foreach (int f in featrueArray[4].data) pyOwnPlayed.Append(FeatureConst.Instance.pyIntMap[f]);

            PyList pyOwnPlayable = new PyList();
            foreach (int[] action in featrueArray[5].data)
            {
                PyList row = new PyList();
                foreach (int f in action) row.Append(FeatureConst.Instance.pyIntMap[f]);
                PythonUtils.AppendRecycle(pyOwnPlayable, row);
            }

            PyList pyEnemyMinionHp = new PyList();
            foreach (int f in featrueArray[6].data) pyEnemyMinionHp.Append(FeatureConst.Instance.pyIntMap[f]);

            PyList pyEnemyMinion = new PyList();
            foreach (int f in featrueArray[7].data) pyEnemyMinion.Append(FeatureConst.Instance.pyIntMap[f]);

            PyList pyEnemyCard = new PyList();
            foreach (int f in featrueArray[8].data) pyEnemyCard.Append(FeatureConst.Instance.pyIntMap[f]);

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
        //4:0; 3:0; 2:0; 4:1; 3:1; 4:2; 2:1, 3:2; 4:3; 1:0
        //4:4; 3:3, 2:2, 1:1, 0.0 
        //reverse line 1

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

            ret.featrueArray[1] = new Feature("OwnMinionHp", new int[FeatureConst.Instance.minionCardArray.Length]);
            ret.featrueArray[2] = new Feature("OwnMinion", new int[FeatureConst.Instance.minionCardArray.Length]);

            foreach (PlayerKeyInfo.MinionKeyInfo m in mPlayer.minionJsonList)
            {
                int idx = FeatureConst.Instance.cardStrIdxDict[m.name];
                ret.featrueArray[1].data[idx] = Math.Max(ret.featrueArray[1].data[idx], m.Hp);
                ret.featrueArray[2].data[idx]++;
            }

            ret.featrueArray[3] = new Feature("OwnCardFeature", new int[FeatureConst.Instance.cardArray.Length]);
            foreach (PlayerKeyInfo.CardKeyInfo hc in mPlayer.handcardJsonList)
            {
                int idx = FeatureConst.Instance.cardStrIdxDict[hc.cardName];
                ret.featrueArray[3].data[idx]++;
            }

            ret.featrueArray[4] = new Feature("OwnPlayedFeature", new int[10]);
            ret.featrueArray[5] = new Feature("OwnPlayableFeature", new int[10][]);

            for (int i = 0; i < 10; i++) ret.featrueArray[5].data[i] = new int[FeatureConst.Instance.cardArray.Length];
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
                            ret.featrueArray[5].data[playedCount][FeatureConst.Instance.cardStrIdxDict[hc.cardName]] = 1;
                        }
                        if (mPlayer.canPlayHeroPower[playedCount] == 1)
                        {
                            ret.featrueArray[5].data[playedCount][FeatureConst.Instance.cardStrIdxDict["fireblast"]] = 1;
                        }
                    }
                    int playedActionIdx;
                    if (actionKeyInfo.actionType == actionEnum.playcard)
                    {
                        playedActionIdx = FeatureConst.Instance.cardStrIdxDict[mPlayer.playedCardJsonList[playedCardCount].cardName];
                        playedCardCount++;
                    }
                    else
                    {
                        playedActionIdx = FeatureConst.Instance.cardStrIdxDict["fireblast"];
                    }
                    ret.featrueArray[4].data[playedCount] = playedActionIdx;
                    playedCount++;
                }
            }

            ret.featrueArray[6] = new Feature("EnemyMinionHp", new int[FeatureConst.Instance.minionCardArray.Length]);
            ret.featrueArray[7] = new Feature("EnemyMinion", new int[FeatureConst.Instance.minionCardArray.Length]);

            foreach (PlayerKeyInfo.MinionKeyInfo m in ePlayer.minionJsonList)
            {
                int idx = FeatureConst.Instance.cardStrIdxDict[m.name];
                ret.featrueArray[6].data[idx]++;
                ret.featrueArray[7].data[idx] = Math.Max(ret.featrueArray[7].data[idx], m.Hp);
            }

            ret.featrueArray[8] = new Feature("EnemyCardFeature", new int[FeatureConst.Instance.cardArray.Length]);

            foreach (PlayerKeyInfo.CardKeyInfo hc in ePlayer.handcardJsonList)
            {
                int idx = FeatureConst.Instance.cardStrIdxDict[hc.cardName];
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
            if (e <= 1) return e + 1;
            if (e <= 3) return 3;
            if (e <= 6) return 4;
            return 5;
        }

        public static double getHeroPowerMinionInteraction(Playfield pf, List<Minion> minions)
        {
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

        public static BinaryFeature encodeHandDeck(Playfield pf)
        {

            /*
             *  hand_length = 23
                hand = np.clip(own_hand, 0, 2) * 10 + np.clip(enemy_hand, 0, 2)
                hand = hand.reshape(-1, 1)

                # print (np.unique(hand))

                hand = np.select(
                    [hand == 2,
                     hand == 1,
                     hand == 12,
                     hand == 0,
                     hand == 11,
                     hand == 22,
                     hand == 21,
                     hand == 10,
                     hand == 20],
                    [np.array([1, 0, 0, 0, 0, 0, 0, 0, 0]),
                     np.array([0, 1, 0, 0, 0, 0, 0, 0, 0]),
                     np.array([0, 0, 1, 0, 0, 0, 0, 0, 0]),
                     np.array([0, 0, 0, 1, 0, 0, 0, 0, 0]),
                     np.array([0, 0, 0, 0, 1, 0, 0, 0, 0]),
                     np.array([0, 0, 0, 0, 0, 1, 0, 0, 0]),
                     np.array([0, 0, 0, 0, 0, 0, 1, 0, 0]),
                     np.array([0, 0, 0, 0, 0, 0, 0, 1, 0]),
                     np.array([0, 0, 0, 0, 0, 0, 0, 0, 1])
                     ])

                one_hot_length = 9
                hand_feature = hand.reshape(-1, hand_length, one_hot_length)

                hand_feature = np.rollaxis(hand_feature, 2, 1)
             */

            Player mPlayer = pf.getCurrentPlayer(true);
            Player ePlayer = pf.getCurrentPlayer(false);

            BinaryFeature ret = new TwoDBinaryFeature(FeatureConst.Instance.cardArray.Length);

            foreach (Handmanager.Handcard hc in mPlayer.owncards)
            {
                int idx = FeatureConst.Instance.cardIdxDict[hc.card.name];
                if (ret.data[idx] < 2) ret.data[idx]++;
            }

            foreach (Handmanager.Handcard hc in ePlayer.owncards)
            {
                int idx = FeatureConst.Instance.cardIdxDict[hc.card.name];
                if (ret.data[idx] < 20) ret.data[idx] += 10;
            }

            for (int i = 0; i < ret.data.Length; i++)
            {
                ret.data[i] = FeatureConst.Instance.countDict[ret.data[i]];
            }

            //deck
            List<CardDB.Card> mDeck, eDeck;
            if (pf.isOwnTurn)
            {
                mDeck = pf.homeDeck;
                eDeck = pf.awayDeck;
            }
            else
            {
                eDeck = pf.homeDeck;
                mDeck = pf.awayDeck;
            }

            int offset = FeatureConst.Instance.cardArray.Length;

            foreach (CardDB.Card card in mDeck)
            {
                int idx = FeatureConst.Instance.cardIdxDict[card.name];
                ret.data[idx]++;
            }

            foreach (CardDB.Card card in eDeck)
            {
                int idx = FeatureConst.Instance.cardIdxDict[card.name];
                ret.data[idx] += 10;
            }

            for (int i = 0; i < ret.data.Length; i++)
            {
                ret.data[i] = FeatureConst.Instance.countDict[ret.data[i]];
            }

            return ret;

        }


        public static BinaryFeature encodeHand(Playfield pf)
        {

            /*
             *  hand_length = 23
                hand = np.clip(own_hand, 0, 2) * 10 + np.clip(enemy_hand, 0, 2)
                hand = hand.reshape(-1, 1)

                # print (np.unique(hand))

                hand = np.select(
                    [hand == 2,
                     hand == 1,
                     hand == 12,
                     hand == 0,
                     hand == 11,
                     hand == 22,
                     hand == 21,
                     hand == 10,
                     hand == 20],
                    [np.array([1, 0, 0, 0, 0, 0, 0, 0, 0]),
                     np.array([0, 1, 0, 0, 0, 0, 0, 0, 0]),
                     np.array([0, 0, 1, 0, 0, 0, 0, 0, 0]),
                     np.array([0, 0, 0, 1, 0, 0, 0, 0, 0]),
                     np.array([0, 0, 0, 0, 1, 0, 0, 0, 0]),
                     np.array([0, 0, 0, 0, 0, 1, 0, 0, 0]),
                     np.array([0, 0, 0, 0, 0, 0, 1, 0, 0]),
                     np.array([0, 0, 0, 0, 0, 0, 0, 1, 0]),
                     np.array([0, 0, 0, 0, 0, 0, 0, 0, 1])
                     ])

                one_hot_length = 9
                hand_feature = hand.reshape(-1, hand_length, one_hot_length)

                hand_feature = np.rollaxis(hand_feature, 2, 1)
             */

            Player mPlayer = pf.getCurrentPlayer(true);
            Player ePlayer = pf.getCurrentPlayer(false);

            BinaryFeature ret = new TwoDBinaryFeature(FeatureConst.Instance.cardArray.Length);

            foreach (Handmanager.Handcard hc in mPlayer.owncards)
            {
                int idx = FeatureConst.Instance.cardIdxDict[hc.card.name];
                if (ret.data[idx] < 2) ret.data[idx]++;
            }

            foreach (Handmanager.Handcard hc in ePlayer.owncards)
            {
                int idx = FeatureConst.Instance.cardIdxDict[hc.card.name];
                if (ret.data[idx] < 20) ret.data[idx] += 10;
            }

            for (int i = 0; i < ret.data.Length; i++)
            {
                ret.data[i] = FeatureConst.Instance.countDict[ret.data[i]];
            }

            return ret;

        }

        public static BinaryFeature encodeDeck(Playfield pf)
        {
            List<CardDB.Card> mDeck, eDeck;
            if (pf.isOwnTurn)
            {
                mDeck = pf.homeDeck;
                eDeck = pf.awayDeck;
            }
            else
            {
                eDeck = pf.homeDeck;
                mDeck = pf.awayDeck;
            }

            BinaryFeature ret = new TwoDBinaryFeature(FeatureConst.Instance.cardArray.Length);

            foreach (CardDB.Card card in mDeck)
            {
                int idx = FeatureConst.Instance.cardIdxDict[card.name];
                ret.data[idx]++;
            }

            foreach (CardDB.Card card in eDeck)
            {
                int idx = FeatureConst.Instance.cardIdxDict[card.name];
                ret.data[idx] += 10;
            }

            for (int i = 0; i < ret.data.Length; i++)
            {
                ret.data[i] = FeatureConst.Instance.countDict[ret.data[i]];
            }

            return ret;
        }

        public static BinaryFeature encodePlayed(Playfield pf)
        {
            Player mPlayer = pf.getCurrentPlayer(true);
            Player ePlayer = pf.getCurrentPlayer(false);

            BinaryFeature ret = new OneDBinaryFeature(FeatureConst.Instance.cardArray.Length);

            foreach (Action action in mPlayer.playactions)
            {
                if (action.actionType == actionEnum.playcard)
                {
                    int idx = FeatureConst.Instance.cardIdxDict[action.card.card.name];
                    if (ret.data[idx] < 2) ret.data[idx]++;
                }
                else if (action.actionType == actionEnum.playcard)
                {
                    ret.data[FeatureConst.Instance.heroPowerIdx] = 1;
                }
            }

            return ret;
        }

        public static BinaryFeature encodePlay(Playfield pf)
        {
            Player mPlayer = pf.getCurrentPlayer(true);
            Player ePlayer = pf.getCurrentPlayer(false);

            BinaryFeature ret = new OneDBinaryFeature(FeatureConst.Instance.cardArray.Length);

            foreach (Handmanager.Handcard hc in mPlayer.owncards)
            {
                if (hc.canplayCard(pf, pf.isOwnTurn))
                {
                    int idx = FeatureConst.Instance.cardIdxDict[hc.card.name];
                    ret.data[idx] = 1;
                }
            }

            if (mPlayer.mana >= 2 && mPlayer.ownAbilityReady) ret.data[FeatureConst.Instance.heroPowerIdx] = 1;

            return ret;
        }

        public static BinaryFeature encodeBoard(Playfield pf)
        {
            /*
                count = own_board_count * 10 + enemy_board_count
                count = count.reshape(-1, 1)

                count = np.select(
                    [count == 2,
                     count == 1,
                     count == 12,
                     count == 0,
                     count == 11,
                     count == 22,
                     count == 21,
                     count == 10,
                     count == 20],
                    [np.array([1, 0, 0, 0, 0, 0, 0, 0, 0]),
                     np.array([0, 1, 0, 0, 0, 0, 0, 0, 0]),
                     np.array([0, 0, 1, 0, 0, 0, 0, 0, 0]),
                     np.array([0, 0, 0, 1, 0, 0, 0, 0, 0]),
                     np.array([0, 0, 0, 0, 1, 0, 0, 0, 0]),
                     np.array([0, 0, 0, 0, 0, 1, 0, 0, 0]),
                     np.array([0, 0, 0, 0, 0, 0, 1, 0, 0]),
                     np.array([0, 0, 0, 0, 0, 0, 0, 1, 0]),
                     np.array([0, 0, 0, 0, 0, 0, 0, 0, 1])
                    ])
           
            special = get_minion_special(name_idx)
            */

            int numHpLv = 5;
            Player mPlayer = pf.getCurrentPlayer(true);
            Player ePlayer = pf.getCurrentPlayer(false);
            BinaryFeature ret = new ThreeDBinaryFeature(FeatureConst.Instance.minionCardArray.Length, numHpLv);

            foreach (Minion m in mPlayer.ownMinions)
            {
                int hpLV = m.getHpValue();
                int idx = FeatureConst.Instance.cardIdxDict[m.handcard.card.name];
                if (ret.data[idx][hpLV] < 2) ret.data[idx][hpLV]++;
            }

            foreach (Minion m in ePlayer.ownMinions)
            {
                int hpLV = m.getHpValue();
                int idx = FeatureConst.Instance.cardIdxDict[m.handcard.card.name];
                if (ret.data[idx][hpLV] < 20) ret.data[idx][hpLV] += 10;
            }

            
            for (int i = 0; i < ret.data.Length; i++)
            {
                for (int j = 0; j < ret.data[j].Length; j++)
                {
                    ret.data[i][j] = FeatureConst.Instance.countDict[ret.data[i][j]];
                }
            }

            return ret;
        }

        public static void TestNumpyFeaturization(Playfield pf, NumpyStateFeature npStFeature)
        { 
            
        }

        public static void NumpyHLTarget(Playfield pf, dynamic targetArr)
        {
            Player mPlayer = pf.getCurrentPlayer(true);
            Player ePlayer = pf.getCurrentPlayer(false);
            int idx = 0;

            foreach (Action action in mPlayer.playactions)
            {
                if (action.actionType == actionEnum.playcard)
                {
                    idx = FeatureConst.Instance.cardIdxDict[action.card.card.name];
                    //targetArr.itemset(idx, 1);
                    targetArr.Append(FeatureConst.Instance.pyIntMap[idx]);
                }
                else if (action.actionType == actionEnum.useHeroPower)
                {
                    //targetArr.itemset(FeatureConst.Instance.heroPowerIdx, 1);
                    targetArr.Append(FeatureConst.Instance.pyIntMap[FeatureConst.Instance.heroPowerIdx]);
                }
                if (action.target != null && action.target.own != pf.isOwnTurn)
                {
                    idx = FeatureConst.Instance.cardIdxDict[action.target.handcard.card.name] +
                        FeatureConst.Instance.cardArray.Length;
                    targetArr.Append(FeatureConst.Instance.pyIntMap[idx]);
                    //targetArr.itemset(idx + FeatureConst.Instance.cardArray.Length, 1);
                }
            }

            //playedFeature.FillNpArr(playNpArr, 1);
        }

        public static BinaryFeature EncodeGlobal(Playfield pf)
        {
            Player mPlayer = pf.getCurrentPlayer(true);
            Player ePlayer = pf.getCurrentPlayer(false);
            int ownMana = mPlayer.ownMaxMana;
            int enemyMana = ePlayer.ownMaxMana;
            int ownHp = mPlayer.ownHero.getHeroHpValue();
            int enemyHp = ePlayer.ownHero.getHeroHpValue();
            int glbIdx = 0;
            glbIdx += ownMana - 1;

            BinaryFeature globalFeature = new OneDBinaryFeature(36);
            globalFeature.data[glbIdx] = 1;

            glbIdx = 10;
            if (!pf.isOwnTurn) globalFeature.data[glbIdx] = 1;

            int heroIdx = FeatureConst.Instance.heroHpDict[ownHp + enemyHp * 10];
            glbIdx += heroIdx;
            globalFeature.data[glbIdx] = 1;

            return globalFeature;
        }

        public static string FeaturizationToString(Playfield pf)
        {
            BinaryFeature globalFeature = EncodeGlobal(pf);
            string globalStr = globalFeature.EncodeToString();

            BinaryFeature boardFeature = encodeBoard(pf);
            string boardStr = boardFeature.EncodeToString();

            BinaryFeature handFeature = encodeHand(pf);
            string handStr = handFeature.EncodeToString();

            BinaryFeature playableFeature = encodePlay(pf);
            string playStr = playableFeature.EncodeToString();

            return globalStr + "|" + boardStr + "|" + handStr + "|" + playStr;
        }

        public static string FeaturizationToStringFlatten(Playfield pf)
        {
            BinaryFeature globalFeature = EncodeGlobal(pf);
            string globalStr = globalFeature.EncodeToStringFlatten();

            BinaryFeature boardFeature = encodeBoard(pf);
            string boardStr = boardFeature.EncodeToStringFlatten();

            BinaryFeature handFeature = encodeHand(pf);
            string handStr = handFeature.EncodeToStringFlatten();

            BinaryFeature playableFeature = encodePlay(pf);
            string playStr = playableFeature.EncodeToStringFlatten();

            return globalStr + "|" + boardStr + "|" + handStr + "|" + playStr;
        }

        public static void NumpyFeaturization(Playfield pf, dynamic globalIdxArr, dynamic boardIdxArr, dynamic handIdxArr, dynamic playIdxArr)
        {
            Player mPlayer = pf.getCurrentPlayer(true);
            Player ePlayer = pf.getCurrentPlayer(false);
            int ownMana = mPlayer.ownMaxMana;
            int enemyMana = ePlayer.ownMaxMana;
            int ownHp = mPlayer.ownHero.getHeroHpValue();
            int enemyHp = ePlayer.ownHero.getHeroHpValue();
            int glbIdx = 0;
            glbIdx += ownMana - 1;
            //globalIdxArr.itemset(glbIdx, 1);
            globalIdxArr.Append(FeatureConst.Instance.pyIntMap[glbIdx]);
            glbIdx = 10;
            if (!pf.isOwnTurn) globalIdxArr.Append(FeatureConst.Instance.pyIntMap[glbIdx]);
            int heroIdx = FeatureConst.Instance.heroHpDict[ownHp + enemyHp * 10];
            glbIdx += heroIdx;
            globalIdxArr.Append(FeatureConst.Instance.pyIntMap[glbIdx]);

            BinaryFeature boardFeature = encodeBoard(pf);
            boardFeature.FillIdxArr(boardIdxArr, 0);

            BinaryFeature handFeature = encodeHand(pf);
            //BinaryFeature DeckFeature = encodeDeck(pf);

            PyList dim1 = new PyList();
            PyList dim2 = new PyList();

            for (int i = 0; i < handFeature.data.Length; i++)
            {
                dim1.Append(FeatureConst.Instance.pyIntMap[handFeature.data[i]]);
                dim2.Append(FeatureConst.Instance.pyIntMap[i]);
            }

            //for (int i = 0; i < DeckFeature.data.Length; i++)
            //{
            //    dim1.Append(FeatureConst.Instance.pyIntMap[DeckFeature.data[i] + 9]);
            //    dim2.Append(FeatureConst.Instance.pyIntMap[i]);
            //}

            PythonUtils.AppendRecycle(handIdxArr, dim1);
            PythonUtils.AppendRecycle(handIdxArr, dim2);

            BinaryFeature playableFeature = encodePlay(pf);

            dim1 = new PyList();
            dim2 = new PyList();

            for (int i = 0; i < playableFeature.data.Length; i++)
            {
                if (playableFeature.data[i] > 0)
                {
                    dim1.Append(FeatureConst.Instance.pyIntMap[0]);
                    dim2.Append(FeatureConst.Instance.pyIntMap[i]);
                }
            }

            BinaryFeature playedFeature = encodePlayed(pf);
            for (int i = 0; i < playedFeature.data.Length; i++)
            {
                if (playedFeature.data[i] > 0)
                {
                    dim1.Append(FeatureConst.Instance.pyIntMap[1]);
                    dim2.Append(FeatureConst.Instance.pyIntMap[i]);
                }
            }
            PythonUtils.AppendRecycle(playIdxArr, dim1);
            PythonUtils.AppendRecycle(playIdxArr, dim2);

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

            ret.featrueArray[1] = new Feature("OwnCardFeature", new int[FeatureConst.Instance.cardArray.Length]);
            ret.featrueArray[2] = new Feature("OwnPlayableFeature", new int[FeatureConst.Instance.cardArray.Length]);
            ret.featrueArray[3] = new Feature("OwnPlayedFeature", new int[FeatureConst.Instance.cardArray.Length]);
            ret.featrueArray[4] = new Feature("EffectFeature", new int[FeatureConst.Instance.cardArray.Length]);
            ret.featrueArray[5] = new Feature("CanPlayAfter", new int[FeatureConst.Instance.cardArray.Length]);

            double totalEffect = 0;
            foreach (Minion m in ePlayer.ownMinions)
            {
                totalEffect += 2 * m.getDefMinionValue();
            }

            if (mPlayer.ownAbilityReady && mPlayer.mana >= 2)
            {
                int idx = FeatureConst.Instance.cardIdxDict[CardDB.cardName.fireblast];
                ret.featrueArray[2].data[idx] = 1;

                double effect = getHeroPowerMinionInteraction(pf, ePlayer.ownMinions);
                int scaledEffect = (effect != 0) ? calcScaledEffect(effect, totalEffect) : 0;
                ret.featrueArray[4].data[idx] = scaledEffect;
            }

            foreach (Handmanager.Handcard hc in mPlayer.owncards)
            {
                int idx = FeatureConst.Instance.cardIdxDict[hc.card.name];
                ret.featrueArray[1].data[idx]++;
                if (ret.featrueArray[1].data[idx] > 6)
                {
                    int debug = 1;
                }
                if (hc.canplayCard(pf, pf.isOwnTurn))
                {
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

            foreach (Handmanager.Handcard hc in mPlayer.owncards)
            {
                int idx = FeatureConst.Instance.cardIdxDict[hc.card.name];
                if (ret.featrueArray[2].data[idx] == 1)
                {
                    //mana after play
                    int temp = mPlayer.mana;
                    int manaAfterPlay = mPlayer.mana - hc.getManaCost(pf, pf.isOwnTurn);
                    mPlayer.mana = manaAfterPlay;
                    int canPlayAfter = 0;
                    foreach (Handmanager.Handcard hc1 in mPlayer.owncards)
                    {
                        if (hc1.canplayCard(pf, pf.isOwnTurn))
                        {
                            int idx1 = FeatureConst.Instance.cardIdxDict[hc1.card.name];
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
                    int idx = FeatureConst.Instance.cardIdxDict[action.card.card.name];
                    ret.featrueArray[3].data[idx]++;
                }
                else if (action.actionType == actionEnum.useHeroPower)
                {
                    int idx = FeatureConst.Instance.cardIdxDict[CardDB.cardName.fireblast];
                    ret.featrueArray[3].data[idx]++;
                }
            }

            return ret;
        }

    }
}
