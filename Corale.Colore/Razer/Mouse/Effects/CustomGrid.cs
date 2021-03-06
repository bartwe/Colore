﻿// ---------------------------------------------------------------------------------------
// <copyright file="CustomGrid.cs" company="Corale">
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

namespace Corale.Colore.Razer.Mouse.Effects {
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    using Corale.Colore.Annotations;
    using Corale.Colore.Core;

    /// <summary>
    /// Custom grid effect for mouse LEDs.
    /// </summary>
    public struct CustomGrid : IEquatable<CustomGrid>, IEquatable<Color[][]>, IEquatable<Color[]> {
        /// <summary>
        /// Color definitions for each led on the mouse.
        /// </summary>
        /// <remarks>
        /// The array is 1-dimensional, but will be passed to code expecting
        /// a 2-dimensional array. Access to this array is done using indices
        /// according to: <c>column + row * Constants.MaxColumns</c>
        /// </remarks>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.MaxGridLeds)]
        readonly Color[] _colors;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomGrid" /> struct.
        /// </summary>
        /// <param name="colors">The colors to use.</param>
        /// <exception cref="ArgumentException">Thrown if the colors array supplied is of an incorrect size.</exception>
        public CustomGrid(Color[][] colors) {
            var rows = colors.GetLength(0);

            if (rows != Constants.MaxRows) {
                throw new ArgumentException(
                    "Colors array has incorrect number of rows, should be " + Constants.MaxRows + ", received " + rows + ".", "colors");
            }

            _colors = new Color[Constants.MaxGridLeds];

            for (var row = 0; row < Constants.MaxRows; row++) {
                if (colors[row].Length != Constants.MaxColumns) {
                    throw new ArgumentException(
                        "Colors array has incorrect number of columns, should be " + Constants.MaxColumns + ", received " + colors[row].Length + " for row " + row + ".",
                        "colors")
                        ;
                }

                for (var column = 0; column < Constants.MaxColumns; column++)
                    this[row, column] = colors[row][column];
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomGrid" /> struct.
        /// </summary>
        /// <param name="colors">The colors to use.</param>
        /// <exception cref="ArgumentException">Thrown if the colors array supplied is of an invalid size.</exception>
        public CustomGrid(IList<Color> colors) {
            if (colors.Count != Constants.MaxGridLeds) {
                throw new ArgumentException(
                    "Colors array has incorrect size, should be " + Constants.MaxGridLeds + ", actual is " + colors.Count + ".",
                    "colors")
                    ;
            }

            _colors = new Color[Constants.MaxGridLeds];

            for (var index = 0; index < Constants.MaxGridLeds; index++)
                this[index] = colors[index];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomGrid" /> struct
        /// with every position set to a specific color.
        /// </summary>
        /// <param name="color">The <see cref="Color" /> to set each position to.</param>
        public CustomGrid(Color color) {
            _colors = new Color[Constants.MaxGridLeds];

            for (var index = 0; index < Constants.MaxGridLeds; index++)
                this[index] = color;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomGrid" /> struct
        /// with the colors copied from another struct of the same type.
        /// </summary>
        /// <param name="other">The <see cref="CustomGrid" /> struct to copy data from.</param>
        public CustomGrid(CustomGrid other)
            : this(other._colors) {}

        /// <summary>
        /// Gets or sets cells in the <see cref="CustomGrid" />.
        /// </summary>
        /// <param name="row">Row to access, zero indexed.</param>
        /// <param name="column">Column to access, zero indexed.</param>
        /// <returns>The <see cref="Color" /> at the specified position.</returns>
        [PublicAPI]
        public Color this[int row, int column] {
            get {
                if (row < 0 || row >= Constants.MaxRows) {
                    throw new ArgumentOutOfRangeException(
                        "row",
                        row,
                        "Attempted to access a row that does not exist.");
                }

                if (column < 0 || column >= Constants.MaxColumns) {
                    throw new ArgumentOutOfRangeException(
                        "column",
                        column,
                        "Attempted to access a column that does not exist.");
                }

#pragma warning disable SA1407 // Arithmetic expressions must declare precedence
                return _colors[column + row * Constants.MaxColumns];
#pragma warning restore SA1407 // Arithmetic expressions must declare precedence
            }

            set {
                if (row < 0 || row >= Constants.MaxRows) {
                    throw new ArgumentOutOfRangeException(
                        "row",
                        row,
                        "Attempted to access a row that does not exist.");
                }

                if (column < 0 || column >= Constants.MaxColumns) {
                    throw new ArgumentOutOfRangeException(
                        "column",
                        column,
                        "Attempted to access a column that does not exist.");
                }

#pragma warning disable SA1407 // Arithmetic expressions must declare precedence
                _colors[column + row * Constants.MaxColumns] = value;
#pragma warning restore SA1407 // Arithmetic expressions must declare precedence
            }
        }

        /// <summary>
        /// Gets or sets a position in the custom grid.
        /// </summary>
        /// <param name="index">The index to access, zero indexed.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the requested index is out of range.</exception>
        [PublicAPI]
        public Color this[int index] {
            get {
                if (index < 0 || index >= Constants.MaxGridLeds) {
                    throw new ArgumentOutOfRangeException(
                        "index",
                        index,
                        "Attempted to access an index that does not exist.");
                }

                return _colors[index];
            }

            set {
                if (index < 0 || index >= Constants.MaxGridLeds) {
                    throw new ArgumentOutOfRangeException(
                        "index",
                        index,
                        "Attempted to access an index that does not exist.");
                }

                _colors[index] = value;
            }
        }

        /// <summary>
        /// Gets or sets the color for a specific LED in the <see cref="CustomGrid" />.
        /// </summary>
        /// <param name="led">The <see cref="GridLed" /> to access.</param>
        /// <returns>The <see cref="Color" /> for the specified led.</returns>
        [PublicAPI]
        public Color this[GridLed led] {
            get {
                var row = (int)led >> 8;
                var column = (int)led & 0xFF;
                return this[row, column];
            }

            set {
                var row = (int)led >> 8;
                var column = (int)led & 0xFF;
                this[row, column] = value;
            }
        }

        /// <summary>
        /// Indicates whether the current object is equal to an instance of
        /// a 2-dimensional array of <see cref="Color" />.
        /// </summary>
        /// <param name="other">
        /// A 2-dimensional array of <see cref="Color" /> to compare with this object.
        /// </param>
        /// <returns>
        /// <c>true</c> if the <paramref name="other" /> object has the same
        /// number of rows and columns, and contain matching colors; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Color[][] other) {
            if (other == null || other.GetLength(0) != Constants.MaxRows)
                return false;

            for (var row = 0; row < Constants.MaxRows; row++) {
                if (other[row].Length != Constants.MaxColumns)
                    return false;

                for (var col = 0; col < Constants.MaxColumns; col++) {
                    if (this[row, col] != other[row][col])
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Indicates whether the current object is equal to an instance of
        /// an array of <see cref="Color" />.
        /// </summary>
        /// <param name="other">An array of <see cref="Color" /> to compare with this object.</param>
        /// <returns>
        /// <c>true</c> if the <paramref name="other" /> object has the same
        /// number of elements, and contain matching colors; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Color[] other) {
            if (other == null || other.Length != Constants.MaxGridLeds)
                return false;

            for (var index = 0; index < Constants.MaxColumns; index++) {
                if (this[index] != other[index])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">A <see cref="CustomGrid" /> to compare with this object.</param>
        /// <returns>
        /// <c>true</c> if the current object is equal to the <paramref name="other"/> parameter;
        /// otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(CustomGrid other) {
            for (var row = 0; row < Constants.MaxRows; row++) {
                for (var col = 0; col < Constants.MaxColumns; col++) {
                    if (this[row, col] != other[row, col])
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Compares an instance of <see cref="CustomGrid" /> with
        /// another object for equality.
        /// </summary>
        /// <param name="left">The left operand, an instance of <see cref="CustomGrid" />.</param>
        /// <param name="right">The right operand, any type of object.</param>
        /// <returns><c>true</c> if the two objects are equal, otherwise <c>false</c>.</returns>
        public static bool operator ==(CustomGrid left, object right) {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares an instance of <see cref="CustomGrid" /> with
        /// another object for inequality.
        /// </summary>
        /// <param name="left">The left operand, an instance of <see cref="CustomGrid" />.</param>
        /// <param name="right">The right operand, any type of object.</param>
        /// <returns><c>true</c> if the two objects are not equal, otherwise <c>false</c>.</returns>
        public static bool operator !=(CustomGrid left, object right) {
            return !left.Equals(right);
        }

        /// <summary>
        /// Creates a new empty <see cref="CustomGrid" /> struct.
        /// </summary>
        /// <returns>An instance of <see cref="CustomGrid" />
        /// filled with the color black.</returns>
        [PublicAPI]
        public static CustomGrid Create() {
            return new CustomGrid(Color.Black);
        }

        /// <summary>
        /// Returns a copy of this struct.
        /// </summary>
        /// <returns>A copy of this struct.</returns>
        [PublicAPI]
        public CustomGrid Clone() {
            return new CustomGrid(this);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode() {
            if (_colors == null)
                return 0;
            return _colors.GetHashCode();
        }

        /// <summary>
        /// Clears the colors from the grid, setting them to <see cref="Color.Black" />.
        /// </summary>
        [PublicAPI]
        public void Clear() {
            Set(Color.Black);
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// <c>true</c> if <paramref name="obj"/> and this instance are of compatible types
        /// and represent the same value; otherwise, <c>false</c>.
        /// </returns>
        /// <param name="obj">Another object to compare to. </param>
        public override bool Equals(object obj) {
            if (ReferenceEquals(obj, null))
                return false;

            if (obj is CustomGrid)
                return Equals((CustomGrid)obj);

            var arr2D = obj as Color[][];

            if (arr2D != null)
                return Equals(arr2D);

            var arr1D = obj as Color[];

            return arr1D != null && Equals(arr1D);
        }

        /// <summary>
        /// Sets the entire grid to a specific <see cref="Color" />.
        /// </summary>
        /// <param name="color">The <see cref="Color" /> to apply.</param>
        [PublicAPI]
        public void Set(Color color) {
            for (var index = 0; index < Constants.MaxGridLeds; index++)
                this[index] = color;
        }
    }
}
