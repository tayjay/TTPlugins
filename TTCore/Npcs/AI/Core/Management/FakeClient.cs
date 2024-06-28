using System;
using Mirror;

namespace TTCore.Npcs.AI.Core.Management;

public class FakeClient(int id) : NetworkConnectionToClient(id)
{
    public override string address => "127.0.0.1";

    public override void Send(ArraySegment<byte> segment, int channelId = 0) { }

    public override void Disconnect() { }
}