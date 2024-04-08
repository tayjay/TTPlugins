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

* **RoundModifiers**
    * Introduces a variety of gameplay modifiers that can be applied during rounds.
        * Range from minor adjustments (lighting) to complete gameplay overhauls.
        * Players can vote on modifiers, or admins can directly assign them.
     
* **TTAdmin**
  * Server-side code for a web based SCP:SL server management system
       * Non-functional
         
* **SCriPt**
   * A Lua to EXILED scripting framework (MoonSharp)
        * Write Lua script in a similar structure to EXILED C# code
        * Started writing this as a way to automate server testing
        * New scripts can be added to the local Scirpts folder of the server install without recompiling any DLLs.
        * Not feature complete yet
      
