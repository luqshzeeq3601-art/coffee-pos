using System.Text;

namespace CoffeePos.Infrastructure.Printing;

public sealed class EscPosBuilder
{
    private readonly List<byte> _buffer = new();
    private static readonly Encoding AsciiEncoding = Encoding.ASCII;

    public EscPosBuilder Initialize()
    {
        _buffer.AddRange(new byte[] { 0x1B, 0x40 }); // ESC @
        return this;
    }

    public EscPosBuilder AlignLeft()
    {
        _buffer.AddRange(new byte[] { 0x1B, 0x61, 0x00 }); // ESC a 0
        return this;
    }

    public EscPosBuilder AlignCenter()
    {
        _buffer.AddRange(new byte[] { 0x1B, 0x61, 0x01 }); // ESC a 1
        return this;
    }

    public EscPosBuilder AlignRight()
    {
        _buffer.AddRange(new byte[] { 0x1B, 0x61, 0x02 }); // ESC a 2
        return this;
    }

    public EscPosBuilder SetBold(bool bold)
    {
        _buffer.AddRange(new byte[] { 0x1B, 0x45, (byte)(bold ? 0x01 : 0x00) }); // ESC E n
        return this;
    }

    public EscPosBuilder SetDoubleHeight(bool enabled)
    {
        _buffer.AddRange(new byte[] { 0x1D, 0x21, (byte)(enabled ? 0x10 : 0x00) }); // GS ! n
        return this;
    }

    public EscPosBuilder AppendText(string text)
    {
        _buffer.AddRange(AsciiEncoding.GetBytes(text));
        return this;
    }

    public EscPosBuilder AppendLine(string text = "")
    {
        _buffer.AddRange(AsciiEncoding.GetBytes(text + "\n"));
        return this;
    }

    public EscPosBuilder AppendDivider(char ch = '-', int width = 48)
    {
        _buffer.AddRange(AsciiEncoding.GetBytes(new string(ch, width) + "\n"));
        return this;
    }

    public EscPosBuilder AppendTwoColumnRow(string left, string right, int width = 48)
    {
        var spaceCount = Math.Max(1, width - left.Length - right.Length);
        var row = left + new string(' ', spaceCount) + right;
        return AppendLine(row);
    }

    public EscPosBuilder KickDrawer()
    {
        _buffer.AddRange(new byte[] { 0x1B, 0x70, 0x00, 0x19, 0xFA }); // ESC p 0 25 250 (Pin 2 kick pulse)
        return this;
    }

    public EscPosBuilder FeedAndCut(int linesToFeed = 4)
    {
        for (var i = 0; i < linesToFeed; i++)
        {
            _buffer.Add(0x0A); // LF
        }
        _buffer.AddRange(new byte[] { 0x1D, 0x56, 0x42, 0x00 }); // GS V 66 0 (Full Cut)
        return this;
    }

    public byte[] ToByteArray() => _buffer.ToArray();
    public string ToBase64() => Convert.ToBase64String(_buffer.ToArray());
}
