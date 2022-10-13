# GfdbFramework

[![NuGet version (Newtonsoft.Json)](https://img.shields.io/nuget/v/GfdbFramework.svg?style=flat-square)](https://www.nuget.org/packages/GfdbFramework/)

**这是一个可以让你用拉姆达表达式执行各种数据库操作的框架**

在正式使用该框架之前，首先你得准备好实体类以及数据操作上下文类，可参考[Entities](GfdbFramework.Test/Entities)以及[DataContext.cs](GfdbFramework.Test/DataContext.cs)实现

## 对比 Linq To Sql
| 功能点 | Linq To Sql | GfdbFramework | 描述 |
| :----- | :----: | :----: | :----- |
| 创建数据库或表 | × | √ | 免去各种初始化数据库的操作 |
| 直接删除和修改数据 | × | √ | GfdbFramework 可直接修改或插入实体数据，和 ADO.NET 效率一致 |
| 多数据库支持 | × | √ | GfdbFramework 作者已实现 MSSql、MySql、Sqlite 支持，其他数据库可自行实现支持 |
| insert ... select ... 语法 | × | √ | GfdbFramework 可直接将查询结果插入到数据库 |
| delete ... from ... 语法 | × | √ | GfdbFramework 支持关联删除语法 |
| 实体类复杂度 | ★★★★★ | ✰ | GfdbFramework 支持任意实体类做为映射类，而 Linq To Sql 必须实现各种接口 |
| 手动提交数据修改 | √ | × | GfdbFramework 修改即提交， Linq To Sql 必须频繁调用 SubmitChanges() 方法 |
| 开源 | × | √ | GfdbFramework 托管于 Github，任何人都可以下载并添加自己想要的功能 |
| 语义语法 | √ | × | GfdbFramework 不支持语义语法，只支持拉姆达表达式操作，美中不足 |

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
3. 向 Users 中添加一条用户信息
```c#
DataContext dataContext = new DataContext();

Entities.User user = new Entities.User()
{
    Account = "Admin",
    Password = "123456",
    Name = "著管理员",
    CreateTime = DateTime.Now,
    JobNumber = "000001"
};

ntext.Users.Insert(user);

Console.WriteLine($"新增用户 ID 为：{user.ID}");
```
4. 获取 Users 表中所有数据（直接循环该表对应的对象即可，会在首次尝试读取数据时查询表中所有数据）
```c#
DataContext dataContext = new DataContext();

foreach (var item in dataContext.Users)
{
    Console.WriteLine(item.Name);
}
```
5. 内连接（查询所有商品名称以及创建该商品的用户名）
```c#
DataContext dataContext = new DataContext();

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
6. 左外连接（查询所有商品名称以及创建该商品的用户主键 ID）
```c#
DataContext dataContext = new DataContext();

var data = dataContext.Commodities.InnerJoin(dataContext.Users, (commodity, user) => new
{
    UserID = user.ID.ToNull(),
    CommodityName = commodity.Name
}, (commodity, user) => commodity.CreateUID == user.ID);

foreach (var item in data)
{
    Console.WriteLine($"商品 {item.CommodityName} 由用户 ID 为 {(item.UserID.HasValue ? item.UserID.Value : 0)} 的用户创建");
}
```
