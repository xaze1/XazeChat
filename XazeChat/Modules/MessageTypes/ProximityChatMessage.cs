// // Copyright (c) 2025 xaze_
// //
// // This source code is licensed under the MIT license found in the
// // LICENSE file in the root directory of this source tree.
// //
// // I <3 🦈s :3c

using System;
using System.Collections.Generic;
using System.Linq;
using CustomPlayerEffects;
using Discord;
using InventorySystem.Items.Firearms.Modules;
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
    public const float ProximityRange = 15;
    public PlayerRoleBase Role { get; }
    public uint Owner { get; }
    public string Username { get; }
    public string Message { get; }
    public StopTimer Timer { get; }
    public DateTimeOffset Timestamp { get; }
    private readonly List<Player> VisibleTo = new();
    
    public bool IsVisible(Player Viewer)
    {
        // Any spectator spectating the player sees the message and anyone that was nearby at the time of sending the message
        if (VisibleTo.Contains(Viewer))
            return true;
        
        var plr = Player.Get(Owner);
        if (plr != null && (plr.GetEffect<Scp1576>()?.IsEnabled ?? false))
            return true;
        
        return Viewer.CurrentlySpectating != null && Viewer.CurrentlySpectating.NetworkId == Owner;
    }
    
    public void Remove()
    {
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
        Timestamp = DateTimeOffset.Now;
        VisibleTo.Add(user);

        foreach (var col in Physics.OverlapSphere(user.Position, ProximityRange, HitscanHitregModuleBase.HitregMask))
        {
            if (!col.TryGetComponent(out HitboxIdentity hitbox) || VisibleTo.Any(plr => plr.ReferenceHub == hitbox.TargetHub))
            {
                continue;
            }
            
            VisibleTo.Add(Player.Get(hitbox.TargetHub));
        }
    }
}