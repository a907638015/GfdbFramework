# GfdbFramework

[![NuGet version (GfdbFramework)](https://img.shields.io/nuget/v/GfdbFramework.svg?style=flat-square)](https://www.nuget.org/packages/GfdbFramework/)

**这是一个可以让你用拉姆达表达式执行各种数据库操作的框架，除非你手动实现接口，否则你无需引用该框架，只需要引用 GfdbFramework.SqlServer、GfdbFramework.MySql、GfdbFramework.Sqlite 任意一个即可，示例程序以 GfdbFramework.SqlServer 为准，你可以直接下载示例程序运行查看效果（需要修改 GfdbFramework.Test/DataContext.cs 类中连接字符串的账号密码，数据库会自动创建）**

在正式使用该框架之前，首先你得准备好实体类以及数据操作上下文类，可参考[Entities](GfdbFramework.Test/Entities)以及[DataContext.cs](GfdbFramework.Test/DataContext.cs)实现

查看 [更新日志](../../wiki/更新日志) 或 [常见问题](../../wiki/常见问题)

## 对比 Linq To Sql
| 功能点 | Linq To Sql | GfdbFramework | 描述 |
| :----- | :----: | :----: | :----- |
| 直接删除和修改数据 | × | √ | GfdbFramework 可直接修改或插入实体数据，和 ADO.NET 效率一致 |
| 多数据库支持 | × | √ | GfdbFramework 作者已实现 MSSql、MySql、Sqlite 支持，其他数据库可自行实现支持 |
| insert ... select ... 语法 | × | √ | GfdbFramework 可直接将查询结果插入到数据库 |
| delete ... from ... 语法 | × | √ | GfdbFramework 支持关联删除语法 |
| 实体类复杂度 | ★★★★★ | ✰ | GfdbFramework 支持任意实体类做为映射类，而 Linq To Sql 必须实现各种接口，且 Linq To Sql 实体类不支持继承 |
| 手动提交数据修改 | √ | × | GfdbFramework 修改即提交， Linq To Sql 必须频繁调用 SubmitChanges() 方法 |
| 开源 | × | √ | GfdbFramework 托管于 Github，任何人都可以下载并添加自己想要的功能 |
| 语义语法 | √ | × | GfdbFramework 不支持语义语法，只支持拉姆达表达式操作，美中不足 |

## 使用教程（仅包含最常见的用法，更多功能请自行探索）
1. 创建数据库
```c#
DataContext dataContext = new DataContext();

//默认数据库文件存放在 Databases 目录下
dataContext.CreateDatabase(new DatabaseInfo("TestDB"));
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

dataContext.Users.Insert(user);

Console.WriteLine($"新增用户 ID 为：{user.ID}");
```
4. 条件 in （查询创建 波力海苔 以及 人参果 两个商品的用户信息）
```c#
DataContext dataContext = new DataContext();

var users = dataContext.Users.Where(user => dataContext.Commodities.Select(commodity => commodity.CreateUID).Where(commodity => commodity.Name == "波力海苔" || commodity.Name == "人参果").Contains(user.ID));

foreach (var item in users)
{
    Console.WriteLine($"创建 波力海苔 或 人参果 的用户名称：{item.Name}");
}
```
5. 条件 like （查询商品名称包含 硬壳 的商品信息）
```c#
DataContext dataContext = new DataContext();

var commodities = dataContext.Commodities.Where(commodity => commodity.Name.Like("%硬壳%"));

foreach (var item in commodities)
{
    Console.WriteLine($"包含 硬壳 的商品名称：{item.Name}");
}
```
6. 普通修改（将 东北大米 商品名称改成 泰国香米）
```c#
DataContext dataContext = new DataContext();

dataContext.Commodities.Update(new Entities.Commodity()
{
    Name = "泰国香米"
}, commodity => commodity.Name == "东北大米");
```
8. 主键删除（将 ID 值为 100 的用户删除）
```c#
DataContext dataContext = new DataContext();

dataContext.Users.Delete(100);
```
9. 获取 Users 表中所有数据（直接循环该表对应的对象即可，会在首次尝试读取数据时查询表中所有数据）
```c#
DataContext dataContext = new DataContext();

foreach (var item in dataContext.Users)
{
    Console.WriteLine(item.Name);
}
```
10. 内连接（查询所有商品名称以及创建该商品的用户名）
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
11. 左外连接（查询所有商品名称以及创建该商品的用户主键 ID）
```c#
DataContext dataContext = new DataContext();

var data = dataContext.Commodities.LeftJoin(dataContext.Users, (commodity, user) => new
{
    UserID = user.ID.ToNull(),
    CommodityName = commodity.Name
}, (commodity, user) => commodity.CreateUID == user.ID);

foreach (var item in data)
{
    Console.WriteLine($"商品 {item.CommodityName} 由用户 ID 为 {(item.UserID.HasValue ? item.UserID.Value : 0)} 的用户创建");
}
```
12. 查询插入（新增一条父级分类为化妆品的子分类）
```c#
DataContext dataContext = new DataContext();

dataContext.Classifies.Insert(dataContext.Classifies.Select(classify => new Entities.Classify()
{
    Name = "口红",
    ParentID = classify.ID,
    Code = GetRandomString(6),
    CreateTime = DateTime.Now,
    CreateUID = dataContext.Users.Select(user => user.ID).Ascending(user => DBFun.NewID()).First()
}).Where(classify => classify.Name == "化妆品"));
```
13. 关联修改（将张三创建的商品名称改成 张三的登录账号 + 原有商品名称）
```c#
DataContext dataContext = new DataContext();

dataContext.Commodities.InnerJoin(dataContext.Users, (commodity, user) => commodity.CreateUID == user.ID).Update(source => new Entities.Commodity()
{
    Name = source.Right.Account + source.Left.Name
}, source => source.Right.Name == "张三");
```
14. 关联删除（将 李四 用户创建的商品删除）
```c#
DataContext dataContext = new DataContext();

dataContext.Commodities.InnerJoin(dataContext.Users, (left, right) => left.CreateUID == right.ID).Delete(source => source.Right.Name == "李四");
```
15. 多表关联（查询商品名称、最大包装单位、中包包装单位、最小单位、分类名称、品牌名称）
```c#
DataContext dataContext = new DataContext();

var data = dataContext.Commodities
    .InnerJoin(dataContext.Classifies, (left, right) => left.ClassifyID == right.ID)
    .InnerJoin(dataContext.Units, (left, right) => left.Left.PackageUnitID == right.ID)
    .InnerJoin(dataContext.Units, (left, right) => left.Left.Left.MiddleUnitID == right.ID)
    .InnerJoin(dataContext.Units, (left, right) => left.Left.Left.Left.MinimumUnitID == right.ID)
    .LeftJoin(dataContext.Brands, (left, right) => left.Left.Left.Left.Left.BrandID == right.ID)
    .Select(source => new
    {
        Name = source.Left.Left.Left.Left.Left.Name,
        Classify = source.Left.Left.Left.Left.Right.Name,
        PackageUnit = source.Left.Left.Left.Right.Name,
        MiddleUnit = source.Left.Left.Right.Name,
        MinimumUnit = source.Left.Right.Name,
        Brand = source.Right.Name
    });

foreach (var item in data)
{
    Console.WriteLine($"商品名称：{item.Name}，分类：{item.Classify}，最大包装单位：{item.PackageUnit}，中包包装单位：{item.MiddleUnit}，零售包装单位：{ item.MinimumUnit}，品牌：{ item.Brand}");
}
```
16. 数据合并 union all（查询 ID 大于 1 的用户名以及 ID 大于 1 的商品名）
```c#
DataContext dataContext = new DataContext();

var data = dataContext.Users.Select(item => new
{
    item.ID,
    item.Name,
    Type = "User"
}).Where(item => item.ID > 1).UnionAll(dataContext.Commodities.Select(item => new
{
    item.ID,
    item.Name,
    Type = "Commodity"
}).Where(item => item.ID > 1));

foreach (var item in data)
{
    Console.WriteLine($"类型：{(item.Type == "User" ? "用户：" : "商品：")}{item.Name} ID:{item.ID}");
}
```
