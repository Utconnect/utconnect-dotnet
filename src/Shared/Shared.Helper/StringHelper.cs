using Diacritics;

namespace Shared.Helper;

public class StringHelper(IDiacriticsMapper diacriticsMapper)
{
    public string RemoveDiacritics(string str)
    {
        return diacriticsMapper.RemoveDiacritics(str);
    }
}