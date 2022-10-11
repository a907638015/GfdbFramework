using System;
using System.Collections.Generic;
using GfdbFramework.Attribute;
using GfdbFramework.Core;
using GfdbFramework.Realize;

namespace GfdbFramework.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            DataContext dataContext = new DataContext();

            var commodities = dataContext.Commodities.InnerJoin(dataContext.Users, (left, right) => left, (left, right) => left.CreateUID == right.ID);

            foreach (var item in commodities)
            {
                Console.WriteLine(item.Name);
            }
        }
    }
}
