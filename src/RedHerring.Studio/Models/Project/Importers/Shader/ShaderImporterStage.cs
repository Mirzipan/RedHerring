namespace RedHerring.Studio;

// Note: name of stages are used as input to shader compiler! Don't rename without checking it first!
public enum ShaderImporterStage
{
	vertex      = 0,
	fragment    = 1,
	tesscontrol = 2,
	tesseval    = 3,
	geometry    = 4,
	compute     = 5,
}