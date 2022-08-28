dotnet clean
dotnet restore
dotnet publish -p:PublishProfile=FolderProfile

# 如无法识别，先将NSIS安装路径设置到环境变量中
makensis release_script.nsi