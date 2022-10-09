#pragma once

#include "HF/HackingFrameworkFWD.h"
#include "HF/HackingFramework.hpp"

#include <minidumpapiset.h>

// Taken from: http://msdn.microsoft.com/en-us/library/s975zw7k(VS.71).aspx
#ifdef __cplusplus
#define EXTERNC extern "C"
#else
#define EXTERNC
#endif

// _ReturnAddress and _AddressOfReturnAddress should be prototyped before use 
EXTERNC void* _AddressOfReturnAddress(void);
EXTERNC void* _ReturnAddress(void);

namespace AlcatrazUplayR2
{
	namespace ExceptionHanlder
	{
		inline void WriteMiniDump(EXCEPTION_POINTERS* exception = nullptr)
		{
			//
			//	Credits https://stackoverflow.com/questions/5028781/how-to-write-a-sample-code-that-will-crash-and-produce-dump-file
			//
			auto hDbgHelp = LoadLibraryA("dbghelp");
			if (hDbgHelp == nullptr)
				return;
			auto pMiniDumpWriteDump = (decltype(&MiniDumpWriteDump))GetProcAddress(hDbgHelp, "MiniDumpWriteDump");
			if (pMiniDumpWriteDump == nullptr)
				return;

			char name[MAX_PATH];
			{
				auto nameEnd = name + GetModuleFileNameA(GetModuleHandleA(0), name, MAX_PATH);
				SYSTEMTIME t;
				GetSystemTime(&t);

				wsprintfA(nameEnd - strlen(".exe"),
					"_%4d%02d%02d_%02d%02d%02d.dmp",
					t.wYear, t.wMonth, t.wDay, t.wHour, t.wMinute, t.wSecond);
			}

			auto hFile = CreateFileA(name, GENERIC_WRITE, FILE_SHARE_READ, 0, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, 0);
			if (hFile == INVALID_HANDLE_VALUE)
				return;

			MINIDUMP_EXCEPTION_INFORMATION exceptionInfo;
			exceptionInfo.ThreadId = GetCurrentThreadId();
			exceptionInfo.ExceptionPointers = exception;
			exceptionInfo.ClientPointers = FALSE;

			auto dumped = pMiniDumpWriteDump(
				GetCurrentProcess(),
				GetCurrentProcessId(),
				hFile,
				MINIDUMP_TYPE(MiniDumpWithIndirectlyReferencedMemory | MiniDumpScanMemory),
				exception ? &exceptionInfo : nullptr,
				nullptr,
				nullptr);

			CloseHandle(hFile);
		}

		inline void NotifyAboutException(EXCEPTION_POINTERS* exceptionInfoFrame)
		{
			MessageBox(
				NULL,
				"We got an fatal error.\nMinidump will be saved near exe.",
				"Driver San Francisco",
				MB_ICONERROR | MB_OK
			);

			WriteMiniDump(exceptionInfoFrame);
			exit(0);
		}

		inline LONG __stdcall ExceptionFilterWin32(EXCEPTION_POINTERS* exceptionInfoFrame)
		{
			if (exceptionInfoFrame->ExceptionRecord->ExceptionCode < 0x80000000)
			{
				return EXCEPTION_CONTINUE_EXECUTION;
			}

			NotifyAboutException(exceptionInfoFrame);

			return EXCEPTION_EXECUTE_HANDLER;
		}

		inline void GetExceptionPointers(DWORD dwExceptionCode, EXCEPTION_POINTERS** ppExceptionPointers)
		{
			// The following code was taken from VC++ 8.0 CRT (invarg.c: line 104)

			EXCEPTION_RECORD ExceptionRecord;
			CONTEXT ContextRecord;
			memset(&ContextRecord, 0, sizeof(CONTEXT));

#ifdef _X86_

			__asm {
				mov dword ptr[ContextRecord.Eax], eax
				mov dword ptr[ContextRecord.Ecx], ecx
				mov dword ptr[ContextRecord.Edx], edx
				mov dword ptr[ContextRecord.Ebx], ebx
				mov dword ptr[ContextRecord.Esi], esi
				mov dword ptr[ContextRecord.Edi], edi
				mov word ptr[ContextRecord.SegSs], ss
				mov word ptr[ContextRecord.SegCs], cs
				mov word ptr[ContextRecord.SegDs], ds
				mov word ptr[ContextRecord.SegEs], es
				mov word ptr[ContextRecord.SegFs], fs
				mov word ptr[ContextRecord.SegGs], gs
				pushfd
				pop[ContextRecord.EFlags]
			}

			ContextRecord.ContextFlags = CONTEXT_CONTROL;
#pragma warning(push)
#pragma warning(disable:4311)
			ContextRecord.Eip = (ULONG)_ReturnAddress();
			ContextRecord.Esp = (ULONG)_AddressOfReturnAddress();
#pragma warning(pop)
			ContextRecord.Ebp = *((ULONG*)_AddressOfReturnAddress() - 1);


#elif defined (_IA64_) || defined (_AMD64_)

			/* Need to fill up the Context in IA64 and AMD64. */
			RtlCaptureContext(&ContextRecord);

#else  /* defined (_IA64_) || defined (_AMD64_) */

			ZeroMemory(&ContextRecord, sizeof(ContextRecord));

#endif  /* defined (_IA64_) || defined (_AMD64_) */

			ZeroMemory(&ExceptionRecord, sizeof(EXCEPTION_RECORD));

			ExceptionRecord.ExceptionCode = dwExceptionCode;
			ExceptionRecord.ExceptionAddress = _ReturnAddress();

			///

			EXCEPTION_RECORD* pExceptionRecord = new EXCEPTION_RECORD;
			memcpy(pExceptionRecord, &ExceptionRecord, sizeof(EXCEPTION_RECORD));
			CONTEXT* pContextRecord = new CONTEXT;
			memcpy(pContextRecord, &ContextRecord, sizeof(CONTEXT));

			*ppExceptionPointers = new EXCEPTION_POINTERS;
			(*ppExceptionPointers)->ExceptionRecord = pExceptionRecord;
			(*ppExceptionPointers)->ContextRecord = pContextRecord;
		}

		inline void PureCallHandler()
		{
			// Retrieve exception information
			EXCEPTION_POINTERS* pExceptionPtrs = NULL;
			GetExceptionPointers(0, &pExceptionPtrs);

			NotifyAboutException(pExceptionPtrs);
		}
	}

	inline void InitExceptionHandler()
	{
		_set_purecall_handler(ExceptionHanlder::PureCallHandler);
		AddVectoredExceptionHandler(0UL, ExceptionHanlder::ExceptionFilterWin32);
	}
}