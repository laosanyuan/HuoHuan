$tmpScript = "./tmp_release_script.nsi"
# 编译发布
dotnet clean
dotnet restore
dotnet publish -p:PublishProfile=FolderProfile

# 读取版本号
$xml_data = [xml](Get-Content ./HuoHuan/HuoHuan.csproj)
$version =  $xml_data.Project.PropertyGroup.Version
"版本号： $version"

# 创建临时NSIS脚本文件
$original_txt = (Get-Content ./release_script.nsi)
New-Item $tmpScript -type file
Set-Content $tmpScript $original_txt
(Get-Content $tmpScript) -Replace '#replace_version#', $version | Set-Content $tmpScript

# 需要将NSIS脚本安装路径设置到环境变量中
makensis $tmpScript

# 删除NSIS临时文件
Remove-Item $tmpScript