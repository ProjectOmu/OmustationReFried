using Content.Shared.Chat.Prototypes;
using Content.Shared.Whitelist;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Chasm;

/// <summary>
/// Marks a component that will cause entities to fall into them on a step trigger activation
/// </summary>
[NetworkedComponent, RegisterComponent, Access(typeof(ChasmSystem))]
public sealed partial class ChasmComponent : Component
{
    /// <summary>
    /// Entities allowed to fall into the hole. If null, anything not on the blacklist can fall into the hole. If both
    /// are null, anything can.
    /// </summary>
    [DataField]
    public EntityWhitelist? Whitelist;

    /// <summary>
    /// Entities not allowed to fall into the hole. If null, anything on the whitelist can fall into the hole. If both
    /// are null, anything can.
    /// </summary>
    [DataField]
    public EntityWhitelist? Blacklist;

    /// <summary>
    /// Sound that should be played when an entity falls into the chasm
    /// </summary>
    [DataField]
    public SoundSpecifier FallingSound = new SoundPathSpecifier("/Audio/Effects/falling.ogg");

    /// <summary>
    /// Optional emote that should play when an entity falls into the chasm.
    /// </summary>
    [DataField]
    public ProtoId<EmotePrototype>? Emote = "Scream";
}
