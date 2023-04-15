namespace SteamReactProject.SteamAPI.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class MethodConfiguration : Attribute
{
    public readonly string Method;
    public readonly string Version;

    public MethodConfiguration(string method, string version)
    {
        Method = method;
        Version = version;
    }
}