using System;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;
using XazeAPI.API;
using XazeChat.Modules;

namespace XazeChat
{
    public class Plugin : Plugin<Config>
    {
        public override string Name => "Xaze Chat";
        public override string Description => "Adds a chat to SL";
        public override string Author => "xaze_";
        public override Version Version => new(1, 0, 0);
        public override Version RequiredApiVersion => LabApiProperties.CurrentVersion;
        
        public static Plugin Singleton { get; private set; }

        public static Config PluginConfig
        {
            get
            {
                if (Singleton == null)
                {
                    return new();
                }
                
                return Singleton.Config;
            }
        }
        
        public override void Enable()
        {
            Singleton = this;
            ChatDisplay.Enable();
            Logging.Info("[XazeChat] Enabled");
        }

        public override void Disable()
        {
            Singleton = null;
            ChatDisplay.Disable();
            Logging.Info("[XazeChat] Disabled");
        }
    }
}