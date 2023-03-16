namespace HelloWorld.Services;

public interface IKeyValueStorage
{
    string Get(string key, string defaultValue);
    void Set(string key, string value);
}

#region 键值数据 is not键值存储

internal class DictionaryDemo
{
    public void SomeFunction()
    {
        Dictionary<string, int> hashTable = new Dictionary<string, int>();
        hashTable["zhangsan"] = 100;
        hashTable["lisi"] = 200;

        var value01 = hashTable["zhangsan"]; //100
    }
}

#endregion