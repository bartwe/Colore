﻿// ---------------------------------------------------------------------------------------
// <copyright file="HeadsetViewModel.cs" company="Corale">
//     Copyright © 2015-2016 by Adam Hellberg and Brandon Scott.
// 
//     Permission is hereby granted, free of charge, to any person obtaining a copy of
//     this software and associated documentation files (the "Software"), to deal in
//     the Software without restriction, including without limitation the rights to
//     use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
//     of the Software, and to permit persons to whom the Software is furnished to do
//     so, subject to the following conditions:
// 
//     The above copyright notice and this permission notice shall be included in all
//     copies or substantial portions of the Software.
// 
//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//     AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
//     WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//     CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
//     "Razer" is a trademark of Razer USA Ltd.
// </copyright>
// ---------------------------------------------------------------------------------------

namespace Corale.Colore.Tester.ViewModels {
    using System.ComponentModel;
    using System.Windows.Input;
    using System.Windows.Media;

    using Corale.Colore.Annotations;
    using Corale.Colore.Tester.Classes;
    using Corale.Colore.Wpf;

    public class HeadsetViewModel : INotifyPropertyChanged {
        public HeadsetViewModel() {
            ColorOne = new SolidColorBrush();
            ColorOne.Color = Core.Color.Red.ToWpfColor();
        }

        public SolidColorBrush ColorOne { get; set; }

        public ICommand AllCommand {
            get {
                return
                    new DelegateCommand(() => Core.Headset.Instance.SetAll(ColorOne.Color.ToColoreColor()));
            }
        }

        public ICommand BreathingCommand {
            get {
                return new DelegateCommand(() => Core.Headset.Instance.SetBreathing(ColorOne.Color.ToColoreColor()));
            }
        }

        public ICommand StaticCommand {
            get {
                return new DelegateCommand(() => Core.Headset.Instance.SetStatic(ColorOne.Color.ToColoreColor()));
            }
        }

        public ICommand SpectrumCommand {
            get {
                return new DelegateCommand(() => Core.Headset.Instance.SetSpectrum());
            }
        }

        public ICommand ClearCommand {
            get {
                return new DelegateCommand(() => Core.Headset.Instance.Clear());
            }
        }

        public string Connected {
            get {
                return "Connected: " + Core.Headset.Instance.Connected.ToString();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName) {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
