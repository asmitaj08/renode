//
// Copyright (c) 2010-2021 Antmicro
//
// This file is licensed under the MIT License.
// Full license text is available in 'licenses/MIT.txt'.
//
#include "apb3.h"

void APB3::tick(bool countEnable, uint64_t steps = 1)
{
    for(uint64_t i = 0; i < steps; i++) {
        *pclk = 1;
        evaluateModel();
        *pclk = 0;
        evaluateModel();
    }

    if(countEnable) {
        tickCounter += steps;
    }
}

void APB3::timeoutTick(uint8_t* signal, uint8_t expectedValue, int timeout = DEFAULT_TIMEOUT)
{
    do
    {
        tick(true);
        timeout--;
    }
    while((*signal != expectedValue) && timeout > 0);

    if(timeout == 0) {
        throw "Operation timeout";
    }
}

void APB3::write(uint64_t addr, uint64_t value)
{
    *psel = 1;
    *pwrite = 1;
    *paddr = addr;
    *pwdata = value;

    timeoutTick(pready, 1);

    *penable = 1;
    
    tick(true);

    *psel = 0;
    *penable = 0;

    tick(true);
}

uint64_t APB3::read(uint64_t addr)
{
    *psel = 1;
    *pwrite = 0;
    *paddr = addr;

    timeoutTick(pready, 1);

    *penable = 1;

    tick(true);

    uint64_t result = *prdata;

    *psel = 0;
    *penable = 0;

    tick(true);

    return result;
}

void APB3::reset()
{
    *prst = 1;
    tick(true);
    *prst = 0;
    tick(true);
}
