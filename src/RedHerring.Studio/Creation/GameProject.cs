using Microsoft.Build.Construction;

namespace RedHerring.Studio.Creation;

public static class GameProject
{
	public static void CreateAt(string path, string name)
	{
		string solutionPath = $"{path}/{name}.sln";
		using (var file = File.Create(solutionPath))
		{
			var solutionBytes = "Microsoft Visual Studio Solution File, Format Version 12.00"u8.ToArray().AsSpan();
			file.Write(solutionBytes);
		}
		
		SolutionFile? solution = SolutionFile.Parse(solutionPath);
		if (solution is null)
		{
			return;
		}

		var library = LibraryProject(path, $"{name}Library");
		var executable = ExecutableProject(path, name);
	}
	
	private static ProjectRootElement LibraryProject(string path, string name)
	{
		var root = ProjectRootElement.Create();

		root.AddProperty("TargetFramework", "net8.0");
		root.AddProperty("ImplicitUsings", "enable");
		root.AddProperty("Nullable", "enable");
		root.AddProperty("LangVersion", "12");
		
		root.Save($"{path}/{name}/{name}.csproj");
		return root;
	}
	
	private static ProjectRootElement ExecutableProject(string path, string name)
	{
		var root = ProjectRootElement.Create();

		root.AddProperty("OutputType", "Exe");
		root.AddProperty("TargetFramework", "net8.0");
		root.AddProperty("ImplicitUsings", "enable");
		root.AddProperty("Nullable", "enable");
		root.AddProperty("LangVersion", "12");
		
		root.AddItem("Compile", "Program.cs");
		
		root.Save($"{path}/{name}/{name}.csproj");
		return root;
	}
}