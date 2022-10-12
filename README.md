# GfdbFramework

[![NuGet version (Newtonsoft.Json)](https://img.shields.io/nuget/v/GfdbFramework.svg?style=flat-square)](https://www.nuget.org/packages/GfdbFramework/)

**这是一个可以让你用拉姆达表达式执行各种数据库操作的框架**

在正式使用该框架之前，首先你得准备好实体类以及数据操作上下文类，可参考[Entities](GfdbFramework.Test/Entities)以及[DataContext.cs](GfdbFramework.Test/DataContext.cs)实现

## 使用教程
1. 创建数据库
```c#
DataContext dataContext = new DataContext();

dataContext.CreateDatabase(new DatabaseInfo()
{
    Name = "TestDB"
});
```
