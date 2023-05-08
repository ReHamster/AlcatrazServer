#pragma once

#include <memory>

namespace HF {
namespace Win32
{
    class Module;
    class Process;

    using ModuleRef = std::weak_ptr<Module>;
    using ModulePtr = std::shared_ptr<Module>;
    using ProcessRef = std::weak_ptr<Process>;
    using ProcessPtr = std::shared_ptr<Process>;
}
}