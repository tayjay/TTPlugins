# *SC*ri*P*t Docs (WIP)

## TL;DR

Alright, this page is long, REALLY long. However, everything you may need should be searchable on this page. If you just want to get into coding, jump into the "Getting Started" section, and skim the rest as needed.

## Introduction
This is the definitive guide to using the SCriPt "Lua -> EXILED" plugin for SCP:SL. This guide will cover all the types, events, and global variables that are available to you when writing scripts for the plugin.
Scripts ran by this plugin do not require any kind of compilation, 

This plugin is meant to be a middle ground between plugins in "ScriptedEvents" and making an EXILED plugin. 
ScriptedEvents is an amazing plugin if you want to make a quick game mode as long as you learn their language. I needed something with a little more control.

To start, a solid understanding of Lua is benificial. Programming knowledge in general will also let you get started.
You will also want to understand Event Based systems, as it's the core of the plugin.
For bonus points, knowing how MoonSharp works for its translation of Lua to C# will also be helpful.

The target audience of this plugin is between a few groups:
 - Plugin Developers who want to automate certain tasks on their testing server and not need to change their own code
 - Server Owners who want to add custom functionality to their server without needing to create, commission, or install a plugin
 - Server Owners that have a repetitive task that they believe could be automated

If you are running a verified server, be sure to check <https://scpslgame.com/Verified_server_rules.pdf> to make sure you are not breaking any rules.

### A note on safety

The code ran in this plugin can do anything a user with the "Server Owner" role can do. Privileged functions such as file system access are disabled for your safety, but it is possible to crash the server with infinite loops or cause unwanted changes with other bad code. Be sure to test your code on a local server before running it on a live server.
And if you are accepting scripts from others, be sure to check them for malicious code. (ChatGPT/Gemini are good at spotting logical errors in Lua) If something looks suspicious it's best not to run it, the fun of this plugin is to make it your own.

## Getting Started

### Setting up your environment

1. First off, if you don't have one, we need a game server to run this on. This can be your local computer if you want. Follow NorthWood's instructions to setup a local SCP:SL server. 
2. Then install the latest EXILED version.
3. Get the latest version of the SCriPt.dll plugin from the releases page and place it in your EXILED plugins folder.
4. Get the latest version of MoonSharp.Interpreter.dll from their releases page and place it in your EXILED plugins "dependencies" subfolder.
5. Run the server once, then check the directory where you ran the "LocalAdmin" file. You should see a "Scripts" folder. This is where you will place your lua scripts.
6. Make sure you can connect to the server. Direct Connect to ```127.0.0.1``` if it's running on your local computer.

### Scripts Structure

If you check the "Scripts" folder, you will see a few folders. Here's how they work:

- **Global**: These scripts are ran when any script is loaded, and are meant to be used for custom global variables and functions.
- **AutoLoad**: These scripts are ran when the server starts, all scripts in this folder run in the same context, and can talk to eachother if needed.
- Anything in the root of the "Scripts" folder will not be ran automatically, they can be triggered in RemoteAdmin but someone with the "SCriPt.Execute" permission. They are ran in a seperate context from the AutoLoad files.

### Hello World

Like any good programming tutorial, let's start with a "Hello World" script. Create a new file in the "Scripts/AutoLoad" folder called "HelloWorld.lua" and put the following code in it:

```lua
print('Hello, World!')
```

When you launch the server, you will see ```[Info] [Lua] Hello, World!``` in the console. It doesn't do anything fancy, but if you see this then you're ready to proceed.

### Writing your first script

The start of any good project is a plan. What do you want to do? In this example I want the server to automatically do the following:
- When a player joins, they are greeted with a message

Simple enough, a good way to structure your idea is to keep the event based nature of this plugin in mind, for example:
- When X happens, do Y

Let's look at the code that will do this and break it down. Create a new file in the "Scripts/AutoLoad" folder called "HelloUser.lua" and put the following code in it:

