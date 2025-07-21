namespace TicTacToe.Domain.ValueObjects;

public class Mark
{
    private const byte EmptyValue = byte.MaxValue;

    private readonly byte Value;
    public bool IsEmpty => Value == EmptyValue;
    public bool IsOccupied => !IsEmpty;
    public byte? PlayerIndex => IsEmpty ? null : Value;

    public static readonly Mark Empty = new(EmptyValue);
    
    public static Mark FromPlayerIndex(byte playerIndex)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(playerIndex, byte.MaxValue);
        return new Mark(playerIndex);
    }

    private Mark(byte value) => Value = value;

    public override int GetHashCode() => Value;
    public override bool Equals(object? other) => other is Mark mark && Value == mark.Value;
}