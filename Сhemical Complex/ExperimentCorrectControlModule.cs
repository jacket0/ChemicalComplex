using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Сhemical_Complex
{
    public class ExperimentCorrectControlModule
    {
        private List<string> _errors = new List<string>();
        private double[] _correctValues;

        public IReadOnlyList<string> Errors => _errors.AsReadOnly();
        public bool IsValid => !_errors.Any();
        public double[] CorrectValues => IsValid ? _correctValues : null;

        public ExperimentCorrectControlModule(string minSpeedInput, string maxSpeedInput, string stepInput)
        {
            ValidateInputs(minSpeedInput, maxSpeedInput, stepInput);
            if (_errors.Any()) return;

            ConvertToDoubleArray(minSpeedInput, maxSpeedInput, stepInput);
        }

        private void ValidateInputs(string minSpeedInput, string maxSpeedInput, string stepInput)
        {
            // Проверка на пустые значения
            if (string.IsNullOrWhiteSpace(minSpeedInput))
                _errors.Add("Минимальная скорость покрытия: Поле не заполнено.");

            if (string.IsNullOrWhiteSpace(maxSpeedInput))
                _errors.Add("Максимальная скорость покрытия: Поле не заполнено.");

            if (string.IsNullOrWhiteSpace(stepInput))
                _errors.Add("Шаг варьирования: Поле не заполнено.");

            if (_errors.Any()) return;

            // Валидация числовых значений
            ValidateNonNegativeDouble(minSpeedInput, "Минимальная скорость покрытия");
            ValidateNonNegativeDouble(maxSpeedInput, "Максимальная скорость покрытия");
            ValidatePositiveDouble(stepInput, "Шаг варьирования");

            if (_errors.Any()) return;

            // Конвертация для проверки логических соотношений
            if (TryParseDouble(minSpeedInput, out double minSpeed) &&
                TryParseDouble(maxSpeedInput, out double maxSpeed) &&
                TryParseDouble(stepInput, out double step))
            {
                // Проверка minSpeed <= maxSpeed
                if (minSpeed > maxSpeed)
                {
                    _errors.Add("Минимальная скорость покрытия не может превышать максимальную.");
                }
                // Проверка шага
                else if (step > (maxSpeed - minSpeed))
                {
                    _errors.Add("Шаг варьирования превышает разницу между максимальной и минимальной скоростью.");
                }
            }
        }

        private void ConvertToDoubleArray(string minSpeedInput, string maxSpeedInput, string stepInput)
        {
            _correctValues = new double[3];
            if (TryParseDouble(minSpeedInput, out _correctValues[0]) &&
                TryParseDouble(maxSpeedInput, out _correctValues[1]) &&
                TryParseDouble(stepInput, out _correctValues[2]))
            {
                // Конвертация успешна
            }
            else
            {
                _errors.Add("Неожиданная ошибка конвертации параметров.");
            }
        }

        private void ValidateNonNegativeDouble(string input, string paramName)
        {
            if (!TryParseDouble(input, out double value))
            {
                _errors.Add($"{paramName}: Некорректное числовое значение.");
                return;
            }

            if (value < 0)
            {
                _errors.Add($"{paramName}: Значение не может быть отрицательным.");
            }
        }

        private void ValidatePositiveDouble(string input, string paramName)
        {
            if (!TryParseDouble(input, out double value))
            {
                _errors.Add($"{paramName}: Некорректное числовое значение.");
                return;
            }

            if (value <= 0)
            {
                _errors.Add($"{paramName}: Значение должно быть положительным.");
            }
        }

        private bool TryParseDouble(string input, out double value)
        {
            input = input.Replace(',', '.');
            return double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out value);
        }
    }
}