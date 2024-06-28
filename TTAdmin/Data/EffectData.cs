using System.Collections.Generic;
using CustomPlayerEffects;

namespace TTAdmin.Data;

public class EffectData
{
    public string EffectName { get; set; }
    public float Duration { get; set; }
    public int Intensity { get; set; }
    
    public EffectData(StatusEffectBase effect)
    {
        EffectName = effect.name;
        Duration = effect.Duration;
        Intensity = effect.Intensity;
    }
    
    public static List<EffectData> FromEffects(StatusEffectBase[] effects)
    {
        List<EffectData> data = new List<EffectData>();
        for (int i = 0; i < effects.Length; i++)
        {
            if(effects[i].Intensity == 0) continue;
            data.Add(new EffectData(effects[i]));
        }
        return data;
    }
}