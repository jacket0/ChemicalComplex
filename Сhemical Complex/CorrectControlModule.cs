using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Сhemical_Complex
{
    public class CorrectControlModule
    {
        private List<string> _errors = new List<string>();
        private double[] _correctValues;

        public IReadOnlyList<string> Errors => _errors.AsReadOnly();
        public bool IsValid => !_errors.Any();
        public double[] CorrectValues => IsValid ? _correctValues : null;

        public CorrectControlModule(string[] inputs)
        {
            if (inputs == null || inputs.Length != 17)
            {
                _errors.Add("Ошибка: Неверное количество входных параметров.");
                return;
            }

            CheckEmptyValues(inputs);
            if (_errors.Any()) return;

            ValidateParameters(inputs);
            if (_errors.Any()) return;

            ConvertToDoubleArray(inputs.Skip(1).ToArray());
        }

        private void CheckEmptyValues(string[] inputs)
        {
            string[] paramNames =
            {
            "Тип материала",
            "Шаг расчета",
            "Длина канала",
            "Ширина канала",
            "Высота канала",
            "Скорость покрытия",
            "Температура покрытия",
            "Плотность материала",
            "Средняя теплоемкость",
            "Температура плавления",
            "Температура стеклования",
            "Коэффициент консистенции",
            "Первая константа WLF",
            "Вторая константа WLF",
            "Температура литья",
            "Коэффициент теплопередачи",
            "Индекс течения"
        };

            for (int i = 0; i < inputs.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(inputs[i]))
                {
                    _errors.Add($"{paramNames[i]}: Поле не заполнено.");
                }
            }
        }

        private void ValidateParameters(string[] inputs)
        {
            ValidateComboBoxType(inputs[0]);
            ValidatePositiveDouble(inputs[1], "Шаг расчета");
            ValidatePositiveDouble(inputs[2], "Длина канала");
            ValidatePositiveDouble(inputs[3], "Ширина канала");
            ValidatePositiveDouble(inputs[4], "Высота канала");
            ValidateNonNegativeDouble(inputs[5], "Скорость покрытия");
            ValidateTemperature(inputs[6], "Температура покрытия");
            ValidatePositiveDouble(inputs[7], "Плотность материала");
            ValidatePositiveDouble(inputs[8], "Средняя теплоемкость");
            ValidateTemperature(inputs[9], "Температура плавления");
            ValidateTemperature(inputs[10], "Температура стеклования");
            ValidatePositiveDouble(inputs[11], "Коэффициент консистенции");
            ValidateWLFConstant(inputs[12], "Первая константа WLF");
            ValidateWLFConstant(inputs[13], "Вторая константа WLF");
            ValidateTemperature(inputs[14], "Температура литья");
            ValidatePositiveDouble(inputs[15], "Коэффициент теплопередачи");
            ValidateFlowIndex(inputs[16]);

            // Проверка температурных соотношений
            if (TryParseDouble(inputs[9], out double meltingT) &&
                TryParseDouble(inputs[10], out double glassTransitionT) &&
                meltingT <= glassTransitionT)
            {
                _errors.Add("Температура плавления должна быть выше температуры стеклования.");
            }
        }

        private void ConvertToDoubleArray(string[] numericInputs)
        {
            _correctValues = new double[numericInputs.Length];
            for (int i = 0; i < numericInputs.Length; i++)
            {
                if (TryParseDouble(numericInputs[i], out double value))
                {
                    _correctValues[i] = value;
                }
                else
                {
                    _errors.Add($"Неожиданная ошибка конвертации параметра {i + 1}");
                    break;
                }
            }
        }

        private void ValidateComboBoxType(string value)
        {
            //string[] validTypes = { "Полимер A", "Полимер B" }; // Замените на реальные значения
            //if (!validTypes.Contains(value.Trim()))
            //{
            //    _errors.Add("Неверный тип материала. Выберите значение из списка.");
            //}
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

        private void ValidateTemperature(string input, string paramName)
        {
            if (!TryParseDouble(input, out double value))
            {
                _errors.Add($"{paramName}: Некорректное числовое значение.");
                return;
            }

            if (value < -273.15)
            {
                _errors.Add($"{paramName}: Температура не может быть ниже абсолютного нуля (-273.15°C).");
            }
        }

        private void ValidateWLFConstant(string input, string paramName)
        {
            if (!TryParseDouble(input, out double _))
            {
                _errors.Add($"{paramName}: Некорректное числовое значение.");
            }
        }

        private void ValidateFlowIndex(string input)
        {
            if (!TryParseDouble(input, out double value))
            {
                _errors.Add("Индекс течения: Некорректное числовое значение.");
                return;
            }

            if (value <= 0 || value > 1)
            {
                _errors.Add("Индекс течения должен быть в диапазоне (0, 1].");
            }
        }

        private bool TryParseDouble(string input, out double value)
        {
            input = input.Replace(',', '.');
            return double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out value);
        }
    }
}
