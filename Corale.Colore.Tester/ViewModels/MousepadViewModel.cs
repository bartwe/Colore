// ---------------------------------------------------------------------------------------
// <copyright file="MousepadViewModel.cs" company="Corale">
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
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    using Corale.Colore.Razer.Mousepad.Effects;
    using Corale.Colore.Tester.Classes;
    using Corale.Colore.Wpf;

    public class MousepadViewModel : INotifyPropertyChanged {
        Direction _selectedWaveDirection;

        public MousepadViewModel() {
            SelectedWaveDirection = Direction.LeftToRight;
            ColorOne = new SolidColorBrush();
            ColorTwo = new SolidColorBrush();
            ColorOne.Color = Core.Color.Red.ToWpfColor();
            ColorTwo.Color = Core.Color.Blue.ToWpfColor();
        }

        public int Index { get; set; }

        public SolidColorBrush ColorOne { get; set; }

        public SolidColorBrush ColorTwo { get; set; }

        public Direction SelectedWaveDirection {
            get {
                return _selectedWaveDirection;
            }

            set {
                _selectedWaveDirection = value;
                OnPropertyChanged("SelectedWaveDirection");
            }
        }

        public ICommand AllCommand {
            get {
                return new DelegateCommand(() => Core.Mousepad.Instance.SetAll(ColorOne.Color.ToColoreColor()));
            }
        }

        public ICommand BreathingCommand {
            get {
                return
                    new DelegateCommand(
                        () =>
                            Core.Mousepad.Instance.SetBreathing(
                                ColorOne.Color.ToColoreColor(),
                                ColorTwo.Color.ToColoreColor()));
            }
        }

        public ICommand WaveCommand {
            get {
                return new DelegateCommand(SetWaveEffect);
            }
        }

        public ICommand StaticCommand {
            get {
                return new DelegateCommand(() => Core.Mousepad.Instance.SetStatic(ColorOne.Color.ToColoreColor()));
            }
        }

        public ICommand IndexerCommand {
            get {
                return new DelegateCommand(SetIndexerEffect);
            }
        }

        public ICommand ClearCommand {
            get {
                return new DelegateCommand(() => Core.Mousepad.Instance.Clear());
            }
        }

        public IEnumerable<Razer.Mouse.Led> LedValues {
            get {
                return Enum.GetValues(typeof(Razer.Mouse.Led)).Cast<Razer.Mouse.Led>();
            }
        }

        public IEnumerable<Direction> WaveDirectionValues {
            get {
                return Enum.GetValues(typeof(Direction)).Cast<Direction>();
            }
        }

        public string Connected {
            get {
                return "Connected: " + Core.Mousepad.Instance.Connected.ToString();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [Annotations.NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName) {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void SetWaveEffect() {
            try {
                Core.Mousepad.Instance.SetWave(SelectedWaveDirection);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }

        void SetIndexerEffect() {
            try {
                Core.Mousepad.Instance[Index] = ColorOne.Color.ToColoreColor();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
