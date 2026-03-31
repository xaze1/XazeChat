// // Copyright (c) 2025 xaze_
// //
// // This source code is licensed under the MIT license found in the
// // LICENSE file in the root directory of this source tree.
// //
// // I <3 🦈s :3c

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CustomPlayerEffects;
using LabApi.Features.Wrappers;
using NorthwoodLib.Pools;
using PlayerRoles;
using RueI.Extensions.HintBuilding;
using XazeAPI.API;
using XazeAPI.API.Helpers;

namespace XazeChat.Modules.MessageTypes;

public class GlobalChatMessage : IMessage
{
    public static readonly Dictionary<ReferenceHub, DateTimeOffset> LastScp1576Usage = new();
    public PlayerRoleBase Role { get; }
    public uint Owner { get; }
    public string Username { get; }
    public string Message { get; }
    public StopTimer Timer { get; }
    public DateTimeOffset Timestamp { get; }
    public TimeSpan TimeSinceSent => DateTimeOffset.Now - Timestamp;
    
    public void Remove()
    {
    }

    public virtual bool IsVisible(Player Viewer)
    {
        // Player sees the message if dead OR actively using SCP-1576 AND message was sent after usage started
        return !Viewer.IsAlive || (Viewer.GetEffect<Scp1576>()?.IsEnabled ?? false) && LastScp1576Usage.TryGetValue(Viewer.ReferenceHub, out var started) && started < Timestamp;
    }

    public virtual string DisplayMessage(Player Viewer)
    {
        var sb = StringBuilderPool.Shared.Rent();
        sb.SetColor(MainHelper.ColorFromRGB(Role.RoleColor))
            .Append(Username)
            .CloseColor()
            .SetColor(Color.DarkGray)
            .Append(": " + Message);

        return StringBuilderPool.Shared.ToStringReturn(sb);
    }

    public GlobalChatMessage(Player user, string message)
    {
        Role = user.RoleBase;
        Owner = user.NetworkId;
        Username = user.DisplayName;
        Message = message;
        Timer = new(TimeSpan.FromSeconds(30), () => ChatManager.RemoveMessage(this));
        Timestamp = DateTimeOffset.Now;
    }

    static GlobalChatMessage()
    {
        StatusEffectBase.OnEnabled += (effect) =>
        {
            if (effect is not Scp1576)
                return;
            
            LastScp1576Usage[effect.Hub] = DateTimeOffset.Now;
        };
    }
}