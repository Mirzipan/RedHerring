using System.Diagnostics;
using System.Text;

namespace RedHerring.Studio;

public static class FileExecutionUtility
{
	// based on StackOverflow
	public static string ExecuteFile(string filename, string? arguments = null)
	{
		string Format(string? formatFilename, string? formatArguments)
		{
			return "'" + formatFilename + ((string.IsNullOrEmpty(formatArguments)) ? string.Empty : " " + formatArguments) + "'";
		}
		
		Process process = new();

		process.StartInfo.FileName = filename;
		if (!string.IsNullOrEmpty(arguments))
		{
			process.StartInfo.Arguments = arguments;
		}

		process.StartInfo.CreateNoWindow  = true;
		process.StartInfo.WindowStyle     = ProcessWindowStyle.Hidden;
		process.StartInfo.UseShellExecute = false;

		process.StartInfo.RedirectStandardError = true;
		process.StartInfo.RedirectStandardOutput = true;
		StringBuilder stdOutput = new();
		process.OutputDataReceived += (sender, args) => stdOutput.AppendLine(args.Data); // Use AppendLine rather than Append since args.Data is one line of output, not including the newline character.

		string? stdError;
		try
		{
			process.Start();
			process.BeginOutputReadLine();
			stdError = process.StandardError.ReadToEnd();
			process.WaitForExit();
		}
		catch (Exception e)
		{
			throw new Exception("OS error while executing " + Format(filename, arguments) + ": " + e.Message, e);
		}

		if (process.ExitCode == 0)
		{
			return stdOutput.ToString();
		}

		StringBuilder message = new ();

		if (!string.IsNullOrEmpty(stdError))
		{
			message.AppendLine(stdError);
		}

		if (stdOutput.Length != 0)
		{
			message.AppendLine("Std output:");
			message.AppendLine(stdOutput.ToString());
		}

		throw new Exception(Format(filename, arguments) + " finished with exit code = " + process.ExitCode + ": " + message);
	}
}