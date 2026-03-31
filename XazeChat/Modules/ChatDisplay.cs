// // Copyright (c) 2025 xaze_
// //
// // This source code is licensed under the MIT license found in the
// // LICENSE file in the root directory of this source tree.
// //
// // I <3 🦈s :3c

using System.Drawing;
using System.Linq;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using LabApi.Features.Wrappers;
using NorthwoodLib.Pools;
using PlayerRoles;
using RueI.Displays;
using RueI.Elements;
using RueI.Extensions.HintBuilding;
using RueI.Parsing.Enums;

namespace XazeChat.Modules;

public static class ChatDisplay
{
    public static DynamicElement ChatElement;
    private static readonly IElemReference<DynamicElement> _ref = DisplayCore.GetReference<DynamicElement>();

    public static void Enable()
    {
        ChatElement ??= new(FormatChatMessages, 350);
        PlayerEvents.Joined += OnJoined;
        foreach (var plr in Player.ReadyList)
        {
            var core = DisplayCore.Get(plr.ReferenceHub);
            core.AddAsReference(_ref, ChatElement);
            core.Update();
        }
    }

    public static void Disable()
    {
        PlayerEvents.Joined -= OnJoined;
        foreach (var plr in Player.ReadyList)
        {
            var core = DisplayCore.Get(plr.ReferenceHub);
            core.RemoveReference(_ref);
            core.Update();
        }
    }

    private static void OnJoined(PlayerJoinedEventArgs args)
    {
        if (!args.Player.IsPlayer)
            return;

        var core = DisplayCore.Get(args.Player.ReferenceHub);
        core.AddAsReference(_ref, ChatElement);
        core.Update();
    }

    public static string FormatChatMessages(DisplayCore core)
    {
        var user = Player.Get(core.Hub);
        if (user == null || user.Role == RoleTypeId.None)
        {
            return "";
        }
        
        var sb = StringBuilderPool.Shared.Rent()
            .SetAlignment(HintBuilding.AlignStyle.Left)
            .SetLineHeight(-25)
            .SetSize(50, MeasurementUnit.Percentage);

        foreach (var msg in ChatManager.LastMessages.Where(m => m.IsVisible(user)).OrderByDescending(msg => msg.Timestamp))
        {
            sb.SetColor(Color.DimGray)
                .Append("[" + msg.Timestamp.ToString("HH:mm:ss") + "] ")
                .CloseColor()
                .AppendLine(msg.DisplayMessage(user));
        }
        
        return StringBuilderPool.Shared.ToStringReturn(sb.CloseLineHeight());
    }
}