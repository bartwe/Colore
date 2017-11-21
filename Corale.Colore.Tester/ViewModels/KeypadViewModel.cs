// ---------------------------------------------------------------------------------------
// <copyright file="KeypadViewModel.cs" company="Corale">
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

    using Corale.Colore.Razer.Keypad.Effects;
    using Corale.Colore.Razer.Mouse;
    using Corale.Colore.Tester.Classes;
    using Corale.Colore.Wpf;

    using Duration = Corale.Colore.Razer.Keypad.Effects.Duration;
    using Key = Corale.Colore.Razer.Keyboard.Key;

    public class KeypadViewModel : INotifyPropertyChanged {
        Key _selectedKey;
        Duration _selectedReactiveDuration;
        Direction _selectedWaveDirection;

        public KeypadViewModel() {
            SelectedKey = Key.A;
            SelectedReactiveDuration = Duration.Long;
            SelectedWaveDirection = Direction.LeftToRight;
            ColorOne = new SolidColorBrush();
            ColorTwo = new SolidColorBrush();
            ColorOne.Color = Core.Color.Red.ToWpfColor();
            ColorTwo.Color = Core.Color.Blue.ToWpfColor();
        }

        public int Col { get; set; }

        public int Row { get; set; }

        public SolidColorBrush ColorOne { get; set; }

        public SolidColorBrush ColorTwo { get; set; }

        public Led Keys { get; set; }

        public Key SelectedKey {
            get {
                return _selectedKey;
            }

            set {
                _selectedKey = value;
                OnPropertyChanged("SelectedKey");
            }
        }

        public Duration SelectedReactiveDuration {
            get {
                return _selectedReactiveDuration;
            }

            set {
                _selectedReactiveDuration = value;
                OnPropertyChanged("SelectedReactiveDuration");
            }
        }

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
                return new DelegateCommand(() => Core.Keypad.Instance.SetAll(ColorOne.Color.ToColoreColor()));
            }
        }

        public ICommand BreathingCommand {
            get {
                return
                    new DelegateCommand(
                        () =>
                            Core.Keypad.Instance.SetBreathing(
                                ColorOne.Color.ToColoreColor(),
                                ColorTwo.Color.ToColoreColor()))
                    ;
            }
        }

        public ICommand ReactiveCommand {
            get {
                return
                    new DelegateCommand(SetReactiveEffect);
            }
        }

        public ICommand WaveCommand {
            get {
                return new DelegateCommand(SetWaveEffect);
            }
        }

        public ICommand StaticCommand {
            get {
                return new DelegateCommand(() => Core.Keypad.Instance.SetStatic(new Static(ColorOne.Color.ToColoreColor())));
            }
        }

        public ICommand IndexerCommand {
            get {
                return new DelegateCommand(SetIndexerEffect);
            }
        }

        public IEnumerable<Direction> WaveDirectionValues {
            get {
                return Enum.GetValues(typeof(Direction)).Cast<Direction>();
            }
        }

        public IEnumerable<Duration> ReactiveDurationValues {
            get {
                return Enum.GetValues(typeof(Duration)).Cast<Duration>();
            }
        }

        public ICommand ClearCommand {
            get {
                return new DelegateCommand(() => Core.Keypad.Instance.Clear());
            }
        }

        public string Connected {
            get {
                return "Connected: " + Core.Keypad.Instance.Connected.ToString();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [Annotations.NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName) {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void SetReactiveEffect() {
            try {
                Core.Keypad.Instance.SetReactive(ColorOne.Color.ToColoreColor(), SelectedReactiveDuration);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }

        void SetWaveEffect() {
            try {
                Core.Keypad.Instance.SetWave(SelectedWaveDirection);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }

        void SetIndexerEffect() {
            try {
                Core.Keypad.Instance[Row, Col] = ColorOne.Color.ToColoreColor();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
