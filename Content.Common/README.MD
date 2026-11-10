# Dependency info

The `Content.Common` module is used to comunicate between `Core` and `Modules` by being a "Common" package that both depend on.
Since it is independent of each, we can reach it from any system.

# Typical module loading example:
```
                         Content.Common
                               ┃
                               ┃
                               ┃
                         Content.Shared ━━━━━> (Content.Client, Content.Server, etc)
                               ┃
                               ┃
                               ┃
                     Content.Module.Shared
```

## Communicating through commmon

However, there are tradeoffs for having a module that `Content.Shared` depends on.
`Common` cannot use anything defined in `Content.Shared`.

It is best to leave any added content to custom modules, but occasionally there is an unavoidable need to reach module code from `core`.

[](todo rewrite I don't know how to phrase this other than calling it boilerplate)
Thus, typically it is best to write merely basic boilerplate communicator code in the `Common` module and leave the hard work for inheriting systems.

For example, we can define an abstract entity system in `Content.Common`

```
namespace Content.Common.Example;

public abstract class CommonExampleEntitySystem : EntitySystem
{
    public abstract bool TryDoThing(EntityUid uid);
}
```

Abstract systems require an implementation, thus we implement this system in our desired module:
```
using Content.Common.Example;

namespace Content.Module.Shared.ExampleSystem;

public sealed partial class ExampleEntitySystem : CommonExampleEntitySystem
{
```
Since `ExampleEntitySystem` is inheriting from `CommonExampleEntitySystem` which is an `EntitySystem`, it will be an `EntitySystem`.

we can then implement the abstract methods that we defined earlier as an `override`:
```
using Content.Common.Example;

namespace Content.Module.Shared.ExampleSystem;

public sealed partial class ExampleEntitySystem : CommonExampleEntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
    }

    // This will run when Core calls the TryDoThing method we defined earlier in Content.Common
    public override bool TryDoThing(EntityUid uid)
    {
        var someBool = true;

        // do stuff

        return someBool
    }

}
```

Now, we can import `CommonExampleEntitySystem` and call it from `Content.Shared`

For example, if we want to run a check before someone slips

(Note, this is pseudocode for easier readabiliy)
```
// upstream usings
using Robust.Shared.Physics.Systems;
using Robust.Shared.Physics.Events;
// (...)

// add our usings
using Content.Common.Example;

namespace Content.Shared.Slippery;

[UsedImplicitly]
public sealed partial class SlipperySystem : EntitySystem
{
    // Upstream dependencies
    [Dependency] private SharedPhysicsSystem _physics = default!;
    (...)
    [Dependency] private SharedTransformSystem _xform = default!;

    // Upstream code should be modified as litle as possible, and should be made explicitly clear what was modified
    // Here we can just add a start and end, or make an inline comment
    //
    // Forkname Start
    [Dependency] private CommonExampleEntitySystem _exampleEntSys = default!;
    // Forkname End
    (...)

    public void TrySlip(EntityUid uid, SlipperyComponent component, EntityUid other, bool requiresContact = true)
    {
        var knockedDown = _knockedDownQuery.HasComp(other);
        if (knockedDown && !component.SlipData.SuperSlippery)
            return;

        // Fork start, run code before raising this event
        if (!_exampleEntSys.TryDoThing(uid))
            return;
        // Fork end
        // Upstream code (...)

        var attemptEv = new SlipAttemptEvent(uid);
        RaiseLocalEvent(other, attemptEv);
        if (attemptEv.SlowOverSlippery)
            _speedModifier.AddModifiedEntity(other);

        if (attemptEv.NoSlip)
            return;

        // (etc...)
    }

```

Likewise, if we have components that `Core` systems need to be aware of, it is best to define them in `Content.Common`.
Given `Common` components may be limited in datatypes, we can simply use them as marker components.

We can also use events that we can raise or listen to on either side.

```
namespace Content.Common.Example;

// Check if we can do a flip instead of slipping, cancel to flip.
[ByRefEvent]
public record struct CheckDoFlipBeforeSlipEvent(EntityUid ent, bool Cancelled = false);
```

Listen to it on a system

```
using Content.Common.Example

namespace Content.Module.Shared

public sealed partial class SomeModuleEntitySys : EntitySystem
{

    // pretend we have our dependencies and whatnot here
    // (...)

    [SubscribeLocalEvent]
    private void CanThisGuyFlipNotSlip(Entity<BackFlipperComponent> uid, ref CheckDoFlipBeforeSlipEvent args)
    {
        if (uid.Comp.CoolEnoughToFlip)
        {
            args.Cancelled = true;
            return;
        }
    }

}
```

and in shared...


```
(...)
// add our usings
using Content.Common.Example;

namespace Content.Shared.Slippery;

[UsedImplicitly]
public sealed partial class SlipperySystem : EntitySystem
{
    (dependencies and etc...)


    public void TrySlip(EntityUid uid, SlipperyComponent component, EntityUid other, bool requiresContact = true)
    {
        var knockedDown = _knockedDownQuery.HasComp(other);
        if (knockedDown && !component.SlipData.SuperSlippery)
            return;

        // Fork start, our own event here
        var flipAttemptEv = new CheckDoFlipBeforeSlipEvent(uid)
        RaiseLocalEvent(uid, flipAttemptEv)
        if (flipAttemptEv.Cancelled)
            return;
        // Fork end
        // Upstream code (...)

        var attemptEv = new SlipAttemptEvent(uid);
        RaiseLocalEvent(other, attemptEv);
        if (attemptEv.SlowOverSlippery)
            _speedModifier.AddModifiedEntity(other);

        if (attemptEv.NoSlip)
            return;

        // (etc...)
    }
}
```

Notice that here `Core` is entirely unaware of what systems or components we may have defined in our modules, which allows us to keep our assemblies separate.

Note, you may find using `Common` obtuse when you want to pass some more complex or really specific datatypes.
In these scenarios, currently there are no good ways around it, but it is recommended to create a simple abstract system in `Core` that we can override in our module

[](todo this isn't great and at this point you could just run the actual system in shared but i think keeping most code in the modules is better separation)
```
namespace Content.Shared.Specific

public sealed abstract class CommonSpecificSystem : EntitySystem
{
    public abstract void RunModuleCode(EntityUid uid);
}
``


