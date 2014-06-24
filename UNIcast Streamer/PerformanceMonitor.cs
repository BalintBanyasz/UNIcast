using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace UNIcast_Streamer
{
    class PerformanceMonitor
    {
        private const string PcCategoryProcess = "Process";
        private const string PcCounterProcessor = "% Processor Time";
        private const string PcCounterMemory = "Working Set - Private";
        private const string PcInstanceTotal = "_Total";

        /// <summary>
        /// Event Handler for Performance values
        /// </summary>
        public delegate void PerfValuesReceivedEventHandler(object sender, PerfValuesEventArgs e);
        public event PerfValuesReceivedEventHandler PerfValuesReceived;

        List<Performance> monitoredProcesses;
        PerformanceCounter pcCPUTotal;

        volatile bool isRunning;

        public class PerfValuesEventArgs : System.EventArgs
        {
            public string processName;
            public float cpuPercentage;
            public float ramK;
        }

        struct Performance
        {
            public string processName;
            public PerformanceCounter cpu;
            public PerformanceCounter ram;
        }

        public PerformanceMonitor(string[] monitoredProcesses)
        {
            pcCPUTotal = new PerformanceCounter(PcCategoryProcess, PcCounterProcessor, PcInstanceTotal);
            this.monitoredProcesses = new List<Performance>();
            foreach (string process in monitoredProcesses)
            {
                addProcess(process);
            }
        }

        public void StartMonitoring()
        {
            isRunning = true;
            Thread t = new System.Threading.Thread(Update);
            t.Start();
        }

        public void StopMonitoring()
        {
            isRunning = false;
        }

        public void Update()
        {
            while (isRunning)
            {
                Thread.Sleep(1000);
                float t = pcCPUTotal.NextValue();
                foreach (Performance perf in monitoredProcesses)
                {
                    try
                    {
                        PerfValuesEventArgs args = new PerfValuesEventArgs();
                        args.processName = perf.processName;
                        args.cpuPercentage = perf.cpu.NextValue() / t * 100;
                        args.ramK = perf.ram.NextValue() / 1024;
                        PerfValuesReceived(this, args);
                    }
                    catch (Exception) { }
                }
            }
        }

        public void addProcess(string processName)
        {
            if (monitoredProcesses.Any(p => p.processName == processName))
                return;
            Performance perf = new Performance();
            perf.processName = processName;
            try
            {
                perf.cpu = new PerformanceCounter(PcCategoryProcess, PcCounterProcessor, processName);
                perf.ram = new PerformanceCounter(PcCategoryProcess, PcCounterMemory, processName);
                perf.cpu.NextValue();
                perf.ram.NextValue();
                this.monitoredProcesses.Add(perf);
            }
            catch (Exception) { }
        }

        public void removeProcess(string processName)
        {
            if (!monitoredProcesses.Any(p => p.processName == processName))
                return;
            monitoredProcesses.RemoveAll(p => p.processName == processName);
        }
    }
}