```lua
hello_user = {}

function hello_user:load()
 Events.Player.Joined:add(self.onPlayerJoined)
end

function hello_user:unload()
 Events.Player.Joined:remove(self.onPlayerJoined)
end

function hello_user:onPlayerJoined(args)
    Server:Broadcast("Welcome to the server, " .. args.Player.Nickname .. "!")
    print("Player " .. args.Player.Nickname .. " has joined the server.")
end



hello_user:load()
```
Most of this code is boilerplate, but let's break it down:
- ```hello_user = {}``` creates a table to hold our functions. It's best practice to keep all your functions in a table to avoid conflicts with other scripts. The table name can be any unique name you want.
- ```function:load()...``` and ```function:unload()``` are special functions that are called when the script is loaded and unloaded. This is where you should add and remove your event listeners.
- ```Events.Player.Joined:add(self.onPlayerJoined)``` adds the ```onPlayerJoined``` function to the Player.Joined event. This means that when a player joins the server, the onPlayerJoined function will be called.
- ```function hello_user:onPlayerJoined(args)``` is the function that will be called when a player joins the server. It takes a single argument, args, which contains information about the player that joined.
- ```Server:Broadcast("Welcome to the server, " .. args.Player.Nickname .. "!")``` sends a message to all players on the server welcoming the new player.
- ```hello_user:load()``` calls the load function when the script is loaded, making sure they are ready for their event to trigger.

Now, when a player joins the server, a broadcast message will be sent welcoming them to the server. The print function is used so you can see it happen in the console too.

### Another Example

I miss the Skeleton from Halloween 2023, so I want to add a chance that a player can spawn as Scp3114 at the start of the game. There are plugins that do this already, see my "TTAddons". But let's try it ourselves.

```lua 
-- Start by creating a table to hold our functions
scp3114 = {}

-- Load function, called when the script is loaded
function scp3114:load()
    -- Add the function to the event
    Events.Server.RoundStarted:add(self.onRoundStarted)
end

-- Unload function, called when the script is unloaded
function scp3114:unload()
    -- Remove the function from the event
    Events.Server.RoundStarted:remove(self.onRoundStarted)
end

-- Function called when the round starts
function scp3114:onRoundStarted()
    -- Get a random number between 1 and 100
    local chance = math.random(1, 100)
    -- If the number is less than or equal to 5
    if chance <= 25 then
        -- Get a random player
        local player = Player.GetRandom()
        -- Change the player's role to Scp3114
        player:ChangeRole('Scp3114')
    end
 
    -- This will unregister the event, so it doesn't trigger again. If you want this to continue through restarts you can remove this line.
    self:unload()
end

-- Call the load function
scp3114:load()
```

Put this in a file ```Scp3114.lua```. Now you have a choice
- Place the file in ```Scripts/AutoLoad``` and it will run when the server starts. If you do this I suggest removing the self:unload() line.
- Place the file directly in ```Scripts```. Run it using RemoteAdmin in the lobby if you only want to add him once in awhile. ```luaf Scp3114.lua```



# Globals

The following are global variables that can be accessed from anywhere in the code.

### Cassie
For interacting with the in-game announcer.

| Type | Variable          | Interaction |
| --- |-------------------|-------------|
 | bool | Cassie.IsSpeaking | Get |

| Type | Function               | Arguments                                                                                     |
| --- |------------------------|-----------------------------------------------------------------------------------------------|
| void | Cassie:Message         | _string_ **words** <br/>_bool_ **makeHold** = true <br/>_bool_ **makeNoise** = true <br/>_bool_ **isAllowed** = false |
| void | Cassie:GlitchyMessage  | string message <br/>float glitchChance <br/>float jamChance                                   |
| bool | Cassie:IsValidMessage  | string message                                                                                |
| bool | Cassie:IsValidSentence | string sentence                                                                               |

### Timing
For creating and managing coroutines.

*See Coroutines Section for more info*

| Type | Function             | Arguments                                         |
|------|----------------------|---------------------------------------------------|
| void | Timing:CallDelayed   | float delay <br/>Function action                  |
| void | Timing:CallDelayed   | float delay <br/>Function action <br/>object[] args |
| void | Timing:CallCoroutine | Function action                                    |
| void | Timing:CallCoroutine | Function action <br/>object[] args                 |

