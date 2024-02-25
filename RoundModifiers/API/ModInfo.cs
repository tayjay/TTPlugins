namespace RoundModifiers.API
{
    public struct ModInfo
    {
        public string Name { get; set;  }
        
        public string FormattedName { get; set;  }
        public string Description { get;set; }
        public string[] Aliases { get; set;  }
        public ImpactLevel Impact { get; set;  }
        
        public bool Equals(ModInfo other)
        {
            return Name == other.Name;
        }

        public bool MustPreload { get; set; }
    }
}