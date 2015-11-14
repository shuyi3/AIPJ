using System.Collections.Generic;

//TODO: Died Minions

namespace HRSim
{
    using System;

    public class Player {

        //aura minions##########################
        //todo reduce buffing vars
        public int anzOwnRaidleader = 0;
        public int anzOwnStormwindChamps = 0;
        public int anzOwnTundrarhino = 0;
        public int anzOwnTimberWolfs = 0;
        public int anzMurlocWarleader = 0;
        public int anzGrimscaleOracle = 0;
        public int anzOwnAuchenaiSoulpriest = 0;
        public int anzOwnsorcerersapprentice = 0;
        public int anzOwnsorcerersapprenticeStarted = 0;
        public int anzOwnSouthseacaptain = 0;
        public int anzOwnMalGanis = 0;
        public int anzOwnChromaggus = 0;
        public int anzOwnDragonConsort = 0;
        public int anzOwnDragonConsortStarted = 0;

        //implementation of loatheb
        public int enemyLoatheb = 0;
        public int enemyMillhouse = 0;
        //end of implementation of loatheb

        public int anzOwnMechwarper = 0;
        public int anzOwnMechwarperStarted = 0;

        //public int anzEnemyTaunt = 0;
        public int ownMinionsDiedTurn = 0;

        public bool feugenDead = false;
        public bool stalaggDead = false;

        public bool weHaveSteamwheedleSniper = false;

        public int doublepriest = 0;

        public int ownBaronRivendare = 0;
        //#########################################

        public int tempanzOwnCards = 0; // for Goblin Sapper
        //public bool isOwnTurn = true; // its your turn?
        //public int turnCounter = 0;
        public bool sEnemTurn = false;//should the enemy turn be simulated?

        public bool attacked = false;
        public int attackFaceHP = 15;

        public int evaluatePenality = 0;
        public int ownController = 0;

        //public int ownHeroEntity = -1;
        //public int enemyHeroEntity = -1;

        public int hashcode = 0;
        public float value = Int32.MinValue;
        public int guessingHeroHP = 30;

        public int mana = 0;
        public int manaTurnEnd = 0;

        public List<CardDB.cardIDEnum> ownSecretsIDList = new List<CardDB.cardIDEnum>();

        public Minion ownHero;
        public HeroEnum ownHeroName = HeroEnum.druid;
        public TAG_CLASS ownHeroStartClass = TAG_CLASS.INVALID;

        public CardDB.cardName ownWeaponName = CardDB.cardName.unknown;
        public int ownWeaponAttack = 0;
        public int ownWeaponDurability = 0;

        public List<Minion> ownMinions = new List<Minion>();

        public List<Handmanager.Handcard> owncards = new List<Handmanager.Handcard>();
        public int owncarddraw = 0;

        public List<Action> playactions = new List<Action>();

        public int spellpower = 0;

        public bool playedmagierinderkirintor = false;
        public bool playedPreparation = false;

        public bool loatheb = false;
        public int winzigebeschwoererin = 0;
        public int startedWithWinzigebeschwoererin = 0;
        public int managespenst = 0;
        public int startedWithManagespenst = 0;
        public int soeldnerDerVenture = 0;
        public int startedWithsoeldnerDerVenture = 0;
        public int beschwoerungsportal = 0;
        public int startedWithbeschwoerungsportal = 0;
        public int nerubarweblord = 0;
        public int startedWithnerubarweblord = 0;

        public bool startedWithDamagedMinions = false; // needed for manacalculation of the spell "Crush"

        public int ownWeaponAttackStarted = 0;
        public int ownMobsCountStarted = 0;
        public int ownCardsCountStarted = 0;
        public int ownHeroHpStarted = 30;

        public int mobsplayedThisTurn = 0;
        public int startedWithMobsPlayedThisTurn = 0;

        public int optionsPlayedThisTurn = 0;
        public int cardsPlayedThisTurn = 0;
        public int ueberladung = 0; //=recall

        public int ownMaxMana = 0;

        public int lostDamage = 0;
        public int lostHeal = 0;
        public int lostWeaponDamage = 0;

        public int ownDeckSize = 30;
        public int ownHeroFatigue = 0;

        public bool ownAbilityReady = false;
        public Handmanager.Handcard ownHeroAblility;

        // just for saving which minion to revive with secrets (=the first one that died);
        public CardDB.cardIDEnum revivingOwnMinion = CardDB.cardIDEnum.None;
        public CardDB.cardIDEnum ownLastDiedMinion = CardDB.cardIDEnum.None;