### Decon
For interacting with the LCZ decontamination sequence.

| Type  | Function      | Arguments |
|-------|---------------|-----------|
 | void  | Decon:Disable |          |

### Events
For subscribing to events.
```angular2html
See Events section
```

### Facility
For interacting with the Map/Facility.


### Lobby
For interacting with the lobby.

### Player
A helper for interacting with players.

### Role
For interacting with player roles.

### Round
For interacting with the current round.

### Server
For interacting with the server.

| Type | Variable | Interaction |
|------|----------|-------------|
| int | PlayerCount | Get         |
| int | NpcCount | Get         |
| int | ScpCount | Get         |
| int | HumanCount | Get         |
| int | DClassCount | Get         |
| int | ScientistCount | Get         |
| int | FoundationCount | Get         |
| int | ChaosCount | Get         |
| bool | FriendlyFire | Get/Set     |
| List<Player> | Players | Get         |

| Type | Function             | Arguments                        |
|------|----------------------|----------------------------------|
| string | Server:RACommand     | string command                   |
| void | Server:LACommand     | string command                   |
| void | Server:Restart       |                                  |
| void | Server:Shutdown      |                                  |
| void | Server:SendBroadcast | string message <br/>float duration |

### Warhead
For interacting with the warhead.


# Events
Events can be subscribed to in order to run code when a certain action is taken in game. To use this, you need to create a function that will accept the arguments given by the event
```lua
function MyFunction(args)
    -- Do something
end
```
Then you can subscribe to the event like so
```lua
Events.Item.KeycardInteracting:add(MyFunction)
```
When the event is triggered, your function will be called. Information on what arguments are passed to the function can be found in the event's documentation below.

## Cassie
**Events.Cassie.SendingCassieMessage**

| Type | Member   |
| --- |----------|
| string | Words    |
| bool | MakeHold |
| bool | MakeNoise |
| bool | IsAllowed |
---
## Item
**Events.Item.ChangingAmmo**

| Type | Member   |
| --- |----------|
| Player | Player |
| Firearm | Firearm |
| Item | Item |
| byte | OldAmmo |
| byte | NewAmmo |
| bool | IsAllowed |

**Events.Item.ChangingAttachments**

| Type | Member   |
| --- |----------|
| IEnumerable<AttachmentIdentifier> | CurrentAttachmentIdentifiers |
| List<AttachmentIdentifier> | NewAttachmentIdentifiers |
| uint | CurrentCode |
| uint | NewCode |
| bool | IsAllowed |
| Firearm | Firearm |
| Item | Item |
| Player | Player |

**Events.Item.ReceivingPreference**

| Type | Member   |
| --- |----------|
| FirearmType | Item |
| IEnumerable<AttachmentIdentifier> | CurrentAttachmentIdentifiers |
| List<AttachmentIdentifier> | NewAttachmentIdentifiers |
| uint | CurrentCode |
| uint | NewCode |
| bool | IsAllowed |
| Player | Player |

**KeycardInteracting**

| Type | Member   |
| --- |----------|
| Pickup | Pickup |
| Player | Player |
| Door | Door |
| bool | IsAllowed |

**Swinging**

| Type | Member   |
| --- |----------|
| Player | Player |
| Item | Item |
| bool | IsAllowed |

**ChargingJailbird**

| Type | Member   |
| --- |----------|
| Player | Player |
| Item | Item |
| bool | IsAllowed |

**UsingRadioPickupBattery**

| Type | Member   |
| --- |----------|
| bool | IsAllowed |
| Pickup | Pickup |
| RadioPickup | RadioPickup |
| float | Drain |
---
## Map
**Events.Map.PlacingBulletHole**

| Type | Member   |
| --- |----------|
| Vector3 | Position |
| Quaternion | Rotation |
| bool | IsAllowed |
| Player | Player |
    
**PlacingBlood**
    
| Type | Member   |
| --- |----------|
| Player | Player |
| Player | Target |
| Vector3 | Position |
| bool | IsAllowed |
    
**AnnouncingDecontamination**

