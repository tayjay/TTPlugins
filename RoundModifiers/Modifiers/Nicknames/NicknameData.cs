using System.Collections.Generic;
using System.IO;
using PlayerRoles;
using Utf8Json;

namespace RoundModifiers.Modifiers.Nicknames;

public class NicknameData
{
    public string[] Nicknames { get; set; }
    
    public NicknameData()
    {
        Nicknames = new[]
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
    }

    /*public string GetRolePrefix(RoleTypeId role)
    {
        if (RolePrefixes.ContainsKey(role.ToString()))
        {
            return RolePrefixes[role.ToString()];
        }

        return "";
    }
    
    public string GetRandomNickname()
    {
        if (Nicknames.Length == 0)
        {
            return "";
        }
        return Nicknames[UnityEngine.Random.Range(0, Nicknames.Length)];
    }*/
    
    
    
    
    public static NicknameData GetSaved()
    {
        if (!Directory.Exists("TTCore"))
        {
            Directory.CreateDirectory("TTCore");
        }
        if (!Directory.Exists("TTCore/RoundModifiers"))
        {
            Directory.CreateDirectory("TTCore/RoundModifiers");
        }
        
        
        
        if(!File.Exists("TTCore/RoundModifiers/Nicknames.json"))
        {
            NicknameData nicknameData = new NicknameData();
            string data = JsonSerializer.ToJsonString<NicknameData>(nicknameData);
            File.WriteAllText("TTCore/RoundModifiers/Nicknames.json", JsonSerializer.PrettyPrint(data));
            return nicknameData;
        }
        else
        {
            string data = File.ReadAllText("TTCore/RoundModifiers/Nicknames.json");
            return JsonSerializer.Deserialize<NicknameData>(data);
        }
    }
}