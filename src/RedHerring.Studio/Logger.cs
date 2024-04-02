using System.Runtime.InteropServices;
using System.Text;

namespace RedHerring.Studio;

public static class Logger
{
	public static void SystemInfo()
	{
		var sb = new StringBuilder();
		sb.AppendLine("--- System Information Begin ---");
		sb.Append("OS Description: ");
		sb.AppendLine(RuntimeInformation.OSDescription);
		sb.Append("OS Architecture: ");
		sb.AppendLine(RuntimeInformation.OSArchitecture.ToString());
		sb.Append("Process Architecture: ");
		sb.AppendLine(RuntimeInformation.ProcessArchitecture.ToString());
		sb.Append("Framework Description: ");
		sb.AppendLine(RuntimeInformation.FrameworkDescription);
		sb.Append("Runtime Identifier: ");
		sb.AppendLine(RuntimeInformation.RuntimeIdentifier);
		sb.AppendLine("--- System Information End ---");
		
		Console.WriteLine(sb.ToString());
	}
}