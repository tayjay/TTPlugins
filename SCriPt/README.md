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

More docs

https://exiled-team.github.io/EXILED/api/Exiled.API.Enums.html

## Disclaimer

This plugin enables the ability to use Lua scripts for task automation. Please be advised of the following:

- Potential for Abuse: While Lua scripts offer valuable automation tools, they can also be misused. Misconfigured or intentionally malicious scripts may cause disruptions, in-game issues, or stability problems within the server environment.
- Responsibility: Users are fully responsible for the Lua scripts they write, execute, or share. Neither the server owners nor the modification creators are liable for any damages or negative consequences arising from the use of these scripts.
- Use at Your Own Risk: You agree to use this modification and any associated scripting features at your own risk. Always exercise caution and thoroughly test scripts in a safe environment before deployment.
- No Guarantees: The server owners and modification creators make no guarantees regarding the safety, reliability, or error-free performance of user-created scripts.

### Important Reminders

- Back up important data before executing any new or unknown scripts.
- Research Lua scripting principles and best practices for secure coding.
- Never run scripts from untrusted sources.
- By using this plugin, you acknowledge and agree to the terms of this disclaimer.

## Getting Started

### Setting up your environment

1. First off, if you don't have one, we need a game server to run this on. This can be your local computer if you want. Follow [NorthWood's Instructions](https://en.scpslgame.com/index.php?title=Guide:Hosting_a_server) to setup a local SCP:SL server. 
2. Then install the latest [EXILED](https://github.com/Exiled-Team/EXILED) version.
3. Get the latest version of the SCriPt.dll plugin from the releases page and place it in your EXILED plugins folder.
4. Get the latest release of [MoonSharp.Interpreter.dll](https://github.com/moonsharp-devs/moonsharp/releases/latest) (Currently 2.0.0.0) from their releases page and place it in your EXILED plugins "dependencies" subfolder.
5. Run the server once, then check the directory where you ran the `LocalAdmin` file. You should see a `Scripts` folder. This is where you will place your lua scripts.
6. Add your SteamID to the Owner permission on the server.
7. Make sure you can connect to the server. Direct Connect to ```127.0.0.1``` if it's running on your local computer.


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

```
Most of this code is boilerplate, but let's break it down:
- ```hello_user = {}``` creates a table to hold our functions. It's best practice to keep all your functions in a table to avoid conflicts with other scripts. The table name can be any unique name you want.
- ```hello_user:load()...``` and ```hello_user:unload()``` are special functions that are called when the script is loaded and unloaded. This is where you should add and remove your event listeners. They are **automatically** called when the scripts loads and unloads.
- ```Events.Player.Joined:add(self.onPlayerJoined)``` adds the ```onPlayerJoined``` function to the Player.Joined event. This means that when a player joins the server, the onPlayerJoined function will be called.
- ```function hello_user:onPlayerJoined(args)``` is the function that will be called when a player joins the server. It takes a single argument, args, which contains information about the player that joined.
- ```Server:Broadcast("Welcome to the server, " .. args.Player.Nickname .. "!")``` sends a message to all players on the server welcoming the new player.

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
    -- If the number is less than or equal to 25
    if chance <= 25 then
        -- Get a random player
        local player = Player.GetRandom()
        -- Change the player's role to Scp3114
        player:ChangeRole('Scp3114')
    end
 
    -- This will unregister the event, so it doesn't trigger again. If you want this to continue through restarts you can remove this line.
    self:unload()
end
```

Put this in a file ```Scp3114.lua```. Now you have a choice
- Place the file in ```Scripts/AutoLoad``` and it will run when the server starts. If you do this I suggest removing the self:unload() line.
- Place the file directly in ```Scripts```. Run it using RemoteAdmin in the lobby if you only want to add him once in awhile. ```script load Scp3114.lua```

### Custom Commands?

Technically, this plugin can create custom command, both general player and RemoteAdmin. Let's try an example:

Let's do something simple, a command that lets any player send a 1 word CASSIE anncouncement. 

```lua
-- Start by creating a table to hold our functions
cassie = {}

-- Load function, called when the script is loaded
function cassie:load()
    -- Add the function to the event
    Events.Command.PlayerGameConsoleCommandExecuted:add(self.onPlayerGameConsoleCommandExecuted)
end

-- Unload function, called when the script is unloaded
function cassie:unload()
    -- Remove the function from the event
    Events.Command.PlayerGameConsoleCommandExecuted:remove(self.onPlayerGameConsoleCommandExecuted)
end

-- Function called when a player uses a command
function cassie:onPlayerGameConsoleCommandExecuted(args)
    -- Check if the command is !cassie
    if args.Command == "!cassie" then
        if args.Arguments.Length ~= 1 then
            -- Send a response to the player if the command is invalid
            args.Response = "Usage: !cassie <word>"
            args.Result = false
            return
        end
        -- Get the message the player wants to send
        local message = args.Arguments[1]
        -- Check if the message is valid
        if Cassie:IsValidWord(message) then
            -- Send the message through Cassie
            -- message, makeHold, makeNoise, isSubtitles
            Cassie:Message(message,true, false, true)
            -- Send a response to the player
            args.Response = "Message sent to Cassie."
            args.Result = true
        else
            -- Send a response to the player if the message is invalid
            args.Response = "Invalid message."
            args.Result = false
        end
    end
end
```

Now if a player uses the `~` console, they can type ```!cassie <word>``` and it will be sent to Cassie. I wouldn't recommend using the command on a large server, but it servers as an example.

## Using other Plugins

I am currently working on an API that will allow developers to add events and globals to your code.
For now though, as long as whatever you need to do can be ran by in the RA console you can use the following:

    Server:RACommand("command")

Variables from your Lua code can be concatenated onto the command as needed, such as PlayerIDs.


# Globals

The following are global variables that can be accessed from anywhere in the code.

### AdminToys
For spawning and interacting with Admin Toys. These are Unity Primitive objects that can be spawned in the game such as cubes, spheres, and lights.

| Type      | Function          | Arguments                                                                                           |
|-----------|-------------------|-----------------------------------------------------------------------------------------------------|
| Primitive | AdminToys:Spawn   | string name <br/>Vector3 position <br/>Quaternion rotation <br/>Color color                         |
| Primitive | AdminToys:Destroy | string name                                                                                         |
| Primitive | AdminToys:Create  | PrimitiveType type <br/>Vector3 position <br/>Vector3 rotation <br/> Vector3 scale <br/>Color color |

### Cassie
For interacting with the in-game announcer.

| Type | Variable          | Interaction |
| --- |-------------------|-------------|
 | bool | Cassie.IsSpeaking | Get |

| Type | Function               | Arguments                                                                                                               |
| --- |------------------------|-------------------------------------------------------------------------------------------------------------------------|
| void | Cassie:Message         | _string_ **words** <br/>_bool_ **makeHold** = true <br/>_bool_ **makeNoise** = true <br/>_bool_ **isSubtitles** = false |
| void | Cassie:GlitchyMessage  | string message <br/>float glitchChance <br/>float jamChance                                                             |
| bool | Cassie:IsValidWord     | string word                                                                                                             |
| bool | Cassie:IsValidSentence | string sentence                                                                                                         |

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
```
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

| Type         | Variable | Interaction |
|--------------|----------|-------------|
| int          | PlayerCount | Get         |
| int          | NpcCount | Get         |
| int          | ScpCount | Get         |
| int          | HumanCount | Get         |
| int          | DClassCount | Get         |
| int          | ScientistCount | Get         |
| int          | FoundationCount | Get         |
| int          | ChaosCount | Get         |
| bool         | FriendlyFire | Get/Set     |
| List(Player) | Players | Get         |
| List(Player) | RemoteAdmins | Get         |
| double       | Tps | Get         |
| double       | Frametime | Get         |
| string       | Ip | Get         |
| string       | Port | Get         |
| string       | Name | Get         |
| string       | Version | Get         |
| bool         | StreamingAllowed | Get |
| bool         | IsBeta | Get |
| bool         | IsIdleModeEnabled | Get/Set |


| Type | Function             | Arguments                        |
|------|----------------------|----------------------------------|
| string | Server:RACommand     | string command                   |
| void | Server:LACommand     | string command                   |
| void | Server:Restart       |                                  |
| void | Server:Shutdown      |                                  |
| void | Server:SendBroadcast | string message <br/>float duration |
| object | Server:GetSessionVariable | string key |
| void | Server:SetSessionVariable | string key <br/>object value |

### Warhead
For interacting with the warhead.


# Events
Events can be subscribed to in order to run code when a certain action is taken in game. To use this, you need to create a function that will accept the arguments given by the event
```lua
function MyFunction(args)
    -- Do something
end
```
Then you can subscribe/unsubscribed to the event like so
```lua
Events.Item.KeycardInteracting:add(MyFunction)
Events.Item.KeycardInteracting:remove(MyFunction)
```
When the event is triggered, your function will be called. Information on what arguments are passed to the function can be found in the event's documentation below.

Some events have an alias associated. For example, `Events.Scp049...` can also be accessed as `Events.Doctor...`

## Cassie
Events related to the in-game announcer.

**Events.Cassie.SendingCassieMessage**

| Type | Member   |
| --- |----------|
| string | Words    |
| bool | MakeHold |
| bool | MakeNoise |
| bool | IsAllowed |
---

## Command
Events related to commands being received and executed.

**Events.Command.RemoteAdminCommand**

| Type | Member   |
| --- |----------|
| ICommandSender | Sender |
| string | Command |
| string[] | Arguments |

**Events.Command.RemoteAdminCommandExecuted**

| Type | Member    |
| --- |-----------|
| ICommandSender | Sender    |
| string | Command   |
| string[] | Arguments |
| bool | Result    |
| string | Response  |

**Events.Command.ConsoleCommand**

| Type | Member   |
| --- |----------|
| ICommandSender | Sender |
| string | Command |
| string[] | Arguments |

**Events.Command.ConsoleCommandExecuted**

| Type | Member    |
| --- |-----------|
| ICommandSender | Sender    |
| string | Command   |
| string[] | Arguments |
| bool | Result    |
| string | Response  |

**Events.Command.PlayerGameConsoleCommand**

| Type | Member   |
| --- |----------|
| Player | Player |
| string | Command |
| string[] | Arguments |

**Events.Command.PlayerGameConsoleCommandExecuted**

| Type | Member    |
| --- |-----------|
| Player | Player    |
| string | Command   |
| string[] | Arguments |
| bool | Result    |
| string | Response  |


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

**Events.Item.KeycardInteracting**

| Type | Member   |
| --- |----------|
| Pickup | Pickup |
| Player | Player |
| Door | Door |
| bool | IsAllowed |

**Events.Item.Swinging**

| Type | Member   |
| --- |----------|
| Player | Player |
| Item | Item |
| bool | IsAllowed |

**Events.Item.ChargingJailbird**

| Type | Member   |
| --- |----------|
| Player | Player |
| Item | Item |
| bool | IsAllowed |

**Events.Item.UsingRadioPickupBattery**

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
    
**Events.Map.PlacingBlood**
    
| Type | Member   |
| --- |----------|
| Player | Player |
| Player | Target |
| Vector3 | Position |
| bool | IsAllowed |
    
**Events.Map.AnnouncingDecontamination**

| Type | Member   |
| --- |----------|
| int | Id |
| DecontaminationState | State |
| DecontaminationController.DecontaminationPhase.PhaseFunction | PhaseFunction |

**Events.Map.AnnouncingScpTermination**

| Type | Member   |
| --- |----------|
| Role | Role |
| string | TerminationCause |
| Player | Player |
| Player | Attacker |
| CustomDamageHandler | DamageHandler |
| bool | IsAllowed |

**Events.Map.AnnouncingNtfEntrance**

| Type | Member   |
| --- |----------|
| int | ScpsLeft |
| string | UnitName |
| int | UnitNumber |
| bool | IsAllowed |

**Events.Map.GeneratorActivating**

| Type | Member   |
| --- |----------|
| Generator | Generator |
| bool | IsAllowed |

**Events.Map.Decontaminating**

| Type | Member   |
| --- |----------|
| bool | IsAllowed |

**Events.Map.ExplodingGrenade**

| Type | Member   |
| --- |----------|
| Vector3 | Position |
| List<Player> | TargetsToAffect |
| EffectGrenadeProjectile | Projectile |
| bool | IsAllowed |
| Player | Player |

**Events.Map.SpawningItem**

| Type | Member   |
| --- |----------|
| Pickup | Pickup |
| bool | ShouldInitiallySpawn |
| Door | TriggerDoor |
| bool | IsAllowed |

**Events.Map.FillingLocker**

| Type | Member   |
| --- |----------|
| Pickup | Pickup |
| LockerChamber | LockerChamber |
| bool | IsAllowed |

**Events.Map.Generated**

| Type | Member   |
| --- |----------|


**Events.Map.ChangingIntoGrenade**

| Type | Member   |
| --- |----------|
| Pickup | Pickup |
| ItemType | Type |
| bool | IsAllowed |

**Events.Map.ChangedIntoGrenade**

| Type | Member   |
| --- |----------|
| GrenadePickup | Pickup |
| Projectile | Projectile |
| double | FuseTime (DEPRICATED) |

**Events.Map.TurningOffLights**

| Type | Member   |
| --- |----------|
| RoomLightController | RoomLightController |
| float | Duration |
| bool | IsAllowed |

**Events.Map.PickupAdded**

| Type | Member   |
| --- |----------|
| Pickup | Pickup |

**Events.Map.PickupDestroyed**
    
| Type | Member   |
| --- |----------|
| Pickup | Pickup |

**Events.Map.SpawningTeamVehicle**

| Type | Member   |
| --- |----------|
| SpawnableTeamType | Team |
| bool | IsAllowed |
---
## Player

**Events.Player.PreAuthenticating**

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

**Events.Player.ReservedSlot**

| Type | Member   |
| --- |----------|
| string | UserId |
| bool | HasReservedSlot |

**Events.Player.Kicking**

| Type | Member   |
| --- |----------|
| Player | Target |
| string | Reason |
| string | FullMessage |
| bool | IsAllowed |

**Events.Player.Kicked**

| Type | Member   |
| --- |----------|
| Player | Player |
| string | Reason |
    
**Events.Player.Banning**
    
| Type | Member   |
| --- |----------|
| long | Duration |
| Player | Target |
| string | Reason |
| string | FullMessage |
| bool | IsAllowed |
| Player | Player |

**Events.Player.Banned**

| Type | Member   |
| --- |----------|
| Player | Target |
| Player | Player |
| BanDetails | Details |
| BanHandler.BanType | Type |
| bool | IsForced |
    
**Events.Player.EarningAchievement**

| Type | Member   |
| --- |----------|
| AchievementName | AchievementName |
| bool | IsAllowed |
| Player | Player |

**Events.Player.UsingItem**

| Type | Member   |
| --- |----------|
| Usable | Usable |
| Item | Item |
| Player | Player |
| float | Cooldown |
| bool | IsAllowed |

**Events.Player.UsingItemCompleted**

| Type | Member   |
| --- |----------|
| Usable | Usable |
| Item | Item |
| Player | Player |
| bool | IsAllowed |

**Events.Player.UsedItem**

| Type | Member   |
| --- |----------|
| Usable | Usable |
| Item | Item |
| Player | Player |


**Events.Player.CancellingItemUse**

| Type | Member   |
| --- |----------|
| Usable | Usable |
| Item | Item |
| Player | Player |
| bool | IsAllowed |

**Events.Player.CancelledItemUse**

| Type | Member   |
| --- |----------|
| Usable | Usable |
| Item | Item |
| Player | Player |

**Events.Player.Interacted**

| Type | Member   |
| --- |----------|
| Player | Player |

**Events.Player.SpawnedRagdoll**

| Type | Member   |
| --- |----------|
| Vector3 | Position |
| Quaternion | Rotation |
| RoleTypeId | Role |
| double | CreationTime |
| string | Nickname |
| RagdollData | Info |
| DamageHandlerBase | DamageHandlerBase |
| Ragdoll | Ragdoll |
| Player | Player |

**Events.Player.ActivatingWarheadPanel**

| Type | Member   |
| --- |----------|
| Player | Player |
| bool | IsAllowed |

**Events.Player.ActivatingWorkstation**

| Type | Member   |
| --- |----------|
| Player | Player |
| WorkstationController | WorkstationController |
| WorkstationController.WorkstationStatus| Status |
| bool | IsAllowed |

**Events.Player.DeactivatingWorkstation**

| Type | Member   |
| --- |----------|
| Player | Player |
| WorkstationController | WorkstationController |
| WorkstationController.WorkstationStatus| Status |
| bool | IsAllowed |

**Events.Player.Joined**

| Type | Member   |
| --- |----------|
| Player | Player |

**Events.Player.Verified**

| Type | Member   |
| --- |----------|
| Player | Player |

**Events.Player.Left**

| Type | Member   |
| --- |----------|
| Player | Player |

**Events.Player.Destroying**

| Type | Member   |
| --- |----------|
| Player | Player |

**Events.Player.Hurting**

| Type | Member   |
| --- |----------|
| Player | Player   |
| Player | Attacker |
| float | Amount |
| CustomDamageHandler | DamageHandler |
| bool | IsAllowed |

**Events.Player.Hurt**

| Type | Member   |
| --- |----------|
| Player | Player   |
| Player | Attacker |
| float | Amount |
| PlayerStatsSystem.DamageHandlerBase.HandlerOutput | HandlerOutput |
| CustomDamageHandler | DamageHandler |

**Events.Player.Dying**

| Type | Member                     |
| --- |----------------------------|
| List<Item> | ItemsToDrop (Set Obsolete) |
| Player | Player                   |
| Player | Attacker                 |
| CustomDamageHandler | DamageHandler |
| bool | IsAllowed                 |

**Events.Player.Died**

| Type | Member   |
| --- |----------|
| Player | Player   |
| Player | Attacker |
| RoleTypeId | TargetOldRole |
| CustomDamageHandler | DamageHandler |

**Events.Player.ChangingRole**

| Type       | Member   |
|------------|----------|
| Player     | Player |
| RoleTypeId | NewRole |
| SpawnReason | Reason |
| List<ItemType> | Items |
| Dictionary<ItemType, short> | Ammo |
| bool | ShouldPreserveInventory |
| RoleSpawnFlags | SpawnFlags |
| bool | IsAllowed |

**Events.Player.ThrownProjectile**

| Type | Member   |
| --- |----------|
| Player | Player |
| Throwable | Throwable |
| Item | Item |
| Pickup | Pickup |

**Events.Player.ThrowingRequest**

| Type | Member   |
| --- |----------|
| Player | Player |
| Throwable | Throwable |
| Item | Item |
| ThrowRequest | RequestType |

**Events.Player.DroppingItem**

| Type | Member   |
| --- |----------|
| Player | Player |
| Item | Item |
| bool | IsAllowed |
| bool | IsThrown |

**Events.Player.DroppedItem**

| Type | Member   |
| --- |----------|
| Player | Player |
| Pickup | Pickup |
| bool | WasThrown |

**Events.Player.DroppingNothing**

| Type | Member   |
| --- |----------|
| Player | Player |

**Events.Player.PickingUpItem**

| Type | Member   |
| --- |----------|
| Player | Player |
| Pickup | Pickup |
| bool | IsAllowed |

**Events.Player.Handcuffing**

| Type | Member   |
| --- |----------|
| Player | Player |
| Player | Target |
| bool | IsAllowed |

**Events.Player.RemovingHandcuffs**

| Type | Member   |
| --- |----------|
| Player | Player |
| Player | Target |
| bool | IsAllowed |

**Events.Player.IntercomSpeaking**

| Type | Member   |
| --- |----------|
| Player | Player |
| bool | IsAllowed |

**Events.Player.Shot**

| Type | Member   |
| --- |----------|
| Player | Player |
| Firearm | Firearm |
| Item | Item |
| HitboxIdentity | Hitbox |
| float | Damage |
| float | Distance |
| Vector3 | Position |
| RaycastHit | RaycastHit |
| Player | Target |
| bool | CanHurt |

**Events.Player.Shooting**

| Type | Member   |
| --- |----------|
| Player | Player |
| Firearm | Firearm |
| Item | Item |
| ShotMessage | ShotMessage |
| uint | TargetNetId |
| bool | IsAllowed |


**Events.Player.EnteringPocketDimension**

| Type | Member   |
| --- |----------|
| Player | Player |
| bool | IsAllowed |
| Player | Scp106 |

**Events.Player.EscapingPocketDimension**

| Type | Member   |
| --- |----------|
| Player | Player |
| Vector3 | TeleportPosition |
| bool | IsAllowed |

**Events.Player.FailingEscapePocketDimension**

| Type | Member   |
| --- |----------|
| Player | Player |
| bool | IsAllowed |
| PocketDimensionTeleport | Teleporter |

**Events.Player.EnteringKillerCollision**

| Type | Member   |
| --- |----------|
| Player | Player |
| bool | IsAllowed |

**Events.Player.ReloadingWeapon**

| Type | Member   |
| --- |----------|
| Player | Player |
| Firearm | Firearm |
| Item | Item |
| bool | IsAllowed |

**Events.Player.Spawning**

| Type | Member   |
| --- |----------|
| Player | Player |
| Role | OldRole |
| Vector3 | Position |
| float | HorizontalRotation |

**Events.Player.Spawned**

| Type | Member   |
| --- |----------|
| Player | Player |
| Role | OldRole |
| SpawnReason | Reason |
| RoleSpawnFlags | SpawnFlags |

**Events.Player.ChangedItem**

| Type | Member   |
| --- |----------|
| Player | Player |
| Item | Item |
| Item | OldItem |

**Events.Player.ChangingItem**

| Type | Member   |
| --- |----------|
| Player | Player |
| Item | Item |
| bool | IsAllowed |

**Events.Player.ChangingGroup**

| Type | Member   |
| --- |----------|
| Player | Player |
| UserGroup | NewGroup |
| bool | IsAllowed |

**Events.Player.InteractingDoor**

| Type | Member   |
| --- |----------|
| Player | Player |
| Door | Door |
| bool | IsAllowed |

**Events.Player.InteractingElevator**

| Type | Member   |
| --- |----------|
| Player | Player |
| ElevatorChamber | Elevator |
| Lift | Lift |
| bool | IsAllowed |

**Events.Player.InteractingLocker**

| Type | Member   |
| --- |----------|
| Player | Player |
| Locker | Locker |
| LockerChamber | Chamber |
| byte | ChamberId |
| bool | IsAllowed |


**Events.Player.TriggeringTesla**

| Type | Member   |
| --- |----------|
| Player | Player |
| TeslaGate | Tesla |
| bool | IsInHurtingRange |
| bool | IsTriggerable |
| bool | IsInIdleRange |
| bool | IsAllowed |
| bool | DisableTesla |

**Events.Player.UnlockingGenerator**

**Events.Player.OpeningGenerator**

**Events.Player.ClosingGenerator**

**Events.Player.ActivatingGenerator**

**Events.Player.StoppingGenerator**

**Events.Player.ReceivingEffect**

**Events.Player.IssuingMute**

**Events.Player.RevokingMute**

**Events.Player.UsingRadioBattery**

**Events.Player.ChangingRadioPreset**

**Events.Player.UsingMicroHIDEnergy**

**Events.Player.DroppingAmmo**

**Events.Player.DroppedAmmo**

**Events.Player.InteractingShootingTarget**

**Events.Player.DamagingShootingTarget**

**Events.Player.FlippingCoin**

**Events.Player.TogglingFlashlight**

**Events.Player.UnloadingWeapon**

**Events.Player.AimingDownSight**

**Events.Player.TogglingWeaponFlashlight**

**Events.Player.DryfiringWeapon**

**Events.Player.VoiceChatting**

**Events.Player.MakingNoise**

**Events.Player.Jumping**

**Events.Player.Landing**

**Events.Player.Transmitting**

**Events.Player.ChangingMoveState**

**Events.Player.ChangingSpectatedPlayer**

**Events.Player.TogglingNoClip**

**Events.Player.TogglingOverwatch**

**Events.Player.TogglingRadio**

**Events.Player.SearchingPickup**

**Events.Player.SendingAdminChatMessage**

**Events.Player.PlayerDamageWindow**

**Events.Player.DamagingDoor**

**Events.Player.ItemAdded**

**Events.Player.ItemRemoved**

**Events.Player.KillingPlayer**

**Events.Player.EnteringEnvironmentalHazard**

**Events.Player.StayingOnEnvironmentalHazard**

**Events.Player.ExitingEnvironmentalHazard**

**Events.Player.ChangingNickname**

---

## Scp049 (Doctor)

**Events.Scp049.FinishingRecall**

**Events.Scp049.StartingRecall**

**Events.Scp049.ActivatingSense**

**Events.Scp049.SendingCall**

**Events.Scp049.Attacking**

---

## Scp0492 (Zombie)

**Events.Scp0492.TriggeringBloodlust**

**Events.Scp0492.ConsumedCorpse**

**Events.Scp0492.ConsumingCorpse**

---

## Scp079 (Computer/Camera)

**Events.Scp079.ChangingCamera**

**Events.Scp079.GainingExperience**

**Events.Scp079.GainingLevel**

**Events.Scp079.InteractingTesla**

**Events.Scp079.TriggeringDoor**

**Events.Scp079.ElevatorTeleporting**

**Events.Scp079.LockingDown**

**Events.Scp079.ChangingSpeakerStatus**

**Events.Scp079.Recontained**

**Events.Scp079.Pinging**

**Events.Scp079.RoomBlackout**

**Events.Scp079.ZoneBlackout**

---

## Scp096 (ShyGuy)

**Events.Scp096.Enraging**

**Events.Scp096.CalmingDown**

**Events.Scp096.AddingTarget**

**Events.Scp096.StartPryingGate**

**Events.Scp096.Charging**

**Events.Scp096.TryingNotToCry**

---

## Scp106 (Larry/OldMan)

**Attacking**

**Teleporting**

**Stalking**

**ExitStalking**

---

## Scp173 (Peanut)

**Blinking**

**BlinkingRequest**

**PlacingTantrum**

**UsingBreakneckSpeeds**

---
## Scp244 (Vase)

**UsingScp244**

| Type | Member   |
| --- |----------|
| Player | Player |
| bool | IsAllowed |
| Scp244 | Scp244 |

**DamagingScp244**

| Type | Member   |
| --- |----------|
| Scp244Pickup | Pickup |
| DamageHandler | Handler |
| bool | IsAllowed |

**OpeningScp244**

| Type | Member   |
| --- |----------|
| Scp244Pickup | Pickup |
| bool | IsAllowed |

---

## Scp330 (Candy)

**InteractingScp330**

| Type | Member   |
| --- |----------|
| Player | Player |
| CandyKindID | Candy |
| bool | IsAllowed |
| int | UsageCount |
| bool | ShouldSever |

**DroppingScp330**

| Type | Member   |
| --- |----------|
| Player | Player |
| CandyKindID | Candy |
| bool | IsAllowed |
| Scp330 | Scp330 |

**EatingScp330**

| Type | Member   |
| --- |----------|
| Player | Player |
| ICandy | Candy |
| bool | IsAllowed |

**EatenScp330**

| Type | Member   |
| --- |----------|
| Player | Player |
| ICandy | Candy |

---

## Scp914

**UpgradingPickup**

**UpgradingInventoryItem**

**UpgradingPlayer**

**Activating**

---

## Scp939 (Dog)

**ChangingFocus**

**Lunging**

**PlacingAmnesticCloud**

**PlayingVoice**

**SavingVoice**

**PlayingSound**

**Clawed**

**ValidatingVisibility**

---

## Scp3114 (Skeleton)

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

# Coroutines
Coroutines are a way to run code in parallel with the main thread. 
This can be useful for:
- running code that takes a long time to complete
- running code that needs to wait for a certain condition to be met
- running code that needs to be executed at regular intervals

The Timing global variable provides functions for creating and managing coroutines.

### Timing:CallDelayed
Call a function after a delay.

```lua
Timing:CallDelayed(5, function()
    print('This will be printed after 5 seconds')
end)
```

### Timing:CallCoroutine

Call a coroutine.

Note that Lua Coroutine logic partially applies here. You can use `coroutine.yield(x)` to pause the coroutine for x seconds, 0.1 = 10 loops/second.

```lua
Timing:CallCoroutine(function()
    print('This will be printed immediately')
    while true do
        print('This will be printed every second')
        coroutine.yield(1)
    end
end)
```

### Arguments
These functions can also be called with arguments. The arguments need to be passed along in an object[].

```lua
Timing:CallDelayed(5, function(word1, word2)
    print('This will be printed after 5 seconds')
    print('The arguments are: ' .. word1 .. ' ' .. word2)
end, {'Hello',' World!'})
```




