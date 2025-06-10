using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Сhemical_Complex
{
    public class MathModule
    {
        private Stopwatch _calculationStopwatch;
        private double _temp, _c2, _c1, _f, _qCH, _gamma, _qGamma, _bCoef, _qAlpha;

        private double _step;

        private double _length;
        private double _width;
        private double _height;

        private double _coverSpeed;
        private double _coverT;

        private double _density;
        private double _averageHeatCapacity;
        private double _meltingT;
        private double _glassTransitionT;

        private double _firstConstWLF;
        private double _secondConstWLF;
        private double _consistencyCoef;
        private double _castingT;
        private double _flowIndex;
        private double _heatTransferTCoef;

        private double Temp => (_meltingT + (_glassTransitionT + 100)) / 2;
        private double C2 => _secondConstWLF + _castingT - _glassTransitionT;
        private double C1 => _firstConstWLF * _secondConstWLF / C2;

        public double F => 0.125 * Math.Pow(_height / _width, 2) - 0.625 * _height / _width + 1;
        public double QCH => (_height * _width * _coverSpeed) / 2 * F;
        public double Gamma => _coverSpeed / _height;
        public double QGamma => _height * _width * _consistencyCoef * Math.Pow(Gamma, _flowIndex + 1);
        private double bCoef => C1 / (C2 + (Temp - _castingT));
        public double QAlpha => _width * _heatTransferTCoef * (Math.Pow(bCoef, -1) - _coverT + _castingT);
        public double AmountSteps => Math.Round(_length / _step);
        public double Q => Math.Round(3600 * _density * QCH);

        public List<double> ProductT;
        public List<double> ProductViscosity;

        public double LastT => ProductT.Last();
        public double LastViscosity => ProductViscosity.Last();
        public TimeSpan CalculationTime { get; private set; }

        public long MemoryUsedBytes { get; private set; }

        public long OperationCount { get; private set; }

        public void SetCoverSpeed(double speed)
        {
            _coverSpeed = speed;
        }

        private void ComputeConstants()
        {
            // Расчёт Temp
            _temp = (_meltingT + (_glassTransitionT + 100)) / 2;
            OperationCount += 3; // 2 сложения + деление

            // Расчёт C2
            _c2 = _secondConstWLF + _castingT - _glassTransitionT;
            OperationCount += 2; // сложение + вычитание

            // Расчёт C1
            _c1 = (_firstConstWLF * _secondConstWLF) / _c2;
            OperationCount += 2; // умножение + деление

            // Расчёт F
            double hwRatio = _height / _width;
            OperationCount += 1; // деление
            _f = 0.125 * Math.Pow(hwRatio, 2) - 0.625 * hwRatio + 1;
            OperationCount += 5; // Pow(2) + 2 умножения + вычитание + сложение

            // Расчёт QCH
            _qCH = (_height * _width * _coverSpeed) / 2 * _f;
            OperationCount += 4; // 3 умножения + деление

            // Расчёт Gamma
            _gamma = _coverSpeed / _height;
            OperationCount += 1; // деление

            // Расчёт QGamma
            _qGamma = _height * _width * _consistencyCoef * Math.Pow(_gamma, _flowIndex + 1);
            OperationCount += 4; // 3 умножения + Pow()

            // Расчёт bCoef
            _bCoef = _c1 / (_c2 + (_temp - _castingT));
            OperationCount += 3; // сложение + вычитание + деление

            // Расчёт QAlpha
            _qAlpha = _width * _heatTransferTCoef * (Math.Pow(_bCoef, -1) - _coverT + _castingT);
            OperationCount += 5; // Pow(-1) + 2 умножения + вычитание + сложение
        }

        public MathModule(double[] values)
        {
            _step = values[0];
            _length = values[1];
            _width = values[2];
            _height = values[3];

            _coverSpeed = values[4];
            _coverT = values[5];

            _density = values[6];
            _averageHeatCapacity = values[7];
            _meltingT = values[8];
            _glassTransitionT = values[9];

            _consistencyCoef = values[10];
            _firstConstWLF = values[11];
            _secondConstWLF = values[12];
            _castingT = values[13];
            _heatTransferTCoef = values[14];
            _flowIndex = values[15];

            ProductT = new List<double>();
            ProductViscosity = new List<double>();
        }

        public void CalculateOutputParameters()
        {
            OperationCount = 0;
            ComputeConstants();
            _calculationStopwatch = Stopwatch.StartNew();


            for (int i = 0; i <= AmountSteps; i++)
            {
                double stepi = _step * i;
                OperationCount += 1; // умножение

                // Вычисление температуры
                double expArg1 = (-_bCoef * _qAlpha * stepi) / (_density * _averageHeatCapacity * _qCH);
                OperationCount += 6; // 3 умножения + деление + унарный минус

                double term1 = (_bCoef * _qGamma + _width * _heatTransferTCoef) / (_bCoef * _qAlpha);
                OperationCount += 5; // 2 умножения + сложение + 2 умножения + деление

                double term2 = 1 - Math.Exp(expArg1);
                OperationCount += 2; // вычитание + Exp()

                double term3 = term1 * term2;
                OperationCount += 1; // умножение

                double expArg2 = _bCoef * (_meltingT - _castingT - (_qAlpha * stepi) / (_density * _averageHeatCapacity * _qCH));
                OperationCount += 7; // 2 умножения + 2 вычитания + деление + 2 умножения

                double term4 = Math.Exp(expArg2);
                OperationCount += 1; // Exp()

                double logArg = term3 + term4;
                OperationCount += 1; // сложение

                double temperature = _castingT + (1 / _bCoef) * Math.Log(logArg);
                OperationCount += 4; // деление + умножение + Log() + сложение

                double viscArg1 = -_bCoef * (temperature - _castingT);
                OperationCount += 2; // вычитание + умножение

                double viscTerm1 = Math.Exp(viscArg1);
                OperationCount += 1; // Exp()

                double viscTerm2 = Math.Pow(_gamma, _flowIndex - 1);
                OperationCount += 1; // Pow()

                double viscosity = _consistencyCoef * viscTerm1 * viscTerm2;
                OperationCount += 2; // 2 умножения

                ProductT.Add(Math.Round(temperature * 100) / 100);
                ProductViscosity.Add(Math.Round(viscosity * 100) / 100);
            }

            _calculationStopwatch.Stop();
            CalculationTime = _calculationStopwatch.Elapsed;
        }
    }
}
