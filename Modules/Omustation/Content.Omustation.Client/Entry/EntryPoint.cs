using Content.Omustation.Client.IoC;
using Robust.Shared.ContentPack;
using Robust.Shared.Timing;

namespace Content.Omustation.Client.Entry;

public sealed class EntryPoint : GameClient
{

    public override void Init()
    {
        IoCManager.BuildGraph();
        IoCManager.InjectDependencies(this);
    }

    public override void PostInit()
    {
        base.PostInit();
    }

    public override void Update(ModUpdateLevel level, FrameEventArgs frameEventArgs)
    {
        base.Update(level, frameEventArgs);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
    }
}
