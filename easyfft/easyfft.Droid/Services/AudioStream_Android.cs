using System;
using easyfft.Core.Services;
using easyfft.Core.Helpers;
using Android.Media;
using System.Threading.Tasks;

namespace easyfft.Droid.Services
{
    public class AudioStream_Android : IAudioStream
    {
        private int bufferSize;

        /// <summary>
        /// The audio source.
        /// </summary>
        private AudioRecord audioSource;

        /// <summary>
        /// Occurs when new audio has been streamed.
        /// </summary>
        public event EventHandler<EventArgs<float[]>> OnBroadcast;

        /// <summary>
        /// The default device.
        /// </summary>
        public static AudioSource DefaultDevice = AudioSource.Mic;

        /// <summary>
        /// Gets the sample rate.
        /// </summary>
        /// <value>
        /// The sample rate.
        /// </value>
        public int SampleRate
        {
            get
            {
                return this.audioSource.SampleRate;
            }
        }

        /// <summary>
        /// Gets bits per sample.
        /// </summary>
        public int BitsPerSample
        {
            get
            {
                return (this.audioSource.AudioFormat == Encoding.PcmFloat) ? 32 : 8;
            }
        }

        /// <summary>
        /// Gets the channel count.
        /// </summary>
        /// <value>
        /// The channel count.
        /// </value>        
        public int ChannelCount
        {
            get
            {
                return this.audioSource.ChannelCount;
            }
        }

        /// <summary>
        /// Gets the average data transfer rate
        /// </summary>
        /// <value>The average data transfer rate in bytes per second.</value>
        public int AverageBytesPerSecond
        {
            get
            {
                return this.SampleRate * this.BitsPerSample / 8 * this.ChannelCount;
            }
        }

        public bool Active
        {
            get
            {
                return (this.audioSource.RecordingState == RecordState.Recording);
            }
        }

        /// <summary>
        /// Start recording from the hardware audio source.
        /// </summary>
        public bool Start()
        {
            Android.OS.Process.SetThreadPriority(Android.OS.ThreadPriority.UrgentAudio);

            if (this.Active)
            {
                return this.Active;
            }

            this.audioSource.StartRecording();
            _recording = true;

            Record();
            return this.Active;
        }

        /// <summary>
        /// Stops recording.
        /// </summary>
        public void Stop()
        {
            this.audioSource.Stop();
            _recording = false;
        }
        private bool _recording = false;
        private async void Record()
        {
            await Task.Run(() =>
            {
                float[] buffer = new float[bufferSize * sizeof(float)];
                while (_recording)
                {
                    audioSource.Read(buffer, 0, sizeof(float), 0);
                    OnBroadcast(this, new EventArgs<float[]>(buffer));
                }
            });
        }

        public void Init(int sampleRate, int bufferSize)
        {
            this.bufferSize = bufferSize;
            this.audioSource = new AudioRecord(
                AudioStream_Android.DefaultDevice,
                sampleRate,
                ChannelIn.Mono,
                Encoding.PcmFloat,
                this.bufferSize);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimplyMobile.Media.AudioStream"/> class.
        /// </summary>
        /// <param name="sampleRate">Sample rate.</param>
        /// <param name="bufferSize">Buffer size.</param>
        public AudioStream_Android()
        {
            
        }


        public event EventHandler<EventArgs<bool>> OnActiveChanged;

        public event EventHandler<EventArgs<Exception>> OnException;
    }
}
