using System;
using System.Net;
using Exiled.API.Features;
using Respawning;
using TTAdmin.Data;
using TTAdmin.WebNew.DataTypes;

namespace TTAdmin.WebNew.Handlers;

/*public class RespawnRequestHandler : AllRequestHandler
{
    public override string Path { get; } = "/server/respawn";
    public override void ProcessGetRequest(HttpListenerContext context)
    {
        JsonResponse<RespawnData>.Send(context.Response, new RespawnData());
    }

    public override void ProcessPostRequest(HttpListenerContext context)
    {
        using var reader = new System.IO.StreamReader(context.Request.InputStream);
        string body = reader.ReadToEnd();
        PostRespawnRequest postRespawnRequest = Utf8Json.JsonSerializer.Deserialize<PostRespawnRequest>(body);

        if (postRespawnRequest.Action == "force")
        {
            SpawnableTeamType team = Respawn.NextKnownTeam;
            if (postRespawnRequest.Team != "")
            {
                if(Enum.TryParse(postRespawnRequest.Team, out SpawnableTeamType team2))
                {
                    team = team2;
                }
                else
                {
                    ErrorResponse.SendError(context.Response, HttpStatusCode.BadRequest, "Invalid team");
                    return;
                }
            }
            Respawn.ForceWave(team);
        }
        else if (postRespawnRequest.Action == "grant")
        {
            if(Enum.TryParse(postRespawnRequest.Team, out SpawnableTeamType team))
            {
                Respawn.GrantTickets(team, postRespawnRequest.Tickets);
            }
            else
            {
                ErrorResponse.SendError(context.Response, HttpStatusCode.BadRequest, "Invalid team");
                return;
            }
        }
        else if (postRespawnRequest.Action == "protection")
        {
            Respawn.ProtectionEnabled = postRespawnRequest.Enabled;
        }
        else if (postRespawnRequest.Action == "protectedCanShoot")
        {
            Respawn.ProtectedCanShoot = !Respawn.ProtectedCanShoot;
        }
        else
        {
            ErrorResponse.SendError(context.Response, HttpStatusCode.BadRequest, "Invalid action");
            return;
        }

        JsonResponse<RespawnData>.Send(context.Response, new RespawnData());
    }
    
    public class PostRespawnRequest
    {
        public string Action { get; set; } = "";
        public float Tickets { get; set; } = 0;
        public bool Enabled { get; set; } = false;
        public string Team { get; set; } = "";
    }
}*/