// // Copyright (c) 2025 xaze_
// //
// // This source code is licensed under the MIT license found in the
// // LICENSE file in the root directory of this source tree.
// //
// // I <3 🦈s :3c

using System;
using LabApi.Features.Wrappers;
using NorthwoodLib.Pools;
using PlayerRoles;
using RueI.Extensions.HintBuilding;
using UnityEngine;
using XazeAPI.API;
using XazeAPI.API.Helpers;

namespace XazeChat.Modules.MessageTypes;

public class ProximityChatMessage : IMessage
{
    public PlayerRoleBase Role { get; }
    public uint Owner { get; }
    public string Username { get; }
    public string Message { get; }
    public StopTimer Timer { get; }
    public TextToy TextObject { get; }
    public DateTimeOffset Timestamp { get; }

    public void Remove()
    {
        TextObject.Destroy();
    }
    
    public bool IsVisible(Player Viewer)
    {
        return Viewer.CurrentlySpectating != null && Viewer.CurrentlySpectating.PlayerId == Owner;
    }
    
    public string DisplayMessage(Player Viewer)
    {
        var sb = StringBuilderPool.Shared.Rent();
        sb.SetColor(MainHelper.ColorFromRGB(Role.RoleColor))
            .Append(Username)
            .CloseColor()
            .SetColor(System.Drawing.Color.DarkGray)
            .Append(": " + Message);

        return StringBuilderPool.Shared.ToStringReturn(sb);
    }

    public ProximityChatMessage(Player user, string message)
    {
        Role = user.RoleBase;
        Owner = user.NetworkId;
        Username = user.DisplayName;
        Message = message;
        Timer = new(TimeSpan.FromSeconds(30), () => ChatManager.RemoveMessage(this));
        
        TextObject = TextToy.Create(new Vector3(0, 1, 0), user.Camera);
        TextObject.TextFormat = message;
        TextObject.Spawn();
        Timestamp = DateTimeOffset.Now;
    }
}