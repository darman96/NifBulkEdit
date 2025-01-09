namespace NifBulkEdit.Core.Dtos;

public record NifTextureSlotMask(
    bool Slot0,
    bool Slot1,
    bool Slot2,
    bool Slot3,
    bool Slot4,
    bool Slot5,
    bool Slot6,
    bool Slot7,
    bool Slot8)
{
    public bool Get(int index)
        => index switch
        {
            0 => Slot0,
            1 => Slot1,
            2 => Slot2,
            3 => Slot3,
            4 => Slot4,
            5 => Slot5,
            6 => Slot6,
            7 => Slot7,
            8 => Slot8,
            _ => throw new ArgumentOutOfRangeException(nameof(index))
        };
}