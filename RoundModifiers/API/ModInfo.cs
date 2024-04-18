namespace RoundModifiers.API
{
    public struct ModInfo
    {
        public ModInfo()
        {
            Name = null;
            FormattedName = null;
            Description = null;
            Aliases = new string[] { };
            Impact = ImpactLevel.None;
            MustPreload = false;
            Hidden = false;
        }

        /**
         * The name of the modifier. This is the first value checked when attempting to select a modifier.
         */
        public string Name { get; set;  }
        
        /**
         * The formatted name of the modifier. This is the name that will be displayed in the broadcast.
         */
        public string FormattedName { get; set;  }
        /**
         * The description of the modifier. This is the description that will be displayed to the user.
         */
        public string Description { get;set; }
        /**
         * The alternative names to be searched when looking for this modifier.
         */
        public string[] Aliases { get; set;  }
        /**
         * How impactful this modifier is to the game.
         */
        public ImpactLevel Impact { get; set;  }
        
        public bool Equals(ModInfo other)
        {
            return Name == other.Name;
        }

        /**
         * Does this modifier need to be started before the lobby is loaded? If false then an admin can add it mid-game or in the lobby.
         */
        public bool MustPreload { get; set; }

        /**
         * Should this modifier be hidden from the list of modifiers? If this is true then players cannot vote for it, but admins can still add it.
         * It will not be displayed in the broadcast of current modifiers.
         */
        public bool Hidden { get; set; }
    }
}