# Migration 創建Table

## 一.新建Entity
建立路徑:Innolux.EFCore\Model\

## 二.建立Table的配置文檔Configs
建立路徑:Innolux.EFCore\Configs\

## 三.添加DBset
在Innolux.EFCore\DbContexts.cs文件中添加DBset

## 四.建立DbBuildFactory
建立路徑:Innolux.EFCore\DbBuildFactory\

## 五.使用Migration 創建Table

1. Innolux.EFCore設定為啟動專案
2. 添加Migration的腳本
```
add-migration addyujian4 -context Innolux.EFCore.OQCDBConnection
```
3. 更新到DB上
```
update-database  -context Innolux.EFCore.OQCDBConnection
```