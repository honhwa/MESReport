# 簡紹
本篇記錄如何使用**cmd:dotnet**配合**VScode**指令去開發C#程式

## 環境搭建
1. 獲取管理員權限
2. 到Microsoft下載對應的C# framework 的SDK安裝
3. 到Microsoft下載 VScode的安裝包安裝
4. 安裝VScode安裝名為 **C#** 的插件
5. 安裝 **vscode-solution-explorer** 的插件
6. 安裝 **Auto-Using for C#** 的插件

## 指令創建項目

1. 創建文件夾并進入文件夾
2. 創建sln解決方案
```powershell
dotnet new sln
```
3. 創建你的項目
```powershell
# 查看你擁有的C# SDK
dotnet --list-sdks
# 我這裡用的是.net6 且不啟用https 命名為Innolux.Api
dotnet new webapi --framework net6.0 --no-https -o Innolux.Api
```
4. 
```powershell
# 把你的項目加入到解決方案裡面
dotnet sln add Innolux.Api
```
5. 
```powershell
# 新增類庫項目
dotnet new classlib --framework net6.0 -o Innolux.Core
```
6. (這是windows的cmd開發,如果是linux開發,斜桿要使用/)
```powershell
# 將類庫添加到api的引用
dotnet add .\Innolux.Api\Innolux.Api.csproj reference .\Innolux.Core\Innolux.Core.csproj

# 移除引用
dotnet remove .\Innolux.Api\Innolux.Api.csproj reference .\Innolux.Core\Innolux.Core.csproj
```
7. 
```powershell
# 添加nuget引用
dotnet add .\Innolux.Core\Innolux.Core.csproj package newtonsoft.json 

# 移除引用
dotnet remove .\Innolux.Core\Innolux.Core.csproj package newtonsoft.json 
```
8. 到VSCode的左邊工具欄**執行與偵錯**去新建一個VSCode開發debug逐步執行的.vs的文件夾項目(這個會自動生成)