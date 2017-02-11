using System;
using System.IO;

public class CommandController
{
	private string[] Commands;
	private int Pointer;

	private string[] CachedCommand;
	private int CachedIndex;

	public CommandController(string fileName)
	{
		Commands = File.ReadAllLines(fileName);
		Pointer = 0;
		CachedIndex = -1;
	}

	private string[] Take(int index)
	{
		if (CachedIndex != index)
		{
			CachedIndex = index;
			CachedCommand = Commands[index].Split(' ');
		}

		return CachedCommand;
	}

	public string[] Top()
	{
		return Take(Pointer);
	}

	public string[] Pop()
	{
		Pointer++;
		return Take(Pointer - 1);
	}

	public bool IsEnd()
	{
		return Pointer == Commands.Length;
	}
}
