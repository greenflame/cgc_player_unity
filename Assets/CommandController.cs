using System;
using System.IO;
using System.Globalization;

public class CommandController
{
	private string[] Commands;
	private int Pointer;

	private int CachedIndex;
	private string[] CachedCommand;
	private float CachedTime;

	public CommandController(string fileName)
	{
		Commands = File.ReadAllLines(fileName);
		Pointer = 0;
		CachedIndex = -1;
	}

	private void UpdateCache(int index)
	{
		if (CachedIndex != index)
		{
			CachedIndex = index;
			CachedCommand = Commands[index].Split(' ');
			CachedTime = float.Parse(CachedCommand[1], CultureInfo.InvariantCulture.NumberFormat);
		}
	}

	public string[] TopCommand()
	{
		UpdateCache(Pointer);
		return CachedCommand;
	}

	public float TopTime()
	{
		UpdateCache(Pointer);
		return CachedTime;
	}

	public string[] PopCommand()
	{
		UpdateCache(Pointer);
		string[] command = TopCommand();
		Pointer++;
		return command;
	}

	public bool IsEnd()
	{
		return Pointer == Commands.Length;
	}
}
