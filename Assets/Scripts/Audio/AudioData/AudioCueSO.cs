using System;
using UnityEngine;


namespace Audio
{
	/// <summary>
	/// ���в��ŵ���Ƶ�������ϣ���֧���������
	/// </summary>
	[CreateAssetMenu(fileName = "newAudioCue", menuName = "Audio/Audio Cue")]
	public class AudioCueSO : ScriptableObject
	{
		/// <summary>
		/// �Ƿ�ѭ��
		/// </summary>
		public bool looping = false;
		
		/// <summary>
		/// �洢��ƵƬ����
		/// </summary>
		[SerializeField] private AudioClipsGroup[] _audioClipGroups = default;

		/// <summary>
		/// �� _audioClipGroups �����е�ÿ�� AudioClipsGroup ʵ������ȡһ����ƵƬ�Σ�������ЩƬ�δ洢��һ�� AudioClip �����з���
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
	/// ��һ�����л����࣬���ڱ�ʾһ����ƵƬ�Σ��ṩ�������˳�򲥷Ź���
	/// </summary>
	[Serializable]
	public class AudioClipsGroup
	{
		/// <summary>
		/// ��ʾ AudioClip �Ĳ���ģʽ
		/// </summary>
		public enum SequenceMode
		{
			Random,						// ��ȫ�������
			RandomNoImmediateRepeat,	// ������ţ����������ظ���һ�����ŵ�Ƭ��
			Sequential,					// ˳�򲥷�
		}
		public SequenceMode sequenceMode = SequenceMode.RandomNoImmediateRepeat;
		/// <summary>
		/// �洢AudioClip������
		/// </summary>
		public AudioClip[] audioClips;

		private int _nextClipToPlay = -1;		// ��һ��Ҫ���ŵ� AudioClip ������
		private int _lastClipPlayed = -1;		// ��һ�����ŵ� AudioClip ������

		/// <summary>
		/// ���� sequenceMode ѡ����һ����ƵƬ�β�����
		/// </summary>
		/// <returns>A reference to an AudioClip</returns>
		public AudioClip GetNextClip()
		{
			// ���ֻ��һ�� AudioClip ����ֱ�ӷ��ظ�Ƭ��
			if (audioClips.Length == 1)
				return audioClips[0];

			if (_nextClipToPlay == -1)
			{
				// ��ʼ�� _nextClipToPlay�������˳�򲥷���� 0 ��ʼ�������������������ƵƬ�����������ѡ��һ������
				_nextClipToPlay = (sequenceMode == SequenceMode.Sequential) ? 0 : UnityEngine.Random.Range(0, audioClips.Length);
			}
			else
			{
				// ���� sequenceMode ѡ����һ��Ƭ�ε�����
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
			
			_lastClipPlayed = _nextClipToPlay;		// ���� _lastClipPlayed

			return audioClips[_nextClipToPlay];		// ������ѡ������ƵƬ��
		}
	}
}
