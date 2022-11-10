using PixelWorldsServer.Protocol.Constants;

namespace PixelWorldsServer.Protocol.Utils;

public class BlockTuple
{
    public BlockType First;

    public BlockType Second;

    public BlockTuple(BlockType newFirst, BlockType newSecond)
    {
        if (newFirst > newSecond)
        {
            First = newFirst;
            Second = newSecond;
        }
        else
        {
            First = newSecond;
            Second = newFirst;
        }
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (obj is not BlockTuple blockTuple)
        {
            return false;
        }

        return First == blockTuple.First && Second == blockTuple.Second;
    }

    public override int GetHashCode()
    {
        return (int)First << (int)(16 + Second);
    }
}
