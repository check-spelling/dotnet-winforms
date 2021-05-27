// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.InteropServices;

internal partial class Interop
{
    internal static partial class Ole32
    {
        [ComImport]
        [Guid("0000001d-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public unsafe interface IMallocSpy
        {
            [PreserveSig]
            nuint PreAlloc(
                nuint cbRequest);

            [PreserveSig]
            void* PostAlloc(
                void* pActual);

            [PreserveSig]
            void* PreFree(
                void* pRequest,
                BOOL fSpied);

            [PreserveSig]
            void PostFree(
                BOOL fSpied);

            [PreserveSig]
            nuint PreRealloc(
                void* pRequest,
                nuint cbRequest,
                void** ppNewRequest,
                BOOL fSpied);

            [PreserveSig]
            void* PostRealloc(
                void* pActual,
                BOOL fSpied);

            [PreserveSig]
            void* PreGetSize(
                void* pRequest,
                BOOL fSpied);

            [PreserveSig]
            nuint PostGetSize(
                nuint cbActual,
                BOOL fSpied);

            [PreserveSig]
            void* PreDidAlloc(
                void* pRequest,
                BOOL fSpied);

            [PreserveSig]
            int PostDidAlloc(
                void* pRequest,
                BOOL fSpied,
                int fActual);

            [PreserveSig]
            void PreHeapMinimize();

            [PreserveSig]
            void PostHeapMinimize();
        }
    }
}
