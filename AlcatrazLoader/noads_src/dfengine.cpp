#include <windows.h>
#include <stdio.h>

HINSTANCE gHInstance = NULL;

class IDummyInterface
{
public:
	virtual ~IDummyInterface() = default;
	virtual int __stdcall Initialize(int version, int* arguments) = 0;	// 4
	virtual void __stdcall Shutdown() = 0;	// 8

	virtual void __stdcall _padFunc1() { }// 12
	virtual int __stdcall _padFunc2(int a0, int a1, int* ptr1, int* ptr2) {  return -1; }// 16
	virtual void __stdcall _padFunc3() { }// 20
	virtual void __stdcall _padFunc4() { }// 24
	virtual void __stdcall _padFunc5() { }// 28
	virtual void __stdcall _padFunc6() { }// 32
	virtual void __stdcall _padFunc7(const char* arg) {  }// 36
	virtual void __stdcall _padFunc8() { }// 40
	virtual void __stdcall _padFunc9() { }// 44
	virtual void __stdcall _padFunc10() { }// 48
	virtual void __stdcall _padFunc11() { }// 52
	virtual void __stdcall _padFunc12(int arg) {  }// 56
	virtual void __stdcall _padFunc13(int arg) {  }// 60
	virtual void __stdcall _padFunc14() { }// 64
	virtual void __stdcall _padFunc15() { }// 68
	virtual int __stdcall _padFunc16(int arg) { return 0; }// 72
	virtual void __stdcall _padFunc17() { }// 76
	virtual void __stdcall _padFunc18() { }// 80
	virtual void __stdcall _padFunc19(int arg) {  }// 84
};

class CDummy : public IDummyInterface
{
public:
	~CDummy()
	{
	}

	int __stdcall Initialize(int version, int* arguments) override
	{
		printf("DFEngine game %d\n", version);
		return 0;
	};

	void __stdcall Shutdown() override
	{
		printf("DFEngine SHUTDOWN\n");
	}
};

static CDummy s_dummy;

void * __stdcall GetDFEngine(void* arg)
{
	return &s_dummy;
}

BOOL WINAPI DllMain(HINSTANCE hinstDLL, DWORD fdwReason, LPVOID lpvReserved)
{
	switch (fdwReason)
	{
		case DLL_PROCESS_ATTACH:
		{
			gHInstance = hinstDLL;
			break;
		}
	}

	return TRUE;
}