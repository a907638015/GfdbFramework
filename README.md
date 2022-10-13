# GfdbFramework

[![NuGet version (Newtonsoft.Json)](https://img.shields.io/nuget/v/GfdbFramework.svg?style=flat-square)](https://www.nuget.org/packages/GfdbFramework/)

**这是一个可以让你用拉姆达表达式执行各种数据库操作的框架**

在正式使用该框架之前，首先你得准备好实体类以及数据操作上下文类，可参考[Entities](GfdbFramework.Test/Entities)以及[DataContext.cs](GfdbFramework.Test/DataContext.cs)实现

## 对比 Linq To Sql
| 左对齐 | 右对齐 | 居中对齐 |
| :-----| ----: | :----: |
| 单元格 | 单元格 | 单元格 |
| 单元格 | 单元格 | 单元格 |

## 使用教程
1. 创建数据库
```c#
DataContext dataContext = new DataContext();

//默认数据库文件存放在 Databases 目录下
dataContext.CreateDatabase(new DatabaseInfo()
{
    Name = "TestDB"
});
```
2. 创建数据表
```c#
DataContext dataContext = new DataContext();

dataContext.CreateTable(dataContext.Users);
```
3. 获取 Users 表中所有数据（直接循环该表对应的对象即可，会在首次尝试读取数据时查询表中所有数据）
```c#
DataContext dataContext = new DataContext();

foreach (var item in dataContext.Users)
{
    Console.WriteLine(item.Name);
}
```
4. 内连接（查询所有商品名称以及创建该商品的用户名）
```c#
var data = dataContext.Commodities.InnerJoin(dataContext.Users, (commodity, user) => new
{
    UserName = user.Name,
    CommodityName = commodity.Name
}, (commodity, user) => commodity.CreateUID == user.ID);

foreach (var item in data)
{
    Console.WriteLine($"商品 {item.CommodityName} 由 {item.UserName} 创建");
}
```
