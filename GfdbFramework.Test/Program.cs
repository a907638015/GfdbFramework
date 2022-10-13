using System;
using System.Collections.Generic;
using System.Text;
using GfdbFramework.Attribute;
using GfdbFramework.Core;
using GfdbFramework.Realize;

namespace GfdbFramework.Test
{
    class Program
    {
        static readonly Random rd = new Random();
        static readonly string[] userNames = new string[] { "张三", "李四", "王五", "赵六", "孙八", "周九" };
        static readonly string[] brandNames = new string[] { "白沙", "利群", "芙蓉王", "黄金叶", "中华", "红塔山", "红双喜" };
        static readonly string[] classifyNames = new string[] { "日用品", "水产品", "蔬菜", "坚果", "化妆品", "零食", "医疗用品", "其他" };
        static readonly string[] unitNames = new string[] { "克", "千克", "斤", "两", "吨", "方", "包", "箱", "台", "件" };

        static void Main(string[] args)
        {
            DataContext dataContext = new DataContext();

            if (!dataContext.ExistsDatabase("TestDB2"))
            {
                Console.WriteLine($"{Environment.NewLine}----- 初始化数据库 ----{Environment.NewLine}");

                //创建数据库（在测试时建议给当前程序目录添加 Everyone 的全部访问权限，不然极有可能报 拒绝访问 错误信息）
                dataContext.CreateDatabase(new DatabaseInfo("TestDB2"));

                //创建各种表
                dataContext.CreateTable(dataContext.Users);
                dataContext.CreateTable(dataContext.Commodities);
                dataContext.CreateTable(dataContext.Classifies);
                dataContext.CreateTable(dataContext.Brands);
                dataContext.CreateTable(dataContext.Units);

                Console.WriteLine($"{Environment.NewLine}----- 随机添加测试数据 ----{Environment.NewLine}");

                //添加用户信息
                for (int i = 0; i < userNames.Length; i++)
                {
                    Entities.User user = new Entities.User()
                    {
                        Account = GetRandomString(16),
                        Password = "123456",
                        Name = userNames[i],
                        CreateTime = DateTime.Now,
                        JobNumber = GetRandomString(5)
                    };

                    dataContext.Users.Insert(user);

                    Console.WriteLine($"新增用户 ID 为：{user.ID}");
                }

                //添加分类信息
                for (int i = 0; i < classifyNames.Length; i++)
                {
                    //此处会生成两个插入 SQL，第一条 SQL 先去随机获得一个用户 ID，然后再将这个 ID 传入插入 SQL 作为变量使用
                    dataContext.Classifies.Insert(new Entities.Classify()
                    {
                        Code = GetRandomString(6),
                        Name = classifyNames[i],
                        CreateTime = DateTime.Now,
                        CreateUID = dataContext.Users.Select(user => user.ID).Ascending(user => DBFun.NewID()).First()  //随机获取一个用户 ID 作为创建者
                    });
                }

                //添加品牌信息
                for (int i = 0; i < brandNames.Length; i++)
                {
                    //此处只会生成一个插入 SQL， CreateUID 的值会改成子查询
                    dataContext.Brands.Insert(() => new Entities.Brand()
                    {
                        Code = GetRandomString(6),
                        Name = brandNames[i],
                        CreateTime = DateTime.Now,
                        CreateUID = dataContext.Users.Select(user => user.ID).Ascending(user => DBFun.NewID()).First()  //随机获取一个用户 ID 作为创建者
                    });
                }

                //添加单位信息
                for (int i = 0; i < unitNames.Length; i++)
                {
                    dataContext.Units.Insert(() => new Entities.Unit()
                    {
                        Name = unitNames[i],
                        CreateTime = DateTime.Now,
                        CreateUID = dataContext.Users.Select(user => user.ID).Ascending(user => DBFun.NewID()).First()  //随机获取一个用户 ID 作为创建者
                    });
                }
            }

            Console.WriteLine($"{Environment.NewLine}----- 获取 Users 表中所有数据 ----{Environment.NewLine}");

            //获取 Users 表中所有数据（直接循环该表对应的对象即可，会在首次尝试读取数据时查询表中所有数据）
            foreach (var item in dataContext.Users)
            {
                Console.WriteLine($"用户名称：{item.Name}");
            }

            Console.WriteLine($"{Environment.NewLine}----- 查询所有商品名称以及创建该商品的用户名（内连接） ----{Environment.NewLine}");

            //内连接（查询所有商品名称以及创建该商品的用户名）
            var data = dataContext.Commodities.InnerJoin(dataContext.Users, (commodity, user) => new
            {
                UserName = user.Name,
                CommodityName = commodity.Name
            }, (commodity, user) => commodity.CreateUID == user.ID);

            foreach (var item in data)
            {
                Console.WriteLine($"商品 【{item.CommodityName}】 由 【{item.UserName}】 创建");
            }

            Console.WriteLine($"{Environment.NewLine}----- 查询所有商品名称以及创建该商品的用户主键 ID（左外连接） ----{Environment.NewLine}");

            //左外连接（查询所有商品名称以及创建该商品的用户主键 ID）
            var data1 = dataContext.Commodities.LeftJoin(dataContext.Users, (commodity, user) => new
            {
                UserID = user.ID.ToNull(),
                CommodityName = commodity.Name
            }, (commodity, user) => commodity.CreateUID == user.ID);

            foreach (var item in data1)
            {
                Console.WriteLine($"商品 【{item.CommodityName}】 由用户 ID 为 【{(item.UserID.HasValue ? item.UserID.Value : 0)}】 的用户创建");
            }
        }

        static string GetRandomString(int len)
        {
            string str = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_@";
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < len; i++)
            {
                result.Append(str[rd.Next(str.Length)]);
            }

            return result.ToString();
        }
    }
}
