using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Shared.Domain.Services;

public class PrivateSetterJsonResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(
        MemberInfo member,
        MemberSerialization memberSerialization
    )
    {
        var prop = base.CreateProperty(member, memberSerialization);
        if (prop.Writable)
            return prop;
        if (member is PropertyInfo property)
        {
            var hasPrivateSetter = property.GetSetMethod(true) is not null;
            prop.Writable = hasPrivateSetter;
        }

        return prop;
    }
}

public static class JsonSerializer
{
    public static void PrivateSetterAllow(this JsonSerializerSettings settings)
    {
        settings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
        settings.ContractResolver = new PrivateSetterJsonResolver();
    }
}
