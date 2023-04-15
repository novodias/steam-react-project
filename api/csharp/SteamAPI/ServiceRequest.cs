using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using SteamReactProject.SteamAPI.Attributes;

namespace SteamReactProject.SteamAPI;

public abstract class ServiceRequest 
{
    protected string _interface = "";
    protected string _base = "";

    protected Uri CreateUri(string path, string query)
        => new UriBuilder(_base) { Path = path, Query = query }
            .Uri;

    protected string CreatePath([CallerMemberName] string caller = "")
    {
        var delimiter = '/';

        var type = GetType();
        var attributeType = typeof(MethodConfiguration);
        var method = GetType().GetMethod(caller);

        if (Attribute.GetCustomAttribute(method!, attributeType) is not MethodConfiguration attribute)
        {
            var message = string.Format("Attribute MethodConfiguration is not set for {0} at {1}", caller, type.Name);
            throw new NullReferenceException(message);
        }

        string path = _interface + delimiter + 
            attribute.Method + delimiter + 
            attribute.Version + delimiter;
        // string path = directories.Aggregate((current, next) => current + delimiter + next);

        return path;
    }

    protected string CreateQuery(params Query[] queries)
    {
        var delimiter = '&';
        
        string query = queries.Select(query => string.Format("{0}={1}", query.Key, query.Value))
            .Aggregate((current, next) => current + delimiter + next);

        return query;
    }

    protected string SerializeAnonymousObject(object anonymous)
    {
        return JsonConvert.SerializeObject(anonymous, new JsonSerializerSettings() 
        {
            NullValueHandling = NullValueHandling.Ignore
        });
    }
}

public record Query(string Key, object Value);