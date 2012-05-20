using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleApp.Entities;

namespace ConsoleApp
{
    internal class Tests
    {
        private const string CONNECTION_STRING = "DefaultEndpointsProtocol=http;AccountName=storagespf;AccountKey=0FsVToOeZ1tDGhxuESR8oZr+BUNtYxk7Uvffmd9U2g24hsJ9e5qj95WyT/OmIb7AXdfjrlSvPv7PUDYXgiVdHA==";

        static void Separator()
        {
            Console.WriteLine("-------------------------------");
        }

        public static void TestingTables1()
        {
            TableUtilities tableUtil = new TableUtilities(CONNECTION_STRING);
            List<string> tables;
            Console.WriteLine("List tables ");
            if (tableUtil.ListTables(out tables))
            {
                Console.WriteLine("true");
                if (tables != null)
                {
                    foreach (string tableName in tables)
                    {
                        Console.Write(tableName + " ");
                    }
                    Console.WriteLine();
                }
            }
            else
                Console.WriteLine("false");
            Separator();
            Console.WriteLine("Create table ");
            if (tableUtil.CreateTable("sampletable"))
                Console.WriteLine("true");
            else
                Console.WriteLine("false");
            Separator();
        }

        public static void TestingTables2()
        {
            TableUtilities tableUtil = new TableUtilities(CONNECTION_STRING);

            Console.WriteLine("Insert entity ");
            if (tableUtil.InsertEntity("sampletable",
            new Contact("USA", "Pallmann")
            {
                LastName = "Pallmann",
                FirstName = "David",
                Email = "dpallmann@hotmail.com",
                Country = "USA"
            }))
                Console.WriteLine("true");
            else
                Console.WriteLine("false");
            Separator();
            Console.WriteLine("Insert entity ");
            if (tableUtil.InsertEntity("sampletable",
            new Contact("USA", "Smith")
            {
                LastName = "Smith",
                FirstName = "John",
                Email =
                    "john.smith@hotmail.com",
                Country = "USA"
            }))
                Console.WriteLine("true");
            else
                Console.WriteLine("false");
            Separator();
            Console.Write("Insert entity ");
            if (tableUtil.InsertEntity("sampletable", new Contact("USA", "Jones")
            {
                LastName = "Jones",
                FirstName = "Tom",
                Email =
                    "tom.jones@hotmail.com",
                Country = "USA"
            }))
                Console.WriteLine("true");
            else
                Console.WriteLine("false");
            Separator();
            Console.Write("Insert entity ");
            if (tableUtil.InsertEntity("sampletable",
            new Contact("USA", "Peters")
            {
                LastName = "Peters",
                FirstName = "Sally",
                Email =
                    "sally.peters@hotmail.com",
                Country = "USA"
            }))
                Console.WriteLine("true");
            else
                Console.WriteLine("false");
            Separator();
        }

        public static void TestingTables3()
        {
            TableUtilities tableUtil = new TableUtilities(CONNECTION_STRING);

            Console.Write("Replace Update entity ");
            if (tableUtil.ReplaceUpdateEntity("sampletable", "USA", "Pallmann",
            new Contact("USA", "Pallmann")
            {
                LastName = "Pallmann",
                FirstName = "David",
                Email = "david.pallmann@hotmail.com",
                Country = "USA"
            }))
                Console.WriteLine("true");
            else
                Console.WriteLine("false");
            Separator();
            Console.Write("Replace Update entity ");
            if (tableUtil.ReplaceUpdateEntity("sampletable", "USA", "Peters",
            new Contact("USA", "Peters")
            {
                LastName = "Peters",
                FirstName = "Sally",
                Country = "USA"
            }))
                Console.WriteLine("true");
            else
                Console.WriteLine("false");
            Separator();
        }

        public static void TestingTables4()
        {
            TableUtilities tableUtil = new TableUtilities(CONNECTION_STRING);

            Console.Write("Merge Update entity. Preserves the unchanged properties. ");
            if (tableUtil.MergeUpdateEntity("sampletable", "USA", "Peters",
            new MiniContact("USA", "Peters")
            {
                LastName = "Peters",
                FirstName = "Sally",
                Email = "sally.peters@hotmail.com"
            }))
                Console.WriteLine("true");
            else
                Console.WriteLine("false");
        }
    }
}
