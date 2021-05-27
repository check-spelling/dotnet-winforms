// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using static Interop;

namespace System
{
    internal class MallocSpy : Ole32.IMallocSpy
    {
        public virtual nuint PreAlloc(nuint cbRequest) => cbRequest;
        public virtual unsafe void* PostAlloc(void* pActual) => pActual;
        public virtual unsafe void* PreFree(void* pRequest, BOOL fSpied) => pRequest;
        public virtual void PostFree(BOOL fSpied) { }
        public virtual unsafe nuint PreRealloc(void* pRequest, nuint cbRequest, void** ppNewRequest, BOOL fSpied) => cbRequest;
        public virtual unsafe void* PostRealloc(void* pActual, BOOL fSpied) => pActual;
        public virtual unsafe void* PreGetSize(void* pRequest, BOOL fSpied) => pRequest;
        public virtual nuint PostGetSize(nuint cbActual, BOOL fSpied) => cbActual;
        public virtual unsafe void* PreDidAlloc(void* pRequest, BOOL fSpied) => pRequest;
        public virtual unsafe int PostDidAlloc(void* pRequest, BOOL fSpied, int fActual) => fActual;
        public virtual void PreHeapMinimize() { }
        public virtual void PostHeapMinimize() { }

        internal class FreeTracker : MallocSpy
        {
            public List<IntPtr> FreedBlocks { get; } = new();

            public override unsafe void* PreFree(void* pRequest, BOOL fSpied)
            {
                FreedBlocks.Add((IntPtr)pRequest);
                return pRequest;
            }
        }
    }
}
