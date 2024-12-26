using System.Collections.Generic;
using System.IO;
using System.Linq;
using Exiled.API.Features;
using MapGeneration;
using TTCore.API;
using TTCore.Events.EventArgs;
using UnityEngine;

namespace TTAddons.Handlers
{
    public class MapChangeHandler :IRegistered
    {
        public void OnGenerated()
        {
            if(!Directory.Exists("TTCore"))
            {
                Directory.CreateDirectory("TTCore");
            }
            if(!Directory.Exists("TTCore/TTAddons"))
            {
                Directory.CreateDirectory("TTCore/TTAddons");
            }
            if(!Directory.Exists("TTCore/TTAddons/Maps"))
            {
                Directory.CreateDirectory("TTCore/TTAddons/Maps");
            }
            
            // Save all maps to disk
            /*foreach (ImageGenerator generator in ImageGenerator.ZoneGenerators)
            {
                int i = 0;
                foreach (Texture2D texture2D in generator.maps)
                {
                    if (texture2D == null) continue;
                    if (!texture2D.isReadable) continue;
                    //Log.Debug($"Loading image {generator.alias}");
                    byte[] data = texture2D.EncodeToPNG();
                    File.WriteAllBytes("TTCore/TTAddons/Maps/" + generator.alias + "-" + i++ + ".png", data);
                }
            }*/
            
            
            
        }
        
        public void OnRoundRestart()
        {
            try
            {
                Dictionary<string, List<Texture2D>> maps = new Dictionary<string, List<Texture2D>>();
                foreach(string file in Directory.GetFiles("TTCore/TTAddons/Maps", "*.png"))
                {
                    Log.Debug("Loading map from disk. File: " + file);
                    string formattedFile = file.Replace("\\", "/");
                    string[] split = formattedFile.Split('/');
                    string[] split2 = split[split.Length - 1].Split('-');
                    string alias = split2[0];
                    if(!maps.ContainsKey(alias)) maps.Add(alias, new List<Texture2D>());
                    byte[] data = File.ReadAllBytes(file);
                    Log.Debug("Read " + data.Length + " bytes from disk.");
                    Texture2D texture2D = new Texture2D(32, 32);
                    texture2D.LoadImage(data);
                    maps[alias].Add(texture2D);
                    Log.Debug("Loaded map from disk. Alias: " + alias);
                }
                /*foreach (ImageGenerator generator in ImageGenerator.ZoneGenerators)
                {
                    if(!maps.ContainsKey(generator.alias)) continue;
                    Log.Debug("Attempting to replace "+generator.maps.Length+" maps for "+generator.alias+" with maps from disk.");
                    generator.maps = new Texture2D[maps[generator.alias].Count];
                    for (int i = 0; i < maps[generator.alias].Count; i++)
                    {
                        Log.Debug("Loading map " + i + " for " + generator.alias + " from disk.");
                        generator.maps[i] = maps[generator.alias][i];
                    }
                } */
            } catch (System.Exception e)
            {
                Log.Error("Error loading maps from disk: " + e.Message);
            }
        }
        
        /*public void OnPreGenerateMap()
        {
            Log.Debug("MapChangeHandler.OnPreGenerateMap");
            try
            {
                Dictionary<string, List<Texture2D>> maps = new Dictionary<string, List<Texture2D>>();
                foreach(string file in Directory.GetFiles("TTCore/TTAddons/Maps", "*.png"))
                {
                    Log.Debug("Loading map from disk. File: " + file);
                    string formattedFile = file.Replace("\\", "/");
                    string[] split = formattedFile.Split('/');
                    string[] split2 = split[split.Length - 1].Split('-');
                    string alias = split2[0];
                    if(!maps.ContainsKey(alias)) maps.Add(alias, new List<Texture2D>());
                    byte[] data = File.ReadAllBytes(file);
                    Log.Debug("Read " + data.Length + " bytes from disk.");
                    Texture2D texture2D = new Texture2D(32, 32);
                    texture2D.LoadImage(data);
                    maps[alias].Add(texture2D);
                    Log.Debug("Loaded map from disk. Alias: " + alias);
                }

                if (ImageGenerator.ZoneGenerators == null)
                {
                    Log.Debug("ImageGenerator.ZoneGenerators is null");
                    return;
                }
                if(ImageGenerator.ZoneGenerators.Length == 0)
                {
                    Log.Debug("ImageGenerator.ZoneGenerators is empty");
                    return;
                }
                
                foreach (ImageGenerator generator in ImageGenerator.ZoneGenerators)
                {
                    Log.Debug("Checking generator "+generator.alias);
                    if(generator.alias == null) continue;
                    if(!maps.ContainsKey(generator.alias)) continue;
                    Log.Debug("Attempting to replace "+generator.maps.Length+" maps for "+generator.alias+" with maps from disk.");
                    generator.maps = new Texture2D[maps[generator.alias].Count];
                    for (int i = 0; i < maps[generator.alias].Count; i++)
                    {
                        Log.Debug("Loading map " + i + " for " + generator.alias + " from disk.");
                        generator.maps[i] = maps[generator.alias][i];
                    }
                } 
            } catch (System.Exception e)
            {
                Log.Error("Error loading maps from disk: " + e.Message);
            }
        }

        public void OnPreGenerateZone(PreGenerateZoneEventArgs ev)
        {
            Log.Debug("MapChangeHandler.OnPreGenerateZone "+ev.ZoneName);
            List<Texture2D> maps = new List<Texture2D>();
            try
            {
                foreach(string file in Directory.GetFiles("TTCore/TTAddons/Maps", "*.png"))
                {
                    string formattedFile = file.Replace("\\", "/");
                    string[] split = formattedFile.Split('/');
                    string[] split2 = split[split.Length - 1].Split('-');
                    string alias = split2[0];
                    if(alias != ev.ZoneName) continue;
                    byte[] data = File.ReadAllBytes(file);
                    Log.Debug("Read " + data.Length + " bytes from disk.");
                    Texture2D texture2D = new Texture2D(32, 32);
                    texture2D.LoadImage(data);
                    maps.Add(texture2D);
                    Log.Debug("Loaded map from disk. Alias: " + alias);
                }
                ev.ImageGenerator.maps = new Texture2D[maps.Count];
                for (int i = 0; i < maps.Count; i++)
                {
                    Log.Debug("Loading map " + i + " for " + ev.ZoneName + " from disk.");
                    ev.ImageGenerator.maps[i] = maps[i];
                }
            } catch (System.Exception e)
            {
                Log.Error("Error loading maps from disk: " + e.Message);
            }
           
        }*/

        public void Register()
        {
            Exiled.Events.Handlers.Map.Generated += OnGenerated;
            //Exiled.Events.Handlers.Server.RestartingRound += OnRoundRestart;
            //TTCore.Events.Handlers.Custom.PreGenerateMap += OnPreGenerateMap;
            //TTCore.Events.Handlers.Custom.PreGenerateZone += OnPreGenerateZone;
        }

        public void Unregister()
        {
            Exiled.Events.Handlers.Map.Generated -= OnGenerated;
            //Exiled.Events.Handlers.Server.RestartingRound -= OnRoundRestart;
            //TTCore.Events.Handlers.Custom.PreGenerateMap -= OnPreGenerateMap;
            //TTCore.Events.Handlers.Custom.PreGenerateZone -= OnPreGenerateZone;
        }
    }
}