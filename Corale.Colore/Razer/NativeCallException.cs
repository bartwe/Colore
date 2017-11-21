﻿// ---------------------------------------------------------------------------------------
// <copyright file="NativeCallException.cs" company="Corale">
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

namespace Corale.Colore.Razer
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Runtime.Serialization;
    using System.Security;
    using System.Security.Permissions;

    /// <summary>
    /// Thrown when a native function returns an erroneous result value.
    /// </summary>
    [Serializable]
    public sealed class NativeCallException : ColoreException
    {
        /// <summary>
        /// Template used to construct exception message from.
        /// </summary>
        private const string MessageTemplate = "Call to native Chroma SDK function {0} failed with error: {1}";

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeCallException" /> class.
        /// </summary>
        /// <param name="function">The name of the function that was called.</param>
        /// <param name="result">The result returned from the called function.</param>
        internal NativeCallException(string function, Result result)
            : base(
                string.Format(CultureInfo.InvariantCulture, MessageTemplate, function, result),
                new Win32Exception(result))
        {
            Function = function;
            Result = result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeCallException" /> class.
        /// </summary>
        /// <param name="info">Serialization info object.</param>
        /// <param name="context">Streaming context.</param>
        private NativeCallException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Function = info.GetString("Function");
            Result = info.GetInt32("Result");
        }

        /// <summary>
        /// Gets the name of the native function that was called.
        /// </summary>
        public string Function;

        /// <summary>
        /// Gets the <see cref="Result" /> object indicating
        /// the result returned from the native function.
        /// </summary>
        public Result Result;

        /// <summary>
        /// Adds object data to serialization object.
        /// </summary>
        /// <param name="info">Serialization info object.</param>
        /// <param name="context">Streaming context.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("Function", Function);
            info.AddValue("Result", (int)Result);
        }
    }
}
