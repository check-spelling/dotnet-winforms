// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using static Interop;

namespace System
{
    internal ref partial struct MallocSpyScope
    {
        /// <summary>
        ///  Redirecting spy that we register as a global.
        /// </summary>
        private class MasterSpy : Ole32.IMallocSpy
        {
            private Ole32.IMallocSpy _currentSpy;
            private uint _registeredThread;
            private readonly object _lock = new();

            public Ole32.IMallocSpy CurrentSpy
            {
                get
                {
                    lock (_lock)
                    {
                        return (_registeredThread == 0 || _registeredThread == Kernel32.GetCurrentThreadId())
                            ? _currentSpy : null;
                    }
                }
            }

            public void SetSpy(Ole32.IMallocSpy spy, bool currentThreadOnly)
            {
                lock (_lock)
                {
                    _currentSpy = spy;
                    _registeredThread = currentThreadOnly ? Kernel32.GetCurrentThreadId() : 0;
                }
            }

            public nuint PreAlloc(nuint cbRequest) => CurrentSpy?.PreAlloc(cbRequest) ?? cbRequest;

            public unsafe void* PostAlloc(void* pActual)
            {
                Ole32.IMallocSpy current = CurrentSpy;
                return current is null
                    ? pActual
                    : current.PostAlloc(pActual);
            }

            public unsafe void* PreFree(void* pRequest, BOOL fSpied)
            {
                Ole32.IMallocSpy current = CurrentSpy;
                return current is null
                    ? pRequest
                    : current.PreFree(pRequest, fSpied);
            }

            public void PostFree(BOOL fSpied) => CurrentSpy?.PostFree(fSpied);

            public unsafe nuint PreRealloc(void* pRequest, nuint cbRequest, void** ppNewRequest, BOOL fSpied)
                => CurrentSpy?.PreRealloc(pRequest, cbRequest, ppNewRequest, fSpied) ?? cbRequest;

            public unsafe void* PostRealloc(void* pActual, BOOL fSpied)
            {
                Ole32.IMallocSpy current = CurrentSpy;
                return current is null
                    ? pActual
                    : current.PostRealloc(pActual, fSpied);
            }

            public unsafe void* PreGetSize(void* pRequest, BOOL fSpied)
            {
                Ole32.IMallocSpy current = CurrentSpy;
                return current is null
                    ? pRequest
                    : current.PreGetSize(pRequest, fSpied);
            }

            public nuint PostGetSize(nuint cbActual, BOOL fSpied)
                => CurrentSpy?.PostGetSize(cbActual, fSpied) ?? cbActual;

            public unsafe void* PreDidAlloc(void* pRequest, BOOL fSpied)
            {
                Ole32.IMallocSpy current = CurrentSpy;
                return current is null
                    ? pRequest
                    : current.PreDidAlloc(pRequest, fSpied);
            }

            public unsafe int PostDidAlloc(void* pRequest, BOOL fSpied, int fActual)
                => CurrentSpy?.PostDidAlloc(pRequest, fSpied, fActual) ?? fActual;

            public void PreHeapMinimize() => CurrentSpy?.PreHeapMinimize();

            public void PostHeapMinimize() => CurrentSpy?.PostHeapMinimize();
        }
    }
}