| Type | Member   |
| --- |----------|
| int | Id |
| DecontaminationState | State |
| DecontaminationController.DecontaminationPhase.PhaseFunction | PhaseFunction |

**AnnouncingScpTermination**

| Type | Member   |
| --- |----------|
| Role | Role |
| string | TerminationCause |
| Player | Player |
| Player | Attacker |
| CustomDamageHandler | DamageHandler |
| bool | IsAllowed |

**AnnouncingNtfEntrance**

| Type | Member   |
| --- |----------|
| int | ScpsLeft |
| string | UnitName |
| int | UnitNumber |
| bool | IsAllowed |

**GeneratorActivating**

| Type | Member   |
| --- |----------|
| Generator | Generator |
| bool | IsAllowed |

**Decontaminating**

| Type | Member   |
| --- |----------|
| bool | IsAllowed |

**ExplodingGrenade**

| Type | Member   |
| --- |----------|
| Vector3 | Position |
| List<Player> | TargetsToAffect |
| EffectGrenadeProjectile | Projectile |
| bool | IsAllowed |
| Player | Player |

**SpawningItem**

| Type | Member   |
| --- |----------|
| Pickup | Pickup |
| bool | ShouldInitiallySpawn |
| Door | TriggerDoor |
| bool | IsAllowed |

**FillingLocker**

| Type | Member   |
| --- |----------|
| Pickup | Pickup |
| LockerChamber | LockerChamber |
| bool | IsAllowed |

**Generated**

| Type | Member   |
| --- |----------|


**ChangingIntoGrenade**

| Type | Member   |
| --- |----------|
| Pickup | Pickup |
| ItemType | Type |
| bool | IsAllowed |

**ChangedIntoGrenade**

| Type | Member   |
| --- |----------|
| GrenadePickup | Pickup |
| Projectile | Projectile |
| double | FuseTime (DEPRICATED) |

**TurningOffLights**

| Type | Member   |
| --- |----------|
| RoomLightController | RoomLightController |
| float | Duration |
| bool | IsAllowed |

**PickupAdded**

| Type | Member   |
| --- |----------|
| Pickup | Pickup |

**PickupDestroyed**
    
| Type | Member   |
| --- |----------|
| Pickup | Pickup |

**SpawningTeamVehicle**

| Type | Member   |
| --- |----------|
| SpawnableTeamType | Team |
| bool | IsAllowed |
---
## Player

**PreAuthenticating**

| Type | Member   |
| --- |----------|
| string | UserId |
| string | IpAddress |
| long | Expiration |
| CentralAuthPreauthFlags | Flags |
| string | Country |
| byte[] | Signature |
| int | ReaderStartPosition |
| ConnectionRequest | Request |
| bool | IsAllowed |

**ReservedSlot**

| Type | Member   |
| --- |----------|
| string | UserId |
| bool | HasReservedSlot |

**Kicking**

| Type | Member   |
| --- |----------|
| Player | Target |
| string | Reason |
| string | FullMessage |
| bool | IsAllowed |

**Kicked**

| Type | Member   |
| --- |----------|
| Player | Player |
| string | Reason |
    
**Banning**
    
| Type | Member   |
| --- |----------|
| long | Duration |
| Player | Target |
| string | Reason |
| string | FullMessage |
| bool | IsAllowed |
| Player | Player |

**Banned**

| Type | Member   |
| --- |----------|
| Player | Target |
| Player | Player |
| BanDetails | Details |
| BanHandler.BanType | Type |
| bool | IsForced |
    
**EarningAchievement**

| Type | Member   |
| --- |----------|
| AchievementName | AchievementName |
| bool | IsAllowed |
| Player | Player |

**UsingItem**

| Type | Member   |
| --- |----------|
| Usable | Usable |
| Item | Item |
| Player | Player |
| float | Cooldown |
| bool | IsAllowed |

**UsingItemCompleted**

**UsedItem**

**CancellingItemUse**

**CancelledItemUse**

**Interacted**

**SpawnedRagdoll**

**ActivatingWarheadPanel**

**ActivatingWorkstation**

**Joined**

**Verified**

**Left**

**Destroying**

**Hurting**

**Hurt**

**Dying**

**Died**

