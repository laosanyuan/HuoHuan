#addin nuget:?package=Cake.FileHelpers
#addin nuget:?package=Cake.Yaml
#addin nuget:?package=YamlDotNet&version=6.1.2

using Cake.FileHelpers;

var target = Argument("target", "Default");

public class VersionInfo
{
    public string Version { get; set; }
    public string Title { get; set; }
	public string[] Messages { get; set; }
}

Task("UpdateVersion")
    .Does(() =>
{
    // 反序列化VersionInfo文件
    var config = Context.DeserializeYamlFromFile<VersionInfo>("../VersionInfo.yml");
    var version = config.Version;
    // 更新项目版本号
    ReplaceRegexInFiles("./**/*.csproj","(?<=<Version>).*?(?=</Version>)",version);
});

Task("Default")
    .IsDependentOn("UpdateVersion");

RunTarget(target);
