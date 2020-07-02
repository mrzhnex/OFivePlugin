using Smod2.Attributes;
using Smod2;
using Smod2.Config;

namespace OFivePlugin
{
    [PluginDetails(
        author = "Innocence",
        description = "description",
        id = "o.five.plugin",
        name = "OFivePlugin",
        configPrefix = "ofp",
        SmodMajor = 3,
        SmodMinor = 0,
        SmodRevision = 0,
        version = Global.Version
    )]

    public class MainSettings : Plugin
    {
        public override void Register()
        {
            AddEventHandlers(new SetEvents(this));
            AddConfig(new ConfigSetting("IsHost", "false", true, "This is a description"));
        }

        public override void OnEnable()
        {
            Info(Details.name + " on");
        }

        public override void OnDisable()
        {
            Info(Details.name + " off");
        }

    }
}
