using System;
using System.Linq;

namespace everyone
{
    public static class CommandLineArgumentTests
    {
        public static void Test(TestRunner runner)
        {
            runner.TestGroup(typeof(CommandLineArgument), () =>
            {
                runner.TestGroup("Create(string,string)", () =>
                {
                    void CreateErrorTest(string namePrefix, string name, string nameValueSeparator, string value, Exception expected)
                    {
                        runner.Test($"with {Language.AndList(new[] { namePrefix, name, nameValueSeparator, value }.Select(runner.ToString))}", (Test test) =>
                        {
                            test.AssertThrows(expected, () =>
                            {
                                CommandLineArgument.Create(namePrefix, name, nameValueSeparator, value);
                            });
                        });
                    }

                    CreateErrorTest(null!, "a", "=", "b", new ArgumentNullException("namePrefix"));
                    CreateErrorTest("", null!, "", "fake-value", new ArgumentNullException("name"));
                    CreateErrorTest("-", "fake-name", null!, "", new ArgumentNullException("nameValueSeparator"));
                    CreateErrorTest("-", "fake-name", "", null!, new ArgumentNullException("value"));
                    CreateErrorTest("", "fake-name", "", "", new ArgumentException("namePrefix cannot be empty if name is not empty."));
                    CreateErrorTest("", "", "", "", new ArgumentException("name or value must be non-empty."));
                    CreateErrorTest("-", "", "", "", new ArgumentException("name cannot be empty if namePrefix is not empty."));
                    CreateErrorTest("-", "", "=", "", new ArgumentException("name cannot be empty if namePrefix is not empty."));
                    CreateErrorTest("-", "", "=", "b", new ArgumentException("name cannot be empty if namePrefix is not empty."));
                    CreateErrorTest("", "", " ", "", new ArgumentException("name cannot be empty if nameValueSeparator is not empty."));
                    CreateErrorTest("-", "a", "", "b", new ArgumentException("nameValueSeparator cannot be empty if name and value are not empty."));

                    void CreateTest(string namePrefix, string name, string nameValueSeparator, string value)
                    {
                        runner.Test($"with {Language.AndList(new[] { namePrefix, name, nameValueSeparator, value }.Select(runner.ToString))}", (Test test) =>
                        {
                            CommandLineArgument arg = CommandLineArgument.Create(namePrefix, name, nameValueSeparator, value);
                            test.AssertNotNull(arg);
                            test.AssertEqual(namePrefix, arg.NamePrefix);
                            test.AssertEqual(name, arg.Name);
                            test.AssertEqual(nameValueSeparator, arg.NameValueSeparator);
                            test.AssertEqual(value, arg.Value);
                        });
                    }

                    CreateTest("-", "a", "", "");
                    CreateTest("--", "a", "", "");
                    CreateTest("--", "a", "=", "");
                    CreateTest("", "", "", "b");
                    CreateTest("-", "a", "=", "b");
                    CreateTest("--", "a", " ", "b");
                });

                runner.TestGroup("ToString()", () =>
                {
                    void ToStringTest(CommandLineArgument arg, string expected)
                    {
                        runner.Test($"with {arg}", (Test test) =>
                        {
                            test.AssertEqual(expected, arg.ToString());
                        });
                    }

                    ToStringTest(CommandLineArgument.Create("-", "a", "", ""), "-a");
                    ToStringTest(CommandLineArgument.Create("--", "a", "", ""), "--a");
                    ToStringTest(CommandLineArgument.Create("--", "a", "=", ""), "--a");
                    ToStringTest(CommandLineArgument.Create("", "", "", "b"), "b");
                    ToStringTest(CommandLineArgument.Create("-", "a", "=", "b"), "-a=b");
                    ToStringTest(CommandLineArgument.Create("-", "a", " ", "b"), "-a b");
                    ToStringTest(CommandLineArgument.Create("--", "a", "=", "b"), "--a=b");
                });

                runner.TestGroup("GetHashCode()", () =>
                {
                    void GetHashCodeTest(CommandLineArgument lhs, CommandLineArgument rhs, bool expected)
                    {
                        runner.Test($"with {Language.AndList(lhs, rhs)}", (Test test) =>
                        {
                            test.AssertEqual(expected, lhs.GetHashCode() == rhs.GetHashCode(), $"Expected the hash codes to {(expected ? "" : "not ")}be equal.");
                        });
                    }

                    GetHashCodeTest(
                        CommandLineArgument.Create("a", "b", "c", "d"),
                        CommandLineArgument.Create("a", "b", "c", "d"),
                        true);
                    GetHashCodeTest(
                        CommandLineArgument.Create("a", "b", "c", "d"),
                        CommandLineArgument.Create("aa", "b", "c", "d"),
                        false);
                    GetHashCodeTest(
                        CommandLineArgument.Create("a", "b", "c", "d"),
                        CommandLineArgument.Create("a", "bb", "c", "d"),
                        false);
                    GetHashCodeTest(
                        CommandLineArgument.Create("a", "b", "c", "d"),
                        CommandLineArgument.Create("a", "b", "cc", "d"),
                        false);
                    GetHashCodeTest(
                        CommandLineArgument.Create("a", "b", "c", "d"),
                        CommandLineArgument.Create("a", "b", "c", "dd"),
                        false);
                });

                runner.TestGroup("Equals(object?)", () =>
                {
                    void EqualsTest(CommandLineArgument lhs, object? rhs, bool expected)
                    {
                        runner.Test($"with {Language.AndList(lhs, rhs)}", (Test test) =>
                        {
                            test.AssertEqual(expected, lhs.Equals(rhs));
                        });
                    }

                    EqualsTest(
                        CommandLineArgument.Create("a", "b", "c", "d"),
                        null,
                        false);
                    EqualsTest(
                        CommandLineArgument.Create("a", "b", "c", "d"),
                        50,
                        false);
                    EqualsTest(
                        CommandLineArgument.Create("a", "b", "c", "d"),
                        CommandLineArgument.Create("a", "b", "c", "d"),
                        true);
                    EqualsTest(
                        CommandLineArgument.Create("a", "b", "c", "d"),
                        CommandLineArgument.Create("aa", "b", "c", "d"),
                        false);
                    EqualsTest(
                        CommandLineArgument.Create("a", "b", "c", "d"),
                        CommandLineArgument.Create("a", "bb", "c", "d"),
                        false);
                    EqualsTest(
                        CommandLineArgument.Create("a", "b", "c", "d"),
                        CommandLineArgument.Create("a", "b", "cc", "d"),
                        false);
                    EqualsTest(
                        CommandLineArgument.Create("a", "b", "c", "d"),
                        CommandLineArgument.Create("a", "b", "c", "dd"),
                        false);
                });
            });
        }
    }
}
