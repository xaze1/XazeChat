// // Copyright (c) 2025 xaze_
// //
// // This source code is licensed under the MIT license found in the
// // LICENSE file in the root directory of this source tree.
// //
// // I <3 🦈s :3c

using System;
using LabApi.Features.Wrappers;
using PlayerRoles;
using XazeAPI.API;

namespace XazeChat.Modules.MessageTypes;

public class GlobalChatMessage : IMessage
{
    public PlayerRoleBase Role { get; }
    public uint Owner { get; }
    public string Username { get; }
    public string Message { get; }
    public StopTimer Timer { get; }
    public DateTimeOffset Timestamp { get; }
    
    public void Remove()
    {
    }

    public virtual bool IsVisible(Player Viewer)
    {
        return !Viewer.IsAlive;
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
}