using System;

namespace AutoLead
{
	[Flags]
	public enum Win32ProcessAccessType
	{
		Terminate = 1,
		CreateThread = 2,
		VMOperation = 8,
		VMRead = 16,
		VMWrite = 32,
		DuplicateHandle = 64,
		SetInformation = 512,
		QueryInformation = 1024,
		Synchronize = 1048576,
		AllAccess = 1050235
	}
}