using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using easyfft.Core.Services;
using MathNet.Numerics.IntegralTransforms;

namespace easyfft.Core.ViewModels
{
    public class MainViewModel : MvvmCross.Core.ViewModels.MvxViewModel
    {
        IAudioStream _audio;
        const int _bufferSize = 0x1000;
        const int _sampleRate = 44140;
        public MainViewModel(IAudioStream audio)
        {
            _audio = audio;
            
        }
        public override void ViewAppeared()
        {
            base.ViewAppeared();
            try
            {
                _audio.Init(_sampleRate, _bufferSize);
                _audio.OnBroadcast += MicrophoneBufferRecieved;
                _audio.Start();
            }
            catch (Exception e)
            {

            }
        }
        private void MicrophoneBufferRecieved(object sender, Helpers.EventArgs<float[]> e)
        {
            var buffer = e.Item;
            //FFTW.NET.FftwArrayDouble fftwArrayDouble = buffer.
            Fourier.ForwardReal(buffer, _bufferSize);
        }

        string _hello = "Hello";
        public object Hello
        {
            get => _hello;
            set
            {
                _hello = value.ToString();
                RaisePropertyChanged(nameof(Hello));
            }
        }
    }
}
