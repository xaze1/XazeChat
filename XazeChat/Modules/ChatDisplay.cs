// // Copyright (c) 2025 xaze_
// //
// // This source code is licensed under the MIT license found in the
// // LICENSE file in the root directory of this source tree.
// //
// // I <3 🦈s :3c

using System.Drawing;
using System.Linq;
using System.Text;
using LabApi.Features.Wrappers;
using PlayerRoles;
using RueI.Displays;
using RueI.Elements;
using RueI.Extensions.HintBuilding;
using RueI.Parsing.Enums;
using XazeAPI.API.Extensions;
using XazeAPI.API.Helpers;
using XazeChat.Modules.MessageTypes;

namespace XazeChat.Modules;

public static class ChatDisplay
{
    public static DynamicElement ChatElement;
    public static AutoElement Auto;

    public static void Enable()
    {
        ChatElement ??= new(FormatChatMessages, 350);
        Auto = new(Roles.Dead | Roles.Scps, ChatElement);
    }

    public static void Disable()
    {
        Auto?.Disable();
    }

    public static string FormatChatMessages(DisplayCore core)
    {
        var user = Player.Get(core.Hub);
        if (user == null || user.Role == RoleTypeId.None)
        {
            return "";
        }
        
        var sb = new StringBuilder()
            .SetAlignment(HintBuilding.AlignStyle.Left) 
            .SetLineHeight(-25)
            .SetSize(50, MeasurementUnit.Percentage);

        foreach (var msg in ChatManager.LastMessages.Where(m => m.IsVisible(user)).OrderByDescending(msg => msg.Timestamp))
        {
            sb.SetColor(Color.DimGray)
                .Append("[" + msg.Timestamp.ToString("HH:mm:ss") + "] ")
                .CloseColor()
                .SetColor(MainHelper.ColorFromRGB(msg.Role.RoleColor))
                .Append(msg.Username)
                .CloseColor()
                .SetColor(Color.DarkGray)
                .AppendLine(": " + msg.Message);
        }
        
        return sb.ToString();
    }
}