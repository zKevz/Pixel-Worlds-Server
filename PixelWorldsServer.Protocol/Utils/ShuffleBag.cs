namespace PixelWorldsServer.Protocol.Utils;

public class ShuffleBag<T>
{
    private int m_Cursor = -1;
    private readonly List<T> m_Data = new(100);
    private readonly Random m_Random = new();

    public void Add(T item, int count)
    {
        while (count-- > 0)
        {
            m_Data.Add(item);
        }
        m_Cursor = m_Data.Count - 1;
    }

    public T? Next()
    {
        if (m_Data.Count == 0)
        {
            return default;
        }

        if (m_Cursor < 1)
        {
            m_Cursor = m_Data.Count - 1;
            return m_Data[0];
        }

        int index = m_Random.Next(0, m_Cursor + 1);

        T value = m_Data[index];
        m_Data[index] = m_Data[m_Cursor];
        m_Data[m_Cursor] = value;
        m_Cursor--;

        return value;
    }

    public void Clear()
    {
        m_Data.Clear();
    }
}
