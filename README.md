[![GitHub Release](https://img.shields.io/github/v/release/tayjay/TTPlugins?include_prereleases)](https://github.com/tayjay/TTPlugins/releases/latest)
[![GitHub Downloads (all assets, all releases)](https://img.shields.io/github/downloads/tayjay/TTPlugins/total)](https://github.com/tayjay/TTPlugins/releases/latest)



# SCP:SL EXILED Plugin Collection

**Introduction**
This repository contains a collection of custom plugins I've developed for SCP:SL EXILED game servers.

Currently written for latest EXILED 8.X.X

## Plugins

* **TTCore**
    * Serves as the foundation for my other plugins, providing centralized code, HUD elements, player resizing tools, NPC management, class extensions, and additional Harmony patches.

* **TTAddons**
    * Offers a set of specific modifications tailored for my personal server. Includes:
        * An 'unstuck' command to resolve SCP spawn location issues.
        * The ability to adjust SCP-3114 spawn probability.
        * Adds configs to change SCP-049 respawning logic for old respawn limit, and changing ragdoll viability time.

* **RoundModifiers**
    * Introduces a variety of gameplay modifiers that can be applied during rounds.
        * Range from minor adjustments (lighting) to complete gameplay overhauls.
        * Players can vote on modifiers, or admins can directly assign them.
     
* **TTAdmin**
  * A web based SCP:SL server management framework
  * Adds a REST API for requests and Websocket for listening to EXILED events in realtime
         
* **SCriPt**
   * A Lua to EXILED scripting framework (MoonSharp)
   * Moved to it's own [Repo](https://github.com/tayjay/SCriPt)
   * Does not require TTCore to function
   * Has an official [Pre-Release](https://github.com/tayjay/SCriPt/releases)
      
