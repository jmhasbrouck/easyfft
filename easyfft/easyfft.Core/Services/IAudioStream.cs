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
using System;
using easyfft.Core.Helpers;
namespace easyfft.Core.Services
{
    /// <summary>
    /// The AudioStream interface.
    /// </summary>
    public interface IAudioStream 
    {
        /// <summary>
        /// Occurs when new audio has been streamed.
        /// </summary>
        event EventHandler<EventArgs<float[]>> OnBroadcast;

        /// <summary>
        /// Gets the sample rate.
        /// </summary>
        /// <value>
        /// The sample rate.
        /// </value>
        int SampleRate { get; }

        /// <summary>
        /// Gets the channel count.
        /// </summary>
        /// <value>
        /// The channel count.
        /// </value>
        int ChannelCount { get; }

        /// <summary>
        /// Gets bits per sample.
        /// </summary>
        int BitsPerSample { get; }
        /// <summary>
        /// Initialize the audio connection
        /// </summary>
        void Init(int sampleRate, int bufferSize);
        /// <summary>
        /// Start Broadcasting bytes comming from microphone
        /// </summary>
        bool Start();
        /// <summary>
        /// Stop Broadcasting bytes comming from microphone
        /// </summary>
        void Stop();
    }
}