**ChangingRole**

**ThrownProjectile**

**ThrowingRequest**

**DroppingItem**

**DroppedItem**

**DroppingNothing**

**PickingUpItem**

**Handcuffing**

**RemovingHandcuffs**

**IntercomSpeaking**

**Shot**

**Shooting**

**EnteringPocketDimension**

**EscapingPocketDimension**

**FailingEscapePocketDimension**

**EnteringKillerCollision**

**ReloadingWeapon**

**Spawning**

**Spawned**

**ChangedItem**

**ChangingItem**

**ChangingGroup**

**InteractingDoor**

**InteractingElevator**

**InteractingLocker**

**TriggeringTesla**

**UnlockingGenerator**

**OpeningGenerator**

**ClosingGenerator**

**ActivatingGenerator**

**StoppingGenerator**

**ReceivingEffect**

**IssuingMute**

**RevokingMute**

**UsingRadioBattery**

**ChangingRadioPreset**

**UsingMicroHIDEnergy**

**DroppingAmmo**

**DroppedAmmo**

**InteractingShootingTarget**

**DamagingShootingTarget**

**FlippingCoin**

**TogglingFlashlight**

**UnloadingWeapon**

**AimingDownSight**

**TogglingWeaponFlashlight**

**DryfiringWeapon**

**VoiceChatting**

**MakingNoise**

**Jumping**

**Landing**

**Transmitting**

**ChangingMoveState**

**ChangingSpectatedPlayer**

**TogglingNoClip**

**TogglingOverwatch**

**TogglingRadio**

**SearchingPickup**

**SendingAdminChatMessage**

**PlayerDamageWindow**

**DamagingDoor**

**ItemAdded**

**ItemRemoved**

**KillingPlayer**

**EnteringEnvironmentalHazard**

**StayingOnEnvironmentalHazard**

**ExitingEnvironmentalHazard**

**ChangingNickname**

---

## Scp049

**FinishingRecall**

**StartingRecall**

**ActivatingSense**

**SendingCall**

**Attacking**

---

## Scp0492

**TriggeringBloodlust**

**ConsumedCorpse**

**ConsumingCorpse**

---

## Scp079

**ChangingCamera**

**GainingExperience**

**GainingLevel**

**InteractingTesla**

**TriggeringDoor**

**ElevatorTeleporting**

**LockingDown**

**ChangingSpeakerStatus**

**Recontained**

**Pinging**

**RoomBlackout**

**ZoneBlackout**

---

## Scp096

**Enraging**

**CalmingDown**

**AddingTarget**

**StartPryingGate**

**Charging**

**TryingNotToCry**

---

## Scp106

**Attacking**

**Teleporting**

**Stalking**

**ExitStalking**

---

## Scp173

**Blinking**

**BlinkingRequest**

**PlacingTantrum**

**UsingBreakneckSpeeds**

---

## Scp244

**UsingScp244**

**DamagingScp244**

**OpeningScp244**

---

## Scp330

**InteractingScp330**

**DroppingScp330**

**EatingScp330**

**EatenScp330**

---

## Scp914

**UpgradingPickup**

**UpgradingInventoryItem**

**UpgradingPlayer**

**Activating**

---

## Scp939

**ChangingFocus**

**Lunging**

**PlacingAmnesticCloud**

**PlayingVoice**

**SavingVoice**

**PlayingSound**

**Clawed**

**ValidatingVisibility**

---

## Scp3114

**Disguising**

**Disguised**

**TryUseBody**

**Revealed**

**Revealing**

**VoiceLines**

---

## Server

**WaitingForPlayers**

**RoundStarted**

**EndingRound**

**RoundEnded**

**RestartingRound**

**ReportingCheater**

**RespawningTeam**

**AddingUnitName**

**LocalReporting**

**ChoosingStartTeamQueue**

**ReloadedConfigs**

**ReloadedTranslations**

**ReloadedGameplay**

**ReloadedRA**

**ReloadedPlugins**

**ReloadedPermissions**

---

## Warhead

**Stopping**

**Starting**

**ChangingLeverStatus**

**Detonated**

**Detonating**

---
