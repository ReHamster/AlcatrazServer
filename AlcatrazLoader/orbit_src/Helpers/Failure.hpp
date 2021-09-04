#pragma once

#include "pch.h"

namespace AlcatrazUplayR2
{
    inline void Fail(const String &message, const bool silent, const char *file = nullptr, const int line = 0)
    {
        if (silent)
        {
            Console::error(message);
        }
        else
        {
            if (file != nullptr && line > 0)
            {
                MessageBoxA(nullptr, String::fromPrintf("Message: %s\nFile: %s\nLine: %d\n", (const char*)message, file, line),
                            "Error...", MB_ICONERROR | MB_ICONERROR);
            }
            else
            {
                MessageBoxA(nullptr, message, "Error...", MB_ICONERROR | MB_ICONERROR);
            }
        }

        exit(EXIT_FAILURE);
    }
} // namespace AlcatrazUplayR2
