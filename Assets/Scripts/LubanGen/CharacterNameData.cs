
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;
using SimpleJSON;


namespace cfg
{
public sealed partial class CharacterNameData : Luban.BeanBase
{
    public CharacterNameData(JSONNode _buf) 
    {
        { if(!_buf["ID"].IsNumber) { throw new SerializationException(); }  ID = _buf["ID"]; }
        { if(!_buf["FirstName"].IsString) { throw new SerializationException(); }  FirstName = _buf["FirstName"]; }
        { if(!_buf["LastName"].IsString) { throw new SerializationException(); }  LastName = _buf["LastName"]; }
    }

    public static CharacterNameData DeserializeCharacterNameData(JSONNode _buf)
    {
        return new CharacterNameData(_buf);
    }

    public readonly int ID;
    public readonly string FirstName;
    public readonly string LastName;
   
    public const int __ID__ = 177391358;
    public override int GetTypeId() => __ID__;

    public  void ResolveRef(Tables tables)
    {
        
        
        
    }

    public override string ToString()
    {
        return "{ "
        + "ID:" + ID + ","
        + "FirstName:" + FirstName + ","
        + "LastName:" + LastName + ","
        + "}";
    }
}

}