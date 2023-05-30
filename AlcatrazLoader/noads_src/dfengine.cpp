#include <windows.h>
#include <stdio.h>

HINSTANCE gHInstance = NULL;

class IDummyInterface
{
public:
	virtual ~IDummyInterface() = default;
	virtual int __stdcall Initialize(int version, int* arguments) = 0;	// 4
	virtual void __stdcall Shutdown() = 0;	// 8

	virtual void __stdcall _padFunc1() { printf("%s\n", __FUNCTION__); }// 12
	virtual int __stdcall _padFunc2(int a0, int a1, int* ptr1, int* ptr2) { printf("%s\n", __FUNCTION__); return -1; }// 16
	virtual void __stdcall _padFunc3() { printf("%s\n", __FUNCTION__); }// 20
	virtual void __stdcall _padFunc4() { printf("%s\n", __FUNCTION__); }// 24
	virtual void __stdcall _padFunc5() { printf("%s\n", __FUNCTION__); }// 28
	virtual void __stdcall _padFunc6() { printf("%s\n", __FUNCTION__); }// 32
	virtual void __stdcall _padFunc7(const char* arg) { printf("%s\n", __FUNCTION__); }// 36
	virtual void __stdcall _padFunc8() { printf("%s\n", __FUNCTION__); }// 40
	virtual void __stdcall _padFunc9() { printf("%s\n", __FUNCTION__); }// 44
	virtual void __stdcall _padFunc10() { printf("%s\n",__FUNCTION__); }// 48
	virtual void __stdcall _padFunc11() { printf("%s\n",__FUNCTION__); }// 52
	virtual void __stdcall _padFunc12(int arg) { printf("%s\n",__FUNCTION__); }// 56
	virtual void __stdcall _padFunc13(int arg) { printf("%s\n",__FUNCTION__); }// 60
	virtual void __stdcall _padFunc14() { printf("%s\n",__FUNCTION__); }// 64
	virtual void __stdcall _padFunc15() { printf("%s\n",__FUNCTION__); }// 68
	virtual int __stdcall _padFunc16(int arg) { printf("%s\n", __FUNCTION__); return 0; }// 72
	virtual void __stdcall _padFunc17() { printf("%s\n",__FUNCTION__); }// 76
	virtual void __stdcall _padFunc18() { printf("%s\n",__FUNCTION__); }// 80
	virtual void __stdcall _padFunc19(int arg) { printf("%s\n",__FUNCTION__); }// 84
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