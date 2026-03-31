// // Copyright (c) 2025 xaze_
// //
// // This source code is licensed under the MIT license found in the
// // LICENSE file in the root directory of this source tree.
// //
// // I <3 🦈s :3c

using LabApi.Features.Wrappers;

namespace XazeChat.Modules.MessageTypes;

public class ScpChatMessage(Player user, string message) : GlobalChatMessage(user, message)
{
    public override bool IsVisible(Player Viewer)
    {
        // Player is a SCP AND the message was sent AFTER they became one
        return Viewer.IsSCP && Viewer.RoleBase.ActiveTime >= TimeSinceSent.TotalSeconds;
    }
}