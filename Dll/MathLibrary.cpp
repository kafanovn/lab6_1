#include "pch.h" // use stdafx.h in Visual Studio 2017 and earlier
#include "MathLibrary.h"
#include <mkl.h>
#include <cmath>
#include <ctime>

extern "C" _declspec(dllexport)
void vmf(int n, float min, float max, int num_func, int time[], float max_value[])
{
    float* args = new float[n];
    for (int i = 0; i < n; i++) {
        args[i] = ((max - min) / n)*i + min;
    }
    float* value_HA = new float[n];
    float* value_EP = new float[n];
    float* value_base = new float[n];

    if (num_func == 0) {
        unsigned int start_time = clock();
        vmsExp(n, args, value_HA, VML_HA);
        unsigned int end_time = clock();
        time[0] = end_time - start_time;

        start_time = clock();
        vmsExp(n, args, value_EP, VML_EP);
        end_time = clock();
        time[1] = end_time - start_time;

        start_time = clock();
        for (int i = 0; i < n; i++) {
            value_base[i] = exp(args[i]);
        }
        end_time = clock();
        time[2] = end_time - start_time;
    }
    if (num_func == 1) {
        unsigned int start_time = clock();
        vmsErf(n, args, value_HA, VML_HA);
        unsigned int end_time = clock();
        time[0] = end_time - start_time;

        start_time = clock();
        vmsErf(n, args, value_EP, VML_EP);
        end_time = clock();
        time[1] = end_time - start_time;

        start_time = clock();
        for (int i = 0; i < n; i++) {
            value_base[i] = erf(args[i]);
        }
        end_time = clock();
        time[2] = end_time - start_time;
    }
    if (num_func == 2) {
        double* args_d = new double[n];
        for (int i = 0; i < n; i++) {
            args_d[i] = (((double)max - min) / n) * i + min;
        }
        double* value_HA_d = new double[n];
        double* value_EP_d = new double[n];
        unsigned int start_time = clock();
        vmdExp(n, args_d, value_HA_d, VML_HA);
        unsigned int end_time = clock();
        time[0] = end_time - start_time;

        start_time = clock();
        vmdExp(n, args_d, value_EP_d, VML_EP);
        end_time = clock();
        time[1] = end_time - start_time;

        start_time = clock();
        for (int i = 0; i < n; i++) {
            value_base[i] = exp(args[i]);
        }
        end_time = clock();
        time[2] = end_time - start_time;
        for (int i = 0; i < n; i++) {
            value_HA[i] = (float)value_HA_d[i];
        }
        for (int i = 0; i < n; i++) {
            value_EP[i] = (float)value_EP_d[i];
        }
        delete[] value_HA_d;
        delete[] value_EP_d;
        delete[] args_d;
    }
    if (num_func == 3) {
        double* args_d = new double[n];
        for (int i = 0; i < n; i++) {
            args_d[i] = (((double)max - min) / n) * i + min;
        }
        double* value_HA_d = new double[n];
        double* value_EP_d = new double[n];
        unsigned int start_time = clock();
        vmdErf(n, args_d, value_HA_d, VML_HA);
        unsigned int end_time = clock();
        time[0] = end_time - start_time;

        start_time = clock();
        vmdErf(n, args_d, value_EP_d, VML_EP);
        end_time = clock();
        time[1] = end_time - start_time;

        start_time = clock();
        for (int i = 0; i < n; i++) {
            value_base[i] = erf(args[i]);
        }
        end_time = clock();
        time[2] = end_time - start_time;
        for (int i = 0; i < n; i++) {
            args[i] = (float)args_d[i];
        }
        for (int i = 0; i < n; i++) {
            value_HA[i] = (float)value_HA_d[i];
        }
        for (int i = 0; i < n; i++) {
            value_EP[i] = (float)value_EP_d[i];
        }
        delete[] value_HA_d;
        delete[] value_EP_d;
        delete[] args_d;
    }
    float max_sub = -1;
    for (int i = 0; i < n; i++) {
        if (abs(value_HA[i] - value_EP[i]) > max_sub) {
            max_sub = abs(value_HA[i] - value_EP[i]);
            max_value[0] = value_HA[i];
            max_value[1] = value_EP[i];
            max_value[2] = args[i];
        }
    }
    delete[] args;
    delete[] value_base;
    delete[] value_EP;
    delete[] value_HA;
}