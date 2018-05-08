//
//  Copyright 2013, Sami M. Kallio
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
using easyfft.Core.Helpers;
using easyfft.Core.Services;
using Foundation;
using System;
using AudioToolbox;

namespace easyfft.iOS.Services
{

    public class AudioStream_iOS : IAudioStream
    {
        private InputAudioQueue audioQueue;

        private int bufferSize;

        #region IAudioStream implementation

        public event EventHandler<EventArgs<byte[]>> OnBroadcast;

        public int AverageBytesPerSecond
        {
            get
            {
                throw new NotImplementedException ();
            }
        }

        public int SampleRate
        {
            get;
            private set;
        }

        public int ChannelCount
        {
            get
            {
                return 1;
            }
        }

        public int BitsPerSample
        {
            get
            {
                return 8 * sizeof(float);
            }
        }

        #endregion

        #region IMonitor implementation

        public event EventHandler<EventArgs<bool>> OnActiveChanged;

        public event EventHandler<EventArgs<Exception>> OnException;

        public bool Start ()
        {
            return (this.audioQueue.Start() == AudioQueueStatus.Ok);
        }

        public void Stop ()
        {
            this.audioQueue.Stop (true);
        }

        public bool Active
        {
            get
            {
                return this.audioQueue.IsRunning;
            }
        }

        #endregion

        public AudioStream_iOS()
        {
        }
        private void CheckMicrophonePermission()
        {
            switch(AVFoundation.AVAudioSession.SharedInstance().RecordPermission)
            {
                case AVFoundation.AVAudioSessionRecordPermission.Granted:
                    break;
                case AVFoundation.AVAudioSessionRecordPermission.Undetermined:
                case AVFoundation.AVAudioSessionRecordPermission.Denied:
                    NSRunLoop.Main.BeginInvokeOnMainThread(() =>
                    {
                        AVFoundation.AVAudioSession.SharedInstance().RequestRecordPermission((bool granted) =>
                        {
                            if (!granted)
                            {
                                Acr.UserDialogs.UserDialogs.Instance.AlertAsync("This app cannot function properly without microphone access.");
                            }
                        });
                    });
                    break;
            }
        }
        public void Init(int sampleRate, int bufferSize)
        {
            CheckMicrophonePermission();
            this.SampleRate = sampleRate;
            this.bufferSize = bufferSize;
            var audioFormat = new AudioStreamBasicDescription()
            {
                SampleRate = this.SampleRate,
                Format = AudioFormatType.LinearPCM,
                FormatFlags = AudioFormatFlags.LinearPCMIsFloat,
                FramesPerPacket = 1,
                ChannelsPerFrame = 1,
                BitsPerChannel = this.BitsPerSample,
                BytesPerPacket = 2,
                BytesPerFrame = 2,
                Reserved = 0
            };

            audioQueue = new InputAudioQueue(audioFormat);
            audioQueue.InputCompleted += QueueInputCompleted;

            var bufferByteSize = this.bufferSize * audioFormat.BytesPerPacket;

            IntPtr bufferPtr;
            for (var index = 0; index < 3; index++)
            {
                audioQueue.AllocateBufferWithPacketDescriptors(bufferByteSize, this.bufferSize, out bufferPtr);
                audioQueue.EnqueueBuffer(bufferPtr, bufferByteSize, null);
            }
        }

        /// <summary>
        /// Handles iOS audio buffer queue completed message.
        /// </summary>
        /// <param name='sender'>Sender object</param>
        /// <param name='e'> Input completed parameters.</param>
        private void QueueInputCompleted(object sender, InputCompletedEventArgs e)
        {
            // return if we aren't actively monitoring audio packets
            if (!this.Active)
            {
                return;
            }

            var buffer = (AudioQueueBuffer)System.Runtime.InteropServices.Marshal.PtrToStructure(e.IntPtrBuffer, typeof(AudioQueueBuffer));
            if (this.OnBroadcast != null)
            {
                var send = new byte[buffer.AudioDataByteSize];
                System.Runtime.InteropServices.Marshal.Copy(buffer.AudioData, send, 0, (int)buffer.AudioDataByteSize);

                this.OnBroadcast(this, new EventArgs<byte[]>(send));
            }
                               
            var status = audioQueue.EnqueueBuffer(e.IntPtrBuffer, this.bufferSize, e.PacketDescriptions);  

            if (status != AudioQueueStatus.Ok)
            {
                // todo: 
            }
        }       

    }
}