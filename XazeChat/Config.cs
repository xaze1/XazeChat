// // Copyright (c) 2025 xaze_
// //
// // This source code is licensed under the MIT license found in the
// // LICENSE file in the root directory of this source tree.
// //
// // I <3 🦈s :3c

using System.Collections.Generic;
using System.ComponentModel;

namespace XazeChat;

public class Config
{
    [Description("Words to be censored, when said")]
    public List<string> CensoredWords { get; set; } = ["Shit", "Fuck"];
    
    [Description("Words to be blocked, when said")]
    public List<string> BlockedWords { get; set; }  = ["Nigger"];
}