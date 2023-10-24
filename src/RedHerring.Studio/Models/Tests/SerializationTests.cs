using OdinSerializer;

namespace RedHerring.Studio.Models.Tests;

[Serializable]
public class TestClassA
{
	public int    IntValue;
	public string StringValue = string.Empty;

	public virtual TestClassA Init()
	{
		IntValue    = 2;
		StringValue = "abc";
		return this;
	}
}

[Serializable]
public class TestClassB : TestClassA
{
	public float FloatValue;

	public override TestClassA Init()
	{
		FloatValue = 3;
		return base.Init();
	}
}

[Serializable]
public class TestClassC
{
	public List<TestClassA> TestList = new();
	public int              IntValue;

	public void Init()
	{
		TestList.Add(new TestClassA().Init());
		TestList.Add(new TestClassB().Init());
		TestList.Add(new TestClassA().Init());
		TestList.Add(new TestClassB().Init());
		IntValue = 5;
	}
}

public class SerializationTests
{
	public static void Test()
	{
		TestClassC t_in = new();
		t_in.Init();

		byte[] json = SerializationUtility.SerializeValue(t_in, DataFormat.JSON);
		string json_str = System.Text.Encoding.UTF8.GetString(json);
		Console.WriteLine(json_str);

		TestClassC t_out = SerializationUtility.DeserializeValue<TestClassC>(json, DataFormat.JSON);

		Console.WriteLine("Done");
	}
}
