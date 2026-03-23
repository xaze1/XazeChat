// // Copyright (c) 2025 xaze_
// //
// // This source code is licensed under the MIT license found in the
// // LICENSE file in the root directory of this source tree.
// //
// // I <3 🦈s :3c

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CommandSystem;
using LabApi.Features.Wrappers;
using RemoteAdmin;
using XazeAPI.API.Helpers;
using XazeChat.Modules;

namespace XazeChat;

[CommandHandler(typeof(ClientCommandHandler))]
[CommandHandler(typeof(GameConsoleCommandHandler))]
public class ChatCommand : ICommand
{
    public string Command => "chat";
    public string[] Aliases => ["message", "msg"];
    public string Description => "Send a Chat Message";
    
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, [UnscopedRef] out string response)
    {
        if (Plugin.Singleton == null)
        {
            response = "Plugin not enabled";
            return false;
        }
        
        if (sender is not PlayerCommandSender cmdSender)
        {
            if (!MainHelper.getArrayText(arguments, 0, out string systemMsg))
            {
                response = "Invalid chat message";
                return false;
            }
            
            response = "Send System message!";
            ChatManager.SendSystemMessage(systemMsg);
            return true;
        }

        var user = Player.Get(cmdSender.ReferenceHub);
        if (arguments.Count < 1)
        {
            response = "Invalid chat message";
            return false;
        }
        
        if (arguments.At(0) == "delete" || arguments.At(0) == "remove")
        {
            if (!cmdSender.CheckPermission(PlayerPermissions.KickingAndShortTermBanning))
            {
                response = "You do not have permission to delete messages!";
                return false;
            }
            
            if (!Player.TryGet(int.Parse(arguments.At(1)), out var target))
            {
                response = "Player not found";
                return false;
            }

            ChatManager.RemoveMessages(target);
            response = "Removed Messages from " + target.Nickname;
            return true;
        }

        if (user?.IsMuted?? false)
        {
            response = "You are globally muted!";
            return false;
        }

        if (!MainHelper.getArrayText(arguments, 0, out string msg))
        {
            response = "Invalid chat message";
            return false;
        }

        foreach (var word in Plugin.PluginConfig.BlockedWords.Where(word => msg.ToLower().Contains(word.ToLower())))
        {
            response = "Your message contains a blocked word: " + word;
            ServerRolesHelper.SendAdminChatMessage("Name: " + cmdSender.Nickname + "\nSteamId: " + cmdSender.SenderId + "\nTried saying the following blocked word: " + word, "[XazeChat] Blocked word", Player.ReadyList);
            return false;
        }
        
        ChatManager.SendMessage(user, msg);
        response = "Send chat message!";
        return true;
    }
}