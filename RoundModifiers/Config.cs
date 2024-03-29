﻿using System.ComponentModel;
using Exiled.API.Interfaces;

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
        
        
        //Modifiers
        
        
        //Blackout
        [Description("Duration of the rolling lights effect. Default is 10.")]
        public float Blackout_LightRollDuration { get; set; } = 10f;
        
        
        //MicroHIV
        [Description("The amount of health drained when Charging the MicroHID. Default is 0.75f.")]
        public float MicroHIV_ChargeDrain { get; set; } = 0.75f;
        [Description("The amount of health drained when Firing the MicroHID. Default is 3.5f.")]
        public float MicroHIV_FireDrain { get; set; } = 3.5f;
        
        
        //RandomSpawnSize
        [Description("The minimum and maximum size of spawned objects. Default is 0.5f and 1.2f")]
        public float RandomSpawnSize_SizeMin { get; set; } = 0.5f;
        public float RandomSpawnSize_SizeMax { get; set; } = 1.2f;
        [Description("Should the RandomSpawnSize modifier affect pickups? Default is true.")]
        public bool RandomSpawnSize_AffectPickups { get; set; } = true;
        
        
        //Rainbow
        [Description("The speed of the rainbow effect. Default is 0.01f")]
        public float Rainbow_Speed { get; set; } = 0.01f;
        [Description("How long a color will stay before changing. Default is 3f.")]
        public float Rainbow_ColorChangeTime { get; set; } = 3f;
        [Description("The minimum and maximum RGB values for the rainbow effect. Default is 0.1f and 2f")]
        public float Rainbow_MinRGB { get; set; } = 0.1f;
        public float Rainbow_MaxRGB { get; set; } = 2f;
        
        //Imposter
        [Description("The amount of time in seconds an SCP is revealed after being hurt. Default is 30f.")]
        public float Imposter_RevealTimeOnHurt { get; set; } = 30f;
        [Description("Whether to skip the little jump when an SCP is revealed. True: No screen jump when hiding. False: Screen jump, possibly more reliable. Default is false.")]
        public bool Imposter_SkipJump { get; set; } = false;
        
        
        //MultiBall
        [Description("The scale of the MultiBall balls when thrown. Default is 3.")]
        public float MultiBall_BallScale { get; set; } = 3;
        [Description("The chance of a locker spawning SCP-018 instead of the original item. Default is 0.01. (0-1)f")]
        public float MultiBall_LockerSpawnChance { get; set; } = 0.01f;
        
        [Description("The chance of an SCP-018 ball spawning extra balls when it bounces. Default is 0.01. (0-1)f")]
        public float MultiBall_ExtraBallsChance { get; set; } = 0.01f;
        [Description("Do the bonus balls from SCP-018 spawn more balls? Default is false.")]
        public bool MultiBall_Recursive { get; set; } = false;
        
        
        //NoDecon
        [Description("The starting multiplier of damage dealt to players in light containment during the NoDecon modifier. Default is 1f.")]
        public float NoDecon_DamageMultiplierMin { get; set; } = 1f;
        [Description("The maximum multiplier of damage dealt to players in light containment during the NoDecon modifier. Default is 10f.")]
        public float NoDecon_DamageMultiplierMax { get; set; } = 10f;
        
        
        //Puppies
        [Description("The scale SCPs should spawn as during the Puppies modifier. Default is 0.5f.")]
        public float Puppies_SCPScale { get; set; } = 0.5f;
        //[Description("Health Multiplier for SCPs during the Puppies modifier. Default is 0.5f.")]
        //public float Puppies_SCPHealthMultiplier { get; set; } = 0.5f;
        [Description("How much HP SCP939 should start with during the Puppies modifier. Default is 750f.")]
        public float Puppies_Scp939HealthStart { get; set; } = 750f;
        [Description("How much Hume Shield SCP939 should start with during the Puppies modifier. Default is 500f.")]
        public float Puppies_Scp939HumeStart { get; set; } = 500f;
        
        [Description("Should Scp-079 (Computer) be converted by the Puppies modifier? Default is true.")]
        public bool Puppies_AffectScp079 { get; set; } = true;
        [Description("Should Scp-3114 (Skeleton) be converted by the Puppies modifier? Default is true.")]
        public bool Puppies_AffectScp3114 { get; set; } = true;
        
        //Scp249
        [Description("The amount of time in seconds between door movements during the Scp249 modifier. Default is 30f. Suggest lowering this with larget DoorCount values.")]
        public float Scp249_MoveInterval { get; set; } = 30f;
        [Description("The amount of time in seconds between testing for telepoting a player. Default is 0.1f. Lower numbers will be more accurate but more resource intensive.")]
        public float Scp249_TeleportCheckInterval { get; set; } = 0.1f;
        [Description("Number of doors to place during the Scp249 modifier. Default is 2. Must be 2 or greater.")]
        public int Scp249_DoorCount { get; set; } = 2;
        
        //MysteryBox
        [Description("Whether the MysteryBox modifier is enabled. Default is true.")]
        public bool MysteryBox_Enable { get; set; } = true;
        
        
        //HealthBars
        [Description("Whether the HealthBars modifier is enabled. Default is true.")]
        public bool HealthBars_Enable { get; set; } = true;
        [Description("The length of the health bars. Default is 1f.")]
        public float HealthBars_Length { get; set; } = 1f;
        [Description("The vertical offset of the health bars. Default is 1f.")]
        public float HealthBars_Height { get; set; } = 1f;
    }
}