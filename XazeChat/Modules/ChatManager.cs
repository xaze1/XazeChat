// // Copyright (c) 2025 xaze_
// //
// // This source code is licensed under the MIT license found in the
// // LICENSE file in the root directory of this source tree.
// //
// // I <3 🦈s :3c

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using LabApi.Features.Wrappers;
using XazeAPI.API.Extensions;
using XazeChat.Modules.MessageTypes;

namespace XazeChat.Modules
{
    public static class ChatManager
    {
        public static readonly List<IMessage> LastMessages = new();
        
        public static void SendMessage(Player User, string message)
        {
            if (User.ReferenceHub == Player.Host?.ReferenceHub)
            {
                SendSystemMessage(message);
                return;
            }
            
            foreach (var word in Plugin.PluginConfig.CensoredWords)
            {
                var pattern = $@"\b{Regex.Escape(word)}\b";
                message = Regex.Replace(
                    message,
                    pattern,
                    new string('#', word.Length),
                    RegexOptions.IgnoreCase
                );
            }
            
            IMessage chatMessage;
            if (User.IsSCP)
            {
                chatMessage = new ScpChatMessage(User, message);
            }
            else if (User.IsAlive)
            {
                chatMessage = new ProximityChatMessage(User, message);
            }
            else
            {
                chatMessage = new GlobalChatMessage(User, message);
            }
            
            LastMessages.Add(chatMessage);
            chatMessage.Timer.Start();
        }
        
        public static void SendSystemMessage(string message)
        {
            foreach (var word in Plugin.PluginConfig.CensoredWords)
            {
                var pattern = $@"\b{Regex.Escape(word)}\b";
                message = Regex.Replace(
                    message,
                    pattern,
                    new string('#', word.Length),
                    RegexOptions.IgnoreCase
                );
            }

            var chatMessage = new SystemChatMessage(message);
            LastMessages.Add(chatMessage);
            chatMessage.Timer.Start();
        }
        
        public static void RemoveMessage(IMessage msg)
        {
            msg.Remove();
            LastMessages.Remove(msg);
        }
        
        public static void RemoveMessages(Player User)
        {
            LastMessages.ToList().Where(msg => msg.Owner == User.NetworkId).ForEach(RemoveMessage);
        }
        
        public static void RemoveMessages(uint networkId)
        {
            LastMessages.ToList().Where(msg => msg.Owner == networkId).ForEach(RemoveMessage);
        }
    }
}