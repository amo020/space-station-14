using Content.Shared.Tools;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Audio;
using Robust.Shared.Containers;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.PAI;

/// <summary>
/// pAIs, or Personal AIs, are essentially portable ghost role generators.
/// In their current implementation in SS14, they create a ghost role anyone can access,
/// and that a player can also "wipe" (reset/kick out player).
/// Theoretically speaking pAIs are supposed to use a dedicated "offer and select" system,
///  with the player holding the pAI being able to choose one of the ghosts in the round.
/// This seems too complicated for an initial implementation, though,
///  and there's not always enough players and ghost roles to justify it.
/// All logic in PAISystem.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class PAIComponent : Component
{
    #region SmallAIChip
    /// <summary>
    ///     The tool required to extract the SmallAIChip from device.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("smallAIChipExtractionMethod", customTypeSerializer: typeof(PrototypeIdSerializer<ToolQualityPrototype>))]
    public string SmallAIChipExtractionMethod = "Screwing";

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("smallAIChipExtractionSound")]
    public SoundSpecifier SmallAIChipExtractionSound = new SoundPathSpecifier("/Audio/Items/pistol_magout.ogg");

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("smallAIChipInsertionSound")]
    public SoundSpecifier SmallAIChipInsertionSound = new SoundPathSpecifier("/Audio/Items/pistol_magin.ogg");

    /// <summary>
    ///     The id container for the super compact AI Chip
    /// </summary>
    [ViewVariables]
    public Container SmallAIChipContainer = default!;
    public const string SmallAIChipContainerName = "smallaichip_slot";

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("smallAIChipSlots")]
    public int SmallAIChipSlots = 1;
    #endregion

    /// <summary>
    /// The last person who activated this PAI.
    /// Used for assigning the name.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public EntityUid? LastUser;

    [DataField(serverOnly: true)]
    public EntProtoId? MidiActionId = "ActionPAIPlayMidi";

    [DataField(serverOnly: true)] // server only, as it uses a server-BUI event !type
    public EntityUid? MidiAction;

    [DataField]
    public ProtoId<EntityPrototype> MapActionId = "ActionPAIOpenMap";

    [DataField, AutoNetworkedField]
    public EntityUid? MapAction;

    /// <summary>
    /// When microwaved there is this chance to brick the pai, kicking out its player and preventing it from being used again.
    /// </summary>
    [DataField]
    public float BrickChance = 0.5f;

    /// <summary>
    /// Locale id for the popup shown when the pai gets bricked.
    /// </summary>
    [DataField]
    public string BrickPopup = "pai-system-brick-popup";

    /// <summary>
    /// Locale id for the popup shown when the pai is microwaved but does not get bricked.
    /// </summary>
    [DataField]
    public string ScramblePopup = "pai-system-scramble-popup";
}
