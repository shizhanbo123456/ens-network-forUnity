using System;
using System.Collections.Generic;
using System.Text;

public static class Format
{
    private const char BoundaryStart = '{';
    private const char BoundaryEnd = '}';
    private const char ListSeparator = '/';
    private const char DictionaryPair = ':';
    private const char DictionarySeparator = ',';

    public static string DictionaryToString<Tkey,Tvalue>(Dictionary<Tkey,Tvalue>dict,
        char pair=DictionaryPair,char separator=DictionarySeparator,bool addboundary=true,
        Func<Tkey,string> keyconverter=null,Func<Tvalue,string>valueconverter=null)
    {
        StringBuilder sb = new StringBuilder();
        if (keyconverter == null) keyconverter = t => t.ToString();
        if(valueconverter == null) valueconverter = t => t.ToString();

        bool first = true;
        if (addboundary)
        {
            foreach (var i in dict)
            {
                if (first) first = false;
                else sb.Append(pair);
                sb.Append(BoundaryStart + keyconverter.Invoke(i.Key) + BoundaryEnd+pair+BoundaryStart + valueconverter.Invoke(i.Value) +BoundaryEnd);
            }
        }
        else
        {
            foreach (var i in dict)
            {
                if (first) first = false;
                else sb.Append(pair);
                sb.Append(i.Key.ToString() + separator + i.Value.ToString());
            }
        }
        return sb.ToString();
    }
    public static Dictionary<Tkey, Tvalue> StringToDictionary<Tkey, Tvalue>(string data, Func<string, Tkey> keyconverter, Func<string, Tvalue> valueconverter, char pair = DictionaryPair, char separator = DictionarySeparator, bool removeboudary = true)
    {
        Dictionary<Tkey, Tvalue> r = new Dictionary<Tkey, Tvalue>();
        var s = data.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        if (removeboudary)
        {
            foreach (var i in s)
            {
                var list = SplitWithBoundaries(i, pair);
                r.Add(keyconverter.Invoke(list[0]), valueconverter.Invoke(list[1]));
            }
        }
        else
        {
            foreach (var i in s)
            {
                var list = i.Split(pair);
                r.Add(keyconverter.Invoke(list[0]), valueconverter.Invoke(list[1]));
            }
        }
        return r;
    }

    public static string ListToString<T>(IEnumerable<T> list, char c = ListSeparator)
    {
        StringBuilder sb= new StringBuilder();
        bool isfirst = true;
        foreach(var i in list)
        {
            if(isfirst)isfirst= false;
            else sb.Append(ListSeparator);
            sb.Append(i.ToString());
        }
        return sb.ToString();
    }
    public static List<T> StringToList<T>(string a,Func<string,T>converter, char c = ListSeparator)
    {
        string[] s = a.Split(c, StringSplitOptions.RemoveEmptyEntries);
        List<T> list = new List<T>();
        foreach (var i in s) list.Add(converter.Invoke(i));
        return list;
    }


    public static List<string> SplitWithBoundaries(string s, char separator = ListSeparator, char boundaryStart = BoundaryStart, char boundaryEnd = BoundaryEnd)
    {
        List<string> result = new List<string>();
        int currentStart = 0;
        int boundaryDepth = 0; // 用于处理嵌套边界的情况

        for (int i = 0; i < s.Length; i++)
        {
            char c = s[i];

            // 处理边界开始符
            if (c == boundaryStart)
            {
                boundaryDepth++;
            }
            // 处理边界结束符
            else if (c == boundaryEnd)
            {
                if (boundaryDepth > 0)
                {
                    boundaryDepth--;
                }
                // 忽略不匹配的边界结束符
            }
            // 当遇到分隔符且不在边界内时进行分割
            else if (c == separator && boundaryDepth == 0)
            {
                // 提取当前片段并处理边界
                string segment = s.Substring(currentStart, i - currentStart);
                string processedSegment = ProcessBoundaries(segment, boundaryStart, boundaryEnd);
                result.Add(processedSegment);

                currentStart = i + 1; // 移动到下一个片段的起始位置
            }
        }

        // 添加最后一个片段
        if (currentStart <= s.Length - 1)
        {
            string lastSegment = s.Substring(currentStart);
            string processedLastSegment = ProcessBoundaries(lastSegment, boundaryStart, boundaryEnd);
            result.Add(processedLastSegment);
        }

        return result;
    }
    // 处理片段中的边界符，移除最外层的边界
    private static string ProcessBoundaries(string segment, char boundaryStart, char boundaryEnd)
    {
        // 检查是否同时包含完整的边界符
        if (segment.Length >= 2 && segment[0] == boundaryStart && segment[segment.Length - 1] == boundaryEnd)
        {
            // 移除最外层的边界符
            return segment.Substring(1, segment.Length - 2);
        }
        return segment;
    }
}