using System;
using UnityEngine;


namespace Audio
{
	/// <summary>
	/// 并行播放的音频剪辑集合，并支持随机化。
	/// </summary>
	[CreateAssetMenu(fileName = "newAudioCue", menuName = "Audio/Audio Cue")]
	public class AudioCueSO : ScriptableObject
	{
		/// <summary>
		/// 是否循环
		/// </summary>
		public bool looping = false;
		
		/// <summary>
		/// 存储音频片段组
		/// </summary>
		[SerializeField] private AudioClipsGroup[] _audioClipGroups = default;

		/// <summary>
		/// 从 _audioClipGroups 数组中的每个 AudioClipsGroup 实例中提取一个音频片段，并将这些片段存储在一个 AudioClip 数组中返回
		/// </summary>
		/// <returns></returns>
		public AudioClip[] GetClips()
		{
			int numberOfClips = _audioClipGroups.Length;
			AudioClip[] resultingClips = new AudioClip[numberOfClips];

			for (int i = 0; i < numberOfClips; i++)
			{
				resultingClips[i] = _audioClipGroups[i].GetNextClip();
			}

			return resultingClips;
		}
	}


	/// <summary>
	/// 是一个序列化的类，用于表示一组音频片段，提供随机化或顺序播放功能
	/// </summary>
	[Serializable]
	public class AudioClipsGroup
	{
		/// <summary>
		/// 表示 AudioClip 的播放模式
		/// </summary>
		public enum SequenceMode
		{
			Random,						// 完全随机播放
			RandomNoImmediateRepeat,	// 随机播放，但不立即重复上一个播放的片段
			Sequential,					// 顺序播放
		}
		public SequenceMode sequenceMode = SequenceMode.RandomNoImmediateRepeat;
		/// <summary>
		/// 存储AudioClip的数组
		/// </summary>
		public AudioClip[] audioClips;

		private int _nextClipToPlay = -1;		// 下一个要播放的 AudioClip 的索引
		private int _lastClipPlayed = -1;		// 上一个播放的 AudioClip 的索引

		/// <summary>
		/// 根据 sequenceMode 选择下一个音频片段并返回
		/// </summary>
		/// <returns>A reference to an AudioClip</returns>
		public AudioClip GetNextClip()
		{
			// 如果只有一个 AudioClip ，则直接返回该片段
			if (audioClips.Length == 1)
				return audioClips[0];

			if (_nextClipToPlay == -1)
			{
				// 初始化 _nextClipToPlay，如果是顺序播放则从 0 开始；如果是随机播放则从音频片段数组中随机选择一个索引
				_nextClipToPlay = (sequenceMode == SequenceMode.Sequential) ? 0 : UnityEngine.Random.Range(0, audioClips.Length);
			}
			else
			{
				// 根据 sequenceMode 选择下一个片段的索引
				switch (sequenceMode)
				{
					case SequenceMode.Random:
						_nextClipToPlay = UnityEngine.Random.Range(0, audioClips.Length);
						break;

					case SequenceMode.RandomNoImmediateRepeat:
						do
						{
							_nextClipToPlay = UnityEngine.Random.Range(0, audioClips.Length);
						} while (_nextClipToPlay == _lastClipPlayed);
						break;

					case SequenceMode.Sequential:
						_nextClipToPlay = (int)Mathf.Repeat(++_nextClipToPlay, audioClips.Length);
						break;
				}
			}
			
			_lastClipPlayed = _nextClipToPlay;		// 更新 _lastClipPlayed

			return audioClips[_nextClipToPlay];		// 并返回选定的音频片段
		}
	}
}
