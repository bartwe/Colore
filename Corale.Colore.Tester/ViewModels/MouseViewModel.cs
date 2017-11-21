// ---------------------------------------------------------------------------------------
// <copyright file="MouseViewModel.cs" company="Corale">
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

    using Corale.Colore.Razer.Mouse;
    using Corale.Colore.Razer.Mouse.Effects;
    using Corale.Colore.Tester.Classes;
    using Corale.Colore.Wpf;

    using Duration = Corale.Colore.Razer.Mouse.Effects.Duration;

    public class MouseViewModel : INotifyPropertyChanged {
        GridLed _selectedGridLed;
        Led _selectedLed;

        Duration _selectedReactiveDuration;

        Direction _selectedWaveDirection;

        public MouseViewModel() {
            SelectedLed = Led.All;
            SelectedGridLed = GridLed.Logo;
            SelectedReactiveDuration = Duration.Long;
            SelectedWaveDirection = Direction.FrontToBack;
            ColorOne = new SolidColorBrush();

            ColorTwo = new SolidColorBrush();
            ColorOne.Color = Core.Color.Red.ToWpfColor();
            ColorTwo.Color = Core.Color.Blue.ToWpfColor();
        }

        public int Col { get; set; }

        public int Row { get; set; }

        public SolidColorBrush ColorOne { get; set; }

        public SolidColorBrush ColorTwo { get; set; }

        public Led Leds { get; set; }

        public Led SelectedLed {
            get {
                return _selectedLed;
            }

            set {
                _selectedLed = value;
                OnPropertyChanged("SelectedLed");
            }
        }

        public GridLed SelectedGridLed {
            get {
                return _selectedGridLed;
            }

            set {
                _selectedGridLed = value;
                OnPropertyChanged("SelectedGridLed");
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
                return new DelegateCommand(() => Core.Mouse.Instance.SetAll(ColorOne.Color.ToColoreColor()));
            }
        }

        public ICommand BreathingOneColorCommand {
            get {
                return new DelegateCommand(() => Core.Mouse.Instance.SetBreathing(ColorOne.Color.ToColoreColor(), SelectedLed));
            }
        }

        public ICommand BreathingTwoColorCommand {
            get {
                return
                    new DelegateCommand(
                        () =>
                            Core.Mouse.Instance.SetBreathing(ColorOne.Color.ToColoreColor(), ColorTwo.Color.ToColoreColor()))
                    ;
            }
        }

        public ICommand BreathingRandomColorCommand {
            get {
                return new DelegateCommand(() => Core.Mouse.Instance.SetBreathing(SelectedLed));
            }
        }

        public ICommand ReactiveCommand {
            get {
                return new DelegateCommand(SetReactiveEffect);
            }
        }

        public ICommand WaveCommand {
            get {
                return new DelegateCommand(SetWaveEffect);
            }
        }

        public ICommand StaticCommand {
            get {
                return
                    new DelegateCommand(
                        () => Core.Mouse.Instance.SetStatic(new Static(SelectedLed, ColorOne.Color.ToColoreColor())));
            }
        }

        public ICommand GridLedCommand {
            get {
                return new DelegateCommand(SetGridLedEffect);
            }
        }

        public ICommand LedCommand {
            get {
                return new DelegateCommand(SetLedEffect);
            }
        }

        public ICommand BlinkingCommand {
            get {
                return new DelegateCommand(() => Core.Mouse.Instance.SetBlinking(ColorOne.Color.ToColoreColor(), SelectedLed));
            }
        }

        public ICommand ClearCommand {
            get {
                return new DelegateCommand(() => Core.Mouse.Instance.Clear());
            }
        }

        public IEnumerable<Led> LedValues {
            get {
                return Enum.GetValues(typeof(Led)).Cast<Led>();
            }
        }

        public IEnumerable<GridLed> GridLedValues {
            get {
                return Enum.GetValues(typeof(GridLed)).Cast<GridLed>();
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

        public string Connected {
            get {
                return "Connected: " + Core.Mouse.Instance.Connected.ToString();
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
                Core.Mouse.Instance.SetReactive(SelectedReactiveDuration, ColorOne.Color.ToColoreColor(), SelectedLed);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }

        void SetWaveEffect() {
            try {
                Core.Mouse.Instance.SetWave(SelectedWaveDirection);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }

        void SetGridLedEffect() {
            try {
                Core.Mouse.Instance[SelectedGridLed] = ColorOne.Color.ToColoreColor();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }

        void SetLedEffect() {
            try {
                Core.Mouse.Instance[SelectedLed] = ColorOne.Color.ToColoreColor();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
