using SevenZip.Compression.LZMA;

namespace PixelWorldsServer.Protocol;

public static class LZMATools
{
    public static byte[] Compress(byte[] data)
    {
        using var stream = new MemoryStream(data);
        return Compress(stream);
    }

    public static byte[] Compress(Stream stream)
    {
        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms);

        var ec = new Encoder();
        ec.WriteCoderProperties(ms);
        bw.Write(stream.Length);
        ec.Code(stream, ms, stream.Length, -1, null);
        bw.Flush();

        return ms.ToArray();
    }

    public static byte[] Decompress(Stream stream)
    {
        using var br = new BinaryReader(stream);
        using var ms = new MemoryStream();

        var properties = br.ReadBytes(5);
        var outSize = br.ReadInt64();

        var dc = new Decoder();
        dc.SetDecoderProperties(properties);
        dc.Code(stream, ms, stream.Length, outSize, null);
        ms.Flush();

        return ms.ToArray();
    }
}
