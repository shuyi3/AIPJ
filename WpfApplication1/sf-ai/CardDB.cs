﻿namespace SilverfishAi
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public struct targett
    {
        public int target;
        public int targetEntity;

        public targett(int targ, int ent)
        {
            this.target = targ;
            this.targetEntity = ent;
        }
    }


    public class CardDB
    {

        public Dictionary<CardDB.cardName, int> HealTargetDatabase = new Dictionary<CardDB.cardName, int>();
        public Dictionary<CardDB.cardName, int> HealHeroDatabase = new Dictionary<CardDB.cardName, int>();
        public Dictionary<CardDB.cardName, int> HealAllDatabase = new Dictionary<CardDB.cardName, int>();


        public Dictionary<CardDB.cardName, int> DamageAllDatabase = new Dictionary<CardDB.cardName, int>();
        public Dictionary<CardDB.cardName, int> DamageHeroDatabase = new Dictionary<CardDB.cardName, int>();
        public Dictionary<CardDB.cardName, int> DamageRandomDatabase = new Dictionary<CardDB.cardName, int>();
        public Dictionary<CardDB.cardName, int> DamageAllEnemysDatabase = new Dictionary<CardDB.cardName, int>();

        public Dictionary<CardDB.cardName, int> enrageDatabase = new Dictionary<CardDB.cardName, int>();
        public Dictionary<CardDB.cardName, int> silenceDatabase = new Dictionary<CardDB.cardName, int>();
        public Dictionary<CardDB.cardName, int> OwnNeedSilenceDatabase = new Dictionary<CardDB.cardName, int>();

        public Dictionary<CardDB.cardName, int> heroAttackBuffDatabase = new Dictionary<CardDB.cardName, int>();
        public Dictionary<CardDB.cardName, int> attackBuffDatabase = new Dictionary<CardDB.cardName, int>();
        public Dictionary<CardDB.cardName, int> healthBuffDatabase = new Dictionary<CardDB.cardName, int>();
        public Dictionary<CardDB.cardName, int> tauntBuffDatabase = new Dictionary<CardDB.cardName, int>();

        public Dictionary<CardDB.cardName, int> lethalHelpers = new Dictionary<CardDB.cardName, int>();

        public Dictionary<CardDB.cardName, int> spellDependentDatabase = new Dictionary<CardDB.cardName, int>();

        public Dictionary<CardDB.cardName, int> cardDiscardDatabase = new Dictionary<CardDB.cardName, int>();
        public Dictionary<CardDB.cardName, int> destroyOwnDatabase = new Dictionary<CardDB.cardName, int>();
        public Dictionary<CardDB.cardName, int> destroyDatabase = new Dictionary<CardDB.cardName, int>();
        public Dictionary<CardDB.cardName, int> buffingMinionsDatabase = new Dictionary<CardDB.cardName, int>();
        public Dictionary<CardDB.cardName, int> buffing1TurnDatabase = new Dictionary<CardDB.cardName, int>();
        public Dictionary<CardDB.cardName, int> heroDamagingAoeDatabase = new Dictionary<CardDB.cardName, int>();
        public Dictionary<CardDB.cardName, int> randomEffects = new Dictionary<CardDB.cardName, int>();

        public Dictionary<CardDB.cardName, int> silenceTargets = new Dictionary<CardDB.cardName, int>();

        public Dictionary<CardDB.cardName, int> returnHandDatabase = new Dictionary<CardDB.cardName, int>();

        public Dictionary<CardDB.cardName, int> priorityDatabase = new Dictionary<CardDB.cardName, int>();
        public Dictionary<CardDB.cardName, int> UsefulNeedKeepDatabase = new Dictionary<CardDB.cardName, int>();
        public Dictionary<CardDB.cardName, CardDB.cardIDEnum> choose1database = new Dictionary<CardDB.cardName, CardDB.cardIDEnum>();
        public Dictionary<CardDB.cardName, CardDB.cardIDEnum> choose2database = new Dictionary<CardDB.cardName, CardDB.cardIDEnum>();

        public Dictionary<CardDB.cardName, int> DamageTargetDatabase = new Dictionary<CardDB.cardName, int>();
        public Dictionary<CardDB.cardName, int> DamageTargetSpecialDatabase = new Dictionary<CardDB.cardName, int>();
        public Dictionary<CardDB.cardName, int> cardDrawBattleCryDatabase = new Dictionary<CardDB.cardName, int>();
        public Dictionary<CardDB.cardName, int> priorityTargets = new Dictionary<CardDB.cardName, int>();
        public Dictionary<CardDB.cardName, int> specialMinions = new Dictionary<CardDB.cardName, int>(); //minions with cardtext, but no battlecry

        // Data is stored in hearthstone-folder -> data->win cardxml0
        //(data-> cardxml0 seems outdated (blutelfkleriker has 3hp there >_>)
        public enum cardtype
        {
            NONE,
            MOB,
            SPELL,
            WEAPON,
            HEROPWR,
            ENCHANTMENT,

        }

        public enum cardrace
        {
            INVALID,
            BLOODELF,
            DRAENEI,
            DWARF,
            GNOME,
            GOBLIN,
            HUMAN,
            NIGHTELF,
            ORC,
            TAUREN,
            TROLL,
            UNDEAD,
            WORGEN,
            GOBLIN2,
            MURLOC,
            DEMON,
            SCOURGE,
            MECHANICAL,
            ELEMENTAL,
            OGRE,
            PET,
            TOTEM,
            NERUBIAN,
            PIRATE,
            DRAGON
        }


        public enum cardIDEnum
        {
            None,
            CS1h_001,
            CS1_042,
            CS1_112,
            CS1_113,
            CS1_113e,
            CS1_130,
            CS2_003,
            CS2_004,
            CS2_004e,
            CS2_005,
            CS2_005o,
            CS2_007,
            CS2_008,
            CS2_009,
            CS2_009e,
            CS2_011,
            CS2_011o,
            CS2_012,
            CS2_013,
            CS2_013t,
            CS2_017,
            CS2_017o,
            CS2_022,
            CS2_022e,
            CS2_023,
            CS2_024,
            CS2_025,
            CS2_026,
            CS2_027,
            CS2_029,
            CS2_032,
            CS2_033,
            CS2_034,
            CS2_037,
            CS2_039,
            CS2_041,
            CS2_041e,
            CS2_042,
            CS2_045,
            CS2_045e,
            CS2_046,
            CS2_046e,
            CS2_049,
            CS2_050,
            CS2_051,
            CS2_052,
            CS2_056,
            CS2_057,
            CS2_061,
            CS2_062,
            CS2_063,
            CS2_063e,
            CS2_064,
            CS2_065,
            CS2_072,
            CS2_074,
            CS2_074e,
            CS2_075,
            CS2_076,
            CS2_077,
            CS2_080,
            CS2_082,
            CS2_083b,
            CS2_083e,
            CS2_084,
            CS2_084e,
            CS2_087,
            CS2_087e,
            CS2_088,
            CS2_089,
            CS2_091,
            CS2_092,
            CS2_092e,
            CS2_093,
            CS2_094,
            CS2_097,
            CS2_101,
            CS2_101t,
            CS2_102,
            CS2_103,
            CS2_103e2,
            CS2_105,
            CS2_105e,
            CS2_106,
            CS2_108,
            CS2_112,
            CS2_114,
            CS2_118,
            CS2_119,
            CS2_120,
            CS2_121,
            CS2_122,
            CS2_122e,
            CS2_124,
            CS2_125,
            CS2_127,
            CS2_131,
            CS2_141,
            CS2_142,
            CS2_147,
            CS2_150,
            CS2_155,
            CS2_162,
            CS2_168,
            CS2_171,
            CS2_172,
            CS2_173,
            CS2_179,
            CS2_182,
            CS2_186,
            CS2_187,
            CS2_189,
            CS2_196,
            CS2_197,
            CS2_200,
            CS2_201,
            CS2_213,
            CS2_222,
            CS2_222o,
            CS2_226,
            CS2_226e,
            CS2_232,
            CS2_234,
            CS2_235,
            CS2_236,
            CS2_236e,
            CS2_237,
            CS2_boar,
            CS2_mirror,
            CS2_tk1,
            DS1h_292,
            DS1_055,
            DS1_070,
            DS1_070o,
            DS1_175,
            DS1_175o,
            DS1_178,
            DS1_178e,
            DS1_183,
            DS1_184,
            DS1_185,
            DS1_233,
            EX1_011,
            EX1_015,
            EX1_019,
            EX1_019e,
            EX1_025,
            EX1_025t,
            EX1_066,
            EX1_084,
            EX1_084e,
            EX1_129,
            EX1_169,
            EX1_173,
            EX1_244,
            EX1_244e,
            EX1_246,
            EX1_246e,
            EX1_277,
            EX1_278,
            EX1_302,
            EX1_306,
            EX1_308,
            EX1_360,
            EX1_360e,
            EX1_371,
            EX1_399,
            EX1_399e,
            EX1_400,
            EX1_506,
            EX1_506a,
            EX1_508,
            EX1_508o,
            EX1_539,
            EX1_565,
            EX1_565o,
            EX1_581,
            EX1_582,
            EX1_587,
            EX1_593,
            EX1_606,
            EX1_622,
            GAME_001,
            GAME_002,
            GAME_003,
            GAME_003e,
            GAME_004,
            GAME_005,
            GAME_005e,
            GAME_006,
            HERO_01,
            HERO_02,
            HERO_03,
            HERO_04,
            HERO_05,
            HERO_06,
            HERO_07,
            HERO_08,
            HERO_09,
            hexfrog,
            NEW1_003,
            NEW1_004,
            NEW1_009,
            NEW1_011,
            NEW1_031,
            NEW1_032,
            NEW1_033,
            NEW1_033o,
            NEW1_034,
            skele11,
            CS1_069,
            CS1_129,
            CS1_129e,
            CS2_028,
            CS2_031,
            CS2_038,
            CS2_038e,
            CS2_053,
            CS2_053e,
            CS2_059,
            CS2_059o,
            CS2_073,
            CS2_073e,
            CS2_073e2,
            CS2_104,
            CS2_104e,
            CS2_117,
            CS2_146,
            CS2_146o,
            CS2_151,
            CS2_152,
            CS2_161,
            CS2_169,
            CS2_181,
            CS2_181e,
            CS2_188,
            CS2_188o,
            CS2_203,
            CS2_221,
            CS2_221e,
            CS2_227,
            CS2_231,
            CS2_233,
            DREAM_01,
            DREAM_02,
            DREAM_03,
            DREAM_04,
            DREAM_05,
            DREAM_05e,
            DS1_188,
            ds1_whelptoken,
            EX1_001,
            EX1_001e,
            EX1_002,
            EX1_004,
            EX1_004e,
            EX1_005,
            EX1_006,
            EX1_007,
            EX1_008,
            EX1_009,
            EX1_010,
            EX1_012,
            EX1_014,
            EX1_014t,
            EX1_014te,
            EX1_016,
            EX1_017,
            EX1_020,
            EX1_021,
            EX1_023,
            EX1_028,
            EX1_029,
            EX1_032,
            EX1_033,
            EX1_043,
            EX1_043e,
            EX1_044,
            EX1_044e,
            EX1_045,
            EX1_046,
            EX1_046e,
            EX1_048,
            EX1_049,
            EX1_050,
            EX1_055,
            EX1_055o,
            EX1_057,
            EX1_058,
            EX1_059,
            EX1_059e,
            EX1_067,
            EX1_076,
            EX1_080,
            EX1_080o,
            EX1_082,
            EX1_083,
            EX1_085,
            EX1_089,
            EX1_091,
            EX1_093,
            EX1_093e,
            EX1_095,
            EX1_096,
            EX1_097,
            EX1_100,
            EX1_102,
            EX1_103,
            EX1_103e,
            EX1_105,
            EX1_110,
            EX1_110t,
            EX1_116,
            EX1_116t,
            EX1_124,
            EX1_126,
            EX1_128,
            EX1_128e,
            EX1_130,
            EX1_130a,
            EX1_131,
            EX1_131t,
            EX1_132,
            EX1_133,
            EX1_134,
            EX1_136,
            EX1_137,
            EX1_144,
            EX1_145,
            EX1_145o,
            EX1_154,
            EX1_154a,
            EX1_154b,
            EX1_155,
            EX1_155a,
            EX1_155ae,
            EX1_155b,
            EX1_155be,
            EX1_158,
            EX1_158e,
            EX1_158t,
            EX1_160,
            EX1_160a,
            EX1_160b,
            EX1_160be,
            EX1_160t,
            EX1_161,
            EX1_161o,
            EX1_162,
            EX1_162o,
            EX1_164,
            EX1_164a,
            EX1_164b,
            EX1_165,
            EX1_165a,
            EX1_165b,
            EX1_165t1,
            EX1_165t2,
            EX1_166,
            EX1_166a,
            EX1_166b,
            EX1_170,
            EX1_178,
            EX1_178a,
            EX1_178ae,
            EX1_178b,
            EX1_178be,
            EX1_238,
            EX1_241,
            EX1_243,
            EX1_245,
            EX1_247,
            EX1_248,
            EX1_249,
            EX1_250,
            EX1_251,
            EX1_258,
            EX1_258e,
            EX1_259,
            EX1_274,
            EX1_274e,
            EX1_275,
            EX1_279,
            EX1_283,
            EX1_284,
            EX1_287,
            EX1_289,
            EX1_294,
            EX1_295,
            EX1_295o,
            EX1_298,
            EX1_301,
            EX1_303,
            EX1_304,
            EX1_304e,
            EX1_309,
            EX1_310,
            EX1_312,
            EX1_313,
            EX1_315,
            EX1_316,
            EX1_316e,
            EX1_317,
            EX1_317t,
            EX1_319,
            EX1_320,
            EX1_323,
            EX1_323h,
            EX1_323w,
            EX1_332,
            EX1_334,
            EX1_334e,
            EX1_335,
            EX1_339,
            EX1_341,
            EX1_345,
            EX1_345t,
            EX1_349,
            EX1_350,
            EX1_354,
            EX1_355,
            EX1_355e,
            EX1_362,
            EX1_363,
            EX1_363e,
            EX1_363e2,
            EX1_365,
            EX1_366,
            EX1_366e,
            EX1_379,
            EX1_379e,
            EX1_382,
            EX1_382e,
            EX1_383,
            EX1_383t,
            EX1_384,
            EX1_390,
            EX1_391,
            EX1_392,
            EX1_393,
            EX1_396,
            EX1_398,
            EX1_398t,
            EX1_402,
            EX1_405,
            EX1_407,
            EX1_408,
            EX1_409,
            EX1_409e,
            EX1_409t,
            EX1_410,
            EX1_411,
            EX1_411e,
            EX1_411e2,
            EX1_412,
            EX1_414,
            EX1_507,
            EX1_507e,
            EX1_509,
            EX1_509e,
            EX1_522,
            EX1_531,
            EX1_531e,
            EX1_533,
            EX1_534,
            EX1_534t,
            EX1_536,
            EX1_536e,
            EX1_537,
            EX1_538,
            EX1_538t,
            EX1_543,
            EX1_544,
            EX1_549,
            EX1_549o,
            EX1_554,
            EX1_554t,
            EX1_556,
            EX1_557,
            EX1_558,
            EX1_559,
            EX1_560,
            EX1_561,
            EX1_561e,
            EX1_562,
            EX1_563,
            EX1_564,
            EX1_567,
            EX1_570,
            EX1_570e,
            EX1_571,
            EX1_572,
            EX1_573,
            EX1_573a,
            EX1_573ae,
            EX1_573b,
            EX1_573t,
            EX1_575,
            EX1_577,
            EX1_578,
            EX1_583,
            EX1_584,
            EX1_584e,
            EX1_586,
            EX1_590,
            EX1_590e,
            EX1_591,
            EX1_594,
            EX1_595,
            EX1_596,
            EX1_596e,
            EX1_597,
            EX1_598,
            EX1_603,
            EX1_603e,
            EX1_604,
            EX1_604o,
            EX1_607,
            EX1_607e,
            EX1_608,
            EX1_609,
            EX1_610,
            EX1_611,
            EX1_611e,
            EX1_612,
            EX1_612o,
            EX1_613,
            EX1_613e,
            EX1_614,
            EX1_614t,
            EX1_616,
            EX1_617,
            EX1_619,
            EX1_619e,
            EX1_620,
            EX1_621,
            EX1_623,
            EX1_623e,
            EX1_624,
            EX1_625,
            EX1_625t,
            EX1_625t2,
            EX1_626,
            EX1_finkle,
            EX1_tk11,
            EX1_tk28,
            EX1_tk29,
            EX1_tk31,
            EX1_tk33,
            EX1_tk34,
            EX1_tk9,
            NEW1_005,
            NEW1_007,
            NEW1_007a,
            NEW1_007b,
            NEW1_008,
            NEW1_008a,
            NEW1_008b,
            NEW1_010,
            NEW1_012,
            NEW1_012o,
            NEW1_014,
            NEW1_017,
            NEW1_017e,
            NEW1_018,
            NEW1_018e,
            NEW1_019,
            NEW1_020,
            NEW1_021,
            NEW1_022,
            NEW1_023,
            NEW1_024,
            NEW1_024o,
            NEW1_025,
            NEW1_025e,
            NEW1_026,
            NEW1_026t,
            NEW1_027,
            NEW1_027e,
            NEW1_029,
            NEW1_029t,
            NEW1_030,
            NEW1_036,
            NEW1_036e,
            NEW1_036e2,
            NEW1_037,
            NEW1_037e,
            NEW1_038,
            NEW1_038o,
            NEW1_040,
            NEW1_040t,
            NEW1_041,
            skele21,
            tt_004,
            tt_004o,
            tt_010,
            tt_010a,
            CRED_01,
            CRED_02,
            CRED_03,
            CRED_04,
            CRED_05,
            CRED_06,
            CRED_07,
            CRED_08,
            CRED_09,
            CRED_10,
            CRED_11,
            CRED_12,
            CRED_13,
            CRED_14,
            CRED_15,
            CRED_16,
            CRED_17,
            EX1_062,
            NEW1_016,
            TU4a_001,
            TU4a_002,
            TU4a_003,
            TU4a_004,
            TU4a_005,
            TU4a_006,
            TU4b_001,
            TU4c_001,
            TU4c_002,
            TU4c_003,
            TU4c_004,
            TU4c_005,
            TU4c_006,
            TU4c_006e,
            TU4c_007,
            TU4c_008,
            TU4c_008e,
            TU4d_001,
            TU4d_002,
            TU4d_003,
            TU4e_001,
            TU4e_002,
            TU4e_002t,
            TU4e_003,
            TU4e_004,
            TU4e_005,
            TU4e_007,
            TU4f_001,
            TU4f_002,
            TU4f_003,
            TU4f_004,
            TU4f_004o,
            TU4f_005,
            TU4f_006,
            TU4f_006o,
            TU4f_007,
            XXX_001,
            XXX_002,
            XXX_003,
            XXX_004,
            XXX_005,
            XXX_006,
            XXX_007,
            XXX_008,
            XXX_009,
            XXX_009e,
            XXX_010,
            XXX_011,
            XXX_012,
            XXX_013,
            XXX_014,
            XXX_015,
            XXX_016,
            XXX_017,
            XXX_018,
            XXX_019,
            XXX_020,
            XXX_021,
            XXX_022,
            XXX_022e,
            XXX_023,
            XXX_024,
            XXX_025,
            XXX_026,
            XXX_027,
            XXX_028,
            XXX_029,
            XXX_030,
            XXX_039,
            XXX_040,
            XXX_041,
            XXX_042,
            XXX_043,
            XXX_044,
            XXX_045,
            XXX_046,
            XXX_047,
            XXX_048,
            XXX_049,
            XXX_050,
            XXX_051,
            XXX_052,
            XXX_053,
            XXX_054,
            XXX_054e,
            XXX_055,
            XXX_055e,
            XXX_056,
            XXX_057,
            XXX_095,
            XXX_096,
            XXX_097,
            XXX_098,
            XXX_099,
            EX1_112,
            Mekka1,
            Mekka2,
            Mekka3,
            Mekka3e,
            Mekka4,
            Mekka4e,
            Mekka4t,
            PRO_001,
            PRO_001a,
            PRO_001at,
            PRO_001b,
            PRO_001c,
            FP1_001,
            FP1_002,
            FP1_002t,
            FP1_003,
            FP1_004,
            FP1_005,
            FP1_005e,
            FP1_006,
            FP1_007,
            FP1_007t,
            FP1_008,
            FP1_009,
            FP1_010,
            FP1_011,
            FP1_012,
            FP1_012t,
            FP1_013,
            FP1_014,
            FP1_014t,
            FP1_015,
            FP1_016,
            FP1_017,
            FP1_018,
            FP1_019,
            FP1_019t,
            FP1_020,
            FP1_020e,
            FP1_021,
            FP1_022,
            FP1_023,
            FP1_023e,
            FP1_024,
            FP1_025,
            FP1_026,
            FP1_027,
            FP1_028,
            FP1_028e,
            FP1_029,
            FP1_030,
            FP1_030e,
            FP1_031,
            NAX10_01,
            NAX10_01H,
            NAX10_02,
            NAX10_02H,
            NAX10_03,
            NAX10_03H,
            NAX11_01,
            NAX11_01H,
            NAX11_02,
            NAX11_02H,
            NAX11_03,
            NAX11_04,
            NAX11_04e,
            NAX12_01,
            NAX12_01H,
            NAX12_02,
            NAX12_02e,
            NAX12_02H,
            NAX12_03,
            NAX12_03e,
            NAX12_03H,
            NAX12_04,
            NAX12_04e,
            NAX13_01,
            NAX13_01H,
            NAX13_02,
            NAX13_02e,
            NAX13_03,
            NAX13_03e,
            NAX13_04H,
            NAX13_05H,
            NAX14_01,
            NAX14_01H,
            NAX14_02,
            NAX14_03,
            NAX14_04,
            NAX15_01,
            NAX15_01e,
            NAX15_01H,
            NAX15_01He,
            NAX15_02,
            NAX15_02H,
            NAX15_03n,
            NAX15_03t,
            NAX15_04,
            NAX15_04a,
            NAX15_04H,
            NAX15_05,
            NAX1h_01,
            NAX1h_03,
            NAX1h_04,
            NAX1_01,
            NAX1_03,
            NAX1_04,
            NAX1_05,
            NAX2_01,
            NAX2_01H,
            NAX2_03,
            NAX2_03H,
            NAX2_05,
            NAX2_05H,
            NAX3_01,
            NAX3_01H,
            NAX3_02,
            NAX3_02H,
            NAX3_03,
            NAX4_01,
            NAX4_01H,
            NAX4_03,
            NAX4_03H,
            NAX4_04,
            NAX4_04H,
            NAX4_05,
            NAX5_01,
            NAX5_01H,
            NAX5_02,
            NAX5_02H,
            NAX5_03,
            NAX6_01,
            NAX6_01H,
            NAX6_02,
            NAX6_02H,
            NAX6_03,
            NAX6_03t,
            NAX6_03te,
            NAX6_04,
            NAX7_01,
            NAX7_01H,
            NAX7_02,
            NAX7_03,
            NAX7_03H,
            NAX7_04,
            NAX7_04H,
            NAX7_05,
            NAX8_01,
            NAX8_01H,
            NAX8_02,
            NAX8_02H,
            NAX8_03,
            NAX8_03t,
            NAX8_04,
            NAX8_04t,
            NAX8_05,
            NAX8_05t,
            NAX9_01,
            NAX9_01H,
            NAX9_02,
            NAX9_02H,
            NAX9_03,
            NAX9_03H,
            NAX9_04,
            NAX9_04H,
            NAX9_05,
            NAX9_05H,
            NAX9_06,
            NAX9_07,
            NAX9_07e,
            NAXM_001,
            NAXM_002,
            GVG_001,
            GVG_002,
            GVG_003,
            GVG_004,
            GVG_005,
            GVG_006,
            GVG_007,
            GVG_008,
            GVG_009,
            GVG_010,
            GVG_010b,
            GVG_011,
            GVG_011a,
            GVG_012,
            GVG_013,
            GVG_014,
            GVG_014a,
            GVG_015,
            GVG_016,
            GVG_017,
            GVG_018,
            GVG_019,
            GVG_019e,
            GVG_020,
            GVG_021,
            GVG_021e,
            GVG_022,
            GVG_022a,
            GVG_022b,
            GVG_023,
            GVG_023a,
            GVG_024,
            GVG_025,
            GVG_026,
            GVG_027,
            GVG_027e,
            GVG_028,
            GVG_028t,
            GVG_029,
            GVG_030,
            GVG_030a,
            GVG_030ae,
            GVG_030b,
            GVG_030be,
            GVG_031,
            GVG_032,
            GVG_032a,
            GVG_032b,
            GVG_033,
            GVG_034,
            GVG_035,
            GVG_036,
            GVG_036e,
            GVG_037,
            GVG_038,
            GVG_039,
            GVG_040,
            GVG_041,
            GVG_041a,
            GVG_041b,
            GVG_041c,
            GVG_042,
            GVG_043,
            GVG_043e,
            GVG_044,
            GVG_045,
            GVG_045t,
            GVG_046,
            GVG_046e,
            GVG_047,
            GVG_048,
            GVG_048e,
            GVG_049,
            GVG_049e,
            GVG_050,
            GVG_051,
            GVG_052,
            GVG_053,
            GVG_054,
            GVG_055,
            GVG_055e,
            GVG_056,
            GVG_056t,
            GVG_057,
            GVG_057a,
            GVG_058,
            GVG_059,
            GVG_060,
            GVG_060e,
            GVG_061,
            GVG_062,
            GVG_063,
            GVG_063a,
            GVG_064,
            GVG_065,
            GVG_066,
            GVG_067,
            GVG_067a,
            GVG_068,
            GVG_068a,
            GVG_069,
            GVG_069a,
            GVG_070,
            GVG_071,
            GVG_072,
            GVG_073,
            GVG_074,
            GVG_075,
            GVG_076,
            GVG_076a,
            GVG_077,
            GVG_078,
            GVG_079,
            GVG_080,
            GVG_080t,
            GVG_081,
            GVG_082,
            GVG_083,
            GVG_084,
            GVG_085,
            GVG_086,
            GVG_086e,
            GVG_087,
            GVG_088,
            GVG_089,
            GVG_090,
            GVG_091,
            GVG_092,
            GVG_092t,
            GVG_093,
            GVG_094,
            GVG_095,
            GVG_096,
            GVG_097,
            GVG_098,
            GVG_099,
            GVG_100,
            GVG_100e,
            GVG_101,
            GVG_101e,
            GVG_102,
            GVG_102e,
            GVG_103,
            GVG_104,
            GVG_104a,
            GVG_105,
            GVG_106,
            GVG_106e,
            GVG_107,
            GVG_108,
            GVG_109,
            GVG_110,
            GVG_110t,
            GVG_111,
            GVG_111t,
            GVG_112,
            GVG_113,
            GVG_114,
            GVG_115,
            GVG_116,
            GVG_117,
            GVG_118,
            GVG_119,
            GVG_120,
            GVG_121,
            GVG_122,
            GVG_123,
            GVG_123e,
            PART_001,
            PART_001e,
            PART_002,
            PART_003,
            PART_004,
            PART_004e,
            PART_005,
            PART_006,
            PART_006a,
            PART_007,
            PART_007e,
            BRM_001,
            BRM_001e,
            BRM_002,
            BRM_003,
            BRM_003e,
            BRM_004,
            BRM_004e,
            BRM_004t,
            BRM_005,
            BRM_006,
            BRM_006t,
            BRM_007,
            BRM_008,
            BRM_009,
            BRM_010,
            BRM_010a,
            BRM_010b,
            BRM_010t,
            BRM_010t2,
            BRM_011,
            BRM_011t,
            BRM_012,
            BRM_012e,
            BRM_013,
            BRM_014,
            BRM_014e,
            BRM_015,
            BRM_016,
            BRM_017,
            BRM_018,
            BRM_018e,
            BRM_019,
            BRM_020,
            BRM_020e,
            BRM_022,
            BRM_022t,
            BRM_024,
            BRM_024e,
            BRM_025,
            BRM_026,
            BRM_027,
            BRM_027h,
            BRM_027p,
            BRM_027pH,
            BRM_028,
            BRM_028e,
            BRM_029,
            BRM_030,
            BRM_030t,
            BRM_031,
            BRM_033,
            BRM_033e,
            BRM_034,
            BRMA_01,
            BRMA01_1,
            BRMA01_1H,
            BRMA01_2,
            BRMA01_2H,
            BRMA01_3,
            BRMA01_4,
            BRMA01_4t,
            BRMA02_1,
            BRMA02_1H,
            BRMA02_2,
            BRMA02_2H,
            BRMA02_2t,
            BRMA03_1,
            BRMA03_1H,
            BRMA03_2,
            BRMA03_3,
            BRMA03_3H,
            BRMA04_1,
            BRMA04_1H,
            BRMA04_2,
            BRMA04_3,
            BRMA04_3H,
            BRMA04_4,
            BRMA04_4H,
            BRMA05_1,
            BRMA05_1H,
            BRMA05_2,
            BRMA05_2H,
            BRMA05_3,
            BRMA05_3e,
            BRMA05_3H,
            BRMA05_3He,
            BRMA06_1,
            BRMA06_1H,
            BRMA06_2,
            BRMA06_2H,
            BRMA06_3,
            BRMA06_3H,
            BRMA06_4,
            BRMA06_4H,
            BRMA07_1,
            BRMA07_1H,
            BRMA07_2,
            BRMA07_2H,
            BRMA07_3,
            BRMA08_1,
            BRMA08_1H,
            BRMA08_2,
            BRMA08_2H,
            BRMA08_3,
            BRMA09_1,
            BRMA09_1H,
            BRMA09_2,
            BRMA09_2H,
            BRMA09_2Ht,
            BRMA09_2t,
            BRMA09_3,
            BRMA09_3H,
            BRMA09_3Ht,
            BRMA09_3t,
            BRMA09_4,
            BRMA09_4H,
            BRMA09_4Ht,
            BRMA09_4t,
            BRMA09_5,
            BRMA09_5H,
            BRMA09_5Ht,
            BRMA09_5t,
            BRMA09_6,
            BRMA10_1,
            BRMA10_1H,
            BRMA10_3,
            BRMA10_3e,
            BRMA10_3H,
            BRMA10_4,
            BRMA10_4H,
            BRMA10_5,
            BRMA10_5H,
            BRMA10_6,
            BRMA10_6e,
            BRMA11_1,
            BRMA11_1H,
            BRMA11_2,
            BRMA11_2H,
            BRMA11_3,
            BRMA12_1,
            BRMA12_10,
            BRMA12_1H,
            BRMA12_2,
            BRMA12_2H,
            BRMA12_3,
            BRMA12_3H,
            BRMA12_4,
            BRMA12_4H,
            BRMA12_5,
            BRMA12_5H,
            BRMA12_6,
            BRMA12_6H,
            BRMA12_7,
            BRMA12_7H,
            BRMA12_8,
            BRMA12_8t,
            BRMA12_9,
            BRMA13_1,
            BRMA13_1H,
            BRMA13_2,
            BRMA13_2H,
            BRMA13_3,
            BRMA13_3H,
            BRMA13_4,
            BRMA13_4H,
            BRMA13_5,
            BRMA13_6,
            BRMA13_7,
            BRMA13_8,
            BRMA14_1,
            BRMA14_10,
            BRMA14_10H,
            BRMA14_11,
            BRMA14_12,
            BRMA14_1H,
            BRMA14_2,
            BRMA14_2H,
            BRMA14_3,
            BRMA14_4,
            BRMA14_4H,
            BRMA14_5,
            BRMA14_5H,
            BRMA14_6,
            BRMA14_6H,
            BRMA14_7,
            BRMA14_7H,
            BRMA14_8,
            BRMA14_8H,
            BRMA14_9,
            BRMA14_9H,
            BRMA15_1,
            BRMA15_1H,
            BRMA15_2,
            BRMA15_2H,
            BRMA15_2He,
            BRMA15_3,
            BRMA15_4,
            BRMA16_1,
            BRMA16_1H,
            BRMA16_2,
            BRMA16_2H,
            BRMA16_3,
            BRMA16_3e,
            BRMA16_4,
            BRMA16_5,
            BRMA16_5e,
            BRMA17_2,
            BRMA17_2H,
            BRMA17_3,
            BRMA17_3H,
            BRMA17_4,
            BRMA17_5,
            BRMA17_5H,
            BRMA17_6,
            BRMA17_6H,
            BRMA17_7,
            BRMA17_8,
            BRMA17_8H,
            BRMA17_9,
            BRMC_01,
            BRMC_02,
            PlaceholderCard,
        }

        public cardIDEnum cardIdstringToEnum(string s)
        {
            CardDB.cardIDEnum CardEnum;
            if (Enum.TryParse<cardIDEnum>(s, false, out CardEnum)) return CardEnum;
            else return CardDB.cardIDEnum.None;
        }

        public enum cardName
        {
            unknown,
            lesserheal,
            goldshirefootman,
            holynova,
            mindcontrol,
            holysmite,
            mindvision,
            powerwordshield,
            claw,
            healingtouch,
            moonfire,
            markofthewild,
            savageroar,
            swipe,
            wildgrowth,
            excessmana,
            shapeshift,
            polymorph,
            arcaneintellect,
            frostbolt,
            arcaneexplosion,
            frostnova,
            mirrorimage,
            fireball,
            flamestrike,
            waterelemental,
            fireblast,
            frostshock,
            windfury,
            ancestralhealing,
            fireelemental,
            rockbiterweapon,
            bloodlust,
            totemiccall,
            searingtotem,
            stoneclawtotem,
            wrathofairtotem,
            lifetap,
            shadowbolt,
            drainlife,
            hellfire,
            corruption,
            dreadinfernal,
            voidwalker,
            backstab,
            deadlypoison,
            sinisterstrike,
            assassinate,
            sprint,
            assassinsblade,
            wickedknife,
            daggermastery,
            huntersmark,
            blessingofmight,
            guardianofkings,
            holylight,
            lightsjustice,
            blessingofkings,
            consecration,
            hammerofwrath,
            truesilverchampion,
            reinforce,
            silverhandrecruit,
            armorup,
            charge,
            heroicstrike,
            fierywaraxe,
            execute,
            arcanitereaper,
            cleave,
            magmarager,
            oasissnapjaw,
            rivercrocolisk,
            frostwolfgrunt,
            raidleader,
            wolfrider,
            ironfurgrizzly,
            silverbackpatriarch,
            stormwindknight,
            ironforgerifleman,
            koboldgeomancer,
            gnomishinventor,
            stormpikecommando,
            archmage,
            lordofthearena,
            murlocraider,
            stonetuskboar,
            bloodfenraptor,
            bluegillwarrior,
            senjinshieldmasta,
            chillwindyeti,
            wargolem,
            bootybaybodyguard,
            elvenarcher,
            razorfenhunter,
            ogremagi,
            boulderfistogre,
            corehound,
            recklessrocketeer,
            stormwindchampion,
            frostwolfwarlord,
            ironbarkprotector,
            shadowwordpain,
            northshirecleric,
            divinespirit,
            starvingbuzzard,
            boar,
            sheep,
            steadyshot,
            darkscalehealer,
            houndmaster,
            timberwolf,
            tundrarhino,
            multishot,
            tracking,
            arcaneshot,
            mindblast,
            voodoodoctor,
            noviceengineer,
            shatteredsuncleric,
            dragonlingmechanic,
            mechanicaldragonling,
            acidicswampooze,
            warsongcommander,
            fanofknives,
            innervate,
            starfire,
            totemicmight,
            hex,
            arcanemissiles,
            shiv,
            mortalcoil,
            succubus,
            soulfire,
            humility,
            handofprotection,
            gurubashiberserker,
            whirlwind,
            murloctidehunter,
            murlocscout,
            grimscaleoracle,
            killcommand,
            flametonguetotem,
            sap,
            dalaranmage,
            windspeaker,
            nightblade,
            shieldblock,
            shadowworddeath,
            avatarofthecoin,
            thecoin,
            noooooooooooo,
            garroshhellscream,
            thrall,
            valeerasanguinar,
            utherlightbringer,
            rexxar,
            malfurionstormrage,
            guldan,
            jainaproudmoore,
            anduinwrynn,
            frog,
            sacrificialpact,
            vanish,
            healingtotem,
            korkronelite,
            animalcompanion,
            misha,
            leokk,
            huffer,
            skeleton,
            fencreeper,
            innerfire,
            blizzard,
            icelance,
            ancestralspirit,
            farsight,
            bloodimp,
            coldblood,
            rampage,
            earthenringfarseer,
            southseadeckhand,
            silverhandknight,
            squire,
            ravenholdtassassin,
            youngdragonhawk,
            injuredblademaster,
            abusivesergeant,
            ironbeakowl,
            spitefulsmith,
            venturecomercenary,
            wisp,
            bladeflurry,
            laughingsister,
            yseraawakens,
            emeralddrake,
            dream,
            nightmare,
            gladiatorslongbow,
            whelp,
            lightwarden,
            theblackknight,
            youngpriestess,
            biggamehunter,
            alarmobot,
            acolyteofpain,
            argentsquire,
            angrychicken,
            worgeninfiltrator,
            bloodmagethalnos,
            kingmukla,
            bananas,
            sylvanaswindrunner,
            junglepanther,
            scarletcrusader,
            thrallmarfarseer,
            silvermoonguardian,
            stranglethorntiger,
            lepergnome,
            sunwalker,
            windfuryharpy,
            twilightdrake,
            questingadventurer,
            ancientwatcher,
            darkirondwarf,
            spellbreaker,
            youthfulbrewmaster,
            coldlightoracle,
            manaaddict,
            ancientbrewmaster,
            sunfuryprotector,
            crazedalchemist,
            argentcommander,
            pintsizedsummoner,
            secretkeeper,
            madbomber,
            tinkmasteroverspark,
            mindcontroltech,
            arcanegolem,
            cabalshadowpriest,
            defenderofargus,
            gadgetzanauctioneer,
            loothoarder,
            abomination,
            lorewalkercho,
            demolisher,
            coldlightseer,
            mountaingiant,
            cairnebloodhoof,
            bainebloodhoof,
            leeroyjenkins,
            eviscerate,
            betrayal,
            conceal,
            noblesacrifice,
            defender,
            defiasringleader,
            defiasbandit,
            eyeforaneye,
            perditionsblade,
            si7agent,
            redemption,
            headcrack,
            shadowstep,
            preparation,
            wrath,
            markofnature,
            souloftheforest,
            treant,
            powerofthewild,
            summonapanther,
            leaderofthepack,
            panther,
            naturalize,
            direwolfalpha,
            nourish,
            druidoftheclaw,
            catform,
            bearform,
            keeperofthegrove,
            dispel,
            emperorcobra,
            ancientofwar,
            rooted,
            uproot,
            lightningbolt,
            lavaburst,
            dustdevil,
            earthshock,
            stormforgedaxe,
            feralspirit,
            barongeddon,
            earthelemental,
            forkedlightning,
            unboundelemental,
            lightningstorm,
            etherealarcanist,
            coneofcold,
            pyroblast,
            frostelemental,
            azuredrake,
            counterspell,
            icebarrier,
            mirrorentity,
            iceblock,
            ragnarosthefirelord,
            felguard,
            shadowflame,
            voidterror,
            siphonsoul,
            doomguard,
            twistingnether,
            pitlord,
            summoningportal,
            poweroverwhelming,
            sensedemons,
            worthlessimp,
            flameimp,
            baneofdoom,
            lordjaraxxus,
            bloodfury,
            silence,
            shadowmadness,
            lightspawn,
            thoughtsteal,
            lightwell,
            mindgames,
            shadowofnothing,
            divinefavor,
            prophetvelen,
            layonhands,
            blessedchampion,
            argentprotector,
            blessingofwisdom,
            holywrath,
            swordofjustice,
            repentance,
            aldorpeacekeeper,
            tirionfordring,
            ashbringer,
            avengingwrath,
            taurenwarrior,
            slam,
            battlerage,
            amaniberserker,
            mogushanwarden,
            arathiweaponsmith,
            battleaxe,
            armorsmith,
            shieldbearer,
            brawl,
            mortalstrike,
            upgrade,
            heavyaxe,
            shieldslam,
            gorehowl,
            ragingworgen,
            grommashhellscream,
            murlocwarleader,
            murloctidecaller,
            patientassassin,
            scavenginghyena,
            misdirection,
            savannahhighmane,
            hyena,
            eaglehornbow,
            explosiveshot,
            unleashthehounds,
            hound,
            kingkrush,
            flare,
            bestialwrath,
            snaketrap,
            snake,
            harvestgolem,
            natpagle,
            harrisonjones,
            archmageantonidas,
            nozdormu,
            alexstrasza,
            onyxia,
            malygos,
            facelessmanipulator,
            doomhammer,
            bite,
            forceofnature,
            ysera,
            cenarius,
            demigodsfavor,
            shandoslesson,
            manatidetotem,
            thebeast,
            savagery,
            priestessofelune,
            ancientmage,
            seagiant,
            bloodknight,
            auchenaisoulpriest,
            vaporize,
            cultmaster,
            demonfire,
            impmaster,
            imp,
            crueltaskmaster,
            frothingberserker,
            innerrage,
            sorcerersapprentice,
            snipe,
            explosivetrap,
            freezingtrap,
            kirintormage,
            edwinvancleef,
            illidanstormrage,
            flameofazzinoth,
            manawraith,
            deadlyshot,
            equality,
            moltengiant,
            circleofhealing,
            templeenforcer,
            holyfire,
            shadowform,
            mindspike,
            mindshatter,
            massdispel,
            finkleeinhorn,
            spiritwolf,
            squirrel,
            devilsaur,
            inferno,
            infernal,
            kidnapper,
            starfall,
            ancientoflore,
            ancientteachings,
            ancientsecrets,
            alakirthewindlord,
            manawyrm,
            masterofdisguise,
            hungrycrab,
            bloodsailraider,
            knifejuggler,
            wildpyromancer,
            doomsayer,
            dreadcorsair,
            faeriedragon,
            captaingreenskin,
            bloodsailcorsair,
            violetteacher,
            violetapprentice,
            southseacaptain,
            millhousemanastorm,
            deathwing,
            commandingshout,
            masterswordsmith,
            gruul,
            hogger,
            gnoll,
            stampedingkodo,
            damagedgolem,
            flesheatingghoul,
            spellbender,
            jasonchayes,
            ericdodds,
            bobfitch,
            stevengabriel,
            kyleharrison,
            dereksakamoto,
            zwick,
            benbrode,
            benthompson,
            michaelschweitzer,
            jaybaxter,
            rachelledavis,
            brianschwab,
            yongwoo,
            andybrock,
            hamiltonchu,
            robpardo,
            oldmurkeye,
            captainsparrot,
            riverpawgnoll,
            hoggersmash,
            massivegnoll,
            barreltoss,
            barrel,
            stomp,
            hiddengnome,
            muklasbigbrother,
            willofmukla,
            hemetnesingwary,
            crazedhunter,
            shotgunblast,
            flamesofazzinoth,
            nagamyrmidon,
            warglaiveofazzinoth,
            flameburst,
            dualwarglaives,
            pandarenscout,
            shadopanmonk,
            legacyoftheemperor,
            brewmaster,
            transcendence,
            crazymonkey,
            damage1,
            damage5,
            restore1,
            restore5,
            destroy,
            breakweapon,
            enableforattack,
            freeze,
            enchant,
            silencedebug,
            summonarandomsecret,
            bounce,
            discard,
            mill10,
            crash,
            snakeball,
            draw3cards,
            destroyallminions,
            molasses,
            damageallbut1,
            restoreallhealth,
            freecards,
            destroyallheroes,
            damagereflector,
            donothing,
            enableemotes,
            servercrash,
            revealhand,
            opponentconcede,
            opponentdisconnect,
            becomehogger,
            destroyheropower,
            handtodeck,
            mill30,
            handswapperminion,
            stealcard,
            forceaitouseheropower,
            destroydeck,
            durability,
            destroyallmana,
            destroyamanacrystal,
            makeimmune,
            grantmegawindfury,
            armor,
            weaponbuff,
            stats,
            silencedestroy,
            destroysecrets,
            aibuddyallcharge,
            aibuddydamageownhero5,
            aibuddydestroyminions,
            aibuddynodeckhand,
            aihelperbuddy,
            gelbinmekkatorque,
            homingchicken,
            repairbot,
            emboldener3000,
            poultryizer,
            chicken,
            elitetaurenchieftain,
            iammurloc,
            murloc,
            roguesdoit,
            powerofthehorde,
            zombiechow,
            hauntedcreeper,
            spectralspider,
            echoingooze,
            madscientist,
            shadeofnaxxramas,
            deathcharger,
            nerubianegg,
            nerubian,
            spectralknight,
            deathlord,
            maexxna,
            webspinner,
            sludgebelcher,
            slime,
            kelthuzad,
            stalagg,
            thaddius,
            feugen,
            wailingsoul,
            nerubarweblord,
            duplicate,
            poisonseeds,
            avenge,
            deathsbite,
            voidcaller,
            darkcultist,
            unstableghoul,
            reincarnate,
            anubarambusher,
            stoneskingargoyle,
            undertaker,
            dancingswords,
            loatheb,
            baronrivendare,
            patchwerk,
            hook,
            hatefulstrike,
            grobbulus,
            poisoncloud,
            falloutslime,
            mutatinginjection,
            gluth,
            decimate,
            jaws,
            enrage,
            polarityshift,
            supercharge,
            sapphiron,
            frostbreath,
            frozenchampion,
            purecold,
            frostblast,
            guardianoficecrown,
            chains,
            mrbigglesworth,
            anubrekhan,
            skitter,
            locustswarm,
            grandwidowfaerlina,
            rainoffire,
            worshipper,
            webwrap,
            necroticpoison,
            noththeplaguebringer,
            raisedead,
            plague,
            heigantheunclean,
            eruption,
            mindpocalypse,
            necroticaura,
            deathbloom,
            spore,
            sporeburst,
            instructorrazuvious,
            understudy,
            unbalancingstrike,
            massiveruneblade,
            mindcontrolcrystal,
            gothiktheharvester,
            harvest,
            unrelentingtrainee,
            spectraltrainee,
            unrelentingwarrior,
            spectralwarrior,
            unrelentingrider,
            spectralrider,
            ladyblaumeux,
            thanekorthazz,
            sirzeliek,
            runeblade,
            unholyshadow,
            markofthehorsemen,
            necroknight,
            skeletalsmith,
            flamecannon,
            snowchugger,
            unstableportal,
            goblinblastmage,
            echoofmedivh,
            mechwarper,
            flameleviathan,
            lightbomb,
            shadowbomber,
            velenschosen,
            shrinkmeister,
            lightofthenaaru,
            cogmaster,
            voljin,
            darkbomb,
            felreaver,
            callpet,
            mistressofpain,
            demonheart,
            felcannon,
            malganis,
            tinkerssharpswordoil,
            goblinautobarber,
            cogmasterswrench,
            oneeyedcheat,
            feigndeath,
            ironsensei,
            tradeprincegallywix,
            gallywixscoin,
            ancestorscall,
            anodizedrobocub,
            attackmode,
            tankmode,
            recycle,
            grovetender,
            giftofmana,
            giftofcards,
            treeoflife,
            mechbearcat,
            malorne,
            powermace,
            whirlingzapomatic,
            crackle,
            vitalitytotem,
            siltfinspiritwalker,
            darkwispers,
            neptulon,
            glaivezooka,
            spidertank,
            implosion,
            kingofbeasts,
            sabotage,
            metaltoothleaper,
            gahzrilla,
            bouncingblade,
            warbot,
            crush,
            shieldmaiden,
            ogrewarmaul,
            screwjankclunker,
            ironjuggernaut,
            burrowingmine,
            sealoflight,
            shieldedminibot,
            coghammer,
            quartermaster,
            musterforbattle,
            cobaltguardian,
            bolvarfordragon,
            puddlestomper,
            ogrebrute,
            dunemaulshaman,
            stonesplintertrogg,
            burlyrockjawtrogg,
            antiquehealbot,
            saltydog,
            losttallstrider,
            shadowboxer,
            cobrashot,
            kezanmystic,
            shipscannon,
            explosivesheep,
            animagolem,
            mechanicalyeti,
            forcetankmax,
            druidofthefang,
            gilblinstalker,
            clockworkgnome,
            upgradedrepairbot,
            flyingmachine,
            annoyotron,
            siegeengine,
            steamwheedlesniper,
            ogreninja,
            illuminator,
            madderbomber,
            arcanenullifierx21,
            gnomishexperimenter,
            targetdummy,
            jeeves,
            goblinsapper,
            pilotedshredder,
            lilexorcist,
            gnomereganinfantry,
            bomblobber,
            floatingwatcher,
            scarletpurifier,
            tinkertowntechnician,
            micromachine,
            hobgoblin,
            pilotedskygolem,
            junkbot,
            enhanceomechano,
            recombobulator,
            minimage,
            drboom,
            boombot,
            mimironshead,
            v07tr0n,
            mogortheogre,
            foereaper4000,
            sneedsoldshredder,
            toshley,
            mekgineerthermaplugg,
            gazlowe,
            troggzortheearthinator,
            blingtron3000,
            clockworkgiant,
            weespellstopper,
            sootspewer,
            armorplating,
            timerewinder,
            rustyhorn,
            finickycloakfield,
            emergencycoolant,
            reversingswitch,
            whirlingblades,
            aberration,
            activate,
            activatearcanotron,
            activateelectron,
            activatemagmatron,
            activatetoxitron,
            arcanotron,
            atramedes,
            axeflinger,
            blackwhelp,
            blackwing,
            blackwingcorruptor,
            blackwingtechnician,
            blindwithrage,
            boneconstruct,
            boneminions,
            broodaffliction,
            broodafflictionblack,
            broodafflictionblue,
            broodafflictionbronze,
            broodafflictiongreen,
            broodafflictionred,
            burningadrenaline,
            chromaggus,
            chromaticdragonkin,
            chromaticdrake,
            chromaticmutation,
            chromaticprototype,
            corendirebrew,
            corerager,
            corruptedegg,
            darkironbouncer,
            darkironskulker,
            darkironspectator,
            demonwrath,
            dieinsect,
            dieinsects,
            dismount,
            draconicpower,
            dragonblood,
            dragonconsort,
            dragonegg,
            dragonkin,
            dragonkinsorcerer,
            dragonsbreath,
            dragonsmight,
            dragonteeth,
            drakkisathscommand,
            drakonidcrusher,
            druidoftheflame,
            echolocate,
            electron,
            emperorthaurissan,
            essenceofthered,
            firecatform,
            fireguarddestroyer,
            firehawkform,
            firesworn,
            flameheart,
            flamewaker,
            flamewakeracolyte,
            gangup,
            garr,
            generaldrakkisath,
            getem,
            grimpatron,
            guzzler,
            gyth,
            highjusticegrimstone,
            highlordomokk,
            hungrydragon,
            ignitemana,
            ihearyou,
            imperialfavor,
            impgangboss,
            incubation,
            intensegaze,
            jeeringcrowd,
            largetalons,
            lava,
            lavashock,
            livingbomb,
            livinglava,
            lordvictornefarius,
            magmapulse,
            magmatron,
            magmaw,
            majordomoexecutus,
            maloriak,
            melt,
            mesmash,
            moirabronzebeard,
            mutation,
            nefarian,
            nefarianstrikes,
            oldhorde,
            oldhordeorc,
            omnotrondefensesystem,
            onfire,
            onyxiclaw,
            openthegates,
            pileon,
            potionofmight,
            powerofthefirelord,
            powerrager,
            quickshot,
            razorgoresclaws,
            razorgoretheuntamed,
            recharge,
            releasetheaberrations,
            rendblackhand,
            resurrect,
            revenge,
            reverberatinggong,
            rockout,
            solemnvigil,
            sonicbreath,
            sonoftheflame,
            tailswipe,
            thealchemist,
            themajordomo,
            therookery,
            thetruewarchief,
            timeforsmash,
            toxitron,
            trueform,
            twilightendurance,
            twilightwhelp,
            unchained,
            vaelastraszthecorrupt,
            volcanicdrake,
            volcaniclumberer,
            whirlingash,
            wildmagic,
            placeholdercard,
        }

        public cardName cardNamestringToEnum(string s)
        {
            CardDB.cardName NameEnum;
            if (Enum.TryParse<cardName>(s, false, out NameEnum)) return NameEnum;
            else return CardDB.cardName.unknown;
        }

        public enum ErrorType2
        {
            NONE,//=0
            REQ_MINION_TARGET,//=1
            REQ_FRIENDLY_TARGET,//=2
            REQ_ENEMY_TARGET,//=3
            REQ_DAMAGED_TARGET,//=4
            REQ_ENCHANTED_TARGET,
            REQ_FROZEN_TARGET,
            REQ_CHARGE_TARGET,
            REQ_TARGET_MAX_ATTACK,//=8
            REQ_NONSELF_TARGET,//=9
            REQ_TARGET_WITH_RACE,//=10
            REQ_TARGET_TO_PLAY,//=11 
            REQ_NUM_MINION_SLOTS,//=12 
            REQ_WEAPON_EQUIPPED,//=13
            REQ_ENOUGH_MANA,//=14
            REQ_YOUR_TURN,
            REQ_NONSTEALTH_ENEMY_TARGET,
            REQ_HERO_TARGET,//17
            REQ_SECRET_CAP,
            REQ_MINION_CAP_IF_TARGET_AVAILABLE,//19
            REQ_MINION_CAP,
            REQ_TARGET_ATTACKED_THIS_TURN,
            REQ_TARGET_IF_AVAILABLE,//=22
            REQ_MINIMUM_ENEMY_MINIONS,//=23 /like spalen :D
            REQ_TARGET_FOR_COMBO,//=24
            REQ_NOT_EXHAUSTED_ACTIVATE,
            REQ_UNIQUE_SECRET,
            REQ_TARGET_TAUNTER,
            REQ_CAN_BE_ATTACKED,
            REQ_ACTION_PWR_IS_MASTER_PWR,
            REQ_TARGET_MAGNET,
            REQ_ATTACK_GREATER_THAN_0,
            REQ_ATTACKER_NOT_FROZEN,
            REQ_HERO_OR_MINION_TARGET,
            REQ_CAN_BE_TARGETED_BY_SPELLS,
            REQ_SUBCARD_IS_PLAYABLE,
            REQ_TARGET_FOR_NO_COMBO,
            REQ_NOT_MINION_JUST_PLAYED,
            REQ_NOT_EXHAUSTED_HERO_POWER,
            REQ_CAN_BE_TARGETED_BY_OPPONENTS,
            REQ_ATTACKER_CAN_ATTACK,
            REQ_TARGET_MIN_ATTACK,//=41
            REQ_CAN_BE_TARGETED_BY_HERO_POWERS,
            REQ_ENEMY_TARGET_NOT_IMMUNE,
            REQ_ENTIRE_ENTOURAGE_NOT_IN_PLAY,//44 (totemic call)
            REQ_MINIMUM_TOTAL_MINIONS,//45 (scharmuetzel)
            REQ_MUST_TARGET_TAUNTER,//=46
            REQ_UNDAMAGED_TARGET,//=47
            REQ_CAN_BE_TARGETED_BY_BATTLECRIES,
            REQ_STEADY_SHOT,//49
            REQ_MINION_OR_ENEMY_HERO,//50
            REQ_TARGET_IF_AVAILABLE_AND_DRAGON_IN_HAND,//51
            REQ_LEGENDARY_TARGET,//52
            REQ_DRAG_TO_PLAY,
            REQ_FRIENDLY_MINION_DIED_THIS_TURN
        }

        public class Card
        {
            //public string CardID = "";
            public cardName name = cardName.unknown;
            public int race = 0;
            public int rarity = 0;
            public int cost = 0;
            public int Class = 0;
            public cardtype type = CardDB.cardtype.NONE;
            //public string description = "";

            public int Attack = 0;
            public int Health = 0;
            public int Durability = 0;//for weapons
            public bool target = false;
            //public string targettext = "";
            public bool tank = false;
            public bool Silence = false;
            public bool choice = false;
            public bool windfury = false;
            public bool poisionous = false;
            public bool deathrattle = false;
            public bool battlecry = false;
            public bool oneTurnEffect = false;
            public bool Enrage = false;
            public bool Aura = false;
            public bool Elite = false;
            public bool Combo = false;
            public bool Recall = false;
            public int recallValue = 0;
            public bool immuneWhileAttacking = false;
            public bool immuneToSpellpowerg = false;
            public bool Stealth = false;
            public bool Freeze = false;
            public bool AdjacentBuff = false;
            public bool Shield = false;
            public bool Charge = false;
            public bool Secret = false;
            public bool Morph = false;
            public bool Spellpower = false;
            public bool GrantCharge = false;
            public bool HealTarget = false;
            //playRequirements, reqID= siehe PlayErrors->ErrorType
            public int needEmptyPlacesForPlaying = 0;
            public int needWithMinAttackValueOf = 0;
            public int needWithMaxAttackValueOf = 0;
            public int needRaceForPlaying = 0;
            public int needMinNumberOfEnemy = 0;
            public int needMinTotalMinions = 0;
            public int needMinionsCapIfAvailable = 0;


            //additional data
            public bool isToken = false;
            public int isCarddraw = 0;
            public bool damagesTarget = false;
            public bool damagesTargetWithSpecial = false;
            public int targetPriority = 0;
            public bool isSpecialMinion = false;

            public int spellpowervalue = 0;
            public cardIDEnum cardIDenum = cardIDEnum.None;
            public List<ErrorType2> playrequires;

            public SimTemplate sim_card;

            public Card()
            {
                playrequires = new List<ErrorType2>();
            }

            public Card(Card c)
            {
                //this.entityID = c.entityID;
                this.rarity = c.rarity;
                this.AdjacentBuff = c.AdjacentBuff;
                this.Attack = c.Attack;
                this.Aura = c.Aura;
                this.battlecry = c.battlecry;
                //this.CardID = c.CardID;
                this.Charge = c.Charge;
                this.choice = c.choice;
                this.Combo = c.Combo;
                this.cost = c.cost;
                this.deathrattle = c.deathrattle;
                //this.description = c.description;
                this.Durability = c.Durability;
                this.Elite = c.Elite;
                this.Enrage = c.Enrage;
                this.Freeze = c.Freeze;
                this.GrantCharge = c.GrantCharge;
                this.HealTarget = c.HealTarget;
                this.Health = c.Health;
                this.immuneToSpellpowerg = c.immuneToSpellpowerg;
                this.immuneWhileAttacking = c.immuneWhileAttacking;
                this.Morph = c.Morph;
                this.name = c.name;
                this.needEmptyPlacesForPlaying = c.needEmptyPlacesForPlaying;
                this.needMinionsCapIfAvailable = c.needMinionsCapIfAvailable;
                this.needMinNumberOfEnemy = c.needMinNumberOfEnemy;
                this.needMinTotalMinions = c.needMinTotalMinions;
                this.needRaceForPlaying = c.needRaceForPlaying;
                this.needWithMaxAttackValueOf = c.needWithMaxAttackValueOf;
                this.needWithMinAttackValueOf = c.needWithMinAttackValueOf;
                this.oneTurnEffect = c.oneTurnEffect;
                this.playrequires = new List<ErrorType2>(c.playrequires);
                this.poisionous = c.poisionous;
                this.race = c.race;
                this.Recall = c.Recall;
                this.recallValue = c.recallValue;
                this.Secret = c.Secret;
                this.Shield = c.Shield;
                this.Silence = c.Silence;
                this.Spellpower = c.Spellpower;
                this.spellpowervalue = c.spellpowervalue;
                this.Stealth = c.Stealth;
                this.tank = c.tank;
                this.target = c.target;
                //this.targettext = c.targettext;
                this.type = c.type;
                this.windfury = c.windfury;
                this.cardIDenum = c.cardIDenum;
                this.sim_card = c.sim_card;
                this.isToken = c.isToken;
            }

            public Card(HRSim.CardDB.Card c)
            {
                this.playrequires = new List<ErrorType2>();
                //this.entityID = c.entityID;
                this.rarity = c.rarity;
                this.AdjacentBuff = c.AdjacentBuff;
                this.Attack = c.Attack;
                this.Aura = c.Aura;
                this.battlecry = c.battlecry;
                //this.CardID = c.CardID;
                this.Charge = c.Charge;
                this.choice = c.choice;
                this.Combo = c.Combo;
                this.cost = c.cost;
                this.deathrattle = c.deathrattle;
                //this.description = c.description;
                this.Durability = c.Durability;
                this.Elite = c.Elite;
                this.Enrage = c.Enrage;
                this.Freeze = c.Freeze;
                this.GrantCharge = c.GrantCharge;
                this.HealTarget = c.HealTarget;
                this.Health = c.Health;
                this.immuneToSpellpowerg = c.immuneToSpellpowerg;
                this.immuneWhileAttacking = c.immuneWhileAttacking;
                this.Morph = c.Morph;
                this.name = (cardName)Enum.Parse(typeof(cardName), c.name.ToString());
                this.needEmptyPlacesForPlaying = c.needEmptyPlacesForPlaying;
                this.needMinionsCapIfAvailable = c.needMinionsCapIfAvailable;
                this.needMinNumberOfEnemy = c.needMinNumberOfEnemy;
                this.needMinTotalMinions = c.needMinTotalMinions;
                this.needRaceForPlaying = c.needRaceForPlaying;
                this.needWithMaxAttackValueOf = c.needWithMaxAttackValueOf;
                this.needWithMinAttackValueOf = c.needWithMinAttackValueOf;
                this.oneTurnEffect = c.oneTurnEffect;
                foreach (HRSim.CardDB.ErrorType2 e in c.playrequires) {
                    ErrorType2 mError = (ErrorType2)Enum.Parse(typeof(ErrorType2), e.ToString());
                    this.playrequires.Add(mError);
                }
                this.poisionous = c.poisionous;
                this.race = c.race;
                this.Recall = c.Recall;
                this.recallValue = c.recallValue;
                this.Secret = c.Secret;
                this.Shield = c.Shield;
                this.Silence = c.Silence;
                this.Spellpower = c.Spellpower;
                this.spellpowervalue = c.spellpowervalue;
                this.Stealth = c.Stealth;
                this.tank = c.tank;
                this.target = c.target;
                //this.targettext = c.targettext;
                this.type = (cardtype)Enum.Parse(typeof(cardtype), c.type.ToString());
                this.windfury = c.windfury;
                this.cardIDenum = (cardIDEnum)Enum.Parse(typeof(cardIDEnum), c.cardIDenum.ToString());
                this.sim_card = instance.getSimCard(this.cardIDenum);
                this.isToken = c.isToken;
            }

            public bool isRequirementInList(CardDB.ErrorType2 et)
            {
                return this.playrequires.Contains(et);
            }

            public List<Minion> getTargetsForCard(Playfield p, bool isLethalCheck)
            {
                //if wereTargets=true and 0 targets at end -> then can not play this card
                List<Minion> retval = new List<Minion>();
                if (this.type == CardDB.cardtype.MOB && p.ownMinions.Count >= 7) return retval; // cant play mob, if we have allready 7 mininos
                if (this.Secret && (p.ownSecretsIDList.Contains(this.cardIDenum) || p.ownSecretsIDList.Count >= 5)) return retval;
                if (p.mana < this.getManaCost(p, 1)) return retval;
                List<Minion> targets = new List<Minion>();

                bool targetAll = false;
                bool targetAllEnemy = false;
                bool targetAllFriendly = false;
                bool targetEnemyHero = false;
                bool targetOwnHero = false;
                bool targetOnlyMinion = false;
                bool extraParam = false;
                bool wereTargets = false;
                bool REQ_UNDAMAGED_TARGET = false;
                bool REQ_TARGET_WITH_RACE = false;
                bool REQ_TARGET_MIN_ATTACK = false;
                bool REQ_TARGET_MAX_ATTACK = false;
                bool REQ_MUST_TARGET_TAUNTER = false;
                bool REQ_MINION_OR_ENEMY_HERO = false;
                bool REQ_HERO_TARGET = false;
                bool REQ_DAMAGED_TARGET = false;
                bool REQ_LEGENDARY_TARGET = false;                

                foreach (CardDB.ErrorType2 PlayReq in this.playrequires)
                {
                    switch (PlayReq)
                    {
                        case ErrorType2.REQ_TARGET_TO_PLAY:
                            targetAll = true;
                            continue;
                        case ErrorType2.REQ_MINION_TARGET:
                            targetOnlyMinion = true;
                            continue;
                        case ErrorType2.REQ_TARGET_IF_AVAILABLE:
                            targetAll = true;
                            continue;
                        case ErrorType2.REQ_FRIENDLY_TARGET:
                            targetAllFriendly = true;
                            targetOwnHero = true;
                            continue;
                        case ErrorType2.REQ_NUM_MINION_SLOTS:
                            if (p.ownMinions.Count > 7 - this.needEmptyPlacesForPlaying) return retval;
                            continue;
                        case ErrorType2.REQ_ENEMY_TARGET:
                            targetAllEnemy = true;
                            targetEnemyHero = true;
                            continue;
                        case ErrorType2.REQ_HERO_TARGET:
                            REQ_HERO_TARGET = true;
                            extraParam = true;
                            continue;
                        case ErrorType2.REQ_MINIMUM_ENEMY_MINIONS:
                            if (p.enemyMinions.Count < this.needMinNumberOfEnemy) return retval;
                            continue;
                        case ErrorType2.REQ_NONSELF_TARGET:
                            targetAll = true;
                            continue;
                        case ErrorType2.REQ_TARGET_WITH_RACE:
                            REQ_TARGET_WITH_RACE = true;
                            extraParam = true;
                            continue;
                        case ErrorType2.REQ_DAMAGED_TARGET:
                            REQ_DAMAGED_TARGET = true;
                            extraParam = true;
                            continue;
                        case ErrorType2.REQ_TARGET_MAX_ATTACK:
                            REQ_TARGET_MAX_ATTACK = true;
                            extraParam = true;
                            continue;
                        case ErrorType2.REQ_WEAPON_EQUIPPED:
                            if (p.ownWeaponName == CardDB.cardName.unknown) return retval;
                            continue;
                        case ErrorType2.REQ_TARGET_FOR_COMBO:
                            if (p.cardsPlayedThisTurn >=1) targetAll = true;
                            continue;
                        case ErrorType2.REQ_TARGET_MIN_ATTACK:
                            REQ_TARGET_MIN_ATTACK = true;
                            extraParam = true;
                            continue;
                        case ErrorType2.REQ_MINIMUM_TOTAL_MINIONS:
                            if (this.needMinTotalMinions > p.ownMinions.Count + p.enemyMinions.Count) return retval;
                            continue;
                        case ErrorType2.REQ_MINION_CAP_IF_TARGET_AVAILABLE:
                            if (p.ownMinions.Count > 7 - this.needMinionsCapIfAvailable) return retval;
                            continue;
                        case ErrorType2.REQ_ENTIRE_ENTOURAGE_NOT_IN_PLAY:
                            int difftotem = 0;
                            foreach (Minion m in p.ownMinions)
                            {
                                if (m.name == CardDB.cardName.healingtotem || m.name == CardDB.cardName.wrathofairtotem || m.name == CardDB.cardName.searingtotem || m.name == CardDB.cardName.stoneclawtotem) difftotem++;
                            }
                            if (difftotem == 4) return retval;
                            continue;
                        case ErrorType2.REQ_MUST_TARGET_TAUNTER:
                            REQ_MUST_TARGET_TAUNTER = true;
                            extraParam = true;
                            continue;
                        case ErrorType2.REQ_TARGET_IF_AVAILABLE_AND_DRAGON_IN_HAND:
                            foreach (Handmanager.Handcard hc in p.owncards)
                            {
                                if ((TAG_RACE)hc.card.race == TAG_RACE.DRAGON) {targetAll = true; break; }
                            }
                            continue;
                        case ErrorType2.REQ_LEGENDARY_TARGET:
                            REQ_LEGENDARY_TARGET = true;
                            extraParam = true;
                            continue;
                        case ErrorType2.REQ_UNDAMAGED_TARGET:
                            REQ_UNDAMAGED_TARGET = true;
                            extraParam = true;
                            continue;
                        //case ErrorType2.REQ_STEADY_SHOT:
                        //    continue;
                        case ErrorType2.REQ_MINION_OR_ENEMY_HERO:
                            REQ_MINION_OR_ENEMY_HERO = true;
                            extraParam = true;
                            continue;
                        //default:
                    }
                }

			    if(targetAll)
			    {
                    wereTargets = true;
                    if (targetAllFriendly != targetAllEnemy)
                    {
                        if(targetAllFriendly) targets.AddRange(p.ownMinions);
                        else targets.AddRange(p.enemyMinions);
                    }
                    else
                    {
                        targets.AddRange(p.ownMinions);
                        targets.AddRange(p.enemyMinions);
                    }
				    if(targetOnlyMinion)
				    {
                        targetEnemyHero = false;
                        targetOwnHero = false;
				    }
                    else
                    {
                        targetEnemyHero = true;
                        targetOwnHero = true;
                    }
			    }

                if(extraParam)
                {
                    wereTargets = true;
                    if(REQ_TARGET_WITH_RACE)
                    {
                        foreach (Minion m in targets)
                        {
                            if (m.handcard.card.race != this.needRaceForPlaying) m.extraParam = true;
                        }
                        targetOwnHero = (p.ownHeroName == HeroEnum.lordjaraxxus && (TAG_RACE)this.needRaceForPlaying == TAG_RACE.DEMON);
                        targetEnemyHero = (p.enemyHeroName == HeroEnum.lordjaraxxus && (TAG_RACE)this.needRaceForPlaying == TAG_RACE.DEMON);
                    }
                    if(REQ_HERO_TARGET)
                    {
                        foreach (Minion m in targets)
                        {
                            m.extraParam = true;
                        }
                        targetOwnHero = true;
                        targetEnemyHero = true;
                    }
                    if(REQ_DAMAGED_TARGET)
                    {
                        foreach (Minion m in targets)
                        {
                            if (!m.wounded)
                            {
                                m.extraParam = true;
                            }
                        }
                        targetOwnHero = false;
                        targetEnemyHero = false;
                    }
                    if(REQ_TARGET_MAX_ATTACK)
                    {
                        foreach (Minion m in targets)
                        {
                            if (m.Angr > this.needWithMaxAttackValueOf)
                            {
                                m.extraParam = true;
                            }
                        }
                        targetOwnHero = false;
                        targetEnemyHero = false;
                    }
                    if(REQ_TARGET_MIN_ATTACK)
                    {
                        foreach (Minion m in targets)
                        {
                            if (m.Angr < this.needWithMinAttackValueOf)
                            {
                                m.extraParam = true;
                            }
                        }
                        targetOwnHero = false;
                        targetEnemyHero = false;
                    }
                    if(REQ_MUST_TARGET_TAUNTER)
                    {
                        foreach (Minion m in targets)
                        {
                            if (!m.taunt)
                            {
                                m.extraParam = true;
                            }
                        }
                        targetOwnHero = false;
                        targetEnemyHero = false;
                    }
                    if(REQ_UNDAMAGED_TARGET)
                    {
                        foreach (Minion m in targets)
                        {
                            if (m.wounded)
                            {
                                m.extraParam = true;
                            }
                        }
                        targetOwnHero = false;
                        targetEnemyHero = false;
                    }
                    if(REQ_LEGENDARY_TARGET)
                    {
                        wereTargets = false;
                        foreach (Minion m in targets)
                        {
                            if (m.handcard.card.rarity != 5) m.extraParam = true;
                        }
                        targetOwnHero = false;
                        targetEnemyHero = false;
                    }
                    if(REQ_MINION_OR_ENEMY_HERO)
                    {
                        if (p.weHaveSteamwheedleSniper)
                        {
                            foreach (Minion m in targets)
                            {
                                if (m.cantBeTargetedBySpellsOrHeroPowers && (this.type == cardtype.SPELL || this.type == cardtype.HEROPWR))
                                {
                                    m.extraParam = true;
                                    if(m.stealth && !m.own) m.extraParam = true;
                                }
                            }
                        }
                        targetEnemyHero = true;
                    }
                }
                
                if (isLethalCheck && targetEnemyHero)
                {
                    retval.Add(p.enemyHero);
                }
                else
                {
                    if (targetEnemyHero) retval.Add(p.enemyHero);
                    if (targetOwnHero) retval.Add(p.ownHero);

                    foreach (Minion m in targets)
                    {
                        if (m.extraParam != true)
                        {
                            if (m.stealth && !m.own) continue;
                            if (m.cantBeTargetedBySpellsOrHeroPowers && (this.type == cardtype.SPELL || this.type == cardtype.HEROPWR)) continue;
                            retval.Add(m);
                        }
                        m.extraParam = false;
                    }
                }

                if (retval.Count == 0 && !wereTargets) retval.Add(null);

                return retval;
            }

            public List<Minion> getTargetsForCardEnemy(Playfield p)
            {
                //todo make it faster!! 
                //todo remove the isRequirementInList with an big list of bools to ask the state of the bool
                bool addOwnHero = false;
                bool addEnemyHero = false;
                bool[] ownMins = new bool[p.ownMinions.Count];
                bool[] enemyMins = new bool[p.enemyMinions.Count];
                for (int i = 0; i < ownMins.Length; i++) ownMins[i] = false;
                for (int i = 0; i < enemyMins.Length; i++) enemyMins[i] = false;

                int k = 0;
                List<Minion> retval = new List<Minion>();

                if (isRequirementInList(CardDB.ErrorType2.REQ_TARGET_FOR_COMBO) && p.cardsPlayedThisTurn == 0) return retval;

                bool moreh = isRequirementInList(CardDB.ErrorType2.REQ_MINION_OR_ENEMY_HERO);
                if (isRequirementInList(CardDB.ErrorType2.REQ_TARGET_TO_PLAY) || isRequirementInList(CardDB.ErrorType2.REQ_NONSELF_TARGET) || isRequirementInList(CardDB.ErrorType2.REQ_TARGET_IF_AVAILABLE) || isRequirementInList(CardDB.ErrorType2.REQ_TARGET_FOR_COMBO))
                {
                    addEnemyHero = true;
                    addOwnHero = true;
                    k = -1;
                    foreach (Minion m in p.ownMinions)
                    {
                        k++;
                        if (((this.type == cardtype.SPELL || this.type == cardtype.HEROPWR) && (m.cantBeTargetedBySpellsOrHeroPowers)) || m.stealth) continue;
                        ownMins[k] = true;

                    }
                    k = -1;
                    foreach (Minion m in p.enemyMinions)
                    {
                        k++;
                        if ((this.type == cardtype.SPELL || this.type == cardtype.HEROPWR) && (m.cantBeTargetedBySpellsOrHeroPowers)) continue;
                        enemyMins[k] = true;
                    }

                }

                if (moreh)
                {
                    addOwnHero = true;

                    if (p.enemyHaveSteamwheedleSniper)
                    {
                        k = -1;
                        foreach (Minion m in p.ownMinions)
                        {
                            k++;
                            if (((this.type == cardtype.SPELL || this.type == cardtype.HEROPWR) && (m.cantBeTargetedBySpellsOrHeroPowers)) || m.stealth) continue;
                            ownMins[k] = true;

                        }

                    }
                }

                if (isRequirementInList(CardDB.ErrorType2.REQ_HERO_TARGET))
                {
                    for (int i = 0; i < ownMins.Length; i++) ownMins[i] = false;
                    for (int i = 0; i < enemyMins.Length; i++) enemyMins[i] = false;

                }

                if (isRequirementInList(CardDB.ErrorType2.REQ_MINION_TARGET))
                {
                    addOwnHero = false;
                    addEnemyHero = false;
                }

                if (isRequirementInList(CardDB.ErrorType2.REQ_FRIENDLY_TARGET))
                {
                    addOwnHero = false;
                    for (int i = 0; i < ownMins.Length; i++) ownMins[i] = false;

                }

                if (isRequirementInList(CardDB.ErrorType2.REQ_ENEMY_TARGET))
                {
                    addEnemyHero = false;
                    for (int i = 0; i < enemyMins.Length; i++) enemyMins[i] = false;
                }



                if (isRequirementInList(CardDB.ErrorType2.REQ_DAMAGED_TARGET))
                {
                    addEnemyHero = false;
                    addOwnHero = false;
                    k = -1;
                    foreach (Minion m in p.ownMinions)
                    {
                        k++;
                        if (!m.wounded)
                        {
                            ownMins[k] = false;
                        }
                    }
                    k = -1;
                    foreach (Minion m in p.enemyMinions)
                    {
                        k++;
                        if (!m.wounded)
                        {
                            enemyMins[k] = false;
                        }
                    }
                }

                if (isRequirementInList(CardDB.ErrorType2.REQ_UNDAMAGED_TARGET))
                {
                    addEnemyHero = false;
                    addOwnHero = false;
                    k = -1;
                    foreach (Minion m in p.ownMinions)
                    {
                        k++;
                        if (m.wounded)
                        {
                            ownMins[k] = false;
                        }
                    }
                    k = -1;
                    foreach (Minion m in p.enemyMinions)
                    {
                        k++;
                        if (m.wounded)
                        {
                            enemyMins[k] = false;
                        }
                    }
                }

                if (isRequirementInList(CardDB.ErrorType2.REQ_TARGET_MAX_ATTACK))
                {
                    addEnemyHero = false;
                    addOwnHero = false;
                    k = -1;
                    foreach (Minion m in p.ownMinions)
                    {
                        k++;
                        if (m.Angr > this.needWithMaxAttackValueOf)
                        {
                            ownMins[k] = false;
                        }
                    }
                    k = -1;
                    foreach (Minion m in p.enemyMinions)
                    {
                        k++;
                        if (m.Angr > this.needWithMaxAttackValueOf)
                        {
                            enemyMins[k] = false;
                        }
                    }
                }

                if (isRequirementInList(CardDB.ErrorType2.REQ_TARGET_MIN_ATTACK))
                {
                    addEnemyHero = false;
                    addOwnHero = false;
                    k = -1;
                    foreach (Minion m in p.ownMinions)
                    {
                        k++;
                        if (m.Angr < this.needWithMinAttackValueOf)
                        {
                            ownMins[k] = false;
                        }
                    }
                    k = -1;
                    foreach (Minion m in p.enemyMinions)
                    {
                        k++;
                        if (m.Angr < this.needWithMinAttackValueOf)
                        {
                            enemyMins[k] = false;
                        }
                    }
                }

                if (isRequirementInList(CardDB.ErrorType2.REQ_TARGET_WITH_RACE))
                {
                    addEnemyHero = false;
                    addOwnHero = false;
                    addOwnHero = (p.ownHeroName == HeroEnum.lordjaraxxus && (TAG_RACE)this.needRaceForPlaying == TAG_RACE.DEMON);
                    addEnemyHero = (p.enemyHeroName == HeroEnum.lordjaraxxus && (TAG_RACE)this.needRaceForPlaying == TAG_RACE.DEMON);
                    k = -1;
                    foreach (Minion m in p.ownMinions)
                    {
                        k++;
                        if ((m.handcard.card.race != this.needRaceForPlaying))
                        {
                            ownMins[k] = false;
                        }
                    }
                    k = -1;
                    foreach (Minion m in p.enemyMinions)
                    {
                        k++;
                        if ((m.handcard.card.race != this.needRaceForPlaying))
                        {
                            enemyMins[k] = false;
                        }
                    }
                }

                if (isRequirementInList(CardDB.ErrorType2.REQ_MUST_TARGET_TAUNTER))
                {
                    addEnemyHero = false;
                    addOwnHero = false;
                    k = -1;
                    foreach (Minion m in p.ownMinions)
                    {
                        k++;
                        if (!m.taunt)
                        {
                            ownMins[k] = false;
                        }
                    }
                    k = -1;
                    foreach (Minion m in p.enemyMinions)
                    {
                        k++;
                        if (!m.taunt)
                        {
                            enemyMins[k] = false;
                        }
                    }
                }

                if (addEnemyHero) retval.Add(p.enemyHero);
                if (addOwnHero) retval.Add(p.ownHero);

                k = -1;
                foreach (Minion m in p.ownMinions)
                {
                    k++;
                    if (ownMins[k]) retval.Add(m);
                }
                k = -1;
                foreach (Minion m in p.enemyMinions)
                {
                    k++;
                    if (enemyMins[k]) retval.Add(m);
                }

                return retval;

            }

            public int calculateManaCost(Playfield p)//calculates the mana from orginal mana, needed for back-to hand effects
            {
                int retval = this.cost;
                int offset = 0;

                if (this.type == cardtype.MOB)
                {
                    offset += p.soeldnerDerVenture * 3;

                    offset += p.managespenst;

                    int temp = -(p.startedWithbeschwoerungsportal) * 2;
                    if (retval + temp <= 0) temp = -retval + 1;
                    offset = offset + temp;

                    if (p.mobsplayedThisTurn == 0)
                    {
                        offset -= p.winzigebeschwoererin;
                    }

                    if (this.battlecry)
                    {
                        offset += p.nerubarweblord * 2;
                    }

                    if ((TAG_RACE)this.race == TAG_RACE.MECHANICAL)
                    { //if the number of zauberlehrlings change
                        offset -= p.anzOwnMechwarper;
                    }

                }

                if (this.type == cardtype.SPELL)
                { //if the number of zauberlehrlings change
                    offset -= (p.anzOwnsorcerersapprentice);
                    if (p.playedPreparation)
                    { //if the number of zauberlehrlings change
                        offset -= 3;
                    }

                }

                switch (this.name)
                {
                    case CardDB.cardName.dreadcorsair:
                        retval = retval + offset - p.ownWeaponAttack;
                        break;
                    case CardDB.cardName.seagiant:
                        retval = retval + offset - p.ownMinions.Count - p.enemyMinions.Count;
                        break;
                    case CardDB.cardName.mountaingiant:
                        retval = retval + offset - p.owncards.Count;
                        break;
                    case CardDB.cardName.clockworkgiant:
                        retval = retval + offset - p.enemyAnzCards;
                        break;
                    case CardDB.cardName.moltengiant:
                        retval = retval + offset - Math.Min(p.ownHero.Hp, p.ownHeroHpStarted);
                        break;
                    case CardDB.cardName.volcaniclumberer:
                        retval = retval + offset - p.ownMinionsDiedTurn - p.enemyMinionsDiedTurn;
                        break;
                    case CardDB.cardName.crush:
                        // cost 4 less if we have a dmged minion
                        bool dmgedminions = false;
                        foreach (Minion m in p.ownMinions)
                        {
                            if (m.wounded) dmgedminions = true;
                        }
                        if (dmgedminions)
                        {
                            retval = retval + offset - 4;
                        }
                        break;
                    default:
                        retval = retval + offset;
                        break;
                }

                if (this.Secret && p.playedmagierinderkirintor)
                {
                    retval = 0;
                }

                retval = Math.Max(0, retval);

                return retval;
            }

            public int getManaCost(Playfield p, int currentcost)//calculates mana from current mana
            {
                int retval = currentcost;


                int offset = 0; // if offset < 0 costs become lower, if >0 costs are higher at the end

                // CARDS that increase the manacosts of others ##############################
                //Manacosts changes with soeldner der venture co.
                if (p.soeldnerDerVenture != p.startedWithsoeldnerDerVenture && this.type == cardtype.MOB)
                {
                    offset += (p.soeldnerDerVenture - p.startedWithsoeldnerDerVenture) * 3;
                }

                //Manacosts changes with mana-ghost
                if (p.managespenst != p.startedWithManagespenst && this.type == cardtype.MOB)
                {
                    offset += (p.managespenst - p.startedWithManagespenst);
                }

                if (this.battlecry && p.nerubarweblord != p.startedWithnerubarweblord && this.type == cardtype.MOB)
                {
                    offset += (p.nerubarweblord - p.startedWithnerubarweblord) * 2;
                }

                //Manacosts changes with the Dragon Consort
                if (p.anzOwnDragonConsort != p.anzOwnDragonConsortStarted && (TAG_RACE)this.race == TAG_RACE.DRAGON)
                {
                    offset += (p.anzOwnDragonConsortStarted - p.anzOwnDragonConsort) * 2;
                }

                // CARDS that decrease the manacosts of others ##############################

                //Manacosts changes with the summoning-portal >_>
                if (p.startedWithbeschwoerungsportal != p.beschwoerungsportal && this.type == cardtype.MOB)
                { //cant lower the mana to 0
                    int temp = (p.startedWithbeschwoerungsportal - p.beschwoerungsportal) * 2;
                    if (retval + temp <= 0) temp = -retval + 1;
                    offset = offset + temp;
                }

                //Manacosts changes with the pint-sized summoner
                if (p.winzigebeschwoererin >= 1 && p.mobsplayedThisTurn >= 1 && p.startedWithMobsPlayedThisTurn == 0 && this.type == cardtype.MOB)
                { // if we start oure calculations with 0 mobs played, then the cardcost are 1 mana to low in the further calculations (with the little summoner on field)
                    offset += p.winzigebeschwoererin;
                }
                if (p.mobsplayedThisTurn == 0 && p.winzigebeschwoererin <= p.startedWithWinzigebeschwoererin && this.type == cardtype.MOB)
                { // one pint-sized summoner got killed, before we played the first mob -> the manacost are higher of all mobs
                    offset += (p.startedWithWinzigebeschwoererin - p.winzigebeschwoererin);
                }

                //Manacosts changes with the zauberlehrling summoner
                if (p.anzOwnsorcerersapprentice != p.anzOwnsorcerersapprenticeStarted && this.type == cardtype.SPELL)
                { //if the number of zauberlehrlings change
                    offset += (p.anzOwnsorcerersapprenticeStarted - p.anzOwnsorcerersapprentice);
                }

                //manacosts changes with Mechwarper
                if (p.anzOwnMechwarper != p.anzOwnMechwarperStarted && this.type == cardtype.MOB && (TAG_RACE)this.race == TAG_RACE.MECHANICAL)
                { //if the number of zauberlehrlings change
                    offset += (p.anzOwnMechwarperStarted - p.anzOwnMechwarper);
                }


                //manacosts are lowered, after we played preparation
                if (p.playedPreparation && this.type == cardtype.SPELL)
                { //if the number of zauberlehrlings change
                    offset -= 3;
                }


                switch (this.name)
                {
                    case CardDB.cardName.volcaniclumberer:
                        retval = retval + offset - p.ownMinionsDiedTurn - p.enemyMinionsDiedTurn;
                        break;
                    case CardDB.cardName.solemnvigil:
                        retval = retval + offset - p.ownMinionsDiedTurn - p.enemyMinionsDiedTurn;
                        break;
                    case CardDB.cardName.volcanicdrake:
                        retval = retval + offset - p.ownMinionsDiedTurn - p.enemyMinionsDiedTurn;
                        break;
                    case CardDB.cardName.dragonsbreath:
                        retval = retval + offset - p.ownMinionsDiedTurn - p.enemyMinionsDiedTurn;
                        break;
                    case CardDB.cardName.dreadcorsair:
                        retval = retval + offset - p.ownWeaponAttack + p.ownWeaponAttackStarted; // if weapon attack change we change manacost
                        break;
                    case CardDB.cardName.seagiant:
                        retval = retval + offset - p.ownMinions.Count - p.enemyMinions.Count + p.ownMobsCountStarted;
                        break;
                    case CardDB.cardName.mountaingiant:
                        retval = retval + offset - p.owncards.Count + p.ownCardsCountStarted;
                        break;
                    case CardDB.cardName.clockworkgiant:
                        retval = retval + offset - p.enemyAnzCards + p.enemyCardsCountStarted;
                        break;
                    case CardDB.cardName.moltengiant:
                        retval = retval + offset - Math.Min(p.ownHero.Hp, p.ownHeroHpStarted) + p.ownHeroHpStarted;
                        break;
                    case CardDB.cardName.crush:
                        // cost 4 less if we have a dmged minion
                        bool dmgedminions = false;
                        foreach (Minion m in p.ownMinions)
                        {
                            if (m.wounded) dmgedminions = true;
                        }
                        if (dmgedminions != p.startedWithDamagedMinions)
                        {
                            if (dmgedminions)
                            {
                                retval = retval + offset - 4;
                            }
                            else
                            {
                                retval = retval + offset + 4;
                            }
                        }
                        break;
                    default:
                        retval = retval + offset;
                        break;
                }

                if (this.Secret && p.playedmagierinderkirintor)
                {
                    retval = 0;
                }

                retval = Math.Max(0, retval);

                return retval;
            }
            public bool canplayCard(Playfield p, int manacost)
            {
                //is playrequirement?
                bool haveToDoRequires = isRequirementInList(CardDB.ErrorType2.REQ_TARGET_TO_PLAY);
                bool retval = true;
                // cant play if i have to few mana

                if (p.mana < this.getManaCost(p, manacost)) return false;

                // cant play mob, if i have allready 7 mininos
                if (this.type == CardDB.cardtype.MOB && p.ownMinions.Count >= 7) return false;

                if (isRequirementInList(CardDB.ErrorType2.REQ_MINIMUM_ENEMY_MINIONS))
                {
                    if (p.enemyMinions.Count < this.needMinNumberOfEnemy) return false;
                }
                if (isRequirementInList(CardDB.ErrorType2.REQ_NUM_MINION_SLOTS))
                {
                    if (p.ownMinions.Count > 7 - this.needEmptyPlacesForPlaying) return false;
                }

                if (isRequirementInList(CardDB.ErrorType2.REQ_WEAPON_EQUIPPED))
                {
                    if (p.ownWeaponName == CardDB.cardName.unknown) return false;
                }

                if (isRequirementInList(CardDB.ErrorType2.REQ_MINIMUM_TOTAL_MINIONS))
                {
                    if (this.needMinTotalMinions > p.ownMinions.Count + p.enemyMinions.Count) return false;
                }

                if (haveToDoRequires)
                {
                    if (this.getTargetsForCard(p, false).Count == 0) return false;

                    //it requires a target-> return false if 
                }

                if (isRequirementInList(CardDB.ErrorType2.REQ_TARGET_IF_AVAILABLE) && isRequirementInList(CardDB.ErrorType2.REQ_MINION_CAP_IF_TARGET_AVAILABLE))
                {
                    if (this.getTargetsForCard(p, false).Count >= 1 && p.ownMinions.Count > 7 - this.needMinionsCapIfAvailable) return false;
                }

                if (isRequirementInList(CardDB.ErrorType2.REQ_ENTIRE_ENTOURAGE_NOT_IN_PLAY))
                {
                    int difftotem = 0;
                    foreach (Minion m in p.ownMinions)
                    {
                        if (m.name == CardDB.cardName.healingtotem || m.name == CardDB.cardName.wrathofairtotem || m.name == CardDB.cardName.searingtotem || m.name == CardDB.cardName.stoneclawtotem) difftotem++;
                    }
                    if (difftotem == 4) return false;
                }


                if (this.Secret)
                {
                    if (p.ownSecretsIDList.Contains(this.cardIDenum)) return false;
                    if (p.ownSecretsIDList.Count >= 5) return false;
                }


                return true;
            }



        }

        List<string> namelist = new List<string>();
        List<Card> cardlist = new List<Card>();
        Dictionary<cardIDEnum, Card> cardidToCardList = new Dictionary<cardIDEnum, Card>();
        List<string> allCardIDS = new List<string>();
        public Card unknownCard;
        public bool installedWrong = false;

        public Card teacherminion;
        public Card illidanminion;
        public Card lepergnome;
        public Card burlyrockjaw;
        private static CardDB instance;

        public static CardDB Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CardDB();
                    //instance.enumCreator();// only call this to get latest cardids
                    /*foreach (KeyValuePair<cardIDEnum, Card> kvp in instance.cardidToCardList)
                    {
                        HRSim.Helpfunctions.Instance.logg(kvp.Value.name + " " + kvp.Value.Attack);
                    }*/
                    // have to do it 2 times (or the kids inside the simcards will not have a simcard :D
                    foreach (Card c in instance.cardlist)
                    {
                        c.sim_card = instance.getSimCard(c.cardIDenum);
                    }
                    instance.setAdditionalData();
                }
                return instance;
            }
        }

        public CardDB()
        {
            string[] lines = new string[0] { };
            try
            {
                string path = "C:\\Code\\AIPJ\\ConsoleApplication1\\";
                lines = System.IO.File.ReadAllLines(path + "_carddb.txt");
                HRSim.Helpfunctions.Instance.ErrorLog("read carddb.txt");
            }
            catch
            {
                HRSim.Helpfunctions.Instance.logg("cant find _carddb.txt");
                HRSim.Helpfunctions.Instance.ErrorLog("ERROR#################################################");
                HRSim.Helpfunctions.Instance.ErrorLog("ERROR#################################################");
                HRSim.Helpfunctions.Instance.ErrorLog("ERROR#################################################");
                HRSim.Helpfunctions.Instance.ErrorLog("ERROR#################################################");
                HRSim.Helpfunctions.Instance.ErrorLog("cant find _carddb.txt in ");
                HRSim.Helpfunctions.Instance.ErrorLog("ERROR#################################################");
                HRSim.Helpfunctions.Instance.ErrorLog("ERROR#################################################");
                HRSim.Helpfunctions.Instance.ErrorLog("ERROR#################################################");
                HRSim.Helpfunctions.Instance.ErrorLog("ERROR#################################################");
                HRSim.Helpfunctions.Instance.ErrorLog("you installed it wrong");
                HRSim.Helpfunctions.Instance.ErrorLog("ERROR#################################################");
                HRSim.Helpfunctions.Instance.ErrorLog("ERROR#################################################");
                HRSim.Helpfunctions.Instance.ErrorLog("ERROR#################################################");
                HRSim.Helpfunctions.Instance.ErrorLog("ERROR#################################################");
                this.installedWrong = true;
            }
            cardlist.Clear();
            this.cardidToCardList.Clear();
            Card c = new Card();
            int de = 0;
            //placeholdercard
            Card plchldr = new Card { name = cardName.unknown, cost = 1000 };
            this.namelist.Add("unknown");
            this.cardlist.Add(plchldr);
            this.unknownCard = cardlist[0];
            string name = "";
            foreach (string s in lines)
            {
                if (s.Contains("/Entity"))
                {
                    if (c.type == cardtype.ENCHANTMENT)
                    {
                        //HRSim.Helpfunctions.Instance.logg(c.CardID);
                        //HRSim.Helpfunctions.Instance.logg(c.name);
                        //HRSim.Helpfunctions.Instance.logg(c.description);
                        continue;
                    }
                    if (name != "")
                    {
                        this.namelist.Add(name);
                    }
                    name = "";
                    if (c.name != CardDB.cardName.unknown)
                    {

                        this.cardlist.Add(c);
                        //HRSim.Helpfunctions.Instance.logg(c.name);

                        if (!this.cardidToCardList.ContainsKey(c.cardIDenum))
                        {
                            this.cardidToCardList.Add(c.cardIDenum, c);
                        }
                    }

                }
                if (s.Contains("<Entity version=\"") && s.Contains(" CardID=\""))
                {
                    c = new Card();
                    de = 0;
                    string temp = s.Split(new string[] { "CardID=\"" }, StringSplitOptions.None)[1];
                    temp = temp.Replace("\">", "");
                    //c.CardID = temp;
                    allCardIDS.Add(temp);
                    c.cardIDenum = this.cardIdstringToEnum(temp);

                    //token:
                    if (temp.EndsWith("t"))
                    {
                        c.isToken = true;
                    }
                    if (temp.Equals("ds1_whelptoken")) c.isToken = true;
                    if (temp.Equals("CS2_mirror")) c.isToken = true;
                    if (temp.Equals("CS2_050")) c.isToken = true;
                    if (temp.Equals("CS2_052")) c.isToken = true;
                    if (temp.Equals("CS2_051")) c.isToken = true;
                    if (temp.Equals("NEW1_009")) c.isToken = true;
                    if (temp.Equals("CS2_152")) c.isToken = true;
                    if (temp.Equals("CS2_boar")) c.isToken = true;
                    if (temp.Equals("EX1_tk11")) c.isToken = true;
                    if (temp.Equals("EX1_506a")) c.isToken = true;
                    if (temp.Equals("skele21")) c.isToken = true;
                    if (temp.Equals("EX1_tk9")) c.isToken = true;
                    if (temp.Equals("EX1_finkle")) c.isToken = true;
                    if (temp.Equals("EX1_598")) c.isToken = true;
                    if (temp.Equals("EX1_tk34")) c.isToken = true;
                    //if (c.isToken) HRSim.Helpfunctions.Instance.ErrorLog(temp +" is token");

                    continue;
                }
                /*
                if (s.Contains("<Entity version=\"1\" CardID=\""))
                {
                    c = new Card();
                    de = 0;
                    string temp = s.Replace("<Entity version=\"1\" CardID=\"", "");
                    temp = temp.Replace("\">", "");
                    //c.CardID = temp;
                    allCardIDS.Add(temp);
                    c.cardIDenum = this.cardIdstringToEnum(temp);
                    continue;
                }*/

                //health
                if (s.Contains("<Tag enumID=\"45\""))
                {
                    string temp = s.Split(new string[] { "value=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    c.Health = Convert.ToInt32(temp);
                    continue;
                }

                //Class
                if (s.Contains("Tag enumID=\"199\"")) //added fopr sake of figure out which class it belongs too... sorry adds a little more data
                {
                    string temp = s.Split(new string[] { "value=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    c.Class = Convert.ToInt32(temp);
                    continue;
                }

                //attack
                if (s.Contains("<Tag enumID=\"47\""))
                {
                    string temp = s.Split(new string[] { "value=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    c.Attack = Convert.ToInt32(temp);
                    continue;
                }
                //race
                if (s.Contains("<Tag enumID=\"200\""))
                {
                    string temp = s.Split(new string[] { "value=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    c.race = Convert.ToInt32(temp);
                    continue;
                }
                //rarity
                if (s.Contains("<Tag enumID=\"203\""))
                {
                    string temp = s.Split(new string[] { "value=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    c.rarity = Convert.ToInt32(temp);
                    continue;
                }
                //manacost
                if (s.Contains("<Tag enumID=\"48\""))
                {
                    string temp = s.Split(new string[] { "value=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    c.cost = Convert.ToInt32(temp);
                    continue;
                }
                //cardtype
                if (s.Contains("<Tag enumID=\"202\""))
                {
                    string temp = s.Split(new string[] { "value=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    if (c.name != CardDB.cardName.unknown)
                    {
                        //HRSim.Helpfunctions.Instance.logg(temp);
                    }

                    int crdtype = Convert.ToInt32(temp);
                    if (crdtype == 10)
                    {
                        c.type = CardDB.cardtype.HEROPWR;
                    }
                    if (crdtype == 4)
                    {
                        c.type = CardDB.cardtype.MOB;
                    }
                    if (crdtype == 5)
                    {
                        c.type = CardDB.cardtype.SPELL;
                    }
                    if (crdtype == 6)
                    {
                        c.type = CardDB.cardtype.ENCHANTMENT;
                    }
                    if (crdtype == 7)
                    {
                        c.type = CardDB.cardtype.WEAPON;
                    }
                    continue;
                }

                //cardname
                if (s.Contains("<Tag enumID=\"185\""))
                {
                    string temp = s.Split(new string[] { "type=\"String\">" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split(new string[] { "</Tag>" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    temp = temp.Replace("&lt;", "");
                    temp = temp.Replace("b&gt;", "");
                    temp = temp.Replace("/b&gt;", "");
                    temp = temp.ToLower();

                    temp = temp.Replace("'", "");
                    temp = temp.Replace(" ", "");
                    temp = temp.Replace(":", "");
                    temp = temp.Replace(".", "");
                    temp = temp.Replace("!", "");
                    temp = temp.Replace("-", "");

                    //HRSim.Helpfunctions.Instance.logg(temp);
                    c.name = this.cardNamestringToEnum(temp);
                    name = temp;


                    continue;
                }

                //cardtextinhand
                if (s.Contains("<Tag enumID=\"184\""))
                {
                    string temp = s.Split(new string[] { "type=\"String\">" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split(new string[] { "</Tag>" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    temp = temp.Replace("&lt;", "");
                    temp = temp.Replace("b&gt;", "");
                    temp = temp.Replace("/b&gt;", "");
                    temp = temp.ToLower();

                    if (temp.Contains("choose one"))
                    {
                        c.choice = true;
                        //HRSim.Helpfunctions.Instance.logg(c.name + " is choice");
                    }
                    continue;
                }
                //targetingarrowtext
                if (s.Contains("<Tag enumID=\"325\""))
                {

                    string temp = s.Split(new string[] { "type=\"String\">" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split(new string[] { "</Tag>" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    temp = temp.Replace("&lt;", "");
                    temp = temp.Replace("b&gt;", "");
                    temp = temp.Replace("/b&gt;", "");
                    temp = temp.ToLower();

                    c.target = true;
                    continue;
                }


                //poisonous
                if (s.Contains("<Tag enumID=\"363\""))
                {
                    string temp = s.Split(new string[] { "value=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    int ti = Convert.ToInt32(temp);
                    if (ti == 1) c.poisionous = true;
                    continue;
                }
                //enrage
                if (s.Contains("<Tag enumID=\"212\""))
                {
                    string temp = s.Split(new string[] { "value=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    int ti = Convert.ToInt32(temp);
                    if (ti == 1) c.Enrage = true;
                    continue;
                }
                //OneTurnEffect
                if (s.Contains("<Tag enumID=\"338\""))
                {
                    string temp = s.Split(new string[] { "value=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    int ti = Convert.ToInt32(temp);
                    if (ti == 1) c.oneTurnEffect = true;
                    continue;
                }
                //aura
                if (s.Contains("<Tag enumID=\"362\""))
                {
                    string temp = s.Split(new string[] { "value=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    int ti = Convert.ToInt32(temp);
                    if (ti == 1) c.Aura = true;
                    continue;
                }

                //taunt
                if (s.Contains("<Tag enumID=\"190\""))
                {
                    string temp = s.Split(new string[] { "value=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    int ti = Convert.ToInt32(temp);
                    if (ti == 1) c.tank = true;
                    continue;
                }
                //battlecry
                if (s.Contains("<Tag enumID=\"218\""))
                {
                    string temp = s.Split(new string[] { "value=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    int ti = Convert.ToInt32(temp);
                    if (ti == 1) c.battlecry = true;
                    continue;
                }
                //windfury
                if (s.Contains("<Tag enumID=\"189\""))
                {
                    string temp = s.Split(new string[] { "value=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    int ti = Convert.ToInt32(temp);
                    if (ti == 1) c.windfury = true;
                    continue;
                }
                //deathrattle
                if (s.Contains("<Tag enumID=\"217\""))
                {
                    string temp = s.Split(new string[] { "value=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    int ti = Convert.ToInt32(temp);
                    if (ti == 1) c.deathrattle = true;
                    continue;
                }
                //durability
                if (s.Contains("<Tag enumID=\"187\""))
                {
                    string temp = s.Split(new string[] { "value=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    c.Durability = Convert.ToInt32(temp);
                    continue;
                }
                //elite
                if (s.Contains("<Tag enumID=\"114\""))
                {
                    string temp = s.Split(new string[] { "value=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    int ti = Convert.ToInt32(temp);
                    if (ti == 1) c.Elite = true;
                    continue;
                }
                //combo
                if (s.Contains("<Tag enumID=\"220\""))
                {
                    string temp = s.Split(new string[] { "value=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    int ti = Convert.ToInt32(temp);
                    if (ti == 1) c.Combo = true;
                    continue;
                }
                //recall
                if (s.Contains("<Tag enumID=\"215\""))
                {
                    string temp = s.Split(new string[] { "value=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    int ti = Convert.ToInt32(temp);
                    if (ti == 1) c.Recall = true;
                    c.recallValue = 1;
                    if (c.name == CardDB.cardName.forkedlightning) c.recallValue = 2;
                    if (c.name == CardDB.cardName.dustdevil) c.recallValue = 2;
                    if (c.name == CardDB.cardName.lightningstorm) c.recallValue = 2;
                    if (c.name == CardDB.cardName.lavaburst) c.recallValue = 2;
                    if (c.name == CardDB.cardName.feralspirit) c.recallValue = 2;
                    if (c.name == CardDB.cardName.doomhammer) c.recallValue = 2;
                    if (c.name == CardDB.cardName.earthelemental) c.recallValue = 3;
					if (c.name == CardDB.cardName.neptulon) c.recallValue = 3;
                    continue;
                }
                //immunetospellpower
                if (s.Contains("<Tag enumID=\"349\""))
                {
                    string temp = s.Split(new string[] { "value=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    int ti = Convert.ToInt32(temp);
                    if (ti == 1) c.immuneToSpellpowerg = true;
                    continue;
                }
                //stealh
                if (s.Contains("<Tag enumID=\"191\""))
                {
                    string temp = s.Split(new string[] { "value=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    int ti = Convert.ToInt32(temp);
                    if (ti == 1) c.Stealth = true;
                    continue;
                }
                //secret
                if (s.Contains("<Tag enumID=\"219\""))
                {
                    string temp = s.Split(new string[] { "value=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    int ti = Convert.ToInt32(temp);
                    if (ti == 1) c.Secret = true;
                    continue;
                }
                //freeze
                if (s.Contains("<Tag enumID=\"208\""))
                {
                    string temp = s.Split(new string[] { "value=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    int ti = Convert.ToInt32(temp);
                    if (ti == 1) c.Freeze = true;
                    continue;
                }
                //adjacentbuff
                if (s.Contains("<Tag enumID=\"350\""))
                {
                    string temp = s.Split(new string[] { "value=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    int ti = Convert.ToInt32(temp);
                    if (ti == 1) c.AdjacentBuff = true;
                    continue;
                }
                //divineshield
                if (s.Contains("<Tag enumID=\"194\""))
                {
                    string temp = s.Split(new string[] { "value=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    int ti = Convert.ToInt32(temp);
                    if (ti == 1) c.Shield = true;
                    continue;
                }
                //charge
                if (s.Contains("<Tag enumID=\"197\""))
                {
                    string temp = s.Split(new string[] { "value=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    int ti = Convert.ToInt32(temp);
                    if (ti == 1) c.Charge = true;
                    continue;
                }
                //silence
                if (s.Contains("<Tag enumID=\"339\""))
                {
                    string temp = s.Split(new string[] { "value=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    int ti = Convert.ToInt32(temp);
                    if (ti == 1) c.Silence = true;
                    continue;
                }
                //morph
                if (s.Contains("<Tag enumID=\"293\""))
                {
                    string temp = s.Split(new string[] { "value=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    int ti = Convert.ToInt32(temp);
                    if (ti == 1) c.Morph = true;
                    continue;
                }
                //spellpower
                if (s.Contains("<Tag enumID=\"192\""))
                {
                    string temp = s.Split(new string[] { "value=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    int ti = Convert.ToInt32(temp);
                    if (ti == 1) c.Spellpower = true;
                    c.spellpowervalue = 1;
                    if (c.name == CardDB.cardName.ancientmage) c.spellpowervalue = 0;
                    if (c.name == CardDB.cardName.malygos) c.spellpowervalue = 5;
                    continue;
                }
                //grantcharge
                if (s.Contains("<Tag enumID=\"355\""))
                {
                    string temp = s.Split(new string[] { "value=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    int ti = Convert.ToInt32(temp);
                    if (ti == 1) c.GrantCharge = true;
                    continue;
                }
                //healtarget
                if (s.Contains("<Tag enumID=\"361\""))
                {
                    string temp = s.Split(new string[] { "value=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    int ti = Convert.ToInt32(temp);
                    if (ti == 1) c.HealTarget = true;
                    continue;
                }
                if (s.Contains("<PlayRequirement"))
                {
                    //if (!s.Contains("param=\"\"")) Console.WriteLine(s);

                    string temp = s.Split(new string[] { "reqID=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    ErrorType2 et2 = (ErrorType2)Convert.ToInt32(temp);
                    c.playrequires.Add(et2);
                }


                if (s.Contains("<PlayRequirement reqID=\"12\" param=\""))
                {
                    string temp = s.Split(new string[] { "param=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    c.needEmptyPlacesForPlaying = Convert.ToInt32(temp);
                    continue;
                }
                if (s.Contains("PlayRequirement reqID=\"41\" param=\""))
                {
                    string temp = s.Split(new string[] { "param=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    c.needWithMinAttackValueOf = Convert.ToInt32(temp);
                    continue;
                }
                if (s.Contains("PlayRequirement reqID=\"8\" param=\""))
                {
                    string temp = s.Split(new string[] { "param=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    c.needWithMaxAttackValueOf = Convert.ToInt32(temp);
                    continue;
                }
                if (s.Contains("PlayRequirement reqID=\"10\" param=\""))
                {
                    string temp = s.Split(new string[] { "param=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    c.needRaceForPlaying = Convert.ToInt32(temp);
                    continue;
                }
                if (s.Contains("PlayRequirement reqID=\"23\" param=\""))
                {
                    string temp = s.Split(new string[] { "param=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    c.needMinNumberOfEnemy = Convert.ToInt32(temp);
                    continue;
                }
                if (s.Contains("PlayRequirement reqID=\"45\" param=\""))
                {
                    string temp = s.Split(new string[] { "param=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    c.needMinTotalMinions = Convert.ToInt32(temp);
                    continue;
                }
                if (s.Contains("PlayRequirement reqID=\"19\" param=\""))
                {
                    string temp = s.Split(new string[] { "param=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    c.needMinionsCapIfAvailable = Convert.ToInt32(temp);
                    continue;
                }



                if (s.Contains("<Tag name="))
                {
                    string temp = s.Split(new string[] { "<Tag name=\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    temp = temp.Split('\"')[0];
                    /*
                    if (temp != "DevState" && temp != "FlavorText" && temp != "ArtistName" && temp != "Cost" && temp != "EnchantmentIdleVisual" && temp != "EnchantmentBirthVisual" && temp != "Collectible" && temp != "CardSet" && temp != "AttackVisualType" && temp != "CardName" && temp != "Class" && temp != "CardTextInHand" && temp != "Rarity" && temp != "TriggerVisual" && temp != "Faction" && temp != "HowToGetThisGoldCard" && temp != "HowToGetThisCard" && temp != "CardTextInPlay")
                        HRSim.Helpfunctions.Instance.logg(s);*/
                }


            }

            this.teacherminion = this.getCardDataFromID(CardDB.cardIDEnum.NEW1_026t);
            this.illidanminion = this.getCardDataFromID(CardDB.cardIDEnum.EX1_614t);
            this.lepergnome = this.getCardDataFromID(CardDB.cardIDEnum.EX1_029);
            this.burlyrockjaw = this.getCardDataFromID(CardDB.cardIDEnum.GVG_068);

            setupHealDatabase();
            setupEnrageDatabase();
            setupDamageDatabase();
            setupPriorityList();
            setupsilenceDatabase();
            setupAttackBuff();
            setupHealthBuff();
            setupCardDrawBattlecry();
            setupDiscardCards();
            setupDestroyOwnCards();
            setupSpecialMins();
            setupEnemyTargetPriority();
            setupHeroDamagingAOE();
            setupBuffingMinions();
            setupRandomCards();
            setupLethalHelpMinions();
            setupSilenceTargets();
            setupUsefulNeedKeepDatabase();
            setupRelations();
            setupChooseDatabase();

        }

        public Card getCardData(CardDB.cardName cardname)
        {

            foreach (Card ca in this.cardlist)
            {
                if (ca.name == cardname)
                {
                    return ca;
                }
            }

            return unknownCard;
        }

        public Card getDeckCardData(CardDB.cardName cardname)
        {

            foreach (Card ca in this.cardlist)
            {
                if (ca.name == cardname && ca.type != cardtype.NONE)
                {
                    return ca;
                }
            }

            return unknownCard;
        }

        public Card getCardDataFromID(cardIDEnum id)
        {
            return this.cardidToCardList.ContainsKey(id) ? this.cardidToCardList[id] : this.unknownCard;
        }

        public SimTemplate getSimCard(cardIDEnum id)
        {
            switch (id)
            {
                case cardIDEnum.NEW1_007b:
                    return new Sim_NEW1_007b();
                case cardIDEnum.EX1_613:
                    return new Sim_EX1_613();
                case cardIDEnum.EX1_133:
                    return new Sim_EX1_133();
                case cardIDEnum.NEW1_018:
                    return new Sim_NEW1_018();
                case cardIDEnum.EX1_012:
                    return new Sim_EX1_012();
                case cardIDEnum.EX1_178a:
                    return new Sim_EX1_178a();
                case cardIDEnum.CS2_231:
                    return new Sim_CS2_231();
                case cardIDEnum.CS2_179:
                    return new Sim_CS2_179();
                case cardIDEnum.EX1_244:
                    return new Sim_EX1_244();
                case cardIDEnum.EX1_178b:
                    return new Sim_EX1_178b();
                case cardIDEnum.EX1_573b:
                    return new Sim_EX1_573b();
                case cardIDEnum.NEW1_007a:
                    return new Sim_NEW1_007a();
                case cardIDEnum.EX1_345t:
                    return new Sim_EX1_345t();
                case cardIDEnum.FP1_007t:
                    return new Sim_FP1_007t();
                case cardIDEnum.EX1_025:
                    return new Sim_EX1_025();
                case cardIDEnum.EX1_396:
                    return new Sim_EX1_396();
                case cardIDEnum.NEW1_017:
                    return new Sim_NEW1_017();
                case cardIDEnum.NEW1_008a:
                    return new Sim_NEW1_008a();
                case cardIDEnum.EX1_533:
                    return new Sim_EX1_533();
                case cardIDEnum.EX1_522:
                    return new Sim_EX1_522();

                // case CardDB.cardIDEnum.NAX11_04: return new Sim_NAX11_04();
                case cardIDEnum.NEW1_026:
                    return new Sim_NEW1_026();
                case cardIDEnum.EX1_398:
                    return new Sim_EX1_398();

                // case CardDB.cardIDEnum.NAX4_04: return new Sim_NAX4_04();
                case cardIDEnum.EX1_007:
                    return new Sim_EX1_007();
                case cardIDEnum.CS1_112:
                    return new Sim_CS1_112();
                case cardIDEnum.NEW1_036:
                    return new Sim_NEW1_036();
                case cardIDEnum.EX1_258:
                    return new Sim_EX1_258();
                case cardIDEnum.HERO_01:
                    return new Sim_HERO_01();
                case cardIDEnum.CS2_087:
                    return new Sim_CS2_087();
                case cardIDEnum.DREAM_05:
                    return new Sim_DREAM_05();
                case cardIDEnum.CS2_092:
                    return new Sim_CS2_092();
                case cardIDEnum.CS2_022:
                    return new Sim_CS2_022();
                case cardIDEnum.EX1_046:
                    return new Sim_EX1_046();
                case cardIDEnum.PRO_001b:
                    return new Sim_PRO_001b();
                case cardIDEnum.PRO_001a:
                    return new Sim_PRO_001a();
                case cardIDEnum.CS2_103:
                    return new Sim_CS2_103();
                case cardIDEnum.NEW1_041:
                    return new Sim_NEW1_041();
                case cardIDEnum.EX1_360:
                    return new Sim_EX1_360();
                case cardIDEnum.FP1_023:
                    return new Sim_FP1_023();
                case cardIDEnum.NEW1_038:
                    return new Sim_NEW1_038();
                case cardIDEnum.CS2_009:
                    return new Sim_CS2_009();
                case cardIDEnum.EX1_010:
                    return new Sim_EX1_010();
                case cardIDEnum.CS2_024:
                    return new Sim_CS2_024();
                case cardIDEnum.EX1_565:
                    return new Sim_EX1_565();
                case cardIDEnum.CS2_076:
                    return new Sim_CS2_076();
                case cardIDEnum.FP1_004:
                    return new Sim_FP1_004();
                case cardIDEnum.CS2_162:
                    return new Sim_CS2_162();
                case cardIDEnum.EX1_110t:
                    return new Sim_EX1_110t();
                case cardIDEnum.CS2_181:
                    return new Sim_CS2_181();
                case cardIDEnum.EX1_309:
                    return new Sim_EX1_309();
                case cardIDEnum.EX1_354:
                    return new Sim_EX1_354();
                case cardIDEnum.EX1_023:
                    return new Sim_EX1_023();
                case cardIDEnum.NEW1_034:
                    return new Sim_NEW1_034();
                case cardIDEnum.CS2_003:
                    return new Sim_CS2_003();
                case cardIDEnum.HERO_06:
                    return new Sim_HERO_06();
                case cardIDEnum.CS2_201:
                    return new Sim_CS2_201();
                case cardIDEnum.EX1_508:
                    return new Sim_EX1_508();
                case cardIDEnum.EX1_259:
                    return new Sim_EX1_259();
                case cardIDEnum.EX1_341:
                    return new Sim_EX1_341();
                case cardIDEnum.EX1_103:
                    return new Sim_EX1_103();
                case cardIDEnum.FP1_021:
                    return new Sim_FP1_021();
                case cardIDEnum.EX1_411:
                    return new Sim_EX1_411();
                case cardIDEnum.CS2_053:
                    return new Sim_CS2_053();
                case cardIDEnum.CS2_182:
                    return new Sim_CS2_182();
                case cardIDEnum.CS2_008:
                    return new Sim_CS2_008();
                case cardIDEnum.CS2_233:
                    return new Sim_CS2_233();
                case cardIDEnum.EX1_626:
                    return new Sim_EX1_626();
                case cardIDEnum.EX1_059:
                    return new Sim_EX1_059();
                case cardIDEnum.EX1_334:
                    return new Sim_EX1_334();
                case cardIDEnum.EX1_619:
                    return new Sim_EX1_619();
                case cardIDEnum.NEW1_032:
                    return new Sim_NEW1_032();
                case cardIDEnum.EX1_158t:
                    return new Sim_EX1_158t();
                case cardIDEnum.EX1_006:
                    return new Sim_EX1_006();
                case cardIDEnum.NEW1_031:
                    return new Sim_NEW1_031();
                case cardIDEnum.DREAM_04:
                    return new Sim_DREAM_04();
                case cardIDEnum.EX1_004:
                    return new Sim_EX1_004();
                case cardIDEnum.EX1_095:
                    return new Sim_EX1_095();
                case cardIDEnum.NEW1_007:
                    return new Sim_NEW1_007();
                case cardIDEnum.EX1_275:
                    return new Sim_EX1_275();
                case cardIDEnum.EX1_245:
                    return new Sim_EX1_245();
                case cardIDEnum.EX1_383:
                    return new Sim_EX1_383();
                case cardIDEnum.FP1_016:
                    return new Sim_FP1_016();
                case cardIDEnum.CS2_125:
                    return new Sim_CS2_125();
                case cardIDEnum.EX1_137:
                    return new Sim_EX1_137();
                case cardIDEnum.DS1_185:
                    return new Sim_DS1_185();
                case cardIDEnum.FP1_010:
                    return new Sim_FP1_010();
                case cardIDEnum.EX1_598:
                    return new Sim_EX1_598();
                case cardIDEnum.EX1_304:
                    return new Sim_EX1_304();
                case cardIDEnum.EX1_302:
                    return new Sim_EX1_302();
                case cardIDEnum.EX1_614t:
                    return new Sim_EX1_614t();
                case cardIDEnum.CS2_108:
                    return new Sim_CS2_108();
                case cardIDEnum.CS2_046:
                    return new Sim_CS2_046();
                case cardIDEnum.EX1_014t:
                    return new Sim_EX1_014t();
                case cardIDEnum.NEW1_005:
                    return new Sim_NEW1_005();
                case cardIDEnum.EX1_062:
                    return new Sim_EX1_062();
                case cardIDEnum.Mekka1:
                    return new Sim_Mekka1();
                case cardIDEnum.tt_010a:
                    return new Sim_tt_010a();
                case cardIDEnum.CS2_072:
                    return new Sim_CS2_072();
                case cardIDEnum.EX1_tk28:
                    return new Sim_EX1_tk28();
                case cardIDEnum.FP1_014:
                    return new Sim_FP1_014();
                case cardIDEnum.EX1_409t:
                    return new Sim_EX1_409t();
                case cardIDEnum.EX1_507:
                    return new Sim_EX1_507();
                case cardIDEnum.EX1_144:
                    return new Sim_EX1_144();
                case cardIDEnum.CS2_038:
                    return new Sim_CS2_038();
                case cardIDEnum.EX1_093:
                    return new Sim_EX1_093();
                case cardIDEnum.CS2_080:
                    return new Sim_CS2_080();
                case cardIDEnum.EX1_005:
                    return new Sim_EX1_005();
                case cardIDEnum.EX1_382:
                    return new Sim_EX1_382();
                case cardIDEnum.CS2_028:
                    return new Sim_CS2_028();
                case cardIDEnum.EX1_538:
                    return new Sim_EX1_538();
                case cardIDEnum.DREAM_02:
                    return new Sim_DREAM_02();
                case cardIDEnum.EX1_581:
                    return new Sim_EX1_581();
                case cardIDEnum.EX1_131t:
                    return new Sim_EX1_131t();
                case cardIDEnum.CS2_147:
                    return new Sim_CS2_147();
                case cardIDEnum.CS1_113:
                    return new Sim_CS1_113();
                case cardIDEnum.CS2_161:
                    return new Sim_CS2_161();
                case cardIDEnum.CS2_031:
                    return new Sim_CS2_031();
                case cardIDEnum.EX1_166b:
                    return new Sim_EX1_166b();
                case cardIDEnum.EX1_066:
                    return new Sim_EX1_066();
                case cardIDEnum.EX1_355:
                    return new Sim_EX1_355();
                case cardIDEnum.EX1_534:
                    return new Sim_EX1_534();
                case cardIDEnum.EX1_162:
                    return new Sim_EX1_162();
                case cardIDEnum.EX1_363:
                    return new Sim_EX1_363();
                case cardIDEnum.EX1_164a:
                    return new Sim_EX1_164a();
                case cardIDEnum.CS2_188:
                    return new Sim_CS2_188();
                case cardIDEnum.EX1_016:
                    return new Sim_EX1_016();
                case cardIDEnum.EX1_603:
                    return new Sim_EX1_603();
                case cardIDEnum.EX1_238:
                    return new Sim_EX1_238();
                case cardIDEnum.EX1_166:
                    return new Sim_EX1_166();
                case cardIDEnum.DS1h_292:
                    return new Sim_DS1h_292();
                case cardIDEnum.DS1_183:
                    return new Sim_DS1_183();
                case cardIDEnum.EX1_076:
                    return new Sim_EX1_076();
                case cardIDEnum.EX1_048:
                    return new Sim_EX1_048();
                case cardIDEnum.FP1_026:
                    return new Sim_FP1_026();
                case cardIDEnum.CS2_074:
                    return new Sim_CS2_074();
                case cardIDEnum.FP1_027:
                    return new Sim_FP1_027();
                case cardIDEnum.EX1_323w:
                    return new Sim_EX1_323w();
                case cardIDEnum.EX1_129:
                    return new Sim_EX1_129();
                case cardIDEnum.EX1_405:
                    return new Sim_EX1_405();
                case cardIDEnum.EX1_317:
                    return new Sim_EX1_317();
                case cardIDEnum.EX1_606:
                    return new Sim_EX1_606();
                case cardIDEnum.FP1_006:
                    return new Sim_FP1_006();
                case cardIDEnum.NEW1_008:
                    return new Sim_NEW1_008();
                case cardIDEnum.CS2_119:
                    return new Sim_CS2_119();
                case cardIDEnum.CS2_121:
                    return new Sim_CS2_121();
                case cardIDEnum.CS1h_001:
                    return new Sim_CS1h_001();
                case cardIDEnum.EX1_tk34:
                    return new Sim_EX1_tk34();
                case cardIDEnum.NEW1_020:
                    return new Sim_NEW1_020();
                case cardIDEnum.CS2_196:
                    return new Sim_CS2_196();
                case cardIDEnum.EX1_312:
                    return new Sim_EX1_312();
                case cardIDEnum.FP1_022:
                    return new Sim_FP1_022();
                case cardIDEnum.EX1_160b:
                    return new Sim_EX1_160b();
                case cardIDEnum.EX1_563:
                    return new Sim_EX1_563();
                case cardIDEnum.FP1_031:
                    return new Sim_FP1_031();
                case cardIDEnum.NEW1_029:
                    return new Sim_NEW1_029();
                case cardIDEnum.CS1_129:
                    return new Sim_CS1_129();
                case cardIDEnum.HERO_03:
                    return new Sim_HERO_03();
                case cardIDEnum.Mekka4t:
                    return new Sim_Mekka4t();
                case cardIDEnum.EX1_158:
                    return new Sim_EX1_158();
                case cardIDEnum.NEW1_025:
                    return new Sim_NEW1_025();
                case cardIDEnum.FP1_012t:
                    return new Sim_FP1_012t();
                case cardIDEnum.EX1_083:
                    return new Sim_EX1_083();
                case cardIDEnum.EX1_295:
                    return new Sim_EX1_295();
                case cardIDEnum.EX1_407:
                    return new Sim_EX1_407();
                case cardIDEnum.NEW1_004:
                    return new Sim_NEW1_004();
                case cardIDEnum.FP1_019:
                    return new Sim_FP1_019();
                case cardIDEnum.PRO_001at:
                    return new Sim_PRO_001at();
                case cardIDEnum.EX1_625t:
                    return new Sim_EX1_625t();
                case cardIDEnum.EX1_014:
                    return new Sim_EX1_014();
                case cardIDEnum.CS2_097:
                    return new Sim_CS2_097();
                case cardIDEnum.EX1_558:
                    return new Sim_EX1_558();
                case cardIDEnum.EX1_tk29:
                    return new Sim_EX1_tk29();
                case cardIDEnum.CS2_186:
                    return new Sim_CS2_186();
                case cardIDEnum.EX1_084:
                    return new Sim_EX1_084();
                case cardIDEnum.NEW1_012:
                    return new Sim_NEW1_012();
                case cardIDEnum.FP1_014t:
                    return new Sim_FP1_014t();
                case cardIDEnum.EX1_578:
                    return new Sim_EX1_578();
                case cardIDEnum.CS2_221:
                    return new Sim_CS2_221();
                case cardIDEnum.EX1_019:
                    return new Sim_EX1_019();
                case cardIDEnum.FP1_019t:
                    return new Sim_FP1_019t();
                case cardIDEnum.EX1_132:
                    return new Sim_EX1_132();
                case cardIDEnum.EX1_284:
                    return new Sim_EX1_284();
                case cardIDEnum.EX1_105:
                    return new Sim_EX1_105();
                case cardIDEnum.NEW1_011:
                    return new Sim_NEW1_011();
                case cardIDEnum.EX1_017:
                    return new Sim_EX1_017();
                case cardIDEnum.EX1_249:
                    return new Sim_EX1_249();
                case cardIDEnum.FP1_002t:
                    return new Sim_FP1_002t();
                case cardIDEnum.EX1_313:
                    return new Sim_EX1_313();
                case cardIDEnum.EX1_155b:
                    return new Sim_EX1_155b();
                case cardIDEnum.NEW1_033:
                    return new Sim_NEW1_033();
                case cardIDEnum.CS2_106:
                    return new Sim_CS2_106();
                case cardIDEnum.FP1_018:
                    return new Sim_FP1_018();
                case cardIDEnum.DS1_233:
                    return new Sim_DS1_233();
                case cardIDEnum.DS1_175:
                    return new Sim_DS1_175();
                case cardIDEnum.NEW1_024:
                    return new Sim_NEW1_024();
                case cardIDEnum.CS2_189:
                    return new Sim_CS2_189();
                case cardIDEnum.NEW1_037:
                    return new Sim_NEW1_037();
                case cardIDEnum.EX1_414:
                    return new Sim_EX1_414();
                case cardIDEnum.EX1_538t:
                    return new Sim_EX1_538t();
                case cardIDEnum.EX1_586:
                    return new Sim_EX1_586();
                case cardIDEnum.EX1_310:
                    return new Sim_EX1_310();
                case cardIDEnum.NEW1_010:
                    return new Sim_NEW1_010();
                case cardIDEnum.EX1_534t:
                    return new Sim_EX1_534t();
                case cardIDEnum.FP1_028:
                    return new Sim_FP1_028();
                case cardIDEnum.EX1_604:
                    return new Sim_EX1_604();
                case cardIDEnum.EX1_160:
                    return new Sim_EX1_160();
                case cardIDEnum.EX1_165t1:
                    return new Sim_EX1_165t1();
                case cardIDEnum.CS2_062:
                    return new Sim_CS2_062();
                case cardIDEnum.CS2_155:
                    return new Sim_CS2_155();
                case cardIDEnum.CS2_213:
                    return new Sim_CS2_213();
                case cardIDEnum.CS2_004:
                    return new Sim_CS2_004();
                case cardIDEnum.CS2_023:
                    return new Sim_CS2_023();
                case cardIDEnum.EX1_164:
                    return new Sim_EX1_164();
                case cardIDEnum.EX1_009:
                    return new Sim_EX1_009();
                case cardIDEnum.FP1_007:
                    return new Sim_FP1_007();
                case cardIDEnum.EX1_345:
                    return new Sim_EX1_345();
                case cardIDEnum.EX1_116:
                    return new Sim_EX1_116();
                case cardIDEnum.EX1_399:
                    return new Sim_EX1_399();
                case cardIDEnum.EX1_587:
                    return new Sim_EX1_587();
                case cardIDEnum.EX1_571:
                    return new Sim_EX1_571();
                case cardIDEnum.EX1_335:
                    return new Sim_EX1_335();
                case cardIDEnum.HERO_08:
                    return new Sim_HERO_08();
                case cardIDEnum.EX1_166a:
                    return new Sim_EX1_166a();
                case cardIDEnum.EX1_finkle:
                    return new Sim_EX1_finkle();
                case cardIDEnum.EX1_164b:
                    return new Sim_EX1_164b();
                case cardIDEnum.EX1_283:
                    return new Sim_EX1_283();
                case cardIDEnum.EX1_339:
                    return new Sim_EX1_339();
                case cardIDEnum.EX1_531:
                    return new Sim_EX1_531();
                case cardIDEnum.EX1_134:
                    return new Sim_EX1_134();
                case cardIDEnum.EX1_350:
                    return new Sim_EX1_350();
                case cardIDEnum.EX1_308:
                    return new Sim_EX1_308();
                case cardIDEnum.CS2_197:
                    return new Sim_CS2_197();
                case cardIDEnum.skele21:
                    return new Sim_skele21();
                case cardIDEnum.FP1_013:
                    return new Sim_FP1_013();
                case cardIDEnum.EX1_509:
                    return new Sim_EX1_509();
                case cardIDEnum.EX1_612:
                    return new Sim_EX1_612();
                case cardIDEnum.EX1_021:
                    return new Sim_EX1_021();
                case cardIDEnum.CS2_226:
                    return new Sim_CS2_226();
                case cardIDEnum.EX1_608:
                    return new Sim_EX1_608();
                case cardIDEnum.EX1_624:
                    return new Sim_EX1_624();
                case cardIDEnum.EX1_616:
                    return new Sim_EX1_616();
                case cardIDEnum.EX1_008:
                    return new Sim_EX1_008();
                case cardIDEnum.PlaceholderCard:
                    return new Sim_PlaceholderCard();
                case cardIDEnum.EX1_045:
                    return new Sim_EX1_045();
                case cardIDEnum.EX1_015:
                    return new Sim_EX1_015();
                case cardIDEnum.CS2_171:
                    return new Sim_CS2_171();
                case cardIDEnum.CS2_041:
                    return new Sim_CS2_041();
                case cardIDEnum.EX1_128:
                    return new Sim_EX1_128();
                case cardIDEnum.CS2_112:
                    return new Sim_CS2_112();
                case cardIDEnum.HERO_07:
                    return new Sim_HERO_07();
                case cardIDEnum.EX1_412:
                    return new Sim_EX1_412();
                case cardIDEnum.CS2_117:
                    return new Sim_CS2_117();
                case cardIDEnum.EX1_562:
                    return new Sim_EX1_562();
                case cardIDEnum.EX1_055:
                    return new Sim_EX1_055();
                case cardIDEnum.FP1_012:
                    return new Sim_FP1_012();
                case cardIDEnum.EX1_317t:
                    return new Sim_EX1_317t();
                case cardIDEnum.EX1_278:
                    return new Sim_EX1_278();
                case cardIDEnum.CS2_tk1:
                    return new Sim_CS2_tk1();
                case cardIDEnum.EX1_590:
                    return new Sim_EX1_590();
                case cardIDEnum.CS1_130:
                    return new Sim_CS1_130();
                case cardIDEnum.NEW1_008b:
                    return new Sim_NEW1_008b();
                case cardIDEnum.EX1_365:
                    return new Sim_EX1_365();
                case cardIDEnum.CS2_141:
                    return new Sim_CS2_141();
                case cardIDEnum.PRO_001:
                    return new Sim_PRO_001();
                case cardIDEnum.CS2_173:
                    return new Sim_CS2_173();
                case cardIDEnum.CS2_017:
                    return new Sim_CS2_017();
                case cardIDEnum.EX1_392:
                    return new Sim_EX1_392();
                case cardIDEnum.EX1_593:
                    return new Sim_EX1_593();
                case cardIDEnum.EX1_049:
                    return new Sim_EX1_049();
                case cardIDEnum.EX1_002:
                    return new Sim_EX1_002();
                case cardIDEnum.CS2_056:
                    return new Sim_CS2_056();
                case cardIDEnum.EX1_596:
                    return new Sim_EX1_596();
                case cardIDEnum.EX1_136:
                    return new Sim_EX1_136();
                case cardIDEnum.EX1_323:
                    return new Sim_EX1_323();
                case cardIDEnum.CS2_073:
                    return new Sim_CS2_073();
                case cardIDEnum.EX1_001:
                    return new Sim_EX1_001();
                case cardIDEnum.EX1_044:
                    return new Sim_EX1_044();
                case cardIDEnum.Mekka4:
                    return new Sim_Mekka4();
                case cardIDEnum.CS2_142:
                    return new Sim_CS2_142();
                case cardIDEnum.EX1_573:
                    return new Sim_EX1_573();
                case cardIDEnum.FP1_009:
                    return new Sim_FP1_009();
                case cardIDEnum.CS2_050:
                    return new Sim_CS2_050();
                case cardIDEnum.EX1_390:
                    return new Sim_EX1_390();
                case cardIDEnum.EX1_610:
                    return new Sim_EX1_610();
                case cardIDEnum.hexfrog:
                    return new Sim_hexfrog();
                case cardIDEnum.CS2_082:
                    return new Sim_CS2_082();
                case cardIDEnum.NEW1_040:
                    return new Sim_NEW1_040();
                case cardIDEnum.DREAM_01:
                    return new Sim_DREAM_01();
                case cardIDEnum.EX1_595:
                    return new Sim_EX1_595();
                case cardIDEnum.CS2_013:
                    return new Sim_CS2_013();
                case cardIDEnum.CS2_077:
                    return new Sim_CS2_077();
                case cardIDEnum.NEW1_014:
                    return new Sim_NEW1_014();
                case cardIDEnum.GAME_002:
                    return new Sim_GAME_002();
                case cardIDEnum.EX1_165:
                    return new Sim_EX1_165();
                case cardIDEnum.CS2_013t:
                    return new Sim_CS2_013t();
                case cardIDEnum.EX1_tk11:
                    return new Sim_EX1_tk11();
                case cardIDEnum.EX1_591:
                    return new Sim_EX1_591();
                case cardIDEnum.EX1_549:
                    return new Sim_EX1_549();
                case cardIDEnum.CS2_045:
                    return new Sim_CS2_045();
                case cardIDEnum.CS2_237:
                    return new Sim_CS2_237();
                case cardIDEnum.CS2_027:
                    return new Sim_CS2_027();
                case cardIDEnum.CS2_101t:
                    return new Sim_CS2_101t();
                case cardIDEnum.CS2_063:
                    return new Sim_CS2_063();
                case cardIDEnum.EX1_145:
                    return new Sim_EX1_145();
                case cardIDEnum.EX1_110:
                    return new Sim_EX1_110();
                case cardIDEnum.EX1_408:
                    return new Sim_EX1_408();
                case cardIDEnum.EX1_544:
                    return new Sim_EX1_544();
                case cardIDEnum.CS2_151:
                    return new Sim_CS2_151();
                case cardIDEnum.CS2_088:
                    return new Sim_CS2_088();
                case cardIDEnum.EX1_057:
                    return new Sim_EX1_057();
                case cardIDEnum.FP1_020:
                    return new Sim_FP1_020();
                case cardIDEnum.CS2_169:
                    return new Sim_CS2_169();
                case cardIDEnum.EX1_573t:
                    return new Sim_EX1_573t();
                case cardIDEnum.EX1_323h:
                    return new Sim_EX1_323h();
                case cardIDEnum.EX1_tk9:
                    return new Sim_EX1_tk9();
                case cardIDEnum.CS2_037:
                    return new Sim_CS2_037();
                case cardIDEnum.CS2_007:
                    return new Sim_CS2_007();
                case cardIDEnum.CS2_227:
                    return new Sim_CS2_227();
                case cardIDEnum.NEW1_003:
                    return new Sim_NEW1_003();
                case cardIDEnum.GAME_006:
                    return new Sim_GAME_006();
                case cardIDEnum.EX1_320:
                    return new Sim_EX1_320();
                case cardIDEnum.EX1_097:
                    return new Sim_EX1_097();
                case cardIDEnum.tt_004:
                    return new Sim_tt_004();
                case cardIDEnum.EX1_096:
                    return new Sim_EX1_096();
                case cardIDEnum.EX1_126:
                    return new Sim_EX1_126();
                case cardIDEnum.EX1_577:
                    return new Sim_EX1_577();
                case cardIDEnum.EX1_319:
                    return new Sim_EX1_319();
                case cardIDEnum.EX1_611:
                    return new Sim_EX1_611();
                case cardIDEnum.CS2_146:
                    return new Sim_CS2_146();
                case cardIDEnum.EX1_154b:
                    return new Sim_EX1_154b();
                case cardIDEnum.skele11:
                    return new Sim_skele11();
                case cardIDEnum.EX1_165t2:
                    return new Sim_EX1_165t2();
                case cardIDEnum.CS2_172:
                    return new Sim_CS2_172();
                case cardIDEnum.CS2_114:
                    return new Sim_CS2_114();
                case cardIDEnum.CS1_069:
                    return new Sim_CS1_069();
                case cardIDEnum.EX1_173:
                    return new Sim_EX1_173();
                case cardIDEnum.CS1_042:
                    return new Sim_CS1_042();
                case cardIDEnum.EX1_506a:
                    return new Sim_EX1_506a();
                case cardIDEnum.EX1_298:
                    return new Sim_EX1_298();
                case cardIDEnum.CS2_104:
                    return new Sim_CS2_104();
                case cardIDEnum.FP1_001:
                    return new Sim_FP1_001();
                case cardIDEnum.HERO_02:
                    return new Sim_HERO_02();
                case cardIDEnum.CS2_051:
                    return new Sim_CS2_051();
                case cardIDEnum.NEW1_016:
                    return new Sim_NEW1_016();
                case cardIDEnum.EX1_033:
                    return new Sim_EX1_033();
                case cardIDEnum.EX1_028:
                    return new Sim_EX1_028();
                case cardIDEnum.EX1_621:
                    return new Sim_EX1_621();
                case cardIDEnum.EX1_554:
                    return new Sim_EX1_554();
                case cardIDEnum.EX1_091:
                    return new Sim_EX1_091();
                case cardIDEnum.FP1_017:
                    return new Sim_FP1_017();
                case cardIDEnum.EX1_409:
                    return new Sim_EX1_409();
                case cardIDEnum.EX1_410:
                    return new Sim_EX1_410();
                case cardIDEnum.CS2_039:
                    return new Sim_CS2_039();
                case cardIDEnum.EX1_557:
                    return new Sim_EX1_557();
                case cardIDEnum.DS1_070:
                    return new Sim_DS1_070();
                case cardIDEnum.CS2_033:
                    return new Sim_CS2_033();
                case cardIDEnum.EX1_536:
                    return new Sim_EX1_536();
                case cardIDEnum.EX1_559:
                    return new Sim_EX1_559();
                case cardIDEnum.CS2_052:
                    return new Sim_CS2_052();
                case cardIDEnum.EX1_539:
                    return new Sim_EX1_539();
                case cardIDEnum.EX1_575:
                    return new Sim_EX1_575();
                case cardIDEnum.CS2_083b:
                    return new Sim_CS2_083b();
                case cardIDEnum.CS2_061:
                    return new Sim_CS2_061();
                case cardIDEnum.NEW1_021:
                    return new Sim_NEW1_021();
                case cardIDEnum.DS1_055:
                    return new Sim_DS1_055();
                case cardIDEnum.EX1_625:
                    return new Sim_EX1_625();
                case cardIDEnum.CS2_026:
                    return new Sim_CS2_026();
                case cardIDEnum.EX1_294:
                    return new Sim_EX1_294();
                case cardIDEnum.EX1_287:
                    return new Sim_EX1_287();
                case cardIDEnum.EX1_625t2:
                    return new Sim_EX1_625t2();
                case cardIDEnum.CS2_118:
                    return new Sim_CS2_118();
                case cardIDEnum.CS2_124:
                    return new Sim_CS2_124();
                case cardIDEnum.Mekka3:
                    return new Sim_Mekka3();
                case cardIDEnum.EX1_112:
                    return new Sim_EX1_112();
                case cardIDEnum.FP1_011:
                    return new Sim_FP1_011();
                case cardIDEnum.HERO_04:
                    return new Sim_HERO_04();
                case cardIDEnum.EX1_607:
                    return new Sim_EX1_607();
                case cardIDEnum.DREAM_03:
                    return new Sim_DREAM_03();
                case cardIDEnum.FP1_003:
                    return new Sim_FP1_003();
                case cardIDEnum.CS2_105:
                    return new Sim_CS2_105();
                case cardIDEnum.FP1_002:
                    return new Sim_FP1_002();
                case cardIDEnum.EX1_567:
                    return new Sim_EX1_567();
                case cardIDEnum.FP1_008:
                    return new Sim_FP1_008();
                case cardIDEnum.DS1_184:
                    return new Sim_DS1_184();
                case cardIDEnum.CS2_029:
                    return new Sim_CS2_029();
                case cardIDEnum.GAME_005:
                    return new Sim_GAME_005();
                case cardIDEnum.CS2_187:
                    return new Sim_CS2_187();
                case cardIDEnum.EX1_020:
                    return new Sim_EX1_020();
                case cardIDEnum.EX1_011:
                    return new Sim_EX1_011();
                case cardIDEnum.CS2_057:
                    return new Sim_CS2_057();
                case cardIDEnum.EX1_274:
                    return new Sim_EX1_274();
                case cardIDEnum.EX1_306:
                    return new Sim_EX1_306();
                case cardIDEnum.EX1_170:
                    return new Sim_EX1_170();
                case cardIDEnum.EX1_617:
                    return new Sim_EX1_617();
                case cardIDEnum.CS2_101:
                    return new Sim_CS2_101();
                case cardIDEnum.FP1_015:
                    return new Sim_FP1_015();
                case cardIDEnum.CS2_005:
                    return new Sim_CS2_005();
                case cardIDEnum.EX1_537:
                    return new Sim_EX1_537();
                case cardIDEnum.EX1_384:
                    return new Sim_EX1_384();
                case cardIDEnum.EX1_362:
                    return new Sim_EX1_362();
                case cardIDEnum.EX1_301:
                    return new Sim_EX1_301();
                case cardIDEnum.CS2_235:
                    return new Sim_CS2_235();
                case cardIDEnum.EX1_029:
                    return new Sim_EX1_029();
                case cardIDEnum.CS2_042:
                    return new Sim_CS2_042();
                case cardIDEnum.EX1_155a:
                    return new Sim_EX1_155a();
                case cardIDEnum.CS2_102:
                    return new Sim_CS2_102();
                case cardIDEnum.EX1_609:
                    return new Sim_EX1_609();
                case cardIDEnum.NEW1_027:
                    return new Sim_NEW1_027();
                case cardIDEnum.EX1_165a:
                    return new Sim_EX1_165a();
                case cardIDEnum.EX1_570:
                    return new Sim_EX1_570();
                case cardIDEnum.EX1_131:
                    return new Sim_EX1_131();
                case cardIDEnum.EX1_556:
                    return new Sim_EX1_556();
                case cardIDEnum.EX1_543:
                    return new Sim_EX1_543();
                case cardIDEnum.NEW1_009:
                    return new Sim_NEW1_009();
                case cardIDEnum.EX1_100:
                    return new Sim_EX1_100();
                case cardIDEnum.EX1_573a:
                    return new Sim_EX1_573a();
                case cardIDEnum.CS2_084:
                    return new Sim_CS2_084();
                case cardIDEnum.EX1_582:
                    return new Sim_EX1_582();
                case cardIDEnum.EX1_043:
                    return new Sim_EX1_043();
                case cardIDEnum.EX1_050:
                    return new Sim_EX1_050();
                case cardIDEnum.FP1_005:
                    return new Sim_FP1_005();
                case cardIDEnum.EX1_620:
                    return new Sim_EX1_620();
                case cardIDEnum.EX1_303:
                    return new Sim_EX1_303();
                case cardIDEnum.HERO_09:
                    return new Sim_HERO_09();
                case cardIDEnum.EX1_067:
                    return new Sim_EX1_067();
                case cardIDEnum.EX1_277:
                    return new Sim_EX1_277();
                case cardIDEnum.Mekka2:
                    return new Sim_Mekka2();
                case cardIDEnum.FP1_024:
                    return new Sim_FP1_024();
                case cardIDEnum.FP1_030:
                    return new Sim_FP1_030();
                case cardIDEnum.EX1_178:
                    return new Sim_EX1_178();
                case cardIDEnum.CS2_222:
                    return new Sim_CS2_222();
                case cardIDEnum.EX1_160a:
                    return new Sim_EX1_160a();
                case cardIDEnum.CS2_012:
                    return new Sim_CS2_012();
                case cardIDEnum.EX1_246:
                    return new Sim_EX1_246();
                case cardIDEnum.EX1_572:
                    return new Sim_EX1_572();
                case cardIDEnum.EX1_089:
                    return new Sim_EX1_089();
                case cardIDEnum.CS2_059:
                    return new Sim_CS2_059();
                case cardIDEnum.EX1_279:
                    return new Sim_EX1_279();
                case cardIDEnum.CS2_168:
                    return new Sim_CS2_168();
                case cardIDEnum.tt_010:
                    return new Sim_tt_010();
                case cardIDEnum.NEW1_023:
                    return new Sim_NEW1_023();
                case cardIDEnum.CS2_075:
                    return new Sim_CS2_075();
                case cardIDEnum.EX1_316:
                    return new Sim_EX1_316();
                case cardIDEnum.CS2_025:
                    return new Sim_CS2_025();
                case cardIDEnum.CS2_234:
                    return new Sim_CS2_234();
                case cardIDEnum.EX1_130:
                    return new Sim_EX1_130();
                case cardIDEnum.CS2_064:
                    return new Sim_CS2_064();
                case cardIDEnum.EX1_161:
                    return new Sim_EX1_161();
                case cardIDEnum.CS2_049:
                    return new Sim_CS2_049();
                case cardIDEnum.EX1_154:
                    return new Sim_EX1_154();
                case cardIDEnum.EX1_080:
                    return new Sim_EX1_080();
                case cardIDEnum.NEW1_022:
                    return new Sim_NEW1_022();
                case cardIDEnum.EX1_251:
                    return new Sim_EX1_251();
                case cardIDEnum.FP1_025:
                    return new Sim_FP1_025();
                case cardIDEnum.EX1_371:
                    return new Sim_EX1_371();
                case cardIDEnum.CS2_mirror:
                    return new Sim_CS2_mirror();
                case cardIDEnum.EX1_594:
                    return new Sim_EX1_594();
                case cardIDEnum.EX1_560:
                    return new Sim_EX1_560();
                case cardIDEnum.CS2_236:
                    return new Sim_CS2_236();
                case cardIDEnum.EX1_402:
                    return new Sim_EX1_402();
                case cardIDEnum.EX1_506:
                    return new Sim_EX1_506();
                case cardIDEnum.DS1_178:
                    return new Sim_DS1_178();
                case cardIDEnum.EX1_315:
                    return new Sim_EX1_315();
                case cardIDEnum.CS2_094:
                    return new Sim_CS2_094();
                case cardIDEnum.NEW1_040t:
                    return new Sim_NEW1_040t();
                case cardIDEnum.CS2_131:
                    return new Sim_CS2_131();
                case cardIDEnum.EX1_082:
                    return new Sim_EX1_082();
                case cardIDEnum.CS2_093:
                    return new Sim_CS2_093();
                case cardIDEnum.CS2_boar:
                    return new Sim_CS2_boar();
                case cardIDEnum.NEW1_019:
                    return new Sim_NEW1_019();
                case cardIDEnum.EX1_289:
                    return new Sim_EX1_289();
                case cardIDEnum.EX1_025t:
                    return new Sim_EX1_025t();
                case cardIDEnum.EX1_398t:
                    return new Sim_EX1_398t();
                case cardIDEnum.CS2_091:
                    return new Sim_CS2_091();
                case cardIDEnum.EX1_241:
                    return new Sim_EX1_241();
                case cardIDEnum.EX1_085:
                    return new Sim_EX1_085();
                case cardIDEnum.CS2_200:
                    return new Sim_CS2_200();
                case cardIDEnum.CS2_034:
                    return new Sim_CS2_034();
                case cardIDEnum.EX1_583:
                    return new Sim_EX1_583();
                case cardIDEnum.EX1_584:
                    return new Sim_EX1_584();
                case cardIDEnum.EX1_155:
                    return new Sim_EX1_155();
                case cardIDEnum.EX1_622:
                    return new Sim_EX1_622();
                case cardIDEnum.CS2_203:
                    return new Sim_CS2_203();
                case cardIDEnum.EX1_124:
                    return new Sim_EX1_124();
                case cardIDEnum.EX1_379:
                    return new Sim_EX1_379();
                case cardIDEnum.EX1_032:
                    return new Sim_EX1_032();
                case cardIDEnum.EX1_391:
                    return new Sim_EX1_391();
                case cardIDEnum.EX1_366:
                    return new Sim_EX1_366();
                case cardIDEnum.EX1_400:
                    return new Sim_EX1_400();
                case cardIDEnum.EX1_614:
                    return new Sim_EX1_614();
                case cardIDEnum.EX1_561:
                    return new Sim_EX1_561();
                case cardIDEnum.EX1_332:
                    return new Sim_EX1_332();
                case cardIDEnum.HERO_05:
                    return new Sim_HERO_05();
                case cardIDEnum.CS2_065:
                    return new Sim_CS2_065();
                case cardIDEnum.ds1_whelptoken:
                    return new Sim_ds1_whelptoken();
                case cardIDEnum.CS2_032:
                    return new Sim_CS2_032();
                case cardIDEnum.CS2_120:
                    return new Sim_CS2_120();
                case cardIDEnum.EX1_247:
                    return new Sim_EX1_247();
                case cardIDEnum.EX1_154a:
                    return new Sim_EX1_154a();
                case cardIDEnum.EX1_554t:
                    return new Sim_EX1_554t();
                case cardIDEnum.NEW1_026t:
                    return new Sim_NEW1_026t();
                case cardIDEnum.EX1_623:
                    return new Sim_EX1_623();
                case cardIDEnum.EX1_383t:
                    return new Sim_EX1_383t();
                case cardIDEnum.EX1_597:
                    return new Sim_EX1_597();
                case cardIDEnum.EX1_130a:
                    return new Sim_EX1_130a();
                case cardIDEnum.CS2_011:
                    return new Sim_CS2_011();
                case cardIDEnum.EX1_169:
                    return new Sim_EX1_169();
                case cardIDEnum.EX1_tk33:
                    return new Sim_EX1_tk33();
                case cardIDEnum.EX1_250:
                    return new Sim_EX1_250();
                case cardIDEnum.EX1_564:
                    return new Sim_EX1_564();
                case cardIDEnum.EX1_349:
                    return new Sim_EX1_349();
                case cardIDEnum.EX1_102:
                    return new Sim_EX1_102();
                case cardIDEnum.EX1_058:
                    return new Sim_EX1_058();
                case cardIDEnum.EX1_243:
                    return new Sim_EX1_243();
                case cardIDEnum.PRO_001c:
                    return new Sim_PRO_001c();
                case cardIDEnum.EX1_116t:
                    return new Sim_EX1_116t();
                case cardIDEnum.FP1_029:
                    return new Sim_FP1_029();
                case cardIDEnum.CS2_089:
                    return new Sim_CS2_089();
                case cardIDEnum.EX1_248:
                    return new Sim_EX1_248();
                case cardIDEnum.CS2_122:
                    return new Sim_CS2_122();
                case cardIDEnum.EX1_393:
                    return new Sim_EX1_393();
                case cardIDEnum.CS2_232:
                    return new Sim_CS2_232();
                case cardIDEnum.EX1_165b:
                    return new Sim_EX1_165b();
                case cardIDEnum.NEW1_030:
                    return new Sim_NEW1_030();
                case cardIDEnum.CS2_150:
                    return new Sim_CS2_150();
                case cardIDEnum.CS2_152:
                    return new Sim_CS2_152();
                case cardIDEnum.EX1_160t:
                    return new Sim_EX1_160t();
                case cardIDEnum.CS2_127:
                    return new Sim_CS2_127();
                case cardIDEnum.DS1_188:
                    return new Sim_DS1_188();
                case CardDB.cardIDEnum.GVG_001:
                    return new Sim_GVG_001();
                case CardDB.cardIDEnum.GVG_002:
                    return new Sim_GVG_002();
                case CardDB.cardIDEnum.GVG_003:
                    return new Sim_GVG_003();
                case CardDB.cardIDEnum.GVG_004:
                    return new Sim_GVG_004();
                case CardDB.cardIDEnum.GVG_005:
                    return new Sim_GVG_005();
                case CardDB.cardIDEnum.GVG_006:
                    return new Sim_GVG_006();
                case CardDB.cardIDEnum.GVG_007:
                    return new Sim_GVG_007();
                case CardDB.cardIDEnum.GVG_008:
                    return new Sim_GVG_008();
                case CardDB.cardIDEnum.GVG_009:
                    return new Sim_GVG_009();
                case CardDB.cardIDEnum.GVG_010:
                    return new Sim_GVG_010();
                case CardDB.cardIDEnum.GVG_011:
                    return new Sim_GVG_011();
                case CardDB.cardIDEnum.GVG_012:
                    return new Sim_GVG_012();
                case CardDB.cardIDEnum.GVG_013:
                    return new Sim_GVG_013();
                case CardDB.cardIDEnum.GVG_014:
                    return new Sim_GVG_014();
                case CardDB.cardIDEnum.GVG_015:
                    return new Sim_GVG_015();
                case CardDB.cardIDEnum.GVG_016:
                    return new Sim_GVG_016();
                case CardDB.cardIDEnum.GVG_017:
                    return new Sim_GVG_017();
                case CardDB.cardIDEnum.GVG_018:
                    return new Sim_GVG_018();
                case CardDB.cardIDEnum.GVG_019:
                    return new Sim_GVG_019();
                case CardDB.cardIDEnum.GVG_020:
                    return new Sim_GVG_020();
                case CardDB.cardIDEnum.GVG_021:
                    return new Sim_GVG_021();
                case CardDB.cardIDEnum.GVG_022:
                    return new Sim_GVG_022();
                case CardDB.cardIDEnum.GVG_023:
                    return new Sim_GVG_023();
                case CardDB.cardIDEnum.GVG_024:
                    return new Sim_GVG_024();
                case CardDB.cardIDEnum.GVG_025:
                    return new Sim_GVG_025();
                case CardDB.cardIDEnum.GVG_026:
                    return new Sim_GVG_026();
                case CardDB.cardIDEnum.GVG_027:
                    return new Sim_GVG_027();
                case CardDB.cardIDEnum.GVG_028:
                    return new Sim_GVG_028();
                case CardDB.cardIDEnum.GVG_028t:
                    return new Sim_GVG_028t();
                case CardDB.cardIDEnum.GVG_029:
                    return new Sim_GVG_029();
                case CardDB.cardIDEnum.GVG_030:
                    return new Sim_GVG_030();
                case CardDB.cardIDEnum.GVG_030a:
                    return new Sim_GVG_030a();
                case CardDB.cardIDEnum.GVG_030b:
                    return new Sim_GVG_030b();
                case CardDB.cardIDEnum.GVG_031:
                    return new Sim_GVG_031();
                case CardDB.cardIDEnum.GVG_032:
                    return new Sim_GVG_032();
                case CardDB.cardIDEnum.GVG_032a:
                    return new Sim_GVG_032a();
                case CardDB.cardIDEnum.GVG_032b:
                    return new Sim_GVG_032b();
                case CardDB.cardIDEnum.GVG_033:
                    return new Sim_GVG_033();
                case CardDB.cardIDEnum.GVG_034:
                    return new Sim_GVG_034();
                case CardDB.cardIDEnum.GVG_035:
                    return new Sim_GVG_035();
                case CardDB.cardIDEnum.GVG_036:
                    return new Sim_GVG_036();
                case CardDB.cardIDEnum.GVG_037:
                    return new Sim_GVG_037();
                case CardDB.cardIDEnum.GVG_038:
                    return new Sim_GVG_038();
                case CardDB.cardIDEnum.GVG_039:
                    return new Sim_GVG_039();
                case CardDB.cardIDEnum.GVG_040:
                    return new Sim_GVG_040();
                case CardDB.cardIDEnum.GVG_041:
                    return new Sim_GVG_041();
                case CardDB.cardIDEnum.GVG_041a:
                    return new Sim_GVG_041a();
                case CardDB.cardIDEnum.GVG_041b:
                    return new Sim_GVG_041b();
                case CardDB.cardIDEnum.GVG_042:
                    return new Sim_GVG_042();
                case CardDB.cardIDEnum.GVG_043:
                    return new Sim_GVG_043();
                case CardDB.cardIDEnum.GVG_044:
                    return new Sim_GVG_044();
                case CardDB.cardIDEnum.GVG_045:
                    return new Sim_GVG_045();
                case CardDB.cardIDEnum.GVG_045t:
                    return new Sim_GVG_045t();
                case CardDB.cardIDEnum.GVG_046:
                    return new Sim_GVG_046();
                case CardDB.cardIDEnum.GVG_047:
                    return new Sim_GVG_047();
                case CardDB.cardIDEnum.GVG_048:
                    return new Sim_GVG_048();
                case CardDB.cardIDEnum.GVG_049:
                    return new Sim_GVG_049();
                case CardDB.cardIDEnum.GVG_050:
                    return new Sim_GVG_050();
                case CardDB.cardIDEnum.GVG_051:
                    return new Sim_GVG_051();
                case CardDB.cardIDEnum.GVG_052:
                    return new Sim_GVG_052();
                case CardDB.cardIDEnum.GVG_053:
                    return new Sim_GVG_053();
                case CardDB.cardIDEnum.GVG_054:
                    return new Sim_GVG_054();
                case CardDB.cardIDEnum.GVG_055:
                    return new Sim_GVG_055();
                case CardDB.cardIDEnum.GVG_056:
                    return new Sim_GVG_056();
                case CardDB.cardIDEnum.GVG_056t:
                    return new Sim_GVG_056t();
                case CardDB.cardIDEnum.GVG_057:
                    return new Sim_GVG_057();
                case CardDB.cardIDEnum.GVG_058:
                    return new Sim_GVG_058();
                case CardDB.cardIDEnum.GVG_059:
                    return new Sim_GVG_059();
                case CardDB.cardIDEnum.GVG_060:
                    return new Sim_GVG_060();
                case CardDB.cardIDEnum.GVG_061:
                    return new Sim_GVG_061();
                case CardDB.cardIDEnum.GVG_062:
                    return new Sim_GVG_062();
                case CardDB.cardIDEnum.GVG_063:
                    return new Sim_GVG_063();
                case CardDB.cardIDEnum.GVG_064:
                    return new Sim_GVG_064();
                case CardDB.cardIDEnum.GVG_065:
                    return new Sim_GVG_065();
                case CardDB.cardIDEnum.GVG_066:
                    return new Sim_GVG_066();
                case CardDB.cardIDEnum.GVG_067:
                    return new Sim_GVG_067();
                case CardDB.cardIDEnum.GVG_068:
                    return new Sim_GVG_068();
                case CardDB.cardIDEnum.GVG_069:
                    return new Sim_GVG_069();
                case CardDB.cardIDEnum.GVG_070:
                    return new Sim_GVG_070();
                case CardDB.cardIDEnum.GVG_071:
                    return new Sim_GVG_071();
                case CardDB.cardIDEnum.GVG_072:
                    return new Sim_GVG_072();
                case CardDB.cardIDEnum.GVG_073:
                    return new Sim_GVG_073();
                case CardDB.cardIDEnum.GVG_074:
                    return new Sim_GVG_074();
                case CardDB.cardIDEnum.GVG_075:
                    return new Sim_GVG_075();
                case CardDB.cardIDEnum.GVG_076:
                    return new Sim_GVG_076();
                case CardDB.cardIDEnum.GVG_077:
                    return new Sim_GVG_077();
                case CardDB.cardIDEnum.GVG_078:
                    return new Sim_GVG_078();
                case CardDB.cardIDEnum.GVG_079:
                    return new Sim_GVG_079();
                case CardDB.cardIDEnum.GVG_080:
                    return new Sim_GVG_080();
                case CardDB.cardIDEnum.GVG_080t:
                    return new Sim_GVG_080t();
                case CardDB.cardIDEnum.GVG_081:
                    return new Sim_GVG_081();
                case CardDB.cardIDEnum.GVG_082:
                    return new Sim_GVG_082();
                case CardDB.cardIDEnum.GVG_083:
                    return new Sim_GVG_083();
                case CardDB.cardIDEnum.GVG_084:
                    return new Sim_GVG_084();
                case CardDB.cardIDEnum.GVG_085:
                    return new Sim_GVG_085();
                case CardDB.cardIDEnum.GVG_086:
                    return new Sim_GVG_086();
                case CardDB.cardIDEnum.GVG_087:
                    return new Sim_GVG_087();
                case CardDB.cardIDEnum.GVG_088:
                    return new Sim_GVG_088();
                case CardDB.cardIDEnum.GVG_089:
                    return new Sim_GVG_089();
                case CardDB.cardIDEnum.GVG_090:
                    return new Sim_GVG_090();
                case CardDB.cardIDEnum.GVG_091:
                    return new Sim_GVG_091();
                case CardDB.cardIDEnum.GVG_092:
                    return new Sim_GVG_092();
                case CardDB.cardIDEnum.GVG_093:
                    return new Sim_GVG_093();
                case CardDB.cardIDEnum.GVG_094:
                    return new Sim_GVG_094();
                case CardDB.cardIDEnum.GVG_095:
                    return new Sim_GVG_095();
                case CardDB.cardIDEnum.GVG_096:
                    return new Sim_GVG_096();
                case CardDB.cardIDEnum.GVG_097:
                    return new Sim_GVG_097();
                case CardDB.cardIDEnum.GVG_098:
                    return new Sim_GVG_098();
                case CardDB.cardIDEnum.GVG_099:
                    return new Sim_GVG_099();
                case CardDB.cardIDEnum.GVG_100:
                    return new Sim_GVG_100();
                case CardDB.cardIDEnum.GVG_101:
                    return new Sim_GVG_101();
                case CardDB.cardIDEnum.GVG_102:
                    return new Sim_GVG_102();
                case CardDB.cardIDEnum.GVG_103:
                    return new Sim_GVG_103();
                case CardDB.cardIDEnum.GVG_104:
                    return new Sim_GVG_104();
                case CardDB.cardIDEnum.GVG_105:
                    return new Sim_GVG_105();
                case CardDB.cardIDEnum.GVG_106:
                    return new Sim_GVG_106();
                case CardDB.cardIDEnum.GVG_107:
                    return new Sim_GVG_107();
                case CardDB.cardIDEnum.GVG_108:
                    return new Sim_GVG_108();
                case CardDB.cardIDEnum.GVG_109:
                    return new Sim_GVG_109();
                case CardDB.cardIDEnum.GVG_110:
                    return new Sim_GVG_110();
                case CardDB.cardIDEnum.GVG_110t:
                    return new Sim_GVG_110t();
                case CardDB.cardIDEnum.GVG_111:
                    return new Sim_GVG_111();
                case CardDB.cardIDEnum.GVG_111t:
                    return new Sim_GVG_111t();
                case CardDB.cardIDEnum.GVG_112:
                    return new Sim_GVG_112();
                case CardDB.cardIDEnum.GVG_113:
                    return new Sim_GVG_113();
                case CardDB.cardIDEnum.GVG_114:
                    return new Sim_GVG_114();
                case CardDB.cardIDEnum.GVG_115:
                    return new Sim_GVG_115();
                case CardDB.cardIDEnum.GVG_116:
                    return new Sim_GVG_116();
                case CardDB.cardIDEnum.GVG_117:
                    return new Sim_GVG_117();
                case CardDB.cardIDEnum.GVG_118:
                    return new Sim_GVG_118();
                case CardDB.cardIDEnum.GVG_119:
                    return new Sim_GVG_119();
                case CardDB.cardIDEnum.GVG_120:
                    return new Sim_GVG_120();
                case CardDB.cardIDEnum.GVG_121:
                    return new Sim_GVG_121();
                case CardDB.cardIDEnum.GVG_122:
                    return new Sim_GVG_122();
                case CardDB.cardIDEnum.GVG_123:
                    return new Sim_GVG_123();
                case CardDB.cardIDEnum.PART_001:
                    return new Sim_PART_001();
                case CardDB.cardIDEnum.PART_002:
                    return new Sim_PART_002();
                case CardDB.cardIDEnum.PART_003:
                    return new Sim_PART_003();
                case CardDB.cardIDEnum.PART_004:
                    return new Sim_PART_004();
                case CardDB.cardIDEnum.PART_005:
                    return new Sim_PART_005();
                case CardDB.cardIDEnum.PART_006:
                    return new Sim_PART_006();
                case CardDB.cardIDEnum.PART_007:
                    return new Sim_PART_007();
                case cardIDEnum.NAX1_05:
                    return new Sim_NAX1_05();
                case cardIDEnum.NAX11_04:
                    return new Sim_NAX11_04();
                case cardIDEnum.NAX14_04:
                    return new Sim_NAX14_04();
                case cardIDEnum.NAX15_02:
                    return new Sim_NAX15_02();
                case cardIDEnum.NAX15_02H:
                    return new Sim_NAX15_02H();
                case cardIDEnum.NAX2_03:
                    return new Sim_NAX2_03();
                case cardIDEnum.NAX2_03H:
                    return new Sim_NAX2_03H();
                case cardIDEnum.NAX5_03:
                    return new Sim_NAX5_03();
                case cardIDEnum.NAX6_03t:
                    return new Sim_NAX6_03t();
                case cardIDEnum.NAX8_02:
                    return new Sim_NAX8_02();
                case cardIDEnum.NAX8_02H:
                    return new Sim_NAX8_02H();
                case cardIDEnum.NAX8_03:
                    return new Sim_NAX8_03();
                case cardIDEnum.NAX8_03t:
                    return new Sim_NAX8_03t();
                case cardIDEnum.NAX8_04:
                    return new Sim_NAX8_04();
                case cardIDEnum.NAX8_04t:
                    return new Sim_NAX8_04t();
                case cardIDEnum.NAX8_05:
                    return new Sim_NAX8_05();
                case cardIDEnum.NAX8_05t:
                    return new Sim_NAX8_05t();
                case cardIDEnum.NAXM_001:
                    return new Sim_NAXM_001();
                case cardIDEnum.BRM_001:
                    return new Sim_BRM_001();
                case cardIDEnum.BRM_002:
                    return new Sim_BRM_002();
                case cardIDEnum.BRM_003:
                    return new Sim_BRM_003();
                case cardIDEnum.BRM_004:
                    return new Sim_BRM_004();
                case cardIDEnum.BRM_004t:
                    return new Sim_BRM_004t();
                case cardIDEnum.BRM_005:
                    return new Sim_BRM_005();
                case cardIDEnum.BRM_006:
                    return new Sim_BRM_006();
                case cardIDEnum.BRM_006t:
                    return new Sim_BRM_006t();
                case cardIDEnum.BRM_007:
                    return new Sim_BRM_007();
                case cardIDEnum.BRM_008:
                    return new Sim_BRM_008();
                case cardIDEnum.BRM_009:
                    return new Sim_BRM_009();
                case cardIDEnum.BRM_010:
                    return new Sim_BRM_010();
                case cardIDEnum.BRM_010a:
                    return new Sim_BRM_010a();
                case cardIDEnum.BRM_010b:
                    return new Sim_BRM_010b();
                case cardIDEnum.BRM_010t:
                    return new Sim_BRM_010t();
                case cardIDEnum.BRM_010t2:
                    return new Sim_BRM_010t2();
                case cardIDEnum.BRM_011:
                    return new Sim_BRM_011();
                case cardIDEnum.BRM_012:
                    return new Sim_BRM_012();
                case cardIDEnum.BRM_013:
                    return new Sim_BRM_013();
                case cardIDEnum.BRM_014:
                    return new Sim_BRM_014();
                case cardIDEnum.BRM_015:
                    return new Sim_BRM_015();
                case cardIDEnum.BRM_016:
                    return new Sim_BRM_016();
                case cardIDEnum.BRM_017:
                    return new Sim_BRM_017();
                case cardIDEnum.BRM_018:
                    return new Sim_BRM_018();
                case cardIDEnum.BRM_019:
                    return new Sim_BRM_019();
                case cardIDEnum.BRM_020:
                    return new Sim_BRM_020();
                case cardIDEnum.BRM_022:
                    return new Sim_BRM_022();
                case cardIDEnum.BRM_022t:
                    return new Sim_BRM_022t();
                case cardIDEnum.BRM_024:
                    return new Sim_BRM_024();
                case cardIDEnum.BRM_025:
                    return new Sim_BRM_025();
                case cardIDEnum.BRM_026:
                    return new Sim_BRM_026();
                case cardIDEnum.BRM_027:
                    return new Sim_BRM_027();
                case cardIDEnum.BRM_027p:
                    return new Sim_BRM_027p();
                case cardIDEnum.BRM_028:
                    return new Sim_BRM_028();
                case cardIDEnum.BRM_029:
                    return new Sim_BRM_029();
                case cardIDEnum.BRM_030:
                    return new Sim_BRM_030();
                case cardIDEnum.BRM_031:
                    return new Sim_BRM_031();
                case cardIDEnum.BRM_033:
                    return new Sim_BRM_033();
                case cardIDEnum.BRM_034:
                    return new Sim_BRM_034();
            }

            return new SimTemplate();
        }

        private void enumCreator()
        {
            //call this, if carddb.txt was changed, to get latest public enum cardIDEnum
            HRSim.Helpfunctions.Instance.logg("public enum cardIDEnum");
            HRSim.Helpfunctions.Instance.logg("{");
            HRSim.Helpfunctions.Instance.logg("None,");
            foreach (string cardid in this.allCardIDS)
            {
                HRSim.Helpfunctions.Instance.logg(cardid + ",");
            }
            HRSim.Helpfunctions.Instance.logg("}");



            HRSim.Helpfunctions.Instance.logg("public cardIDEnum cardIdstringToEnum(string s)");
            HRSim.Helpfunctions.Instance.logg("{");
            foreach (string cardid in this.allCardIDS)
            {
                HRSim.Helpfunctions.Instance.logg("if(s==\"" + cardid + "\") return CardDB.cardIDEnum." + cardid + ";");
            }
            HRSim.Helpfunctions.Instance.logg("return CardDB.cardIDEnum.None;");
            HRSim.Helpfunctions.Instance.logg("}");

            List<string> namelist = new List<string>();

            foreach (string cardid in this.namelist)
            {
                if (namelist.Contains(cardid)) continue;
                namelist.Add(cardid);
            }


            HRSim.Helpfunctions.Instance.logg("public enum cardName");
            HRSim.Helpfunctions.Instance.logg("{");
            foreach (string cardid in namelist)
            {
                HRSim.Helpfunctions.Instance.logg(cardid + ",");
            }
            HRSim.Helpfunctions.Instance.logg("}");

            HRSim.Helpfunctions.Instance.logg("public cardName cardNamestringToEnum(string s)");
            HRSim.Helpfunctions.Instance.logg("{");
            foreach (string cardid in namelist)
            {
                HRSim.Helpfunctions.Instance.logg("if(s==\"" + cardid + "\") return CardDB.cardName." + cardid + ";");
            }
            HRSim.Helpfunctions.Instance.logg("return CardDB.cardName.unknown;");
            HRSim.Helpfunctions.Instance.logg("}");

            // simcard creator:

            HRSim.Helpfunctions.Instance.logg("public SimTemplate getSimCard(cardIDEnum id)");
            HRSim.Helpfunctions.Instance.logg("{");
            foreach (string cardid in this.allCardIDS)
            {
                HRSim.Helpfunctions.Instance.logg("if(id == CardDB.cardIDEnum." + cardid + ") return new Sim_" + cardid + "();");
            }
            HRSim.Helpfunctions.Instance.logg("return new SimTemplate();");
            HRSim.Helpfunctions.Instance.logg("}");

        }

        private void setupEnrageDatabase()
        {
            enrageDatabase.Add(CardDB.cardName.spitefulsmith, 2);
            enrageDatabase.Add(CardDB.cardName.angrychicken, 5);
            enrageDatabase.Add(CardDB.cardName.taurenwarrior, 3);
            enrageDatabase.Add(CardDB.cardName.amaniberserker, 3);
            enrageDatabase.Add(CardDB.cardName.ragingworgen, 2);
            enrageDatabase.Add(CardDB.cardName.grommashhellscream, 6);
            enrageDatabase.Add(CardDB.cardName.warbot, 1);
        }

        private void setupHealDatabase()
        {
            HealAllDatabase.Add(CardDB.cardName.holynova, 2);//to all own minions
            HealAllDatabase.Add(CardDB.cardName.circleofhealing, 4);//allminions
            HealAllDatabase.Add(CardDB.cardName.darkscalehealer, 2);//all friends
            HealAllDatabase.Add(CardDB.cardName.treeoflife, 1000);//all friends

            HealHeroDatabase.Add(CardDB.cardName.drainlife, 2);//tohero
            HealHeroDatabase.Add(CardDB.cardName.guardianofkings, 6);//tohero
            HealHeroDatabase.Add(CardDB.cardName.holyfire, 5);//tohero
            HealHeroDatabase.Add(CardDB.cardName.priestessofelune, 4);//tohero
            HealHeroDatabase.Add(CardDB.cardName.sacrificialpact, 5);//tohero
            HealHeroDatabase.Add(CardDB.cardName.siphonsoul, 3); //tohero
            HealHeroDatabase.Add(CardDB.cardName.sealoflight, 4); //tohero
            HealHeroDatabase.Add(CardDB.cardName.antiquehealbot, 8); //tohero

            HealTargetDatabase.Add(CardDB.cardName.lightofthenaaru, 3);
            HealTargetDatabase.Add(CardDB.cardName.ancestralhealing, 1000);
            HealTargetDatabase.Add(CardDB.cardName.ancientsecrets, 5);
            HealTargetDatabase.Add(CardDB.cardName.holylight, 6);
            HealTargetDatabase.Add(CardDB.cardName.earthenringfarseer, 3);
            HealTargetDatabase.Add(CardDB.cardName.healingtouch, 8);
            HealTargetDatabase.Add(CardDB.cardName.layonhands, 8);
            HealTargetDatabase.Add(CardDB.cardName.lesserheal, 2);
            HealTargetDatabase.Add(CardDB.cardName.voodoodoctor, 2);
            HealTargetDatabase.Add(CardDB.cardName.willofmukla, 8);
            HealTargetDatabase.Add(CardDB.cardName.ancientoflore, 5);
            //HealTargetDatabase.Add(CardDB.cardName.divinespirit, 2);
        }

        private void setupDamageDatabase()
        {
            DamageAllDatabase.Add(CardDB.cardName.abomination, 2);
            DamageAllDatabase.Add(CardDB.cardName.barongeddon, 2);
            DamageAllDatabase.Add(CardDB.cardName.damagereflector, 1);
            DamageAllDatabase.Add(CardDB.cardName.deathsbite, 1);
            DamageAllDatabase.Add(CardDB.cardName.dreadinfernal, 1);
            DamageAllDatabase.Add(CardDB.cardName.explosivesheep, 2);
            //DamageAllDatabase.Add(CardDB.cardName.flameleviathan, 2);
            DamageAllDatabase.Add(CardDB.cardName.hellfire, 3);
            DamageAllDatabase.Add(CardDB.cardName.lightbomb, 5);
            DamageAllDatabase.Add(CardDB.cardName.poisoncloud, 1);//todo 1 or 2
            DamageAllDatabase.Add(CardDB.cardName.scarletpurifier, 2);
            DamageAllDatabase.Add(CardDB.cardName.unstableghoul, 1);
            DamageAllDatabase.Add(CardDB.cardName.whirlwind, 1);
            DamageAllDatabase.Add(CardDB.cardName.wildpyromancer, 1);
            DamageAllDatabase.Add(CardDB.cardName.yseraawakens, 5);
            DamageAllDatabase.Add(CardDB.cardName.revenge, 1);
            DamageAllDatabase.Add(CardDB.cardName.demonwrath, 1);

            DamageAllEnemysDatabase.Add(CardDB.cardName.arcaneexplosion, 1);
            DamageAllEnemysDatabase.Add(CardDB.cardName.bladeflurry, 1);
            DamageAllEnemysDatabase.Add(CardDB.cardName.blizzard, 2);
            DamageAllEnemysDatabase.Add(CardDB.cardName.consecration, 2);
            DamageAllEnemysDatabase.Add(CardDB.cardName.fanofknives, 1);
            DamageAllEnemysDatabase.Add(CardDB.cardName.flamestrike, 4);
            DamageAllEnemysDatabase.Add(CardDB.cardName.holynova, 2);
            DamageAllEnemysDatabase.Add(CardDB.cardName.lightningstorm, 2);
            DamageAllEnemysDatabase.Add(CardDB.cardName.locustswarm, 3);
            DamageAllEnemysDatabase.Add(CardDB.cardName.shadowflame, 2);
            DamageAllEnemysDatabase.Add(CardDB.cardName.sporeburst, 1);
            DamageAllEnemysDatabase.Add(CardDB.cardName.starfall, 2);
            DamageAllEnemysDatabase.Add(CardDB.cardName.stomp, 2);
            DamageAllEnemysDatabase.Add(CardDB.cardName.swipe, 1);
            DamageAllEnemysDatabase.Add(CardDB.cardName.darkironskulker, 2);

            DamageHeroDatabase.Add(CardDB.cardName.headcrack, 2);
            DamageHeroDatabase.Add(CardDB.cardName.lepergnome, 2);
            DamageHeroDatabase.Add(CardDB.cardName.mindblast, 5);
            DamageHeroDatabase.Add(CardDB.cardName.nightblade, 3);
            DamageHeroDatabase.Add(CardDB.cardName.purecold, 8);
            DamageHeroDatabase.Add(CardDB.cardName.shadowbomber, 3);
            DamageHeroDatabase.Add(CardDB.cardName.sinisterstrike, 3);

            DamageRandomDatabase.Add(CardDB.cardName.arcanemissiles, 1);
            DamageRandomDatabase.Add(CardDB.cardName.avengingwrath, 1);
            DamageRandomDatabase.Add(CardDB.cardName.bomblobber, 4);
            DamageRandomDatabase.Add(CardDB.cardName.boombot, 1);
            DamageRandomDatabase.Add(CardDB.cardName.bouncingblade, 1);
            DamageRandomDatabase.Add(CardDB.cardName.cleave, 2);
            DamageRandomDatabase.Add(CardDB.cardName.demolisher, 2);
            DamageRandomDatabase.Add(CardDB.cardName.flamecannon, 4);
            DamageRandomDatabase.Add(CardDB.cardName.forkedlightning, 2);
            DamageRandomDatabase.Add(CardDB.cardName.goblinblastmage, 1);
            DamageRandomDatabase.Add(CardDB.cardName.knifejuggler, 1);
            DamageRandomDatabase.Add(CardDB.cardName.madbomber, 1);
            DamageRandomDatabase.Add(CardDB.cardName.madderbomber, 1);
            DamageRandomDatabase.Add(CardDB.cardName.multishot, 3);
            DamageRandomDatabase.Add(CardDB.cardName.ragnarosthefirelord, 8);
            DamageRandomDatabase.Add(CardDB.cardName.shadowboxer, 1);
            DamageRandomDatabase.Add(CardDB.cardName.shipscannon, 2);
            DamageRandomDatabase.Add(CardDB.cardName.flamewaker, 2);

            DamageTargetDatabase.Add(CardDB.cardName.arcaneshot, 2);
            DamageTargetDatabase.Add(CardDB.cardName.backstab, 2);
            DamageTargetDatabase.Add(CardDB.cardName.baneofdoom, 2);
            DamageTargetDatabase.Add(CardDB.cardName.barreltoss, 2);
            DamageTargetDatabase.Add(CardDB.cardName.betrayal, 2);
            DamageTargetDatabase.Add(CardDB.cardName.cobrashot, 3);
            DamageTargetDatabase.Add(CardDB.cardName.coneofcold, 1);
            DamageTargetDatabase.Add(CardDB.cardName.crackle, 3);
            DamageTargetDatabase.Add(CardDB.cardName.damage1, 1);
            DamageTargetDatabase.Add(CardDB.cardName.damage5, 5);
            DamageTargetDatabase.Add(CardDB.cardName.darkbomb, 3);
            DamageTargetDatabase.Add(CardDB.cardName.drainlife, 2);
            DamageTargetDatabase.Add(CardDB.cardName.elvenarcher, 1);
            DamageTargetDatabase.Add(CardDB.cardName.eviscerate, 2);
            DamageTargetDatabase.Add(CardDB.cardName.explosiveshot, 5);
            DamageTargetDatabase.Add(CardDB.cardName.felcannon, 2);
            DamageTargetDatabase.Add(CardDB.cardName.fireball, 6);
            DamageTargetDatabase.Add(CardDB.cardName.fireblast, 1);
            DamageTargetDatabase.Add(CardDB.cardName.fireelemental, 3);
            DamageTargetDatabase.Add(CardDB.cardName.frostbolt, 3);
            DamageTargetDatabase.Add(CardDB.cardName.frostshock, 1);
            DamageTargetDatabase.Add(CardDB.cardName.hoggersmash, 4);
            DamageTargetDatabase.Add(CardDB.cardName.holyfire, 5);
            DamageTargetDatabase.Add(CardDB.cardName.holysmite, 2);
            DamageTargetDatabase.Add(CardDB.cardName.icelance, 4);//only if iced
            DamageTargetDatabase.Add(CardDB.cardName.implosion, 2);
            DamageTargetDatabase.Add(CardDB.cardName.ironforgerifleman, 1);
            DamageTargetDatabase.Add(CardDB.cardName.keeperofthegrove, 2); // or silence
            DamageTargetDatabase.Add(CardDB.cardName.killcommand, 3);//or 5
            DamageTargetDatabase.Add(CardDB.cardName.lavaburst, 5);
            DamageTargetDatabase.Add(CardDB.cardName.lightningbolt, 3);
            DamageTargetDatabase.Add(CardDB.cardName.mindshatter, 3);
            DamageTargetDatabase.Add(CardDB.cardName.mindspike, 2);
            DamageTargetDatabase.Add(CardDB.cardName.moonfire, 1);
            DamageTargetDatabase.Add(CardDB.cardName.mortalcoil, 1);
            DamageTargetDatabase.Add(CardDB.cardName.mortalstrike, 4);
            DamageTargetDatabase.Add(CardDB.cardName.perditionsblade, 1);
            DamageTargetDatabase.Add(CardDB.cardName.pyroblast, 10);
            DamageTargetDatabase.Add(CardDB.cardName.shadowbolt, 4);
            DamageTargetDatabase.Add(CardDB.cardName.shadowform, 2);
            DamageTargetDatabase.Add(CardDB.cardName.shotgunblast, 1);
            DamageTargetDatabase.Add(CardDB.cardName.si7agent, 2);
            DamageTargetDatabase.Add(CardDB.cardName.starfall, 5);//2 to all enemy
            DamageTargetDatabase.Add(CardDB.cardName.starfire, 5);//draw a card
            DamageTargetDatabase.Add(CardDB.cardName.steadyshot, 2);//or 1 + card
            DamageTargetDatabase.Add(CardDB.cardName.stormpikecommando, 2);
            DamageTargetDatabase.Add(CardDB.cardName.swipe, 4);//1 to others
            DamageTargetDatabase.Add(CardDB.cardName.wrath, 1);//todo 3 or 1+card
            DamageTargetDatabase.Add(CardDB.cardName.dragonsbreath, 4);
            DamageTargetDatabase.Add(CardDB.cardName.lavashock, 2);
            DamageTargetDatabase.Add(CardDB.cardName.blackwingcorruptor, 3);//if dragon in hand

            DamageTargetSpecialDatabase.Add(CardDB.cardName.crueltaskmaster, 1); // gives 2 attack
            DamageTargetSpecialDatabase.Add(CardDB.cardName.deathbloom, 5);
            DamageTargetSpecialDatabase.Add(CardDB.cardName.demonfire, 2); // friendly demon get +2/+2
            DamageTargetSpecialDatabase.Add(CardDB.cardName.demonheart, 5);
            DamageTargetSpecialDatabase.Add(CardDB.cardName.earthshock, 1); //SILENCE /good for raggy etc or iced
            DamageTargetSpecialDatabase.Add(CardDB.cardName.hammerofwrath, 3); //draw a card
            DamageTargetSpecialDatabase.Add(CardDB.cardName.holywrath, 2);//draw a card
            DamageTargetSpecialDatabase.Add(CardDB.cardName.innerrage, 1); // gives 2 attack
            DamageTargetSpecialDatabase.Add(CardDB.cardName.roguesdoit, 4);//draw a card
            DamageTargetSpecialDatabase.Add(CardDB.cardName.savagery, 1);//dmg=herodamage
            DamageTargetSpecialDatabase.Add(CardDB.cardName.shieldslam, 1);//dmg=armor
            DamageTargetSpecialDatabase.Add(CardDB.cardName.shiv, 1);//draw a card
            DamageTargetSpecialDatabase.Add(CardDB.cardName.slam, 2);//draw card if it survives
            DamageTargetSpecialDatabase.Add(CardDB.cardName.soulfire, 4);//delete a card
            DamageTargetSpecialDatabase.Add(CardDB.cardName.quickshot, 3); //draw a card

        }

        private void setupsilenceDatabase()
        {
            this.silenceDatabase.Add(CardDB.cardName.dispel, 1);
            this.silenceDatabase.Add(CardDB.cardName.earthshock, 1);
            this.silenceDatabase.Add(CardDB.cardName.massdispel, 1);
            this.silenceDatabase.Add(CardDB.cardName.silence, 1);
            this.silenceDatabase.Add(CardDB.cardName.keeperofthegrove, 1);
            this.silenceDatabase.Add(CardDB.cardName.ironbeakowl, 1);
            this.silenceDatabase.Add(CardDB.cardName.spellbreaker, 1);

            this.silenceDatabase.Add(CardDB.cardName.wailingsoul, -2);//1=1 target, 2=all enemy, -2=all own

            //own need silence
            this.OwnNeedSilenceDatabase.Add(CardDB.cardName.ancientwatcher, 2);
            this.OwnNeedSilenceDatabase.Add(CardDB.cardName.zombiechow, 2);
            this.OwnNeedSilenceDatabase.Add(CardDB.cardName.animagolem, 1);
            this.OwnNeedSilenceDatabase.Add(CardDB.cardName.thebeast, 1);
            this.OwnNeedSilenceDatabase.Add(CardDB.cardName.deathcharger, 1);
            //this.OwnNeedSilenceDatabase.Add(CardDB.cardName.deathlord, 0);//if hp<3
            this.OwnNeedSilenceDatabase.Add(CardDB.cardName.dancingswords, 1);
            this.OwnNeedSilenceDatabase.Add(CardDB.cardName.spore, 3);
            //this.OwnNeedSilenceDatabase.Add(CardDB.cardName.barongeddon, 0); //2 damage to ALL other - if profitable
            this.OwnNeedSilenceDatabase.Add(CardDB.cardName.ragnarosthefirelord, 0);
            this.OwnNeedSilenceDatabase.Add(CardDB.cardName.mogortheogre, 1);
            this.OwnNeedSilenceDatabase.Add(CardDB.cardName.venturecomercenary, 1);
            this.OwnNeedSilenceDatabase.Add(CardDB.cardName.spectraltrainee, 1);
            this.OwnNeedSilenceDatabase.Add(CardDB.cardName.spectralwarrior, 1);
            this.OwnNeedSilenceDatabase.Add(CardDB.cardName.spectralrider, 1);
            this.OwnNeedSilenceDatabase.Add(CardDB.cardName.felreaver, 3);

        }


        private void setupPriorityList()
        {
            this.priorityDatabase.Add(CardDB.cardName.prophetvelen, 5);
            this.priorityDatabase.Add(CardDB.cardName.archmageantonidas, 5);
            this.priorityDatabase.Add(CardDB.cardName.flametonguetotem, 6);
            this.priorityDatabase.Add(CardDB.cardName.raidleader, 5);
            this.priorityDatabase.Add(CardDB.cardName.grimscaleoracle, 5);
            this.priorityDatabase.Add(CardDB.cardName.direwolfalpha, 6);
            this.priorityDatabase.Add(CardDB.cardName.murlocwarleader, 5);
            this.priorityDatabase.Add(CardDB.cardName.southseacaptain, 5);
            this.priorityDatabase.Add(CardDB.cardName.stormwindchampion, 5);
            this.priorityDatabase.Add(CardDB.cardName.timberwolf, 5);
            this.priorityDatabase.Add(CardDB.cardName.leokk, 5);
            this.priorityDatabase.Add(CardDB.cardName.northshirecleric, 5);
            this.priorityDatabase.Add(CardDB.cardName.sorcerersapprentice, 3);
            this.priorityDatabase.Add(CardDB.cardName.summoningportal, 5);
            this.priorityDatabase.Add(CardDB.cardName.pintsizedsummoner, 3);
            this.priorityDatabase.Add(CardDB.cardName.scavenginghyena, 5);
            this.priorityDatabase.Add(CardDB.cardName.manatidetotem, 5);
            this.priorityDatabase.Add(CardDB.cardName.mechwarper, 1);

            this.priorityDatabase.Add(CardDB.cardName.emperorthaurissan, 5);
            this.priorityDatabase.Add(CardDB.cardName.grimpatron, 5);
        }

        private void setupAttackBuff()
        {
            heroAttackBuffDatabase.Add(CardDB.cardName.bite, 4);
            heroAttackBuffDatabase.Add(CardDB.cardName.claw, 2);
            heroAttackBuffDatabase.Add(CardDB.cardName.heroicstrike, 2);

            this.attackBuffDatabase.Add(CardDB.cardName.abusivesergeant, 2);
            this.attackBuffDatabase.Add(CardDB.cardName.bananas, 1);
            this.attackBuffDatabase.Add(CardDB.cardName.bestialwrath, 2); // NEVER ON enemy MINION
            this.attackBuffDatabase.Add(CardDB.cardName.blessingofkings, 4);
            this.attackBuffDatabase.Add(CardDB.cardName.blessingofmight, 3);
            this.attackBuffDatabase.Add(CardDB.cardName.coldblood, 2);
            this.attackBuffDatabase.Add(CardDB.cardName.crueltaskmaster, 2);
            this.attackBuffDatabase.Add(CardDB.cardName.darkirondwarf, 2);
            this.attackBuffDatabase.Add(CardDB.cardName.innerrage, 2);
            this.attackBuffDatabase.Add(CardDB.cardName.markofnature, 4);//choice1 
            this.attackBuffDatabase.Add(CardDB.cardName.markofthewild, 2);
            this.attackBuffDatabase.Add(CardDB.cardName.nightmare, 5); //destroy minion on next turn
            this.attackBuffDatabase.Add(CardDB.cardName.rampage, 3);//only damaged minion 
            this.attackBuffDatabase.Add(CardDB.cardName.uproot, 5);
            this.attackBuffDatabase.Add(CardDB.cardName.velenschosen, 2);

            this.attackBuffDatabase.Add(CardDB.cardName.darkwispers, 5);//choice 2
            this.attackBuffDatabase.Add(CardDB.cardName.whirlingblades, 1);

        }

        private void setupHealthBuff()
        {

            //this.healthBuffDatabase.Add(CardDB.cardName.ancientofwar, 5);//choice2 is only buffing himself!
            this.healthBuffDatabase.Add(CardDB.cardName.bananas, 1);
            this.healthBuffDatabase.Add(CardDB.cardName.blessingofkings, 4);
            this.healthBuffDatabase.Add(CardDB.cardName.markofnature, 4);//choice2
            this.healthBuffDatabase.Add(CardDB.cardName.markofthewild, 2);
            this.healthBuffDatabase.Add(CardDB.cardName.nightmare, 5);
            this.healthBuffDatabase.Add(CardDB.cardName.powerwordshield, 2);
            this.healthBuffDatabase.Add(CardDB.cardName.rampage, 3);
            this.healthBuffDatabase.Add(CardDB.cardName.velenschosen, 4);
            this.healthBuffDatabase.Add(CardDB.cardName.darkwispers, 5);//choice2
            this.healthBuffDatabase.Add(CardDB.cardName.upgradedrepairbot, 4);
            this.healthBuffDatabase.Add(CardDB.cardName.armorplating, 1);
            //this.healthBuffDatabase.Add(CardDB.cardName.rooted, 5);

            this.tauntBuffDatabase.Add(CardDB.cardName.markofnature, 1);
            this.tauntBuffDatabase.Add(CardDB.cardName.markofthewild, 1);
            this.tauntBuffDatabase.Add(CardDB.cardName.darkwispers, 1);
            this.tauntBuffDatabase.Add(CardDB.cardName.rustyhorn, 1);
            this.tauntBuffDatabase.Add(CardDB.cardName.mutatinginjection, 1);
            this.tauntBuffDatabase.Add(CardDB.cardName.ancestralhealing, 1);
        }

        private void setupCardDrawBattlecry()
        {
            cardDrawBattleCryDatabase.Add(CardDB.cardName.echoofmedivh, 0);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.elitetaurenchieftain, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.flare, 1);
            //cardDrawBattleCryDatabase.Add(CardDB.cardName.ironjuggernaut, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.kingmukla, 2);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.mindpocalypse, 2);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.mindvision, 0);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.thoughtsteal, 0);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.tinkertowntechnician, 0); // If you have a Mech
            cardDrawBattleCryDatabase.Add(CardDB.cardName.toshley, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.tracking, 1); //NOT SUPPORTED YET
            cardDrawBattleCryDatabase.Add(CardDB.cardName.unstableportal, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.ancientoflore, 2);// choice =1
            cardDrawBattleCryDatabase.Add(CardDB.cardName.ancientteachings, 2);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.arcaneintellect, 2);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.azuredrake, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.battlerage, 0);//only if wounded own minions
            cardDrawBattleCryDatabase.Add(CardDB.cardName.callpet, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.coldlightoracle, 2);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.commandingshout, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.divinefavor, 0);//only if enemy has more cards than you
            cardDrawBattleCryDatabase.Add(CardDB.cardName.excessmana, 0);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.fanofknives, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.farsight, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.gnomishexperimenter, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.gnomishinventor, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.grovetender, 1); //choice = 2
            cardDrawBattleCryDatabase.Add(CardDB.cardName.hammerofwrath, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.harrisonjones, 0);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.holywrath, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.layonhands, 3);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.lifetap, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.massdispel, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.mortalcoil, 0);//only if kills
            cardDrawBattleCryDatabase.Add(CardDB.cardName.neptulon, 4);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.nourish, 3); //choice = 2
            cardDrawBattleCryDatabase.Add(CardDB.cardName.noviceengineer, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.powerwordshield, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.roguesdoit, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.shieldblock, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.shiv, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.slam, 0); //if survives
            cardDrawBattleCryDatabase.Add(CardDB.cardName.sprint, 4);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.starfire, 1);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.wrath, 1); //choice=2
            cardDrawBattleCryDatabase.Add(CardDB.cardName.quickshot, 0);//only if your hand is empty
            cardDrawBattleCryDatabase.Add(CardDB.cardName.solemnvigil, 2);
            cardDrawBattleCryDatabase.Add(CardDB.cardName.nefarian, 2);
        }


        private void setupUsefulNeedKeepDatabase()
        {
            UsefulNeedKeepDatabase.Add(CardDB.cardName.tradeprincegallywix, 5);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.ragnarosthefirelord, 5);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.kelthuzad, 18);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.mekgineerthermaplugg, 5);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.malganis, 13);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.gruul, 4);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.archmageantonidas, 7);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.troggzortheearthinator, 4);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.stormwindchampion, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.gazlowe, 6);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.weespellstopper, 11);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.violetteacher, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.siltfinspiritwalker, 5);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.siegeengine, 8);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.lightwell, 13);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.junkbot, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.impmaster, 5);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.illidanstormrage, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.felcannon, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.burlyrockjawtrogg, 5);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.summoningportal, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.natpagle, 2);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.leokk, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.jeeves, 0);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.hogger, 13);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.gadgetzanauctioneer, 9);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.frothingberserker, 9);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.floatingwatcher, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.emboldener3000, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.demolisher, 11);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.armorsmith, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.warsongcommander, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.vitalitytotem, 8);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.stonesplintertrogg, 8);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.southseacaptain, 11);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.shipscannon, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.shadowboxer, 11);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.repairbot, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.northshirecleric, 11);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.murlocwarleader, 11);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.mechwarper, 11);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.masterswordsmith, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.manawyrm, 9);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.manatidetotem, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.hobgoblin, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.flesheatingghoul, 9);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.flametonguetotem, 30);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.cobaltguardian, 8);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.alarmobot, 4);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.undertaker, 8);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.starvingbuzzard, 8);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.sorcerersapprentice, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.shadeofnaxxramas, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.secretkeeper, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.scavenginghyena, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.raidleader, 11);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.questingadventurer, 9);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.pintsizedsummoner, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.murloctidecaller, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.micromachine, 12);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.lightwarden, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.ironsensei, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.healingtotem, 9);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.direwolfalpha, 30);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.cultmaster, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.youngpriestess, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.timberwolf, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.homingchicken, 12);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.grimscaleoracle, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.bloodimp, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.illuminator, 2);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.faeriedragon, 7);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.prophetvelen, 5);

            UsefulNeedKeepDatabase.Add(CardDB.cardName.emperorthaurissan, 11);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.knifejuggler, 10);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.flamewaker, 12);
            UsefulNeedKeepDatabase.Add(CardDB.cardName.dragonkinsorcerer, 9);
        }

        private void setupDiscardCards()
        {
            cardDiscardDatabase.Add(CardDB.cardName.doomguard, 5);
            cardDiscardDatabase.Add(CardDB.cardName.soulfire, 0);
            cardDiscardDatabase.Add(CardDB.cardName.succubus, 2);
        }

        private void setupDestroyOwnCards()
        {
            this.destroyOwnDatabase.Add(CardDB.cardName.brawl, 0);
            this.destroyOwnDatabase.Add(CardDB.cardName.deathwing, 0);
            this.destroyOwnDatabase.Add(CardDB.cardName.twistingnether, 0);
            this.destroyOwnDatabase.Add(CardDB.cardName.naturalize, 0);//not own mins
            this.destroyOwnDatabase.Add(CardDB.cardName.shadowworddeath, 0);//not own mins
            this.destroyOwnDatabase.Add(CardDB.cardName.shadowwordpain, 0);//not own mins
            this.destroyOwnDatabase.Add(CardDB.cardName.siphonsoul, 0);//not own mins
            this.destroyOwnDatabase.Add(CardDB.cardName.biggamehunter, 0);//not own mins
            this.destroyOwnDatabase.Add(CardDB.cardName.hungrycrab, 0);//not own mins
            this.destroyOwnDatabase.Add(CardDB.cardName.sacrificialpact, 0);//not own mins

            this.destroyDatabase.Add(CardDB.cardName.assassinate, 0);//not own mins
            this.destroyDatabase.Add(CardDB.cardName.corruption, 0);//not own mins
            this.destroyDatabase.Add(CardDB.cardName.execute, 0);//not own mins
            this.destroyDatabase.Add(CardDB.cardName.naturalize, 0);//not own mins
            this.destroyDatabase.Add(CardDB.cardName.siphonsoul, 0);//not own mins
            this.destroyDatabase.Add(CardDB.cardName.mindcontrol, 0);//not own mins
            this.destroyDatabase.Add(CardDB.cardName.theblackknight, 0);//not own mins
            this.destroyDatabase.Add(CardDB.cardName.sabotage, 0);//not own mins
            this.destroyDatabase.Add(CardDB.cardName.crush, 0);//not own mins
            this.destroyDatabase.Add(CardDB.cardName.hemetnesingwary, 0);//not own mins
            this.destroyDatabase.Add(CardDB.cardName.deadlyshot, 0);
            this.destroyDatabase.Add(CardDB.cardName.shadowwordpain, 0);
            this.destroyDatabase.Add(CardDB.cardName.rendblackhand, 0);


        }

        private void setupReturnBackToHandCards()
        {
            returnHandDatabase.Add(CardDB.cardName.ancientbrewmaster, 0);
            returnHandDatabase.Add(CardDB.cardName.dream, 0);
            returnHandDatabase.Add(CardDB.cardName.kidnapper, 0);//if combo
            returnHandDatabase.Add(CardDB.cardName.shadowstep, 0);
            returnHandDatabase.Add(CardDB.cardName.vanish, 0);
            returnHandDatabase.Add(CardDB.cardName.youthfulbrewmaster, 0);
            returnHandDatabase.Add(CardDB.cardName.timerewinder, 0);
            returnHandDatabase.Add(CardDB.cardName.recycle, 0);
        }

        private void setupHeroDamagingAOE()
        {
            this.heroDamagingAoeDatabase.Add(CardDB.cardName.unknown, 0);
        }

        private void setupSpecialMins()
        {
            this.specialMinions.Add(CardDB.cardName.amaniberserker, 0);
            this.specialMinions.Add(CardDB.cardName.angrychicken, 0);
            this.specialMinions.Add(CardDB.cardName.abomination, 0);
            this.specialMinions.Add(CardDB.cardName.acolyteofpain, 0);
            this.specialMinions.Add(CardDB.cardName.alarmobot, 0);
            this.specialMinions.Add(CardDB.cardName.archmage, 0);
            this.specialMinions.Add(CardDB.cardName.archmageantonidas, 0);
            this.specialMinions.Add(CardDB.cardName.armorsmith, 0);
            this.specialMinions.Add(CardDB.cardName.auchenaisoulpriest, 0);
            this.specialMinions.Add(CardDB.cardName.azuredrake, 0);
            this.specialMinions.Add(CardDB.cardName.barongeddon, 0);
            this.specialMinions.Add(CardDB.cardName.bloodimp, 0);
            this.specialMinions.Add(CardDB.cardName.bloodmagethalnos, 0);
            this.specialMinions.Add(CardDB.cardName.cairnebloodhoof, 0);
            this.specialMinions.Add(CardDB.cardName.cultmaster, 0);
            this.specialMinions.Add(CardDB.cardName.dalaranmage, 0);
            this.specialMinions.Add(CardDB.cardName.demolisher, 0);
            this.specialMinions.Add(CardDB.cardName.direwolfalpha, 0);
            this.specialMinions.Add(CardDB.cardName.doomsayer, 0);
            this.specialMinions.Add(CardDB.cardName.emperorcobra, 0);
            this.specialMinions.Add(CardDB.cardName.etherealarcanist, 0);
            this.specialMinions.Add(CardDB.cardName.flametonguetotem, 0);
            this.specialMinions.Add(CardDB.cardName.flesheatingghoul, 0);
            this.specialMinions.Add(CardDB.cardName.gadgetzanauctioneer, 0);
            this.specialMinions.Add(CardDB.cardName.grimscaleoracle, 0);
            this.specialMinions.Add(CardDB.cardName.grommashhellscream, 0);
            this.specialMinions.Add(CardDB.cardName.gruul, 0);
            this.specialMinions.Add(CardDB.cardName.gurubashiberserker, 0);
            this.specialMinions.Add(CardDB.cardName.harvestgolem, 0);
            this.specialMinions.Add(CardDB.cardName.hogger, 0);
            this.specialMinions.Add(CardDB.cardName.illidanstormrage, 0);
            this.specialMinions.Add(CardDB.cardName.impmaster, 0);
            this.specialMinions.Add(CardDB.cardName.knifejuggler, 0);
            this.specialMinions.Add(CardDB.cardName.koboldgeomancer, 0);
            this.specialMinions.Add(CardDB.cardName.lepergnome, 0);
            this.specialMinions.Add(CardDB.cardName.lightspawn, 0);
            this.specialMinions.Add(CardDB.cardName.lightwarden, 0);
            this.specialMinions.Add(CardDB.cardName.lightwell, 0);
            this.specialMinions.Add(CardDB.cardName.loothoarder, 0);
            this.specialMinions.Add(CardDB.cardName.lorewalkercho, 0);
            this.specialMinions.Add(CardDB.cardName.malygos, 0);
            this.specialMinions.Add(CardDB.cardName.manaaddict, 0);
            this.specialMinions.Add(CardDB.cardName.manatidetotem, 0);
            this.specialMinions.Add(CardDB.cardName.manawraith, 0);
            this.specialMinions.Add(CardDB.cardName.manawyrm, 0);
            this.specialMinions.Add(CardDB.cardName.masterswordsmith, 0);
            this.specialMinions.Add(CardDB.cardName.murloctidecaller, 0);
            this.specialMinions.Add(CardDB.cardName.murlocwarleader, 0);
            this.specialMinions.Add(CardDB.cardName.natpagle, 0);
            this.specialMinions.Add(CardDB.cardName.northshirecleric, 0);
            this.specialMinions.Add(CardDB.cardName.ogremagi, 0);
            this.specialMinions.Add(CardDB.cardName.oldmurkeye, 0);
            this.specialMinions.Add(CardDB.cardName.patientassassin, 0);
            this.specialMinions.Add(CardDB.cardName.pintsizedsummoner, 0);
            this.specialMinions.Add(CardDB.cardName.prophetvelen, 0);
            this.specialMinions.Add(CardDB.cardName.questingadventurer, 0);
            this.specialMinions.Add(CardDB.cardName.ragingworgen, 0);
            this.specialMinions.Add(CardDB.cardName.raidleader, 0);
            this.specialMinions.Add(CardDB.cardName.savannahhighmane, 0);
            this.specialMinions.Add(CardDB.cardName.scavenginghyena, 0);
            this.specialMinions.Add(CardDB.cardName.secretkeeper, 0);
            this.specialMinions.Add(CardDB.cardName.sorcerersapprentice, 0);
            this.specialMinions.Add(CardDB.cardName.southseacaptain, 0);
            this.specialMinions.Add(CardDB.cardName.spitefulsmith, 0);
            this.specialMinions.Add(CardDB.cardName.starvingbuzzard, 0);
            this.specialMinions.Add(CardDB.cardName.stormwindchampion, 0);
            this.specialMinions.Add(CardDB.cardName.summoningportal, 0);
            this.specialMinions.Add(CardDB.cardName.sylvanaswindrunner, 0);
            this.specialMinions.Add(CardDB.cardName.taurenwarrior, 0);
            this.specialMinions.Add(CardDB.cardName.thebeast, 0);
            this.specialMinions.Add(CardDB.cardName.timberwolf, 0);
            this.specialMinions.Add(CardDB.cardName.tirionfordring, 0);
            this.specialMinions.Add(CardDB.cardName.tundrarhino, 0);
            this.specialMinions.Add(CardDB.cardName.unboundelemental, 0);
            //this.specialMinions.Add(CardDB.cardName.venturecomercenary, 0);
            this.specialMinions.Add(CardDB.cardName.violetteacher, 0);
            this.specialMinions.Add(CardDB.cardName.warsongcommander, 0);
            this.specialMinions.Add(CardDB.cardName.waterelemental, 0);

            this.specialMinions.Add(CardDB.cardName.grimpatron, 0);
            this.specialMinions.Add(CardDB.cardName.emperorthaurissan, 0);
            this.specialMinions.Add(CardDB.cardName.impgangboss, 0);
            this.specialMinions.Add(CardDB.cardName.axeflinger, 0);
            this.specialMinions.Add(CardDB.cardName.dragonegg, 0);

            // naxx cards
            this.specialMinions.Add(CardDB.cardName.baronrivendare, 0);
            this.specialMinions.Add(CardDB.cardName.undertaker, 0);
            this.specialMinions.Add(CardDB.cardName.dancingswords, 0);
            this.specialMinions.Add(CardDB.cardName.darkcultist, 0);
            this.specialMinions.Add(CardDB.cardName.deathlord, 0);
            this.specialMinions.Add(CardDB.cardName.feugen, 0);
            this.specialMinions.Add(CardDB.cardName.stalagg, 0);
            this.specialMinions.Add(CardDB.cardName.hauntedcreeper, 0);
            this.specialMinions.Add(CardDB.cardName.kelthuzad, 0);
            this.specialMinions.Add(CardDB.cardName.madscientist, 0);
            this.specialMinions.Add(CardDB.cardName.maexxna, 0);
            this.specialMinions.Add(CardDB.cardName.nerubarweblord, 0);
            this.specialMinions.Add(CardDB.cardName.shadeofnaxxramas, 0);
            this.specialMinions.Add(CardDB.cardName.unstableghoul, 0);
            this.specialMinions.Add(CardDB.cardName.voidcaller, 0);
            this.specialMinions.Add(CardDB.cardName.anubarambusher, 0);
            this.specialMinions.Add(CardDB.cardName.webspinner, 0);
            this.specialMinions.Add(CardDB.cardName.flamewaker, 0);
            this.specialMinions.Add(CardDB.cardName.chromaggus, 0);
            this.specialMinions.Add(CardDB.cardName.dragonkinsorcerer, 0);

        }

        private void setupBuffingMinions()
        {
            buffingMinionsDatabase.Add(CardDB.cardName.abusivesergeant, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.captaingreenskin, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.cenarius, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.coldlightseer, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.crueltaskmaster, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.darkirondwarf, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.defenderofargus, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.direwolfalpha, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.flametonguetotem, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.grimscaleoracle, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.houndmaster, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.leokk, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.murlocwarleader, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.raidleader, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.shatteredsuncleric, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.southseacaptain, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.spitefulsmith, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.stormwindchampion, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.templeenforcer, 0);
            buffingMinionsDatabase.Add(CardDB.cardName.timberwolf, 0);

            buffing1TurnDatabase.Add(CardDB.cardName.abusivesergeant, 0);
            buffing1TurnDatabase.Add(CardDB.cardName.darkirondwarf, 0);

        }

        private void setupEnemyTargetPriority()
        {
            priorityTargets.Add(CardDB.cardName.angrychicken, 10);
            priorityTargets.Add(CardDB.cardName.lightwarden, 10);
            priorityTargets.Add(CardDB.cardName.secretkeeper, 10);
            priorityTargets.Add(CardDB.cardName.youngdragonhawk, 10);
            priorityTargets.Add(CardDB.cardName.bloodmagethalnos, 10);
            priorityTargets.Add(CardDB.cardName.direwolfalpha, 10);
            priorityTargets.Add(CardDB.cardName.doomsayer, 10);
            priorityTargets.Add(CardDB.cardName.knifejuggler, 10);
            priorityTargets.Add(CardDB.cardName.koboldgeomancer, 10);
            priorityTargets.Add(CardDB.cardName.manaaddict, 10);
            priorityTargets.Add(CardDB.cardName.masterswordsmith, 10);
            priorityTargets.Add(CardDB.cardName.natpagle, 10);
            priorityTargets.Add(CardDB.cardName.murloctidehunter, 10);
            priorityTargets.Add(CardDB.cardName.pintsizedsummoner, 10);
            priorityTargets.Add(CardDB.cardName.wildpyromancer, 10);
            priorityTargets.Add(CardDB.cardName.alarmobot, 10);
            priorityTargets.Add(CardDB.cardName.acolyteofpain, 10);
            priorityTargets.Add(CardDB.cardName.demolisher, 10);
            priorityTargets.Add(CardDB.cardName.flesheatingghoul, 10);
            priorityTargets.Add(CardDB.cardName.impmaster, 10);
            priorityTargets.Add(CardDB.cardName.questingadventurer, 10);
            priorityTargets.Add(CardDB.cardName.raidleader, 10);
            priorityTargets.Add(CardDB.cardName.thrallmarfarseer, 10);
            priorityTargets.Add(CardDB.cardName.cultmaster, 10);
            priorityTargets.Add(CardDB.cardName.leeroyjenkins, 10);
            priorityTargets.Add(CardDB.cardName.violetteacher, 10);
            priorityTargets.Add(CardDB.cardName.gadgetzanauctioneer, 10);
            priorityTargets.Add(CardDB.cardName.hogger, 10);
            priorityTargets.Add(CardDB.cardName.illidanstormrage, 10);
            priorityTargets.Add(CardDB.cardName.barongeddon, 10);
            priorityTargets.Add(CardDB.cardName.stormwindchampion, 10);
            priorityTargets.Add(CardDB.cardName.gurubashiberserker, 10);

            //BRM cards
            priorityTargets.Add(CardDB.cardName.grimpatron, 10);
            priorityTargets.Add(CardDB.cardName.emperorthaurissan, 10);
            priorityTargets.Add(CardDB.cardName.impgangboss, 10);
            priorityTargets.Add(CardDB.cardName.dragonegg, 10);
            priorityTargets.Add(CardDB.cardName.chromaggus, 10);
            priorityTargets.Add(CardDB.cardName.flamewaker, 10);
            priorityTargets.Add(CardDB.cardName.dragonkinsorcerer, 4);

            //warrior cards
            priorityTargets.Add(CardDB.cardName.frothingberserker, 10);
            priorityTargets.Add(CardDB.cardName.warsongcommander, 10);

            //warlock cards
            priorityTargets.Add(CardDB.cardName.summoningportal, 10);

            //shaman cards
            priorityTargets.Add(CardDB.cardName.dustdevil, 10);
            priorityTargets.Add(CardDB.cardName.wrathofairtotem, 1);
            priorityTargets.Add(CardDB.cardName.flametonguetotem, 10);
            priorityTargets.Add(CardDB.cardName.manatidetotem, 10);
            priorityTargets.Add(CardDB.cardName.unboundelemental, 10);

            //rogue cards

            //priest cards
            priorityTargets.Add(CardDB.cardName.northshirecleric, 10);
            priorityTargets.Add(CardDB.cardName.lightwell, 10);
            priorityTargets.Add(CardDB.cardName.auchenaisoulpriest, 10);
            priorityTargets.Add(CardDB.cardName.prophetvelen, 10);

            //paladin cards

            //mage cards
            priorityTargets.Add(CardDB.cardName.manawyrm, 10);
            priorityTargets.Add(CardDB.cardName.sorcerersapprentice, 10);
            priorityTargets.Add(CardDB.cardName.etherealarcanist, 10);
            priorityTargets.Add(CardDB.cardName.archmageantonidas, 10);

            //hunter cards
            priorityTargets.Add(CardDB.cardName.timberwolf, 10);
            priorityTargets.Add(CardDB.cardName.scavenginghyena, 10);
            priorityTargets.Add(CardDB.cardName.starvingbuzzard, 10);
            priorityTargets.Add(CardDB.cardName.leokk, 10);
            priorityTargets.Add(CardDB.cardName.tundrarhino, 10);

            //naxx cards
            priorityTargets.Add(CardDB.cardName.baronrivendare, 10);
            priorityTargets.Add(CardDB.cardName.kelthuzad, 10);
            priorityTargets.Add(CardDB.cardName.nerubarweblord, 10);
            priorityTargets.Add(CardDB.cardName.shadeofnaxxramas, 10);
            priorityTargets.Add(CardDB.cardName.undertaker, 10);

        }

        private void setupLethalHelpMinions()
        {
            lethalHelpers.Add(CardDB.cardName.auchenaisoulpriest, 0);
            //spellpower minions
            lethalHelpers.Add(CardDB.cardName.archmage, 0);
            lethalHelpers.Add(CardDB.cardName.dalaranmage, 0);
            lethalHelpers.Add(CardDB.cardName.koboldgeomancer, 0);
            lethalHelpers.Add(CardDB.cardName.ogremagi, 0);
            lethalHelpers.Add(CardDB.cardName.ancientmage, 0);
            lethalHelpers.Add(CardDB.cardName.azuredrake, 0);
            lethalHelpers.Add(CardDB.cardName.bloodmagethalnos, 0);
            lethalHelpers.Add(CardDB.cardName.malygos, 0);
            lethalHelpers.Add(CardDB.cardName.velenschosen, 0);
            lethalHelpers.Add(CardDB.cardName.sootspewer, 0);
            lethalHelpers.Add(CardDB.cardName.minimage, 0);
            //

        }

        private void setupRelations()
        {
            spellDependentDatabase.Add(CardDB.cardName.wildpyromancer, 1);
            spellDependentDatabase.Add(CardDB.cardName.lorewalkercho, 0);
            spellDependentDatabase.Add(CardDB.cardName.gazlowe, 1);
            spellDependentDatabase.Add(CardDB.cardName.archmageantonidas, 2);
            spellDependentDatabase.Add(CardDB.cardName.gadgetzanauctioneer, 2);
            spellDependentDatabase.Add(CardDB.cardName.manawyrm, 1);
            spellDependentDatabase.Add(CardDB.cardName.manaaddict, 1);
            spellDependentDatabase.Add(CardDB.cardName.violetteacher, 1);
            spellDependentDatabase.Add(CardDB.cardName.stonesplintertrogg, -1);
            spellDependentDatabase.Add(CardDB.cardName.burlyrockjawtrogg, -1);
            spellDependentDatabase.Add(CardDB.cardName.tradeprincegallywix, -2);
            spellDependentDatabase.Add(CardDB.cardName.troggzortheearthinator, -1);
            spellDependentDatabase.Add(CardDB.cardName.flamewaker, 1);
            spellDependentDatabase.Add(CardDB.cardName.chromaggus, 1);
        }

        private void setupSilenceTargets()
        {
            this.silenceTargets.Add(CardDB.cardName.abomination, 0);
            this.silenceTargets.Add(CardDB.cardName.acolyteofpain, 0);
            this.silenceTargets.Add(CardDB.cardName.archmageantonidas, 0);
            this.silenceTargets.Add(CardDB.cardName.armorsmith, 0);
            this.silenceTargets.Add(CardDB.cardName.auchenaisoulpriest, 0);
            this.silenceTargets.Add(CardDB.cardName.barongeddon, 0);
            this.silenceTargets.Add(CardDB.cardName.baronrivendare, 0);
            this.silenceTargets.Add(CardDB.cardName.bloodimp, 0);
            this.silenceTargets.Add(CardDB.cardName.bolvarfordragon, 0);
            this.silenceTargets.Add(CardDB.cardName.burlyrockjawtrogg, 0);
            this.silenceTargets.Add(CardDB.cardName.cobaltguardian, 0);
            this.silenceTargets.Add(CardDB.cardName.cultmaster, 0);
            this.silenceTargets.Add(CardDB.cardName.direwolfalpha, 0);
            this.silenceTargets.Add(CardDB.cardName.doomsayer, 0);
            this.silenceTargets.Add(CardDB.cardName.emboldener3000, 0);
            this.silenceTargets.Add(CardDB.cardName.emperorcobra, 0);
            this.silenceTargets.Add(CardDB.cardName.etherealarcanist, 0);
            this.silenceTargets.Add(CardDB.cardName.flametonguetotem, 0);
            this.silenceTargets.Add(CardDB.cardName.flesheatingghoul, 0);
            this.silenceTargets.Add(CardDB.cardName.floatingwatcher, 0);
            this.silenceTargets.Add(CardDB.cardName.foereaper4000, 0);
            this.silenceTargets.Add(CardDB.cardName.frothingberserker, 0);
            this.silenceTargets.Add(CardDB.cardName.gadgetzanauctioneer, 10);
            this.silenceTargets.Add(CardDB.cardName.gahzrilla, 0);
            this.silenceTargets.Add(CardDB.cardName.grimscaleoracle, 0);
            this.silenceTargets.Add(CardDB.cardName.grommashhellscream, 0);
            this.silenceTargets.Add(CardDB.cardName.gruul, 0);
            this.silenceTargets.Add(CardDB.cardName.gurubashiberserker, 0);
            this.silenceTargets.Add(CardDB.cardName.hobgoblin, 0);
            this.silenceTargets.Add(CardDB.cardName.hogger, 0);
            this.silenceTargets.Add(CardDB.cardName.homingchicken, 0);
            this.silenceTargets.Add(CardDB.cardName.illidanstormrage, 0);
            this.silenceTargets.Add(CardDB.cardName.impmaster, 0);
            this.silenceTargets.Add(CardDB.cardName.ironsensei, 0);
            this.silenceTargets.Add(CardDB.cardName.jeeves, 0);
            this.silenceTargets.Add(CardDB.cardName.junkbot, 0);
            this.silenceTargets.Add(CardDB.cardName.kelthuzad, 10);
            this.silenceTargets.Add(CardDB.cardName.knifejuggler, 0);
            this.silenceTargets.Add(CardDB.cardName.leokk, 0);
            this.silenceTargets.Add(CardDB.cardName.lightspawn, 0);
            this.silenceTargets.Add(CardDB.cardName.lightwarden, 0);
            this.silenceTargets.Add(CardDB.cardName.lightwell, 0);
            this.silenceTargets.Add(CardDB.cardName.lorewalkercho, 0);
            this.silenceTargets.Add(CardDB.cardName.maexxna, 0);
            this.silenceTargets.Add(CardDB.cardName.malganis, 0);
            this.silenceTargets.Add(CardDB.cardName.malygos, 0);
            this.silenceTargets.Add(CardDB.cardName.manaaddict, 0);
            this.silenceTargets.Add(CardDB.cardName.manatidetotem, 0);
            this.silenceTargets.Add(CardDB.cardName.manawraith, 0);
            this.silenceTargets.Add(CardDB.cardName.manawyrm, 0);
            this.silenceTargets.Add(CardDB.cardName.masterswordsmith, 0);
            this.silenceTargets.Add(CardDB.cardName.mekgineerthermaplugg, 0);
            this.silenceTargets.Add(CardDB.cardName.micromachine, 0);
            this.silenceTargets.Add(CardDB.cardName.mogortheogre, 0);
            this.silenceTargets.Add(CardDB.cardName.murloctidecaller, 0);
            this.silenceTargets.Add(CardDB.cardName.murlocwarleader, 0);
            this.silenceTargets.Add(CardDB.cardName.natpagle, 0);
            this.silenceTargets.Add(CardDB.cardName.nerubarweblord, 0);
            this.silenceTargets.Add(CardDB.cardName.northshirecleric, 0);
            this.silenceTargets.Add(CardDB.cardName.oldmurkeye, 0);
            this.silenceTargets.Add(CardDB.cardName.oneeyedcheat, 0);
            this.silenceTargets.Add(CardDB.cardName.prophetvelen, 0);
            this.silenceTargets.Add(CardDB.cardName.questingadventurer, 0);
            this.silenceTargets.Add(CardDB.cardName.ragingworgen, 0);
            this.silenceTargets.Add(CardDB.cardName.raidleader, 0);
            this.silenceTargets.Add(CardDB.cardName.scavenginghyena, 0);
            this.silenceTargets.Add(CardDB.cardName.secretkeeper, 0);
            this.silenceTargets.Add(CardDB.cardName.shadeofnaxxramas, 0);
            this.silenceTargets.Add(CardDB.cardName.shadowboxer, 0);
            this.silenceTargets.Add(CardDB.cardName.shipscannon, 0);
            this.silenceTargets.Add(CardDB.cardName.siegeengine, 0);
            this.silenceTargets.Add(CardDB.cardName.siltfinspiritwalker, 0);
            this.silenceTargets.Add(CardDB.cardName.sorcerersapprentice, 0);
            this.silenceTargets.Add(CardDB.cardName.southseacaptain, 0);
            this.silenceTargets.Add(CardDB.cardName.spitefulsmith, 0);
            this.silenceTargets.Add(CardDB.cardName.starvingbuzzard, 0);
            this.silenceTargets.Add(CardDB.cardName.steamwheedlesniper, 0);
            this.silenceTargets.Add(CardDB.cardName.stonesplintertrogg, 0);
            this.silenceTargets.Add(CardDB.cardName.stormwindchampion, 0);
            this.silenceTargets.Add(CardDB.cardName.summoningportal, 0);
            this.silenceTargets.Add(CardDB.cardName.timberwolf, 0);
            this.silenceTargets.Add(CardDB.cardName.tirionfordring, 0);
            this.silenceTargets.Add(CardDB.cardName.tradeprincegallywix, 0);
            this.silenceTargets.Add(CardDB.cardName.troggzortheearthinator, 0);
            this.silenceTargets.Add(CardDB.cardName.tundrarhino, 0);
            this.silenceTargets.Add(CardDB.cardName.unboundelemental, 0);
            this.silenceTargets.Add(CardDB.cardName.undertaker, 0);
            this.silenceTargets.Add(CardDB.cardName.v07tr0n, 0);
            this.silenceTargets.Add(CardDB.cardName.violetteacher, 0);
            this.silenceTargets.Add(CardDB.cardName.warsongcommander, 0);
            this.silenceTargets.Add(CardDB.cardName.youngpriestess, 0);
            this.silenceTargets.Add(CardDB.cardName.ysera, 0);

            this.silenceTargets.Add(CardDB.cardName.grimpatron, 0);
            this.silenceTargets.Add(CardDB.cardName.emperorthaurissan, 0);
            this.silenceTargets.Add(CardDB.cardName.impgangboss, 0);
            this.silenceTargets.Add(CardDB.cardName.axeflinger, 0);
            this.silenceTargets.Add(CardDB.cardName.dragonegg, 0);
            this.silenceTargets.Add(CardDB.cardName.flamewaker, 0);
            this.silenceTargets.Add(CardDB.cardName.chromaggus, 0);
            this.silenceTargets.Add(CardDB.cardName.dragonkinsorcerer, 0);

            //this.silenceTargets.Add(CardDB.cardName.bloodimp, 0);
            //this.specialMinions.Add(CardDB.cardName.unboundelemental, 0);
            //this.specialMinions.Add(CardDB.cardName.venturecomercenary, 0);
            //this.specialMinions.Add(CardDB.cardName.waterelemental, 0);
            //this.specialMinions.Add(CardDB.cardName.voidcaller, 0);
        }

        private void setupRandomCards()
        {
            this.randomEffects.Add(CardDB.cardName.deadlyshot, 1);
            this.randomEffects.Add(CardDB.cardName.multishot, 1);

            this.randomEffects.Add(CardDB.cardName.animalcompanion, 1);
            this.randomEffects.Add(CardDB.cardName.arcanemissiles, 3);
            this.randomEffects.Add(CardDB.cardName.goblinblastmage, 4);
            this.randomEffects.Add(CardDB.cardName.avengingwrath, 8);

            this.randomEffects.Add(CardDB.cardName.flamecannon, 1);

            //this.randomEffects.Add(CardDB.cardName.baneofdoom, 1);
            this.randomEffects.Add(CardDB.cardName.brawl, 1);
            this.randomEffects.Add(CardDB.cardName.captainsparrot, 1);
            this.randomEffects.Add(CardDB.cardName.cleave, 2);
            this.randomEffects.Add(CardDB.cardName.forkedlightning, 1);
            this.randomEffects.Add(CardDB.cardName.gelbinmekkatorque, 1);
            this.randomEffects.Add(CardDB.cardName.iammurloc, 3);
            this.randomEffects.Add(CardDB.cardName.lightningstorm, 1);
            this.randomEffects.Add(CardDB.cardName.madbomber, 3);
            this.randomEffects.Add(CardDB.cardName.mindgames, 1);
            this.randomEffects.Add(CardDB.cardName.mindcontroltech, 1);
            this.randomEffects.Add(CardDB.cardName.mindvision, 1);
            this.randomEffects.Add(CardDB.cardName.powerofthehorde, 1);
            this.randomEffects.Add(CardDB.cardName.sensedemons, 2);
            this.randomEffects.Add(CardDB.cardName.tinkmasteroverspark, 1);
            this.randomEffects.Add(CardDB.cardName.totemiccall, 1);
            this.randomEffects.Add(CardDB.cardName.elitetaurenchieftain, 1);
            this.randomEffects.Add(CardDB.cardName.lifetap, 1);

            this.randomEffects.Add(CardDB.cardName.unstableportal, 1);
            this.randomEffects.Add(CardDB.cardName.crackle, 1);
            this.randomEffects.Add(CardDB.cardName.bouncingblade, 3);
            this.randomEffects.Add(CardDB.cardName.coghammer, 1);
            this.randomEffects.Add(CardDB.cardName.madderbomber, 1);
            this.randomEffects.Add(CardDB.cardName.bomblobber, 1);
            this.randomEffects.Add(CardDB.cardName.enhanceomechano, 1);
        }


        private void setupChooseDatabase()
        {
            this.choose1database.Add(CardDB.cardName.wrath, CardDB.cardIDEnum.EX1_154a);
            this.choose1database.Add(CardDB.cardName.starfall, CardDB.cardIDEnum.NEW1_007b);
            this.choose1database.Add(CardDB.cardName.powerofthewild, CardDB.cardIDEnum.EX1_160b);
            this.choose1database.Add(CardDB.cardName.nourish, CardDB.cardIDEnum.EX1_164a);
            this.choose1database.Add(CardDB.cardName.markofnature, CardDB.cardIDEnum.EX1_155a);
            this.choose1database.Add(CardDB.cardName.keeperofthegrove, CardDB.cardIDEnum.EX1_166a);
            this.choose1database.Add(CardDB.cardName.grovetender, CardDB.cardIDEnum.GVG_032a);
            this.choose1database.Add(CardDB.cardName.druidoftheflame, CardDB.cardIDEnum.BRM_010t);
            this.choose1database.Add(CardDB.cardName.druidoftheclaw, CardDB.cardIDEnum.EX1_165t1);
            this.choose1database.Add(CardDB.cardName.darkwispers, CardDB.cardIDEnum.GVG_041b);
            this.choose1database.Add(CardDB.cardName.cenarius, CardDB.cardIDEnum.EX1_573a);
            this.choose1database.Add(CardDB.cardName.anodizedrobocub, CardDB.cardIDEnum.GVG_030a);
            this.choose1database.Add(CardDB.cardName.ancientofwar, CardDB.cardIDEnum.EX1_178a);
            this.choose1database.Add(CardDB.cardName.ancientoflore, CardDB.cardIDEnum.NEW1_008a);

            this.choose2database.Add(CardDB.cardName.wrath, CardDB.cardIDEnum.EX1_154b);
            this.choose2database.Add(CardDB.cardName.starfall, CardDB.cardIDEnum.NEW1_007a);
            this.choose2database.Add(CardDB.cardName.powerofthewild, CardDB.cardIDEnum.EX1_160a);
            this.choose2database.Add(CardDB.cardName.nourish, CardDB.cardIDEnum.EX1_164b);
            this.choose2database.Add(CardDB.cardName.markofnature, CardDB.cardIDEnum.EX1_155b);
            this.choose2database.Add(CardDB.cardName.keeperofthegrove, CardDB.cardIDEnum.EX1_166b);
            this.choose2database.Add(CardDB.cardName.grovetender, CardDB.cardIDEnum.GVG_032b);
            this.choose2database.Add(CardDB.cardName.druidoftheflame, CardDB.cardIDEnum.BRM_010t2);
            this.choose2database.Add(CardDB.cardName.druidoftheclaw, CardDB.cardIDEnum.EX1_165t2);
            this.choose2database.Add(CardDB.cardName.darkwispers, CardDB.cardIDEnum.GVG_041a);
            this.choose2database.Add(CardDB.cardName.cenarius, CardDB.cardIDEnum.EX1_573b);
            this.choose2database.Add(CardDB.cardName.anodizedrobocub, CardDB.cardIDEnum.GVG_030b);
            this.choose2database.Add(CardDB.cardName.ancientofwar, CardDB.cardIDEnum.EX1_178b);
            this.choose2database.Add(CardDB.cardName.ancientoflore, CardDB.cardIDEnum.NEW1_008b);
        }

        private void setAdditionalData()
        {
            foreach (Card c in this.cardlist)
            {
                if (cardDrawBattleCryDatabase.ContainsKey(c.name))
                {
                    c.isCarddraw = cardDrawBattleCryDatabase[c.name];
                }

                if (DamageTargetSpecialDatabase.ContainsKey(c.name))
                {
                    c.damagesTargetWithSpecial = true;
                }

                if (DamageTargetDatabase.ContainsKey(c.name))
                {
                    c.damagesTarget = true;
                }

                if (priorityTargets.ContainsKey(c.name))
                {
                    c.targetPriority = priorityTargets[c.name];
                }

                if (specialMinions.ContainsKey(c.name))
                {
                    c.isSpecialMinion = true;
                }
            }
        }

    }

}