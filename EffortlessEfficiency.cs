using HarmonyLib;
using ResoniteModLoader;

namespace EffortlessEfficiency;

public partial class EffortlessEfficiency : ResoniteMod
{
    public override string Name => "Effortless Efficiency";
    public override string Author => "Cyro";
    public override string Version => typeof(EffortlessEfficiency).Assembly.GetName().Version.ToString();
    public override string Link => "https://github.com/BlueCyro/EffortlessEfficiency";
    public static ModConfiguration? Config;

    public override void OnEngineInit()
    {
        Harmony harmony = new("net.Cyro.EffortlessEfficiency");
        Config = GetConfiguration();
        Config?.Save(true);
        harmony.PatchAll();
    }
}
