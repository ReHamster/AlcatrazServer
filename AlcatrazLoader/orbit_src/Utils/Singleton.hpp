#pragma once

#include "pch.h"

namespace AlcatrazUplayR2
{
    template <class T>
    class SingletonBase
    {
    public:
        static T &Instance()
        {
            static T INSTANCE;
            return INSTANCE;
        }
    };

    template <class T>
    class Singleton : public SingletonBase<Singleton<T>>
    {
        T Value;

    public:
        void Set(T value)
        {
            Value = value;
        }
        T &Get()
        {
            return Value;
        }
    };
} // namespace AlcatrazUplayR2
