using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace ClassLibrary
{
    public enum VMf
    {
        vmsExp,
        vmsErf,
        vmdExp,
        vmdErf
    }
    public class VMGrid
    {
        public int N { get; set; }
        public float Min { get; set; }
        public float Max { get; set; }
        public VMGrid(int N = 2, float Min = 0, float Max = 0, VMf vmf = VMf.vmdExp)
        {
            this.N = N;
            this.Min = Min;
            this.Max = Max;
            this.VMf = vmf;
        }
        public float Step
        {
            get { return (Max - Min) / (N - 1); }
        }
        public VMf VMf { get; set; }
    }
    public struct VMTime
    {
        public VMGrid Grid { get; set; }
        public int Time_HA { get; set; }
        public int Time_EP { get; set; }
        public int Time_base { get; set; }
        public VMTime(VMGrid Grid, int Time_HA, int Time_EP, int Time_base)
        {
            this.Grid = Grid;
            this.Time_HA = Time_HA;
            this.Time_EP = Time_EP;
            this.Time_base = Time_base;
        }
        public float Time_EP_base
        {
            get
            {
                if (Time_base == 0) return 0;
                else return (float)Time_EP / Time_base;
            }
        }
        public float Time_HA_base
        {
            get
            {
                if (Time_base == 0) return 0;
                else return (float)Time_HA / Time_base;
            }
        }
        public override string ToString()
        {
            return $"N = {Grid.N}, Min = {Grid.Min}, Max = {Grid.Max}, " +
                $"Step = {Grid.Step}, func = {Grid.VMf}, Time_HA = {Time_HA}, " +
                $"Time_EP = {Time_EP}, Time_base = {Time_base}, Time_HA / Time_base = {Time_HA_base}, " +
                $"Time_EP / Time_base = {Time_EP_base}";
        }
    }
    public struct VMAccuracy
    {
        public VMGrid Grid { get; set; }
        public float Max_sub_value_HA { get; set; }
        public float Max_sub_value_EP { get; set; }
        public float Max_sub_arg { get; set; }
        public VMAccuracy(VMGrid Grid, float Max_sub_value_HA, float Max_sub_value_EP, float Max_sub_arg)
        {
            this.Grid = Grid;
            this.Max_sub_value_HA = Max_sub_value_HA;
            this.Max_sub_value_EP = Max_sub_value_EP;
            this.Max_sub_arg = Max_sub_arg;
        }
        public float Max_sub_value
        {
            get { return Math.Abs(Max_sub_value_EP - Max_sub_value_HA); }
        }
        public override string ToString()
        {
            return $"N = {Grid.N}, Min = {Grid.Min}, Max = {Grid.Max}, " +
                $"Step = {Grid.Step}, func = {Grid.VMf}, Max_sub_value = {Max_sub_value}, " +
                $"Max_sub_arg = {Max_sub_arg}, Value VML_HA = {Max_sub_value_HA}, Value VML_EP = {Max_sub_value_EP}";
        }
    }
    public class VMBenchmark
    {
        [DllImport(@"C:\\Users\kafanovn\source\repos\lab6\x64\Debug\Dll.dll")]
        public static extern void vmf(int n, float min, float max, int num_func, int[] time, float[] max_value);
        public ObservableCollection<VMTime> VMTimes { get; set; }
        public ObservableCollection<VMAccuracy> VMAccuracies { get; set; }
        public VMBenchmark()
        {
            try
            {
                VMTimes = new ObservableCollection<VMTime>();
                VMAccuracies = new ObservableCollection<VMAccuracy>();
            }
            catch { throw; }
        }
        public void AddVMTime(VMGrid New_grid)
        {
            try
            {
                VMGrid copyGrid = new VMGrid(New_grid.N, New_grid.Min, New_grid.Max, New_grid.VMf);
                int[] time = new int[3];
                float[] max_value = new float[3];
                vmf(copyGrid.N, copyGrid.Min, copyGrid.Max, (int)copyGrid.VMf, time, max_value);
                var new_time = new VMTime(copyGrid, time[0], time[1], time[2]);
                VMTimes.Add(new_time);
            }
            catch
            {
                throw;
            }
        }
        public void AddVMAccuracy(VMGrid New_grid)
        {
            try
            {
                VMGrid copyGrid = new VMGrid(New_grid.N, New_grid.Min, New_grid.Max, New_grid.VMf);
                int[] time = new int[3];
                float[] max_value = new float[3];
                vmf(copyGrid.N, copyGrid.Min, copyGrid.Max, (int)copyGrid.VMf, time, max_value);
                var new_accur = new VMAccuracy(copyGrid, max_value[0], max_value[1], max_value[2]);
                VMAccuracies.Add(new_accur);
            }
            catch
            {
                throw;
            }
        }
        public float Time_HA_base
        {
            get
            {
                try
                {
                    float min = VMTimes[0].Time_HA_base;
                    for (int i = 0; i < VMTimes.Count; i++)
                    {
                        if (min > VMTimes[i].Time_HA_base)
                            min = VMTimes[i].Time_HA_base;
                    }
                    return min;
                }
                catch
                {
                    return 0;
                }
            }
        }
        public float Time_EP_base
        {
            get
            {
                try
                {
                    float min = VMTimes[0].Time_EP_base;
                    for (int i = 0; i < VMTimes.Count; i++)
                    {
                        if (min > VMTimes[i].Time_EP_base)
                            min = VMTimes[i].Time_EP_base;
                    }
                    return min;
                }
                catch
                {
                    return 0;
                }
            }
        }
    }
}