        public int shadowmadnessed = 0; //minions has switched controllers this turn.
        //Helpfunctions help = Helpfunctions.Instance;

        //implementation
        //public MyHandManager homeHandManager = new MyHandManager();
        //public MyHandManager awayHandManager = new MyHandManager();
        public static Random rng = null;

        public List<CardDB.Card> homeDeck = new List<CardDB.Card>();
        public List<CardDB.Card> awayDeck = new List<CardDB.Card>();

        public List<GraveYardItem> graveYard = new List<GraveYardItem>();

        public Player(int player)
        {

            this.ownController = player;
 
            this.mana = (player == 0)? 1: 0;
            this.manaTurnEnd = this.mana;
            this.ownMaxMana = mana;
            this.evaluatePenality = 0;

            this.attackFaceHP = 15;

            //this.complete = false;

            this.ownHero = new Minion();
            this.ownHero.cardClass = TAG_CLASS.MAGE;

            //implementation
            this.ownHero.isHero = true;
            this.ownHero.own = true;
            this.ownHero.maxHp = 30;
            this.ownHero.entitiyID = player;

            this.ownHero.Angr = 0;
            this.ownHero.Hp = 30;
            this.ownHero.armor = 0;
            this.ownHero.frozen = false;
            this.ownHero.immuneWhileAttacking = false;
            this.ownHero.immune = false;
            this.ownHero.numAttacksThisTurn = 0;
            this.ownHero.windfury = false;

            //end of implementation

            //addCardsReal(homeHandManager.handCards);

            this.ownHeroName = HeroEnum.mage;
            this.ownHeroStartClass = TAG_CLASS.MAGE;
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

            this.anzOwnRaidleader = 0;
            this.anzOwnStormwindChamps = 0;
            this.anzOwnTundrarhino = 0;
            this.anzOwnTimberWolfs = 0;
            this.anzMurlocWarleader = 0;
            this.anzGrimscaleOracle = 0;
            this.anzOwnAuchenaiSoulpriest = 0;
            this.anzOwnsorcerersapprentice = 0;
            this.anzOwnsorcerersapprenticeStarted = 0;
            this.anzOwnSouthseacaptain = 0;
            this.anzOwnDragonConsortStarted = 0;

            this.ownMinionsDiedTurn = 0;

            //this.feugenDead = false;
            //this.stalaggDead = false;

            this.doublepriest = 0;

            this.ownBaronRivendare = 0;
            //#########################################

            this.ownWeaponDurability = 0;
            this.ownWeaponAttack = 0;
            this.ownWeaponName = CardDB.cardName.unknown;
            this.owncarddraw = 0;

            this.ownAbilityReady = true;
            this.ownHeroAblility = new Handmanager.Handcard(CardDB.Instance.getCardData(CardDB.Instance.cardNamestringToEnum("fireblast")));

            this.mobsplayedThisTurn = 0;
            this.startedWithMobsPlayedThisTurn = 0;// only change mobsplayedthisturm
            this.cardsPlayedThisTurn = 0;
            //todo:
            this.optionsPlayedThisTurn = 0;

            this.ueberladung = 0;

            this.ownHeroFatigue = 0;
            this.ownDeckSize = 30;

            //need the following for manacost-calculation
            this.ownHeroHpStarted = this.ownHero.Hp;
            this.ownWeaponAttackStarted = this.ownWeaponAttack;
            this.ownCardsCountStarted = this.owncards.Count;
            //this.ownMobsCountStarted = this.ownMinions.Count + this.enemyMinions.Count;

            this.playedmagierinderkirintor = false;
            this.playedPreparation = false;

            this.winzigebeschwoererin = 0;
            this.managespenst = 0;
            this.soeldnerDerVenture = 0;
            this.beschwoerungsportal = 0;
            this.nerubarweblord = 0;

            this.startedWithnerubarweblord = 0;
            this.startedWithbeschwoerungsportal = 0;
            this.startedWithManagespenst = 0;
            this.startedWithWinzigebeschwoererin = 0;
            this.startedWithsoeldnerDerVenture = 0;

            this.ownBaronRivendare = 0;

            this.loatheb = false;
            this.spellpower = 0;

            this.startedWithDamagedMinions = false;

            foreach (Minion m in this.ownMinions)
            {
                if (m.Hp < m.maxHp && m.Hp >= 1) this.startedWithDamagedMinions = true;
                if (m.playedThisTurn && m.name == CardDB.cardName.loatheb) this.loatheb = true;

                spellpower = spellpower + m.spellpower;
                if (m.silenced) continue;
                spellpower += m.handcard.card.spellpowervalue;
                if (m.name == CardDB.cardName.prophetvelen) this.doublepriest++;


                if (m.name == CardDB.cardName.pintsizedsummoner)
                {
                    this.winzigebeschwoererin++;
                    this.startedWithWinzigebeschwoererin++;
                }

                if (m.name == CardDB.cardName.manawraith)
                {
                    this.managespenst++;
                    this.startedWithManagespenst++;
                }
                if (m.name == CardDB.cardName.nerubarweblord)
                {
                    this.nerubarweblord++;
                    this.startedWithnerubarweblord++;
                }
                if (m.name == CardDB.cardName.venturecomercenary)
                {
                    this.soeldnerDerVenture++;
                    this.startedWithsoeldnerDerVenture++;
                }
                if (m.name == CardDB.cardName.summoningportal)
                {
                    this.beschwoerungsportal++;
                    this.startedWithbeschwoerungsportal++;
                }

                if (m.handcard.card.name == CardDB.cardName.baronrivendare)
                {
                    this.ownBaronRivendare++;
                }
                if (m.handcard.card.name == CardDB.cardName.kelthuzad)
                {
                    //this.needGraveyard = true;
                }

                if (m.name == CardDB.cardName.raidleader) this.anzOwnRaidleader++;
                if (m.name == CardDB.cardName.malganis) this.anzOwnMalGanis++;
                if (m.name == CardDB.cardName.stormwindchampion) this.anzOwnStormwindChamps++;
                if (m.name == CardDB.cardName.tundrarhino) this.anzOwnTundrarhino++;
                if (m.name == CardDB.cardName.timberwolf) this.anzOwnTimberWolfs++;
                if (m.name == CardDB.cardName.murlocwarleader) this.anzMurlocWarleader++;
                if (m.name == CardDB.cardName.grimscaleoracle) this.anzGrimscaleOracle++;
                if (m.name == CardDB.cardName.auchenaisoulpriest) this.anzOwnAuchenaiSoulpriest++;
                if (m.name == CardDB.cardName.sorcerersapprentice)
                {
                    this.anzOwnsorcerersapprentice++;
                    this.anzOwnsorcerersapprenticeStarted++;
                }
                if (m.name == CardDB.cardName.southseacaptain) this.anzOwnSouthseacaptain++;
                if (m.name == CardDB.cardName.chromaggus) this.anzOwnChromaggus++;
                if (m.name == CardDB.cardName.dragonconsort && anzOwnDragonConsort > 0) this.anzOwnDragonConsortStarted++;
                if (m.name == CardDB.cardName.mechwarper)
                {
                    this.anzOwnMechwarper++;
                    this.anzOwnMechwarperStarted++;
                }
                if (m.name == CardDB.cardName.steamwheedlesniper && this.ownHeroName == HeroEnum.hunter)
                {
                    this.weHaveSteamwheedleSniper = true;
                }

            }

            foreach (Handmanager.Handcard hc in this.owncards)
            {

                if (hc.card.name == CardDB.cardName.kelthuzad)
                {
                    //this.needGraveyard = true;
                }
            }
        }

