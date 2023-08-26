using FundealSlack;

namespace FundealSlackTest;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var parse = ParseHelper.ParseArguments("'뭐 먹을까' '김밥' '떡볶이'");
        Assert.That(new List<string> { "뭐 먹을까", "김밥", "떡볶이" }, Is.EqualTo(parse));
    }

    [Test]
    public void Test2()
    {
        var parse = ParseHelper.ParseArguments("'뭐 먹을까' '김밥 떡볶이");
        Assert.That(new List<string> { "뭐 먹을까", "김밥 떡볶이"}, Is.EqualTo(parse));
    }

    [Test]
    public void Test3()
    {
        var parse = ParseHelper.ParseArguments("'뭐 먹을까' '김밥' '떡볶이' ''");
        Assert.That(new List<string> { "뭐 먹을까", "김밥", "떡볶이", ""}, Is.EqualTo(parse));
    }
}