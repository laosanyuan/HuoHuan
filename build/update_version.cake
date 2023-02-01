#addin "nuget:?package=Cake.FileHelpers&Version=5.0.0"
#addin "nuget:?package=Cake.Yaml&Version=5.0.0"
#addin "nuget:?package=YamlDotNet&Version=12.0.0"

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
    ReplaceRegexInFiles("../src/**/*.csproj","(?<=<Version>).*?(?=</Version>)",version);
});

Task("Default")
    .IsDependentOn("UpdateVersion");

RunTarget(target);