        public Player(Player p) {

            //this.isOwnTurn = p.isOwnTurn;

            this.attacked = p.attacked;
            this.sEnemTurn = p.sEnemTurn;
            this.ownController = p.ownController;
            //this.ownHeroEntity = p.ownHeroEntity;
            //this.enemyHeroEntity = p.enemyHeroEntity;

            this.evaluatePenality = p.evaluatePenality;
            this.ownSecretsIDList.AddRange(p.ownSecretsIDList);

            this.mana = p.mana;
            this.manaTurnEnd = p.manaTurnEnd;
            this.ownMaxMana = p.ownMaxMana;
            addMinionsReal(p.ownMinions, ownMinions);
            this.ownHero = new Minion(p.ownHero);
            addCardsReal(p.owncards);

            this.ownHeroName = p.ownHeroName;

            this.playactions.AddRange(p.playactions);

            this.attackFaceHP = p.attackFaceHP;

            this.owncarddraw = p.owncarddraw;

            this.ownWeaponDurability = p.ownWeaponDurability;
            this.ownWeaponAttack = p.ownWeaponAttack;
            this.ownWeaponName = p.ownWeaponName;

            this.lostDamage = p.lostDamage;
            this.lostWeaponDamage = p.lostWeaponDamage;
            this.lostHeal = p.lostHeal;

            this.ownAbilityReady = p.ownAbilityReady;
            this.ownHeroAblility = new Handmanager.Handcard(p.ownHeroAblility);

            this.spellpower = 0;
            this.mobsplayedThisTurn = p.mobsplayedThisTurn;
            this.startedWithMobsPlayedThisTurn = p.startedWithMobsPlayedThisTurn;
            this.optionsPlayedThisTurn = p.optionsPlayedThisTurn;
            this.cardsPlayedThisTurn = p.cardsPlayedThisTurn;
            this.ueberladung = p.ueberladung;

            this.ownDeckSize = p.ownDeckSize;
            this.ownHeroFatigue = p.ownHeroFatigue;

            //need the following for manacost-calculation
            this.ownHeroHpStarted = p.ownHeroHpStarted;
            this.ownWeaponAttackStarted = p.ownWeaponAttackStarted;
            this.ownCardsCountStarted = p.ownCardsCountStarted;
            this.ownMobsCountStarted = p.ownMobsCountStarted;

            this.startedWithWinzigebeschwoererin = p.startedWithWinzigebeschwoererin;
            this.playedmagierinderkirintor = p.playedmagierinderkirintor;

            this.startedWithWinzigebeschwoererin = p.startedWithWinzigebeschwoererin;
            this.startedWithManagespenst = p.startedWithManagespenst;
            this.startedWithsoeldnerDerVenture = p.startedWithsoeldnerDerVenture;
            this.startedWithbeschwoerungsportal = p.startedWithbeschwoerungsportal;
            this.startedWithnerubarweblord = p.startedWithnerubarweblord;

            this.startedWithDamagedMinions = p.startedWithDamagedMinions;

            this.nerubarweblord = p.nerubarweblord;
            this.winzigebeschwoererin = p.winzigebeschwoererin;
            this.managespenst = p.managespenst;
            this.soeldnerDerVenture = p.soeldnerDerVenture;
            this.loatheb = p.loatheb;

            this.spellpower = p.spellpower;

            this.ownLastDiedMinion = p.ownLastDiedMinion;

            //####buffs#############################

            this.anzOwnRaidleader = p.anzOwnRaidleader;
            this.anzOwnMalGanis = p.anzOwnMalGanis;
            this.anzOwnStormwindChamps = p.anzOwnStormwindChamps;
            this.anzOwnTundrarhino = p.anzOwnTundrarhino;
            this.anzOwnTimberWolfs = p.anzOwnTimberWolfs;
            this.anzMurlocWarleader = p.anzMurlocWarleader;
            this.anzGrimscaleOracle = p.anzGrimscaleOracle;
            this.anzOwnAuchenaiSoulpriest = p.anzOwnAuchenaiSoulpriest;
            this.anzOwnsorcerersapprentice = p.anzOwnsorcerersapprentice;
            this.anzOwnsorcerersapprenticeStarted = p.anzOwnsorcerersapprenticeStarted;
            this.anzOwnSouthseacaptain = p.anzOwnSouthseacaptain;
            this.anzOwnMechwarper = p.anzOwnMechwarper;
            this.anzOwnMechwarperStarted = p.anzOwnMechwarperStarted;
            this.anzOwnChromaggus = p.anzOwnChromaggus;
            this.anzOwnDragonConsort = p.anzOwnDragonConsort;
            this.anzOwnDragonConsortStarted = p.anzOwnDragonConsortStarted;

            //implementation of loatheb
            this.enemyLoatheb = p.enemyLoatheb;
            this.enemyMillhouse = p.enemyMillhouse; 
            //end

            this.ownMinionsDiedTurn = p.ownMinionsDiedTurn;

            this.feugenDead = p.feugenDead;
            this.stalaggDead = p.stalaggDead;

            this.doublepriest = p.doublepriest;

            this.ownBaronRivendare = p.ownBaronRivendare;

            this.weHaveSteamwheedleSniper = p.weHaveSteamwheedleSniper;
            //#########################################

            this.tempanzOwnCards = this.owncards.Count;

        }

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
                this.owncards.Add(new Handmanager.Handcard(m));
            }

        }
    } 
    
}