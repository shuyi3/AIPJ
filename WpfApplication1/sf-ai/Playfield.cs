﻿namespace SilverfishAi
{
    using System;
    using System.Collections.Generic;

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

    //todo save "started" variables outside (they doesnt change)

    public class Playfield
    {
        //Todo: delete all new list<minion>
        //TODO: graveyard change (list <card,owner>)
        //Todo: vanish clear all auras/buffs (NEW1_004)
        bool isPingOwnMinion = false;

        public Silverfish sf;

        public bool logging = false;
        public bool complete = false;

        public int nextEntity = 70;

        public triggerCounter tempTrigger = new triggerCounter();

        //aura minions##########################
        //todo reduce buffing vars
        public int anzOwnRaidleader = 0;
        public int anzEnemyRaidleader = 0;
        public int anzOwnStormwindChamps = 0;
        public int anzEnemyStormwindChamps = 0;
        public int anzOwnTundrarhino = 0;
        public int anzEnemyTundrarhino = 0;
        public int anzOwnTimberWolfs = 0;
        public int anzEnemyTimberWolfs = 0;
        public int anzMurlocWarleader = 0;
        public int anzGrimscaleOracle = 0;
        public int anzOwnAuchenaiSoulpriest = 0;
        public int anzEnemyAuchenaiSoulpriest = 0;
        public int anzOwnsorcerersapprentice = 0;
        public int anzOwnsorcerersapprenticeStarted = 0;
        public int anzEnemysorcerersapprentice = 0;
        public int anzEnemysorcerersapprenticeStarted = 0;
        public int anzOwnSouthseacaptain = 0;
        public int anzEnemySouthseacaptain = 0;
        public int anzOwnMalGanis = 0;
        public int anzEnemyMalGanis = 0;
        public int anzOwnChromaggus = 0;
        public int anzEnemyChromaggus = 0;
        public int anzOwnDragonConsort = 0;
        public int anzOwnDragonConsortStarted = 0;

        public int anzOwnMechwarper = 0;
        public int anzOwnMechwarperStarted = 0;
        public int anzEnemyMechwarper = 0;
        public int anzEnemyMechwarperStarted = 0;

        public int anzEnemyTaunt = 0;
        public int ownMinionsDiedTurn = 0;
        public int enemyMinionsDiedTurn = 0;

        public bool feugenDead = false;
        public bool stalaggDead = false;

        public bool weHavePlayedMillhouseManastorm = false;
        public bool weHaveSteamwheedleSniper = false;
        public bool enemyHaveSteamwheedleSniper = false;

        public bool needGraveyard = false;


        public int doublepriest = 0;
        public int enemydoublepriest = 0;


        public int ownBaronRivendare = 0;
        public int enemyBaronRivendare = 0;
        //#########################################

        public int tempanzOwnCards = 0; // for Goblin Sapper
        public int tempanzEnemyCards = 0;// for Goblin Sapper

        public bool isOwnTurn = true; // its your turn?
        public int turnCounter = 0;
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
        public List<SecretItem> enemySecretList = new List<SecretItem>();

        public int enemySecretCount = 0;

        public Minion ownHero;
        public Minion enemyHero;
        public HeroEnum ownHeroName = HeroEnum.druid;
        public HeroEnum enemyHeroName = HeroEnum.druid;
        public TAG_CLASS ownHeroStartClass = TAG_CLASS.INVALID;
        public TAG_CLASS enemyHeroStartClass = TAG_CLASS.INVALID;

        public CardDB.cardName ownWeaponName = CardDB.cardName.unknown;
        public int ownWeaponAttack = 0;
        public int ownWeaponDurability = 0;

        public CardDB.cardName enemyWeaponName = CardDB.cardName.unknown;
        public int enemyWeaponAttack = 0;
        public int enemyWeaponDurability = 0;

        public List<Minion> ownMinions = new List<Minion>();
        public List<Minion> enemyMinions = new List<Minion>();
        public List<GraveYardItem> diedMinions = null;

        public List<Handmanager.Handcard> owncards = new List<Handmanager.Handcard>();
        public int owncarddraw = 0;

        public List<Action> playactions = new List<Action>();

        public int enemycarddraw = 0;
        public int enemyAnzCards = 0;

        public int spellpower = 0;
        public int enemyspellpower = 0;

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
        public int enemyCardsCountStarted = 0;
        public int ownHeroHpStarted = 30;
        public int enemyHeroHpStarted = 30;

        public int mobsplayedThisTurn = 0;
        public int startedWithMobsPlayedThisTurn = 0;

        public int optionsPlayedThisTurn = 0;
        public int cardsPlayedThisTurn = 0;
        public int ueberladung = 0; //=recall

        public int enemyOptionsDoneThisTurn = 0;

        public int ownMaxMana = 0;
        public int enemyMaxMana = 0;

        public int lostDamage = 0;
        public int lostHeal = 0;
        public int lostWeaponDamage = 0;

        public int ownDeckSize = 30;
        public int enemyDeckSize = 30;
        public int ownHeroFatigue = 0;
        public int enemyHeroFatigue = 0;

        public bool ownAbilityReady = false;
        public Handmanager.Handcard ownHeroAblility;

        public bool enemyAbilityReady = false;
        public Handmanager.Handcard enemyHeroAblility;

        // just for saving which minion to revive with secrets (=the first one that died);
        public CardDB.cardIDEnum revivingOwnMinion = CardDB.cardIDEnum.None;
        public CardDB.cardIDEnum revivingEnemyMinion = CardDB.cardIDEnum.None;
        public CardDB.cardIDEnum OwnLastDiedMinion = CardDB.cardIDEnum.None;

        public int shadowmadnessed = 0; //minions has switched controllers this turn.
        //Helpfunctions help = sf.Helpfunctions;


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

        public Playfield(Silverfish sf)
        {
            this.sf = sf;

            this.nextEntity = sf.hrtprozis.getMaxEntity();
            //this.simulateEnemyTurn = sf.Ai.simulateEnemyTurn;
            this.ownController = sf.Hrtprozis.getOwnController();

            //this.ownHeroEntity = sf.Hrtprozis.ownHeroEntity;
            //this.enemyHeroEntity = sf.Hrtprozis.enemyHeroEntitiy;

            this.mana = sf.Hrtprozis.currentMana;
            this.manaTurnEnd = this.mana;
            this.ownMaxMana = sf.Hrtprozis.ownMaxMana;
            this.enemyMaxMana = sf.Hrtprozis.enemyMaxMana;
            this.evaluatePenality = 0;
            this.ownSecretsIDList.AddRange(sf.Hrtprozis.ownSecretList);
            this.enemySecretCount = sf.Hrtprozis.enemySecretCount;


            this.attackFaceHP = sf.Hrtprozis.attackFaceHp;

            this.complete = false;

            addMinionsReal(sf.Hrtprozis.ownMinions, ownMinions);
            addMinionsReal(sf.Hrtprozis.enemyMinions, enemyMinions);
            this.ownHero = new Minion(sf.Hrtprozis.ownHero);
            this.enemyHero = new Minion(sf.Hrtprozis.enemyHero);
            addCardsReal(sf.Handmanager.handCards);

            this.enemySecretList.Clear();
            if (sf.Settings.useSecretsPlayArround)
            {
                foreach (SecretItem si in sf.Probabilitymaker.enemySecrets)
                {
                    this.enemySecretList.Add(new SecretItem(si));
                }
            }

            this.ownHeroName = sf.Hrtprozis.heroname;
            this.enemyHeroName = sf.Hrtprozis.enemyHeroname;
            this.ownHeroStartClass = sf.Hrtprozis.ownHeroStartClass;
            this.enemyHeroStartClass = sf.Hrtprozis.enemyHeroStartClass;

            /*
            this.enemyHeroHp = sf.Hrtprozis.enemyHp;
            this.ownHeroHp = sf.Hrtprozis.heroHp;
            this.ownHeroReady = sf.Hrtprozis.ownheroisread;
            this.ownHeroWindfury = sf.Hrtprozis.ownHeroWindfury;
            this.ownHeroNumAttackThisTurn = sf.Hrtprozis.ownHeroNumAttacksThisTurn;
            this.ownHeroFrozen = sf.Hrtprozis.herofrozen;
            this.enemyHeroFrozen = sf.Hrtprozis.enemyfrozen;
            this.ownheroAngr = sf.Hrtprozis.heroAtk;
            this.heroImmuneWhileAttacking = sf.Hrtprozis.heroImmuneToDamageWhileAttacking;
            this.ownHeroDefence = sf.Hrtprozis.heroDefence;
            this.enemyHeroDefence = sf.Hrtprozis.enemyDefence;
             */

            //####buffs#############################

            this.anzOwnRaidleader = 0;
            this.anzEnemyRaidleader = 0;
            this.anzOwnStormwindChamps = 0;
            this.anzEnemyStormwindChamps = 0;
            this.anzOwnTundrarhino = 0;
            this.anzEnemyTundrarhino = 0;
            this.anzOwnTimberWolfs = 0;
            this.anzEnemyTimberWolfs = 0;
            this.anzMurlocWarleader = 0;
            this.anzGrimscaleOracle = 0;
            this.anzOwnAuchenaiSoulpriest = 0;
            this.anzEnemyAuchenaiSoulpriest = 0;
            this.anzOwnsorcerersapprentice = 0;
            this.anzOwnsorcerersapprenticeStarted = 0;
            this.anzEnemysorcerersapprentice = 0;
            this.anzEnemysorcerersapprenticeStarted = 0;
            this.anzOwnSouthseacaptain = 0;
            this.anzEnemySouthseacaptain = 0;
            this.anzOwnDragonConsortStarted = 0;
            
            this.anzEnemyTaunt = 0;
            this.ownMinionsDiedTurn = 0;
            this.enemyMinionsDiedTurn = 0;

            this.feugenDead = sf.Probabilitymaker.feugenDead;
            this.stalaggDead = sf.Probabilitymaker.stalaggDead;

            this.weHavePlayedMillhouseManastorm = false;

            this.doublepriest = 0;
            this.enemydoublepriest = 0;

            this.ownBaronRivendare = 0;
            this.enemyBaronRivendare = 0;
            //#########################################

            this.ownWeaponDurability = sf.Hrtprozis.heroWeaponDurability;
            this.ownWeaponAttack = sf.Hrtprozis.heroWeaponAttack;
            this.ownWeaponName = sf.Hrtprozis.ownHeroWeapon;
            this.owncarddraw = 0;


            this.enemyWeaponAttack = sf.Hrtprozis.enemyWeaponAttack;//dont know jet
            this.enemyWeaponName = sf.Hrtprozis.enemyHeroWeapon;
            this.enemyWeaponDurability = sf.Hrtprozis.enemyWeaponDurability;
            this.enemycarddraw = 0;

            this.enemyAnzCards = sf.Handmanager.enemyAnzCards;

            this.ownAbilityReady = sf.Hrtprozis.ownAbilityisReady;
            this.ownHeroAblility = new Handmanager.Handcard(sf.Hrtprozis.heroAbility);
            this.enemyHeroAblility = new Handmanager.Handcard(sf.Hrtprozis.enemyAbility);
            this.enemyAbilityReady = false;


            this.mobsplayedThisTurn = sf.Hrtprozis.numMinionsPlayedThisTurn;
            this.startedWithMobsPlayedThisTurn = sf.Hrtprozis.numMinionsPlayedThisTurn;// only change mobsplayedthisturm
            this.cardsPlayedThisTurn = sf.Hrtprozis.cardsPlayedThisTurn;
            //todo:
            this.optionsPlayedThisTurn = sf.Hrtprozis.numOptionsPlayedThisTurn;

            this.ueberladung = sf.Hrtprozis.ueberladung;

            this.ownHeroFatigue = sf.Hrtprozis.ownHeroFatigue;
            this.enemyHeroFatigue = sf.Hrtprozis.enemyHeroFatigue;
            this.ownDeckSize = sf.Hrtprozis.ownDeckSize;
            this.enemyDeckSize = sf.Hrtprozis.enemyDeckSize;

            //need the following for manacost-calculation
            this.ownHeroHpStarted = this.ownHero.Hp;
            this.enemyHeroHpStarted = this.enemyHero.Hp;
            this.ownWeaponAttackStarted = this.ownWeaponAttack;
            this.ownCardsCountStarted = this.owncards.Count;
            this.enemyCardsCountStarted = this.enemyAnzCards;
            this.ownMobsCountStarted = this.ownMinions.Count + this.enemyMinions.Count;

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
            this.enemyBaronRivendare = 0;

            needGraveyard = false;
            this.loatheb = false;
            this.spellpower = 0;
            this.enemyspellpower = 0;

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
                    this.needGraveyard = true;
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
                    this.needGraveyard = true;
                }
            }

            foreach (Minion m in this.enemyMinions)
            {
                this.enemyspellpower = this.enemyspellpower + m.spellpower;
                enemyspellpower += m.handcard.card.spellpowervalue;
                if (m.silenced) continue;
                if (m.taunt) anzEnemyTaunt++;
                if (m.name == CardDB.cardName.prophetvelen) this.enemydoublepriest++;
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
                if (m.handcard.card.name == CardDB.cardName.baronrivendare)
                {
                    this.enemyBaronRivendare++;
                }
                if (m.handcard.card.name == CardDB.cardName.kelthuzad)
                {
                    this.needGraveyard = true;
                }

                if (m.name == CardDB.cardName.raidleader) this.anzEnemyRaidleader++;
                if (m.name == CardDB.cardName.malganis) this.anzEnemyMalGanis++;
                if (m.name == CardDB.cardName.stormwindchampion) this.anzEnemyStormwindChamps++;
                if (m.name == CardDB.cardName.tundrarhino) this.anzEnemyTundrarhino++;
                if (m.name == CardDB.cardName.timberwolf) this.anzEnemyTimberWolfs++;
                if (m.name == CardDB.cardName.murlocwarleader) this.anzMurlocWarleader++;
                if (m.name == CardDB.cardName.grimscaleoracle) this.anzGrimscaleOracle++;
                if (m.name == CardDB.cardName.auchenaisoulpriest) this.anzEnemyAuchenaiSoulpriest++;
                if (m.name == CardDB.cardName.sorcerersapprentice)
                {
                    this.anzEnemysorcerersapprentice++;
                    this.anzEnemysorcerersapprenticeStarted++;
                }
                if (m.name == CardDB.cardName.southseacaptain) this.anzEnemySouthseacaptain++;
                if (m.name == CardDB.cardName.chromaggus) this.anzEnemyChromaggus++;
                if (m.name == CardDB.cardName.mechwarper)
                {
                    this.anzEnemyMechwarper++;
                    this.anzEnemyMechwarperStarted++;
                }
                if (m.name == CardDB.cardName.steamwheedlesniper && this.enemyHeroName == HeroEnum.hunter)
                {
                    this.enemyHaveSteamwheedleSniper = true;
                }
            }
            if (this.enemySecretCount >= 1) this.needGraveyard = true;
            if (this.needGraveyard) this.diedMinions = new List<GraveYardItem>(sf.Probabilitymaker.turngraveyard);

            this.tempanzOwnCards = this.owncards.Count;
            this.tempanzEnemyCards = this.enemyAnzCards;


        }

        public Playfield(Playfield p)
        {
            sf = p.sf;

            this.nextEntity = p.nextEntity;

            this.isOwnTurn = p.isOwnTurn;
            this.turnCounter = p.turnCounter;

            this.attacked = p.attacked;
            this.sEnemTurn = p.sEnemTurn;
            this.ownController = p.ownController;
            //this.ownHeroEntity = p.ownHeroEntity;
            //this.enemyHeroEntity = p.enemyHeroEntity;

            this.evaluatePenality = p.evaluatePenality;
            this.ownSecretsIDList.AddRange(p.ownSecretsIDList);

            this.enemySecretCount = p.enemySecretCount;

            this.enemySecretList.Clear();

            if (sf.Settings.useSecretsPlayArround)
            {
                foreach (SecretItem si in p.enemySecretList)
                {
                    this.enemySecretList.Add(new SecretItem(si));
                }
            }

            this.mana = p.mana;
            this.manaTurnEnd = p.manaTurnEnd;
            this.ownMaxMana = p.ownMaxMana;
            this.enemyMaxMana = p.enemyMaxMana;
            addMinionsReal(p.ownMinions, ownMinions);
            addMinionsReal(p.enemyMinions, enemyMinions);
            this.ownHero = new Minion(p.ownHero);
            this.enemyHero = new Minion(p.enemyHero);
            addCardsReal(p.owncards);

            this.ownHeroName = p.ownHeroName;
            this.enemyHeroName = p.enemyHeroName;

            this.playactions.AddRange(p.playactions);
            this.complete = false;

            this.attackFaceHP = p.attackFaceHP;

            this.owncarddraw = p.owncarddraw;

            this.enemyWeaponAttack = p.enemyWeaponAttack;
            this.enemyWeaponDurability = p.enemyWeaponDurability;
            this.enemyWeaponName = p.enemyWeaponName;
            this.enemycarddraw = p.enemycarddraw;
            this.enemyAnzCards = p.enemyAnzCards;

            this.ownWeaponDurability = p.ownWeaponDurability;
            this.ownWeaponAttack = p.ownWeaponAttack;
            this.ownWeaponName = p.ownWeaponName;

            this.lostDamage = p.lostDamage;
            this.lostWeaponDamage = p.lostWeaponDamage;
            this.lostHeal = p.lostHeal;

            this.ownAbilityReady = p.ownAbilityReady;
            this.enemyAbilityReady = p.enemyAbilityReady;
            this.ownHeroAblility = new Handmanager.Handcard(p.ownHeroAblility);
            this.enemyHeroAblility = new Handmanager.Handcard(p.enemyHeroAblility);

            this.spellpower = 0;
            this.mobsplayedThisTurn = p.mobsplayedThisTurn;
            this.startedWithMobsPlayedThisTurn = p.startedWithMobsPlayedThisTurn;
            this.optionsPlayedThisTurn = p.optionsPlayedThisTurn;
            this.cardsPlayedThisTurn = p.cardsPlayedThisTurn;
            this.ueberladung = p.ueberladung;

            this.ownDeckSize = p.ownDeckSize;
            this.enemyDeckSize = p.enemyDeckSize;
            this.ownHeroFatigue = p.ownHeroFatigue;
            this.enemyHeroFatigue = p.enemyHeroFatigue;

            //need the following for manacost-calculation
            this.ownHeroHpStarted = p.ownHeroHpStarted;
            this.ownWeaponAttackStarted = p.ownWeaponAttackStarted;
            this.ownCardsCountStarted = p.ownCardsCountStarted;
            this.enemyCardsCountStarted = p.enemyCardsCountStarted;
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
            this.enemyspellpower = p.enemyspellpower;

            this.needGraveyard = p.needGraveyard;
            if (p.needGraveyard) this.diedMinions = new List<GraveYardItem>(p.diedMinions);
            this.OwnLastDiedMinion = p.OwnLastDiedMinion;

            //####buffs#############################

            this.anzOwnRaidleader = p.anzOwnRaidleader;
            this.anzEnemyRaidleader = p.anzEnemyRaidleader;
            this.anzOwnMalGanis = p.anzOwnMalGanis;
            this.anzEnemyMalGanis = p.anzEnemyMalGanis;
            this.anzOwnStormwindChamps = p.anzOwnStormwindChamps;
            this.anzEnemyStormwindChamps = p.anzEnemyStormwindChamps;
            this.anzOwnTundrarhino = p.anzOwnTundrarhino;
            this.anzEnemyTundrarhino = p.anzEnemyTundrarhino;
            this.anzOwnTimberWolfs = p.anzOwnTimberWolfs;
            this.anzEnemyTimberWolfs = p.anzEnemyTimberWolfs;
            this.anzMurlocWarleader = p.anzMurlocWarleader;
            this.anzGrimscaleOracle = p.anzGrimscaleOracle;
            this.anzOwnAuchenaiSoulpriest = p.anzOwnAuchenaiSoulpriest;
            this.anzEnemyAuchenaiSoulpriest = p.anzEnemyAuchenaiSoulpriest;
            this.anzOwnsorcerersapprentice = p.anzOwnsorcerersapprentice;
            this.anzOwnsorcerersapprenticeStarted = p.anzOwnsorcerersapprenticeStarted;
            this.anzEnemysorcerersapprentice = p.anzEnemysorcerersapprentice;
            this.anzEnemysorcerersapprenticeStarted = p.anzEnemysorcerersapprenticeStarted;
            this.anzOwnSouthseacaptain = p.anzOwnSouthseacaptain;
            this.anzEnemySouthseacaptain = p.anzEnemySouthseacaptain;
            this.anzOwnMechwarper = p.anzOwnMechwarper;
            this.anzOwnMechwarperStarted = p.anzOwnMechwarperStarted;
            this.anzEnemyMechwarper = p.anzEnemyMechwarper;
            this.anzEnemyMechwarperStarted = p.anzEnemyMechwarperStarted;
            this.anzOwnChromaggus = p.anzOwnChromaggus;
            this.anzEnemyChromaggus = p.anzEnemyChromaggus;
            this.anzOwnDragonConsort = p.anzOwnDragonConsort;
            this.anzOwnDragonConsortStarted = p.anzOwnDragonConsortStarted;

            this.anzEnemyTaunt = p.anzEnemyTaunt;
            this.ownMinionsDiedTurn = p.ownMinionsDiedTurn;
            this.enemyMinionsDiedTurn = p.enemyMinionsDiedTurn;

            this.feugenDead = p.feugenDead;
            this.stalaggDead = p.stalaggDead;

            this.weHavePlayedMillhouseManastorm = p.weHavePlayedMillhouseManastorm;

            this.doublepriest = p.doublepriest;
            this.enemydoublepriest = p.enemydoublepriest;

            this.ownBaronRivendare = p.ownBaronRivendare;
            this.enemyBaronRivendare = p.enemyBaronRivendare;

            this.weHaveSteamwheedleSniper = p.weHaveSteamwheedleSniper;
            this.enemyHaveSteamwheedleSniper = p.enemyHaveSteamwheedleSniper;
            //#########################################


            this.tempanzOwnCards = this.owncards.Count;
            this.tempanzEnemyCards = this.enemyAnzCards;

        }

        public void copyValuesFrom(Playfield p)
        {

        }

        public bool isEqual(Playfield p, bool logg)
        {
            if (logg)
            {
                if (this.value != p.value) return false;
            }
            if (this.enemySecretCount != p.enemySecretCount)
            {

                if (logg) sf.Helpfunctions.logg("enemy secrets changed ");
                return false;
            }

            if (this.enemySecretCount >= 1)
            {
                for (int i = 0; i < this.enemySecretList.Count; i++)
                {
                    if (!this.enemySecretList[i].isEqual(p.enemySecretList[i]))
                    {
                        if (logg) sf.Helpfunctions.logg("enemy secrets changed! ");
                        return false;
                    }
                }
            }

            if (this.mana != p.mana || this.enemyMaxMana != p.enemyMaxMana || this.ownMaxMana != p.ownMaxMana)
            {
                if (logg) sf.Helpfunctions.logg("mana changed " + this.mana + " " + p.mana + " " + this.enemyMaxMana + " " + p.enemyMaxMana + " " + this.ownMaxMana + " " + p.ownMaxMana);
                return false;
            }

            if (this.ownDeckSize != p.ownDeckSize || this.enemyDeckSize != p.enemyDeckSize || this.ownHeroFatigue != p.ownHeroFatigue || this.enemyHeroFatigue != p.enemyHeroFatigue)
            {
                if (logg) sf.Helpfunctions.logg("deck/fatigue changed " + this.ownDeckSize + " " + p.ownDeckSize + " " + this.enemyDeckSize + " " + p.enemyDeckSize + " " + this.ownHeroFatigue + " " + p.ownHeroFatigue + " " + this.enemyHeroFatigue + " " + p.enemyHeroFatigue);
            }

            if (this.cardsPlayedThisTurn != p.cardsPlayedThisTurn || this.mobsplayedThisTurn != p.mobsplayedThisTurn || this.ueberladung != p.ueberladung || this.ownAbilityReady != p.ownAbilityReady)
            {
                if (logg) sf.Helpfunctions.logg("stuff changed " + this.cardsPlayedThisTurn + " " + p.cardsPlayedThisTurn + " " + this.mobsplayedThisTurn + " " + p.mobsplayedThisTurn + " " + this.ueberladung + " " + p.ueberladung + " " + this.ownAbilityReady + " " + p.ownAbilityReady);
                return false;
            }

            if (this.ownHeroName != p.ownHeroName || this.enemyHeroName != p.enemyHeroName)
            {
                if (logg) sf.Helpfunctions.logg("hero name changed ");
                return false;
            }

            if (this.ownHero.Hp != p.ownHero.Hp || this.ownHero.Angr != p.ownHero.Angr || this.ownHero.armor != p.ownHero.armor || this.ownHero.frozen != p.ownHero.frozen || this.ownHero.immuneWhileAttacking != p.ownHero.immuneWhileAttacking || this.ownHero.immune != p.ownHero.immune)
            {
                if (logg) sf.Helpfunctions.logg("ownhero changed " + this.ownHero.Hp + " " + p.ownHero.Hp + " " + this.ownHero.Angr + " " + p.ownHero.Angr + " " + this.ownHero.armor + " " + p.ownHero.armor + " " + this.ownHero.frozen + " " + p.ownHero.frozen + " " + this.ownHero.immuneWhileAttacking + " " + p.ownHero.immuneWhileAttacking + " " + this.ownHero.immune + " " + p.ownHero.immune);
                return false;
            }
            if (this.ownHero.Ready != p.ownHero.Ready || this.ownWeaponAttack != p.ownWeaponAttack || this.ownWeaponDurability != p.ownWeaponDurability || this.ownHero.numAttacksThisTurn != p.ownHero.numAttacksThisTurn || this.ownHero.windfury != p.ownHero.windfury)
            {
                if (logg) sf.Helpfunctions.logg("weapon changed " + this.ownHero.Ready + " " + p.ownHero.Ready + " " + this.ownWeaponAttack + " " + p.ownWeaponAttack + " " + this.ownWeaponDurability + " " + p.ownWeaponDurability + " " + this.ownHero.numAttacksThisTurn + " " + p.ownHero.numAttacksThisTurn + " " + this.ownHero.windfury + " " + p.ownHero.windfury);
                return false;
            }
            if (this.enemyHero.Hp != p.enemyHero.Hp || this.enemyWeaponAttack != p.enemyWeaponAttack || this.enemyHero.armor != p.enemyHero.armor || this.enemyWeaponDurability != p.enemyWeaponDurability || this.enemyHero.frozen != p.enemyHero.frozen || this.enemyHero.immune != p.enemyHero.immune)
            {
                if (logg) sf.Helpfunctions.logg("enemyhero changed " + this.enemyHero.Hp + " " + p.enemyHero.Hp + " " + this.enemyWeaponAttack + " " + p.enemyWeaponAttack + " " + this.enemyHero.armor + " " + p.enemyHero.armor + " " + this.enemyWeaponDurability + " " + p.enemyWeaponDurability + " " + this.enemyHero.frozen + " " + p.enemyHero.frozen + " " + this.enemyHero.immune + " " + p.enemyHero.immune);
                return false;
            }

            /*if (this.auchenaiseelenpriesterin != p.auchenaiseelenpriesterin || this.winzigebeschwoererin != p.winzigebeschwoererin || this.zauberlehrling != p.zauberlehrling || this.managespenst != p.managespenst || this.soeldnerDerVenture != p.soeldnerDerVenture || this.beschwoerungsportal != p.beschwoerungsportal || this.doublepriest != p.doublepriest)
            {
                sf.Helpfunctions.logg("special minions changed " + this.auchenaiseelenpriesterin + " " + p.auchenaiseelenpriesterin + " " + this.winzigebeschwoererin + " " + p.winzigebeschwoererin + " " + this.zauberlehrling + " " + p.zauberlehrling + " " + this.managespenst + " " + p.managespenst + " " + this.soeldnerDerVenture + " " + p.soeldnerDerVenture + " " + this.beschwoerungsportal + " " + p.beschwoerungsportal + " " + this.doublepriest + " " + p.doublepriest);
                return false;
            }*/

            if (this.ownHeroAblility.card.name != p.ownHeroAblility.card.name)
            {
                if (logg) sf.Helpfunctions.logg("hero ability changed ");
                return false;
            }

            if (this.spellpower != p.spellpower)
            {
                if (logg) sf.Helpfunctions.logg("spellpower changed");
                return false;
            }

            if (this.ownMinions.Count != p.ownMinions.Count || this.enemyMinions.Count != p.enemyMinions.Count || this.owncards.Count != p.owncards.Count)
            {
                if (logg) sf.Helpfunctions.logg("minions count or hand changed");
                return false;
            }

            bool minionbool = true;
            for (int i = 0; i < this.ownMinions.Count; i++)
            {
                Minion dis = this.ownMinions[i]; Minion pis = p.ownMinions[i];
                //if (dis.entitiyID == 0) dis.entitiyID = pis.entitiyID;
                //if (pis.entitiyID == 0) pis.entitiyID = dis.entitiyID;
                if (dis.name != pis.name) minionbool = false;
                if (dis.Angr != pis.Angr || dis.Hp != pis.Hp || dis.maxHp != pis.maxHp || dis.numAttacksThisTurn != pis.numAttacksThisTurn) minionbool = false;
                if (dis.Ready != pis.Ready) minionbool = false; // includes frozen, exhaunted
                if (dis.playedThisTurn != pis.playedThisTurn) minionbool = false;
                if (dis.silenced != pis.silenced || dis.stealth != pis.stealth || dis.taunt != pis.taunt || dis.windfury != pis.windfury || dis.zonepos != pis.zonepos) minionbool = false;
                if (dis.divineshild != pis.divineshild || dis.cantLowerHPbelowONE != pis.cantLowerHPbelowONE || dis.immune != pis.immune) minionbool = false;
                if (dis.ownBlessingOfWisdom != pis.ownBlessingOfWisdom || dis.enemyBlessingOfWisdom != pis.enemyBlessingOfWisdom) minionbool = false;
                if (dis.destroyOnEnemyTurnStart != pis.destroyOnEnemyTurnStart || dis.destroyOnEnemyTurnEnd != pis.destroyOnEnemyTurnEnd || dis.destroyOnOwnTurnEnd != pis.destroyOnOwnTurnEnd || dis.destroyOnOwnTurnStart != pis.destroyOnOwnTurnStart) minionbool = false;
                if (dis.ancestralspirit != pis.ancestralspirit || dis.souloftheforest != pis.souloftheforest) minionbool = false;

            }
            if (minionbool == false)
            {
                if (logg) sf.Helpfunctions.logg("ownminions changed");
                return false;
            }

            for (int i = 0; i < this.enemyMinions.Count; i++)
            {
                Minion dis = this.enemyMinions[i]; Minion pis = p.enemyMinions[i];
                //if (dis.entitiyID == 0) dis.entitiyID = pis.entitiyID;
                //if (pis.entitiyID == 0) pis.entitiyID = dis.entitiyID;
                if (dis.name != pis.name) minionbool = false;
                if (dis.Angr != pis.Angr || dis.Hp != pis.Hp || dis.maxHp != pis.maxHp || dis.numAttacksThisTurn != pis.numAttacksThisTurn) minionbool = false;
                if (dis.Ready != pis.Ready) minionbool = false; // includes frozen, exhaunted
                if (dis.playedThisTurn != pis.playedThisTurn) minionbool = false;
                if (dis.silenced != pis.silenced || dis.stealth != pis.stealth || dis.taunt != pis.taunt || dis.windfury != pis.windfury || dis.zonepos != pis.zonepos) minionbool = false;
                if (dis.divineshild != pis.divineshild || dis.cantLowerHPbelowONE != pis.cantLowerHPbelowONE || dis.immune != pis.immune) minionbool = false;
                if (dis.ownBlessingOfWisdom != pis.ownBlessingOfWisdom || dis.enemyBlessingOfWisdom != pis.enemyBlessingOfWisdom) minionbool = false;
                if (dis.destroyOnEnemyTurnStart != pis.destroyOnEnemyTurnStart || dis.destroyOnEnemyTurnEnd != pis.destroyOnEnemyTurnEnd || dis.destroyOnOwnTurnEnd != pis.destroyOnOwnTurnEnd || dis.destroyOnOwnTurnStart != pis.destroyOnOwnTurnStart) minionbool = false;
                if (dis.ancestralspirit != pis.ancestralspirit || dis.souloftheforest != pis.souloftheforest) minionbool = false;
            }
            if (minionbool == false)
            {
                if (logg) sf.Helpfunctions.logg("enemyminions changed");
                return false;
            }

            for (int i = 0; i < this.owncards.Count; i++)
            {
                Handmanager.Handcard dishc = this.owncards[i]; Handmanager.Handcard pishc = p.owncards[i];
                if (dishc.position != pishc.position || dishc.entity != pishc.entity || dishc.getManaCost(this) != pishc.getManaCost(p))
                {
                    if (logg) sf.Helpfunctions.logg("handcard changed: " + dishc.card.name);
                    return false;
                }
            }

            for (int i = 0; i < this.ownMinions.Count; i++)
            {
                Minion dis = this.ownMinions[i]; Minion pis = p.ownMinions[i];
                if (dis.entitiyID != pis.entitiyID) sf.Ai.updateEntitiy(pis.entitiyID, dis.entitiyID);

            }

            for (int i = 0; i < this.enemyMinions.Count; i++)
            {
                Minion dis = this.enemyMinions[i]; Minion pis = p.enemyMinions[i];
                if (dis.entitiyID != pis.entitiyID) sf.Ai.updateEntitiy(pis.entitiyID, dis.entitiyID);

            }
            if (this.ownSecretsIDList.Count != p.ownSecretsIDList.Count)
            {
                if (logg) sf.Helpfunctions.logg("secretsCount changed");
                return false;
            }
            for (int i = 0; i < this.ownSecretsIDList.Count; i++)
            {
                if (this.ownSecretsIDList[i] != p.ownSecretsIDList[i])
                {
                    if (logg) sf.Helpfunctions.logg("secrets changed");
                    return false;
                }
            }
            return true;
        }

        public bool isEqual(Playfield p, bool logg, Action a)
        {
            if (logg)
            {
                if (this.value != p.value) return false;
            }
            if (this.enemySecretCount != p.enemySecretCount)
            {

                if (logg) sf.Helpfunctions.logg("enemy secrets changed ");
                return false;
            }

            if (this.enemySecretCount >= 1)
            {
                for (int i = 0; i < this.enemySecretList.Count; i++)
                {
                    if (!this.enemySecretList[i].isEqual(p.enemySecretList[i]))
                    {
                        if (logg) sf.Helpfunctions.logg("enemy secrets changed! ");
                        return false;
                    }
                }
            }

            if (this.mana != p.mana || this.enemyMaxMana != p.enemyMaxMana || this.ownMaxMana != p.ownMaxMana)
            {
                if (logg) sf.Helpfunctions.logg("mana changed " + this.mana + " " + p.mana + " " + this.enemyMaxMana + " " + p.enemyMaxMana + " " + this.ownMaxMana + " " + p.ownMaxMana);
                return false;
            }

            if (this.ownDeckSize != p.ownDeckSize || this.enemyDeckSize != p.enemyDeckSize || this.ownHeroFatigue != p.ownHeroFatigue || this.enemyHeroFatigue != p.enemyHeroFatigue)
            {
                if (logg) sf.Helpfunctions.logg("deck/fatigue changed " + this.ownDeckSize + " " + p.ownDeckSize + " " + this.enemyDeckSize + " " + p.enemyDeckSize + " " + this.ownHeroFatigue + " " + p.ownHeroFatigue + " " + this.enemyHeroFatigue + " " + p.enemyHeroFatigue);
            }

            if (this.cardsPlayedThisTurn != p.cardsPlayedThisTurn || this.mobsplayedThisTurn != p.mobsplayedThisTurn || this.ueberladung != p.ueberladung || this.ownAbilityReady != p.ownAbilityReady)
            {
                if (logg) sf.Helpfunctions.logg("stuff changed " + this.cardsPlayedThisTurn + " " + p.cardsPlayedThisTurn + " " + this.mobsplayedThisTurn + " " + p.mobsplayedThisTurn + " " + this.ueberladung + " " + p.ueberladung + " " + this.ownAbilityReady + " " + p.ownAbilityReady);
                return false;
            }

            if (this.ownHeroName != p.ownHeroName || this.enemyHeroName != p.enemyHeroName)
            {
                if (logg) sf.Helpfunctions.logg("hero name changed ");
                return false;
            }

            if (this.ownHero.Hp != p.ownHero.Hp || this.ownHero.Angr != p.ownHero.Angr || this.ownHero.armor != p.ownHero.armor || this.ownHero.frozen != p.ownHero.frozen || this.ownHero.immuneWhileAttacking != p.ownHero.immuneWhileAttacking || this.ownHero.immune != p.ownHero.immune)
            {
                if (logg) sf.Helpfunctions.logg("ownhero changed " + this.ownHero.Hp + " " + p.ownHero.Hp + " " + this.ownHero.Angr + " " + p.ownHero.Angr + " " + this.ownHero.armor + " " + p.ownHero.armor + " " + this.ownHero.frozen + " " + p.ownHero.frozen + " " + this.ownHero.immuneWhileAttacking + " " + p.ownHero.immuneWhileAttacking + " " + this.ownHero.immune + " " + p.ownHero.immune);
                return false;
            }
            if (this.ownHero.Ready != p.ownHero.Ready || this.ownWeaponAttack != p.ownWeaponAttack || this.ownWeaponDurability != p.ownWeaponDurability || this.ownHero.numAttacksThisTurn != p.ownHero.numAttacksThisTurn || this.ownHero.windfury != p.ownHero.windfury)
            {
                if (logg) sf.Helpfunctions.logg("weapon changed " + this.ownHero.Ready + " " + p.ownHero.Ready + " " + this.ownWeaponAttack + " " + p.ownWeaponAttack + " " + this.ownWeaponDurability + " " + p.ownWeaponDurability + " " + this.ownHero.numAttacksThisTurn + " " + p.ownHero.numAttacksThisTurn + " " + this.ownHero.windfury + " " + p.ownHero.windfury);
                return false;
            }
            if (this.enemyHero.Hp != p.enemyHero.Hp || this.enemyWeaponAttack != p.enemyWeaponAttack || this.enemyHero.armor != p.enemyHero.armor || this.enemyWeaponDurability != p.enemyWeaponDurability || this.enemyHero.frozen != p.enemyHero.frozen || this.enemyHero.immune != p.enemyHero.immune)
            {
                if (logg) sf.Helpfunctions.logg("enemyhero changed " + this.enemyHero.Hp + " " + p.enemyHero.Hp + " " + this.enemyWeaponAttack + " " + p.enemyWeaponAttack + " " + this.enemyHero.armor + " " + p.enemyHero.armor + " " + this.enemyWeaponDurability + " " + p.enemyWeaponDurability + " " + this.enemyHero.frozen + " " + p.enemyHero.frozen + " " + this.enemyHero.immune + " " + p.enemyHero.immune);
                return false;
            }

            /*if (this.auchenaiseelenpriesterin != p.auchenaiseelenpriesterin || this.winzigebeschwoererin != p.winzigebeschwoererin || this.zauberlehrling != p.zauberlehrling || this.managespenst != p.managespenst || this.soeldnerDerVenture != p.soeldnerDerVenture || this.beschwoerungsportal != p.beschwoerungsportal || this.doublepriest != p.doublepriest)
            {
                sf.Helpfunctions.logg("special minions changed " + this.auchenaiseelenpriesterin + " " + p.auchenaiseelenpriesterin + " " + this.winzigebeschwoererin + " " + p.winzigebeschwoererin + " " + this.zauberlehrling + " " + p.zauberlehrling + " " + this.managespenst + " " + p.managespenst + " " + this.soeldnerDerVenture + " " + p.soeldnerDerVenture + " " + this.beschwoerungsportal + " " + p.beschwoerungsportal + " " + this.doublepriest + " " + p.doublepriest);
                return false;
            }*/

            if (this.ownHeroAblility.card.name != p.ownHeroAblility.card.name)
            {
                if (logg) sf.Helpfunctions.logg("hero ability changed ");
                return false;
            }

            if (this.spellpower != p.spellpower)
            {
                if (logg) sf.Helpfunctions.logg("spellpower changed");
                return false;
            }

            if (this.ownMinions.Count != p.ownMinions.Count || this.enemyMinions.Count != p.enemyMinions.Count || this.owncards.Count != p.owncards.Count)
            {
                if (logg) sf.Helpfunctions.logg("minions count or hand changed");
                return false;
            }

            bool minionbool = true;
            for (int i = 0; i < this.ownMinions.Count; i++)
            {
                Minion dis = this.ownMinions[i]; Minion pis = p.ownMinions[i];
                //if (dis.entitiyID == 0) dis.entitiyID = pis.entitiyID;
                //if (pis.entitiyID == 0) pis.entitiyID = dis.entitiyID;
                if (dis.name != pis.name) minionbool = false;
                if (dis.Angr != pis.Angr || dis.Hp != pis.Hp || dis.maxHp != pis.maxHp || dis.numAttacksThisTurn != pis.numAttacksThisTurn) minionbool = false;
                if (dis.Ready != pis.Ready) minionbool = false; // includes frozen, exhaunted
                if (dis.playedThisTurn != pis.playedThisTurn) minionbool = false;
                if (dis.silenced != pis.silenced || dis.stealth != pis.stealth || dis.taunt != pis.taunt || dis.windfury != pis.windfury || dis.zonepos != pis.zonepos) minionbool = false;
                if (dis.divineshild != pis.divineshild || dis.cantLowerHPbelowONE != pis.cantLowerHPbelowONE || dis.immune != pis.immune) minionbool = false;
                if (dis.ownBlessingOfWisdom != pis.ownBlessingOfWisdom || dis.enemyBlessingOfWisdom != pis.enemyBlessingOfWisdom) minionbool = false;
                if (dis.destroyOnEnemyTurnStart != pis.destroyOnEnemyTurnStart || dis.destroyOnEnemyTurnEnd != pis.destroyOnEnemyTurnEnd || dis.destroyOnOwnTurnEnd != pis.destroyOnOwnTurnEnd || dis.destroyOnOwnTurnStart != pis.destroyOnOwnTurnStart) minionbool = false;
                if (dis.ancestralspirit != pis.ancestralspirit || dis.souloftheforest != pis.souloftheforest) minionbool = false;

            }
            if (minionbool == false)
            {
                if (logg) sf.Helpfunctions.logg("ownminions changed");
                return false;
            }

            for (int i = 0; i < this.enemyMinions.Count; i++)
            {
                Minion dis = this.enemyMinions[i]; Minion pis = p.enemyMinions[i];
                //if (dis.entitiyID == 0) dis.entitiyID = pis.entitiyID;
                //if (pis.entitiyID == 0) pis.entitiyID = dis.entitiyID;
                if (dis.name != pis.name) minionbool = false;
                if (dis.Angr != pis.Angr || dis.Hp != pis.Hp || dis.maxHp != pis.maxHp || dis.numAttacksThisTurn != pis.numAttacksThisTurn) minionbool = false;
                if (dis.Ready != pis.Ready) minionbool = false; // includes frozen, exhaunted
                if (dis.playedThisTurn != pis.playedThisTurn) minionbool = false;
                if (dis.silenced != pis.silenced || dis.stealth != pis.stealth || dis.taunt != pis.taunt || dis.windfury != pis.windfury || dis.zonepos != pis.zonepos) minionbool = false;
                if (dis.divineshild != pis.divineshild || dis.cantLowerHPbelowONE != pis.cantLowerHPbelowONE || dis.immune != pis.immune) minionbool = false;
                if (dis.ownBlessingOfWisdom != pis.ownBlessingOfWisdom || dis.enemyBlessingOfWisdom != pis.enemyBlessingOfWisdom) minionbool = false;
                if (dis.destroyOnEnemyTurnStart != pis.destroyOnEnemyTurnStart || dis.destroyOnEnemyTurnEnd != pis.destroyOnEnemyTurnEnd || dis.destroyOnOwnTurnEnd != pis.destroyOnOwnTurnEnd || dis.destroyOnOwnTurnStart != pis.destroyOnOwnTurnStart) minionbool = false;
                if (dis.ancestralspirit != pis.ancestralspirit || dis.souloftheforest != pis.souloftheforest) minionbool = false;
            }
            if (minionbool == false)
            {
                if (logg) sf.Helpfunctions.logg("enemyminions changed");
                return false;
            }

            for (int i = 0; i < this.owncards.Count; i++)
            {
                Handmanager.Handcard dishc = this.owncards[i]; Handmanager.Handcard pishc = p.owncards[i];
                if (dishc.position != pishc.position || dishc.entity != pishc.entity || dishc.getManaCost(this) != pishc.getManaCost(p))
                {
                    if (logg) sf.Helpfunctions.logg("handcard changed: " + dishc.card.name);
                    return false;
                }
            }

            for (int i = 0; i < this.ownMinions.Count; i++)
            {
                Minion dis = this.ownMinions[i]; Minion pis = p.ownMinions[i];
                if (dis.entitiyID != pis.entitiyID) sf.Ai.updateEntitiy(pis.entitiyID, dis.entitiyID);

            }

            for (int i = 0; i < this.enemyMinions.Count; i++)
            {
                Minion dis = this.enemyMinions[i]; Minion pis = p.enemyMinions[i];
                if (dis.entitiyID != pis.entitiyID) sf.Ai.updateEntitiy(pis.entitiyID, dis.entitiyID);

            }
            if (this.ownSecretsIDList.Count != p.ownSecretsIDList.Count)
            {
                if (logg) sf.Helpfunctions.logg("secretsCount changed");
                return false;
            }
            for (int i = 0; i < this.ownSecretsIDList.Count; i++)
            {
                if (this.ownSecretsIDList[i] != p.ownSecretsIDList[i])
                {
                    if (logg) sf.Helpfunctions.logg("secrets changed");
                    return false;
                }
            }
            return true;
        }


        public bool isEqualf(Playfield p)
        {
            if (this.value != p.value) return false;

            if (this.ownMinions.Count != p.ownMinions.Count || this.enemyMinions.Count != p.enemyMinions.Count || this.owncards.Count != p.owncards.Count) return false;

            if (this.cardsPlayedThisTurn != p.cardsPlayedThisTurn || this.mobsplayedThisTurn != p.mobsplayedThisTurn || this.ueberladung != p.ueberladung || this.ownAbilityReady != p.ownAbilityReady) return false;

            if (this.mana != p.mana || this.enemyMaxMana != p.enemyMaxMana || this.ownMaxMana != p.ownMaxMana) return false;

            if (this.ownHeroName != p.ownHeroName || this.enemyHeroName != p.enemyHeroName || this.enemySecretCount != p.enemySecretCount) return false;

            if (this.ownHero.Hp != p.ownHero.Hp || this.ownHero.Angr != p.ownHero.Angr || this.ownHero.armor != p.ownHero.armor || this.ownHero.frozen != p.ownHero.frozen || this.ownHero.immuneWhileAttacking != p.ownHero.immuneWhileAttacking || this.ownHero.immune != p.ownHero.immune) return false;

            if (this.ownHero.Ready != p.ownHero.Ready || this.ownWeaponAttack != p.ownWeaponAttack || this.ownWeaponDurability != p.ownWeaponDurability || this.ownHero.numAttacksThisTurn != p.ownHero.numAttacksThisTurn || this.ownHero.windfury != p.ownHero.windfury) return false;

            if (this.enemyHero.Hp != p.enemyHero.Hp || this.enemyWeaponAttack != p.enemyWeaponAttack || this.enemyHero.armor != p.enemyHero.armor || this.enemyWeaponDurability != p.enemyWeaponDurability || this.enemyHero.frozen != p.enemyHero.frozen || this.enemyHero.immune != p.enemyHero.immune) return false;

            if (this.ownHeroAblility.card.name != p.ownHeroAblility.card.name || this.spellpower != p.spellpower) return false;

            bool minionbool = true;
            for (int i = 0; i < this.ownMinions.Count; i++)
            {
                Minion dis = this.ownMinions[i]; Minion pis = p.ownMinions[i];
                //if (dis.entitiyID == 0) dis.entitiyID = pis.entitiyID;
                //if (pis.entitiyID == 0) pis.entitiyID = dis.entitiyID;
                if (dis.entitiyID != pis.entitiyID) minionbool = false;
                if (dis.Angr != pis.Angr || dis.Hp != pis.Hp || dis.maxHp != pis.maxHp || dis.numAttacksThisTurn != pis.numAttacksThisTurn) minionbool = false;
                if (dis.Ready != pis.Ready) minionbool = false; // includes frozen, exhaunted
                if (dis.playedThisTurn != pis.playedThisTurn) minionbool = false;
                if (dis.silenced != pis.silenced || dis.stealth != pis.stealth || dis.taunt != pis.taunt || dis.windfury != pis.windfury || dis.zonepos != pis.zonepos) minionbool = false;
                if (dis.divineshild != pis.divineshild || dis.cantLowerHPbelowONE != pis.cantLowerHPbelowONE || dis.immune != pis.immune) minionbool = false;
                if (dis.ownBlessingOfWisdom != pis.ownBlessingOfWisdom || dis.enemyBlessingOfWisdom != pis.enemyBlessingOfWisdom) minionbool = false;
                if (dis.destroyOnEnemyTurnStart != pis.destroyOnEnemyTurnStart || dis.destroyOnEnemyTurnEnd != pis.destroyOnEnemyTurnEnd || dis.destroyOnOwnTurnEnd != pis.destroyOnOwnTurnEnd || dis.destroyOnOwnTurnStart != pis.destroyOnOwnTurnStart) minionbool = false;
                if (dis.ancestralspirit != pis.ancestralspirit || dis.souloftheforest != pis.souloftheforest) minionbool = false;
                if (minionbool == false) break;
            }
            if (minionbool == false)
            {

                return false;
            }

            for (int i = 0; i < this.enemyMinions.Count; i++)
            {
                Minion dis = this.enemyMinions[i]; Minion pis = p.enemyMinions[i];
                //if (dis.entitiyID == 0) dis.entitiyID = pis.entitiyID;
                //if (pis.entitiyID == 0) pis.entitiyID = dis.entitiyID;
                if (dis.entitiyID != pis.entitiyID) minionbool = false;
                if (dis.Angr != pis.Angr || dis.Hp != pis.Hp || dis.maxHp != pis.maxHp || dis.numAttacksThisTurn != pis.numAttacksThisTurn) minionbool = false;
                if (dis.Ready != pis.Ready) minionbool = false; // includes frozen, exhaunted
                if (dis.playedThisTurn != pis.playedThisTurn) minionbool = false;
                if (dis.silenced != pis.silenced || dis.stealth != pis.stealth || dis.taunt != pis.taunt || dis.windfury != pis.windfury || dis.zonepos != pis.zonepos) minionbool = false;
                if (dis.divineshild != pis.divineshild || dis.cantLowerHPbelowONE != pis.cantLowerHPbelowONE || dis.immune != pis.immune) minionbool = false;
                if (dis.ownBlessingOfWisdom != pis.ownBlessingOfWisdom || dis.enemyBlessingOfWisdom != pis.enemyBlessingOfWisdom) minionbool = false;
                if (dis.destroyOnEnemyTurnStart != pis.destroyOnEnemyTurnStart || dis.destroyOnEnemyTurnEnd != pis.destroyOnEnemyTurnEnd || dis.destroyOnOwnTurnEnd != pis.destroyOnOwnTurnEnd || dis.destroyOnOwnTurnStart != pis.destroyOnOwnTurnStart) minionbool = false;
                if (dis.ancestralspirit != pis.ancestralspirit || dis.souloftheforest != pis.souloftheforest) minionbool = false;
                if (minionbool == false) break;
            }
            if (minionbool == false)
            {
                return false;
            }

            for (int i = 0; i < this.owncards.Count; i++)
            {
                Handmanager.Handcard dishc = this.owncards[i]; Handmanager.Handcard pishc = p.owncards[i];
                if (dishc.position != pishc.position || dishc.entity != pishc.entity || dishc.manacost != pishc.manacost)
                {
                    return false;
                }
            }

            if (this.enemySecretCount >= 1)
            {
                for (int i = 0; i < this.enemySecretList.Count; i++)
                {
                    if (!this.enemySecretList[i].isEqual(p.enemySecretList[i]))
                    {
                        return false;
                    }
                }
            }

            if (this.ownSecretsIDList.Count != p.ownSecretsIDList.Count)
            {
                return false;
            }
            for (int i = 0; i < this.ownSecretsIDList.Count; i++)
            {
                if (this.ownSecretsIDList[i] != p.ownSecretsIDList[i])
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            int retval = 0;
            retval += 10000 * this.ownMinions.Count + 100 * this.enemyMinions.Count + 1000 * this.mana + 100000 * (this.ownHero.Hp + this.enemyHero.Hp) + this.owncards.Count + this.enemycarddraw + this.cardsPlayedThisTurn + this.mobsplayedThisTurn + this.ownHero.Angr + this.ownHero.armor + this.ownWeaponAttack + this.enemyWeaponDurability;
            return retval;
        }


        //stuff for playing around enemy aoes
        public void enemyPlaysAoe(int pprob, int pprob2)
        {
            if (!this.loatheb)
            {
                Playfield p = new Playfield(this);
                float oldval = sf.Ai.botBase.getPlayfieldValue(p);
                p.value = int.MinValue;
                p.EnemyCardPlaying(p.enemyHeroStartClass, p.mana, p.enemyAnzCards, pprob, pprob2);
                float newval = sf.Ai.botBase.getPlayfieldValue(p);
                p.value = int.MinValue;
                if (oldval > newval) // new board is better for enemy (value is smaller)
                {
                    this.copyValuesFrom(p);
                }
            }
        }

        public int EnemyCardPlaying(TAG_CLASS enemyHeroStrtClass, int currmana, int cardcount, int playAroundProb, int pap2)
        {
            int mana = currmana;
            if (cardcount == 0) return currmana;

            bool useAOE = false;
            int mobscount = 0;
            foreach (Minion min in this.ownMinions)
            {
                if (min.maxHp >= 2 && min.Angr >= 2) mobscount++;
            }

            if (mobscount >= 3) useAOE = true;

            if (enemyHeroStrtClass == TAG_CLASS.WARRIOR)
            {
                bool usewhirlwind = true;
                foreach (Minion m in this.enemyMinions)
                {
                    if (m.Hp == 1) usewhirlwind = false;
                }
                if (this.ownMinions.Count <= 3) usewhirlwind = false;

                if (usewhirlwind)
                {
                    mana = EnemyPlaysACard(CardDB.cardName.whirlwind, mana, playAroundProb, pap2);
                }
            }

            if (!useAOE) return mana;

            if (enemyHeroStrtClass == TAG_CLASS.MAGE)
            {
                mana = EnemyPlaysACard(CardDB.cardName.flamestrike, mana, playAroundProb, pap2);
                mana = EnemyPlaysACard(CardDB.cardName.blizzard, mana, playAroundProb, pap2);
            }

            if (enemyHeroStrtClass == TAG_CLASS.HUNTER)
            {
                mana = EnemyPlaysACard(CardDB.cardName.unleashthehounds, mana, playAroundProb, pap2);
            }

            if (enemyHeroStrtClass == TAG_CLASS.PRIEST)
            {
                mana = EnemyPlaysACard(CardDB.cardName.holynova, mana, playAroundProb, pap2);
            }

            if (enemyHeroStrtClass == TAG_CLASS.SHAMAN)
            {
                mana = EnemyPlaysACard(CardDB.cardName.lightningstorm, mana, playAroundProb, pap2);
            }

            if (enemyHeroStrtClass == TAG_CLASS.PALADIN)
            {
                mana = EnemyPlaysACard(CardDB.cardName.consecration, mana, playAroundProb, pap2);
            }

            if (enemyHeroStrtClass == TAG_CLASS.DRUID)
            {
                mana = EnemyPlaysACard(CardDB.cardName.swipe, mana, playAroundProb, pap2);
            }



            return mana;
        }

        public int EnemyPlaysACard(CardDB.cardName cardname, int currmana, int playAroundProb, int pap2)
        {

            //todo manacosts
            if (cardname == CardDB.cardName.flamestrike && currmana >= 7)
            {
                bool dontkill = false;
                int prob = sf.Probabilitymaker.getProbOfEnemyHavingCardInHand(CardDB.cardIDEnum.CS2_032, this.enemyAnzCards, this.enemyDeckSize);
                if (playAroundProb > prob) return currmana;
                if (pap2 > prob) dontkill = true;

                List<Minion> temp = this.ownMinions;
                int damage = getEnemySpellDamageDamage(4);
                foreach (Minion enemy in temp)
                {
                    enemy.cantLowerHPbelowONE = dontkill;
                    this.minionGetDamageOrHeal(enemy, damage);
                    enemy.cantLowerHPbelowONE = false;
                }

                currmana -= 7;
                return currmana;
            }

            if (cardname == CardDB.cardName.blizzard && currmana >= 6)
            {
                bool dontkill = false;
                int prob = sf.Probabilitymaker.getProbOfEnemyHavingCardInHand(CardDB.cardIDEnum.CS2_028, this.enemyAnzCards, this.enemyDeckSize);
                if (playAroundProb > prob) return currmana;
                if (pap2 > prob) dontkill = true;

                List<Minion> temp = this.ownMinions;
                int damage = getEnemySpellDamageDamage(2);
                foreach (Minion enemy in temp)
                {
                    enemy.frozen = true;
                    enemy.cantLowerHPbelowONE = dontkill;
                    this.minionGetDamageOrHeal(enemy, damage);
                    enemy.cantLowerHPbelowONE = false;
                }

                currmana -= 6;
                return currmana;
            }


            if (cardname == CardDB.cardName.unleashthehounds && currmana >= 5)
            {
                bool dontkill = false;
                int prob = sf.Probabilitymaker.getProbOfEnemyHavingCardInHand(CardDB.cardIDEnum.EX1_538, this.enemyAnzCards, this.enemyDeckSize);
                if (playAroundProb > prob) return currmana;
                if (pap2 > prob) dontkill = true;

                int anz = this.ownMinions.Count;
                int posi = this.enemyMinions.Count - 1;
                CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_538t);//hound
                for (int i = 0; i < anz; i++)
                {
                    callKid(kid, posi, false);
                }
                currmana -= 5;
                return currmana;
            }





            if (cardname == CardDB.cardName.holynova && currmana >= 5)
            {
                bool dontkill = false;
                int prob = sf.Probabilitymaker.getProbOfEnemyHavingCardInHand(CardDB.cardIDEnum.CS1_112, this.enemyAnzCards, this.enemyDeckSize);
                if (playAroundProb > prob) return currmana;
                if (pap2 > prob) dontkill = true;

                List<Minion> temp = this.enemyMinions;
                int heal = getEnemySpellHeal(2);
                int damage = getEnemySpellDamageDamage(2);
                foreach (Minion enemy in temp)
                {
                    this.minionGetDamageOrHeal(enemy, -heal);
                }
                this.minionGetDamageOrHeal(this.enemyHero, -heal);
                temp = this.ownMinions;
                foreach (Minion enemy in temp)
                {
                    enemy.cantLowerHPbelowONE = dontkill;
                    this.minionGetDamageOrHeal(enemy, damage);
                    enemy.cantLowerHPbelowONE = false;
                }
                this.minionGetDamageOrHeal(this.ownHero, damage);
                currmana -= 5;
                return currmana;
            }




            if (cardname == CardDB.cardName.lightningstorm && currmana >= 4)//3
            {
                bool dontkill = false;
                int prob = sf.Probabilitymaker.getProbOfEnemyHavingCardInHand(CardDB.cardIDEnum.EX1_259, this.enemyAnzCards, this.enemyDeckSize);
                if (playAroundProb > prob) return currmana;
                if (pap2 > prob) dontkill = true;

                List<Minion> temp = this.ownMinions;
                int damage = getEnemySpellDamageDamage(3);
                foreach (Minion enemy in temp)
                {
                    enemy.cantLowerHPbelowONE = dontkill;
                    this.minionGetDamageOrHeal(enemy, damage);
                    enemy.cantLowerHPbelowONE = false;
                }
                currmana -= 3;
                return currmana;
            }



            if (cardname == CardDB.cardName.whirlwind && currmana >= 3)//1
            {
                bool dontkill = false;
                int prob = sf.Probabilitymaker.getProbOfEnemyHavingCardInHand(CardDB.cardIDEnum.EX1_400, this.enemyAnzCards, this.enemyDeckSize);
                if (playAroundProb > prob) return currmana;
                if (pap2 > prob) dontkill = true;

                List<Minion> temp = this.enemyMinions;
                int damage = getEnemySpellDamageDamage(1);
                foreach (Minion enemy in temp)
                {
                    this.minionGetDamageOrHeal(enemy, damage);
                }
                temp = this.ownMinions;
                foreach (Minion enemy in temp)
                {
                    enemy.cantLowerHPbelowONE = dontkill;
                    this.minionGetDamageOrHeal(enemy, damage);
                    enemy.cantLowerHPbelowONE = false;
                }
                currmana -= 1;
                return currmana;
            }



            if (cardname == CardDB.cardName.consecration && currmana >= 4)
            {
                bool dontkill = false;
                int prob = sf.Probabilitymaker.getProbOfEnemyHavingCardInHand(CardDB.cardIDEnum.CS2_093, this.enemyAnzCards, this.enemyDeckSize);
                if (playAroundProb > prob) return currmana;
                if (pap2 > prob) dontkill = true;

                List<Minion> temp = this.ownMinions;
                int damage = getEnemySpellDamageDamage(2);
                foreach (Minion enemy in temp)
                {
                    enemy.cantLowerHPbelowONE = dontkill;
                    this.minionGetDamageOrHeal(enemy, damage);
                    enemy.cantLowerHPbelowONE = false;
                }

                this.minionGetDamageOrHeal(this.ownHero, damage);
                currmana -= 4;
                return currmana;
            }



            if (cardname == CardDB.cardName.swipe && currmana >= 4)
            {
                bool dontkill = false;
                int prob = sf.Probabilitymaker.getProbOfEnemyHavingCardInHand(CardDB.cardIDEnum.CS2_012, this.enemyAnzCards, this.enemyDeckSize);
                if (playAroundProb > prob) return currmana;
                if (pap2 > prob) dontkill = true;

                int damage = getEnemySpellDamageDamage(4);
                // all others get 1 spelldamage
                int damage1 = getEnemySpellDamageDamage(1);

                List<Minion> temp = this.ownMinions;
                Minion target = null;
                foreach (Minion mnn in temp)
                {
                    if (mnn.Hp <= damage || mnn.handcard.card.isSpecialMinion || target == null)
                    {
                        target = mnn;
                    }
                }
                foreach (Minion mnn in temp.ToArray())
                {
                    if (mnn.entitiyID == target.entitiyID)
                    {
                        mnn.cantLowerHPbelowONE = dontkill;
                        this.minionGetDamageOrHeal(mnn, damage1);
                        mnn.cantLowerHPbelowONE = false;
                    }
                    else
                    {
                        mnn.cantLowerHPbelowONE = dontkill;
                        this.minionGetDamageOrHeal(mnn, damage);
                        mnn.cantLowerHPbelowONE = false;
                    }
                }
                currmana -= 4;
                return currmana;
            }





            return currmana;
        }

        public int getNextEntity()
        {
            //i dont trust return this.nextEntity++; !!!
            int retval = this.nextEntity;
            this.nextEntity++;         
            return retval;
        }


        // get all minions which are attackable
        public List<Minion> getAttackTargets(bool own, bool isLethalCheck)
        {
            List<Minion> trgts = new List<Minion>();
            List<Minion> trgts2 = new List<Minion>();

            List<Minion> temp = (own) ? this.enemyMinions : this.ownMinions;
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

            if (own && !this.enemyHero.immune) trgts2.Add(this.enemyHero);
            else if (!own && !this.ownHero.immune) trgts2.Add(this.ownHero);
            return trgts2;
        }

        public int getBestPlace(CardDB.Card card, bool lethal)
        {
            //we return the zonepos!
            if (card.type != CardDB.cardtype.MOB) return 1;
            if (this.ownMinions.Count == 0) return 1;
            if (this.ownMinions.Count == 1) return 2;

            int[] places = new int[this.ownMinions.Count];
            int i = 0;
            int tempval = 0;
            if (lethal && (card.name == CardDB.cardName.defenderofargus || card.name == CardDB.cardName.flametonguetotem || card.name == CardDB.cardName.direwolfalpha || card.name == CardDB.cardName.ancientmage))
            {
                i = 0;
                foreach (Minion m in this.ownMinions)
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
                foreach (Minion m in this.ownMinions)
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
                foreach (Minion m in this.ownMinions)
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

                    tempval += sf.PenalityManager.getValueOfUsefulNeedKeepPriority(m.handcard.card.name);
                    places[i] = tempval;
                    i++;
                }


                i = 0;
                int bestpl = 7;
                int bestval = 10000;
                foreach (Minion m in this.ownMinions)
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
            foreach (Minion m in this.ownMinions)
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
                int[] buffplaces = new int[this.ownMinions.Count];
                int[] whirlwindplaces = new int[this.ownMinions.Count];
                int gesval = 0;
                int minionsBefore = 0;
                foreach (Minion m in this.ownMinions)
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
                for (int j = 0; j <= this.ownMinions.Count; j++)
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

                        if (i < this.ownMinions.Count)
                        {
                            prev -= whirlwindplaces[i - 1] * buffplaces[i];
                        }
                    }
                    if (i < this.ownMinions.Count)
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
                if ((bplace != somebplace) && (minionsBefore > this.ownMinions.Count - somebplace)) bplace = somebplace;
                return bplace + 1;

            }

            // normal placement
            int cardvalue = card.Attack * 2 + card.Health;
            if (card.tank)
            {
                cardvalue += 5;
                cardvalue += card.Health;
            }

            cardvalue += sf.PenalityManager.getValueOfUsefulNeedKeepPriority(card.name); //  add a custom placement rules (replace it)
            cardvalue += 1;

            i = 0;
            foreach (Minion m in this.ownMinions)
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
                    tempval += sf.PenalityManager.getValueOfUsefulNeedKeepPriority(m.handcard.card.name);
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
            for (int j = 0; j <= this.ownMinions.Count; j++)
            {
                int prev = cardvalue;
                int next = cardvalue;
                if (i >= 1) prev = places[i - 1];
                if (i < this.ownMinions.Count) next = places[i];


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
            int ghd = 0;
            foreach (Minion m in this.enemyMinions)
            {
                if (m.frozen) continue;
                if (m.name == CardDB.cardName.ancientwatcher && !m.silenced)
                {
                    continue;
                }
                ghd += m.Angr;
                if (m.windfury) ghd += m.Angr;
            }

            if (!this.enemyHero.frozen)
            {
                if (this.enemyWeaponAttack >= 1)
                {
                    ghd += enemyWeaponAttack;
                    if (this.enemyHero.windfury || this.enemyWeaponName == CardDB.cardName.doomhammer) ghd += enemyWeaponAttack;
                }
                else
                {
                    if (this.enemyHeroName == HeroEnum.druid) ghd++;
                    if (this.enemyHeroName == HeroEnum.thief) ghd++;
                }
            }

            if (this.enemyHeroName == HeroEnum.mage) ghd++;
            if (this.enemyHeroName == HeroEnum.hunter) ghd += 2;
            if (this.enemyHeroName == HeroEnum.ragnarosthefirelord) ghd += 8;


            foreach (Minion m in this.ownMinions)
            {
                if (m.taunt) ghd -= m.Hp;
                if (m.taunt && m.divineshild) ghd -= 1;
            }

            int guessingHeroDamage = Math.Max(0, ghd);
            if (this.ownHero.immune) guessingHeroDamage = 0;
            this.guessingHeroHP = this.ownHero.Hp + this.ownHero.armor - guessingHeroDamage;
        }

        public void simulateTraps()
        {
            //todo rework this
            // DONT KILL ENEMY HERO (cause its only guessing)
            foreach (CardDB.cardIDEnum secretID in this.ownSecretsIDList)
            {
                //hunter secrets############
                if (secretID == CardDB.cardIDEnum.EX1_554) //snaketrap
                {

                    //call 3 snakes (if possible)
                    int posi = this.ownMinions.Count - 1;
                    CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_554t);//snake
                    callKid(kid, posi, true);
                    callKid(kid, posi, true);
                    callKid(kid, posi, true);
                }
                if (secretID == CardDB.cardIDEnum.EX1_609) //snipe
                {
                    //kill weakest minion of enemy
                    List<Minion> temp = new List<Minion>(this.enemyMinions);
                    temp.Sort((a, b) => a.Angr.CompareTo(b.Angr));//take the weakest
                    if (temp.Count == 0) continue;
                    Minion m = temp[0];
                    minionGetDamageOrHeal(m, 4);
                }
                if (secretID == CardDB.cardIDEnum.EX1_610) //explosive trap
                {
                    //take 2 damage to each enemy
                    List<Minion> temp = this.enemyMinions;
                    foreach (Minion m in temp)
                    {
                        minionGetDamageOrHeal(m, 2);
                    }
                    attackEnemyHeroWithoutKill(2);
                }
                if (secretID == CardDB.cardIDEnum.EX1_611) //freezing trap
                {
                    //return weakest enemy minion to hand
                    List<Minion> temp = new List<Minion>(this.enemyMinions);
                    temp.Sort((a, b) => a.Angr.CompareTo(b.Angr));//take the weakest
                    if (temp.Count == 0) continue;
                    Minion m = temp[0];
                    minionReturnToHand(m, false, 0);
                }
                if (secretID == CardDB.cardIDEnum.EX1_533) // missdirection
                {
                    // first damage to your hero is nulled -> lower guessingHeroDamage
                    List<Minion> temp = new List<Minion>(this.enemyMinions);
                    temp.Sort((a, b) => -a.Angr.CompareTo(b.Angr));//take the strongest
                    if (temp.Count == 0) continue;
                    Minion m = temp[0];
                    m.Angr = 0;
                    this.evaluatePenality -= this.enemyMinions.Count;// the more the enemy minions has on board, the more the posibility to destroy something other :D
                }

                //mage secrets############
                if (secretID == CardDB.cardIDEnum.EX1_287) //counterspell
                {
                    // what should we do?
                    this.evaluatePenality -= 8;
                }

                if (secretID == CardDB.cardIDEnum.EX1_289) //ice barrier
                {
                    this.ownHero.armor += 8;
                }

                if (secretID == CardDB.cardIDEnum.EX1_295) //ice block
                {
                    //set the guessed Damage to zero
                    this.ownHero.immune = true;
                }

                if (secretID == CardDB.cardIDEnum.EX1_294) //mirror entity
                {
                    //summon snake ( a weak minion)
                    int posi = this.ownMinions.Count - 1;
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
                    List<Minion> temp = new List<Minion>(this.enemyMinions);
                    temp.Sort((a, b) => a.Angr.CompareTo(b.Angr));//take the weakest
                    if (temp.Count == 0) continue;
                    Minion m = temp[0];
                    minionGetDestroyed(m);
                }
                if (secretID == CardDB.cardIDEnum.FP1_018) // duplicate
                {
                    // first damage to your hero is nulled -> lower guessingHeroDamage and destroy weakest minion
                    List<Minion> temp = new List<Minion>(this.ownMinions);
                    temp.Sort((a, b) => a.Angr.CompareTo(b.Angr));//take the weakest
                    if (temp.Count == 0) continue;
                    Minion m = temp[0];
                    drawACard(m.name, true);
                    drawACard(m.name, true);
                }

                //pala secrets############
                if (secretID == CardDB.cardIDEnum.EX1_132) // eye for an eye
                {
                    // enemy takes one damage
                    List<Minion> temp = new List<Minion>(this.enemyMinions);
                    temp.Sort((a, b) => a.Angr.CompareTo(b.Angr));//take the weakest
                    if (temp.Count == 0) continue;
                    Minion m = temp[0];
                    attackEnemyHeroWithoutKill(m.Angr);
                }
                if (secretID == CardDB.cardIDEnum.EX1_130) // noble sacrifice
                {
                    //spawn a 2/1 taunt!
                    int posi = this.ownMinions.Count - 1;
                    CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_130a);
                    callKid(kid, posi, true);

                }

                if (secretID == CardDB.cardIDEnum.EX1_136) // redemption
                {
                    // we give our weakest minion a divine shield :D
                    List<Minion> temp = new List<Minion>(this.ownMinions);
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
                    List<Minion> temp = new List<Minion>(this.enemyMinions);
                    temp.Sort((a, b) => a.Hp.CompareTo(b.Hp));//take the weakest
                    if (temp.Count == 0) continue;
                    Minion m = temp[0];
                    m.Hp = 1;
                    m.maxHp = 1;
                }

                if (secretID == CardDB.cardIDEnum.FP1_020) // avenge
                {
                    // we give our weakest minion +3/+2 :D
                    List<Minion> temp = new List<Minion>(this.ownMinions);
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
            this.value = int.MinValue;
            if (this.turnCounter == 0) this.manaTurnEnd = this.mana;
            this.turnCounter++;
            //penalty for destroying combo

            int comboBreakPenalty = sf.Combobreaker.checkIfComboWasPlayed(this.playactions, this.ownWeaponName, this.ownHeroName);
            this.evaluatePenality += comboBreakPenalty;

            if (this.complete) return;
            this.triggerEndTurn(this.isOwnTurn);
            this.isOwnTurn = !this.isOwnTurn;
            this.triggerStartTurn(this.isOwnTurn);
            this.optionsPlayedThisTurn = 0;
            if (!this.isOwnTurn) simulateTraps();

            if (!sEnemTurn)
            {
                guessHeroDamage();
                this.triggerEndTurn(false);
                this.triggerStartTurn(true);
                this.complete = true;
            }
            else
            {
                guessHeroDamage();
                /*
                if (this.guessingHeroHP >= 1)
                {
                    //simulateEnemysTurn(simulateTwoTurns, playaround, print, pprob, pprob2);
                    this.prepareNextTurn(this.isOwnTurn);

                    if (this.turnCounter >= 2)
                        sf.Ai.enemySecondTurnSim.simulateEnemysTurn(this, simulateTwoTurns, playaround, print, pprob, pprob2);
                    else
                        sf.Ai.enemyTurnSim.simulateEnemysTurn(this, simulateTwoTurns, playaround, print, pprob, pprob2);
                }
                this.complete = true;*/
            }

        }

        //prepares the turn for the next player
        public void prepareNextTurn(bool own)
        {
            //call this after start turn trigger!

            if (own)
            {
                this.ownMaxMana = Math.Min(10, this.ownMaxMana + 1);
                this.mana = this.ownMaxMana - this.ueberladung;
                foreach (Minion m in ownMinions)
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
                foreach (Minion m in enemyMinions)
                {
                    m.frozen = false;
                }
                this.enemyHero.frozen = false;


                this.ownHero.Angr = this.ownWeaponAttack;
                this.ownHero.numAttacksThisTurn = 0;
                this.ownAbilityReady = true;
                this.ownHero.updateReadyness();
                this.cardsPlayedThisTurn = 0;
                this.mobsplayedThisTurn = 0;
                this.playedPreparation = false;
                this.playedmagierinderkirintor = false;
                this.optionsPlayedThisTurn = 0;
                this.owncarddraw = 0;
                this.sEnemTurn = false;
            }
            else
            {

                this.enemyMaxMana = Math.Min(10, this.enemyMaxMana + 1);
                this.mana = this.enemyMaxMana; ;//todo enemy overload -this.ueberladung;

                foreach (Minion m in enemyMinions)
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
                //unfreeze the own minions
                foreach (Minion m in ownMinions)
                {
                    m.frozen = false;
                }
                this.ownHero.frozen = false;

                this.enemyHero.Angr = this.enemyWeaponAttack;
                this.enemyHero.numAttacksThisTurn = 0;
                this.enemyAbilityReady = true;
                this.enemyHero.updateReadyness();
                this.enemyOptionsDoneThisTurn = 0;

                this.cardsPlayedThisTurn = 0;
                this.mobsplayedThisTurn = 0;
                this.playedPreparation = false;
                this.playedmagierinderkirintor = false;

                this.sEnemTurn = false;
            }


            this.ueberladung = 0;
            this.complete = false;

            this.value = int.MinValue;
            if (this.diedMinions != null) this.diedMinions.Clear();//contains only the minions that died in this turn!
        }

        public void endEnemyTurn()
        {
            this.triggerEndTurn(false);
            this.turnCounter++;
            this.isOwnTurn = true;
            this.triggerStartTurn(true);
            this.complete = true;
            //sf.Ai.botBase.getPlayfieldValue(this);

        }

        //spelldamage calculation---------------------------------------------------
        public int getSpellDamageDamage(int dmg)
        {
            int retval = dmg;
            retval += this.spellpower;
            if (this.doublepriest >= 1) retval *= (2 * this.doublepriest);
            return retval;
        }

        public int getSpellHeal(int heal)
        {
            int retval = heal;
            if (this.anzOwnAuchenaiSoulpriest >= 1)
            {
                retval *= -1;
                retval -= this.spellpower;
            }
            if (this.doublepriest >= 1) retval *= (2 * this.doublepriest);
            return retval;
        }


        public int getMinionHeal(int heal)
        {
            return (this.anzOwnAuchenaiSoulpriest >= 1) ? -heal : heal;
        }

        public int getEnemySpellDamageDamage(int dmg)
        {
            int retval = dmg;
            retval += this.enemyspellpower;
            if (this.enemydoublepriest >= 1) retval *= (2 * this.enemydoublepriest);
            return retval;
        }

        public int getEnemySpellHeal(int heal)
        {
            int retval = heal;
            if (this.anzOwnAuchenaiSoulpriest >= 1)
            {
                retval *= -1;
                retval -= this.enemyspellpower;
            }
            if (this.doublepriest >= 1) retval *= (2 * this.doublepriest);
            return retval;
        }
        
        public int getEnemyMinionHeal(int heal)
        {
            return (this.anzEnemyAuchenaiSoulpriest >= 1) ? -heal : heal;
        }


        // do the action--------------------------------------------------------------

        public void doAction(Action aa)
        {
            //CREATE NEW MINIONS (cant use a.target or a.own) (dont belong to this board)
            Minion trgt = null;
            Minion o = null;
            Handmanager.Handcard ha = null;
            if (aa.target != null)
            {
                foreach (Minion m in this.ownMinions)
                {
                    if (aa.target.entitiyID == m.entitiyID)
                    {
                        trgt = m;
                        break;
                    }
                }
                foreach (Minion m in this.enemyMinions)
                {
                    if (aa.target.entitiyID == m.entitiyID)
                    {
                        trgt = m;
                        break;
                    }
                }
                if (aa.target.entitiyID == this.ownHero.entitiyID) trgt = this.ownHero;
                if (aa.target.entitiyID == this.enemyHero.entitiyID) trgt = this.enemyHero;
            }
            if (aa.own != null)
            {
                foreach (Minion m in this.ownMinions)
                {
                    if (aa.own.entitiyID == m.entitiyID)
                    {
                        o = m;
                        break;
                    }
                }
                foreach (Minion m in this.enemyMinions)
                {
                    if (aa.own.entitiyID == m.entitiyID)
                    {
                        o = m;
                        break;
                    }
                }
                if (aa.own.entitiyID == this.ownHero.entitiyID) o = this.ownHero;
                if (aa.own.entitiyID == this.enemyHero.entitiyID) o = this.enemyHero;
            }

            if (aa.card != null)
            {
                foreach (Handmanager.Handcard hh in this.owncards)
                {
                    if (hh.entity == aa.card.entity)
                    {
                        ha = hh;
                        break;
                    }
                }
                if (aa.actionType == actionEnum.useHeroPower)
                {
                    ha = this.isOwnTurn ? this.ownHeroAblility : this.enemyHeroAblility;
                }
            }
            // create and execute the action------------------------------------------------------------------------
            Action a = new Action(aa.actionType, ha, o, aa.place, trgt, aa.penalty, aa.druidchoice);



            //save the action if its our first turn
            if (this.turnCounter == 0) this.playactions.Add(a);
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
                    foreach (Minion m in this.ownMinions)
                    {
                        if (m.entitiyID == newTarget)
                        {
                            target = m;
                            break;
                        }
                    }
                    foreach (Minion m in this.enemyMinions)
                    {
                        if (m.entitiyID == newTarget)
                        {
                            target = m;
                            break;
                        }
                    }
                    if (this.ownHero.entitiyID == newTarget) target = this.ownHero;
                    if (this.enemyHero.entitiyID == newTarget) target = this.enemyHero;
                    //sf.Helpfunctions.ErrorLog("missdirection target = " + target.entitiyID);
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
                            //enemyplaysACard();
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







            if (this.isOwnTurn)
            {
                this.optionsPlayedThisTurn++;
            }
            else
            {
                this.enemyOptionsDoneThisTurn++;
            }

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

                    if (!this.ownHero.immuneWhileAttacking)
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
            

            if (logging) sf.Helpfunctions.logg(".attck with" + attacker.name + " A " + attacker.Angr + " H " + attacker.Hp);

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
                   if(defender.handcard.card.name == CardDB.cardName.waterelemental || defender.handcard.card.name == CardDB.cardName.snowchugger) attacker.frozen = true;

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
                    List<Minion> temp = (attacker.own) ? this.enemyMinions : this.ownMinions;
                    foreach (Minion mnn in temp)
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
            this.attacked = true;
            this.evaluatePenality += penality;
            hero.numAttacksThisTurn++;

            //hero will end his readyness
            hero.updateReadyness();

            //heal whether truesilverchampion equipped
            if (own)
            {
                if (this.ownWeaponName == CardDB.cardName.truesilverchampion)
                {
                    int heal = this.getMinionHeal(2);//minionheal because it's ignoring spellpower
                    this.minionGetDamageOrHeal(hero, -heal);
                    doDmgTriggers();
                }
            }
            else
            {
                if (this.enemyWeaponName == CardDB.cardName.truesilverchampion)
                {
                    int heal = this.getEnemyMinionHeal(2);
                    this.minionGetDamageOrHeal(hero, -heal);
                    doDmgTriggers();
                }
            }

            if (logging) sf.Helpfunctions.logg("attck with weapon trgt: " + target.entitiyID);

            // hero attacks enemy----------------------------------------------------------------------------------

            if (target.isHero)// trigger secret and change target if necessary
            {
                int newTarget = this.secretTrigger_CharIsAttacked(hero, target);
                if (newTarget >= 1)
                {
                    //search new target!
                    foreach (Minion m in this.ownMinions)
                    {
                        if (m.entitiyID == newTarget)
                        {
                            target = m;
                            break;
                        }
                    }
                    foreach (Minion m in this.enemyMinions)
                    {
                        if (m.entitiyID == newTarget)
                        {
                            target = m;
                            break;
                        }
                    }
                    if (this.ownHero.entitiyID == newTarget) target = this.ownHero;
                    if (this.enemyHero.entitiyID == newTarget) target = this.enemyHero;
                }

            }
            this.minionAttacksMinion(hero, target);
            //-----------------------------------------------------------------------------------------------------

            //gorehowl is not killed if he attacks minions
            if (own)
            {
                if (ownWeaponName == CardDB.cardName.gorehowl && !target.isHero)
                {
                    this.ownWeaponAttack--;
                    hero.Angr--;
                }
                else
                {
                    this.lowerWeaponDurability(1, true);
                }
            }
            else
            {
                if (enemyWeaponName == CardDB.cardName.gorehowl && !target.isHero)
                {
                    this.enemyWeaponAttack--;
                    hero.Angr--;
                }
                else
                {
                    this.lowerWeaponDurability(1, false);
                }
            }

        }

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
            this.mana = this.mana - hc.getManaCost(this);
            removeCard(hc);// remove card from hand

            this.triggerCardsChanged(true);

            if (c.type == CardDB.cardtype.SPELL)
            {
                this.playedPreparation = false;
                if (target != null && target.name == CardDB.cardName.dragonkinsorcerer && target.own) this.minionGetBuffed(target, 1, 1);
            }

            if (c.Secret)
            {
                this.ownSecretsIDList.Add(c.cardIDenum);
                this.playedmagierinderkirintor = false;
            }


            //sf.Helpfunctions.logg("play crd " + c.name + " entitiy# " + cardEntity + " mana " + hc.getManaCost(this) + " trgt " + target);
            if (logging) sf.Helpfunctions.logg("play crd " + c.name + " entitiy# " + hc.entity + " mana " + hc.getManaCost(this) + " trgt " + target);


            this.triggerACardWillBePlayed(hc, true);
            int newTarget = secretTrigger_SpellIsPlayed(target, c.type == CardDB.cardtype.SPELL);
            if (newTarget >= 1)
            {
                //search new target!
                foreach (Minion m in this.ownMinions)
                {
                    if (m.entitiyID == newTarget)
                    {
                        target = m;
                        break;
                    }
                }
                foreach (Minion m in this.enemyMinions)
                {
                    if (m.entitiyID == newTarget)
                    {
                        target = m;
                        break;
                    }
                }
                if (this.ownHero.entitiyID == newTarget) target = this.ownHero;
                if (this.enemyHero.entitiyID == newTarget) target = this.enemyHero;
            }
            if (newTarget != -2) // trigger spell-secrets!
            {

                if (c.type == CardDB.cardtype.MOB)
                {
                    this.placeAmobSomewhere(hc, target, choice, position);
                    this.mobsplayedThisTurn++;

                }
                else
                {

                    c.sim_card.onCardPlay(this, true, target, choice);
                    this.doDmgTriggers();
                    //secret trigger? do here


                }
            }

            //this.ueberladung += c.recallValue;
            this.cardsPlayedThisTurn++;

        }

        public void enemyplaysACard(CardDB.Card c, Minion target, int position, int choice, int penality)
        {

            Handmanager.Handcard hc = new Handmanager.Handcard(c);
            hc.entity = this.getNextEntity();
            //sf.Helpfunctions.logg("play crd " + c.name + " entitiy# " + cardEntity + " mana " + hc.getManaCost(this) + " trgt " + target);
            if (logging) sf.Helpfunctions.logg("enemy play crd " + c.name + " trgt " + target);

            this.enemyAnzCards--;//might be deleted if he got a real hand

            this.triggerACardWillBePlayed(hc, false);
            this.triggerCardsChanged(false);

            int newTarget = secretTrigger_SpellIsPlayed(target, c.type == CardDB.cardtype.SPELL);
            if (newTarget >= 1)
            {
                //search new target!
                foreach (Minion m in this.ownMinions)
                {
                    if (m.entitiyID == newTarget)
                    {
                        target = m;
                        break;
                    }
                }
                foreach (Minion m in this.enemyMinions)
                {
                    if (m.entitiyID == newTarget)
                    {
                        target = m;
                        break;
                    }
                }
                if (this.ownHero.entitiyID == newTarget) target = this.ownHero;
                if (this.enemyHero.entitiyID == newTarget) target = this.enemyHero;
            }
            if (newTarget != -2) // trigger spell-secrets!
            {
                if (c.type == CardDB.cardtype.MOB)
                {
                    //todo mob playing
                    //this.placeAmobSomewhere(hc, target, choice, position);

                }
                else
                {
                    c.sim_card.onCardPlay(this, false, target, choice);
                    this.doDmgTriggers();
                    //secret trigger? do here


                }
            }
        }


        public void playHeroPower(Minion target, int penality, bool ownturn)
        {

            CardDB.Card c = (ownturn) ? this.ownHeroAblility.card : this.enemyHeroAblility.card;

            if (ownturn) this.ownAbilityReady = false;
            else this.enemyAbilityReady = false;

            this.evaluatePenality += penality;
            this.mana = this.mana - 2;

            //sf.Helpfunctions.logg("play crd " + c.name + " entitiy# " + cardEntity + " mana " + hc.getManaCost(this) + " trgt " + target);
            if (logging) sf.Helpfunctions.logg("play crd " + c.name + " trgt " + target);

            c.sim_card.onCardPlay(this, ownturn, target, 0);
            this.doDmgTriggers();
        }


        //lower durability of weapon + destroy them (deathrattle) 
        //todo: test death's bite's dearthrattle
        public void lowerWeaponDurability(int value, bool own)
        {

            if (own)
            {
                if (this.ownWeaponDurability <= 0) return;
                this.ownWeaponDurability -= value;
                if (this.ownWeaponDurability <= 0)
                {
                    //todo deathrattle deathsbite

                    if (this.ownWeaponName == CardDB.cardName.powermace && this.ownMinions.Count>=1)
                    {
                        int sum = 1000;
                        Minion t = null;

                        foreach (Minion m in ownMinions)
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

                    if (this.ownWeaponName == CardDB.cardName.deathsbite)
                    {
                        int anz = 1;
                        if (this.ownBaronRivendare >= 1) anz = 2;
                        int dmg = getSpellDamageDamage(1);
                        foreach (Minion m in this.ownMinions)
                        {
                            this.minionGetDamageOrHeal(m, anz * dmg);
                        }
                        foreach (Minion m in this.enemyMinions)
                        {
                            this.minionGetDamageOrHeal(m, anz * dmg);
                        }
                        this.doDmgTriggers();

                    }


                    this.ownHero.Angr -= this.ownWeaponAttack;
                    this.ownWeaponDurability = 0;
                    this.ownWeaponAttack = 0;
                    this.ownWeaponName = CardDB.cardName.unknown;
                    this.ownHero.windfury = false;

                    foreach (Minion m in this.ownMinions)
                    {
                        if (m.playedThisTurn && m.name == CardDB.cardName.southseadeckhand)
                        {
                            m.charge--;
                            m.updateReadyness();
                        }
                    }
                    this.ownHero.updateReadyness();
                }
            }
            else
            {
                if (this.enemyWeaponDurability <= 0) return;
                this.enemyWeaponDurability -= value;
                if (this.enemyWeaponDurability <= 0)
                {
                    //deathrattle deathsbite

                    if (this.ownWeaponName == CardDB.cardName.powermace && this.enemyMinions.Count >= 1)
                    {
                        int sum = 1000;
                        Minion t = null;

                        foreach (Minion m in enemyMinions)
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

                    if (this.enemyWeaponName == CardDB.cardName.deathsbite)
                    {
                        int anz = 1;
                        if (this.enemyBaronRivendare >= 1) anz = 2;
                        int dmg = getEnemySpellDamageDamage(1);
                        foreach (Minion m in this.ownMinions)
                        {
                            this.minionGetDamageOrHeal(m, anz * dmg);
                        }
                        foreach (Minion m in this.enemyMinions)
                        {
                            this.minionGetDamageOrHeal(m, anz * dmg);
                        }
                        this.doDmgTriggers();
                    }

                    this.enemyHero.Angr -= this.enemyWeaponAttack;
                    this.enemyWeaponDurability = 0;
                    this.enemyWeaponAttack = 0;
                    this.enemyWeaponName = CardDB.cardName.unknown;
                    this.enemyHero.windfury = false;
                    this.enemyHero.updateReadyness();
                }
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
            foreach (Minion mnn in this.ownMinions)
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
                        Minion t = this.searchRandomMinion(this.enemyMinions, searchmode.searchHighestHP);
                        if(t!=null)
                        {
                            this.minionGetDamageOrHeal(t, 1);
                        }
                        else
                        {
                            this.minionGetDamageOrHeal(this.enemyHero, 1);
                        }
                    }
                }
            }
            foreach (Minion mnn in this.enemyMinions)
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
                        Minion t = this.searchRandomMinion(this.ownMinions, searchmode.searchHighestHP);
                        if (t != null)
                        {
                            this.minionGetDamageOrHeal(t, 1);
                        }
                        else
                        {
                            this.minionGetDamageOrHeal(this.ownHero, 1);
                        }
                    }
                }
            }
        }

        public void triggerAMinionGotHealed()
        {
            //also dead minions trigger this
            foreach (Minion mnn in this.ownMinions)
            {
                if (mnn.silenced) continue;
                if (mnn.handcard.card.name == CardDB.cardName.northshirecleric)
                {
                    for (int i = 0; i < this.tempTrigger.minionsGotHealed; i++)
                    {
                        this.drawACard(CardDB.cardName.unknown, true);
                    }
                }
            }

            foreach (Minion mnn in this.enemyMinions)
            {
                if (mnn.silenced) continue;
                if (mnn.handcard.card.name == CardDB.cardName.northshirecleric)
                {
                    for (int i = 0; i < this.tempTrigger.minionsGotHealed; i++)
                    {
                        this.drawACard(CardDB.cardName.unknown, false);
                    }
                }
            }
        }

        public void triggerAMinionGotDmg()
        {
            foreach (Minion m in this.ownMinions.ToArray())
            {
                if (m.silenced) { m.anzGotDmg = 0; continue; }
                m.handcard.card.sim_card.onMinionGotDmgTrigger(this, m, true);
            }

            foreach (Minion m in this.enemyMinions.ToArray())
            {
                if (m.silenced) { m.anzGotDmg = 0; continue; }
                m.handcard.card.sim_card.onMinionGotDmgTrigger(this, m, false);
            }
            this.ownHero.anzGotDmg = 0;
            this.enemyHero.anzGotDmg = 0;
        }

        public void triggerAMinionDied()
        {
            this.ownMinionsDiedTurn += this.tempTrigger.ownMinionsDied;
            this.enemyMinionsDiedTurn += this.tempTrigger.enemyMinionsDied;

            foreach (Minion m in this.ownMinions.ToArray())
            {
                if (m.silenced) continue;
                if (m.Hp <= 0) continue;
                m.handcard.card.sim_card.onMinionDiedTrigger(this, m, m); //onMinionDiedTrigger(Playfield p, Minion triggerEffectMinion, Minion diedMinion) where "diedMinion" - not implemented and is not currently needed
            }
            foreach (Minion m in this.enemyMinions.ToArray())
            {
                if (m.silenced) continue;
                if (m.Hp <= 0) continue;
                m.handcard.card.sim_card.onMinionDiedTrigger(this, m, m);
            }

            foreach (Handmanager.Handcard hc in this.owncards)
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
                    if (this.anzOwnAuchenaiSoulpriest >= 1) // you have a soulpriest? lol you die!!!
                    {
                        this.ownHero.Hp = 0;
                    }
                    else
                    {
                        this.minionGetDamageOrHeal(this.ownHero, -dmgDone);
                    }
                }
                else
                {
                    if (this.anzEnemyAuchenaiSoulpriest >= 1) // you have a soulpriest? lol you die!!!
                    {
                        this.enemyHero.Hp = 0;
                    }
                    else
                    {
                        this.minionGetDamageOrHeal(this.enemyHero, -dmgDone);
                    }
                }
            }
 
        }

        public void triggerACardWillBePlayed(Handmanager.Handcard hc, bool own)
        {
            if (own)
            {
                if (anzOwnDragonConsort > 0 && (TAG_RACE)hc.card.race == TAG_RACE.DRAGON) anzOwnDragonConsort = 0;
                int violetteacher = 0; //we count violetteacher to avoid copying ownminions
                int illidan = 0;
                int burly = 0;
                foreach (Minion m in this.ownMinions)
                {
                    if (m.silenced) continue;

                    if (own && m.name == CardDB.cardName.illidanstormrage)
                    {
                        illidan++;
                        continue;
                    }

                    if (own && m.name == CardDB.cardName.violetteacher)
                    {
                        if (hc.card.type == CardDB.cardtype.SPELL)
                        {
                            violetteacher++;
                        }
                        continue;
                    }
                    if (own && m.name == CardDB.cardName.hobgoblin)
                    {
                        if (hc.card.type == CardDB.cardtype.MOB && hc.card.Attack == 1 )
                        {
                            hc.addattack += 2;
                            hc.addHp += 2;
                        }
                        continue;
                    }

                    m.handcard.card.sim_card.onCardIsGoingToBePlayed(this, hc.card, own, m);
                }

                foreach (Minion m in this.enemyMinions)
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
                    int pos = this.ownMinions.Count;
                    this.callKid(CardDB.Instance.teacherminion, pos, own);
                }

                for (int i = 0; i < illidan; i++)
                {
                    int pos = this.ownMinions.Count;
                    this.callKid(CardDB.Instance.illidanminion, pos, own);
                }

                for (int i = 0; i < burly; i++)//summon for enemy !
                {
                    int pos = this.enemyMinions.Count;
                    this.callKid(CardDB.Instance.burlyrockjaw, pos, !own);
                }
                
                
            }
            else
            {
                int violetteacher = 0; //we count violetteacher to avoid copying ownminions
                int illidan = 0;
                int burly = 0;
                foreach (Minion m in this.enemyMinions)
                {
                    if (m.silenced) continue;
                    if (!own && m.name == CardDB.cardName.illidanstormrage)
                    {
                        illidan++;
                        continue;
                    }
                    if (!own && m.name == CardDB.cardName.violetteacher)
                    {
                        if (hc.card.type == CardDB.cardtype.SPELL)
                        {
                            violetteacher++;
                        }
                        continue;
                    }
                    if (!own && m.name == CardDB.cardName.hobgoblin)
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
                foreach (Minion m in this.ownMinions)
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
                    int pos = this.enemyMinions.Count;
                    this.callKid(CardDB.Instance.teacherminion, pos, own);
                }
                for (int i = 0; i < illidan; i++)
                {
                    int pos = this.enemyMinions.Count;
                    this.callKid(CardDB.Instance.illidanminion, pos, own);
                }

                for (int i = 0; i < burly; i++)//summon for us
                {
                    int pos = this.ownMinions.Count;
                    this.callKid(CardDB.Instance.burlyrockjaw, pos, own);
                }
            }

        }

       // public void triggerACardWasPlayed(CardDB.Card c, bool own) {        }

        public void triggerAMinionIsSummoned(Minion m)
        {
            if (m.own)
            {
                foreach (Minion mnn in this.ownMinions)
                {
                    if (mnn.silenced) continue;
                    mnn.handcard.card.sim_card.onMinionIsSummoned(this, mnn, m);
                }

                foreach (Minion mnn in this.enemyMinions)
                {
                    if (mnn.silenced) continue;
                    if (mnn.name == CardDB.cardName.murloctidecaller) mnn.handcard.card.sim_card.onMinionIsSummoned(this, mnn, m);
                }

                if (this.ownWeaponName == CardDB.cardName.swordofjustice)
                {
                    this.minionGetBuffed(m, 1, 1);
                    this.lowerWeaponDurability(1, true);
                }
            }
            else
            {
                foreach (Minion mnn in this.enemyMinions)
                {
                    if (mnn.silenced) continue;
                    mnn.handcard.card.sim_card.onMinionIsSummoned(this, mnn, m);
                }

                foreach (Minion mnn in this.ownMinions)
                {
                    if (mnn.silenced) continue;
                    if (mnn.name == CardDB.cardName.murloctidecaller) mnn.handcard.card.sim_card.onMinionIsSummoned(this, mnn, m);
                }
                if (this.enemyWeaponName == CardDB.cardName.swordofjustice)
                {
                    this.minionGetBuffed(m, 1, 1);
                    this.lowerWeaponDurability(1, false);
                }
            }

        }

        public void triggerAMinionWasSummoned(Minion mnn)
        {
            if (mnn.own)
            {
                foreach (Minion m in this.ownMinions)
                {
                    if (m.silenced) continue;
                    if (m.name == CardDB.cardName.knifejuggler)
                    {
                        m.handcard.card.sim_card.onMinionWasSummoned(this, m, mnn);
                    }
                }
            }
            else
            {
                foreach (Minion m in this.enemyMinions)
                {
                    if (m.silenced) continue;
                    if (m.name == CardDB.cardName.knifejuggler)
                    {
                        m.handcard.card.sim_card.onMinionWasSummoned(this, m, mnn);
                    }
                }
            }

        }

        public void triggerEndTurn(bool ownturn)
        {
            foreach (Minion m in this.ownMinions.ToArray())
            {
                if (!m.silenced)
                {
                    m.handcard.card.sim_card.onTurnEndsTrigger(this, m, ownturn);
                    this.doDmgTriggers();  //implementation
                }
                if (ownturn && m.destroyOnOwnTurnEnd) this.minionGetDestroyed(m);
            }
            foreach (Minion m in this.enemyMinions.ToArray())
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
                List<Minion> ownm = (ownturn) ? this.ownMinions : this.enemyMinions;
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
            
            this.playedmagierinderkirintor = false;

            foreach (Minion m in this.ownMinions)
            {
                this.minionGetTempBuff(m, -m.tempAttack, 0);
                m.immune = false;
                m.cantLowerHPbelowONE = false;
            }
            foreach (Minion m in this.enemyMinions)
            {
                this.minionGetTempBuff(m, -m.tempAttack, 0);
                m.immune = false;
                m.cantLowerHPbelowONE = false;
            }
        }

        public void triggerStartTurn(bool ownturn)
        {

            List<Minion> ownm = (ownturn) ? this.ownMinions : this.enemyMinions;
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

            List<Minion> enemm = (ownturn) ? this.enemyMinions : this.ownMinions;
            foreach (Minion m in enemm.ToArray())
            {
                if (!m.silenced)
                {
                    if(m.name == CardDB.cardName.micromachine) m.handcard.card.sim_card.onTurnStartTrigger(this, m, ownturn);
                }
                if (ownturn && m.destroyOnOwnTurnStart) this.minionGetDestroyed(m);
                if (!ownturn && m.destroyOnEnemyTurnStart) this.minionGetDestroyed(m);
            }
            
            this.doDmgTriggers();

            this.drawACard(CardDB.cardName.unknown, ownturn);
            this.doDmgTriggers();
        }

        public void triggerAHeroGotArmor(bool ownHero)
        {
            foreach (Minion m in ((ownHero) ? this.ownMinions : this.enemyMinions))
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
                if (this.tempanzOwnCards >= 6 && this.owncards.Count <= 5)
                {
                    //delete effect of enemy Goblin Sapper
                    foreach (Minion m in this.enemyMinions)
                    {
                        if (m.name == CardDB.cardName.goblinsapper && !m.silenced)
                        {
                            this.minionGetBuffed(m, -4, 0);
                        }
                    }
                }
                if (this.owncards.Count >= 6 && this.tempanzOwnCards <= 5)
                {
                    //add effect of enemy Goblin Sapper
                    foreach (Minion m in this.enemyMinions)
                    {
                        if (m.name == CardDB.cardName.goblinsapper && !m.silenced)
                        {
                            this.minionGetBuffed(m, 4, 0);
                        }
                    }
                }

                this.tempanzOwnCards = this.owncards.Count;
            }
            else
            {
                if (this.tempanzEnemyCards >= 6 && this.enemyAnzCards <= 5)
                {
                    //delete effect of own Goblin Sapper
                    foreach (Minion m in this.ownMinions)
                    {
                        if (m.name == CardDB.cardName.goblinsapper && !m.silenced)
                        {
                            this.minionGetBuffed(m, -4, 0);
                        }
                    }
                }
                if (this.enemyAnzCards >= 6 && this.tempanzEnemyCards <= 5)
                {
                    //add effect of own Goblin Sapper
                    foreach (Minion m in this.ownMinions)
                    {
                        if (m.name == CardDB.cardName.goblinsapper && !m.silenced)
                        {
                            this.minionGetBuffed(m, 4, 0);
                        }
                    }
                }

                this.tempanzEnemyCards = this.enemyAnzCards;
            }
        }



        public int secretTrigger_CharIsAttacked(Minion attacker, Minion defender)
        {
            int newTarget = 0;
            int triggered = 0;
            if (this.isOwnTurn && this.enemySecretCount >= 1)
            {

                if (defender.isHero && !defender.own)
                {
                    foreach (SecretItem si in this.enemySecretList.ToArray())
                    {
                        if (si.canBe_explosive)
                        {
                            triggered++;
                            CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_610).sim_card.onSecretPlay(this, false, 0);
                            doDmgTriggers();
                            //sf.Helpfunctions.ErrorLog("trigger explosive" + attacker.Hp);
                            si.usedTrigger_CharIsAttacked(true, attacker.isHero);
                            foreach (SecretItem sii in this.enemySecretList)
                            {
                                sii.canBe_explosive = false;
                            }
                        }

                        if (!attacker.isHero && si.canBe_vaporize)
                        {
                            triggered++;
                            CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_594).sim_card.onSecretPlay(this, false, attacker, 0);
                            doDmgTriggers();

                            si.usedTrigger_CharIsAttacked(true, attacker.isHero);
                            foreach (SecretItem sii in this.enemySecretList)
                            {
                                sii.canBe_vaporize = false;
                            }
                        }

                        if (si.canBe_missdirection)
                        {
                            if (!(attacker.isHero && this.ownMinions.Count + this.enemyMinions.Count == 0))
                            {
                                triggered++;
                                CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_533).sim_card.onSecretPlay(this, false, attacker, defender, out newTarget);
                                si.usedTrigger_CharIsAttacked(true, attacker.isHero);
                                //sf.Helpfunctions.ErrorLog("trigger miss " + attacker.Hp);
                                foreach (SecretItem sii in this.enemySecretList)
                                {
                                    sii.canBe_missdirection = false;
                                }
                            }
                        }

                        if (si.canBe_icebarrier)
                        {
                            triggered++;
                            CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_289).sim_card.onSecretPlay(this, false, defender, 0);
                            si.usedTrigger_CharIsAttacked(true, attacker.isHero);
                            foreach (SecretItem sii in this.enemySecretList)
                            {
                                sii.canBe_icebarrier = false;
                            }
                        }

                    }

                }

                if (!defender.isHero && !defender.own)
                {
                    foreach (SecretItem si in this.enemySecretList)
                    {

                        if (si.canBe_snaketrap)
                        {
                            triggered++;
                            CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_554).sim_card.onSecretPlay(this, false, 0);
                            si.usedTrigger_CharIsAttacked(false, attacker.isHero);
                            foreach (SecretItem sii in this.enemySecretList)
                            {
                                sii.canBe_snaketrap = false;
                            }
                        }
                    }
                }

                if (!attacker.isHero && attacker.own) // minion attacks
                {
                    foreach (SecretItem si in this.enemySecretList)
                    {
                        if (si.canBe_freezing)
                        {
                            triggered++;
                            CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_611).sim_card.onSecretPlay(this, false, attacker, 0);
                            si.usedTrigger_CharIsAttacked(defender.isHero, attacker.isHero);
                            //sf.Helpfunctions.ErrorLog("trigger freeze " + attacker.Hp);
                            foreach (SecretItem sii in this.enemySecretList)
                            {
                                sii.canBe_freezing = false;
                            }
                        }
                    }
                }

                foreach (SecretItem si in this.enemySecretList)
                {

                    if (si.canBe_noblesacrifice)
                    {
                        //triggered++;
                        bool ishero = defender.isHero;
                        //CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_130).sim_card.onSecretPlay(this, false, attacker, defender, out newTarget);
                        si.usedTrigger_CharIsAttacked(ishero, attacker.isHero);
                        foreach (SecretItem sii in this.enemySecretList)
                        {
                            sii.canBe_noblesacrifice = false;
                        }
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
                if (this.isOwnTurn && this.enemySecretCount >= 1)
                {
                    foreach (SecretItem si in this.enemySecretList)
                    {
                        if (si.canBe_eyeforaneye)
                        {
                            triggered++;
                            CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_132).sim_card.onSecretPlay(this, false, dmg);
                            si.usedTrigger_HeroGotDmg();
                            foreach (SecretItem sii in this.enemySecretList)
                            {
                                sii.canBe_eyeforaneye = false;
                            }
                        }

                        if (si.canBe_iceblock && this.enemyHero.Hp <= 0)
                        {
                            triggered++;
                            CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_295).sim_card.onSecretPlay(this, false, this.enemyHero, dmg);
                            si.usedTrigger_HeroGotDmg(true);
                            foreach (SecretItem sii in this.enemySecretList)
                            {
                                sii.canBe_iceblock = false;
                            }

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
            int triggered = 0;
            if (this.isOwnTurn && playedMinion.own && this.enemySecretCount >= 1)
            {
                foreach (SecretItem si in this.enemySecretList.ToArray())
                {
                    if (si.canBe_snipe)
                    {
                        triggered++;
                        CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_609).sim_card.onSecretPlay(this, false, playedMinion, 0);
                        doDmgTriggers();
                        si.usedTrigger_MinionIsPlayed();
                        foreach (SecretItem sii in this.enemySecretList)
                        {
                            sii.canBe_snipe = false;
                        }
                    }

                    if (si.canBe_mirrorentity)
                    {
                        triggered++;
                        CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_294).sim_card.onSecretPlay(this, false, playedMinion, 0);
                        si.usedTrigger_MinionIsPlayed();
                        foreach (SecretItem sii in this.enemySecretList)
                        {
                            sii.canBe_mirrorentity = false;
                        }

                    }

                    if (si.canBe_repentance)
                    {
                        //triggered++;
                        //CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_379).sim_card.onSecretPlay(this, false, playedMinion, 0);
                        si.usedTrigger_MinionIsPlayed();
                        foreach (SecretItem sii in this.enemySecretList)
                        {
                            sii.canBe_repentance = false;
                        }
                    }
                }
            }

            if (turnCounter == 0)
            {
                this.evaluatePenality -= triggered * 50;
            }

        }

        public int secretTrigger_SpellIsPlayed(Minion target, bool isSpell)
        {
            int triggered = 0;
            if (this.isOwnTurn && isSpell && this.enemySecretCount >= 1) //actual secrets need a spell played!
            {
                foreach (SecretItem si in this.enemySecretList)
                {

                    if (si.canBe_counterspell)
                    {
                        triggered++;
                        // dont use spell!
                        si.usedTrigger_SpellIsPlayed(false);
                        foreach (SecretItem sii in this.enemySecretList)
                        {
                            sii.canBe_counterspell = false;
                        }

                        if (turnCounter == 0)
                        {
                            this.evaluatePenality -= triggered * 50;
                        }
                        return -2;//spellbender will NEVER trigger
                    }
                }



                foreach (SecretItem si in this.enemySecretList)
                {

                    if (si.canBe_spellbender && target != null && !target.isHero)
                    {
                        triggered++;
                        int retval = 0;
                        CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.tt_010).sim_card.onSecretPlay(this, false, null, target, out retval);
                        si.usedTrigger_SpellIsPlayed(true);
                        foreach (SecretItem sii in this.enemySecretList)
                        {
                            sii.canBe_spellbender = false;
                        }

                        if (turnCounter == 0)
                        {
                            this.evaluatePenality -= triggered * 50;
                        }
                        return retval;// the new target
                    }




                }

            }

            if (turnCounter == 0)
            {
                this.evaluatePenality -= triggered * 50;
            }

            return 0;

        }

        public void secretTrigger_MinionDied(bool own)
        {
            int triggered = 0;

            if (this.isOwnTurn && !own && this.enemySecretCount >= 1)
            {

                foreach (SecretItem si in this.enemySecretList)
                {
                    if (si.canBe_duplicate)
                    {
                        triggered++;
                        CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.FP1_018).sim_card.onSecretPlay(this, false, 0);
                        si.usedTrigger_MinionDied();
                        foreach (SecretItem sii in this.enemySecretList)
                        {
                            sii.canBe_duplicate = false;
                        }
                    }

                    if (si.canBe_redemption)
                    {
                        //triggered++;
                        //CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_136).sim_card.onSecretPlay(this, false, 0);
                        si.usedTrigger_MinionDied();
                        foreach (SecretItem sii in this.enemySecretList)
                        {
                            sii.canBe_redemption = false;
                        }
                    }

                    if (si.canBe_avenge)
                    {
                        //triggered++;
                        //CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.FP1_020).sim_card.onSecretPlay(this, false, 0);
                        si.usedTrigger_MinionDied();
                        foreach (SecretItem sii in this.enemySecretList)
                        {
                            sii.canBe_avenge = false;
                        }
                    }


                }
            }

            if (turnCounter == 0)
            {
                this.evaluatePenality -= triggered * 50;
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
                    int pos = (m.own) ? this.ownMinions.Count : this.enemyMinions.Count;
                    callKid(kid, pos, m.own);
                }

                for (int i = 0; i < m.ancestralspirit; i++)
                {
                    CardDB.Card kid = m.handcard.card;
                    int pos = (m.own) ? this.ownMinions.Count : this.enemyMinions.Count;
                    callKid(kid, pos, m.own);
                }

                //baron rivendare ??
                if ((m.own && this.ownBaronRivendare >= 1) || (!m.own && this.enemyBaronRivendare >= 1))
                {
                    if (!m.silenced && m.handcard.card.deathrattle) m.handcard.card.sim_card.onDeathrattle(this, m);

                    for (int i = 0; i < m.souloftheforest; i++)
                    {
                        CardDB.Card kid = CardDB.Instance.getCardDataFromID(CardDB.cardIDEnum.EX1_158t);//Treant
                        int pos = (m.own) ? this.ownMinions.Count : this.enemyMinions.Count;
                        callKid(kid, pos, m.own);
                    }

                    for (int i = 0; i < m.ancestralspirit; i++)
                    {
                        CardDB.Card kid = m.handcard.card;
                        int pos = (m.own) ? this.ownMinions.Count : this.enemyMinions.Count;
                        callKid(kid, pos, m.own);
                    }

                }
            }


        }


        public void updateBoards()
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
                foreach (Minion m in this.ownMinions)
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
                        this.OwnLastDiedMinion = m.handcard.card.cardIDenum;
                        if (this.revivingOwnMinion == CardDB.cardIDEnum.None)
                        {
                            this.revivingOwnMinion = m.handcard.card.cardIDenum;
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
                            this.evaluatePenality -= sf.Ai.botBase.getEnemyMinionValue(m, this) - 1;
                        }*/

                    }
                    else
                    {
                        m.zonepos = i;
                        temp.Add(m);
                        i++;
                    }

                }
                this.ownMinions = temp;
                this.updateAdjacentBuffs(true);
            }

            if (this.tempTrigger.enemyMininsChanged)
            {
                this.tempTrigger.enemyMininsChanged = false;
                List<Minion> temp = new List<Minion>();
                int i = 1;
                foreach (Minion m in this.enemyMinions)
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
                        if (this.revivingEnemyMinion == CardDB.cardIDEnum.None)
                        {
                            this.revivingEnemyMinion = m.handcard.card.cardIDenum;
                            minionEnemyReviving = true;
                        }

                        if ((!m.silenced && m.handcard.card.deathrattle) || m.ancestralspirit >= 1 || m.souloftheforest >= 1)
                        {
                            deathrattles.Add(m);
                        }
                        m.handcard.card.sim_card.onAuraEnds(this, m);

                        if ((!m.silenced && (m.handcard.card.name == CardDB.cardName.cairnebloodhoof || m.handcard.card.name == CardDB.cardName.harvestgolem)) || m.ancestralspirit >= 1)
                        {
                            this.evaluatePenality -= sf.Ai.botBase.getEnemyMinionValue(m, this) - 1;
                        }
                    }
                    else
                    {
                        m.zonepos = i;
                        temp.Add(m);
                        i++;
                    }

                }
                this.enemyMinions = temp;
                this.updateAdjacentBuffs(false);
            }


            if (deathrattles.Count >= 1) this.doDeathrattles(deathrattles);

            if (minionOwnReviving)
            {
                this.secretTrigger_MinionDied(true);
                this.revivingOwnMinion = CardDB.cardIDEnum.None;
            }

            if (minionEnemyReviving)
            {
                this.secretTrigger_MinionDied(false);
                this.revivingEnemyMinion = CardDB.cardIDEnum.None;
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
                angr += 2 * anzMurlocWarleader + anzGrimscaleOracle;
                vert += anzMurlocWarleader;

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
                if (get) m.charge += anzOwnTundrarhino;
                else m.charge -= anzOwnTundrarhino;
                angr += anzOwnRaidleader;
                angr += anzOwnStormwindChamps;
                vert += anzOwnStormwindChamps;
                if (m.handcard.card.race == 20)
                {
                    angr += anzOwnTimberWolfs;
                }
                if (m.handcard.card.race == 23)
                {
                    angr += anzOwnSouthseacaptain;
                    vert += anzOwnSouthseacaptain;

                }
                if (m.handcard.card.race == 15)
                {
                    angr += anzOwnMalGanis *2;
                    vert += anzOwnMalGanis * 2;

                }

            }
            else
            {
                if (get) m.charge += anzEnemyTundrarhino;
                else m.charge -= anzEnemyTundrarhino;
                angr += anzEnemyRaidleader;
                angr += anzEnemyStormwindChamps;
                vert += anzEnemyStormwindChamps;

                if (m.handcard.card.race == 20)
                {
                    angr += anzEnemyTimberWolfs;
                }
                if (m.handcard.card.race == 23)
                {
                    angr += anzEnemySouthseacaptain;
                    vert += anzEnemySouthseacaptain;
                }
                if (m.handcard.card.race == 15)
                {
                    angr += anzEnemyMalGanis * 2;
                    vert += anzEnemyMalGanis * 2;

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
                int anz = this.ownMinions.Count;
                for (int i = 0; i < anz; i++)
                {
                    Minion m = this.ownMinions[i];
                    if (!m.silenced)
                    {
                        if (m.name == CardDB.cardName.direwolfalpha)
                        {
                            if (i > 0) this.minionGetAdjacentBuff(this.ownMinions[i - 1], 1, 0);
                            if (i < anz - 1) this.minionGetAdjacentBuff(this.ownMinions[i + 1], 1, 0);
                        }

                        if (m.name == CardDB.cardName.flametonguetotem)
                        {
                            if (i > 0) this.minionGetAdjacentBuff(this.ownMinions[i - 1], 2, 0);
                            if (i < anz - 1) this.minionGetAdjacentBuff(this.ownMinions[i + 1], 2, 0);
                        }

                        if (m.name == CardDB.cardName.weespellstopper)
                        {
                            if (i > 0) this.ownMinions[i - 1].cantBeTargetedBySpellsOrHeroPowers = true;
                            if (i < anz - 1) this.ownMinions[i + 1].cantBeTargetedBySpellsOrHeroPowers = true;
                        }
                    }
                }
            }
            else
            {
                int anz = this.enemyMinions.Count;
                for (int i = 0; i < anz; i++)
                {
                    Minion m = this.enemyMinions[i];
                    if (!m.silenced)
                    {
                        if (m.name == CardDB.cardName.direwolfalpha)
                        {
                            if (i > 0) this.minionGetAdjacentBuff(this.enemyMinions[i - 1], 1, 0);
                            if (i < anz - 1) this.minionGetAdjacentBuff(this.enemyMinions[i + 1], 1, 0);
                        }

                        if (m.name == CardDB.cardName.flametonguetotem)
                        {
                            if (i > 0) this.minionGetAdjacentBuff(this.enemyMinions[i - 1], 2, 0);
                            if (i < anz - 1) this.minionGetAdjacentBuff(this.enemyMinions[i + 1], 2, 0);
                        }

                        if (m.name == CardDB.cardName.weespellstopper)
                        {
                            if (i > 0) this.enemyMinions[i - 1].cantBeTargetedBySpellsOrHeroPowers = true;
                            if (i < anz - 1) this.enemyMinions[i + 1].cantBeTargetedBySpellsOrHeroPowers = true;
                        }
                    }
                }
            }
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

        public void placeAmobSomewhere(Handmanager.Handcard hc, Minion target, int choice, int zonepos)
        {
            int mobplace = zonepos;

            //create the new minion + trigger Summon effects + buffs it
            Minion m = createNewMinion(hc, mobplace, true);


            //trigger the battlecry!
            m.handcard.card.sim_card.getBattlecryEffect(this, m, target, choice);

            //add minion to list + do triggers + do secret trigger +  minion was played trigger
            addMinionToBattlefield(m);

            secretTrigger_MinionIsPlayed(m);


            if (logging) sf.Helpfunctions.logg("added " + m.handcard.card.name);
        }

        public void addMinionToBattlefield(Minion m, bool isSummon = true)
        {
            List<Minion> temp = (m.own) ? this.ownMinions : this.enemyMinions;
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

        }

        public void equipWeapon(CardDB.Card c, bool own)
        {
            Minion hero = (own) ? this.ownHero : this.enemyHero;
            if (own)
            {
                if (this.ownWeaponDurability >= 1)
                {
                    this.lostWeaponDamage += this.ownWeaponDurability * this.ownWeaponAttack * this.ownWeaponAttack;
                    this.lowerWeaponDurability(1000, true);
                    hero.Angr -= this.ownWeaponAttack;
                }
                this.ownWeaponAttack = c.Attack;
                this.ownWeaponDurability = c.Durability;
                this.ownWeaponName = c.name;
            }
            else
            {
                if (this.enemyWeaponDurability >= 1)
                {
                    hero.Angr -= this.enemyWeaponAttack;
                }
                this.enemyWeaponAttack = c.Attack;
                this.enemyWeaponDurability = c.Durability;
                this.enemyWeaponName = c.name;
            }



            hero.Angr += c.Attack;

            hero.windfury = (c.name == CardDB.cardName.doomhammer);

            hero.updateReadyness();

            hero.immuneWhileAttacking = (c.name == CardDB.cardName.gladiatorslongbow);

            List<Minion> temp = (own) ? this.ownMinions : this.enemyMinions;
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
                if (this.ownMinions.Count >= allowed)
                {
                    if(spawnKid) this.evaluatePenality += 20;
                    return;
                }
            }
            else
            {
                if (this.enemyMinions.Count >= allowed)
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
            List<Minion> temp = (own) ? this.ownMinions : this.enemyMinions;
            foreach (Minion m in temp)
            {
                m.becomeSilence(this);
            }
        }

        public void drawACard(CardDB.cardName ss, bool own, bool nopen = false)
        {
            CardDB.cardName s = ss;

            // cant hold more than 10 cards
            if (own)
            {
                
                if (s == CardDB.cardName.unknown && !nopen) // draw a card from deck :D
                {
                    if (ownDeckSize == 0)
                    {
                        this.ownHeroFatigue++;
                        this.ownHero.getDamageOrHeal(this.ownHeroFatigue, this, false, true);
                        return;
                    }
                    else
                    {
                        this.ownDeckSize--;
                        if (this.owncards.Count >= 10)
                        {
                            this.evaluatePenality += 15;
                            return;
                        }
                        this.owncarddraw++;
                    }

                }
                else
                {
                    if (this.owncards.Count >= 10)
                    {
                        this.evaluatePenality += 5;
                        return;
                    }
                    this.owncarddraw++;

                }


            }
            else
            {
                if (s == CardDB.cardName.unknown && !nopen) // draw a card from deck :D
                {
                    if (enemyDeckSize == 0)
                    {
                        this.enemyHeroFatigue++;
                        this.enemyHero.getDamageOrHeal(this.enemyHeroFatigue, this, false, true);
                        return;
                    }
                    else
                    {
                        this.enemyDeckSize--;
                        if (this.enemyAnzCards >= 10)
                        {
                            this.evaluatePenality -= 50;
                            return;
                        }
                        this.enemycarddraw++;
                        this.enemyAnzCards++;
                    }

                }
                else
                {
                    if (this.enemyAnzCards >= 10)
                    {
                        this.evaluatePenality -= 50;
                        return;
                    }
                    this.enemycarddraw++;
                    this.enemyAnzCards++;
                }
                this.triggerCardsChanged(false);

                if (anzEnemyChromaggus > 0 && s == CardDB.cardName.unknown && !nopen)
                {
                    for (int i = 1; i <= anzEnemyChromaggus; i++)
                    {
                        if (this.enemyAnzCards >= 10)
                        {
                            this.evaluatePenality -= 50;
                            return;
                        }
                        this.enemycarddraw++;
                        this.enemyAnzCards++;
                        this.triggerCardsChanged(false);

                    }
                }
                return;
            }

            if (s == CardDB.cardName.unknown)
            {
                CardDB.Card plchldr = new CardDB.Card { name = CardDB.cardName.unknown };
                Handmanager.Handcard hc = new Handmanager.Handcard { card = plchldr, position = this.owncards.Count + 1, manacost = 1000, entity = this.getNextEntity() };
                this.owncards.Add(hc);
                this.triggerCardsChanged(true);
            }
            else
            {
                CardDB.Card c = CardDB.Instance.getCardData(s);
                Handmanager.Handcard hc = new Handmanager.Handcard { card = c, position = this.owncards.Count + 1, manacost = c.calculateManaCost(this), entity = this.getNextEntity() };
                this.owncards.Add(hc);
                this.triggerCardsChanged(true);
            }
           
            if (anzOwnChromaggus > 0 && s == CardDB.cardName.unknown && !nopen) // only draw a card from deck
            {
                CardDB.Card plchldr = new CardDB.Card { name = CardDB.cardName.unknown };
                for (int i = 1; i <= anzOwnChromaggus; i++)
                {
                    if (this.owncards.Count >= 10)
                    {
                        this.evaluatePenality += 15;
                        return;
                    }
                    this.owncarddraw++;

                    Handmanager.Handcard hc = new Handmanager.Handcard { card = plchldr, position = this.owncards.Count + 1, manacost = 1000, entity = this.getNextEntity() };
                    this.owncards.Add(hc);
                    this.triggerCardsChanged(true);
                }
            }

        }

        public void drawACard(CardDB.cardIDEnum ss, bool own, bool nopen = false)
        {
            CardDB.cardIDEnum s = ss;

            // cant hold more than 10 cards

            if (own)
            {
                if (s == CardDB.cardIDEnum.None && !nopen) // draw a card from deck :D
                {
                    if (ownDeckSize == 0)
                    {
                        this.ownHeroFatigue++;
                        this.ownHero.getDamageOrHeal(this.ownHeroFatigue, this, false, true);
                        return;
                    }
                    else
                    {
                        this.ownDeckSize--;
                        if (this.owncards.Count >= 10)
                        {
                            this.evaluatePenality += 15;
                            return;
                        }
                        this.owncarddraw++;
                    }

                }
                else
                {
                    if (this.owncards.Count >= 10)
                    {
                        this.evaluatePenality += 5;
                        return;
                    }
                    this.owncarddraw++;
                }
            }
            else
            {
                if (s == CardDB.cardIDEnum.None && !nopen) // draw a card from deck :D
                {
                    if (enemyDeckSize == 0)
                    {
                        this.enemyHeroFatigue++;
                        this.enemyHero.getDamageOrHeal(this.enemyHeroFatigue, this, false, true);
                        return;
                    }
                    else
                    {
                        this.enemyDeckSize--;
                        if (this.enemyAnzCards >= 10)
                        {
                            this.evaluatePenality -= 50;
                            return;
                        }
                        this.enemycarddraw++;
                        this.enemyAnzCards++;
                    }
                }
                else
                {
                    if (this.enemyAnzCards >= 10)
                    {
                        this.evaluatePenality -= 50;
                        return;
                    }
                    this.enemycarddraw++;
                    this.enemyAnzCards++;

                }
                this.triggerCardsChanged(false);

                if (anzEnemyChromaggus > 0 && s == CardDB.cardIDEnum.None && !nopen)
                {
                    for (int i = 1; i <= anzEnemyChromaggus; i++)
                    {
                        if (this.enemyAnzCards >= 10)
                        {
                            this.evaluatePenality -= 50;
                            return;
                        }
                        this.enemycarddraw++;
                        this.enemyAnzCards++;
                        this.triggerCardsChanged(false);
                    }
                }
                return;
            }

            if (s == CardDB.cardIDEnum.None)
            {
                CardDB.Card plchldr = new CardDB.Card { name = CardDB.cardName.unknown };
                Handmanager.Handcard hc = new Handmanager.Handcard { card = plchldr, position = this.owncards.Count + 1, manacost = 1000, entity = this.getNextEntity() };
                this.owncards.Add(hc);
                this.triggerCardsChanged(true);
            }
            else
            {
                CardDB.Card c = CardDB.Instance.getCardDataFromID(s);
                Handmanager.Handcard hc = new Handmanager.Handcard { card = c, position = this.owncards.Count + 1, manacost = c.calculateManaCost(this), entity = this.getNextEntity() };
                this.owncards.Add(hc);
                this.triggerCardsChanged(true);
            }

            if (anzOwnChromaggus > 0 && s == CardDB.cardIDEnum.None && !nopen) // only draw a card from deck
            {
                CardDB.Card plchldr = new CardDB.Card { name = CardDB.cardName.unknown };
                for (int i = 1; i <= anzOwnChromaggus; i++)
                {
                    if (this.owncards.Count >= 10)
                    {
                        this.evaluatePenality += 15;
                        return;
                    }
                    this.owncarddraw++;

                    Handmanager.Handcard hc = new Handmanager.Handcard { card = plchldr, position = this.owncards.Count + 1, manacost = 1000, entity = this.getNextEntity() };
                    this.owncards.Add(hc);
                    this.triggerCardsChanged(true);
                }
            }

        }


        public void removeCard(Handmanager.Handcard hcc)
        {
            int cardPos = 1;
            int cardNum = 0;
            Handmanager.Handcard hcTmp = null;
            foreach (Handmanager.Handcard hc in this.owncards)
            {
                if (hc.entity == hcc.entity)
                {
                    hcTmp = hc;
                    cardNum++;
                    continue;
                }
                this.owncards[cardNum].position = cardPos;
                cardNum++;
                cardPos++;
            }
            if (hcTmp != null) this.owncards.Remove(hcTmp);
        }


        // some helpfunctions 


        public void attackEnemyHeroWithoutKill(int dmg)
        {
            this.enemyHero.cantLowerHPbelowONE = true;
            this.minionGetDamageOrHeal(this.enemyHero, dmg);
            this.enemyHero.cantLowerHPbelowONE = false;
        }

        public void minionGetDestroyed(Minion m)
        {
            if (m.Hp > 0)
            {
                m.Hp = 0;
                m.minionDied(this);
            }

        }

        public void allMinionsGetDestroyed()
        {
            foreach (Minion m in this.ownMinions)
            {
                minionGetDestroyed(m);
            }
            foreach (Minion m in this.enemyMinions)
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
            List<Minion> temp = (own) ? this.ownMinions : this.enemyMinions;
            m.handcard.card.sim_card.onAuraEnds(this, m);
            temp.Remove(m);

            if (own)
            {
                CardDB.Card c = m.handcard.card;
                Handmanager.Handcard hc = new Handmanager.Handcard { card = c, position = this.owncards.Count + 1, entity = m.entitiyID, manacost = c.calculateManaCost(this) + manachange };
                if (this.owncards.Count < 10)
                {
                    this.owncards.Add(hc);
                    this.triggerCardsChanged(true);
                }
                else
                {
                    this.drawACard(CardDB.cardName.unknown, true);
                }
                this.tempTrigger.ownMinionsChanged = true;
            }
            else
            {
                this.drawACard(CardDB.cardName.unknown, true);
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
                this.evaluatePenality -= sf.Ai.botBase.getEnemyMinionValue(m, this) - 1;
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

            if (logging) sf.Helpfunctions.logg("minion got sheep" + m.name + " " + m.Angr);
        }

        public void minionGetControlled(Minion m, bool newOwner, bool canAttack)
        {
            List<Minion> newOwnerList = (newOwner) ? this.ownMinions : this.enemyMinions;
            List<Minion> oldOwnerList = (newOwner) ? this.enemyMinions : this.ownMinions;



            if (newOwnerList.Count >= 7) return;

            this.tempTrigger.ownMinionsChanged = true;
            this.tempTrigger.enemyMininsChanged = true;
            if (m.taunt && newOwner) this.anzEnemyTaunt--;

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
            List<Minion> temp = (own) ? this.ownMinions : this.enemyMinions;
            foreach (Minion m in temp)
            {
                if (frozen) m.frozen = true;
                minionGetDamageOrHeal(m, damages, true);
            }
        }

        public void allCharsOfASideGetDamage(bool own, int damages)
        {
            //ALL CHARS get same dmg
            List<Minion> temp = (own) ? this.ownMinions : this.enemyMinions;
            foreach (Minion m in temp)
            {
                minionGetDamageOrHeal(m, damages, true);
            }

            this.minionGetDamageOrHeal(own ? this.ownHero : this.enemyHero, damages);
        }

        public void allCharsGetDamage(int damages)
        {
            foreach (Minion m in this.ownMinions)
            {
                minionGetDamageOrHeal(m, damages, true);
            }
            foreach (Minion m in this.enemyMinions)
            {
                minionGetDamageOrHeal(m, damages, true);
            }
            minionGetDamageOrHeal(this.ownHero, damages);
            minionGetDamageOrHeal(this.enemyHero, damages);
        }

        public void allMinionsGetDamage(int damages)
        {
            foreach (Minion m in this.ownMinions)
            {
                minionGetDamageOrHeal(m, damages, true);
            }
            foreach (Minion m in this.enemyMinions)
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


        public void debugMinions()
        {
            sf.Helpfunctions.logg("OWN MINIONS################");

            foreach (Minion m in this.ownMinions)
            {
                sf.Helpfunctions.logg("name,ang, hp, maxhp: " + m.name + ", " + m.Angr + ", " + m.Hp + ", " + m.maxHp);
            }

            sf.Helpfunctions.logg("ENEMY MINIONS############");
            foreach (Minion m in this.enemyMinions)
            {
                sf.Helpfunctions.logg("name,ang, hp: " + m.name + ", " + m.Angr + ", " + m.Hp);
            }
        }

        public void printBoard()
        {
            sf.Helpfunctions.logg("board/hash: " + value + "  /  " + this.hashcode + " ++++++++++++++++++++++");
            sf.Helpfunctions.logg("pen " + this.evaluatePenality);
            sf.Helpfunctions.logg("mana " + this.mana + "/" + this.ownMaxMana);
            sf.Helpfunctions.logg("cardsplayed: " + this.cardsPlayedThisTurn + " handsize: " + this.owncards.Count + " eh " + this.enemyAnzCards);

            sf.Helpfunctions.logg("ownhero: ");
            sf.Helpfunctions.logg("ownherohp: " + this.ownHero.Hp + " + " + this.ownHero.armor);
            sf.Helpfunctions.logg("ownheroattac: " + this.ownHero.Angr);
            sf.Helpfunctions.logg("ownheroweapon: " + this.ownWeaponAttack + " " + this.ownWeaponDurability + " " + this.ownWeaponName);
            sf.Helpfunctions.logg("ownherostatus: frozen" + this.ownHero.frozen + " ");
            sf.Helpfunctions.logg("enemyherohp: " + this.enemyHero.Hp + " + " + this.enemyHero.armor + ((this.enemyHero.immune) ? " immune" : ""));

            if (this.enemySecretCount >= 1) sf.Helpfunctions.logg("enemySecrets: " + sf.Probabilitymaker.getEnemySecretData(this.enemySecretList));
            foreach (Action a in this.playactions)
            {
                a.print(sf.Helpfunctions);
            }
            sf.Helpfunctions.logg("OWN MINIONS################");

            foreach (Minion m in this.ownMinions)
            {
                sf.Helpfunctions.logg("name,ang, hp: " + m.name + ", " + m.Angr + ", " + m.Hp + " " + m.entitiyID + ", own=" + m.own);
            }

            sf.Helpfunctions.logg("ENEMY MINIONS############");
            foreach (Minion m in this.enemyMinions)
            {
                sf.Helpfunctions.logg("name,ang, hp: " + m.name + ", " + m.Angr + ", " + m.Hp + " " + m.entitiyID + ", own=" + m.own);
            }


            sf.Helpfunctions.logg("");
        }

        public void printBoardDebug()
        {
            sf.Helpfunctions.logg("hero " + this.ownHero.Hp + " " + this.ownHero.armor + " " + this.ownHero.entitiyID);
            sf.Helpfunctions.logg("ehero " + this.enemyHero.Hp + " " + this.enemyHero.armor + " " + this.enemyHero.entitiyID);
            foreach (Minion m in ownMinions)
            {
                sf.Helpfunctions.logg(m.name + " " + m.entitiyID);
            }
            sf.Helpfunctions.logg("-");
            foreach (Minion m in enemyMinions)
            {
                sf.Helpfunctions.logg(m.name + " " + m.entitiyID);
            }
            sf.Helpfunctions.logg("-");
            foreach (Handmanager.Handcard hc in this.owncards)
            {
                sf.Helpfunctions.logg(hc.position + " " + hc.card.name + " " + hc.entity);
            }
        }

        public Action getNextAction()
        {
            if (this.playactions.Count >= 1) return this.playactions[0];
            return null;
        }

        public void printActions(bool toBuffer = false)
        {
            foreach (Action a in this.playactions)
            {
                a.print(sf.Helpfunctions, toBuffer);
                sf.Helpfunctions.logg("");
            }
        }

        public void printActionforDummies(Action a)
        {
            if (a.actionType == actionEnum.playcard)
            {
                sf.Helpfunctions.ErrorLog("play " + a.card.card.name);
                if (a.druidchoice >= 1)
                {
                    string choose = (a.druidchoice == 1) ? "left card" : "right card";
                    sf.Helpfunctions.ErrorLog("choose the " + choose);
                }
                if (a.place >= 1)
                {
                    sf.Helpfunctions.ErrorLog("on position " + a.place);
                }
                if (a.target != null)
                {
                    if (!a.target.own && !a.target.isHero)
                    {
                        string ename = "" + a.target.name;
                        sf.Helpfunctions.ErrorLog("and target to the enemy " + ename);
                    }

                    if (a.target.own && !a.target.isHero)
                    {
                        string ename = "" + a.target.name;
                        sf.Helpfunctions.ErrorLog("and target to your own" + ename);
                    }

                    if (a.target.own && a.target.isHero)
                    {
                        sf.Helpfunctions.ErrorLog("and target your own hero");
                    }

                    if (!a.target.own && a.target.isHero)
                    {
                        sf.Helpfunctions.ErrorLog("and target to the enemy hero");
                    }
                }

            }
            if (a.actionType == actionEnum.attackWithMinion)
            {
                string name = "" + a.own.name;
                if (a.target.isHero)
                {
                    sf.Helpfunctions.ErrorLog( "attack with: " + name + " the enemy hero");
                }
                else
                {
                    string ename = "" + a.target.name;
                    sf.Helpfunctions.ErrorLog("attack with: " + name + " the enemy: " + ename);
                }

            }

            if (a.actionType == actionEnum.attackWithHero)
            {
                if (a.target.isHero)
                {
                    sf.Helpfunctions.ErrorLog("attack with your hero the enemy hero!");
                }
                else
                {
                    string ename = "" + a.target.name;
                    sf.Helpfunctions.ErrorLog("attack with the hero, and choose the enemy: " + ename);
                }
            }
            if (a.actionType == actionEnum.useHeroPower)
            {
                sf.Helpfunctions.ErrorLog("use your Heropower ");
                if (a.target != null)
                {
                    if (!a.target.own && !a.target.isHero)
                    {
                        string ename = "" + a.target.name;
                        sf.Helpfunctions.ErrorLog("on enemy: " + ename);
                    }

                    if (a.target.own && !a.target.isHero)
                    {
                        string ename = "" + a.target.name;
                        sf.Helpfunctions.ErrorLog("on your own: " + ename);
                    }

                    if (a.target.own && a.target.isHero)
                    {
                        sf.Helpfunctions.ErrorLog("on your own hero");
                    }

                    if (!a.target.own && a.target.isHero)
                    {
                        sf.Helpfunctions.ErrorLog("on your the enemy hero");
                    }

                }
            }
            sf.Helpfunctions.ErrorLog("");

        }


    }

   

}