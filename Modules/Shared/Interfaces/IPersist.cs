using System.Collections.Generic;

public interface IPersist
{
    Dictionary<string, object> Save();
    void Load(Dictionary<string, string> data);
}
