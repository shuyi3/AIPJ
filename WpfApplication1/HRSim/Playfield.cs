namespace HRSim
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Diagnostics;

    public class Macro
    {
        public List<Action> actionList;
        public float score;
        public int manaCost;
        public CardDB.cardName targetName;
        public int eneity;

        public Macro(Minion m, float score, List<Action> _actionList, int manaCost)
        {
            this.score = score;
            this.actionList = _actionList;
            this.manaCost = manaCost;
            if (m != null)
            {
                this.targetName = m.name;
                this.eneity = m.entitiyID;
            }
            else
            {
                this.targetName = CardDB.cardName.unknown;
            }
        }

        public void printTarget()
        { 
            string targetString = null;
            if (this.targetName == CardDB.cardName.unknown)
            {
                targetString = "No target";
            }
            else
            {
                targetString = "Target:" + this.targetName;
            }
            Helpfunctions.Instance.logg(targetString);
        }

    }

    public class moveChangeTrigger
    {
        public bool minionDied;
        public List<int> minionDiedList = new List<int>();
        public List<int> cardPlayedList = new List<int>();
        public List<int> newHandcardList = new List<int>();
        public int keepCard;
        public bool handcardAdded;
        public bool manaChanged;
        public bool tauntChanged;
        public bool ownNewTarget;
        public bool enemyNewTarget;
        public bool hasOwnTargetMove;
        public int manaSpent;
        public bool coinUsed;
        public int ownMinionDied;
        public int enemyMinionDied;
        public float moveReward;

        public moveChangeTrigger()
        {        
        }

        public moveChangeTrigger(moveChangeTrigger m)
        {
            minionDied = m.minionDied;
            minionDiedList = new List<int>(m.minionDiedList);
            cardPlayedList = new List<int>(m.cardPlayedList);
            newHandcardList = new List<int>(m.newHandcardList);
            keepCard = -10000; 
            handcardAdded = m.handcardAdded;
            manaChanged = m.manaChanged;
            tauntChanged = m.tauntChanged;
            ownNewTarget = m.ownNewTarget;
            enemyNewTarget = m.enemyNewTarget;
            hasOwnTargetMove = m.hasOwnTargetMove;
            manaSpent = m.manaSpent;
            coinUsed = m.coinUsed;
            moveReward = m.moveReward;
        }

        //TODO: cardplaylist其实没用
        public void Clear()
        {
            minionDied = false;
            minionDiedList.Clear();
            cardPlayedList.Clear();
            newHandcardList.Clear();
            keepCard = -10000;
            handcardAdded = false;
            manaChanged = false;
            tauntChanged = false;
            ownNewTarget = false;
            enemyNewTarget = false;
            hasOwnTargetMove = false;
            manaSpent = 0;
            coinUsed = false;
            ownMinionDied = 0;
            enemyMinionDied = 0;
            moveReward = 0.0f;
        }
    }

    public struct triggerCounter
    {
        public int minionsGotHealed;

        public int charsGotHealed;

        public int ownMinionsGotDmg;
        public int enemyMinionsGotDmg;

        public int ownHeroGotDmg;
        public int enemyHeroGotDmg;

        public int ownMinionsDied;
        public int enemyMinionsDied;
        public int ownBeastDied;
        public int enemyBeastDied;
        public int ownMechanicDied;
        public int enemyMechanicDied;
        public int ownMurlocDied;
        public int enemyMurlocDied;

        public bool ownMinionsChanged;
        public bool enemyMininsChanged;
    }

    public class Playfield
    {
        //Todo: delete all new list<minion>
        //TODO: graveyard change (list <card,owner>)
        //Todo: vanish clear all auras/buffs (NEW1_004)
        public float currentActionReward = 0.0f;
        public int rngIndex;
        public List<Action> moveList;
        public Dictionary<Tuple<int, int>, int> moveMap;
        public List<int> keepCardList;

        public bool logging = false;
        //public bool complete = false;

        public int nextEntity = 70;

        public triggerCounter tempTrigger = new triggerCounter();

        public moveChangeTrigger moveTrigger = new moveChangeTrigger();

        //aura minions##########################
        //todo reduce buffing vars
        //public int anzOwnRaidleader = 0;
        //public int anzEnemyRaidleader = 0;
        //public int anzOwnStormwindChamps = 0;
        //public int anzEnemyStormwindChamps = 0;
        //public int anzOwnTundrarhino = 0;
        //public int anzEnemyTundrarhino = 0;
        //public int anzOwnTimberWolfs = 0;
        //public int anzEnemyTimberWolfs = 0;
        //public int anzMurlocWarleader = 0;
        //public int anzGrimscaleOracle = 0;
        //public int anzOwnAuchenaiSoulpriest = 0;
        //public int anzEnemyAuchenaiSoulpriest = 0;
        //public int anzOwnsorcerersapprentice = 0;
        //public int anzOwnsorcerersapprenticeStarted = 0;
        //public int anzEnemysorcerersapprentice = 0;
        //public int anzEnemysorcerersapprenticeStarted = 0;
        //public int anzOwnSouthseacaptain = 0;
        //public int anzEnemySouthseacaptain = 0;
        //public int anzOwnMalGanis = 0;
        //public int anzEnemyMalGanis = 0;
        //public int anzOwnChromaggus = 0;
        //public int anzEnemyChromaggus = 0;
        //public int anzOwnDragonConsort = 0;
        //public int anzOwnDragonConsortStarted = 0;

        //public int anzOwnMechwarper = 0;
        //public int anzOwnMechwarperStarted = 0;
        //public int anzEnemyMechwarper = 0;
        //public int anzEnemyMechwarperStarted = 0;

        //public int anzEnemyTaunt = 0;
        //public int ownMinionsDiedTurn = 0;
        //public int enemyMinionsDiedTurn = 0;

        public bool feugenDead = false;
        public bool stalaggDead = false;

        //public bool weHavePlayedMillhouseManastorm = false;
        //public bool weHaveSteamwheedleSniper = false;
        //public bool enemyHaveSteamwheedleSniper = false;

        public bool needGraveyard = true;

        //public int doublepriest = 0;
        //public int enemydoublepriest = 0;


        //public int ownBaronRivendare = 0;
        //public int enemyBaronRivendare = 0;
        ////#########################################

        //public int tempanzOwnCards = 0; // for Goblin Sapper
        //public int tempanzEnemyCards = 0;// for Goblin Sapper

        public bool isOwnTurn = true; // its your turn?
        public int turnCounter = 0;
        //public bool sEnemTurn = false;//should the enemy turn be simulated?

        //public bool attacked = false;
        //public int attackFaceHP = 15;

        public int evaluatePenality = 0;
        public int lastTurnEvaluatePenality = 0;
        //public int ownController = 0;

        ////public int ownHeroEntity = -1;
        ////public int enemyHeroEntity = -1;

        //public int hashcode = 0;
        public float value = Int32.MinValue; //value for the player to move
        public int guessingHeroHP = 30;

        //public int mana = 0;
        //public int manaTurnEnd = 0;

        //public List<CardDB.cardIDEnum> ownSecretsIDList = new List<CardDB.cardIDEnum>();
        //public List<SecretItem> enemySecretList = new List<SecretItem>();

        //public int enemySecretCount = 0;

        //public Minion ownHero;
        //public Minion enemyHero;
        //public HeroEnum ownHeroName = HeroEnum.druid;
        //public HeroEnum enemyHeroName = HeroEnum.druid;
        //public TAG_CLASS ownHeroStartClass = TAG_CLASS.INVALID;
        //public TAG_CLASS enemyHeroStartClass = TAG_CLASS.INVALID;

        //public CardDB.cardName ownWeaponName = CardDB.cardName.unknown;
        //public int ownWeaponAttack = 0;
        //public int ownWeaponDurability = 0;

        //public CardDB.cardName enemyWeaponName = CardDB.cardName.unknown;
        //public int enemyWeaponAttack = 0;
        //public int enemyWeaponDurability = 0;

        //public List<Minion> ownMinions = new List<Minion>();
        //public List<Minion> enemyMinions = new List<Minion>();
        public List<GraveYardItem> diedMinions = null;

        //public List<Handmanager.Handcard> owncards = new List<Handmanager.Handcard>();
        //public List<Handmanager.Handcard> enemycards = new List<Handmanager.Handcard>();
        //public int owncarddraw = 0;

        //public List<Action> playactions = new List<Action>();

        //public int enemycarddraw = 0;
        //public int enemyAnzCards = 0;

        //public int spellpower = 0;
        //public int enemyspellpower = 0;

        //public bool playedmagierinderkirintor = false;
        //public bool playedPreparation = false;

        //public bool loatheb = false;
        //public int winzigebeschwoererin = 0;
        //public int startedWithWinzigebeschwoererin = 0;
        //public int managespenst = 0;
        //public int startedWithManagespenst = 0;
        //public int soeldnerDerVenture = 0;
        //public int startedWithsoeldnerDerVenture = 0;
        //public int beschwoerungsportal = 0;
        //public int startedWithbeschwoerungsportal = 0;
        //public int nerubarweblord = 0;
        //public int startedWithnerubarweblord = 0;

        //public bool startedWithDamagedMinions = false; // needed for manacalculation of the spell "Crush"

        //public int ownWeaponAttackStarted = 0;
        //public int ownMobsCountStarted = 0;
        //public int ownCardsCountStarted = 0;
        //public int enemyCardsCountStarted = 0;
        //public int ownHeroHpStarted = 30;
        //public int enemyHeroHpStarted = 30;

        //public int mobsplayedThisTurn = 0;
        //public int startedWithMobsPlayedThisTurn = 0;

        //public int optionsPlayedThisTurn = 0;
        //public int cardsPlayedThisTurn = 0;
        public int ueberladung = 0; //=recall

        //public int enemyOptionsDoneThisTurn = 0;

        //public int ownMaxMana = 0;
        //public int enemyMaxMana = 0;

        public int lostDamage = 0;
        public int lostHeal = 0;
        public int lostWeaponDamage = 0;

        //public int ownDeckSize = 30;
        //public int enemyDeckSize = 30;
        //public int ownHeroFatigue = 0;
        //public int enemyHeroFatigue = 0;

        //public bool ownAbilityReady = false;
        //public Handmanager.Handcard ownHeroAblility;

        //public bool enemyAbilityReady = false;
        //public Handmanager.Handcard enemyHeroAblility;

        //// just for saving which minion to revive with secrets (=the first one that died);
        //public CardDB.cardIDEnum revivingOwnMinion = CardDB.cardIDEnum.None;
        //public CardDB.cardIDEnum revivingEnemyMinion = CardDB.cardIDEnum.None;
        //public CardDB.cardIDEnum OwnLastDiedMinion = CardDB.cardIDEnum.None;

        public int shadowmadnessed = 0; //minions has switched controllers this turn.
        //Helpfunctions help = Helpfunctions.Instance;

        //implementation

        public List<CardDB.Card> homeDeck = new List<CardDB.Card>();
        public List<CardDB.Card> awayDeck = new List<CardDB.Card>();

        //public List<List<CardDB.Card>> ownBucketList = new List<List<CardDB.Card>>(5);
        //public List<List<CardDB.Card>> enemyBucketList = new List<List<CardDB.Card>>(5);

        public List<GraveYardItem> graveYard = new List<GraveYardItem>();

        public List<CardDB.Card> ownCardsToDraw = new List<CardDB.Card>();
        public List<CardDB.Card> enemyCardsToDraw = new List<CardDB.Card>();

        public Player playerFirst, playerSecond;

        //public void addCardToBucket(CardDB.Card card, bool own)
        //{
        //    List<List<CardDB.Card>> bucketList;

        //    if (own)
        //    {
        //        bucketList = ownBucketList;
        //    }
        //    else 
        //    {
        //        bucketList = enemyBucketList;
        //    }

        //    int index = 0;
        //    for (index = 0; index < GameManager.bucketMana.Length; index++)
        //    {
        //        if (card.cost <= GameManager.bucketMana[index])
        //            break;
        //    }

        //    Console.WriteLine("added " + card.name + " to bucket " + (index + 1));
        //    bucketList[index].Add(card);

        //}

        //public void debugBucket(bool own)
        //{
        //    List<List<CardDB.Card>> bucketList;
        //    List<CardDB.Card> Deck;

        //    if (own)
        //    {
        //        bucketList = ownBucketList;
        //        Deck = homeDeck;
        //    }
        //    else
        //    {
        //        bucketList = enemyBucketList;
        //        Deck = awayDeck;
        //    }

        //    int count = 0;
        //    foreach (List<CardDB.Card> bu in bucketList)
        //    {
        //        count += bu.Count;
        //    }
        //    Console.WriteLine("decksize = " + Deck.Count);
        //    Console.WriteLine("bucket = " + count);

        //    if (count != Deck.Count)
        //    {
        //        int debug = 1;
        //    }
        //}

        //public void removeCardFromBucket(CardDB.Card card, bool own)
        //{
        //    List<List<CardDB.Card>> bucketList;
        //    List<CardDB.Card> Deck;

        //    if (own)
        //    {
        //        bucketList = ownBucketList;
        //        Deck = homeDeck;
        //    }
        //    else
        //    {
        //        bucketList = enemyBucketList;
        //        Deck = awayDeck;
        //    }

        //    int index = 0;
        //    for (index = 0; index < GameManager.bucketMana.Length; index++)
        //    {
        //        if (card.cost <= GameManager.bucketMana[index])
        //            break;
        //    }

        //    //Console.WriteLine("removed " + card.name + " to bucket " + (index + 1));

        //    //Console.WriteLine("before: " + bucketList[index].Count);

        //    bool isRemoved = bucketList[index].Remove(card);

        //    //Console.WriteLine("after: " + bucketList[index].Count);

        //    if (!isRemoved)
        //    {
        //        int debug = 1;
        //    }
        //}

        public void initDeckFromFile(string filePath, bool own)
        {
            string[] deckLines;
            try
            {
                deckLines = System.IO.File.ReadAllLines(filePath);
                foreach (string line in deckLines)
                {
                    string cardNameString = Regex.Replace(line.ToLower(), @"[^a-z0-9]", "");
                    CardDB.Card card = CardDB.Instance.getDeckCardData(CardDB.Instance.cardNamestringToEnum(cardNameString));
                    if (own)
                    {
                        homeDeck.Add(card);
                    }
                    else
                    {
                        awayDeck.Add(card);
                    }
                    //addCardToBucket(card, own);
                }
            }
            catch
            {
                Console.WriteLine("init deck exception");
            }        
        }

        long LongRandom(long min, long max, Random rand)
        {
            byte[] buf = new byte[8];
            rand.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);

            return (Math.Abs(longRand % (max - min)) + min);
        }

        //public CardDB.cardName getArandomCardFromDeck(bool isHome)
        //{
        //    CardDB.cardName cardToDraw = CardDB.cardName.unknown;
        //    int r;
        //    if (isHome)
        //    {
        //        if (homeDeck.Count == 0) return cardToDraw;
        //        r = GameManager.getRNG().Next(homeDeck.Count);
        //        cardToDraw = homeDeck[r].name;
        //        homeDeck.RemoveAt(r);
        //    }
        //    else
        //    {
        //        if (awayDeck.Count == 0) return cardToDraw;
        //        r = GameManager.getRNG().Next(awayDeck.Count);
        //        cardToDraw = awayDeck[r].name;
        //        awayDeck.RemoveAt(r);
        //    }
        //    if (cardToDraw == CardDB.cardName.unknown)
        //    {
        //        int debug = 1;
        //    }
        //    return cardToDraw;
        //}

        public CardDB.cardName getACardFromDeck(double c, bool own)
        {
            CardDB.cardName cardToDraw = CardDB.cardName.unknown;
            int r;
            if (own)
            {
                if (homeDeck.Count == 0) return cardToDraw;
                r = (int)(homeDeck.Count * c);
                cardToDraw = homeDeck[r].name;
                //removeCardFromBucket(homeDeck[r], own);
                homeDeck.RemoveAt(r);
                //debugBucket(own);
            }
            else
            {
                if (awayDeck.Count == 0) return cardToDraw;
                r = (int)(awayDeck.Count * c);
                cardToDraw = awayDeck[r].name;
                //removeCardFromBucket(awayDeck[r], own);
                awayDeck.RemoveAt(r);
                //debugBucket(own);
            }
            return cardToDraw;
        }


        public CardDB.cardName getArandomCardFromDeck(bool own)
        {
            
            CardDB.cardName cardToDraw = CardDB.cardName.unknown;
            int r;
            if (own)
            {
                if (homeDeck.Count == 0) return cardToDraw;
                r = GameManager.getRNG().Next(homeDeck.Count);
                cardToDraw = homeDeck[r].name;
                //removeCardFromBucket(homeDeck[r], own);
                homeDeck.RemoveAt(r);
            }
            else
            {
                if (awayDeck.Count == 0) return cardToDraw;
                r = GameManager.getRNG().Next(awayDeck.Count);
                cardToDraw = awayDeck[r].name;
                //removeCardFromBucket(awayDeck[r], own);
                awayDeck.RemoveAt(r);
            }
            
            return cardToDraw;
        }

        public CardDB.cardIDEnum getArandomSecretFromDeck(bool own)//TODO: Add bucket, so it's broken
        {
            CardDB.cardIDEnum secIDtoDraw = CardDB.cardIDEnum.None;
            List<Tuple<int, CardDB.cardIDEnum>> secList = new List<Tuple<int, CardDB.cardIDEnum>>();
            List<CardDB.Card> deckToDrawFrom;
            Player mPlayer;
            int index = 0;

            if (own)
            {
                deckToDrawFrom = this.homeDeck;
                mPlayer = this.playerFirst;
            }
            else
            {
                deckToDrawFrom = this.awayDeck;
                mPlayer = this.playerSecond;
            }
            foreach (CardDB.Card card in deckToDrawFrom)
            {
                if (card.Secret)
                {
                    secList.Add(new Tuple<int, CardDB.cardIDEnum>(index, card.cardIDenum));
                }
                index++;
            }

            if (secList.Count > 0)
            {
                int r = GameManager.getRNG().Next(secList.Count);
                secIDtoDraw = secList[r].Item2;
                deckToDrawFrom.RemoveAt(secList[r].Item1);
                //mPlayer.ownDeckSize--;
            }
            return secIDtoDraw;
        }

        public void drawInitCards()
        {
            int r = 0;
            if (r == 0)
            { //home go first
                for (int i = 0; i < 4; i++)
                {
                    drawACard(getArandomCardFromDeck(true), true, passive: true);
                }
                //mulligan for mechmage and zoo TODO: change for handlock and other deck
                //int newDrawSize = 0;
                foreach (Handmanager.Handcard hc in playerFirst.owncards.ToArray())
                {
                    if (hc.manacost > 2)
                    {
                        //newDrawSize++;
                        playerFirst.owncards.Remove(hc);
                        drawACard(getArandomCardFromDeck(true), true, passive: true);
                        addCardBackToDeck(hc, true);
                    }
                }

                //for (int i = 0; i < newDrawSize; i++)
                //{
                //    drawACard(getArandomCardFromDeck(true), true, passive: true);
                //}

                //second player

                for (int i = 0; i < 4; i++)
                {
                    drawACard(getArandomCardFromDeck(false), false, passive: true);
                }

                foreach (Handmanager.Handcard hc in playerSecond.owncards.ToArray())
                {
                    if (hc.manacost > 2)
                    {
                        //newDrawSize++;
                        playerSecond.owncards.Remove(hc);
                        drawACard(getArandomCardFromDeck(false), false, passive: true);
                        addCardBackToDeck(hc, false);
                    }
                }

                //for (int i = 0; i < newDrawSize; i++)
                //{
                //    drawACard(getArandomCardFromDeck(false), false, passive: true);
                //}

                drawACard(CardDB.cardIDEnum.GAME_005, false, nopen: true, passive: true);

            }
            else
            {//home go second, with a coin



            }
            //
        }

        public void addCardBackToDeck(Handmanager.Handcard hc, bool own)
        {
            Player mPlayer;
            List<CardDB.Card> deck;

            if (own)
            {
                mPlayer = playerFirst;
                deck = homeDeck;
            }
            else
            {
                mPlayer = playerSecond;
                deck = awayDeck;
            }

            deck.Add(hc.card);
            //addCardToBucket(hc.card, own);
            //mPlayer.ownDeckSize++;
        }

        //end of implementation

        private void addMinionsReal(List<Minion> source, List<Minion> trgt)
        {
            foreach (Minion m in source)
            {
                trgt.Add(new Minion(m));
            }

        }

        private void addCardsReal(List<Handmanager.Handcard> source)
        {

            foreach (Handmanager.Handcard m in source)
            {
                this.playerFirst.owncards.Add(new Handmanager.Handcard(m));
            }

        }

        private void addEnemyCardsReal(List<Handmanager.Handcard> source)
        {

            foreach (Handmanager.Handcard m in source)
            {
                this.playerSecond.owncards.Add(new Handmanager.Handcard(m));
            }

        }

        public void setPlayer()
        {

            playerFirst = new Player(0);
            playerSecond = new Player(1);

        }

        public Playfield()
        {

            this.nextEntity = 1000;
            setPlayer();

            this.rngIndex = 0;
            this.moveList = new List<Action>();
            this.keepCardList = new List<int>();
            moveMap = new Dictionary<Tuple<int, int>, int>();

            //for (int i = 0; i < 5; i++)
            //{
            //    ownBucketList.Add(new List<CardDB.Card>());
            //    enemyBucketList.Add(new List<CardDB.Card>());
            //}
                //this.ownController = 1;

                //this.mana = 10;
                //this.manaTurnEnd = this.mana;
                //this.ownMaxMana = 10;
                //this.enemyMaxMana = 0;
                this.evaluatePenality = 0;
                //this.enemySecretCount = 0;


                //this.attackFaceHP = 15;

                //this.complete = false;

                //this.ownHero = new Minion();
                //this.enemyHero = new Minion();
                //this.ownHero.cardClass = TAG_CLASS.MAGE;
                //this.enemyHero.cardClass = TAG_CLASS.MAGE;

                ////implementation
                //this.ownHero.isHero = true;
                //this.enemyHero.isHero = true;
                //this.ownHero.own = true;
                //this.enemyHero.own = false;
                //this.ownHero.maxHp = 30;
                //this.enemyHero.maxHp = 30;
                //this.ownHero.entitiyID = 1;
                //this.enemyHero.entitiyID = 2;

                //this.ownHero.Angr = 0;
                //this.ownHero.Hp = 30;
                //this.ownHero.armor = 0;
                //this.ownHero.frozen = false;
                //this.ownHero.immuneWhileAttacking = false;
                //this.ownHero.immune = false;
                //this.ownHero.numAttacksThisTurn = 0;
                //this.ownHero.windfury = false;

                //this.enemyHero.Angr = 0;
                //this.enemyHero.Hp = 30;
                //this.enemyHero.armor = 0;
                //this.enemyHero.frozen = false;
                //this.enemyHero.immuneWhileAttacking = false;
                //this.enemyHero.immune = false;
                //this.enemyHero.numAttacksThisTurn = 0;
                //this.enemyHero.windfury = false;

                ////end of implementation

                //this.ownHeroName = HeroEnum.mage;
                //this.enemyHeroName = HeroEnum.mage;
                //this.ownHeroStartClass = TAG_CLASS.MAGE;
                //this.enemyHeroStartClass = TAG_CLASS.MAGE;

                ///*
                //this.enemyHeroHp = hrtprozis.enemyHp;
                //this.ownHeroHp = hrtprozis.heroHp;
                //this.ownHeroReady = hrtprozis.ownheroisread;
                //this.ownHeroWindfury = hrtprozis.ownHeroWindfury;
                //this.ownHeroNumAttackThisTurn = hrtprozis.ownHeroNumAttacksThisTurn;
                //this.ownHeroFrozen = hrtprozis.herofrozen;
                //this.enemyHeroFrozen = hrtprozis.enemyfrozen;
                //this.ownheroAngr = hrtprozis.heroAtk;
                //this.heroImmuneWhileAttacking = hrtprozis.heroImmuneToDamageWhileAttacking;
                //this.ownHeroDefence = hrtprozis.heroDefence;
                //this.enemyHeroDefence = hrtprozis.enemyDefence;
                // */

                ////####buffs#############################

                //this.anzOwnRaidleader = 0;
                //this.anzEnemyRaidleader = 0;
                //this.anzOwnStormwindChamps = 0;
                //this.anzEnemyStormwindChamps = 0;
                //this.anzOwnTundrarhino = 0;
                //this.anzEnemyTundrarhino = 0;
                //this.anzOwnTimberWolfs = 0;
                //this.anzEnemyTimberWolfs = 0;
                //this.anzMurlocWarleader = 0;
                //this.anzGrimscaleOracle = 0;
                //this.anzOwnAuchenaiSoulpriest = 0;
                //this.anzEnemyAuchenaiSoulpriest = 0;
                //this.anzOwnsorcerersapprentice = 0;
                //this.anzOwnsorcerersapprenticeStarted = 0;
                //this.anzEnemysorcerersapprentice = 0;
                //this.anzEnemysorcerersapprenticeStarted = 0;
                //this.anzOwnSouthseacaptain = 0;
                //this.anzEnemySouthseacaptain = 0;
                //this.anzOwnDragonConsortStarted = 0;

                //this.anzEnemyTaunt = 0;
                //this.ownMinionsDiedTurn = 0;
                //this.enemyMinionsDiedTurn = 0;

                //this.feugenDead = false;
                //this.stalaggDead = false;

                //this.weHavePlayedMillhouseManastorm = false;

                //this.doublepriest = 0;
                //this.enemydoublepriest = 0;

                //this.ownBaronRivendare = 0;
                //this.enemyBaronRivendare = 0;
                ////#########################################

                //this.ownWeaponDurability = 0;
                //this.ownWeaponAttack = 0;
                //this.ownWeaponName = CardDB.cardName.unknown;
                //this.owncarddraw = 0;

                //this.enemyWeaponAttack = 0;//dont know jet
                //this.enemyWeaponName = CardDB.cardName.unknown;
                //this.enemyWeaponDurability = 0;
                //this.enemycarddraw = 0;

                //this.enemyAnzCards = homeHandManager.enemyAnzCards;

                //this.ownAbilityReady = true;
                //this.ownHeroAblility = new Handmanager.Handcard(CardDB.Instance.getCardData(CardDB.Instance.cardNamestringToEnum("fireblast")));
                //this.enemyHeroAblility = new Handmanager.Handcard(CardDB.Instance.getCardData(CardDB.Instance.cardNamestringToEnum("fireblast")));
                //this.enemyAbilityReady = false;


                //this.mobsplayedThisTurn = 0;
                //this.startedWithMobsPlayedThisTurn = 0;// only change mobsplayedthisturm
                //this.cardsPlayedThisTurn = 0;
                ////todo:
                //this.optionsPlayedThisTurn = 0;

                //this.ueberladung = 0;

                //this.ownHeroFatigue = 0;
                //this.enemyHeroFatigue = 0;
                //this.ownDeckSize = 30;
                //this.enemyDeckSize = 30;

                ////need the following for manacost-calculation
                //this.ownHeroHpStarted = this.ownHero.Hp;
                //this.enemyHeroHpStarted = this.enemyHero.Hp;
                //this.ownWeaponAttackStarted = this.ownWeaponAttack;
                //this.ownCardsCountStarted = this.owncards.Count;
                //this.enemyCardsCountStarted = this.enemyAnzCards;
                //this.ownMobsCountStarted = this.ownMinions.Count + this.enemyMinions.Count;

                //this.playedmagierinderkirintor = false;
                //this.playedPreparation = false;

                //this.winzigebeschwoererin = 0;
                //this.managespenst = 0;
                //this.soeldnerDerVenture = 0;
                //this.beschwoerungsportal = 0;
                //this.nerubarweblord = 0;

                //this.startedWithnerubarweblord = 0;
                //this.startedWithbeschwoerungsportal = 0;
                //this.startedWithManagespenst = 0;
                //this.startedWithWinzigebeschwoererin = 0;
                //this.startedWithsoeldnerDerVenture = 0;

                //this.ownBaronRivendare = 0;
                //this.enemyBaronRivendare = 0;

                //needGraveyard = false;
                //this.loatheb = false;
                //this.spellpower = 0;
                //this.enemyspellpower = 0;

                //this.startedWithDamagedMinions = false;

                //foreach (Minion m in this.ownMinions)
                //{
                //    if (m.Hp < m.maxHp && m.Hp >= 1) this.startedWithDamagedMinions = true;
                //    if (m.playedThisTurn && m.name == CardDB.cardName.loatheb) this.loatheb = true;

                //    spellpower = spellpower + m.spellpower;
                //    if (m.silenced) continue;
                //    spellpower += m.handcard.card.spellpowervalue;
                //    if (m.name == CardDB.cardName.prophetvelen) this.doublepriest++;


                //    if (m.name == CardDB.cardName.pintsizedsummoner)
                //    {
                //        this.winzigebeschwoererin++;
                //        this.startedWithWinzigebeschwoererin++;
                //    }

                //    if (m.name == CardDB.cardName.manawraith)
                //    {
                //        this.managespenst++;
                //        this.startedWithManagespenst++;
                //    }
                //    if (m.name == CardDB.cardName.nerubarweblord)
                //    {
                //        this.nerubarweblord++;
                //        this.startedWithnerubarweblord++;
                //    }
                //    if (m.name == CardDB.cardName.venturecomercenary)
                //    {
                //        this.soeldnerDerVenture++;
                //        this.startedWithsoeldnerDerVenture++;
                //    }
                //    if (m.name == CardDB.cardName.summoningportal)
                //    {
                //        this.beschwoerungsportal++;
                //        this.startedWithbeschwoerungsportal++;
                //    }

                //    if (m.handcard.card.name == CardDB.cardName.baronrivendare)
                //    {
                //        this.ownBaronRivendare++;
                //    }
                //    if (m.handcard.card.name == CardDB.cardName.kelthuzad)
                //    {
                //        this.needGraveyard = true;
                //    }

                //    if (m.name == CardDB.cardName.raidleader) this.anzOwnRaidleader++;
                //    if (m.name == CardDB.cardName.malganis) this.anzOwnMalGanis++;
                //    if (m.name == CardDB.cardName.stormwindchampion) this.anzOwnStormwindChamps++;
                //    if (m.name == CardDB.cardName.tundrarhino) this.anzOwnTundrarhino++;
                //    if (m.name == CardDB.cardName.timberwolf) this.anzOwnTimberWolfs++;
                //    if (m.name == CardDB.cardName.murlocwarleader) this.anzMurlocWarleader++;
                //    if (m.name == CardDB.cardName.grimscaleoracle) this.anzGrimscaleOracle++;
                //    if (m.name == CardDB.cardName.auchenaisoulpriest) this.anzOwnAuchenaiSoulpriest++;
                //    if (m.name == CardDB.cardName.sorcerersapprentice)
                //    {
                //        this.anzOwnsorcerersapprentice++;
                //        this.anzOwnsorcerersapprenticeStarted++;
                //    }
                //    if (m.name == CardDB.cardName.southseacaptain) this.anzOwnSouthseacaptain++;
                //    if (m.name == CardDB.cardName.chromaggus) this.anzOwnChromaggus++;
                //    if (m.name == CardDB.cardName.dragonconsort && anzOwnDragonConsort > 0) this.anzOwnDragonConsortStarted++;
                //    if (m.name == CardDB.cardName.mechwarper)
                //    {
                //        this.anzOwnMechwarper++;
                //        this.anzOwnMechwarperStarted++;
                //    }
                //    if (m.name == CardDB.cardName.steamwheedlesniper && this.ownHeroName == HeroEnum.hunter)
                //    {
                //        this.weHaveSteamwheedleSniper = true;
                //    }

                //}

                //foreach (Handmanager.Handcard hc in this.owncards)
                //{

                //    if (hc.card.name == CardDB.cardName.kelthuzad)
                //    {
                //        this.needGraveyard = true;
                //    }
                //}

                //foreach (Minion m in this.enemyMinions)
                //{
                //    this.enemyspellpower = this.enemyspellpower + m.spellpower;
                //    enemyspellpower += m.handcard.card.spellpowervalue;
                //    if (m.silenced) continue;
                //    if (m.taunt) anzEnemyTaunt++;
                //    if (m.name == CardDB.cardName.prophetvelen) this.enemydoublepriest++;
                //    if (m.name == CardDB.cardName.manawraith)
                //    {
                //        this.managespenst++;
                //        this.startedWithManagespenst++;
                //    }
                //    if (m.name == CardDB.cardName.nerubarweblord)
                //    {
                //        this.nerubarweblord++;
                //        this.startedWithnerubarweblord++;
                //    }
                //    if (m.handcard.card.name == CardDB.cardName.baronrivendare)
                //    {
                //        this.enemyBaronRivendare++;
                //    }
                //    if (m.handcard.card.name == CardDB.cardName.kelthuzad)
                //    {
                //        this.needGraveyard = true;
                //    }

                //    if (m.name == CardDB.cardName.raidleader) this.anzEnemyRaidleader++;
                //    if (m.name == CardDB.cardName.malganis) this.anzEnemyMalGanis++;
                //    if (m.name == CardDB.cardName.stormwindchampion) this.anzEnemyStormwindChamps++;
                //    if (m.name == CardDB.cardName.tundrarhino) this.anzEnemyTundrarhino++;
                //    if (m.name == CardDB.cardName.timberwolf) this.anzEnemyTimberWolfs++;
                //    if (m.name == CardDB.cardName.murlocwarleader) this.anzMurlocWarleader++;
                //    if (m.name == CardDB.cardName.grimscaleoracle) this.anzGrimscaleOracle++;
                //    if (m.name == CardDB.cardName.auchenaisoulpriest) this.anzEnemyAuchenaiSoulpriest++;
                //    if (m.name == CardDB.cardName.sorcerersapprentice)
                //    {
                //        this.anzEnemysorcerersapprentice++;
                //        this.anzEnemysorcerersapprenticeStarted++;
                //    }
                //    if (m.name == CardDB.cardName.southseacaptain) this.anzEnemySouthseacaptain++;
                //    if (m.name == CardDB.cardName.chromaggus) this.anzEnemyChromaggus++;
                //    if (m.name == CardDB.cardName.mechwarper)
                //    {
                //        this.anzEnemyMechwarper++;
                //        this.anzEnemyMechwarperStarted++;
                //    }
                //    if (m.name == CardDB.cardName.steamwheedlesniper && this.enemyHeroName == HeroEnum.hunter)
                //    {
                //        this.enemyHaveSteamwheedleSniper = true;
                //    }
                //}
                //if (this.enemySecretCount >= 1) this.needGraveyard = true;
                if (this.needGraveyard) this.diedMinions = new List<GraveYardItem>(Probabilitymaker.Instance.turngraveyard);

            //this.tempanzOwnCards = this.owncards.Count;
            //this.tempanzEnemyCards = this.enemyAnzCards;


        }

        public Playfield(Playfield p)
        {

            //implementation      
            this.rngIndex = p.rngIndex;
            this.moveList = new List<Action>(p.moveList);
            this.keepCardList = new List<int>(p.keepCardList);
            moveMap = new Dictionary<Tuple<int, int>, int>();
            moveTrigger = new moveChangeTrigger(p.moveTrigger);

            this.isOwnTurn = p.isOwnTurn;
            this.homeDeck = new List<CardDB.Card>(p.homeDeck);
            this.awayDeck = new List<CardDB.Card>(p.awayDeck);

            //this.ownBucketList = new List<List<CardDB.Card>>();
            //this.enemyBucketList = new List<List<CardDB.Card>>();

            this.ownCardsToDraw = new List<CardDB.Card>(p.ownCardsToDraw);
            this.enemyCardsToDraw = new List<CardDB.Card>(p.enemyCardsToDraw);

            //foreach (List<CardDB.Card> bucket in p.ownBucketList)
            //{
            //    this.ownBucketList.Add(new List<CardDB.Card>(bucket));
            //}

            //foreach (List<CardDB.Card> bucket in p.enemyBucketList)
            //{
            //    this.enemyBucketList.Add(new List<CardDB.Card>(bucket));
            //}

            this.graveYard = new List<GraveYardItem>(p.graveYard);
            //end of implementation
            this.evaluatePenality = p.evaluatePenality;
            this.lastTurnEvaluatePenality = p.lastTurnEvaluatePenality;

            this.nextEntity = p.nextEntity;

            this.playerFirst = new Player(p.playerFirst);
            this.playerSecond = new Player(p.playerSecond);

            this.needGraveyard = p.needGraveyard;
            if (needGraveyard) this.diedMinions = new List<GraveYardItem>(p.diedMinions);

            //####buffs#############################
        }

        public Playfield(int nextEntity, bool own, PlayerKeyInfo player1Info, PlayerKeyInfo player2Info) //only end turn
        { 

            //implementation      
            //this.rngIndex = p.rngIndex;
            this.moveList = new List<Action>();
            //moveMap = new Dictionary<Tuple<int, int>, int>();
            moveTrigger = new moveChangeTrigger();
            keepCardList = new List<int>();

            this.isOwnTurn = own; //start of turn

            this.homeDeck = new List<CardDB.Card>(player1Info.ownDeckList.Count);

            //for (int i = 0; i < 5; i++)
            //{
            //    ownBucketList.Add(new List<CardDB.Card>());
            //    enemyBucketList.Add(new List<CardDB.Card>());
            //}
            foreach (string cardName in player1Info.ownDeckList)
            {
                CardDB.Card card = CardDB.Instance.getDeckCardData(CardDB.Instance.cardNamestringToEnum(cardName));
                homeDeck.Add(card);
            }

            this.awayDeck = new List<CardDB.Card>(player2Info.ownDeckList.Count);

            foreach (string cardName in player2Info.ownDeckList)
            {
                CardDB.Card card = CardDB.Instance.getDeckCardData(CardDB.Instance.cardNamestringToEnum(cardName));
                awayDeck.Add(card);
            }

            this.ownCardsToDraw = new List<CardDB.Card>();
            this.enemyCardsToDraw = new List<CardDB.Card>();

            this.graveYard = new List<GraveYardItem>();
            //end of implementation

            this.nextEntity = 1000;

            this.playerFirst = this.createPlayerFromKeyInfo(player1Info);
            this.playerSecond = this.createPlayerFromKeyInfo(player2Info);

            this.nextEntity++;
            this.nextEntity = Math.Max(nextEntity, this.nextEntity);

            //this.needGraveyard = p.needGraveyard;
            //if (needGraveyard) this.diedMinions = new List<GraveYardItem>(p.diedMinions);
            this.diedMinions = new List<GraveYardItem>();

        }

        public bool isHandEqual(Playfield p, bool logg)
        {
            List<Handmanager.Handcard> cardList = new List<Handmanager.Handcard>(this.playerFirst.owncards);
            List<Handmanager.Handcard> cardListToCompare = new List<Handmanager.Handcard>(p.playerFirst.owncards);

            cardList.Sort((x, y) => string.Compare(x.card.name.ToString(), y.card.name.ToString()));
            cardListToCompare.Sort((x, y) => string.Compare(x.card.name.ToString(), y.card.name.ToString()));

            for (int i = 0; i < cardList.Count; i++)
            {
                Handmanager.Handcard dishc = cardList[i]; Handmanager.Handcard pishc = cardListToCompare[i];
                if (dishc.card.name != pishc.card.name || dishc.getManaCost(this, true) != pishc.getManaCost(p, true))
                {
                    if (logg) Helpfunctions.Instance.logg("handcard changed: " + dishc.card.name);
                    return false;
                }
            }

            return true;
        }

        public bool isEqual(Playfield p, bool logg)
        {

            if (this.playerSecond.ownSecretsIDList.Count != this.playerSecond.ownSecretsIDList.Count)
            {

                if (logg) Helpfunctions.Instance.logg("enemy secrets changed ");
                return false;
            }

            if (this.playerSecond.ownSecretsIDList.Count >= 1)
            {
                for (int i = 0; i < this.playerSecond.ownSecretsIDList.Count; i++)
                {
                    if (this.playerSecond.ownSecretsIDList[i] != p.playerSecond.ownSecretsIDList[i])
                    {
                        if (logg) Helpfunctions.Instance.logg("enemy secrets changed! ");
                        return false;
                    }
                }
            }

            if (this.playerFirst.mana != p.playerFirst.mana || this.playerSecond.ownMaxMana != p.playerSecond.ownMaxMana || this.playerFirst.ownMaxMana != p.playerFirst.ownMaxMana)
            {
                if (logg) Helpfunctions.Instance.logg("mana changed " + this.playerFirst.mana + " " + p.playerFirst.mana + " " + this.playerSecond.ownMaxMana + " " + p.playerSecond.ownMaxMana + " " + this.playerFirst.ownMaxMana + " " + p.playerFirst.ownMaxMana + " " + this.isOwnTurn + " " + p.isOwnTurn);
                return false;
            }

            if (this.homeDeck.Count != p.homeDeck.Count || this.awayDeck.Count != p.awayDeck.Count || this.playerFirst.ownHeroFatigue != p.playerFirst.ownHeroFatigue || this.playerSecond.ownHeroFatigue != p.playerSecond.ownHeroFatigue)
            {
                if (logg) Helpfunctions.Instance.logg("deck/fatigue changed " + this.homeDeck.Count + " " + p.homeDeck.Count + " " + this.awayDeck.Count + " " + p.awayDeck.Count + " " + this.playerFirst.ownHeroFatigue + " " + p.playerFirst.ownHeroFatigue + " " + this.playerSecond.ownHeroFatigue + " " + p.playerSecond.ownHeroFatigue);
            }

            if (this.playerFirst.cardsPlayedThisTurn != p.playerFirst.cardsPlayedThisTurn || this.playerFirst.mobsplayedThisTurn != p.playerFirst.mobsplayedThisTurn || this.ueberladung != p.ueberladung || this.playerFirst.ownAbilityReady != p.playerFirst.ownAbilityReady)
            {
                if (logg) Helpfunctions.Instance.logg("stuff changed " + this.playerFirst.cardsPlayedThisTurn + " " + p.playerFirst.cardsPlayedThisTurn + " " + this.playerFirst.mobsplayedThisTurn + " " + p.playerFirst.mobsplayedThisTurn + " " + this.ueberladung + " " + p.ueberladung + " " + this.playerFirst.ownAbilityReady + " " + p.playerFirst.ownAbilityReady);
                //this.printBoard();
                //p.printBoard();
                return false;
            }

            if (this.playerFirst.ownHeroName != p.playerFirst.ownHeroName || this.playerSecond.ownHeroName != p.playerSecond.ownHeroName)
            {
                if (logg) Helpfunctions.Instance.logg("hero name changed ");
                return false;
            }

            if (this.playerFirst.ownHero.Hp != p.playerFirst.ownHero.Hp || this.playerFirst.ownHero.Angr != p.playerFirst.ownHero.Angr || this.playerFirst.ownHero.armor != p.playerFirst.ownHero.armor || this.playerFirst.ownHero.frozen != p.playerFirst.ownHero.frozen || this.playerFirst.ownHero.immuneWhileAttacking != p.playerFirst.ownHero.immuneWhileAttacking || this.playerFirst.ownHero.immune != p.playerFirst.ownHero.immune)
            {
                if (logg) Helpfunctions.Instance.logg("ownhero changed " + this.playerFirst.ownHero.Hp + " " + p.playerFirst.ownHero.Hp + " " + this.playerFirst.ownHero.Angr + " " + p.playerFirst.ownHero.Angr + " " + this.playerFirst.ownHero.armor + " " + p.playerFirst.ownHero.armor + " " + this.playerFirst.ownHero.frozen + " " + p.playerFirst.ownHero.frozen + " " + this.playerFirst.ownHero.immuneWhileAttacking + " " + p.playerFirst.ownHero.immuneWhileAttacking + " " + this.playerFirst.ownHero.immune + " " + p.playerFirst.ownHero.immune);
                return false;
            }
            if (this.playerFirst.ownHero.Ready != p.playerFirst.ownHero.Ready || this.playerFirst.ownWeaponAttack != p.playerFirst.ownWeaponAttack || this.playerFirst.ownWeaponDurability != p.playerFirst.ownWeaponDurability || this.playerFirst.ownHero.numAttacksThisTurn != p.playerFirst.ownHero.numAttacksThisTurn || this.playerFirst.ownHero.windfury != p.playerFirst.ownHero.windfury)
            {
                if (logg) Helpfunctions.Instance.logg("weapon changed " + this.playerFirst.ownHero.Ready + " " + p.playerFirst.ownHero.Ready + " " + this.playerFirst.ownWeaponAttack + " " + p.playerFirst.ownWeaponAttack + " " + this.playerFirst.ownWeaponDurability + " " + p.playerFirst.ownWeaponDurability + " " + this.playerFirst.ownHero.numAttacksThisTurn + " " + p.playerFirst.ownHero.numAttacksThisTurn + " " + this.playerFirst.ownHero.windfury + " " + p.playerFirst.ownHero.windfury);
                return false;
            }
            if (this.playerSecond.ownHero.Hp != p.playerSecond.ownHero.Hp || this.playerSecond.ownWeaponAttack != p.playerSecond.ownWeaponAttack || this.playerSecond.ownHero.armor != p.playerSecond.ownHero.armor || this.playerSecond.ownWeaponDurability != p.playerSecond.ownWeaponDurability || this.playerSecond.ownHero.frozen != p.playerSecond.ownHero.frozen || this.playerSecond.ownHero.immune != p.playerSecond.ownHero.immune)
            {
                if (logg) Helpfunctions.Instance.logg("enemyhero changed " + this.playerSecond.ownHero.Hp + " " + p.playerSecond.ownHero.Hp + " " + this.playerSecond.ownWeaponAttack + " " + p.playerSecond.ownWeaponAttack + " " + this.playerSecond.ownHero.armor + " " + p.playerSecond.ownHero.armor + " " + this.playerSecond.ownWeaponDurability + " " + p.playerSecond.ownWeaponDurability + " " + this.playerSecond.ownHero.frozen + " " + p.playerSecond.ownHero.frozen + " " + this.playerSecond.ownHero.immune + " " + p.playerSecond.ownHero.immune);
                return false;
            }

            /*if (this.auchenaiseelenpriesterin != p.auchenaiseelenpriesterin || this.winzigebeschwoererin != p.winzigebeschwoererin || this.zauberlehrling != p.zauberlehrling || this.managespenst != p.managespenst || this.soeldnerDerVenture != p.soeldnerDerVenture || this.beschwoerungsportal != p.beschwoerungsportal || this.doublepriest != p.doublepriest)
            {
                Helpfunctions.Instance.logg("special minions changed " + this.auchenaiseelenpriesterin + " " + p.auchenaiseelenpriesterin + " " + this.winzigebeschwoererin + " " + p.winzigebeschwoererin + " " + this.zauberlehrling + " " + p.zauberlehrling + " " + this.managespenst + " " + p.managespenst + " " + this.soeldnerDerVenture + " " + p.soeldnerDerVenture + " " + this.beschwoerungsportal + " " + p.beschwoerungsportal + " " + this.doublepriest + " " + p.doublepriest);
                return false;
            }*/

            if (this.playerFirst.ownHeroAblility.card.name != p.playerFirst.ownHeroAblility.card.name)
            {
                if (logg) Helpfunctions.Instance.logg("hero ability changed ");
                return false;
            }

            if (this.playerFirst.spellpower != p.playerFirst.spellpower)
            {
                if (logg) Helpfunctions.Instance.logg("spellpower changed");
                return false;
            }

            if (this.playerFirst.ownMinions.Count != p.playerFirst.ownMinions.Count || this.playerSecond.ownMinions.Count != p.playerSecond.ownMinions.Count || this.playerFirst.owncards.Count != p.playerFirst.owncards.Count)
            {
                if (logg) Helpfunctions.Instance.logg("minions count or hand changed");
                return false;
            }


            List<Minion> minionList = new List<Minion>(this.playerFirst.ownMinions);
            List<Minion> minionListToCompare = new List<Minion>(p.playerFirst.ownMinions);

            minionList = minionList.OrderBy(x => (float)x.getMinionDefenseValue() * (int)(x.name)).ThenBy(x => x.Ready).ToList();
            minionListToCompare = minionListToCompare.OrderBy(x => (float)x.getMinionDefenseValue() * (int)(x.name)).ThenBy(x => x.Ready).ToList();

            //minionList.Sort((x, y) => ((float)x.getMinionDefenseValue() * (int)(x.name)).CompareTo(y.getMinionDefenseValue() * (int)(y.name)));
            //minionListToCompare.Sort((x, y) => ((float)x.getMinionDefenseValue() * (int)(x.name)).CompareTo(y.getMinionDefenseValue() * (int)(y.name)));

            bool minionbool = true;
            for (int i = 0; i < minionList.Count; i++)
            {
                Minion dis = minionList[i]; Minion pis = minionListToCompare[i];
                //if (dis.entitiyID == 0) dis.entitiyID = pis.entitiyID;
                //if (pis.entitiyID == 0) pis.entitiyID = dis.entitiyID;
                if (dis.name != pis.name)
                {
                    if (logg) Helpfunctions.Instance.logg("ownminions name changed");
                    minionbool = false;
                    break;
                }
                if (dis.Angr != pis.Angr || dis.Hp != pis.Hp || dis.maxHp != pis.maxHp || dis.numAttacksThisTurn != pis.numAttacksThisTurn)
                {
                    if (logg) Helpfunctions.Instance.logg("ownminions number changed");
                    minionbool = false;
                    break;
                }
                if (dis.Ready != pis.Ready)
                {
                    if (logg) Helpfunctions.Instance.logg("ownminions ready changed");
                    minionbool = false; // includes frozen, exhaunted
                    break;
                }
                //if (dis.playedThisTurn != pis.playedThisTurn) minionbool = false;
                //if (dis.silenced != pis.silenced || dis.stealth != pis.stealth || dis.taunt != pis.taunt || dis.windfury != pis.windfury || dis.zonepos != pis.zonepos) minionbool = false;
                if (dis.silenced != pis.silenced || dis.stealth != pis.stealth || dis.taunt != pis.taunt || dis.windfury != pis.windfury)
                {
                    if (logg) Helpfunctions.Instance.logg("ownminions taunt changed");
                    minionbool = false;
                    break;
                }
                if (dis.divineshild != pis.divineshild || dis.cantLowerHPbelowONE != pis.cantLowerHPbelowONE || dis.immune != pis.immune)
                {
                    if (logg) Helpfunctions.Instance.logg("ownminions devine changed");
                    minionbool = false;
                    break;
                }
                if (dis.ownBlessingOfWisdom != pis.ownBlessingOfWisdom || dis.enemyBlessingOfWisdom != pis.enemyBlessingOfWisdom)
                {
                    if (logg) Helpfunctions.Instance.logg("ownminions bless changed");
                    minionbool = false;
                    break;
                }
                if (dis.destroyOnEnemyTurnStart != pis.destroyOnEnemyTurnStart || dis.destroyOnEnemyTurnEnd != pis.destroyOnEnemyTurnEnd || dis.destroyOnOwnTurnEnd != pis.destroyOnOwnTurnEnd || dis.destroyOnOwnTurnStart != pis.destroyOnOwnTurnStart)
                {
                    if (logg) Helpfunctions.Instance.logg("ownminions destroy changed");
                    minionbool = false;
                    break;
                }
                if (dis.ancestralspirit != pis.ancestralspirit || dis.souloftheforest != pis.souloftheforest)
                {
                    if (logg) Helpfunctions.Instance.logg("ownminions buff deathrattle changed");
                    minionbool = false;
                    break;
                }

            }
            if (minionbool == false)
            {
                if (logg) Helpfunctions.Instance.logg("ownminions changed");
                //p.printBoard();
                //this.printBoard();
                return false;
            }

            minionList = new List<Minion>(this.playerSecond.ownMinions);
            minionListToCompare = new List<Minion>(p.playerSecond.ownMinions);

            minionList = minionList.OrderBy(x => (float)x.getMinionDefenseValue() * (int)(x.name)).ThenBy(x => x.Ready).ToList();
            minionListToCompare = minionListToCompare.OrderBy(x => (float)x.getMinionDefenseValue() * (int)(x.name)).ThenBy(x => x.Ready).ToList();
            //minionList.Sort((x, y) => ((float)x.getMinionDefenseValue() * (int)(x.name)).CompareTo(y.getMinionDefenseValue() * (int)(y.name)));
            //minionListToCompare.Sort((x, y) => ((float)x.getMinionDefenseValue() * (int)(x.name)).CompareTo(y.getMinionDefenseValue() * (int)(y.name)));

            for (int i = 0; i < minionList.Count; i++)
            {
                Minion dis = minionList[i]; Minion pis = minionListToCompare[i];
                //Helpfunctions.Instance.logg("MIN BOOL:" + minionbool);
                //Helpfunctions.Instance.logg("dis value = " + (float)dis.getMinionDefenseValue() * (int)(dis.name));
                //Helpfunctions.Instance.logg("pis value = " + (float)pis.getMinionDefenseValue() * (int)(dis.name));

                //if (dis.entitiyID == 0) dis.entitiyID = pis.entitiyID;
                //if (pis.entitiyID == 0) pis.entitiyID = dis.entitiyID;
                if (dis.name != pis.name) minionbool = false;
                if (dis.Angr != pis.Angr || dis.Hp != pis.Hp || dis.maxHp != pis.maxHp || dis.numAttacksThisTurn != pis.numAttacksThisTurn)
                {
                    minionbool = false;
                }
                //if (dis.Ready != pis.Ready) minionbool = false; // includes frozen, exhaunted
                if (dis.frozen != pis.frozen) minionbool = false; // includes frozen, exhaunted
                //if (dis.playedThisTurn != pis.playedThisTurn) minionbool = false;
                if (dis.silenced != pis.silenced || dis.stealth != pis.stealth || dis.taunt != pis.taunt || dis.windfury != pis.windfury)//|| dis.zonepos != pis.zonepos)
                {
                    minionbool = false;
                }
                if (dis.divineshild != pis.divineshild || dis.cantLowerHPbelowONE != pis.cantLowerHPbelowONE || dis.immune != pis.immune) minionbool = false;
                if (dis.ownBlessingOfWisdom != pis.ownBlessingOfWisdom || dis.enemyBlessingOfWisdom != pis.enemyBlessingOfWisdom) minionbool = false;
                if (dis.destroyOnEnemyTurnStart != pis.destroyOnEnemyTurnStart || dis.destroyOnEnemyTurnEnd != pis.destroyOnEnemyTurnEnd || dis.destroyOnOwnTurnEnd != pis.destroyOnOwnTurnEnd || dis.destroyOnOwnTurnStart != pis.destroyOnOwnTurnStart) minionbool = false;
                if (dis.ancestralspirit != pis.ancestralspirit || dis.souloftheforest != pis.souloftheforest) minionbool = false;
            }
            if (minionbool == false)
            {
                if (logg) Helpfunctions.Instance.logg("enemyminions changed");
                //this.debugMinions();
                //if (logg) Helpfunctions.Instance.logg("P:");
                //p.debugMinions();
                return false;
            }

            List<Handmanager.Handcard> cardList = new List<Handmanager.Handcard>(this.playerFirst.owncards);
            List<Handmanager.Handcard> cardListToCompare = new List<Handmanager.Handcard>(p.playerFirst.owncards);

            cardList.Sort((x, y) => string.Compare(x.card.name.ToString(), y.card.name.ToString()));
            cardListToCompare.Sort((x, y) => string.Compare(x.card.name.ToString(), y.card.name.ToString()));

            for (int i = 0; i < cardList.Count; i++)
            {
                Handmanager.Handcard dishc = cardList[i]; Handmanager.Handcard pishc = cardListToCompare[i];
                if (dishc.card.name != pishc.card.name || dishc.getManaCost(this, true) != pishc.getManaCost(p, true))
                {
                    if (logg) Helpfunctions.Instance.logg("handcard changed: " + dishc.card.name);
                    return false;
                }
            }

            //for (int i = 0; i < this.playerFirst.ownMinions.Count; i++)
            //{
            //    Minion dis = this.playerFirst.ownMinions[i]; Minion pis = p.playerFirst.ownMinions[i];
            //    //if (dis.entitiyID != pis.entitiyID) Ai.Instance.updateEntitiy(pis.entitiyID, dis.entitiyID);

            //}

            //for (int i = 0; i < this.playerSecond.ownMinions.Count; i++)
            //{
            //    Minion dis = this.playerSecond.ownMinions[i]; Minion pis = p.playerSecond.ownMinions[i];
            //    //if (dis.entitiyID != pis.entitiyID) Ai.Instance.updateEntitiy(pis.entitiyID, dis.entitiyID);

            //}
            if (this.playerFirst.ownSecretsIDList.Count != p.playerFirst.ownSecretsIDList.Count)
            {
                if (logg) Helpfunctions.Instance.logg("secretsCount changed");
                return false;
            }
            for (int i = 0; i < this.playerFirst.ownSecretsIDList.Count; i++)
            {
                if (this.playerFirst.ownSecretsIDList[i] != p.playerFirst.ownSecretsIDList[i])
                {
                    if (logg) Helpfunctions.Instance.logg("secrets changed");
                    return false;
                }
            }
            return true;
        }

        //public bool isEqualf(Playfield p)
        //{
        //    if (this.value != p.value) return false;

        //    if (this.ownMinions.Count != p.ownMinions.Count || this.enemyMinions.Count != p.enemyMinions.Count || this.owncards.Count != p.owncards.Count) return false;

        //    if (this.cardsPlayedThisTurn != p.cardsPlayedThisTurn || this.mobsplayedThisTurn != p.mobsplayedThisTurn || this.ueberladung != p.ueberladung || this.ownAbilityReady != p.ownAbilityReady) return false;

        //    if (this.mana != p.mana || this.enemyMaxMana != p.enemyMaxMana || this.ownMaxMana != p.ownMaxMana) return false;

        //    if (this.ownHeroName != p.ownHeroName || this.enemyHeroName != p.enemyHeroName || this.enemySecretCount != p.enemySecretCount) return false;

        //    if (this.ownHero.Hp != p.ownHero.Hp || this.ownHero.Angr != p.ownHero.Angr || this.ownHero.armor != p.ownHero.armor || this.ownHero.frozen != p.ownHero.frozen || this.ownHero.immuneWhileAttacking != p.ownHero.immuneWhileAttacking || this.ownHero.immune != p.ownHero.immune) return false;

        //    if (this.ownHero.Ready != p.ownHero.Ready || this.ownWeaponAttack != p.ownWeaponAttack || this.ownWeaponDurability != p.ownWeaponDurability || this.ownHero.numAttacksThisTurn != p.ownHero.numAttacksThisTurn || this.ownHero.windfury != p.ownHero.windfury) return false;

        //    if (this.enemyHero.Hp != p.enemyHero.Hp || this.enemyWeaponAttack != p.enemyWeaponAttack || this.enemyHero.armor != p.enemyHero.armor || this.enemyWeaponDurability != p.enemyWeaponDurability || this.enemyHero.frozen != p.enemyHero.frozen || this.enemyHero.immune != p.enemyHero.immune) return false;

        //    if (this.ownHeroAblility.card.name != p.ownHeroAblility.card.name || this.spellpower != p.spellpower) return false;

        //    bool minionbool = true;
        //    for (int i = 0; i < this.ownMinions.Count; i++)
        //    {
        //        Minion dis = this.ownMinions[i]; Minion pis = p.ownMinions[i];
        //        //if (dis.entitiyID == 0) dis.entitiyID = pis.entitiyID;
        //        //if (pis.entitiyID == 0) pis.entitiyID = dis.entitiyID;
        //        if (dis.entitiyID != pis.entitiyID) minionbool = false;
        //        if (dis.Angr != pis.Angr || dis.Hp != pis.Hp || dis.maxHp != pis.maxHp || dis.numAttacksThisTurn != pis.numAttacksThisTurn) minionbool = false;
        //        if (dis.Ready != pis.Ready) minionbool = false; // includes frozen, exhaunted
        //        if (dis.playedThisTurn != pis.playedThisTurn) minionbool = false;
        //        if (dis.silenced != pis.silenced || dis.stealth != pis.stealth || dis.taunt != pis.taunt || dis.windfury != pis.windfury || dis.zonepos != pis.zonepos) minionbool = false;
        //        if (dis.divineshild != pis.divineshild || dis.cantLowerHPbelowONE != pis.cantLowerHPbelowONE || dis.immune != pis.immune) minionbool = false;
        //        if (dis.ownBlessingOfWisdom != pis.ownBlessingOfWisdom || dis.enemyBlessingOfWisdom != pis.enemyBlessingOfWisdom) minionbool = false;
        //        if (dis.destroyOnEnemyTurnStart != pis.destroyOnEnemyTurnStart || dis.destroyOnEnemyTurnEnd != pis.destroyOnEnemyTurnEnd || dis.destroyOnOwnTurnEnd != pis.destroyOnOwnTurnEnd || dis.destroyOnOwnTurnStart != pis.destroyOnOwnTurnStart) minionbool = false;
        //        if (dis.ancestralspirit != pis.ancestralspirit || dis.souloftheforest != pis.souloftheforest) minionbool = false;
        //        if (minionbool == false) break;
        //    }
        //    if (minionbool == false)
        //    {

        //        return false;
        //    }

        //    for (int i = 0; i < this.enemyMinions.Count; i++)
        //    {
        //        Minion dis = this.enemyMinions[i]; Minion pis = p.enemyMinions[i];
        //        //if (dis.entitiyID == 0) dis.entitiyID = pis.entitiyID;
        //        //if (pis.entitiyID == 0) pis.entitiyID = dis.entitiyID;
        //        if (dis.entitiyID != pis.entitiyID) minionbool = false;
        //        if (dis.Angr != pis.Angr || dis.Hp != pis.Hp || dis.maxHp != pis.maxHp || dis.numAttacksThisTurn != pis.numAttacksThisTurn) minionbool = false;
        //        if (dis.Ready != pis.Ready) minionbool = false; // includes frozen, exhaunted
        //        if (dis.playedThisTurn != pis.playedThisTurn) minionbool = false;
        //        if (dis.silenced != pis.silenced || dis.stealth != pis.stealth || dis.taunt != pis.taunt || dis.windfury != pis.windfury || dis.zonepos != pis.zonepos) minionbool = false;
        //        if (dis.divineshild != pis.divineshild || dis.cantLowerHPbelowONE != pis.cantLowerHPbelowONE || dis.immune != pis.immune) minionbool = false;
        //        if (dis.ownBlessingOfWisdom != pis.ownBlessingOfWisdom || dis.enemyBlessingOfWisdom != pis.enemyBlessingOfWisdom) minionbool = false;
        //        if (dis.destroyOnEnemyTurnStart != pis.destroyOnEnemyTurnStart || dis.destroyOnEnemyTurnEnd != pis.destroyOnEnemyTurnEnd || dis.destroyOnOwnTurnEnd != pis.destroyOnOwnTurnEnd || dis.destroyOnOwnTurnStart != pis.destroyOnOwnTurnStart) minionbool = false;
        //        if (dis.ancestralspirit != pis.ancestralspirit || dis.souloftheforest != pis.souloftheforest) minionbool = false;
        //        if (minionbool == false) break;
        //    }
        //    if (minionbool == false)
        //    {
        //        return false;
        //    }

        //    owncards.Sort((x, y) => string.Compare(x.LastName, y.LastName));

        //    for (int i = 0; i < this.owncards.Count; i++)
        //    {
        //        Handmanager.Handcard dishc = this.owncards[i]; Handmanager.Handcard pishc = p.owncards[i];
        //        if (dishc.position != pishc.position || dishc.entity != pishc.entity || dishc.manacost != pishc.manacost)
        //        {
        //            return false;
        //        }
        //    }

        //    if (this.enemySecretCount >= 1)
        //    {
        //        for (int i = 0; i < this.enemySecretList.Count; i++)
        //        {
        //            if (!this.enemySecretList[i].isEqual(p.enemySecretList[i]))
        //            {
        //                return false;
        //            }
        //        }
        //    }

        //    if (this.ownSecretsIDList.Count != p.ownSecretsIDList.Count)
        //    {
        //        return false;
        //    }
        //    for (int i = 0; i < this.ownSecretsIDList.Count; i++)
        //    {
        //        if (this.ownSecretsIDList[i] != p.ownSecretsIDList[i])
        //        {
        //            return false;
        //        }
        //    }

        //    return true;
        //}

        //public override int GetHashCode()
        //{
        //    int retval = 0;
        //    retval += 10000 * this.playerFirst.ownMinions.Count + 100 * this.playerSecond.ownMinions.Count + 1000 * this.playerFirst.mana + 100000 * (this.playerFirst.ownHero.Hp + this.playerSecond.ownHero.Hp) + this.playerFirst.owncards.Count + this.playerFirst.owncarddraw + this.playerFirst.cardsPlayedThisTurn + this.playerFirst.mobsplayedThisTurn + this.playerFirst.ownHero.Angr + this.playerFirst.ownHero.armor + this.playerFirst.ownWeaponAttack + this.playerSecond.ownWeaponDurability;
        //    return retval;
        //}

        public override int GetHashCode()
        {
            return BoardHash.Instance.hashPlayfield(this);
        }

        //stuff for playing around enemy aoes
        //public void enemyPlaysAoe(int pprob, int pprob2)
        //{
        //    if (!this.loatheb)
        //    {
        //        MyPlayfield p = new MyPlayfield(this);
        //        float oldval = Ai.Instance.botBase.getPlayfieldValue(p);
        //        p.value = int.MinValue;
        //        p.EnemyCardPlaying(p.enemyHeroStartClass, p.mana, p.enemyAnzCards, pprob, pprob2);
        //        float newval = Ai.Instance.botBase.getPlayfieldValue(p);
        //        p.value = int.MinValue;
        //        if (oldval > newval) // new board is better for enemy (value is smaller)
        //        {
        //            this.copyValuesFrom(p);
        //        }
        //    }
        //}

        //public int EnemyCardPlaying(TAG_CLASS enemyHeroStrtClass, int currmana, int cardcount, int playAroundProb, int pap2)
        //{
        //    int mana = currmana;
        //    if (cardcount == 0) return currmana;

        //    bool useAOE = false;
        //    int mobscount = 0;
        //    foreach (Minion min in this.ownMinions)
        //    {
        //        if (min.maxHp >= 2 && min.Angr >= 2) mobscount++;
        //    }

        //    if (mobscount >= 3) useAOE = true;

        //    if (enemyHeroStrtClass == TAG_CLASS.WARRIOR)
        //    {
        //        bool usewhirlwind = true;
        //        foreach (Minion m in this.enemyMinions)
        //        {
        //            if (m.Hp == 1) usewhirlwind = false;
        //        }
        //        if (this.ownMinions.Count <= 3) usewhirlwind = false;

        //        if (usewhirlwind)
        //        {
        //            mana = EnemyPlaysACard(CardDB.cardName.whirlwind, mana, playAroundProb, pap2);
        //        }
        //    }

        //    if (!useAOE) return mana;

        //    if (enemyHeroStrtClass == TAG_CLASS.MAGE)
        //    {
        //        mana = EnemyPlaysACard(CardDB.cardName.flamestrike, mana, playAroundProb, pap2);
        //        mana = EnemyPlaysACard(CardDB.cardName.blizzard, mana, playAroundProb, pap2);
        //    }

        //    if (enemyHeroStrtClass == TAG_CLASS.HUNTER)
        //    {
        //        mana = EnemyPlaysACard(CardDB.cardName.unleashthehounds, mana, playAroundProb, pap2);
        //    }

        //    if (enemyHeroStrtClass == TAG_CLASS.PRIEST)
        //    {
        //        mana = EnemyPlaysACard(CardDB.cardName.holynova, mana, playAroundProb, pap2);
        //    }

        //    if (enemyHeroStrtClass == TAG_CLASS.SHAMAN)
        //    {
        //        mana = EnemyPlaysACard(CardDB.cardName.lightningstorm, mana, playAroundProb, pap2);
        //    }

        //    if (enemyHeroStrtClass == TAG_CLASS.PALADIN)
        //    {
        //        mana = EnemyPlaysACard(CardDB.cardName.consecration, mana, playAroundProb, pap2);
        //    }

        //    if (enemyHeroStrtClass == TAG_CLASS.DRUID)
        //    {
        //        mana = EnemyPlaysACard(CardDB.cardName.swipe, mana, playAroundProb, pap2);
        //    }



        //    return mana;
        //}

        //public int EnemyPlaysACard(CardDB.cardName cardname, int currmana, int playAroundProb, int pap2)
        //{

        //    //todo manacosts
        //    if (cardname == CardDB.cardName.flamestrike && currmana >= 7)
        //    {
        //        bool dontkill = false;
        //        int prob = Probabilitymaker.Instance.getProbOfEnemyHavingCardInHand(CardDB.cardIDEnum.CS2_032, this.enemyAnzCards, this.enemyDeckSize);
        //        if (playAroundProb > prob) return currmana;
        //        if (pap2 > prob) dontkill = true;

        //        List<Minion> temp = this.ownMinions;
        //        int damage = getEnemySpellDamageDamage(4);
        //        foreach (Minion enemy in temp)
        //        {
        //            enemy.cantLowerHPbelowONE = dontkill;
        //            this.minionGetDamageOrHeal(enemy, damage);
        //            enemy.cantLowerHPbelowONE = false;
        //        }

        //        currmana -= 7;
        //        return currmana;
        //    }

        //    if (cardname == CardDB.cardName.blizzard && currmana >= 6)
        //    {
        //        bool dontkill = false;
        //        int prob = Probabilitymaker.Instance.getProbOfEnemyHavingCardInHand(CardDB.cardIDEnum.CS2_028, this.enemyAnzCards, this.enemyDeckSize);
        //        if (playAroundProb > prob) return currmana;
        //        if (pap2 > prob) dontkill = true;

        //        List<Minion> temp = this.ownMinions;
        //        int damage = getEnemySpellDamageDamage(2);
        //        foreach (Minion enemy in temp)
        //        {
        //            enemy.frozen = true;
        //            enemy.cantLowerHPbelowONE = dontkill;
        //            this.minionGetDamageOrHeal(enemy, damage);
        //            enemy.cantLowerHPbelowONE = false;
        //        }

        //        currmana -= 6;
        //        return currmana;
        //    }


        //    if (cardname == CardDB.cardName.unleashthehounds && currmana >= 5)
        //    {
        //        bool dontkill = false;
        //        int prob = Probabilitymaker.Instance.getProbOfEnemyHavingCardInHand(CardDB.cardIDEnum.EX1_538, this.enemyAnzCards, this.enemyDeckSize);
        //        if (playAroundProb > prob) return currmana;
        //        if (pap2 > prob) dontkill = true;

        //        int anz = this.ownMinions.Count;
        //        int posi = this.enemyMinions.Count - 1;
        //        CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_538t);//hound
        //        for (int i = 0; i < anz; i++)
        //        {
        //            callKid(kid, posi, false);
        //        }
        //        currmana -= 5;
        //        return currmana;
        //    }





        //    if (cardname == CardDB.cardName.holynova && currmana >= 5)
        //    {
        //        bool dontkill = false;
        //        int prob = Probabilitymaker.Instance.getProbOfEnemyHavingCardInHand(CardDB.cardIDEnum.CS1_112, this.enemyAnzCards, this.enemyDeckSize);
        //        if (playAroundProb > prob) return currmana;
        //        if (pap2 > prob) dontkill = true;

        //        List<Minion> temp = this.enemyMinions;
        //        int heal = getEnemySpellHeal(2);
        //        int damage = getEnemySpellDamageDamage(2);
        //        foreach (Minion enemy in temp)
        //        {
        //            this.minionGetDamageOrHeal(enemy, -heal);
        //        }
        //        this.minionGetDamageOrHeal(this.enemyHero, -heal);
        //        temp = this.ownMinions;
        //        foreach (Minion enemy in temp)
        //        {
        //            enemy.cantLowerHPbelowONE = dontkill;
        //            this.minionGetDamageOrHeal(enemy, damage);
        //            enemy.cantLowerHPbelowONE = false;
        //        }
        //        this.minionGetDamageOrHeal(this.ownHero, damage);
        //        currmana -= 5;
        //        return currmana;
        //    }




        //    if (cardname == CardDB.cardName.lightningstorm && currmana >= 4)//3
        //    {
        //        bool dontkill = false;
        //        int prob = Probabilitymaker.Instance.getProbOfEnemyHavingCardInHand(CardDB.cardIDEnum.EX1_259, this.enemyAnzCards, this.enemyDeckSize);
        //        if (playAroundProb > prob) return currmana;
        //        if (pap2 > prob) dontkill = true;

        //        List<Minion> temp = this.ownMinions;
        //        int damage = getEnemySpellDamageDamage(3);
        //        foreach (Minion enemy in temp)
        //        {
        //            enemy.cantLowerHPbelowONE = dontkill;
        //            this.minionGetDamageOrHeal(enemy, damage);
        //            enemy.cantLowerHPbelowONE = false;
        //        }
        //        currmana -= 3;
        //        return currmana;
        //    }



        //    if (cardname == CardDB.cardName.whirlwind && currmana >= 3)//1
        //    {
        //        bool dontkill = false;
        //        int prob = Probabilitymaker.Instance.getProbOfEnemyHavingCardInHand(CardDB.cardIDEnum.EX1_400, this.enemyAnzCards, this.enemyDeckSize);
        //        if (playAroundProb > prob) return currmana;
        //        if (pap2 > prob) dontkill = true;

        //        List<Minion> temp = this.enemyMinions;
        //        int damage = getEnemySpellDamageDamage(1);
        //        foreach (Minion enemy in temp)
        //        {
        //            this.minionGetDamageOrHeal(enemy, damage);
        //        }
        //        temp = this.ownMinions;
        //        foreach (Minion enemy in temp)
        //        {
        //            enemy.cantLowerHPbelowONE = dontkill;
        //            this.minionGetDamageOrHeal(enemy, damage);
        //            enemy.cantLowerHPbelowONE = false;
        //        }
        //        currmana -= 1;
        //        return currmana;
        //    }



        //    if (cardname == CardDB.cardName.consecration && currmana >= 4)
        //    {
        //        bool dontkill = false;
        //        int prob = Probabilitymaker.Instance.getProbOfEnemyHavingCardInHand(CardDB.cardIDEnum.CS2_093, this.enemyAnzCards, this.enemyDeckSize);
        //        if (playAroundProb > prob) return currmana;
        //        if (pap2 > prob) dontkill = true;

        //        List<Minion> temp = this.ownMinions;
        //        int damage = getEnemySpellDamageDamage(2);
        //        foreach (Minion enemy in temp)
        //        {
        //            enemy.cantLowerHPbelowONE = dontkill;
        //            this.minionGetDamageOrHeal(enemy, damage);
        //            enemy.cantLowerHPbelowONE = false;
        //        }

        //        this.minionGetDamageOrHeal(this.ownHero, damage);
        //        currmana -= 4;
        //        return currmana;
        //    }



        //    if (cardname == CardDB.cardName.swipe && currmana >= 4)
        //    {
        //        bool dontkill = false;
        //        int prob = Probabilitymaker.Instance.getProbOfEnemyHavingCardInHand(CardDB.cardIDEnum.CS2_012, this.enemyAnzCards, this.enemyDeckSize);
        //        if (playAroundProb > prob) return currmana;
        //        if (pap2 > prob) dontkill = true;

        //        int damage = getEnemySpellDamageDamage(4);
        //        // all others get 1 spelldamage
        //        int damage1 = getEnemySpellDamageDamage(1);

        //        List<Minion> temp = this.ownMinions;
        //        Minion target = null;
        //        foreach (Minion mnn in temp)
        //        {
        //            if (mnn.Hp <= damage || mnn.handcard.card.isSpecialMinion || target == null)
        //            {
        //                target = mnn;
        //            }
        //        }
        //        foreach (Minion mnn in temp.ToArray())
        //        {
        //            if (mnn.entitiyID == target.entitiyID)
        //            {
        //                mnn.cantLowerHPbelowONE = dontkill;
        //                this.minionGetDamageOrHeal(mnn, damage1);
        //                mnn.cantLowerHPbelowONE = false;
        //            }
        //            else
        //            {
        //                mnn.cantLowerHPbelowONE = dontkill;
        //                this.minionGetDamageOrHeal(mnn, damage);
        //                mnn.cantLowerHPbelowONE = false;
        //            }
        //        }
        //        currmana -= 4;
        //        return currmana;
        //    }





        //    return currmana;
        //}

        public int getNextEntity()
        {
            //i dont trust return this.nextEntity++; !!!
            int retval = this.nextEntity;
            this.nextEntity++;
            return retval;
        }

        public int getGameResult()
        {
            if (playerFirst.ownHero.Hp <= 0)
            {
                return 1;
            }
            else if (playerSecond.ownHero.Hp <= 0)
            {
                return 0;
            }
            return -1;
        }

        // get all minions which are attackable
        public List<Minion> getAttackTargets(bool own, bool isLethalCheck)
        {
            List<Minion> trgts = new List<Minion>();
            List<Minion> trgts2 = new List<Minion>();

            List<Minion> temp = (own) ? this.playerSecond.ownMinions : this.playerFirst.ownMinions;
            bool hasTaunts = false;
            foreach (Minion m in temp)
            {
                if (m.stealth) continue; // cant target stealth

                if (m.taunt)
                {
                    hasTaunts = true;
                    trgts.Add(m);
                }
                else
                {
                    trgts2.Add(m);
                }
            }
            if (hasTaunts) return trgts;

            if (isLethalCheck) trgts2.Clear(); // only target enemy hero during Lethal check!

            if (own && !this.playerFirst.ownHero.immune) trgts2.Add(this.playerSecond.ownHero);
            else if (!own && !this.playerFirst.ownHero.immune) trgts2.Add(this.playerFirst.ownHero);
            return trgts2;
        }

        public int getBestPlace(CardDB.Card card, bool lethal, bool own)
        {
            Player mPlayer, ePlayer;
            if (own)
            {
                mPlayer = this.playerFirst;
                ePlayer = this.playerSecond;
            }
            else
            {
                mPlayer = this.playerSecond;
                ePlayer = this.playerFirst;
            }

            //we return the zonepos!
            if (card.type != CardDB.cardtype.MOB) return 1;
            if (mPlayer.ownMinions.Count == 0) return 1;
            if (mPlayer.ownMinions.Count == 1) return 2;

            int[] places = new int[mPlayer.ownMinions.Count];
            int i = 0;
            int tempval = 0;
            if (lethal && (card.name == CardDB.cardName.defenderofargus || card.name == CardDB.cardName.flametonguetotem || card.name == CardDB.cardName.direwolfalpha || card.name == CardDB.cardName.ancientmage))
            {
                i = 0;
                foreach (Minion m in mPlayer.ownMinions)
                {

                    places[i] = 0;
                    tempval = 0;
                    if (m.Ready)
                    {
                        tempval -= m.Angr - 1;
                        if (m.windfury) tempval -= m.Angr - 1;
                    }
                    places[i] = tempval;

                    i++;
                }


                i = 0;
                int bestpl = 7;
                int bestval = 10000;
                foreach (Minion m in mPlayer.ownMinions)
                {
                    int prev = 0;
                    int next = 0;
                    if (i >= 1) prev = places[i - 1];
                    next = places[i];
                    if (bestval > prev + next)
                    {
                        bestval = prev + next;
                        bestpl = i;
                    }
                    i++;
                }
                return bestpl + 1;
            }
            if (card.name == CardDB.cardName.sunfuryprotector || card.name == CardDB.cardName.defenderofargus) // bestplace, if right and left minions have no taunt + lots of hp, dont make priority-minions to taunt
            {
                if (lethal) return 1;

                i = 0;
                foreach (Minion m in mPlayer.ownMinions)
                {

                    places[i] = 0;
                    tempval = 0;
                    if (!m.taunt)
                    {
                        tempval -= m.Hp;
                    }
                    else
                    {
                        tempval -= m.Hp - 2;
                    }

                    if (m.windfury)
                    {
                        tempval += 2;
                    }

                    tempval += CardDB.Instance.getValueOfUsefulNeedKeepPriority(m.handcard.card.name);
                    places[i] = tempval;
                    i++;
                }


                i = 0;
                int bestpl = 7;
                int bestval = 10000;
                foreach (Minion m in mPlayer.ownMinions)
                {
                    int prev = 0;
                    int next = 0;
                    if (i >= 1) prev = places[i - 1];
                    next = places[i];
                    if (bestval > prev + next)
                    {
                        bestval = prev + next;
                        bestpl = i;
                    }
                    i++;
                }
                return bestpl + 1;
            }

            int cardIsBuffer = 0;
            bool placebuff = false;
            if (card.name == CardDB.cardName.flametonguetotem || card.name == CardDB.cardName.direwolfalpha)
            {
                placebuff = true;
                if (card.name == CardDB.cardName.flametonguetotem) cardIsBuffer = 2;
                if (card.name == CardDB.cardName.direwolfalpha) cardIsBuffer = 1;
            }
            bool commander = false;
            foreach (Minion m in mPlayer.ownMinions)
            {
                if (m.handcard.card.name == CardDB.cardName.warsongcommander) commander = true;
                if (m.handcard.card.name == CardDB.cardName.flametonguetotem || m.handcard.card.name == CardDB.cardName.direwolfalpha) placebuff = true;
            }
            //attackmaxing :D
            if (placebuff)
            {


                int cval = 0;
                if (card.Charge || (card.Attack <= 3 && commander))
                {
                    cval = card.Attack;
                    if (card.windfury) cval = card.Attack;
                }
                if (card.name == CardDB.cardName.nerubianegg)
                {
                    cval += 1;
                }
                cval++;
                i = 0;
                int[] buffplaces = new int[mPlayer.ownMinions.Count];
                int[] whirlwindplaces = new int[mPlayer.ownMinions.Count];
                int gesval = 0;
                int minionsBefore = 0;
                foreach (Minion m in mPlayer.ownMinions)
                {
                    buffplaces[i] = 0;
                    whirlwindplaces[i] = 1;
                    places[i] = 0;
                    tempval = -1;
                    if (!m.Ready && m.Angr == 0 && !m.playedThisTurn) tempval = 0;
                    if (m.Ready)
                    {
                        tempval = m.Angr;
                        if (m.windfury && m.numAttacksThisTurn == 0)
                        {
                            tempval += m.Angr;
                            whirlwindplaces[i] = 2;
                        }


                    }
                    if (m.handcard.card.name == CardDB.cardName.flametonguetotem)
                    {
                        buffplaces[i] = 2;
                        minionsBefore = i;
                    }
                    if (m.handcard.card.name == CardDB.cardName.direwolfalpha)
                    {
                        buffplaces[i] = 1;
                        minionsBefore = i;
                    }
                    tempval++;
                    places[i] = tempval;
                    gesval += tempval;
                    i++;
                }
                //gesval = whole possible damage
                int bplace = 0;
                int bvale = 0;
                int somebplace = 0;
                int somebvale = 0;
                tempval = 0;
                i = 0;
                for (int j = 0; j <= mPlayer.ownMinions.Count; j++)
                {
                    tempval = gesval;
                    int current = cval;
                    int prev = 0;
                    int next = 0;
                    if (i >= 1)
                    {
                        tempval -= places[i - 1];
                        prev = places[i - 1];
                        if (prev >= 0) prev += whirlwindplaces[i - 1] * cardIsBuffer;
                        if (current > 0) current += buffplaces[i - 1];

                        if (i < mPlayer.ownMinions.Count)
                        {
                            prev -= whirlwindplaces[i - 1] * buffplaces[i];
                        }
                    }
                    if (i < mPlayer.ownMinions.Count)
                    {
                        tempval -= places[i];
                        next = places[i];
                        if (next >= 0) next += whirlwindplaces[i] * cardIsBuffer;
                        if (current > 0) current += buffplaces[i];
                        if (i >= 1)
                        {
                            next -= whirlwindplaces[i] * buffplaces[i - 1];
                        }
                    }
                    tempval += current + prev + next;
                    if (tempval > bvale)
                    {
                        bplace = i;
                        bvale = tempval;
                        somebplace = i;
                        somebvale = tempval;
                    }
                    else if (tempval == somebvale)
                    {
                        somebplace = i;
                    }

                    i++;
                }
                if ((bplace != somebplace) && (minionsBefore > mPlayer.ownMinions.Count - somebplace)) bplace = somebplace;
                return bplace + 1;

            }

            // normal placement
            int cardvalue = card.Attack * 2 + card.Health;
            if (card.tank)
            {
                cardvalue += 5;
                cardvalue += card.Health;
            }

            cardvalue += CardDB.Instance.getValueOfUsefulNeedKeepPriority(card.name); //  add a custom placement rules (replace it)
            cardvalue += 1;

            i = 0;
            foreach (Minion m in mPlayer.ownMinions)
            {
                places[i] = 0;
                tempval = m.Angr * 2 + m.maxHp;
                if (m.taunt)
                {
                    tempval += 6;
                    tempval += m.maxHp;
                }
                if (!m.silenced)
                {
                    tempval += CardDB.Instance.getValueOfUsefulNeedKeepPriority(m.handcard.card.name);
                    if (m.stealth) tempval += 40;
                }
                places[i] = tempval;

                i++;
            }

            //bigminion if >=10
            int bestplace = 0;
            int bestvale = 0;
            tempval = 0;
            i = 0;
            for (int j = 0; j <= mPlayer.ownMinions.Count; j++)
            {
                int prev = cardvalue;
                int next = cardvalue;
                if (i >= 1) prev = places[i - 1];
                if (i < mPlayer.ownMinions.Count) next = places[i];


                if (cardvalue >= prev && cardvalue >= next)
                {
                    tempval = 2 * cardvalue - prev - next;
                    if (tempval > bestvale)
                    {
                        bestplace = i;
                        bestvale = tempval;
                    }
                }
                if (cardvalue <= prev && cardvalue <= next)
                {
                    tempval = -2 * cardvalue + prev + next;
                    if (tempval > bestvale)
                    {
                        bestplace = i;
                        bestvale = tempval;
                    }
                }

                i++;
            }

            return bestplace + 1;
        }

        public void guessHeroDamage()
        {
            Player mPlayer, ePlayer;

            if (this.isOwnTurn)
            {
                mPlayer = this.playerFirst;
                ePlayer = this.playerSecond;
            }
            else
            {
                mPlayer = this.playerSecond;
                ePlayer = this.playerFirst;
            }

            int ghd = 0;
            foreach (Minion m in ePlayer.ownMinions)
            {
                if (m.frozen) continue;
                if (m.name == CardDB.cardName.ancientwatcher && !m.silenced)
                {
                    continue;
                }
                ghd += m.Angr;
                if (m.windfury) ghd += m.Angr;
            }

            if (!ePlayer.ownHero.frozen)
            {
                if (ePlayer.ownWeaponAttack >= 1)
                {
                    ghd += ePlayer.ownWeaponAttack;
                    if (ePlayer.ownHero.windfury || ePlayer.ownWeaponName == CardDB.cardName.doomhammer) ghd += ePlayer.ownWeaponAttack;
                }
                else
                {
                    if (ePlayer.ownHeroName == HeroEnum.druid) ghd++;
                    if (ePlayer.ownHeroName == HeroEnum.thief) ghd++;
                }
            }

            if (ePlayer.ownHeroName == HeroEnum.mage) ghd++;
            if (ePlayer.ownHeroName == HeroEnum.hunter) ghd += 2;
            if (ePlayer.ownHeroName == HeroEnum.ragnarosthefirelord) ghd += 8;


            foreach (Minion m in mPlayer.ownMinions)
            {
                if (m.taunt) ghd -= m.Hp;
                if (m.taunt && m.divineshild) ghd -= 1;
            }

            int guessingHeroDamage = Math.Max(0, ghd);
            if (mPlayer.ownHero.immune) guessingHeroDamage = 0;
            this.guessingHeroHP = mPlayer.ownHero.Hp + mPlayer.ownHero.armor - guessingHeroDamage;
        }

        public void simulateTraps()
        {
            Player mPlayer, ePlayer;

            if (isOwnTurn)
            {
                mPlayer = this.playerFirst;
                ePlayer = this.playerSecond;
            }
            else
            {
                mPlayer = this.playerSecond;
                ePlayer = this.playerFirst;
            }

            //todo rework this
            // DONT KILL ENEMY HERO (cause its only guessing)
            foreach (CardDB.cardIDEnum secretID in mPlayer.ownSecretsIDList)
            {
                //hunter secrets############
                if (secretID == CardDB.cardIDEnum.EX1_554) //snaketrap
                {

                    //call 3 snakes (if possible)
                    int posi = mPlayer.ownMinions.Count - 1;
                    CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_554t);//snake
                    callKid(kid, posi, true);
                    callKid(kid, posi, true);
                    callKid(kid, posi, true);
                }
                if (secretID == CardDB.cardIDEnum.EX1_609) //snipe
                {
                    //kill weakest minion of enemy
                    List<Minion> temp = new List<Minion>(ePlayer.ownMinions);
                    temp.Sort((a, b) => a.Angr.CompareTo(b.Angr));//take the weakest
                    if (temp.Count == 0) continue;
                    Minion m = temp[0];
                    minionGetDamageOrHeal(m, 4);
                }
                if (secretID == CardDB.cardIDEnum.EX1_610) //explosive trap
                {
                    //take 2 damage to each enemy
                    List<Minion> temp = ePlayer.ownMinions;
                    foreach (Minion m in temp)
                    {
                        minionGetDamageOrHeal(m, 2);
                    }
                    attackEnemyHeroWithoutKill(2);
                }
                if (secretID == CardDB.cardIDEnum.EX1_611) //freezing trap
                {
                    //return weakest enemy minion to hand
                    List<Minion> temp = new List<Minion>(ePlayer.ownMinions);
                    temp.Sort((a, b) => a.Angr.CompareTo(b.Angr));//take the weakest
                    if (temp.Count == 0) continue;
                    Minion m = temp[0];
                    minionReturnToHand(m, false, 0);
                }
                if (secretID == CardDB.cardIDEnum.EX1_533) // missdirection
                {
                    // first damage to your hero is nulled -> lower guessingHeroDamage
                    List<Minion> temp = new List<Minion>(ePlayer.ownMinions);
                    temp.Sort((a, b) => -a.Angr.CompareTo(b.Angr));//take the strongest
                    if (temp.Count == 0) continue;
                    Minion m = temp[0];
                    m.Angr = 0;
                    this.evaluatePenality -= ePlayer.ownMinions.Count;// the more the enemy minions has on board, the more the posibility to destroy something other :D
                }

                //mage secrets############
                if (secretID == CardDB.cardIDEnum.EX1_287) //counterspell
                {
                    // what should we do?
                    this.evaluatePenality -= 8;
                }

                if (secretID == CardDB.cardIDEnum.EX1_289) //ice barrier
                {
                    mPlayer.ownHero.armor += 8;
                }

                if (secretID == CardDB.cardIDEnum.EX1_295) //ice block
                {
                    //set the guessed Damage to zero
                     mPlayer.ownHero.immune = true;
                }

                if (secretID == CardDB.cardIDEnum.EX1_294) //mirror entity
                {
                    //summon snake ( a weak minion)
                    int posi =  mPlayer.ownMinions.Count - 1;
                    CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_554t);//snake
                    callKid(kid, posi, true);
                }
                if (secretID == CardDB.cardIDEnum.tt_010) //spellbender
                {
                    //whut???
                    // add 2 to your defence (most attack-buffs give +2, lots of damage spells too)
                    this.evaluatePenality -= 4;
                }
                if (secretID == CardDB.cardIDEnum.EX1_594) // vaporize
                {
                    // first damage to your hero is nulled -> lower guessingHeroDamage and destroy weakest minion
                    List<Minion> temp = new List<Minion>(ePlayer.ownMinions);
                    temp.Sort((a, b) => a.Angr.CompareTo(b.Angr));//take the weakest
                    if (temp.Count == 0) continue;
                    Minion m = temp[0];
                    minionGetDestroyed(m);
                }
                if (secretID == CardDB.cardIDEnum.FP1_018) // duplicate
                {
                    // first damage to your hero is nulled -> lower guessingHeroDamage and destroy weakest minion
                    List<Minion> temp = new List<Minion>(mPlayer.ownMinions);
                    temp.Sort((a, b) => a.Angr.CompareTo(b.Angr));//take the weakest
                    if (temp.Count == 0) continue;
                    Minion m = temp[0];
                    drawACard(m.name, true, nopen:true);
                    drawACard(m.name, true, nopen:true);
                }

                //pala secrets############
                if (secretID == CardDB.cardIDEnum.EX1_132) // eye for an eye
                {
                    // enemy takes one damage
                    List<Minion> temp = new List<Minion>(ePlayer.ownMinions);
                    temp.Sort((a, b) => a.Angr.CompareTo(b.Angr));//take the weakest
                    if (temp.Count == 0) continue;
                    Minion m = temp[0];
                    attackEnemyHeroWithoutKill(m.Angr);
                }
                if (secretID == CardDB.cardIDEnum.EX1_130) // noble sacrifice
                {
                    //spawn a 2/1 taunt!
                    int posi =  mPlayer.ownMinions.Count - 1;
                    CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_130a);
                    callKid(kid, posi, true);

                }

                if (secretID == CardDB.cardIDEnum.EX1_136) // redemption
                {
                    // we give our weakest minion a divine shield :D
                    List<Minion> temp = new List<Minion>(mPlayer.ownMinions);
                    temp.Sort((a, b) => a.Hp.CompareTo(b.Hp));//take the weakest
                    if (temp.Count == 0) continue;
                    foreach (Minion m in temp)
                    {
                        if (m.divineshild) continue;
                        m.divineshild = true;
                        break;
                    }
                }

                if (secretID == CardDB.cardIDEnum.EX1_379) // repentance
                {
                    // set his current lowest hp minion to x/1
                    List<Minion> temp = new List<Minion>(ePlayer.ownMinions);
                    temp.Sort((a, b) => a.Hp.CompareTo(b.Hp));//take the weakest
                    if (temp.Count == 0) continue;
                    Minion m = temp[0];
                    m.Hp = 1;
                    m.maxHp = 1;
                }

                if (secretID == CardDB.cardIDEnum.FP1_020) // avenge
                {
                    // we give our weakest minion +3/+2 :D
                    List<Minion> temp = new List<Minion>( mPlayer.ownMinions);
                    temp.Sort((a, b) => a.Hp.CompareTo(b.Hp));//take the weakest
                    if (temp.Count == 0) continue;
                    foreach (Minion m in temp)
                    {
                        minionGetBuffed(m, 3, 2);
                        break;
                    }
                }
            }

            this.doDmgTriggers();
        }



        //old one, will be replaced soon
        public void endTurn(bool simulateTwoTurns, bool playaround, bool print = false, int pprob = 0, int pprob2 = 0)
        {
            //this.value = int.MinValue;
            //if (this.turnCounter == 0)
            //{
                //this.manaTurnEnd = this.mana;
            this.playerFirst.manaTurnEnd = this.playerFirst.mana;
            //}
            this.turnCounter++;
            //penalty for destroying combo

            //this.evaluatePenality += combobreaker.checkIfComboWasPlayed(this.playactions, this.ownWeaponName, this.ownHeroName);

            //if (this.complete) return;
            this.triggerEndTurn(this.isOwnTurn);
            this.isOwnTurn = !this.isOwnTurn;
            //this.triggerStartTurn(this.isOwnTurn);

            if (isOwnTurn)
            {
                this.playerFirst.optionsPlayedThisTurn = 0;
            }
            else
            {
                this.playerSecond.optionsPlayedThisTurn = 0;
            }

            //if (!this.isOwnTurn) simulateTraps();

            //if (!sEnemTurn)
            //{
            //    //guessHeroDamage();
            //    this.triggerEndTurn(false);
            //    this.triggerStartTurn(true);
            //    this.complete = true;
            //}
            //else
            //{
            //guessHeroDamage();
            /*
            if (this.guessingHeroHP >= 1)
            {
                //simulateEnemysTurn(simulateTwoTurns, playaround, print, pprob, pprob2);
                this.prepareNextTurn(this.isOwnTurn);

                if (this.turnCounter >= 2)
                    Ai.Instance.enemySecondTurnSim.simulateEnemysTurn(this, simulateTwoTurns, playaround, print, pprob, pprob2);
                else
                    Ai.Instance.enemyTurnSim.simulateEnemysTurn(this, simulateTwoTurns, playaround, print, pprob, pprob2);
            }
            this.complete = true;*/
            //}
            this.prepareNextTurn(this.isOwnTurn);
            this.triggerStartTurn(this.isOwnTurn);
        }

        //prepares the turn for the next player
        public void prepareNextTurn(bool own)
        {
            //call this after start turn trigger!

            Player mPlayer, ePlayer;
            if (own)
            {
                mPlayer = this.playerFirst;
                ePlayer = this.playerSecond;
            }
            else
            {
                mPlayer = this.playerSecond;
                ePlayer = this.playerFirst;
            }

            mPlayer.ownMaxMana = Math.Min(10, mPlayer.ownMaxMana + 1);
            mPlayer.mana = mPlayer.ownMaxMana - mPlayer.ueberladung;
            foreach (Minion m in mPlayer.ownMinions)
            {
                m.numAttacksThisTurn = 0;
                m.playedThisTurn = false;
                m.updateReadyness();

                if (m.concedal)
                {
                    m.concedal = false;
                    m.stealth = false;
                }

            }
            //unfreeze the enemy minions
            foreach (Minion m in ePlayer.ownMinions)
            {
                m.frozen = false;
            }
            ePlayer.ownHero.frozen = false;


            mPlayer.ownHero.Angr = mPlayer.ownWeaponAttack;
            mPlayer.ownHero.numAttacksThisTurn = 0;
            mPlayer.ownAbilityReady = true;
            mPlayer.ownHero.updateReadyness();
            mPlayer.cardsPlayedThisTurn = 0;
            mPlayer.mobsplayedThisTurn = 0;
            mPlayer.playedPreparation = false;
            mPlayer.playedmagierinderkirintor = false;
            mPlayer.optionsPlayedThisTurn = 0;
            mPlayer.owncarddraw = 0;
            //this.sEnemTurn = false;


            this.ueberladung = 0;
            //this.complete = false;

            //this.value = int.MinValue;
            if (this.diedMinions != null) this.diedMinions.Clear();//contains only the minions that died in this turn! //implementation

            //update all minions count
            //mPlayer.startedWithDamagedMinions = false;
            //foreach (Minion m in ePlayer.ownMinions)
            //{
            //    if (m.Hp < m.maxHp && m.Hp >= 1) ePlayer.startedWithDamagedMinions = true;
            //    break;
            //}

            //mPlayer.startedWithWinzigebeschwoererin = mPlayer.winzigebeschwoererin;
            //mPlayer.startedWithManagespenst = mPlayer.managespenst;
            //mPlayer.startedWithnerubarweblord = mPlayer.nerubarweblord;
            //mPlayer.startedWithsoeldnerDerVenture = mPlayer.soeldnerDerVenture;
            //mPlayer.startedWithbeschwoerungsportal = mPlayer.beschwoerungsportal;
            //mPlayer.anzOwnsorcerersapprenticeStarted = mPlayer.anzOwnsorcerersapprentice;
            //mPlayer.anzOwnMechwarperStarted = mPlayer.anzOwnMechwarper;

            //debugMinions();

            //foreach (Handmanager.Handcard h in mPlayer.owncards)
            //{
            //    h.manacost = h.card.cost + h.permCostChange;
            //}

        }

        //public void endEnemyTurn()
        //{
        //    this.triggerEndTurn(false);
        //    this.turnCounter++;
        //    this.isOwnTurn = true;
        //    this.triggerStartTurn(true);
        //    this.complete = true;
        //    //Ai.Instance.botBase.getPlayfieldValue(this);

        //}

        //spelldamage calculation---------------------------------------------------
        public int getSpellDamageDamage(int dmg, bool own)
        {
            Player mPlayer;
            if (own)
            {
                mPlayer = this.playerFirst;
            }
            else
            {
                mPlayer = this.playerSecond;
            }
            int retval = dmg;
            retval += mPlayer.spellpower;
            if (mPlayer.doublepriest >= 1) retval *= (2 * mPlayer.doublepriest);
            return retval;
        }

        public int getSpellHeal(int heal, bool own)
        {
            Player mPlayer;
            if (own)
            {
                mPlayer = this.playerFirst;
            }
            else
            {
                mPlayer = this.playerSecond;
            }
            int retval = heal;
            if (mPlayer.anzOwnAuchenaiSoulpriest >= 1)
            {
                retval *= -1;
                retval -= mPlayer.spellpower;
            }
            if (mPlayer.doublepriest >= 1) retval *= (2 * mPlayer.doublepriest);
            return retval;
        }


        public int getMinionHeal(int heal, bool own)
        {
            Player mPlayer;
            if (own)
            {
                mPlayer = this.playerFirst;
            }
            else
            {
                mPlayer = this.playerSecond;
            }
            return (mPlayer.anzOwnAuchenaiSoulpriest >= 1) ? -heal : heal;
        }

        //public int getEnemySpellDamageDamage(int dmg)
        //{
        //    int retval = dmg;
        //    retval += this.playerSecond.spellpower;
        //    if (this.playerSecond.doublepriest >= 1) retval *= (2 * this.playerSecond.doublepriest);
        //    return retval;
        //}

        //public int getEnemySpellHeal(int heal)
        //{
        //    int retval = heal;
        //    if (this.playerSecond.anzOwnAuchenaiSoulpriest >= 1)
        //    {
        //        retval *= -1;
        //        retval -= this.playerSecond.spellpower;
        //    }
        //    if (this.playerSecond.doublepriest >= 1) retval *= (2 * this.playerSecond.doublepriest);
        //    return retval;
        //}

        //public int getEnemyMinionHeal(int heal)
        //{
        //    return (this.playerSecond.anzOwnAuchenaiSoulpriest >= 1) ? -heal : heal;
        //}


        // do the action--------------------------------------------------------------

        public void doAction(Action aa)
        {

            Player mPlayer = getCurrentPlayer(true);
            
            //debugMinions();

            //CREATE NEW MINIONS (cant use a.target or a.own) (dont belong to this board)
            Minion trgt = null;
            Minion o = null;
            Handmanager.Handcard ha = null;
            if (aa.target != null) //TODO: target debug
            {
                foreach (Minion m in this.playerFirst.ownMinions)
                {
                    if (aa.target.entitiyID == m.entitiyID)
                    {
                        trgt = m;
                        break;
                    }
                }
                foreach (Minion m in this.playerSecond.ownMinions)
                {
                    if (aa.target.entitiyID == m.entitiyID)
                    {
                        trgt = m;
                        break;
                    }
                }
                if (aa.target.entitiyID == this.playerFirst.ownHero.entitiyID) trgt = this.playerFirst.ownHero;
                if (aa.target.entitiyID == this.playerSecond.ownHero.entitiyID) trgt = this.playerSecond.ownHero;
                if (trgt == null)
                {
                    int debug = 1; //null target debug
                }
            }
            if (aa.own != null)
            {
                foreach (Minion m in this.playerFirst.ownMinions)
                {
                    if (aa.own.entitiyID == m.entitiyID)
                    {
                        o = m;
                        break;
                    }
                }
                foreach (Minion m in this.playerSecond.ownMinions)
                {
                    if (aa.own.entitiyID == m.entitiyID)
                    {
                        o = m;
                        break;
                    }
                }
                if (aa.own.entitiyID == this.playerFirst.ownHero.entitiyID) o = this.playerFirst.ownHero;
                if (aa.own.entitiyID == this.playerSecond.ownHero.entitiyID) o = this.playerSecond.ownHero;
                if (o == null)
                {
                    int debug = 1; //null target debug
                }
            }

            if (aa.card != null)
            {
                foreach (Handmanager.Handcard hh in mPlayer.owncards)
                {
                    if (hh.entity == aa.card.entity)
                    {
                        ha = hh;
                        break;
                    }
                }
                if (aa.actionType == actionEnum.useHeroPower)
                {
                    ha = this.isOwnTurn ? this.playerFirst.ownHeroAblility : this.playerSecond.ownHeroAblility;
                }
                if (ha == null)
                {
                    HREngine.Bots.Playfield ppTest = new HREngine.Bots.Playfield((HREngine.Bots.Silverfish)GameManager.Instance.playerFirst);
                    int debug = 1; //null target debug
                }
            }
            // create and execute the action------------------------------------------------------------------------
            Action a = new Action(aa.actionType, ha, o, aa.place, trgt, aa.penalty, aa.druidchoice, aa.manaCost);



            //save the action if its our first turn
            mPlayer.playactions.Add(a);
            //if (this.isOwnTurn) this.playactions.Add(a);

            // its a minion attack--------------------------------
            if (a.actionType == actionEnum.attackWithMinion)
            {
                this.evaluatePenality += a.penalty;

                Minion target = a.target;
                //secret stuff
                int newTarget = this.secretTrigger_CharIsAttacked(a.own, target);

                if (newTarget >= 1)
                {
                    //search new target!
                    foreach (Minion m in this.playerFirst.ownMinions)
                    {
                        if (m.entitiyID == newTarget)
                        {
                            target = m;
                            break;
                        }
                    }
                    foreach (Minion m in this.playerSecond.ownMinions)
                    {
                        if (m.entitiyID == newTarget)
                        {
                            target = m;
                            break;
                        }
                    }
                    if (this.playerFirst.ownHero.entitiyID == newTarget) target = this.playerFirst.ownHero;
                    if (this.playerSecond.ownHero.entitiyID == newTarget) target = this.playerSecond.ownHero;
                    //Helpfunctions.Instance.ErrorLog("missdirection target = " + target.entitiyID);
                }
                if (a.own.Hp >= 1) minionAttacksMinion(a.own, target);
            }
            else
            {
                // its an hero attack--------------------------------
                if (a.actionType == actionEnum.attackWithHero)
                {
                    //secret trigger is inside
                    attackWithWeapon(a.own, a.target, a.penalty);
                }
                else
                {
                    // its an playing-card--------------------------------
                    if (a.actionType == actionEnum.playcard)
                    {
                        if (this.isOwnTurn)
                        {
                            playACard(a.card, a.target, a.place, a.druidchoice, a.penalty);
                        }
                        else
                        {
                            enemyplaysACard(a.card, a.target, a.place, a.druidchoice, a.penalty);
                        }
                    }
                    else
                    {
                        // its using the hero power--------------------------------
                        if (a.actionType == actionEnum.useHeroPower)
                        {
                            playHeroPower(a.target, a.penalty, this.isOwnTurn);
                        }
                    }
                }
            }

            mPlayer.optionsPlayedThisTurn++;

        }

        //minion attacks a minion

        //dontcount = betrayal effect!
        public void minionAttacksMinion(Minion attacker, Minion defender, bool dontcount = false)
        {

            if (attacker.isHero)
            {
                if (defender.isHero)
                {

                    defender.getDamageOrHeal(attacker.Angr, this, true, false);
                }
                else
                {

                    int enem_attack = defender.Angr;

                    defender.getDamageOrHeal(attacker.Angr, this, true, false);

                    if (!this.playerFirst.ownHero.immuneWhileAttacking)
                    {
                        int oldhp = attacker.Hp;
                        attacker.getDamageOrHeal(enem_attack, this, true, false);
                        if (!defender.silenced && oldhp > attacker.Hp)
                        {
                            if (defender.handcard.card.name == CardDB.cardName.waterelemental || defender.handcard.card.name == CardDB.cardName.snowchugger)
                            {
                                attacker.frozen = true;
                            }

                            this.triggerAMinionDealedDmg(defender, oldhp - attacker.Hp);
                        }
                    }
                }
                doDmgTriggers();
                return;
            }

            if (!dontcount)
            {
                attacker.numAttacksThisTurn++;
                attacker.stealth = false;
                if ((attacker.windfury && attacker.numAttacksThisTurn == 2) || !attacker.windfury)
                {
                    attacker.Ready = false;
                }

            }


            if (logging) Helpfunctions.Instance.logg(".attck with" + attacker.name + " A " + attacker.Angr + " H " + attacker.Hp);

            int attackerAngr = attacker.Angr;
            int defAngr = defender.Angr;

            //trigger attack ---------------------------
            this.triggerAMinionIsGoingToAttack(attacker);
            //------------------------------------------

            if (defender.isHero)//target is enemy hero
            {

                int oldhp = defender.Hp;
                defender.getDamageOrHeal(attacker.Angr, this, true, false);
                if (!attacker.silenced && oldhp > defender.Hp) // attacker did dmg
                {

                    if (attacker.handcard.card.name == CardDB.cardName.waterelemental || attacker.handcard.card.name == CardDB.cardName.snowchugger) defender.frozen = true;

                    this.triggerAMinionDealedDmg(attacker, oldhp - defender.Hp);
                }
                doDmgTriggers();
                return;
            }



            //defender gets dmg
            int oldHP = defender.Hp;
            defender.getDamageOrHeal(attackerAngr, this, true, false);
            if (!attacker.silenced && oldHP > defender.Hp && (attacker.handcard.card.name == CardDB.cardName.waterelemental || attacker.handcard.card.name == CardDB.cardName.snowchugger)) defender.frozen = true;
            bool defenderGotDmg = oldHP > defender.Hp;

            bool attackerGotDmg = false;

            //attacker gets dmg
            if (!dontcount)//betrayal effect :D
            {
                oldHP = attacker.Hp;
                attacker.getDamageOrHeal(defAngr, this, true, false);

                if (!defender.silenced && oldHP > attacker.Hp)
                {
                    if (defender.handcard.card.name == CardDB.cardName.waterelemental || defender.handcard.card.name == CardDB.cardName.snowchugger) attacker.frozen = true;

                    this.triggerAMinionDealedDmg(defender, oldHP - attacker.Hp);
                }
                attackerGotDmg = oldHP > attacker.Hp;
            }


            //trigger poisonous effect of attacker + defender (even if they died due to attacking/defending)

            if (defenderGotDmg && !attacker.silenced && attacker.handcard.card.poisionous && !defender.isHero)
            {
                minionGetDestroyed(defender);
            }

            if (attackerGotDmg && !defender.silenced && defender.handcard.card.poisionous && !attacker.isHero)
            {
                minionGetDestroyed(attacker);
            }

            if (!dontcount) //foe reaper reaps!
            {
                if (attacker.name == CardDB.cardName.foereaper4000 && !attacker.silenced)
                {
                    List<Minion> temp = (attacker.own) ? this.playerSecond.ownMinions : this.playerFirst.ownMinions;
                    foreach (Minion mnn in temp.ToList())
                    {
                        if (mnn.zonepos + 1 == defender.zonepos || mnn.zonepos - 1 == defender.zonepos)
                        {
                            this.minionAttacksMinion(attacker, mnn, true);
                        }
                    }
                }
            }

            this.doDmgTriggers();



        }

        //a hero attacks a minion
        public void attackWithWeapon(Minion hero, Minion target, int penality)
        {
            bool own = hero.own;
            if (own)
            {
                this.playerFirst.attacked = true;
                this.evaluatePenality += penality;
            }
            else
            {
                this.playerSecond.attacked = true;
                this.evaluatePenality += penality;
            }
            //this.attacked = true;
            hero.numAttacksThisTurn++;

            //hero will end his readyness
            hero.updateReadyness();

            //heal whether truesilverchampion equipped
            if (own)
            {
                if (this.playerFirst.ownWeaponName == CardDB.cardName.truesilverchampion)
                {
                    int heal = this.getMinionHeal(2, own);//minionheal because it's ignoring spellpower
                    this.minionGetDamageOrHeal(hero, -heal);
                    doDmgTriggers();
                }
            }
            else
            {
                if (this.playerSecond.ownWeaponName == CardDB.cardName.truesilverchampion)
                {
                    int heal = this.getMinionHeal(2, own);
                    this.minionGetDamageOrHeal(hero, -heal);
                    doDmgTriggers();
                }
            }

            if (logging) Helpfunctions.Instance.logg("attck with weapon trgt: " + target.entitiyID);

            // hero attacks enemy----------------------------------------------------------------------------------

            if (target.isHero)// trigger secret and change target if necessary
            {
                int newTarget = this.secretTrigger_CharIsAttacked(hero, target);
                if (newTarget >= 1)
                {
                    //search new target!
                    foreach (Minion m in this.playerFirst.ownMinions)
                    {
                        if (m.entitiyID == newTarget)
                        {
                            target = m;
                            break;
                        }
                    }
                    foreach (Minion m in this.playerSecond.ownMinions)
                    {
                        if (m.entitiyID == newTarget)
                        {
                            target = m;
                            break;
                        }
                    }
                    if (this.playerFirst.ownHero.entitiyID == newTarget) target = this.playerFirst.ownHero;
                    if (this.playerSecond.ownHero.entitiyID == newTarget) target = this.playerSecond.ownHero;
                }

            }
            this.minionAttacksMinion(hero, target);
            //-----------------------------------------------------------------------------------------------------

            //gorehowl is not killed if he attacks minions
            if (own)
            {
                if (playerFirst.ownWeaponName == CardDB.cardName.gorehowl && !target.isHero)
                {
                    this.playerFirst.ownWeaponAttack--;
                    hero.Angr--;
                }
                else
                {
                    this.lowerWeaponDurability(1, true);
                }
            }
            else
            {
                if (playerSecond.ownWeaponName == CardDB.cardName.gorehowl && !target.isHero)
                {
                    this.playerSecond.ownWeaponAttack--;
                    hero.Angr--;
                }
                else
                {
                    this.lowerWeaponDurability(1, false);
                }
            }

        }

        public void deleteTargetFromList(Minion m)
        { 
            
        }

        public void deleteMinionFromMoveList(Minion m, int targetId)
        {
            Player mPlayer;

            if (this.isOwnTurn)
            {
                mPlayer = this.playerFirst;
            }
            else
            {
                mPlayer = this.playerSecond;
            }

            //if ()
        
        }

        public void deleteHcFromMoveList(Handmanager.Handcard hc, int targetId)
        { 
            //Need implementation for shadowmadness
            Player mPlayer;

            if (this.isOwnTurn)
            {
                mPlayer = this.playerFirst;
            }
            else
            {
                mPlayer = this.playerSecond;
            }
            if (targetId != -1)
            {
                List<Minion> trgts = hc.card.getTargetsForCard(this, false, this.isOwnTurn);
                foreach (Minion m in trgts)
                {
                    Tuple<int, int> key = new Tuple<int, int>(hc.entity, m.entitiyID);
                    moveList.RemoveAt(moveMap[key]);
                    moveMap.Remove(key);
                }
            }
            else
            {
                Tuple<int, int> key = new Tuple<int, int>(hc.entity, -1);
                moveList.RemoveAt(moveMap[key]);
                moveMap.Remove(key);
            }
        }

        public void printMoveList()
        {
            Helpfunctions.Instance.logg("Current Move List:------------------------------------");
            foreach (Action move in this.moveList)
            {
                move.print();
            }
        
        }

        //public void updateMoveList(Action action)
        //{
        //    Helpfunctions.Instance.logg("Action:------------------------------------");
        //    action.print();
        //    printMoveList();
        //    if (this.moveTrigger.tauntChanged || this.moveTrigger.manaChanged)
        //    {
        //        this.moveList = Movegenerator.Instance.getMoveList(this, false, true, true);
        //    }
        //    else
        //    {
        //        Movegenerator.Instance.getMoveListForPlayfield(this, false);
        //    }
        //    if (this.isOwnTurn)
        //    {
        //        Helpfunctions.Instance.logg("player 1 Mana: " + this.playerFirst.mana + "/" + this.playerFirst.ownMaxMana);
        //    }
        //    else
        //    {
        //        Helpfunctions.Instance.logg("player 2 Mana: " + this.playerSecond.mana + "/" + this.playerSecond.ownMaxMana);
        //    }
        //    printMoveList();
        //}

        //play a minion trigger stuff:
        // 1 whenever you play a card whatever triggers
        // 2 Auras
        // 5 whenever you summon a minion triggers (like starving buzzard)
        // 3 battlecry
        // 3.5 place minion
        // 3.75 dmg/died/dthrttl triggers
        // 4 secret: minion is played
        // 4.5 dmg/died/dthrttl triggers
        // 5 after you summon a minion triggers
        // 5.5 dmg/died/dthrttl triggers
        public void playACard(Handmanager.Handcard hc, Minion target, int position, int choice, int penality)
        {
            CardDB.Card c = hc.card;
            this.evaluatePenality += penality;
            int manaCost = hc.getManaCost(this, true);
            this.playerFirst.mana = this.playerFirst.mana - manaCost;

            this.moveTrigger.manaSpent = manaCost;

            removeCard(hc, true);// remove card from hand

            this.triggerCardsChanged(true);

            if (c.type == CardDB.cardtype.SPELL)
            {
                this.playerFirst.playedPreparation = false;
                if (target != null && target.name == CardDB.cardName.dragonkinsorcerer && target.own) this.minionGetBuffed(target, 1, 1);
                //TODO: Coin here
            }

            if (c.Secret)
            {
                this.playerFirst.ownSecretsIDList.Add(c.cardIDenum);
                this.playerFirst.playedmagierinderkirintor = false;
            }


            //Helpfunctions.Instance.logg("play crd " + c.name + " entitiy# " + cardEntity + " mana " + hc.getManaCost(this) + " trgt " + target);
            if (logging) Helpfunctions.Instance.logg("play crd " + c.name + " entitiy# " + hc.entity + " mana " + hc.getManaCost(this, true) + " trgt " + target);


            this.triggerACardWillBePlayed(hc, true);
            this.triggerCardsChanged(true);
            this.doDmgTriggers();

            int newTarget = secretTrigger_SpellIsPlayed(target, c.type == CardDB.cardtype.SPELL, true);
            if (newTarget >= 1)
            {
                //search new target!
                foreach (Minion m in this.playerFirst.ownMinions)
                {
                    if (m.entitiyID == newTarget)
                    {
                        target = m;
                        break;
                    }
                }
                foreach (Minion m in this.playerSecond.ownMinions)
                {
                    if (m.entitiyID == newTarget)
                    {
                        target = m;
                        break;
                    }
                }
                if (this.playerFirst.ownHero.entitiyID == newTarget) target = this.playerFirst.ownHero;
                if (this.playerSecond.ownHero.entitiyID == newTarget) target = this.playerSecond.ownHero;
            }
            if (newTarget != -2) // trigger spell-secrets!
            {

                if (c.type == CardDB.cardtype.MOB)
                {
                    this.placeAmobSomewhere(hc, true, target, choice, position);
                    this.playerFirst.mobsplayedThisTurn++;                             
                }
                else
                {

                    c.sim_card.onCardPlay(this, true, target, choice);
                    this.doDmgTriggers();
                    //secret trigger? do here
                }
            }

            GraveYardItem gyi = new GraveYardItem(c.cardIDenum, hc.entity, true);
            graveYard.Add(gyi);
            //this.ueberladung += c.recallValue;
            this.playerFirst.cardsPlayedThisTurn++;

        }

        public void enemyplaysACard(Handmanager.Handcard hc, Minion target, int position, int choice, int penality)
        {
            //Handmanager.Handcard hc = new Handmanager.Handcard(c);
            //hc.entity = this.getNextEntity();
            //Helpfunctions.Instance.logg("play crd " + c.name + " entitiy# " + cardEntity + " mana " + hc.getManaCost(this) + " trgt " + target);
            CardDB.Card c = hc.card;
            if (logging) Helpfunctions.Instance.logg("enemy play crd " + c.name + " trgt " + target);
            this.evaluatePenality += penality;
            int manaCost = hc.getManaCost(this, false);

            this.moveTrigger.manaSpent = manaCost;

            this.playerSecond.mana = this.playerSecond.mana - manaCost;
            removeCard(hc, false);// remove card from hand

            this.triggerACardWillBePlayed(hc, false);
            this.triggerCardsChanged(false);
            this.doDmgTriggers();

            int newTarget = secretTrigger_SpellIsPlayed(target, c.type == CardDB.cardtype.SPELL, false);
            if (newTarget >= 1)
            {
                //search new target!
                foreach (Minion m in this.playerFirst.ownMinions)
                {
                    if (m.entitiyID == newTarget)
                    {
                        target = m;
                        break;
                    }
                }
                foreach (Minion m in this.playerSecond.ownMinions)
                {
                    if (m.entitiyID == newTarget)
                    {
                        target = m;
                        break;
                    }
                }
                if (this.playerFirst.ownHero.entitiyID == newTarget) target = this.playerFirst.ownHero;
                if (this.playerSecond.ownHero.entitiyID == newTarget) target = this.playerSecond.ownHero;
            }
            if (newTarget != -2) // trigger spell-secrets!
            {
                if (c.type == CardDB.cardtype.MOB)
                {
                    //todo mob playing
                    this.placeAmobSomewhere(hc, false, target, choice, position);
                    this.playerSecond.mobsplayedThisTurn++;                  
                }
                else
                {
                    c.sim_card.onCardPlay(this, false, target, choice);
                    this.doDmgTriggers();
                    //secret trigger? do here
                }
            }

            GraveYardItem gyi = new GraveYardItem(c.cardIDenum, hc.entity, false);
            graveYard.Add(gyi);
            this.playerSecond.cardsPlayedThisTurn++;

        }


        public void playHeroPower(Minion target, int penality, bool ownturn)
        {

            CardDB.Card c = (ownturn) ? this.playerFirst.ownHeroAblility.card : this.playerSecond.ownHeroAblility.card;

            if (ownturn) this.playerFirst.ownAbilityReady = false;
            else this.playerSecond.ownAbilityReady = false;

            this.evaluatePenality += penality;

            if (ownturn)
            {
                this.playerFirst.mana -= 2;
            }
            else
            {
                this.playerSecond.mana -= 2;
            }

            //Helpfunctions.Instance.logg("play crd " + c.name + " entitiy# " + cardEntity + " mana " + hc.getManaCost(this) + " trgt " + target);
            if (logging) Helpfunctions.Instance.logg("play crd " + c.name + " trgt " + target);

            c.sim_card.onCardPlay(this, ownturn, target, 0);
            this.doDmgTriggers();
        }


        //lower durability of weapon + destroy them (deathrattle) 
        //todo: test death's bite's dearthrattle
        public void lowerWeaponDurability(int value, bool own)
        {
            Player mPlayer, ePlayer;
            if (own)
            {
                mPlayer = this.playerFirst;
                ePlayer = this.playerSecond;
            }
            else
            {
                mPlayer = this.playerSecond;
                ePlayer = this.playerFirst;
            }


            if (mPlayer.ownWeaponDurability <= 0) return;
            mPlayer.ownWeaponDurability -= value;
            if (mPlayer.ownWeaponDurability <= 0)
            {
                //todo deathrattle deathsbite

                if (mPlayer.ownWeaponName == CardDB.cardName.powermace && mPlayer.ownMinions.Count >= 1)
                {
                    int sum = 1000;
                    Minion t = null;

                    foreach (Minion m in mPlayer.ownMinions)
                    {
                        if ((TAG_RACE)m.handcard.card.race == TAG_RACE.MECHANICAL)
                        {
                            int s = m.maxHp + m.Angr;
                            if (s < sum)
                            {
                                t = m;
                                sum = s;
                            }
                        }

                    }

                    if (t != null && sum < 999)
                    {
                        this.minionGetBuffed(t, 2, 2);
                    }
                }

                if (mPlayer.ownWeaponName == CardDB.cardName.deathsbite)
                {
                    int anz = 1;
                    if (mPlayer.ownBaronRivendare >= 1) anz = 2;
                    int dmg = getSpellDamageDamage(1, own);
                    foreach (Minion m in mPlayer.ownMinions)
                    {
                        this.minionGetDamageOrHeal(m, anz * dmg);
                    }
                    foreach (Minion m in ePlayer.ownMinions)
                    {
                        this.minionGetDamageOrHeal(m, anz * dmg);
                    }
                    this.doDmgTriggers();

                }


                mPlayer.ownHero.Angr -= mPlayer.ownWeaponAttack;
                mPlayer.ownWeaponDurability = 0;
                mPlayer.ownWeaponAttack = 0;
                mPlayer.ownWeaponName = CardDB.cardName.unknown;
                mPlayer.ownHero.windfury = false;

                foreach (Minion m in mPlayer.ownMinions)
                {
                    if (m.playedThisTurn && m.name == CardDB.cardName.southseadeckhand)
                    {
                        m.charge--;
                        m.updateReadyness();
                    }
                }
                mPlayer.ownHero.updateReadyness();
            }
        }



        public void doDmgTriggers()
        {
            //we do the these trigger manualy (to less minions) (we could trigger them with m.handcard.card.sim_card.ontrigger...)
            if (this.tempTrigger.charsGotHealed >= 1)
            {
                triggerACharGotHealed();//possible effects: gain attack
                this.tempTrigger.charsGotHealed = 0;
            }

            if (this.tempTrigger.minionsGotHealed >= 1)
            {
                triggerAMinionGotHealed();//possible effects: draw card
                this.tempTrigger.minionsGotHealed = 0;
            }


            if (this.tempTrigger.ownMinionsGotDmg + this.tempTrigger.enemyMinionsGotDmg >= 1)
            {
                triggerAMinionGotDmg(); //possible effects: draw card, gain armor, gain attack
                this.tempTrigger.ownMinionsGotDmg = 0;
                this.tempTrigger.enemyMinionsGotDmg = 0;
                this.tempTrigger.ownHeroGotDmg = 0;
                this.tempTrigger.enemyHeroGotDmg = 0;
            }

            if (this.tempTrigger.ownMinionsDied + this.tempTrigger.enemyMinionsDied >= 1)
            {
                triggerAMinionDied(); //possible effects: draw card, gain attack + hp
                if (this.tempTrigger.ownMinionsDied >= 1) this.tempTrigger.ownMinionsChanged = true;
                if (this.tempTrigger.enemyMinionsDied >= 1) this.tempTrigger.enemyMininsChanged = true;
                this.tempTrigger.ownMinionsDied = 0;
                this.tempTrigger.enemyMinionsDied = 0;
                this.tempTrigger.ownBeastDied = 0;
                this.tempTrigger.enemyBeastDied = 0;
                this.tempTrigger.ownMurlocDied = 0;
                this.tempTrigger.enemyMurlocDied = 0;
                this.tempTrigger.ownMechanicDied = 0;
                this.tempTrigger.enemyMechanicDied = 0;
            }

            updateBoards();
            if (this.tempTrigger.charsGotHealed + this.tempTrigger.minionsGotHealed + this.tempTrigger.ownMinionsGotDmg + this.tempTrigger.enemyMinionsGotDmg + this.tempTrigger.ownMinionsDied + this.tempTrigger.enemyMinionsDied >= 1)
            {
                doDmgTriggers();
            }
        }

        public void triggerACharGotHealed()
        {
            foreach (Minion mnn in this.playerFirst.ownMinions)
            {
                if (mnn.silenced) continue;
                if (mnn.handcard.card.name == CardDB.cardName.lightwarden)
                {
                    minionGetBuffed(mnn, 2 * this.tempTrigger.charsGotHealed, 0);
                }
                if (mnn.handcard.card.name == CardDB.cardName.shadowboxer)
                {
                    for (int i = 0; i < this.tempTrigger.charsGotHealed; i++)
                    {
                        Minion t = this.searchRandomMinion(this.playerSecond.ownMinions, searchmode.searchHighestHP);
                        if (t != null)
                        {
                            this.minionGetDamageOrHeal(t, 1);
                        }
                        else
                        {
                            this.minionGetDamageOrHeal(this.playerSecond.ownHero, 1);
                        }
                    }
                }
            }
            foreach (Minion mnn in this.playerSecond.ownMinions)
            {
                if (mnn.silenced) continue;
                if (mnn.handcard.card.name == CardDB.cardName.lightwarden)
                {
                    minionGetBuffed(mnn, 2 * this.tempTrigger.charsGotHealed, 0);
                }
                if (mnn.handcard.card.name == CardDB.cardName.shadowboxer)
                {
                    for (int i = 0; i < this.tempTrigger.charsGotHealed; i++)
                    {
                        Minion t = this.searchRandomMinion(this.playerFirst.ownMinions, searchmode.searchHighestHP);
                        if (t != null)
                        {
                            this.minionGetDamageOrHeal(t, 1);
                        }
                        else
                        {
                            this.minionGetDamageOrHeal(this.playerFirst.ownHero, 1);
                        }
                    }
                }
            }
        }

        public void triggerAMinionGotHealed()
        {
            //also dead minions trigger this
            foreach (Minion mnn in this.playerFirst.ownMinions)
            {
                if (mnn.silenced) continue;
                if (mnn.handcard.card.name == CardDB.cardName.northshirecleric)
                {
                    for (int i = 0; i < this.tempTrigger.minionsGotHealed; i++)
                    {
                        this.drawACard(getArandomCardFromDeck(true), true);
                    }
                }
            }

            foreach (Minion mnn in this.playerSecond.ownMinions)
            {
                if (mnn.silenced) continue;
                if (mnn.handcard.card.name == CardDB.cardName.northshirecleric)
                {
                    for (int i = 0; i < this.tempTrigger.minionsGotHealed; i++)
                    {
                        this.drawACard(getArandomCardFromDeck(false), false);
                    }
                }
            }
        }

        public void triggerAMinionGotDmg()
        {
            foreach (Minion m in this.playerFirst.ownMinions.ToArray())
            {
                if (m.silenced) { m.anzGotDmg = 0; continue; }
                m.handcard.card.sim_card.onMinionGotDmgTrigger(this, m, true);
            }

            foreach (Minion m in this.playerSecond.ownMinions.ToArray())
            {
                if (m.silenced) { m.anzGotDmg = 0; continue; }
                m.handcard.card.sim_card.onMinionGotDmgTrigger(this, m, false);
            }
            this.playerFirst.ownHero.anzGotDmg = 0;
            this.playerSecond.ownHero.anzGotDmg = 0;
        }

        public void triggerAMinionDied()
        {
            this.playerFirst.ownMinionsDiedTurn += this.tempTrigger.ownMinionsDied;
            this.playerSecond.ownMinionsDiedTurn += this.tempTrigger.enemyMinionsDied;

            foreach (Minion m in this.playerFirst.ownMinions.ToArray())
            {
                if (m.silenced) continue;
                if (m.Hp <= 0) continue;
                m.handcard.card.sim_card.onMinionDiedTrigger(this, m, m); //onMinionDiedTrigger(Playfield p, Minion triggerEffectMinion, Minion diedMinion) where "diedMinion" - not implemented and is not currently needed
            }
            foreach (Minion m in this.playerSecond.ownMinions.ToArray())
            {
                if (m.silenced) continue;
                if (m.Hp <= 0) continue;
                m.handcard.card.sim_card.onMinionDiedTrigger(this, m, m);
            }

            foreach (Handmanager.Handcard hc in this.playerFirst.owncards)
            {
                if (hc.card.name == CardDB.cardName.bolvarfordragon) hc.addattack += this.tempTrigger.ownMinionsDied;
            }

        }

        public void triggerAMinionIsGoingToAttack(Minion m)
        {
            //todo trigger secret her too
            //blessing of wisdom (truesilver is located in attackWithWeapon(...))
            if (m.ownBlessingOfWisdom >= 1)
            {
                for (int i = 0; i < m.ownBlessingOfWisdom; i++)
                {
                    this.drawACard(CardDB.cardName.unknown, true);
                }
            }
            if (m.enemyBlessingOfWisdom >= 1)
            {
                for (int i = 0; i < m.enemyBlessingOfWisdom; i++)
                {
                    this.drawACard(CardDB.cardName.unknown, false);
                }
            }

        }

        public void triggerAMinionDealedDmg(Minion m, int dmgDone)
        {
            //only GVG_018 has such an trigger!
            if (m.name == CardDB.cardName.mistressofpain && dmgDone >= 1)
            {
                if (m.own)
                {
                    if (this.playerFirst.anzOwnAuchenaiSoulpriest >= 1) // you have a soulpriest? lol you die!!!
                    {
                        this.playerFirst.ownHero.Hp = 0;
                    }
                    else
                    {
                        this.minionGetDamageOrHeal(this.playerFirst.ownHero, -dmgDone);
                    }
                }
                else
                {
                    if (this.playerSecond.anzOwnAuchenaiSoulpriest >= 1) // you have a soulpriest? lol you die!!!
                    {
                        this.playerSecond.ownHero.Hp = 0;
                    }
                    else
                    {
                        this.minionGetDamageOrHeal(this.playerSecond.ownHero, -dmgDone);
                    }
                }
            }

        }

        public void triggerACardWillBePlayed(Handmanager.Handcard hc, bool own)
        {

            Player mPlayer, ePlayer;

            if (own)
            {
                mPlayer = playerFirst;
                ePlayer = playerSecond;
            }
            else
            {
                mPlayer = playerSecond;
                ePlayer = playerFirst;
            }

            //if (own)
            //{
            if (mPlayer.anzOwnDragonConsort > 0 && (TAG_RACE)hc.card.race == TAG_RACE.DRAGON) mPlayer.anzOwnDragonConsort = 0;
            int violetteacher = 0; //we count violetteacher to avoid copying ownminions
            int illidan = 0;
            int burly = 0;
            foreach (Minion m in mPlayer.ownMinions)
            {
                if (m.silenced) continue;

                if (m.name == CardDB.cardName.illidanstormrage)
                {
                    illidan++;
                    continue;
                }

                if (m.name == CardDB.cardName.violetteacher)
                {
                    if (hc.card.type == CardDB.cardtype.SPELL)
                    {
                        violetteacher++;
                    }
                    continue;
                }
                if (m.name == CardDB.cardName.hobgoblin)
                {
                    if (hc.card.type == CardDB.cardtype.MOB && hc.card.Attack == 1)
                    {
                        hc.addattack += 2;
                        hc.addHp += 2;
                    }
                    continue;
                }

                m.handcard.card.sim_card.onCardIsGoingToBePlayed(this, hc.card, own, m);
            }

            foreach (Minion m in ePlayer.ownMinions)
            {
                if (m.name == CardDB.cardName.troggzortheearthinator)
                {
                    burly++;
                }
                if (m.name == CardDB.cardName.felreaver)
                {
                    m.handcard.card.sim_card.onCardIsGoingToBePlayed(this, hc.card, own, m);
                }
            }

            for (int i = 0; i < violetteacher; i++)
            {
                int pos = mPlayer.ownMinions.Count;
                this.callKid(CardDB.Instance.teacherminion, pos, own);
            }

            for (int i = 0; i < illidan; i++)
            {
                int pos = mPlayer.ownMinions.Count;
                this.callKid(CardDB.Instance.illidanminion, pos, own);
            }

            for (int i = 0; i < burly; i++)//summon for enemy !
            {
                int pos = ePlayer.ownMinions.Count;
                this.callKid(CardDB.Instance.burlyrockjaw, pos, !own);
            }


            //}
            //else
            //{
            //    if (playerSecond.anzOwnDragonConsort > 0 && (TAG_RACE)hc.card.race == TAG_RACE.DRAGON) playerSecond.anzOwnDragonConsort = 0;
            //    int violetteacher = 0; //we count violetteacher to avoid copying ownminions
            //    int illidan = 0;
            //    int burly = 0;
            //    foreach (Minion m in this.playerSecond.ownMinions)
            //    {
            //        if (m.silenced) continue;
            //        if (!own && m.name == CardDB.cardName.illidanstormrage)
            //        {
            //            illidan++;
            //            continue;
            //        }
            //        if (!own && m.name == CardDB.cardName.violetteacher)
            //        {
            //            if (hc.card.type == CardDB.cardtype.SPELL)
            //            {
            //                violetteacher++;
            //            }
            //            continue;
            //        }
            //        if (!own && m.name == CardDB.cardName.hobgoblin)
            //        {
            //            if (hc.card.type == CardDB.cardtype.MOB && hc.card.Attack == 1)
            //            {
            //                hc.addattack += 2;
            //                hc.addHp += 2;
            //            }
            //            continue;
            //        }

            //        m.handcard.card.sim_card.onCardIsGoingToBePlayed(this, hc.card, own, m);
            //    }
            //    foreach (Minion m in this.playerFirst.ownMinions)
            //    {
            //        if (m.name == CardDB.cardName.troggzortheearthinator)
            //        {
            //            burly++;
            //        }
            //        if (m.name == CardDB.cardName.felreaver)
            //        {
            //            m.handcard.card.sim_card.onCardIsGoingToBePlayed(this, hc.card, own, m);
            //        }
            //    }
            //    for (int i = 0; i < violetteacher; i++)
            //    {
            //        int pos = this.playerSecond.ownMinions.Count;
            //        this.callKid(CardDB.Instance.teacherminion, pos, own);
            //    }
            //    for (int i = 0; i < illidan; i++)
            //    {
            //        int pos = this.playerSecond.ownMinions.Count;
            //        this.callKid(CardDB.Instance.illidanminion, pos, own);
            //    }

            //    for (int i = 0; i < burly; i++)//summon for us
            //    {
            //        int pos = this.playerFirst.ownMinions.Count;
            //        this.callKid(CardDB.Instance.burlyrockjaw, pos, own);
            //    }
            //}

        }

        // public void triggerACardWasPlayed(CardDB.Card c, bool own) {        }

        public void triggerAMinionIsSummoned(Minion m)
        {
            if (m.own)
            {
                foreach (Minion mnn in this.playerFirst.ownMinions)
                {
                    if (mnn.silenced) continue;
                    mnn.handcard.card.sim_card.onMinionIsSummoned(this, mnn, m);
                }

                foreach (Minion mnn in this.playerSecond.ownMinions)
                {
                    if (mnn.silenced) continue;
                    if (mnn.name == CardDB.cardName.murloctidecaller) mnn.handcard.card.sim_card.onMinionIsSummoned(this, mnn, m);
                }

                if (this.playerFirst.ownWeaponName == CardDB.cardName.swordofjustice)
                {
                    this.minionGetBuffed(m, 1, 1);
                    this.lowerWeaponDurability(1, true);
                }
            }
            else
            {
                foreach (Minion mnn in this.playerSecond.ownMinions)
                {
                    if (mnn.silenced) continue;
                    mnn.handcard.card.sim_card.onMinionIsSummoned(this, mnn, m);
                }

                foreach (Minion mnn in this.playerFirst.ownMinions)
                {
                    if (mnn.silenced) continue;
                    if (mnn.name == CardDB.cardName.murloctidecaller) mnn.handcard.card.sim_card.onMinionIsSummoned(this, mnn, m);
                }
                if (this.playerSecond.ownWeaponName == CardDB.cardName.swordofjustice)
                {
                    this.minionGetBuffed(m, 1, 1);
                    this.lowerWeaponDurability(1, false);
                }
            }

        }

        public void triggerAMinionWasSummoned(Minion mnn)
        {
            //implementation of cogmaster
            bool hasMech = false;

            if (mnn.own)
            {
                foreach (Minion m in this.playerFirst.ownMinions)
                {
                    if (mnn.name == CardDB.cardName.cogmaster) 
                    {
                        if ((TAG_RACE)m.handcard.card.race == TAG_RACE.MECHANICAL)
                        {
                            hasMech = true;
                        }
                    }
                    if (m.silenced) continue;
                    if (m.name == CardDB.cardName.knifejuggler)
                    {
                        m.handcard.card.sim_card.onMinionWasSummoned(this, m, mnn);
                    }
                }            
            }
            else
            {
                foreach (Minion m in this.playerSecond.ownMinions)
                {
                    if (mnn.name == CardDB.cardName.cogmaster)
                    {
                        if ((TAG_RACE)m.handcard.card.race == TAG_RACE.MECHANICAL)
                        {
                            hasMech = true;
                        }
                    }
                    if (m.silenced) continue;
                    if (m.name == CardDB.cardName.knifejuggler)
                    {
                        m.handcard.card.sim_card.onMinionWasSummoned(this, m, mnn);
                    }
                }
            }
            if (hasMech)
            {
                minionGetBuffed(mnn, 2, 0);
            }
        }

        public void triggerEndTurn(bool ownturn)
        {
            foreach (Minion m in this.playerFirst.ownMinions.ToArray())
            {
                if (!m.silenced)
                {
                    m.handcard.card.sim_card.onTurnEndsTrigger(this, m, ownturn);
                    this.doDmgTriggers();  //implementation              
                }
                if (ownturn && m.destroyOnOwnTurnEnd) this.minionGetDestroyed(m);
            }
            foreach (Minion m in this.playerSecond.ownMinions.ToArray())
            {
                if (!m.silenced)
                {
                    m.handcard.card.sim_card.onTurnEndsTrigger(this, m, ownturn);
                    this.doDmgTriggers();  //implementation
                }
                if (!ownturn && m.destroyOnEnemyTurnEnd) this.minionGetDestroyed(m);
            }

            this.doDmgTriggers();

            //shadowmadness
            if (this.shadowmadnessed >= 1)
            {
                List<Minion> ownm = (ownturn) ? this.playerFirst.ownMinions : this.playerSecond.ownMinions;
                foreach (Minion m in ownm.ToArray())
                {
                    if (m.shadowmadnessed)
                    {
                        m.shadowmadnessed = false;
                        this.minionGetControlled(m, !m.own, false);
                    }
                }
                this.shadowmadnessed = 0;
                updateBoards();
            }

            this.playerFirst.playedmagierinderkirintor = false;

            foreach (Minion m in this.playerFirst.ownMinions)
            {
                this.minionGetTempBuff(m, -m.tempAttack, 0);
                m.immune = false;
                m.cantLowerHPbelowONE = false;
            }
            foreach (Minion m in this.playerSecond.ownMinions)
            {
                this.minionGetTempBuff(m, -m.tempAttack, 0);
                m.immune = false;
                m.cantLowerHPbelowONE = false;
            }

            clearTempAura(ownturn);
        }

        public void clearTempAura(bool ownTurn)
        {
            //millhoust, loatheb
            if (ownTurn)
            {
                playerFirst.enemyLoatheb = 0;
                playerFirst.enemyMillhouse = 0;
                foreach (Handmanager.Handcard h in playerFirst.owncards)
                {
                    if (h.card.type == CardDB.cardtype.SPELL)
                        h.manacost = Math.Max(h.card.cost + h.permCostChange, 0);
                }
            }
            else
            {
                playerSecond.enemyLoatheb = 0;
                playerSecond.enemyMillhouse = 0;
                foreach (Handmanager.Handcard h in playerSecond.owncards)
                {
                    if (h.card.type == CardDB.cardtype.SPELL)
                        h.manacost = Math.Max(h.card.cost + h.permCostChange, 0);
                }
            }
        }

        public void triggerStartTurn(bool ownturn)
        {

            List<Minion> ownm = (ownturn) ? this.playerFirst.ownMinions : this.playerSecond.ownMinions;
            foreach (Minion m in ownm.ToArray())
            {
                m.playedThisTurn = false;
                m.numAttacksThisTurn = 0;
                m.updateReadyness();
                if (!m.silenced)
                {
                    m.handcard.card.sim_card.onTurnStartTrigger(this, m, ownturn);
                }
                if (ownturn && m.destroyOnOwnTurnStart) this.minionGetDestroyed(m);
                if (!ownturn && m.destroyOnEnemyTurnStart) this.minionGetDestroyed(m);
            }

            List<Minion> enemm = (ownturn) ? this.playerSecond.ownMinions : this.playerFirst.ownMinions;
            foreach (Minion m in enemm.ToArray())
            {
                if (!m.silenced)
                {
                    if (m.name == CardDB.cardName.micromachine) m.handcard.card.sim_card.onTurnStartTrigger(this, m, ownturn);
                }
                if (ownturn && m.destroyOnOwnTurnStart) this.minionGetDestroyed(m);
                if (!ownturn && m.destroyOnEnemyTurnStart) this.minionGetDestroyed(m);
            }

            this.doDmgTriggers();

            //this.drawACard(getArandomCardFromDeck(ownturn), ownturn, passive:true);
            //this.doDmgTriggers();


            //action debug
            this.moveList.Clear();
            this.keepCardList.Clear();
            this.moveTrigger.Clear();

            this.playerFirst.lastTurnActions.Clear();
            this.playerFirst.lastTurnActions.AddRange(this.playerFirst.playactions);
            this.playerFirst.lastTurnMacros.Clear();
            this.playerFirst.lastTurnMacros.AddRange(this.playerFirst.playMacros);
            this.playerFirst.playactions.Clear();
            this.playerFirst.playMacros.Clear();

            this.playerSecond.lastTurnActions.Clear();
            this.playerSecond.lastTurnActions.AddRange(this.playerSecond.playactions);
            this.playerSecond.lastTurnMacros.Clear();
            this.playerSecond.lastTurnMacros.AddRange(this.playerFirst.playMacros);
            this.playerSecond.playactions.Clear();
            this.playerSecond.playMacros.Clear();
            this.rngIndex = 0;
            this.lastTurnEvaluatePenality = this.evaluatePenality;
            this.evaluatePenality = 0;

        }

        //public void drawACardFromBucket(int bucketIndex)
        //{          
        //    List<CardDB.Card> deck;
        //    List<List<CardDB.Card>> bucket;

        //    if (isOwnTurn)
        //    {
        //        deck = homeDeck;
        //        //bucket = ownBucketList;
        //    }
        //    else
        //    {
        //        deck = awayDeck;
        //        //bucket = enemyBucketList;
        //    }

        //    List<CardDB.Card> cardList = bucket[bucketIndex];
        //    int cardIndex = GameManager.getRNG().Next(cardList.Count);
        //    CardDB.Card cardToDraw = cardList[cardIndex];
        //    deck.Remove(cardToDraw);
        //    cardList.RemoveAt(cardIndex);

        //    this.drawACard(cardToDraw.name, this.isOwnTurn, passive: true);
        //    this.doDmgTriggers();
        //}

        public void drawCardFromDrawList()
        {
            List<CardDB.Card> cardDrawList;

            if (isOwnTurn)
            {
                cardDrawList = ownCardsToDraw;
            }
            else
            {
                cardDrawList = enemyCardsToDraw;
            }

            if (cardDrawList.Count == 0) //out of cards, so fatigue, TODO: acrolyte of pain
            {
                this.drawACard(getArandomCardFromDeck(this.isOwnTurn), this.isOwnTurn, passive: false);
            }
            else
            {
                CardDB.Card card = cardDrawList[0];
                cardDrawList.RemoveAt(0);
                drawACard(card.name, isOwnTurn);
            }

        }

        public void addCardToDrawList(CardDB.Card card)
        {
            List<CardDB.Card> deck;
            List<CardDB.Card> cardDrawList;

            if (isOwnTurn)
            {
                deck = homeDeck;
                cardDrawList = ownCardsToDraw;
            }
            else
            {
                deck = awayDeck;
                cardDrawList = enemyCardsToDraw;
            }
            deck.Remove(card);
            ownCardsToDraw.Add(card);
        }

        public void drawTurnStartCard(CardDB.Card card)
        {
            List<CardDB.Card> deck;

            if (isOwnTurn)
            {
                deck = homeDeck;
            }
            else
            {
                deck = awayDeck;
            }
            deck.Remove(card);

            this.drawACard(card.name, this.isOwnTurn, passive: true);
            this.doDmgTriggers();
        }

        public void drawTurnStartCard()
        {
            this.drawACard(getArandomCardFromDeck(this.isOwnTurn), this.isOwnTurn, passive: true);
            this.doDmgTriggers();
        }

        public void triggerAHeroGotArmor(bool ownHero)
        {
            foreach (Minion m in ((ownHero) ? this.playerFirst.ownMinions : this.playerSecond.ownMinions))
            {
                if (m.name == CardDB.cardName.siegeengine)
                {
                    this.minionGetBuffed(m, 1, 0);
                }
            }
        }

        public void triggerCardsChanged(bool own)
        {
            if (own)
            {
                if (this.playerFirst.tempanzOwnCards >= 6 && this.playerFirst.owncards.Count <= 5)
                {
                    //delete effect of enemy Goblin Sapper
                    foreach (Minion m in this.playerSecond.ownMinions)
                    {
                        if (m.name == CardDB.cardName.goblinsapper && !m.silenced)
                        {
                            this.minionGetBuffed(m, -4, 0);
                        }
                    }
                }
                if (this.playerFirst.owncards.Count >= 6 && this.playerFirst.tempanzOwnCards <= 5)
                {
                    //add effect of enemy Goblin Sapper
                    foreach (Minion m in this.playerSecond.ownMinions)
                    {
                        if (m.name == CardDB.cardName.goblinsapper && !m.silenced)
                        {
                            this.minionGetBuffed(m, 4, 0);
                        }
                    }
                }

                this.playerFirst.tempanzOwnCards = this.playerFirst.owncards.Count;
            }
            else
            {
                if (this.playerSecond.tempanzOwnCards >= 6 && this.playerSecond.owncards.Count <= 5)
                {
                    //delete effect of own Goblin Sapper
                    foreach (Minion m in this.playerFirst.ownMinions)
                    {
                        if (m.name == CardDB.cardName.goblinsapper && !m.silenced)
                        {
                            this.minionGetBuffed(m, -4, 0);
                        }
                    }
                }
                if (this.playerSecond.owncards.Count >= 6 && this.playerSecond.tempanzOwnCards <= 5)
                {
                    //add effect of own Goblin Sapper
                    foreach (Minion m in this.playerFirst.ownMinions)
                    {
                        if (m.name == CardDB.cardName.goblinsapper && !m.silenced)
                        {
                            this.minionGetBuffed(m, 4, 0);
                        }
                    }
                }

                this.playerSecond.tempanzOwnCards = this.playerSecond.owncards.Count;
            }
        }



        public int secretTrigger_CharIsAttacked(Minion attacker, Minion defender)
        {
            int newTarget = 0;
            int triggered = 0;
            if (this.isOwnTurn && this.playerSecond.ownSecretsIDList.Count >= 1)
            {

                if (defender.isHero && !defender.own)
                {
                    foreach (CardDB.cardIDEnum secretID in this.playerSecond.ownSecretsIDList)
                    {
                        if (secretID == CardDB.cardIDEnum.EX1_610)
                        {
                            triggered++;
                            CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_610).sim_card.onSecretPlay(this, false, 0);
                            doDmgTriggers();
                            //Helpfunctions.Instance.ErrorLog("trigger explosive" + attacker.Hp);
                        }

                        if (!attacker.isHero && secretID == CardDB.cardIDEnum.EX1_594)
                        {
                            triggered++;
                            CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_594).sim_card.onSecretPlay(this, false, attacker, 0);
                            doDmgTriggers();
                        }
                        if (secretID == CardDB.cardIDEnum.EX1_594 && !(attacker.isHero && this.playerFirst.ownMinions.Count + this.playerSecond.ownMinions.Count == 0))
                        {
                            triggered++;
                            CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_533).sim_card.onSecretPlay(this, false, attacker, defender, out newTarget);
                        }

                        if (secretID == CardDB.cardIDEnum.EX1_289)
                        {
                            triggered++;
                            CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_289).sim_card.onSecretPlay(this, false, defender, 0);
                        }

                    }

                }

                if (!defender.isHero && !defender.own)
                {
                    foreach (CardDB.cardIDEnum secretID in this.playerSecond.ownSecretsIDList)
                    {

                        if (secretID == CardDB.cardIDEnum.EX1_554)
                        {
                            triggered++;
                            CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_554).sim_card.onSecretPlay(this, false, 0);
                        }
                    }
                }

                if (!attacker.isHero && attacker.own) // minion attacks
                {
                    foreach (CardDB.cardIDEnum secretID in this.playerSecond.ownSecretsIDList)
                    {
                        if (secretID == CardDB.cardIDEnum.EX1_611)
                        {
                            triggered++;
                            CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_611).sim_card.onSecretPlay(this, false, attacker, 0);
                        }
                    }
                }

                foreach (CardDB.cardIDEnum secretID in this.playerSecond.ownSecretsIDList)
                {

                    if (secretID == CardDB.cardIDEnum.EX1_130)
                    {
                        //triggered++;
                        bool ishero = defender.isHero;
                        CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_130).sim_card.onSecretPlay(this, false, attacker, defender, out newTarget);
                    }
                }


            }

            if (turnCounter == 0)
            {
                this.evaluatePenality -= triggered * 50;
            }

            return newTarget;
        }

        public void secretTrigger_HeroGotDmg(bool own, int dmg)
        {
            int triggered = 0;
            if (own != this.isOwnTurn)
            {
                if (this.isOwnTurn && this.playerSecond.ownSecretsIDList.Count >= 1)
                {
                    foreach (CardDB.cardIDEnum secretID in this.playerSecond.ownSecretsIDList)
                    {
                        if (secretID == CardDB.cardIDEnum.EX1_132)//eye for eye
                        {
                            triggered++;
                            CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_132).sim_card.onSecretPlay(this, false, dmg);
                        }

                        if (secretID == CardDB.cardIDEnum.EX1_132)
                        {
                            triggered++;
                            CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_295).sim_card.onSecretPlay(this, false, this.playerSecond.ownHero, dmg);
                            //si.usedTrigger_HeroGotDmg(true);                        
                        }
                    }
                }
            }

            if (turnCounter == 0)
            {
                this.evaluatePenality -= triggered * 50;
            }

        }

        public void secretTrigger_MinionIsPlayed(Minion playedMinion)
        {
            Player mPlayer;
            if (playedMinion.own)
            {
                mPlayer = this.playerSecond;
            }
            else
            {
                mPlayer = this.playerFirst;
            }
            int triggered = 0;
            if ((this.isOwnTurn == playedMinion.own) && mPlayer.ownSecretsIDList.Count >= 1)
            {
                for (int i = mPlayer.ownSecretsIDList.Count - 1; i >= 0; i--)
                {
                    CardDB.cardIDEnum secretID = mPlayer.ownSecretsIDList[i];
                    if (secretID == CardDB.cardIDEnum.EX1_609)
                    {
                        triggered++;
                        CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_609).sim_card.onSecretPlay(this, !playedMinion.own, playedMinion, 0);
                        doDmgTriggers();
                    }

                    if (secretID == CardDB.cardIDEnum.EX1_294)
                    {
                        triggered++;
                        CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_294).sim_card.onSecretPlay(this, !playedMinion.own, playedMinion, 0);
                    }

                    if (secretID == CardDB.cardIDEnum.EX1_379) //repentence
                    {
                        triggered++;
                        CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_379).sim_card.onSecretPlay(this, !playedMinion.own, playedMinion, 0);
                    }

                    mPlayer.ownSecretsIDList.RemoveAt(i);
                }
            }

            if (turnCounter == 0)
            {
                this.evaluatePenality -= triggered * 50;
            }

        }

        public int secretTrigger_SpellIsPlayed(Minion target, bool isSpell, bool ownTurn)
        {
            Player mPlayer;
            if (ownTurn)
            {
                mPlayer = this.playerSecond;
            }
            else
            {
                mPlayer = this.playerFirst;
            }

            int triggered = 0;
            if (this.isOwnTurn && isSpell && mPlayer.ownSecretsIDList.Count >= 1) //actual secrets need a spell played!
            {
                for (int i = mPlayer.ownSecretsIDList.Count - 1; i >= 0; i--)
                {
                    CardDB.cardIDEnum secretID = this.playerSecond.ownSecretsIDList[i];

                    if (secretID == CardDB.cardIDEnum.EX1_287)
                    {
                        triggered++;

                        CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_287).sim_card.onSecretPlay(this, false, 0);

                        mPlayer.ownSecretsIDList.RemoveAt(i);

                        return -2;//spellbender will NEVER trigger
                    }

                    if (secretID == CardDB.cardIDEnum.tt_010 && target != null && !target.isHero)
                    {
                        triggered++;
                        int retval = 0;
                        CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.tt_010).sim_card.onSecretPlay(this, ownTurn, null, target, out retval);
                        mPlayer.ownSecretsIDList.RemoveAt(i);
                        return retval;// the new target
                    }

                }

                if (turnCounter == 0)
                {
                    this.evaluatePenality -= triggered * 50;
                }

            }

            return 0;
        }

        public void secretTrigger_MinionDied(bool own) //TODO: rework for both sides 
        {
            int triggered = 0;

            if (this.isOwnTurn && !own && this.playerSecond.ownSecretsIDList.Count >= 1)
            {
                for (int i = this.playerSecond.ownSecretsIDList.Count - 1; i >= 0; i--)
                //foreach (CardDB.cardIDEnum secretID in this.playerSecond.ownSecretsIDList)
                {
                    CardDB.cardIDEnum secretID = this.playerSecond.ownSecretsIDList[i];
                    if (secretID == CardDB.cardIDEnum.FP1_018)//duplicate
                    {
                        triggered++;
                        CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.FP1_018).sim_card.onSecretPlay(this, false, 0);
                    }

                    if (secretID == CardDB.cardIDEnum.EX1_136)//redemption
                    {
                        triggered++;
                        CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_136).sim_card.onSecretPlay(this, false, 0);
                    }

                    if (secretID == CardDB.cardIDEnum.FP1_020)//avenge
                    {
                        triggered++;
                        CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.FP1_020).sim_card.onSecretPlay(this, false, 0);
                    }

                    this.playerSecond.ownSecretsIDList.RemoveAt(i);

                }
            }

            if (turnCounter == 0)
            {
                //this.evaluatePenality -= triggered * 50;
            }

        }



        public void doDeathrattles(List<Minion> deathrattles)
        {
            //todo sort them from oldest to newest (first played, first deathrattle)
            //https://www.youtube.com/watch?v=2WrbqsOSbhc
            foreach (Minion m in deathrattles)
            {
                if (!m.silenced && m.handcard.card.deathrattle) m.handcard.card.sim_card.onDeathrattle(this, m);

                for (int i = 0; i < m.souloftheforest; i++)
                {
                    CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_158t);//Treant
                    int pos = (m.own) ? this.playerFirst.ownMinions.Count : this.playerSecond.ownMinions.Count;
                    callKid(kid, pos, m.own);
                }

                for (int i = 0; i < m.ancestralspirit; i++)
                {
                    CardDB.Card kid = m.handcard.card;
                    int pos = (m.own) ? this.playerFirst.ownMinions.Count : this.playerSecond.ownMinions.Count;
                    callKid(kid, pos, m.own);
                }

                //baron rivendare ??
                if ((m.own && this.playerFirst.ownBaronRivendare >= 1) || (!m.own && this.playerSecond.ownBaronRivendare >= 1))
                {
                    if (!m.silenced && m.handcard.card.deathrattle) m.handcard.card.sim_card.onDeathrattle(this, m);

                    for (int i = 0; i < m.souloftheforest; i++)
                    {
                        CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_158t);//Treant
                        int pos = (m.own) ? this.playerFirst.ownMinions.Count : this.playerSecond.ownMinions.Count;
                        callKid(kid, pos, m.own);
                    }

                    for (int i = 0; i < m.ancestralspirit; i++)
                    {
                        CardDB.Card kid = m.handcard.card;
                        int pos = (m.own) ? this.playerFirst.ownMinions.Count : this.playerSecond.ownMinions.Count;
                        callKid(kid, pos, m.own);
                    }

                }
            }


        }


        public void updateBoards()//TODO: some (2 sides) rework here
        {
            if (!this.tempTrigger.ownMinionsChanged && !this.tempTrigger.enemyMininsChanged) return;
            List<Minion> deathrattles = new List<Minion>();

            bool minionOwnReviving = false;
            bool minionEnemyReviving = false;

            if (this.tempTrigger.ownMinionsChanged)
            {
                this.tempTrigger.ownMinionsChanged = false;
                List<Minion> temp = new List<Minion>();
                int i = 1;
                foreach (Minion m in this.playerFirst.ownMinions)
                {
                    //delete adjacent buffs
                    this.minionGetAdjacentBuff(m, -m.AdjacentAngr, 0);
                    m.cantBeTargetedBySpellsOrHeroPowers = false;
                    if ((m.name == CardDB.cardName.faeriedragon || m.name == CardDB.cardName.spectralknight || m.name == CardDB.cardName.laughingsister || m.name == CardDB.cardName.arcanenullifierx21) && !m.silenced)
                    {
                        m.cantBeTargetedBySpellsOrHeroPowers = true;
                    }

                    //kill it!
                    if (m.Hp <= 0)
                    {
                        this.playerFirst.ownLastDiedMinion = m.handcard.card.cardIDenum;
                        if (this.playerFirst.revivingOwnMinion == CardDB.cardIDEnum.None)
                        {
                            this.playerFirst.revivingOwnMinion = m.handcard.card.cardIDenum;
                            minionOwnReviving = true;
                        }

                        if ((!m.silenced && m.handcard.card.deathrattle) || m.ancestralspirit >= 1 || m.souloftheforest >= 1)
                        {
                            deathrattles.Add(m);
                        }
                        // end aura of minion m
                        m.handcard.card.sim_card.onAuraEnds(this, m);

                        /*if (m.handcard.card.name == CardDB.cardName.cairnebloodhoof || m.handcard.card.name == CardDB.cardName.harvestgolem || m.ancestralspirit>=1)
                        {
                            this.evaluatePenality -= Ai.Instance.botBase.getplayerSecond.ownMinionValue(m, this) - 1;
                        }*/

                    }
                    else
                    {
                        m.zonepos = i;
                        temp.Add(m);
                        i++;
                    }

                }
                this.playerFirst.ownMinions = temp;
                this.updateAdjacentBuffs(true);
            }

            if (this.tempTrigger.enemyMininsChanged)
            {
                this.tempTrigger.enemyMininsChanged = false;
                List<Minion> temp = new List<Minion>();
                int i = 1;
                foreach (Minion m in this.playerSecond.ownMinions)
                {
                    //delete adjacent buffs
                    this.minionGetAdjacentBuff(m, -m.AdjacentAngr, 0);
                    m.cantBeTargetedBySpellsOrHeroPowers = false;
                    if ((m.name == CardDB.cardName.faeriedragon || m.name == CardDB.cardName.spectralknight || m.name == CardDB.cardName.laughingsister || m.name == CardDB.cardName.arcanenullifierx21) && !m.silenced)
                    {
                        m.cantBeTargetedBySpellsOrHeroPowers = true;
                    }

                    //kill it!
                    if (m.Hp <= 0)
                    {
                        this.playerSecond.ownLastDiedMinion = m.handcard.card.cardIDenum;
                        if (this.playerSecond.revivingOwnMinion == CardDB.cardIDEnum.None)
                        {
                            this.playerSecond.revivingOwnMinion = m.handcard.card.cardIDenum;
                            minionEnemyReviving = true;
                        }

                        if ((!m.silenced && m.handcard.card.deathrattle) || m.ancestralspirit >= 1 || m.souloftheforest >= 1)
                        {
                            deathrattles.Add(m);
                        }
                        m.handcard.card.sim_card.onAuraEnds(this, m);

                        if ((!m.silenced && (m.handcard.card.name == CardDB.cardName.cairnebloodhoof || m.handcard.card.name == CardDB.cardName.harvestgolem)) || m.ancestralspirit >= 1)
                        {
                            this.evaluatePenality -= GameManager.Instance.botBase.getEnemyMinionValue(m, this) - 1;
                        }
                    }
                    else
                    {
                        m.zonepos = i;
                        temp.Add(m);
                        i++;
                    }

                }
                this.playerSecond.ownMinions = temp;
                this.updateAdjacentBuffs(false);
            }


            if (deathrattles.Count >= 1) this.doDeathrattles(deathrattles);

            if (minionOwnReviving)
            {
                this.secretTrigger_MinionDied(true);
                this.playerFirst.revivingOwnMinion = CardDB.cardIDEnum.None;
            }

            if (minionEnemyReviving)
            {
                this.secretTrigger_MinionDied(false);
                this.playerSecond.revivingOwnMinion = CardDB.cardIDEnum.None;
            }
            //update buffs
        }

        public void minionGetOrEraseAllAreaBuffs(Minion m, bool get)
        {
            if (m.isHero) return;
            int angr = 0;
            int vert = 0;

            if (m.handcard.card.race == 14)
            {
                angr += 2 * playerFirst.anzMurlocWarleader + playerFirst.anzGrimscaleOracle;
                vert += playerFirst.anzMurlocWarleader;

            }

            if (!m.silenced) // if they are not silenced, these minions will give a buff, but cant buff themselfes
            {
                if (m.name == CardDB.cardName.raidleader || m.name == CardDB.cardName.leokk || m.name == CardDB.cardName.timberwolf) angr--;
                if (m.name == CardDB.cardName.stormwindchampion || m.name == CardDB.cardName.southseacaptain)
                {
                    angr--;
                    vert--;
                }
                if (m.name == CardDB.cardName.murlocwarleader)
                {
                    angr -= 2;
                    vert--;
                }
                if (m.name == CardDB.cardName.grimscaleoracle)
                {
                    angr--;
                }
            }


            if (m.own)
            {
                // todo charge:  m.charge -= anzOwnTundrarhino;
                if (get) m.charge += playerFirst.anzOwnTundrarhino;
                else m.charge -= playerFirst.anzOwnTundrarhino;
                angr += playerFirst.anzOwnRaidleader;
                angr += playerFirst.anzOwnStormwindChamps;
                vert += playerFirst.anzOwnStormwindChamps;
                if (m.handcard.card.race == 20)
                {
                    angr += playerFirst.anzOwnTimberWolfs;
                }
                if (m.handcard.card.race == 23)
                {
                    angr += playerFirst.anzOwnSouthseacaptain;
                    vert += playerFirst.anzOwnSouthseacaptain;

                }
                if (m.handcard.card.race == 15)
                {
                    angr += playerFirst.anzOwnMalGanis * 2;
                    vert += playerFirst.anzOwnMalGanis * 2;

                }

            }
            else
            {
                if (get) m.charge += playerSecond.anzOwnTundrarhino;
                else m.charge -= playerSecond.anzOwnTundrarhino;
                angr += playerSecond.anzOwnRaidleader;
                angr += playerSecond.anzOwnStormwindChamps;
                vert += playerSecond.anzOwnStormwindChamps;
                if (m.handcard.card.race == 20)
                {
                    angr += playerSecond.anzOwnTimberWolfs;
                }
                if (m.handcard.card.race == 23)
                {
                    angr += playerSecond.anzOwnSouthseacaptain;
                    vert += playerSecond.anzOwnSouthseacaptain;

                }
                if (m.handcard.card.race == 15)
                {
                    angr += playerSecond.anzOwnMalGanis * 2;
                    vert += playerSecond.anzOwnMalGanis * 2;

                }
            }

            if (get) this.minionGetBuffed(m, angr, vert);
            else this.minionGetBuffed(m, -angr, -vert);

        }

        public void updateAdjacentBuffs(bool own)
        {
            //only call this after update board
            if (own)
            {
                int anz = this.playerFirst.ownMinions.Count;
                for (int i = 0; i < anz; i++)
                {
                    Minion m = this.playerFirst.ownMinions[i];
                    if (!m.silenced)
                    {
                        if (m.name == CardDB.cardName.direwolfalpha)
                        {
                            if (i > 0) this.minionGetAdjacentBuff(this.playerFirst.ownMinions[i - 1], 1, 0);
                            if (i < anz - 1) this.minionGetAdjacentBuff(this.playerFirst.ownMinions[i + 1], 1, 0);
                        }

                        if (m.name == CardDB.cardName.flametonguetotem)
                        {
                            if (i > 0) this.minionGetAdjacentBuff(this.playerFirst.ownMinions[i - 1], 2, 0);
                            if (i < anz - 1) this.minionGetAdjacentBuff(this.playerFirst.ownMinions[i + 1], 2, 0);
                        }

                        if (m.name == CardDB.cardName.weespellstopper)
                        {
                            if (i > 0) this.playerFirst.ownMinions[i - 1].cantBeTargetedBySpellsOrHeroPowers = true;
                            if (i < anz - 1) this.playerFirst.ownMinions[i + 1].cantBeTargetedBySpellsOrHeroPowers = true;
                        }
                    }
                }
            }
            else
            {
                int anz = this.playerSecond.ownMinions.Count;
                for (int i = 0; i < anz; i++)
                {
                    Minion m = this.playerSecond.ownMinions[i];
                    if (!m.silenced)
                    {
                        if (m.name == CardDB.cardName.direwolfalpha)
                        {
                            if (i > 0) this.minionGetAdjacentBuff(this.playerSecond.ownMinions[i - 1], 1, 0);
                            if (i < anz - 1) this.minionGetAdjacentBuff(this.playerSecond.ownMinions[i + 1], 1, 0);
                        }

                        if (m.name == CardDB.cardName.flametonguetotem)
                        {
                            if (i > 0) this.minionGetAdjacentBuff(this.playerSecond.ownMinions[i - 1], 2, 0);
                            if (i < anz - 1) this.minionGetAdjacentBuff(this.playerSecond.ownMinions[i + 1], 2, 0);
                        }

                        if (m.name == CardDB.cardName.weespellstopper)
                        {
                            if (i > 0) this.playerSecond.ownMinions[i - 1].cantBeTargetedBySpellsOrHeroPowers = true;
                            if (i < anz - 1) this.playerSecond.ownMinions[i + 1].cantBeTargetedBySpellsOrHeroPowers = true;
                        }
                    }
                }
            }
        }

        public Minion createMinionFromKeyInfo(PlayerKeyInfo.MinionKeyInfo k, bool own)
        {
            CardDB.cardName cardName = CardDB.Instance.cardNamestringToEnum(k.name);
            CardDB.Card card = CardDB.Instance.getDeckCardData(cardName);
            Handmanager.Handcard hc = new Handmanager.Handcard(card) { entity = k.entity };
            Minion m = new Minion() { 
                handcard = hc
            };
            m.entitiyID = k.entity;
            m.own = own;
            m.name = cardName;
            m.zonepos = k.zonepos;
            m.Hp = k.Hp;
            m.maxHp = k.maxHp;
            m.armor = k.armor;
            m.Angr = k.Angr;
            m.wounded = k.wounded;
            m.divineshild = k.divineshild;
            m.windfury = k.windfury;
            m.frozen = k.frozen;
            m.stealth = k.stealth;
            m.immune = k.immune;
            m.exhausted = k.exhausted;
            m.charge = k.charge;
            m.poisonous = k.poisonous;
            m.silenced = k.silenced;
            m.taunt = k.taunt;
            m.updateReadyness();
            return m;
        }

        public Handmanager.Handcard createHandcardFromKeyInfo(PlayerKeyInfo.CardKeyInfo ck)
        {
            CardDB.Card card = CardDB.Instance.getDeckCardData(CardDB.Instance.cardNamestringToEnum(ck.cardName));
            Handmanager.Handcard hc = new Handmanager.Handcard(card) { entity = ck.entity };
            hc.permCostChange = 0;
            hc.position = ck.position;
            hc.manacost = ck.manacost;
            hc.addattack = 0;
            hc.addHp = 0;

            return hc;
        }

        public Player createPlayerFromKeyInfo(PlayerKeyInfo keyInfo)
        { 
            Player p = new Player(keyInfo.turn);

            p.ownController = keyInfo.turn;
            p.hasCoin = (p.ownController == 1) ? true : false;

            p.mana = keyInfo.maxMana;
            p.manaTurnEnd = 0;
            p.ownMaxMana = keyInfo.maxMana;
            //p.evaluatePenality = 0;

            p.attackFaceHP = 15;

            //this.complete = false;

            p.ownHero = new Minion();
            p.ownHero.cardClass = TAG_CLASS.MAGE;

            //implementation
            p.ownHero.isHero = true;
            p.ownHero.own = (p.ownController == 0) ? true : false;
            p.ownHero.maxHp = 30;
            p.ownHero.entitiyID = p.ownController;

            p.ownHero.Angr = keyInfo.heroInfo.Angr;
            p.ownHero.Hp = keyInfo.heroInfo.Hp;
            p.ownHero.armor = keyInfo.heroInfo.armor;
            p.ownHero.frozen = keyInfo.heroInfo.frozen;
            p.ownHero.immuneWhileAttacking = false; //TODO: here
            p.ownHero.immune = keyInfo.heroInfo.immune;
            p.ownHero.numAttacksThisTurn = 0;
            p.ownHero.windfury = keyInfo.heroInfo.windfury;

            //end of implementation

            //addCardsReal(homeHandManager.handCards);

            p.ownHeroName = HeroEnum.mage;
            p.ownHeroStartClass = TAG_CLASS.MAGE;
            /*
            this.enemyHeroHp = hrtprozis.enemyHp;
            this.ownHeroHp = hrtprozis.heroHp;
            this.ownHeroReady = hrtprozis.ownheroisread;
            this.ownHeroWindfury = hrtprozis.ownHeroWindfury;
            this.ownHeroNumAttackThisTurn = hrtprozis.ownHeroNumAttacksThisTurn;
            this.ownHeroFrozen = hrtprozis.herofrozen;
            this.enemyHeroFrozen = hrtprozis.enemyfrozen;
            this.ownheroAngr = hrtprozis.heroAtk;
            this.heroImmuneWhileAttacking = hrtprozis.heroImmuneToDamageWhileAttacking;
            this.ownHeroDefence = hrtprozis.heroDefence;
            this.enemyHeroDefence = hrtprozis.enemyDefence;
             */

            //####buffs#############################

            p.anzOwnRaidleader = 0;
            p.anzOwnStormwindChamps = 0;
            p.anzOwnTundrarhino = 0;
            p.anzOwnTimberWolfs = 0;
            p.anzMurlocWarleader = 0;
            p.anzGrimscaleOracle = 0;
            p.anzOwnAuchenaiSoulpriest = 0;
            p.anzOwnsorcerersapprentice = 0;
            //this.anzOwnsorcerersapprenticeStarted = 0;
            p.anzOwnSouthseacaptain = 0;
            //this.anzOwnDragonConsortStarted = 0;

            p.ownMinionsDiedTurn = 0;

            //this.feugenDead = false;
            //this.stalaggDead = false;

            p.doublepriest = 0;

            p.ownBaronRivendare = 0;
            //#########################################

            p.ownWeaponDurability = 0;
            p.ownWeaponAttack = 0;
            p.ownWeaponName = CardDB.cardName.unknown;
            p.owncarddraw = 0;

            p.ownAbilityReady = true;
            p.ownHeroAblility = new Handmanager.Handcard(CardDB.Instance.getCardData(CardDB.Instance.cardNamestringToEnum("fireblast")));

            p.mobsplayedThisTurn = 0;
            //this.startedWithMobsPlayedThisTurn = 0;// only change mobsplayedthisturm
            p.cardsPlayedThisTurn = 0;
            //todo:
            p.optionsPlayedThisTurn = 0;

            p.ueberladung = 0;

            p.ownHeroFatigue = 0;
            //p.ownDeckSize = (p.ownController == 0) ? homeDeck.Count : awayDeck.Count;

            //need the following for manacost-calculation
            //this.ownHeroHpStarted = this.ownHero.Hp;
            //this.ownWeaponAttackStarted = this.ownWeaponAttack;
            //this.ownCardsCountStarted = this.owncards.Count;
            //this.ownMobsCountStarted = this.ownMinions.Count + this.enemyMinions.Count;

            p.playedmagierinderkirintor = false;
            p.playedPreparation = false;

            p.winzigebeschwoererin = 0;
            p.managespenst = 0;
            p.soeldnerDerVenture = 0;
            p.beschwoerungsportal = 0;
            p.nerubarweblord = 0;

            //this.startedWithnerubarweblord = 0;
            //this.startedWithbeschwoerungsportal = 0;
            //this.startedWithManagespenst = 0;
            //this.startedWithWinzigebeschwoererin = 0;
            //this.startedWithsoeldnerDerVenture = 0;

            p.ownBaronRivendare = 0;

            //this.loatheb = false;
            p.spellpower = 0;

            //this.startedWithDamagedMinions = false;

            p.ownMinions.Clear();

            foreach (PlayerKeyInfo.MinionKeyInfo mk in keyInfo.minionJsonList)
            {
                Minion m = this.createMinionFromKeyInfo(mk, (p.ownController == 0) ? true : false);
                this.nextEntity = Math.Max(this.nextEntity, m.entitiyID);
                p.ownMinions.Add(m);
            }

            foreach (Minion m in p.ownMinions)
            {
                //if (m.Hp < m.maxHp && m.Hp >= 1) this.startedWithDamagedMinions = true;
                //if (m.playedThisTurn && m.name == CardDB.cardName.loatheb) this.loatheb = true;

                p.spellpower = p.spellpower + m.spellpower;
                if (m.silenced) continue;
                p.spellpower += m.handcard.card.spellpowervalue;
                if (m.name == CardDB.cardName.prophetvelen) p.doublepriest++;


                if (m.name == CardDB.cardName.pintsizedsummoner)
                {
                    p.winzigebeschwoererin++;
                    //this.startedWithWinzigebeschwoererin++;
                }

                if (m.name == CardDB.cardName.manawraith)
                {
                    p.managespenst++;
                    //this.startedWithManagespenst++;
                }
                if (m.name == CardDB.cardName.nerubarweblord)
                {
                    p.nerubarweblord++;
                    //this.startedWithnerubarweblord++;
                }
                if (m.name == CardDB.cardName.venturecomercenary)
                {
                    p.soeldnerDerVenture++;
                    //this.startedWithsoeldnerDerVenture++;
                }
                if (m.name == CardDB.cardName.summoningportal)
                {
                    p.beschwoerungsportal++;
                    //this.startedWithbeschwoerungsportal++;
                }

                if (m.handcard.card.name == CardDB.cardName.baronrivendare)
                {
                    p.ownBaronRivendare++;
                }
                if (m.handcard.card.name == CardDB.cardName.kelthuzad)
                {
                    //this.needGraveyard = true;
                }

                if (m.name == CardDB.cardName.raidleader) p.anzOwnRaidleader++;
                if (m.name == CardDB.cardName.malganis) p.anzOwnMalGanis++;
                if (m.name == CardDB.cardName.stormwindchampion) p.anzOwnStormwindChamps++;
                if (m.name == CardDB.cardName.tundrarhino) p.anzOwnTundrarhino++;
                if (m.name == CardDB.cardName.timberwolf) p.anzOwnTimberWolfs++;
                if (m.name == CardDB.cardName.murlocwarleader) p.anzMurlocWarleader++;
                if (m.name == CardDB.cardName.grimscaleoracle) p.anzGrimscaleOracle++;
                if (m.name == CardDB.cardName.auchenaisoulpriest) p.anzOwnAuchenaiSoulpriest++;
                if (m.name == CardDB.cardName.sorcerersapprentice)
                {
                    p.anzOwnsorcerersapprentice++;
                    //this.anzOwnsorcerersapprenticeStarted++;
                }
                if (m.name == CardDB.cardName.southseacaptain) p.anzOwnSouthseacaptain++;
                if (m.name == CardDB.cardName.chromaggus) p.anzOwnChromaggus++;
                //if (m.name == CardDB.cardName.dragonconsort && anzOwnDragonConsort > 0) this.anzOwnDragonConsortStarted++;
                if (m.name == CardDB.cardName.mechwarper)
                {
                    p.anzOwnMechwarper++;
                    //this.anzOwnMechwarperStarted++;
                }
                if (m.name == CardDB.cardName.steamwheedlesniper && p.ownHeroName == HeroEnum.hunter)
                {
                    p.weHaveSteamwheedleSniper = true;
                }

            }

            p.owncards.Clear();

            foreach (PlayerKeyInfo.CardKeyInfo ck in keyInfo.handcardJsonList)
            {
                Handmanager.Handcard hc = createHandcardFromKeyInfo(ck);
                p.owncards.Add(hc);
                this.nextEntity = Math.Max(this.nextEntity, hc.entity);

                //if (hc.card.name == CardDB.cardName.kelthuzad)
                //{
                //    //this.needGraveyard = true;
                //}
            }

            return p;
        }



        public Minion createNewMinion(Handmanager.Handcard hc, int zonepos, bool own)
        {
            Minion m = new Minion();
            Handmanager.Handcard handc = new Handmanager.Handcard(hc);
            //Handmanager.Handcard handc = hc; // new Handcard(hc)?
            m.handcard = handc;
            m.own = own;
            m.isHero = false;
            m.entitiyID = hc.entity;
            m.Angr = hc.card.Attack + hc.addattack;
            m.Hp = hc.card.Health + hc.addHp;

            hc.addattack = 0;
            hc.addHp = 0;

            m.maxHp = hc.card.Health;
            m.name = hc.card.name;
            m.playedThisTurn = true;
            m.numAttacksThisTurn = 0;
            m.zonepos = zonepos;
            m.windfury = hc.card.windfury;
            m.taunt = hc.card.tank;
            m.charge = (hc.card.Charge) ? 1 : 0;
            m.divineshild = hc.card.Shield;
            m.poisonous = hc.card.poisionous;
            m.stealth = hc.card.Stealth;

            m.updateReadyness();

            if (m.name == CardDB.cardName.lightspawn)
            {
                m.Angr = m.Hp;
            }


            //trigger on summon effect!
            this.triggerAMinionIsSummoned(m);
            //activate onAura effect
            m.handcard.card.sim_card.onAuraStarts(this, m);
            //buffs minion
            this.minionGetOrEraseAllAreaBuffs(m, true);
            return m;
        }

        public void placeAmobSomewhere(Handmanager.Handcard hc, bool own, Minion target, int choice, int zonepos)
        {
            int mobplace = zonepos;

            //create the new minion + trigger Summon effects + buffs it
            Minion m = createNewMinion(hc, mobplace, own);

            //trigger the battlecry!
            m.handcard.card.sim_card.getBattlecryEffect(this, m, target, choice);

            //add minion to list + do triggers + do secret trigger +  minion was played trigger
            addMinionToBattlefield(m);

            secretTrigger_MinionIsPlayed(m);

            if (logging) Helpfunctions.Instance.logg("added " + m.handcard.card.name);
        }

        public void addMinionToBattlefield(Minion m, bool isSummon = true)
        {
            List<Minion> temp = (m.own) ? this.playerFirst.ownMinions : this.playerSecond.ownMinions;
            if (temp.Count >= m.zonepos && m.zonepos >= 1)
            {
                temp.Insert(m.zonepos - 1, m);
            }
            else
            {
                temp.Add(m);
            }
            if (m.own) this.tempTrigger.ownMinionsChanged = true;
            else this.tempTrigger.enemyMininsChanged = true;
            doDmgTriggers();


            //minion was played secrets? trigger here---- (+ do triggers)


            //trigger a minion was summoned
            triggerAMinionWasSummoned(m);
            doDmgTriggers();

            //implementation
            if (m.Hp > 0)
            {
                if (m.own == this.isOwnTurn)
                {
                    if (m.handcard.card.deathrattle || moveTrigger.hasOwnTargetMove)
                    {
                        moveTrigger.ownNewTarget = true;
                    }
                }
                else
                {
                    this.moveTrigger.enemyNewTarget = true;
                }

                //adding summon minion reward
                int maxMana = Math.Max(getCurrentPlayer(true).ownMaxMana, getCurrentPlayer(false).ownMaxMana);
                if (this.isOwnTurn == m.own)
                {
                    moveTrigger.moveReward += m.getTurnValue(maxMana);
                }
                else
                {
                    moveTrigger.moveReward -= m.getTurnValue(maxMana);
                }
            }

        }

        public void equipWeapon(CardDB.Card c, bool own)
        {
            Player mPlayer, ePlayer;
            Minion hero;
            if (own)
            {
                mPlayer = this.playerFirst;
                ePlayer = this.playerSecond;
                hero = this.playerFirst.ownHero;
            }
            else
            {
                mPlayer = this.playerSecond;
                ePlayer = this.playerFirst;
                hero = this.playerSecond.ownHero;
            }

            if (own)
            {
                if (mPlayer.ownWeaponDurability >= 1)
                {
                    this.lostWeaponDamage += mPlayer.ownWeaponDurability * mPlayer.ownWeaponAttack * mPlayer.ownWeaponAttack;
                    this.lowerWeaponDurability(1000, true);
                    hero.Angr -= mPlayer.ownWeaponAttack;
                }
                mPlayer.ownWeaponAttack = c.Attack;
                mPlayer.ownWeaponDurability = c.Durability;
                mPlayer.ownWeaponName = c.name;
            }
            
            hero.Angr += c.Attack;

            hero.windfury = (c.name == CardDB.cardName.doomhammer);

            hero.updateReadyness();

            hero.immuneWhileAttacking = (c.name == CardDB.cardName.gladiatorslongbow);

            List<Minion> temp = mPlayer.ownMinions;
            foreach (Minion m in temp)
            {
                if (m.playedThisTurn && m.name == CardDB.cardName.southseadeckhand)
                {
                    minionGetCharge(m);
                }
            }

        }


        //todo 4th param
        public void callKid(CardDB.Card c, int zonepos, bool own, bool spawnKid = false, bool oneMoreIsAllowed = false)
        {
            //spawnKid = true if its a minion spawned with another one (battlecry)
            int allowed = 7;
            allowed += (oneMoreIsAllowed) ? 1 : 0;
            allowed -= (spawnKid) ? 1 : 0;

            if (own)
            {
                if (this.playerFirst.ownMinions.Count >= allowed)
                {
                    if (spawnKid) this.evaluatePenality += 20;
                    return;
                }
            }
            else
            {
                if (this.playerSecond.ownMinions.Count >= allowed)
                {
                    if (spawnKid) this.evaluatePenality -= 20;
                    return;
                }
            }
            int mobplace = zonepos + 1;//todo check this?

            //create minion (+triggers)
            Handmanager.Handcard hc = new Handmanager.Handcard(c) { entity = this.getNextEntity() };
            Minion m = createNewMinion(hc, mobplace, own);
            //put it on battle field (+triggers)
            addMinionToBattlefield(m);

        }



        public void minionGetSilenced(Minion m)
        {
            //minion cant die due to silencing!
            m.becomeSilence(this);

        }

        public void allMinionsGetSilenced(bool own)
        {
            List<Minion> temp = (own) ? this.playerFirst.ownMinions : this.playerSecond.ownMinions;
            foreach (Minion m in temp)
            {
                m.becomeSilence(this);
            }
        }

        public void getHandCard(Player mPlayer, CardDB.cardName s, bool own)
        {
            CardDB.Card c = CardDB.Instance.getCardData(s);
            Handmanager.Handcard hc = new Handmanager.Handcard { card = c, position = mPlayer.owncards.Count + 1, manacost = c.cost, entity = this.getNextEntity() };

            if (mPlayer.enemyMillhouse > 0 && c.type == CardDB.cardtype.SPELL)
            {
                hc.manacost = 0;
            }
            if (mPlayer.enemyLoatheb > 0 && c.type == CardDB.cardtype.SPELL)
            {
                hc.manacost += 5 * mPlayer.enemyLoatheb;
            }

            mPlayer.owncarddraw++;
            mPlayer.owncards.Add(hc);
            this.triggerCardsChanged(own);
        }

        //public int getNumCardDraw() 
        //{
        //    if (isOwnTurn)
        //    {
        //        return playerFirst.owncarddraw;
        //    }
        //    else 
        //    {
        //        return playerSecond.owncarddraw;
        //    }
        //}

        public int getCardDrawPossibility(int cardDraw)
        {
            Player mPlayer;
            int deckSize;

            if (isOwnTurn)
            {
                mPlayer = this.playerFirst;
                deckSize = this.homeDeck.Count;
            }
            else
            {
                mPlayer = this.playerSecond;
                deckSize = this.awayDeck.Count;
            }

            if (deckSize <= cardDraw) return 1;
            return Helpfunctions.Instance.c(deckSize, cardDraw);
        }

        public void getHandCard(Player mPlayer, CardDB.cardIDEnum s, bool own)
        {
            CardDB.Card c = CardDB.Instance.getCardDataFromID(s);
            Handmanager.Handcard hc = new Handmanager.Handcard { card = c, position = mPlayer.owncards.Count + 1, manacost = c.cost, entity = this.getNextEntity() };

            if (mPlayer.enemyMillhouse > 0 && c.type == CardDB.cardtype.SPELL)
            {
                hc.manacost = 0;
            }
            if (mPlayer.enemyLoatheb > 0 && c.type == CardDB.cardtype.SPELL)
            {
                hc.manacost += 5 * mPlayer.enemyLoatheb;
            }

            mPlayer.owncarddraw++;
            mPlayer.owncards.Add(hc);
            this.triggerCardsChanged(own);
        }

        public int drawACard(CardDB.cardName ss, bool own,  bool nopen = false, bool passive = false) //passive: drawn by system, not by user //nopen: not from the deck //0: draw a card, -1 hand is full, -2 out of cards
        {
            CardDB.cardName s = ss;

            bool ownDraw = own == this.isOwnTurn;
            // cant hold more than 10 cards

            Player mPlayer;
            List<CardDB.Card> mDeck;
            if (own)
            {
                mPlayer = this.playerFirst;
                mDeck = this.homeDeck;
            }
            else
            {
                mPlayer = this.playerSecond;
                mDeck = this.awayDeck;
            }

            if (!nopen)
            {
                if (s == CardDB.cardName.unknown && mDeck.Count == 0)
                { //out of cards
                    mPlayer.ownHeroFatigue++;
                    mPlayer.ownHero.getDamageOrHeal(mPlayer.ownHeroFatigue, this, false, true);
                    return -2;
                }
                else
                {

                    if (mPlayer.owncards.Count >= 10)
                    {
                        if (ownDraw)
                            this.evaluatePenality += 15;
                        else
                            this.evaluatePenality -= 50;
                        return -1;
                    }

                    getHandCard(mPlayer, s, own);

                    if (mPlayer.anzOwnChromaggus > 0)
                    {
                        for (int i = 1; i <= mPlayer.anzOwnChromaggus; i++)
                        {
                            if (mPlayer.owncards.Count >= 10)
                            {
                                if (ownDraw)
                                    this.evaluatePenality += 15;
                                else
                                    this.evaluatePenality -= 50;
                                return -1;
                            }
                            getHandCard(mPlayer, s, own);
                        }
                    }
                }

            }
            else
            {

                if (mPlayer.owncards.Count >= 10)
                {
                    if (ownDraw)
                        this.evaluatePenality += 5;
                    else
                        this.evaluatePenality -= 50;
                    return -1;
                }

                getHandCard(mPlayer, s, own);
            }

            //move trigger
            if (own == isOwnTurn)
            {
                this.moveTrigger.handcardAdded = true;
                if (!nopen && !passive)
                {
                    this.moveTrigger.newHandcardList.Add((int)ss);
                }
            }

            return 0;

        }

        public int drawACard(CardDB.cardIDEnum ss, bool own, bool nopen = false, bool passive = false)
        {
            CardDB.cardIDEnum s = ss;
            CardDB.cardName cn = CardDB.Instance.getCardDataFromID(ss).name;

            bool ownDraw = own == this.isOwnTurn;
            // cant hold more than 10 cards

            Player mPlayer;
            List<CardDB.Card> mDeck;
            if (own)
            {
                mPlayer = this.playerFirst;
                mDeck = this.homeDeck;
            }
            else
            {
                mPlayer = this.playerSecond;
                mDeck = this.awayDeck;
            }

            if (!nopen)
            {
                if (s == CardDB.cardIDEnum.None && mDeck.Count == 0)
                { //out of cards
                    mPlayer.ownHeroFatigue++;
                    mPlayer.ownHero.getDamageOrHeal(mPlayer.ownHeroFatigue, this, false, true);
                    return -2;
                }
                else
                {
                    if (mPlayer.owncards.Count >= 10)
                    {
                        if (ownDraw)
                            this.evaluatePenality += 15;
                        else
                            this.evaluatePenality -= 50;
                        return -1;
                    }

                    getHandCard(mPlayer, s, own);

                    if (mPlayer.anzOwnChromaggus > 0)
                    {
                        for (int i = 1; i <= mPlayer.anzOwnChromaggus; i++)
                        {
                            if (mPlayer.owncards.Count >= 10)
                            {
                                if (ownDraw)
                                    this.evaluatePenality += 15;
                                else
                                    this.evaluatePenality -= 50;
                                return -1;
                            }
                            getHandCard(mPlayer, s, own);
                        }
                    }
                }

            }
            else
            {

                if (mPlayer.owncards.Count >= 10)
                {
                    if (ownDraw)
                        this.evaluatePenality += 5;
                    else
                        this.evaluatePenality -= 50;
                    return -1;
                }
                getHandCard(mPlayer, s, own);
            }

            //move trigger
            if (own == isOwnTurn)
            {
                this.moveTrigger.handcardAdded = true;
                if (!nopen && !passive)
                {
                    this.moveTrigger.newHandcardList.Add((int)cn);
                }
            }

            return 0;

        }


        public void removeCard(Handmanager.Handcard hcc, bool own)
        {
            Player mPlayer;
            if (own)
            {
                mPlayer = this.playerFirst;
            }
            else
            {
                mPlayer = this.playerSecond;
            }

            int cardPos = 1;
            int cardNum = 0;
            Handmanager.Handcard hcTmp = null;
            foreach (Handmanager.Handcard hc in mPlayer.owncards)
            {
                if (hc.entity == hcc.entity)
                {
                    hcTmp = hc;
                    cardNum++;
                    continue;
                }
                mPlayer.owncards[cardNum].position = cardPos;
                cardNum++;
                cardPos++;
            }
            if (hcTmp != null) mPlayer.owncards.Remove(hcTmp);
        }


        // some helpfunctions 


        public void attackEnemyHeroWithoutKill(int dmg)
        {
            this.playerSecond.ownHero.cantLowerHPbelowONE = true;
            this.minionGetDamageOrHeal(this.playerSecond.ownHero, dmg);
            this.playerSecond.ownHero.cantLowerHPbelowONE = false;
        }

        public void minionGetDestroyed(Minion m)
        {
            if (m.Hp > 0)
            {
                m.Hp = 0;
                m.minionDied(this);
                GraveYardItem gyi = new GraveYardItem(m.handcard.card.cardIDenum, m.entitiyID, m.own);
                diedMinions.Add(gyi);
                graveYard.Add(gyi);
            }
        }

        public void allMinionsGetDestroyed()
        {
            foreach (Minion m in this.playerFirst.ownMinions)
            {
                minionGetDestroyed(m);
            }
            foreach (Minion m in this.playerSecond.ownMinions)
            {
                minionGetDestroyed(m);
            }
        }


        public void minionGetArmor(Minion m, int armor)
        {
            m.armor += armor;
            this.triggerAHeroGotArmor(m.own);
        }

        public void minionReturnToHand(Minion m, bool own, int manachange)
        {
            List<Minion> temp = (own) ? this.playerFirst.ownMinions : this.playerSecond.ownMinions;
            m.handcard.card.sim_card.onAuraEnds(this, m);
            temp.Remove(m);

            if (own)
            {
                CardDB.Card c = m.handcard.card;
                Handmanager.Handcard hc = new Handmanager.Handcard { card = c, position = this.playerFirst.owncards.Count + 1, entity = m.entitiyID, manacost = c.cost + manachange, permCostChange = manachange };
                if (this.playerFirst.owncards.Count < 10)
                {
                    this.playerFirst.owncards.Add(hc);
                    this.triggerCardsChanged(true);
                }

                this.tempTrigger.ownMinionsChanged = true;
            }
            else
            {
                CardDB.Card c = m.handcard.card;
                Handmanager.Handcard hc = new Handmanager.Handcard { card = c, position = this.playerSecond.owncards.Count + 1, entity = m.entitiyID, manacost = c.cost + manachange, permCostChange = manachange };
                if (this.playerSecond.owncards.Count < 10)
                {
                    this.playerSecond.owncards.Add(hc);
                    this.triggerCardsChanged(false);
                }

                this.tempTrigger.enemyMininsChanged = true;
            }

        }

        public void minionTransform(Minion m, CardDB.Card c)
        {
            m.handcard.card.sim_card.onAuraEnds(this, m);//end aura of the minion

            Handmanager.Handcard hc = new Handmanager.Handcard(c) { entity = m.entitiyID };
            int ancestral = m.ancestralspirit;
            if (m.handcard.card.name == CardDB.cardName.cairnebloodhoof || m.handcard.card.name == CardDB.cardName.harvestgolem || ancestral >= 1)
            {
                this.evaluatePenality -= GameManager.Instance.botBase.getEnemyMinionValue(m, this) - 1;
            }

            //necessary???
            /*Minion tranform = createNewMinion(hc, m.zonepos, m.own);
            Minion temp = new Minion();
            temp.setMinionTominion(m);
            m.setMinionTominion(tranform);*/

            m.setMinionTominion(createNewMinion(hc, m.zonepos, m.own));

            m.handcard.card.sim_card.onAuraStarts(this, m);
            this.minionGetOrEraseAllAreaBuffs(m, true);

            if (m.own)
            {
                this.tempTrigger.ownMinionsChanged = true;
            }
            else
            {
                this.tempTrigger.enemyMininsChanged = true;
            }

            if (logging) Helpfunctions.Instance.logg("minion got sheep" + m.name + " " + m.Angr);
        }

        public void minionGetControlled(Minion m, bool newOwner, bool canAttack)
        {
            List<Minion> newOwnerList = (newOwner) ? this.playerFirst.ownMinions : this.playerSecond.ownMinions;
            List<Minion> oldOwnerList = (newOwner) ? this.playerSecond.ownMinions : this.playerFirst.ownMinions;



            if (newOwnerList.Count >= 7) return;

            this.tempTrigger.ownMinionsChanged = true;
            this.tempTrigger.enemyMininsChanged = true;
            //if (m.taunt && newOwner) this.playerFirst.anzEnemyTaunt--;

            //end buffs/aura
            m.handcard.card.sim_card.onAuraEnds(this, m);
            this.minionGetOrEraseAllAreaBuffs(m, false);

            //remove minion from list
            oldOwnerList.Remove(m);

            //change site (and minion is played in this turn)
            m.playedThisTurn = true;
            m.own = !m.own;

            // add minion to new list + new buffs
            newOwnerList.Add(m);
            m.handcard.card.sim_card.onAuraStarts(this, m);
            this.minionGetOrEraseAllAreaBuffs(m, true);

            if (m.charge >= 1 || canAttack) // minion can attack if its shadowmadnessed (canAttack = true) or it has charge
            {
                this.minionGetCharge(m);
            }
            m.updateReadyness();

        }



        public void minionGetWindfurry(Minion m)
        {
            if (m.windfury) return;
            m.windfury = true;
            m.updateReadyness();
        }

        public void minionGetCharge(Minion m)
        {
            m.charge++;
            m.updateReadyness();
        }

        public void minionLostCharge(Minion m)
        {
            m.charge--;
            m.updateReadyness();
        }



        public void minionGetTempBuff(Minion m, int tempAttack, int tempHp)
        {
            if (!m.silenced && m.name == CardDB.cardName.lightspawn) return;
            if (tempAttack < 0 && -tempAttack > m.Angr)
            {
                tempAttack = -m.Angr;
            }
            m.tempAttack += tempAttack;
            m.Angr += tempAttack;
        }

        public void minionGetAdjacentBuff(Minion m, int angr, int vert)
        {
            if (!m.silenced && m.name == CardDB.cardName.lightspawn) return;
            m.Angr += angr;
            m.AdjacentAngr += angr;
        }

        public void minionGetBuffed(Minion m, int attackbuff, int hpbuff)
        {
            m.Angr = Math.Max(0, m.Angr + attackbuff);

            if (hpbuff >= 1)
            {
                m.Hp = m.Hp + hpbuff;
                m.maxHp = m.maxHp + hpbuff;
            }
            else
            {
                //debuffing hp, lower only maxhp (unless maxhp < hp)
                m.maxHp = m.maxHp + hpbuff;
                if (m.maxHp < m.Hp)
                {
                    m.Hp = m.maxHp;
                }
            }


            m.wounded = (m.maxHp != m.Hp);

            if (m.name == CardDB.cardName.lightspawn && !m.silenced)
            {
                m.Angr = m.Hp;
            }

        }

        public void minionSetAngrToOne(Minion m)
        {
            if (!m.silenced && m.name == CardDB.cardName.lightspawn) return;
            m.Angr = 1;
            m.tempAttack = 0;
            this.minionGetOrEraseAllAreaBuffs(m, true);
        }

        public void minionSetLifetoOne(Minion m)
        {
            minionGetOrEraseAllAreaBuffs(m, false);
            m.Hp = 1;
            m.maxHp = 1;
            if (m.wounded && !m.silenced) m.handcard.card.sim_card.onEnrageStop(this, m);
            m.wounded = false;
            minionGetOrEraseAllAreaBuffs(m, true);
        }

        public void minionSetAngrToHP(Minion m)
        {
            m.Angr = m.Hp;
            m.tempAttack = 0;
            this.minionGetOrEraseAllAreaBuffs(m, true);

        }

        public void minionSwapAngrAndHP(Minion m)
        {
            this.minionGetOrEraseAllAreaBuffs(m, false);
            bool woundedbef = m.wounded;
            int temp = m.Angr;
            m.Angr = m.Hp;
            m.Hp = temp;
            m.maxHp = temp;
            m.wounded = false;
            if (woundedbef) m.handcard.card.sim_card.onEnrageStop(this, m);
            if (m.Hp <= 0)
            {
                if (m.own) this.tempTrigger.ownMinionsDied++;
                else this.tempTrigger.enemyMinionsDied++;
            }

            this.minionGetOrEraseAllAreaBuffs(m, true);
        }

        public void minionGetDamageOrHeal(Minion m, int dmgOrHeal, bool dontDmgLoss = false)
        {
            m.getDamageOrHeal(dmgOrHeal, this, false, dontDmgLoss);
        }



        public void allMinionOfASideGetDamage(bool own, int damages, bool frozen = false)
        {
            List<Minion> temp = (own) ? this.playerFirst.ownMinions : this.playerSecond.ownMinions;
            foreach (Minion m in temp)
            {
                if (frozen) m.frozen = true;
                minionGetDamageOrHeal(m, damages, true);
            }
        }

        public void allCharsOfASideGetDamage(bool own, int damages)
        {
            //ALL CHARS get same dmg
            List<Minion> temp = (own) ? this.playerFirst.ownMinions : this.playerSecond.ownMinions;
            foreach (Minion m in temp)
            {
                minionGetDamageOrHeal(m, damages, true);
            }

            this.minionGetDamageOrHeal(own ? this.playerFirst.ownHero : this.playerSecond.ownHero, damages);
        }

        public void allCharsGetDamage(int damages)
        {
            foreach (Minion m in this.playerFirst.ownMinions)
            {
                minionGetDamageOrHeal(m, damages, true);
            }
            foreach (Minion m in this.playerSecond.ownMinions)
            {
                minionGetDamageOrHeal(m, damages, true);
            }
            minionGetDamageOrHeal(this.playerFirst.ownHero, damages);
            minionGetDamageOrHeal(this.playerSecond.ownHero, damages);
        }

        public void allMinionsGetDamage(int damages)
        {
            foreach (Minion m in this.playerFirst.ownMinions)
            {
                minionGetDamageOrHeal(m, damages, true);
            }
            foreach (Minion m in this.playerSecond.ownMinions)
            {
                minionGetDamageOrHeal(m, damages, true);
            }
        }

        public enum searchmode
        {
            searchLowestHP,
            searchHighestHP,
            searchLowestAttack,
            searchHighestAttack,
            searchHighAttackLowHP,
            searchHighHPLowAttack,
        }

        public Minion searchRandomMinion(List<Minion> minions, searchmode mode)
        {
            //get = 0 -> get lowest hp
            //get = 1 -> get highest hp
            //get = 2 -> get lowest attack
            //get = 3 -> get highest attack
            //get = 4 -> get max attack/hp ratio
            //get = 5 -> get max hp/attack ratio
            int get = (int)mode;

            if (minions.Count == 0) return null;
            Minion ret = minions[0];
            int value = ret.Hp;
            if (get >= 2) value = ret.Angr;
            if (get >= 4) value = 0;
            foreach (Minion m in minions)
            {
                if (m.Hp <= 0) continue;

                switch (get)
                {
                    case 1:
                        if (m.Hp > value)
                        {
                            ret = m;
                            value = m.Hp;
                        }
                        break;
                    case 2:
                        if (m.Angr < value)
                        {
                            ret = m;
                            value = m.Angr;
                        }
                        break;
                    case 3:
                        if (m.Angr > value)
                        {
                            ret = m;
                            value = m.Angr;
                        }
                        break;
                    case 4:
                        if (m.Angr / m.Hp > value)
                        {
                            ret = m;
                            value = m.Angr / m.Hp;
                        }
                        break;
                    case 5:
                        if (m.Hp / m.Angr > value)
                        {
                            ret = m;
                            value = m.Hp / m.Angr;
                        }
                        break;
                    default:
                        if (m.Hp < value)
                        {
                            ret = m;
                            value = m.Hp;
                        }
                        break;
                }
            }
            if (ret.Hp <= 0) return null;
            return ret;
        }

        public float getDefensiveValue(bool own)
        {
            Player mPlayer;
            if (own)
            {
                mPlayer = this.playerFirst;
            }
            else
            {
                mPlayer = this.playerSecond;
            }

            float minionValue = 0f;

            foreach (Minion m in mPlayer.ownMinions)
            {
                minionValue += (m.Angr != 0) ? (float)Math.Sqrt(m.Angr) * m.Hp : 0.5f * m.Hp;
                if (m.silenced) continue;

            }

            return minionValue;
        }

        public float getOffensiveValue(bool own)
        {
            Player mPlayer;
            if (own)
            {
                mPlayer = this.playerFirst;
            }
            else
            {
                mPlayer = this.playerSecond;
            }

            float minionValue = 0f;

            foreach (Minion m in mPlayer.ownMinions)
            {
                minionValue += (m.Angr != 0) ? m.Angr * (float)Math.Sqrt(m.Hp) : 0.25f * (float)Math.Sqrt(m.Hp);
                if (m.silenced) continue;

            }

            return minionValue;
        }

        public float getBoardAttack(bool own, bool halfFreeze)
        {
            Player mPlayer;
            if (own)
            {
                mPlayer = this.playerFirst;
            }
            else
            {
                mPlayer = this.playerSecond;
            }

            float attackValue = 0;

            foreach (Minion m in mPlayer.ownMinions)
            {
                if (m.frozen && halfFreeze)
                {
                    attackValue += m.Angr / 2;
                }
                else if (m.frozen)
                {
                    continue;
                }
                attackValue += m.Angr;
            }

            if (mPlayer.ownWeaponAttack > 0 && !mPlayer.ownHero.frozen)
                attackValue += mPlayer.ownWeaponAttack;

            return attackValue;
        }

        public void debugMinions()
        {
            Helpfunctions.Instance.logg("Turn:" + this.isOwnTurn);
            Helpfunctions.Instance.logg("OWN MINIONS################");

            foreach (Minion m in this.playerFirst.ownMinions)
            {
                Helpfunctions.Instance.logg("name,ang, hp, maxhp: " + m.name + ", " + m.Angr + ", " + m.Hp + ", " + m.maxHp);
            }

            Helpfunctions.Instance.logg("ENEMY MINIONS############");
            foreach (Minion m in this.playerSecond.ownMinions)
            {
                Helpfunctions.Instance.logg("name,ang, hp: " + m.name + ", " + m.Angr + ", " + m.Hp);
            }
        }

        public void debugHand()
        {
            Helpfunctions.Instance.logg("Turn:" + this.isOwnTurn);
            Helpfunctions.Instance.logg("OWN CARDS################");

            foreach (Handmanager.Handcard c in this.playerFirst.owncards)
            {
                Helpfunctions.Instance.logg("pos " + c.position + " " + c.card.name + " " + c.manacost + " entity " + c.entity + " " + c.card.cardIDenum + " " + c.addattack);
            }

            Helpfunctions.Instance.logg("ENEMY CARDS############");
            foreach (Handmanager.Handcard c in this.playerSecond.owncards)
            {
                Helpfunctions.Instance.logg("pos " + c.position + " " + c.card.name + " " + c.manacost + " entity " + c.entity + " " + c.card.cardIDenum + " " + c.addattack);
            }
        }

        public void printBoard()//TODO: for both sides (secrets)
        {
            Helpfunctions.Instance.logg("Turn:" + this.isOwnTurn);

            Helpfunctions.Instance.logg("board/hash: " + playerFirst.value + "  /  " + this.playerFirst.hashcode + " ++++++++++++++++++++++");
            //Helpfunctions.Instance.logg("pen " + this.playerFirst.evaluatePenality);
            Helpfunctions.Instance.logg("ownmana " + this.playerFirst.mana + "/" + this.playerFirst.ownMaxMana);
            Helpfunctions.Instance.logg("owncardsplayed: " + this.playerFirst.cardsPlayedThisTurn + " handsize: " + this.playerFirst.owncards.Count + " eh " + this.playerSecond.owncards.Count);
            Helpfunctions.Instance.logg("ownhero: ");
            Helpfunctions.Instance.logg("ownherohp: " + this.playerFirst.ownHero.Hp + " + " + this.playerFirst.ownHero.armor);
            Helpfunctions.Instance.logg("ownheroattac: " + this.playerFirst.ownHero.Angr);
            Helpfunctions.Instance.logg("ownheroweapon: " + this.playerFirst.ownWeaponAttack + " " + this.playerFirst.ownWeaponDurability + " " + this.playerFirst.ownWeaponName);
            Helpfunctions.Instance.logg("ownherostatus: frozen" + this.playerFirst.ownHero.frozen + " ");

            Helpfunctions.Instance.logg("enemymana " + this.playerSecond.mana + "/" + this.playerSecond.ownMaxMana);
            Helpfunctions.Instance.logg("enemycardsplayed: " + this.playerSecond.cardsPlayedThisTurn + " handsize: " + this.playerSecond.owncards.Count + " eh " + this.playerSecond.owncards.Count);
            Helpfunctions.Instance.logg("enemyhero: ");
            Helpfunctions.Instance.logg("enemyherohp: " + this.playerSecond.ownHero.Hp + " + " + this.playerSecond.ownHero.armor);
            Helpfunctions.Instance.logg("enemyheroattac: " + this.playerSecond.ownHero.Angr);
            Helpfunctions.Instance.logg("enemyheroweapon: " + this.playerSecond.ownWeaponAttack + " " + this.playerSecond.ownWeaponDurability + " " + this.playerSecond.ownWeaponName);
            Helpfunctions.Instance.logg("enemyherostatus: frozen" + this.playerSecond.ownHero.frozen + " ");

            //if (this.playerSecond.ownSecretsIDList.Count >= 1) Helpfunctions.Instance.logg("playerSecond.ownSecrets: " + Probabilitymaker.Instance.getplayerSecond.ownSecretData(this.playerSecond.ownSecretList));
            foreach (Action a in this.playerFirst.playactions)
            {
                a.print();
            }
            foreach (Action a in this.playerSecond.playactions)
            {
                a.print();
            }

            Helpfunctions.Instance.logg("OWN MINIONS################");

            foreach (Minion m in this.playerFirst.ownMinions)
            {
                Helpfunctions.Instance.logg("name,ang, hp: " + m.name + ", " + m.Angr + ", " + m.Hp + " " + m.entitiyID);
            }

            Helpfunctions.Instance.logg("ENEMY MINIONS############");
            foreach (Minion m in this.playerSecond.ownMinions)
            {
                Helpfunctions.Instance.logg("name,ang, hp: " + m.name + ", " + m.Angr + ", " + m.Hp + " " + m.entitiyID);
            }

            Helpfunctions.Instance.logg("OWN CARDS################");

            foreach (Handmanager.Handcard c in this.playerFirst.owncards)
            {
                Helpfunctions.Instance.logg("pos " + c.position + " " + c.card.name + " " + c.manacost + " entity " + c.entity + " " + c.card.cardIDenum + " " + c.addattack);
            }

            Helpfunctions.Instance.logg("ENEMY CARDS############");
            foreach (Handmanager.Handcard c in this.playerSecond.owncards)
            {
                Helpfunctions.Instance.logg("pos " + c.position + " " + c.card.name + " " + c.manacost + " entity " + c.entity + " " + c.card.cardIDenum + " " + c.addattack);
            }

            Helpfunctions.Instance.logg("");
        }

        public void printBoardDebug()
        {
            Helpfunctions.Instance.logg("hero " + this.playerFirst.ownHero.Hp + " " + this.playerFirst.ownHero.armor + " " + this.playerFirst.ownHero.entitiyID);
            Helpfunctions.Instance.logg("ehero " + this.playerSecond.ownHero.Hp + " " + this.playerSecond.ownHero.armor + " " + this.playerSecond.ownHero.entitiyID);
            foreach (Minion m in this.playerFirst.ownMinions)
            {
                Helpfunctions.Instance.logg(m.name + " " + m.entitiyID);
            }
            Helpfunctions.Instance.logg("-");
            foreach (Minion m in playerSecond.ownMinions)
            {
                Helpfunctions.Instance.logg(m.name + " " + m.entitiyID);
            }
            Helpfunctions.Instance.logg("-");
            foreach (Handmanager.Handcard hc in this.playerFirst.owncards)
            {
                Helpfunctions.Instance.logg(hc.position + " " + hc.card.name + " " + hc.entity);
            }
        }

        public Action getNextAction()
        {
            if (this.playerFirst.playactions.Count >= 1) return this.playerFirst.playactions[0];
            return null;
        }

        public Player getCurrentPlayer(bool own) //turn not end
        {
            if (isOwnTurn == own)
            {
                return this.playerFirst;
            }
            else
            {
                return this.playerSecond;
            }
        }

        public List<Action> getActions() //turn not end
        {
            if (isOwnTurn)
            {
                return this.playerFirst.playactions;
            }
            else
            {
                return this.playerSecond.playactions;
            }
        }

        public List<Macro> getMacroActions() //turn not end
        {
            if (isOwnTurn)
            {
                return this.playerFirst.playMacros;
            }
            else
            {
                return this.playerSecond.playMacros;
            }
        }

        public void printActions(bool toBuffer = false) //turn not end
        {
            if (isOwnTurn)
            {
                foreach (Action a in this.playerFirst.playactions)
                {
                    a.print();
                }
            }
            else
            {
                foreach (Action a in this.playerSecond.playactions)
                {
                    a.print();
                }
            }
        }

        public List<Action> getLastTurnAction()
        {
            if (isOwnTurn)
            {
                return playerSecond.lastTurnActions;            
            }
            else
            {
                return playerFirst.lastTurnActions;              
            }
        }


        public List<Macro> getLastTurnMacros()
        {
            if (isOwnTurn)
            {
                return playerSecond.lastTurnMacros;
            }
            else
            {
                return playerFirst.lastTurnMacros;
            }
        }

        public void printLastTurnMacros(bool toBuffer = false)
        {
            if (isOwnTurn)
            {
                foreach (Macro m in this.playerSecond.lastTurnMacros)
                {
                    m.printTarget();
                }
            }
            else
            {
                foreach (Macro m in this.playerFirst.lastTurnMacros)
                {
                    m.printTarget();
                }
            }
        }


        public void printLastTurnActions(bool toBuffer = false)
        {
            if (isOwnTurn)
            {
                foreach (Action a in this.playerSecond.lastTurnActions)
                {
                    a.print();
                }
            }
            else
            {
                foreach (Action a in this.playerFirst.lastTurnActions)
                {
                    a.print();
                }
            }
        }

        public void printActionforDummies(Action a)
        {
            if (a.actionType == actionEnum.playcard)
            {
                Helpfunctions.Instance.ErrorLog("play " + a.card.card.name);
                if (a.druidchoice >= 1)
                {
                    string choose = (a.druidchoice == 1) ? "left card" : "right card";
                    Helpfunctions.Instance.ErrorLog("choose the " + choose);
                }
                if (a.place >= 1)
                {
                    Helpfunctions.Instance.ErrorLog("on position " + a.place);
                }
                if (a.target != null)
                {
                    if (!a.target.own && !a.target.isHero)
                    {
                        string ename = "" + a.target.name;
                        Helpfunctions.Instance.ErrorLog("and target to the playerSecond.own " + ename);
                    }

                    if (a.target.own && !a.target.isHero)
                    {
                        string ename = "" + a.target.name;
                        Helpfunctions.Instance.ErrorLog("and target to your own" + ename);
                    }

                    if (a.target.own && a.target.isHero)
                    {
                        Helpfunctions.Instance.ErrorLog("and target your own hero");
                    }

                    if (!a.target.own && a.target.isHero)
                    {
                        Helpfunctions.Instance.ErrorLog("and target to the playerSecond.own hero");
                    }
                }

            }
            if (a.actionType == actionEnum.attackWithMinion)
            {
                string name = "" + a.own.name;
                if (a.target.isHero)
                {
                    Helpfunctions.Instance.ErrorLog("attack with: " + name + " the playerSecond.own hero");
                }
                else
                {
                    string ename = "" + a.target.name;
                    Helpfunctions.Instance.ErrorLog("attack with: " + name + " the playerSecond.own: " + ename);
                }

            }

            if (a.actionType == actionEnum.attackWithHero)
            {
                if (a.target.isHero)
                {
                    Helpfunctions.Instance.ErrorLog("attack with your hero the playerSecond.own hero!");
                }
                else
                {
                    string ename = "" + a.target.name;
                    Helpfunctions.Instance.ErrorLog("attack with the hero, and choose the playerSecond.own: " + ename);
                }
            }
            if (a.actionType == actionEnum.useHeroPower)
            {
                Helpfunctions.Instance.ErrorLog("use your Heropower ");
                if (a.target != null)
                {
                    if (!a.target.own && !a.target.isHero)
                    {
                        string ename = "" + a.target.name;
                        Helpfunctions.Instance.ErrorLog("on playerSecond.own: " + ename);
                    }

                    if (a.target.own && !a.target.isHero)
                    {
                        string ename = "" + a.target.name;
                        Helpfunctions.Instance.ErrorLog("on your own: " + ename);
                    }

                    if (a.target.own && a.target.isHero)
                    {
                        Helpfunctions.Instance.ErrorLog("on your own hero");
                    }

                    if (!a.target.own && a.target.isHero)
                    {
                        Helpfunctions.Instance.ErrorLog("on your the playerSecond.own hero");
                    }

                }
            }
            Helpfunctions.Instance.ErrorLog("");

        }

        public void printGraveyard()
        {
            Helpfunctions.Instance.logg("Graveyard:");
            foreach (GraveYardItem gItem in graveYard)
            {
                Helpfunctions.Instance.logg(CardDB.Instance.getCardDataFromID(gItem.cardid).name + ", " + gItem.entity +
                    (gItem.own ? ", P1" : ", P2"));
            }
        }

        public float getHandNonMinonValue()
        {
            Player mPlayer, ePlayer;
            if (isOwnTurn) //after turn end;
            {
                mPlayer = this.playerSecond;
                ePlayer = this.playerFirst;
            }
            else
            {
                mPlayer = this.playerFirst;
                ePlayer = this.playerSecond;
            }            //Minions

            float ownHandValue = 0f;

            foreach (Handmanager.Handcard hc in mPlayer.owncards)
            {
                int manacost = hc.getManaCost(this, !isOwnTurn);
                if (manacost > mPlayer.mana + 1) continue;
                if (hc.card.type == CardDB.cardtype.SPELL)
                {
                    ownHandValue += manacost;
                }
            }

            float enemyHandValue = 0f;

            foreach (Handmanager.Handcard hc in ePlayer.owncards)
            {
                int manacost = hc.getManaCost(this, isOwnTurn);
                if (manacost > ePlayer.mana) continue;
                if (hc.card.type == CardDB.cardtype.SPELL)
                {
                    enemyHandValue += manacost;
                }
            }
            return ownHandValue - enemyHandValue;           
        }

        public double getHeroValue()
        {
            Player mPlayer, ePlayer;
            if (isOwnTurn) //after turn end;
            {
                mPlayer = this.playerSecond;
                ePlayer = this.playerFirst;
            }
            else
            {
                mPlayer = this.playerFirst;
                ePlayer = this.playerSecond;
            }

            int step = (mPlayer.ownHero.Hp + mPlayer.ownHero.armor);
            return (float)step* 0.33f;
        }

        public double getHandValue(bool afterTurnEnd)
        {
            double ownHandValue = getOwnHandValue(afterTurnEnd);
            double enemyHandValue = getEnemyHandValue(afterTurnEnd);
            double bound = 10 * 10 * 2;
            return (ownHandValue - enemyHandValue + bound / 2) / bound;
        }

        public double getOwnHandValue(bool afterTurnEnd)
        {
            Player mPlayer = getCurrentPlayer(!afterTurnEnd);

            double ownHandValue = 0f;
            double discount;

            foreach (Handmanager.Handcard hc in mPlayer.owncards)
            {
                double manacost = (double)hc.getManaCost(this, !isOwnTurn);
                double manaDif = manacost - mPlayer.ownMaxMana - 1;
                if (hc.card.name == CardDB.cardName.thecoin)
                {
                    manacost = 1.5;
                    manaDif = 0.0;
                }
                if (manaDif < 0) manaDif = 0;
                discount = Math.Pow(0.7, manaDif);

                ownHandValue += manacost * discount;
            }

            return ownHandValue;
        }

        public double getEnemyHandValue(bool afterTurnEnd)
        {
            double enemyHandValue = 0.0;

            Player ePlayer = getCurrentPlayer(afterTurnEnd);

            foreach (Handmanager.Handcard hc in ePlayer.owncards)
            {
                int manacost = hc.getManaCost(this, isOwnTurn);
                if (manacost > ePlayer.ownMaxMana) continue;
                float minionValue = 0f;
                if (hc.card.type == CardDB.cardtype.MOB)
                {
                    enemyHandValue += (hc.card.Attack != 0) ? hc.card.Attack * (float)Math.Sqrt(hc.card.Health) : 0.25f * (float)Math.Sqrt(hc.card.Health);

                    switch (hc.card.name)
                    {
                        case CardDB.cardName.archmageantonidas:
                            minionValue *= 1.5f;
                            break;
                    }

                    if (hc.card.deathrattle)
                        minionValue++;

                    if (hc.card.Shield || hc.card.Stealth || hc.card.tank || hc.card.deathrattle || hc.card.isSpecialMinion)
                    {
                        minionValue += 1;
                    }
                    enemyHandValue += minionValue;
                }
            }

            return enemyHandValue;
        }

        public float getHandMinionValue()
        {
            Player mPlayer, ePlayer;
            if (isOwnTurn) //after turn end;
            {
                mPlayer = this.playerSecond;
                ePlayer = this.playerFirst;
            }
            else
            {
                mPlayer = this.playerFirst;
                ePlayer = this.playerSecond;
            }            //Minions

            float ownHandValue = 0f;

            foreach (Handmanager.Handcard hc in mPlayer.owncards)
            {
                int manacost = hc.getManaCost(this, !isOwnTurn);
                if (manacost > mPlayer.ownMaxMana + 1) continue;
                float minionValue = 0f;
                if (hc.card.type == CardDB.cardtype.MOB)
                {
                    minionValue = (hc.card.Attack != 0) ? (float)Math.Sqrt(hc.card.Attack) * hc.card.Health : 0.5f * hc.card.Health;

                    switch (hc.card.name)
                    {
                        case CardDB.cardName.archmageantonidas:
                            minionValue *= 1.2f;
                            break;
                    }

                    if (hc.card.deathrattle)
                        minionValue++;

                    if (hc.card.Shield || hc.card.Stealth || hc.card.tank || hc.card.deathrattle || hc.card.isSpecialMinion)
                    {
                        minionValue += 1;
                    }
                }
                ownHandValue += minionValue;
            }

            float enemyHandValue = 0f;

            foreach (Handmanager.Handcard hc in ePlayer.owncards)
            {
                int manacost = hc.getManaCost(this, isOwnTurn);
                if (manacost > ePlayer.ownMaxMana) continue;
                float minionValue = 0f;
                if (hc.card.type == CardDB.cardtype.MOB)
                {
                    enemyHandValue += (hc.card.Attack != 0) ? hc.card.Attack * (float)Math.Sqrt(hc.card.Health) : 0.25f * (float)Math.Sqrt(hc.card.Health);

                    switch (hc.card.name)
                    {
                        case CardDB.cardName.archmageantonidas:
                            minionValue *= 1.5f;
                            break;
                    }

                    if (hc.card.deathrattle)
                        minionValue++;

                    if (hc.card.Shield || hc.card.Stealth || hc.card.tank || hc.card.deathrattle || hc.card.isSpecialMinion)
                    {
                        minionValue += 1;
                    }
                    enemyHandValue += minionValue;
                }
            }
            float bound = (float)Math.Sqrt(8) * 8 * 10 * 2;
            return (ownHandValue - enemyHandValue + bound/2)/bound;
        }

        public float getEnemyBoardValue(bool afterTurnEnd)
        {

            Player ePlayer = getCurrentPlayer(!afterTurnEnd);

            float enemyMinionValue = 0f;

            foreach (Minion m in ePlayer.ownMinions)
            {
                float minionValue = m.getDefMinionValue();
                enemyMinionValue += minionValue;
            }

            //return enemyMinionValue;
            float bound = (float)Math.Sqrt(8) * 8 * 8;
            return (bound - enemyMinionValue)/bound;
        }

        public float getOwnBoardValue(bool afterTurnEnd)
        {

            Player mPlayer = this.getCurrentPlayer(!afterTurnEnd);
            mPlayer = this.getCurrentPlayer(true);

            float ownMinionValue = 0f;
            foreach (Minion m in mPlayer.ownMinions)
            {
                float minionValue = m.getOffMinionValue();
                ownMinionValue += minionValue;
            }
            float bound = (float)Math.Sqrt(8) * 8 * 8;
            return ownMinionValue / bound;
        }

        public float getBoardValue(bool afterTurnEnd)
        {
            float ownBoard = getOwnBoardValue(afterTurnEnd);
            float enemyBoard = getEnemyBoardValue(afterTurnEnd);
            return (ownBoard - (1.0f - enemyBoard) + 1.0f) / 2;
        }

        public float observerMoveReward() //simple reward
        { 
            float reward = 0;
            //reward -= moveTrigger.ownMinionDied * 0.1f;
            //reward += moveTrigger.enemyMinionDied * 0.1f;
            reward = moveTrigger.moveReward;
            if (getCurrentPlayer(true).ownHero.Hp <= 0)
                return -1.0f;
            if (getCurrentPlayer(false).ownHero.Hp <= 0)
                return 1.0f;

            return reward;
        }

        public double getLethalScore()
        {
            Player ePlayer;
            if (isOwnTurn) //after turn end;
            {
                ePlayer = this.playerFirst;
            }
            else
            {
                ePlayer = this.playerSecond;
            }

            if (ePlayer.ownHero.Hp <= 0) return 1.0;
            int totalHp = ePlayer.ownHero.Hp + ePlayer.ownHero.armor;
            if (totalHp > 30) 
                return 0;
            else 
                return 1.0 - (double)totalHp / 30.0;
        }

        public double[] getDrawCardScore()
        {
            Player mPlayer;
            if (isOwnTurn) //after turn end;
            {
                mPlayer = this.playerFirst;
            }
            else
            {
                mPlayer = this.playerSecond;
            }

            if (mPlayer.playactions.Count == 0 || moveTrigger.newHandcardList.Count == 0)
                return new double[2] { 0.0, 0.0 };

            double manaScore = 1.0 - (double)mPlayer.playactions.Count / (mPlayer.ownMinions.Count + mPlayer.owncards.Count + 1);

            Debug.Assert(manaScore <= 1.0);            

            Action lastAction = mPlayer.playactions.Last();
            int minionCount = 0;

            if (lastAction.card.card.type == CardDB.cardtype.SPELL)
            {
                foreach (Minion m in mPlayer.ownMinions)
                {
                    if (CardDB.Instance.spellDependentDatabase.ContainsKey(m.name) && !m.silenced) minionCount++;
                    if (lastAction.card.card.name == CardDB.cardName.battlerage && m.wounded) minionCount++;
                }
            }
            else if (mPlayer.anzOwnChromaggus > 0)
            {
                minionCount = mPlayer.anzOwnChromaggus;
            }

            double triggerScore = minionCount / 8.0;

            return new double[2]{manaScore, triggerScore};
        }

        //TODO: Coin
        public double[] getEndTurnFeatures()
        {
            Player mPlayer, ePlayer;
            if (isOwnTurn) //after turn end;
            {
                mPlayer = this.playerSecond;
                ePlayer = this.playerFirst;
            }
            else
            {
                mPlayer = this.playerFirst;
                ePlayer = this.playerSecond;
            }           

            double maxHp = 30.0; //30 + 20 armor
            double ownHp = (double)(mPlayer.ownHero.Hp + mPlayer.ownHero.armor);
            double enemyHp = (double)(ePlayer.ownHero.Hp + ePlayer.ownHero.armor);
            double maxMana = 10.0;
            double ownMaxMana = (double)(Math.Min(mPlayer.ownMaxMana + 1, 10));
            double enemyMaxMana = (double)(Math.Min(10.0, ePlayer.ownMaxMana));

            bool afterTurnEnd = true;
            double[] features = new double[] { getBoardValue(afterTurnEnd), -getEnemyBoardValue(afterTurnEnd), getHandValue(afterTurnEnd), ownHp / maxHp, enemyHp / maxHp, ownMaxMana / maxMana, enemyMaxMana / maxMana };
            return features;
        }

        public void doMacroAction(Macro m)
        { 
            foreach (Action action in m.actionList)
            {
                doAction(action);
            }
            this.getCurrentPlayer(true).playMacros.Add(m); 
        }

        public void randomize(double probability)
        {
            Player mPlayer, ePlayer; //not mPlayer, but not ePlayer
            List<CardDB.Card> mDeck, eDeck;
            if (this.isOwnTurn)
            {
                mPlayer = playerFirst;
                ePlayer = playerSecond;
                mDeck = homeDeck;
                eDeck = awayDeck;
            }
            else
            {
                ePlayer = playerFirst;
                mPlayer = playerSecond;
                eDeck = homeDeck;
                mDeck = awayDeck;
            }

            List<int> cardToRemove = new List<int>();
            int cardReturned = 0;
            foreach (Handmanager.Handcard hc in ePlayer.owncards.ToArray())
            {
                if (GameManager.getRNG().NextDouble() < probability)
                {
                    //randomly swap a card in the deack
                    ePlayer.owncards.Remove(hc);
                    addCardBackToDeck(hc, !this.isOwnTurn);
                    cardReturned++;
                }
            }

            for (int i = 0; i < cardReturned; i++)
            {
                drawACard(getArandomCardFromDeck(!this.isOwnTurn), !this.isOwnTurn, passive: true);
            }
        }

    }

}
