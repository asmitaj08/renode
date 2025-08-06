//
// Copyright (c) 2010-2025 Antmicro
//
// This file is licensed under the MIT License.
// Full license text is available in 'licenses/MIT.txt'.
//
using Antmicro.Renode.Core;
using Antmicro.Renode.Peripherals.CPU;
using ELFSharp.ELF;
using System;

namespace Antmicro.Renode.Peripherals.SystemC
{
    public partial class SystemCCortexMCPU : SystemCCPU
    {
        public SystemCCortexMCPU(IMachine machine, string address, int port, string cpuType, Endianess endianess = Endianess.LittleEndian, CpuBitness bitness = CpuBitness.Bits32, int timeSyncPeriodUS = 1000, bool disableTimeoutCheck = false)
            : base(machine, address, port, cpuType, endianess, bitness, timeSyncPeriodUS, disableTimeoutCheck)
        {
            // Intentionally left blank
        }

        [Register]
        public override RegisterValue PC
        {    
            get => GetRegisterValue32((int)SystemCCortexMRegisters.PC);
            set => SetRegisterValue32((int)SystemCCortexMRegisters.PC, value);
        }

        [Register]
        public RegisterValue SP
        {
            get => GetRegisterValue32((int)SystemCCortexMRegisters.SP);
            set => SetRegisterValue32((int)SystemCCortexMRegisters.SP, value);
        }

        public override string Architecture { get { return "arm-m"; } }

        // Fuzz snapshot variables for SystemCCortexMCPU
        private RegisterValue fuzz_snap_pc;
        private RegisterValue fuzz_snap_sp;

        // Implementation of fuzz snapshot/restore for SystemCCortexMCPU
        public override void fuzz_snap_capture()
        {
            Console.WriteLine("^^^^^ SystemCCortexMCPU.cs fuzz_snap_capture()");
            
            // Capture SystemCCortexMCPU-specific state
            fuzz_snap_pc = PC;
            fuzz_snap_sp = SP;
        }

        public override void fuzz_snap_restore()
        {
            Console.WriteLine("^^^^^ SystemCCortexMCPU.cs fuzz_snap_restore()");
            
            // Restore SystemCCortexMCPU-specific state
            PC = fuzz_snap_pc;
            SP = fuzz_snap_sp;
        }
    }
}
