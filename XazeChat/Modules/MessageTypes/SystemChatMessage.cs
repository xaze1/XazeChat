// // Copyright (c) 2025 xaze_
// //
// // This source code is licensed under the MIT license found in the
// // LICENSE file in the root directory of this source tree.
// //
// // I <3 🦈s :3c

using System.Drawing;
using LabApi.Features.Wrappers;
using NorthwoodLib.Pools;
using RueI.Extensions.HintBuilding;
using XazeAPI.API.Helpers;

namespace XazeChat.Modules.MessageTypes;

public class SystemChatMessage(string message) : GlobalChatMessage(Player.Host, message)
{
    public override bool IsVisible(Player Viewer)
    {
        return true;
    }
    
    public override string DisplayMessage(Player Viewer)
    {
        var sb = StringBuilderPool.Shared.Rent();
        sb.SetColor(Color.Yellow)
            .Append(Message);

        return StringBuilderPool.Shared.ToStringReturn(sb);
    }
}