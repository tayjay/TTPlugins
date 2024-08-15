using System;
using System.ComponentModel;
using Exiled.API.Interfaces;
using RoundModifiers.Modifiers;
using RoundModifiers.Modifiers.GunGame;
using RoundModifiers.Modifiers.LevelUp;
using RoundModifiers.Modifiers.Nicknames;
using RoundModifiers.Modifiers.RogueAI;
using RoundModifiers.Modifiers.WeaponStats;

namespace RoundModifiers
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        
        [Description("Whether players will be allowed to vote for what modifiers they would like to see next round. Default is true.")]
        public bool EnableVoting { get; set; } = true;
        
        //public string[] DisabledModifiers { get; set; } = new string[0];
        [Description("Whether a broadcast of what modifiers will be applied next round will appear during the lobby. Default is true.")]
        public bool ShowRoundModInLobby{ get; set; } = true;
        [Description("Whether a broadcast of what modifiers are currently active will appear at the start of the round. Default is true.")]
        public bool ShowRoundModInGame{ get; set; } = true;

        public string[] BlacklistedModifiers { get; set; } = new[] {"Blacklisted"};
        
        //Modifiers
        
        
        //Blackout
        [Description("[Obsolete] Duration of the rolling lights effect. Default is 10."), Obsolete]
        public float Blackout_LightRollDuration { get; set; } = 10f;
        
        
        //MicroHIV
        [Description("[Obsolete] The amount of health drained when Charging the MicroHID. Default is 0.75f."), Obsolete]
        public float MicroHIV_ChargeDrain { get; set; } = 0.75f;
        [Description("[Obsolete] The amount of health drained when Firing the MicroHID. Default is 3.5f."), Obsolete]
        public float MicroHIV_FireDrain { get; set; } = 3.5f;
        
        //GunGame
        [Description("[Obsolete] Should guns be given in a sequential order (true) or randomly (false)? Default is true."), Obsolete]
        public bool GunGame_Sequential { get; set; } = true;
        
        
        //RandomSpawnSize
        [Description("[Obsolete] The minimum and maximum size of spawned objects. Default is 0.5f and 1.2f"), Obsolete]
        public float RandomSpawnSize_SizeMin { get; set; } = 0.5f;
        public float RandomSpawnSize_SizeMax { get; set; } = 1.2f;
        [Description("[Obsolete] Should the RandomSpawnSize modifier affect pickups? Default is true."), Obsolete]
        public bool RandomSpawnSize_AffectPickups { get; set; } = true;
        [Description("[Obsolete] Should the RandomSpawnSize modifier scale uniformly? Default is false."), Obsolete]
        public bool RandomSpawnSize_Uniform { get; set; } = false;
        
        
        //Rainbow
        [Description("[Obsolete] The speed of the rainbow effect. Default is 0.01f"), Obsolete]
        public float Rainbow_Speed { get; set; } = 0.01f;
        [Description("[Obsolete] How long a color will stay before changing. Default is 3f."), Obsolete]
        public float Rainbow_ColorChangeTime { get; set; } = 3f;
        [Description("[Obsolete] The minimum and maximum RGB values for the rainbow effect. Default is 0.1f and 2f"), Obsolete]
        public float Rainbow_MinRGB { get; set; } = 0.1f;
        public float Rainbow_MaxRGB { get; set; } = 2f;
        
        //Imposter
        [Description("[Obsolete] The amount of time in seconds an SCP is revealed after being hurt. Default is 30f."), Obsolete]
        public float Imposter_RevealTimeOnHurt { get; set; } = 30f;
        [Description("[Obsolete] Whether to skip the little jump when an SCP is revealed. True: No screen jump when hiding. False: Screen jump, possibly more reliable. Default is false."), Obsolete]
        public bool Imposter_SkipJump { get; set; } = false;
        
        //LevelUp
        [Description("[Obsolete] The base amount of XP needed to level up. Default is 75"), Obsolete]
        public float LevelUp_BaseXP { get; set; } = 75;
        [Description("[Obsolete] The amount of XP needed to level up per level. Default is 25"), Obsolete]
        public float LevelUp_XPPerLevel { get; set; } = 25;
        
        
        //MultiBall
        [Description("[Obsolete] The scale of the MultiBall balls when thrown. Default is 3."), Obsolete]
        public float MultiBall_BallScale { get; set; } = 3;
        [Description("[Obsolete] The chance of a locker spawning SCP-018 instead of the original item. Default is 0.01. (0-1)f"), Obsolete]
        public float MultiBall_LockerSpawnChance { get; set; } = 0.01f;
        
        [Description("[Obsolete] The chance of an SCP-018 ball spawning extra balls when it bounces. Default is 0.01. (0-1)f"), Obsolete]
        public float MultiBall_ExtraBallsChance { get; set; } = 0.01f;
        [Description("[Obsolete] Do the bonus balls from SCP-018 spawn more balls? Default is false."), Obsolete]
        public bool MultiBall_Recursive { get; set; } = false;
        
        
        //NoDecon
        [Description("[Obsolete] The starting multiplier of damage dealt to players in light containment during the NoDecon modifier. Default is 1f."), Obsolete]
        public float NoDecon_DamageMultiplierMin { get; set; } = 1f;
        [Description("[Obsolete] The maximum multiplier of damage dealt to players in light containment during the NoDecon modifier. Default is 10f."), Obsolete]
        public float NoDecon_DamageMultiplierMax { get; set; } = 10f;
        
        
        //Puppies
        [Description("[Obsolete] The scale SCPs should spawn as during the Puppies modifier. Default is 0.5f."), Obsolete]
        public float Puppies_SCPScale { get; set; } = 0.5f;
        //[Description("Health Multiplier for SCPs during the Puppies modifier. Default is 0.5f."), Obsolete]
        //public float Puppies_SCPHealthMultiplier { get; set; } = 0.5f;
        [Description("[Obsolete] How much HP SCP939 should start with during the Puppies modifier. Default is 750f."), Obsolete]
        public float Puppies_Scp939HealthStart { get; set; } = 750f;
        [Description("[Obsolete] How much Hume Shield SCP939 should start with during the Puppies modifier. Default is 500f."), Obsolete]
        public float Puppies_Scp939HumeStart { get; set; } = 500f;
        
        [Description("[Obsolete] Should Scp-079 (Computer) be converted by the Puppies modifier? Default is true."), Obsolete]
        public bool Puppies_AffectScp079 { get; set; } = true;
        [Description("[Obsolete] Should Scp-3114 (Skeleton) be converted by the Puppies modifier? Default is true."), Obsolete]
        public bool Puppies_AffectScp3114 { get; set; } = true;
        [Description("[Obsolete] Should Puppies share voices when any of them get a kill? Default is true."), Obsolete]
        public bool Puppies_ShareVoices { get; set; } = true;
        
        
        
        //Scp249
        [Description("[Obsolete] The amount of time in seconds between door movements during the Scp249 modifier. Default is 30f. Suggest lowering this with larget DoorCount values."), Obsolete]
        public float Scp249_MoveInterval { get; set; } = 30f;
        [Description("[Obsolete] The amount of time in seconds between testing for telepoting a player. Default is 0.1f. Lower numbers will be more accurate but more resource intensive."), Obsolete]
        public float Scp249_TeleportCheckInterval { get; set; } = 0.1f;

        [Description("[Obsolete] Number of doors to place during the Scp249 modifier. Default is 2. Must be 2 or greater."), Obsolete]
        public int Scp249_DoorCount { get; set; } = 2;
        
        [Description("[Obsolete] How much health to give Scp-3114s during BoneZone. Default is 0.5f."), Obsolete]
        public float BoneZone_Scp3114HealthScale { get; set; } = 0.5f;
        
        //MysteryBox
        [Description("[Obsolete] Whether the MysteryBox modifier is enabled. Default is true."), Obsolete]
        public bool MysteryBox_Enable { get; set; } = true;
        
        
        //HealthBars
        [Description("[Obsolete] Whether the HealthBars modifier is enabled. Default is true."), Obsolete]
        public bool HealthBars_Enable { get; set; } = true;
        [Description("[Obsolete] The length of the health bars. Default is 1f."), Obsolete]
        public float HealthBars_Length { get; set; } = 1f;
        [Description("[Obsolete] The vertical offset of the health bars. Default is 1f."), Obsolete]
        public float HealthBars_Height { get; set; } = 1f;
        
        //Nicknames
        [Description("[Obsolete] A list of nicknames to give to players."), Obsolete]
        public string[] Nicknames_HumanNames { get; set; } = new[]
        {
            "Alan", "Steeve", "Mary", "John", "Alice", "Bob", "Carol", "Davis", "Eve", "Frank",
            "Grace", "Helen", "Ian", "Judy", "Kevin", "Linda", "Mike", "Nora", "Oliver", "Patricia",
            "Quinn", "Rachel", "Sam", "Tina", "Ursula", "Vince", "Wendy", "Xavier", "Yvonne", "Zack",
            "Amber", "Bruce", "Cindy", "Derek", "Elena", "Felix", "Gina", "Harry", "Isla", "Justin",
            "Kara", "Leo", "Mona", "Nathan", "Oscar", "Penny", "Quincy", "Rose", "Seth", "Tara",
            "Ulysses", "Victor", "Willa", "Xander", "Yasmin", "Zeke", "April", "Blaine", "Claire",
            "Dante", "Elise", "Frederick", "Gloria", "Howard", "Ingrid", "Joel", "Krista", "Luke",
            "Megan", "Neil", "Opal", "Paul", "Queenie", "Roger", "Susan", "Thomas", "Una", "Vernon",
            "Whitney", "Xenia", "Yuri", "Zara", "Aaron", "Beth", "Carter", "Deanna", "Elliott", "Faye",
            "George", "Hannah", "Ivan", "Jean", "Kyle", "Leslie", "Mitchell", "Nadia", "Owen", "Paula",
            "Quentin", "Ruth", "Spencer", "Tiffany", "Uma", "Vincent", "Wallace", "Xena", "Yvette", "Zion",
            "Taylar", "Ely", "Jason", "Kevin", "Chance", "Vivian"
        };
        
        //ExtraLife
        [Description("[Obsolete] The minimum amount of time in seconds the ExtraLife modifier will ensure the game lasts. Default is 300f."), Obsolete]
        public float ExtraLife_MinimumRoundTime { get; set; } = 300f;
        
        [Description("[Obsolete] The amount of time in seconds before a player is respawned after dying during the ExtraLife modifier. Need delay for 049 to rez players. Default is 24f."), Obsolete]
        public float ExtraLife_RespawnDelay { get; set; } = 24f;
        
        //ScpChat
        [Description("[Obsolete] Whether SCPs can change the state of the ability. Default is true."), Obsolete]
        public bool ScpChat_CanChangeState { get; set; } = true;
        
        [Description("[Obsolete] Whether the backup NPCs should leave the game on death. Default is true. If false, they will can be resurrected or respawn with humans."), Obsolete]
        public bool ScpBackup_LeaveOnDeath { get; set; } = true;
        
        [Description("[Obsolete] Respawn wave behaviour. If false, 1 SCP will be spawned per wave. If true, it will first spawn 1 SCP, then 2 new wave, then 3, and so on. Default is true."), Obsolete]
        public bool ScpBackup_ExponentialRespawns { get; set; } = true;
        
        
        [Description("=====Puppies=====")]
        public Puppies.Config Puppies { get; set; } = new Puppies.Config();
        [Description("=====MultiBall=====")]
        public MultiBall.Config MultiBall { get; set; } = new MultiBall.Config();
        [Description("=====Blackout=====")]
        public Blackout.Config Blackout { get; set; } = new Blackout.Config();
        [Description("=====Scp249=====")]
        public Scp249.Config Scp249 { get; set; } = new Scp249.Config();
        [Description("=====ScpBackup=====")]
        public ScpBackup.Config ScpBackup { get; set; } = new ScpBackup.Config();
        [Description("=====BoneZone=====")]
        public BoneZone.Config BoneZone { get; set; } = new BoneZone.Config();
        [Description("=====ExtraLife=====")]
        public ExtraLife.Config ExtraLife { get; set; } = new ExtraLife.Config();
        [Description("=====ScpChat=====")]
        public ScpChat.Config ScpChat { get; set; } = new ScpChat.Config();
        [Description("=====MicroHIV=====")]
        public MicroHIV.Config MicroHIV { get; set; } = new MicroHIV.Config();
        [Description("=====NoDecon=====")]
        public NoDecontamination.Config NoDecon { get; set; } = new NoDecontamination.Config();
        [Description("=====HealthBars=====")]
        public HealthBars.Config HealthBars { get; set; } = new HealthBars.Config();
        [Description("=====LevelUp=====")]
        public LevelUpConfig LevelUp { get; set; } = new LevelUpConfig();
        [Description("=====Imposter=====")]
        public Imposter.Config Imposter { get; set; } = new Imposter.Config();
        [Description("=====MysteryBox=====")]
        public MysteryBox.Config MysteryBox { get; set; } = new MysteryBox.Config();
        [Description("=====GunGame=====")]
        public GunGame.Config GunGame { get; set; } = new GunGame.Config();
        [Description("=====PayToWin=====")]
        public PayToWin.Config PayToWin { get; set; } = new PayToWin.Config();
        [Description("=====RandomSpawnSize=====")]
        public RandomSpawnSize.Config RandomSpawnSize { get; set; } = new RandomSpawnSize.Config();
        [Description("=====Rainbow=====")]
        public Rainbow.Config Rainbow { get; set; } = new Rainbow.Config();
        [Description("=====RogueAI=====")]
        public RogueAI.Config RogueAI { get; set; } = new RogueAI.Config();
        [Description("=====Nicknames=====")]
        public Nicknames.Config Nicknames { get; set; } = new Nicknames.Config();
        [Description("=====Insurrection=====")]
        public Insurrection.Config Insurrection { get; set; } = new Insurrection.Config();
        
        [Description("=====WeaponStats=====")]
        public WeaponStats2.Config WeaponStats { get; set; } = new WeaponStats2.Config();
        
        [Description("=====ZombieSurvival=====")]
        public ZombieSurvival.Config ZombieSurvival { get; set; } = new ZombieSurvival.Config();

    }
}