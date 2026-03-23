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

namespace XazeChat.Modules.MessageTypes
{
    public interface IMessage
    {
        PlayerRoleBase Role { get; }
        uint Owner { get; }
        string Username { get; }
        string Message { get; }
        StopTimer Timer { get; }
        DateTimeOffset Timestamp { get; }
        void Remove();
        bool IsVisible(Player Viewer);
    }
}