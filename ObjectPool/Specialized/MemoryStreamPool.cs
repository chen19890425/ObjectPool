﻿// File name: MemoryStreamPool.cs
//
// Author(s): Alessio Parma <alessio.parma@gmail.com>
//
// The MIT License (MIT)
//
// Copyright (c) 2013-2018 Alessio Parma <alessio.parma@gmail.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute,
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT
// OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using CodeProject.ObjectPool.Core;
using System.IO;

namespace CodeProject.ObjectPool.Specialized
{
    /// <summary>
    ///   An <see cref="IObjectPool{PooledMemoryStream}"/> ready to be used.
    ///   <see cref="MemoryStream"/> management can be further configured using the
    ///   <see cref="MinimumMemoryStreamCapacity"/> and <see cref="MaximumMemoryStreamCapacity"/> properties.
    /// </summary>
    public sealed class MemoryStreamPool : ObjectPool<PooledMemoryStream>, IMemoryStreamPool
    {
        private int _minimumItemCapacity = SpecializedPoolConstants.DefaultMinimumMemoryStreamCapacity;
        private int _maximumItemCapacity = SpecializedPoolConstants.DefaultMaximumMemoryStreamCapacity;

        /// <summary>
        ///   Thread-safe pool instance.
        /// </summary>
        public static IMemoryStreamPool Instance { get; } = new MemoryStreamPool();

        /// <summary>
        ///   Builds the specialized pool.
        /// </summary>
        public MemoryStreamPool()
            : base(ObjectPoolConstants.DefaultPoolMinimumSize, ObjectPoolConstants.DefaultPoolMaximumSize, null, false)
        {
            FactoryMethod = () => new PooledMemoryStream(MinimumMemoryStreamCapacity);
            AdjustPoolSizeToBounds(AdjustMode.Minimum | AdjustMode.Maximum);
        }

        /// <summary>
        ///   Minimum capacity a <see cref="MemoryStream"/> should have when created and this is the
        ///   minimum capacity of all streams stored in the pool. Defaults to <see cref="SpecializedPoolConstants.DefaultMinimumMemoryStreamCapacity"/>.
        /// </summary>
        public int MinimumMemoryStreamCapacity
        {
            get { return _minimumItemCapacity; }
            set
            {
                var oldValue = _minimumItemCapacity;
                _minimumItemCapacity = value;
                if (oldValue < value)
                {
                    Clear();
                }
            }
        }

        /// <summary>
        ///   Maximum capacity a <see cref="MemoryStream"/> might have in order to be able to return
        ///   to pool. Defaults to <see cref="SpecializedPoolConstants.DefaultMaximumMemoryStreamCapacity"/>.
        /// </summary>
        public int MaximumMemoryStreamCapacity
        {
            get { return _maximumItemCapacity; }
            set
            {
                var oldValue = _maximumItemCapacity;
                _maximumItemCapacity = value;
                if (oldValue > value)
                {
                    Clear();
                }
            }
        }
    }
}