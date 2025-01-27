using UnityEngine;

namespace Rhythem.Util
{
public class SoundRequirements
{
	public string name;
	public int samples;
	public int channels;
	public FMOD.SOUND_FORMAT format;
	public int defaultFrequency;
	public float[] sampleData;

	/// <summary>
	/// 
	/// </summary>
	/// <param name="name"></param>
	/// <param name="samples"></param>
	/// <param name="channel"></param>
	/// <param name="format"></param>
	/// <param name="defaultFrequency"></param>
	/// <param name="sampleData"></param>
	public SoundRequirements(string name, int samples, int channel, FMOD.SOUND_FORMAT format, int defaultFrequency, float[] sampleData)
	{
		this.name = name;
		this.samples = samples;
		this.channels = channel;
		this.format = format;
		this.defaultFrequency = defaultFrequency;
		this.sampleData = sampleData;
	}
}
}

