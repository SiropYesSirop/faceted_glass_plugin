using Core;
using GlassPlugin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace faceted_glass_plugin
{
    public partial class GUI : Form
    {
        /// <summary>
        /// Объект GlassBuilder для построения гранёного стакана
        /// </summary>
        private GlassBuilder _builder;

        //TODO: refactor
        /// <summary>
        /// Параметры гранёного стакана
        /// </summary>
        private Parameters _parameters;

        /// <summary>
        /// Текущее значение радиуса стакана
        /// </summary>
        private double _radiusCurrent;

        /// <summary>
        /// Текущее значение высоты дна стакана
        /// </summary>
        private double _heightBottomCurrent;

        /// <summary>
        /// Текущее значение толщины верхней стенки стакана
        /// </summary>
        private double _thicknessUpperEdgeCurrent;

        /// <summary>
        /// Текущее значение высоты верхней стенки стакана
        /// </summary>
        private double _heightUpperEdgeCurrent;

        /// <summary>
        /// Текущее количетства граней стакана
        /// </summary>
        private double _numberOfEdgeCurrent;

        /// <summary>
        /// Словарь сообщений об ошибках для параметров
        /// </summary>
        private Dictionary<ParameterType, string> _errorMessages;

        /// <summary>
        /// Список связок параметров с элементами управления
        /// </summary>
        private List<ParameterControlBinding> _parameterBindings;

        /// <summary>
        /// Конструктор формы GUI
        /// </summary>
        public GUI()
        {
            InitializeComponent();
            InitializeErrorMessages();
        }

        /// <summary>
        /// Инициализирует словарь сообщений об ошибках
        /// </summary>
        private void InitializeErrorMessages()
        {
            _errorMessages = new Dictionary<ParameterType, string>
            {
                [ParameterType.HeightTotal] = "Неверно введено значение" +
                    " в поле 'Общая высота'!",
                [ParameterType.Radius] = "Неверно введено значение" +
                    " в поле 'Радиус'!",
                [ParameterType.HeightBottom] = "Неверно введено значение" +
                    " в поле 'Высота дна'!",
                [ParameterType.ThicknessLowerEdge] = "Неверно введено" +
                    " значение в поле 'Толщина нижней стенки'!",
                [ParameterType.ThicknessUpperEdge] = "Неверно введено" +
                    " значение в поле 'Толщина верхней стенки'!",
                [ParameterType.HeightUpperEdge] = "Неверно введено " +
                    "значение в поле 'Высота верхней стенки'!",
                [ParameterType.NumberOfEdge] = "Неверно введено значение" +
                    " в поле 'Количество граней'!"
            };
        }

        /// <summary>
        /// Класс для связки параметра с элементами управления
        /// </summary>
        private class ParameterControlBinding
        {
            //TODO: XML
            public ParameterType Type { get; set; }
            public TextBox TextBox { get; set; }
            public Label LimitLabel { get; set; }
            public Label ErrorLabel { get; set; }
            public double CurrentValue { get; set; }
        }

        /// <summary>
        /// Обработчик события загрузки формы
        /// </summary>
        private void GUI_Load(object sender, EventArgs e)
        {
            _parameters = new Parameters();
            _builder = new GlassBuilder();

            InitializeParameterBindings();
            UpdateAllLabels();
            SetInitialTextValues();
            InitializeEdgeTypeComboBox();
        }

        /// <summary>
        /// Инициализация ComboBox для выбора типа грани
        /// </summary>
        private void InitializeEdgeTypeComboBox()
        {
            // Устанавливаем выбранный тип из параметров
            switch (_parameters.EdgeType)
            {
                case EdgeType.Rectangular:
                    comboBoxEdgeType.SelectedIndex = 0;
                    break;
                case EdgeType.Oval:
                    comboBoxEdgeType.SelectedIndex = 1;
                    break;
                case EdgeType.Trapezoidal:
                    comboBoxEdgeType.SelectedIndex = 2;
                    break;
                default:
                    comboBoxEdgeType.SelectedIndex = 0;
                    _parameters.EdgeType = EdgeType.Rectangular;
                    break;
            }

            // Подписываемся на событие изменения выбора
            comboBoxEdgeType.SelectedIndexChanged += 
                comboBoxEdgeType_SelectedIndexChanged;
        }


        /// <summary>
        /// Инициализирует связки параметров с элементами управления
        /// </summary>
        private void InitializeParameterBindings()
        {
            _parameterBindings = new List<ParameterControlBinding>
            {
                new ParameterControlBinding
                {
                    Type = ParameterType.HeightTotal,
                    TextBox = textBoxHeightTotal,
                    LimitLabel = labelLimitHeightTotal,
                    ErrorLabel = labelHeightTotal,
                    CurrentValue = 0
                },
                new ParameterControlBinding
                {
                    Type = ParameterType.Radius,
                    TextBox = textBoxRadius,
                    LimitLabel = labelLimitRadius,
                    ErrorLabel = labelRadius,
                    CurrentValue = 0
                },
                new ParameterControlBinding
                {
                    Type = ParameterType.HeightBottom,
                    TextBox = textBoxHeightBottom,
                    LimitLabel = labelLimitHeightBottom,
                    ErrorLabel = labelHeightBottom,
                    CurrentValue = 0
                },
                new ParameterControlBinding
                {
                    Type = ParameterType.ThicknessLowerEdge,
                    TextBox = textBoxThicknessLowerEdge,
                    LimitLabel = labelLimitThicknessLowerEdge,
                    ErrorLabel = labelThicknessLowerEdge,
                    CurrentValue = 0
                },
                new ParameterControlBinding
                {
                    Type = ParameterType.ThicknessUpperEdge,
                    TextBox = textBoxThicknessUpperEdge,
                    LimitLabel = labelLimitThicknessUpperEdge,
                    ErrorLabel = labelThicknessUpperEdge,
                    CurrentValue = 0
                },
                new ParameterControlBinding
                {
                    Type = ParameterType.HeightUpperEdge,
                    TextBox = textBoxHeightUpperEdge,
                    LimitLabel = labelLimitHeightUpperEdge,
                    ErrorLabel = labelHeightUpperEdge,
                    CurrentValue = 0
                },
                new ParameterControlBinding
                {
                    Type = ParameterType.NumberOfEdge,
                    TextBox = textBoxNumberOfEdge,
                    LimitLabel = labelLimitNumberOfEdges,
                    ErrorLabel = labelNumberOfEdge,
                    CurrentValue = 0
                }
            };
        }

        /// <summary>
        /// Обновляет все метки с ограничениями параметров
        /// </summary>
        private void UpdateAllLabels()
        {
            foreach (var binding in _parameterBindings)
            {
                var param = _parameters.NumericalParameters[binding.Type];
                string unit = binding.Type ==
                    ParameterType.NumberOfEdge ? "шт." : "мм";
                binding.LimitLabel.Text =
                    //TODO: to const
                    $"от {param.MinValue:F1} до {param.MaxValue:F1} {unit}";
            }
        }

        /// <summary>
        /// Устанавливает начальные значения в текстовые поля
        /// </summary>
        private void SetInitialTextValues()
        {
            foreach (var binding in _parameterBindings)
            {
                var param = _parameters.NumericalParameters[binding.Type];
                binding.TextBox.Text = param.Value.ToString();

                switch (binding.Type)
                {
                    case ParameterType.Radius:
                        _radiusCurrent = param.Value;
                        break;
                    case ParameterType.HeightBottom:
                        _heightBottomCurrent = param.Value;
                        break;
                    case ParameterType.ThicknessUpperEdge:
                        _thicknessUpperEdgeCurrent = param.Value;
                        break;
                    case ParameterType.HeightUpperEdge:
                        _heightUpperEdgeCurrent = param.Value;
                        break;
                    case ParameterType.NumberOfEdge:
                        _numberOfEdgeCurrent = param.Value;
                        break;
                }
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки построения гранёного стакана
        /// </summary>
        private void buttonBuildFacetedGlass_Click(object sender, EventArgs e)
        {
            if (CheckAll())
            {
                try
                {
                    _builder.BuildFacetedGlass(_parameters);

                    foreach (var binding in _parameterBindings)
                    {
                        binding.ErrorLabel.ForeColor = Color.Black;
                    }

                    MessageBox.Show("Построение гранёного стакана начато!",
                        "Информация",
                            MessageBoxButtons.OK, MessageBoxIcon.
                                Information);
                }
                catch (Exception)
                {
                    textBoxError.Text += $"Ошибка при построении стакана!\n";
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля корректно!",
                    "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// Проверка заполненности всех параметров
        /// </summary>
        /// <returns>TRUE, если все параметры заполнены корректно</returns>
        private bool CheckAll()
        {
            var invalidParameters = new List<ParameterType>();

            foreach (var parameter in _parameters.NumericalParameters)
            {
                if (parameter.Value.Value == 0)
                {
                    invalidParameters.Add(parameter.Key);
                }
            }

            if (invalidParameters.Any())
            {
                DisplayErrors(invalidParameters);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Отображает ошибки для некорректных параметров
        /// </summary>
        /// <param name="invalidParameters">Список некорректных
        /// параметров</param>
        private void DisplayErrors(List<ParameterType> invalidParameters)
        {
            textBoxError.Text = "";

            foreach (var paramType in invalidParameters)
            {
                if (_errorMessages.ContainsKey(paramType))
                {
                    textBoxError.Text += _errorMessages[paramType] + "\n";
                }
            }
        }

        //TODO: refactor +
        /// <summary>
        /// Общий обработчик потери фокуса для числовых параметров
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="paramType">Тип параметра</param>
        /// <param name="currentValue">Ссылка на поле с текущим 
        /// значением</param>
        /// <param name="label">Связанная метка</param>
        private void NumericParameter_Leave(object sender,
            ParameterType paramType, ref double currentValue, Label label)
        {
            try
            {
                textBoxError.Text = "";
                TextBox textBox = (TextBox)sender;

                if (textBox.Text != "")
                {
                    double value = 0;
                    if (double.TryParse(textBox.Text, out value)
                        && value != 0)
                    {
                        _parameters.NumericalParameters[paramType].Value
                            = value;
                        currentValue = value;
                        textBox.ForeColor = Color.Black;
                        label.ForeColor = Color.Black;
                        CheckParametersBeforeBuilding();
                    }
                    else
                    {
                        textBoxError.Text += $"В поле" +
                            $" '{label.Text.Replace(":", "")}' " +
                            $"было введено некорректное значение!\n";
                    }
                }
                else
                {
                    if (currentValue != 0)
                    {
                        textBox.Text = currentValue.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ((TextBox)sender).ForeColor = Color.Red;
                CheckParametersBeforeBuilding();
                textBoxError.Text += $"Ошибка при вводе: {ex.Message}\n";
            }
        }

        //TODO: refactor
        /// <summary>
        /// Обработчик потери фокуса полем ввода количества граней
        /// </summary>
        private void textBoxNumberOfEdge_Leave(object sender, EventArgs e)
        {
            NumericParameter_Leave(sender, ParameterType.NumberOfEdge,
                ref _numberOfEdgeCurrent, labelNumberOfEdge);
        }

        /// <summary>
        /// Обработчик потери фокуса полем ввода высоты верхней стенки
        /// </summary>
        private void textBoxHeightUpperEdge_Leave(object sender, EventArgs e)
        {
            NumericParameter_Leave(sender, ParameterType.HeightUpperEdge,
                ref _heightUpperEdgeCurrent, labelHeightUpperEdge);
        }

        /// <summary>
        /// Обработчик потери фокуса полем ввода толщины верхней стенки
        /// </summary>
        private void textBoxThicknessUpperEdge_Leave(object sender, EventArgs e)
        {
            NumericParameter_Leave(sender, ParameterType.ThicknessUpperEdge,
                ref _thicknessUpperEdgeCurrent, labelThicknessUpperEdge);
        }

        /// <summary>
        /// Обработчик потери фокуса полем ввода толщины нижней стенки
        /// </summary>
        private void textBoxThicknessLowerEdge_Leave(object sender,
            EventArgs e)
        {
            try
            {
                textBoxError.Text = "";
                double value = Convert.ToDouble
                    (textBoxThicknessLowerEdge.Text);
                _parameters.NumericalParameters
                    [ParameterType.ThicknessLowerEdge].Value = value;
                textBoxThicknessLowerEdge.ForeColor = Color.Black;
                labelThicknessLowerEdge.ForeColor = Color.Black;

                _parameters.SetDependencies(
                    _parameters.NumericalParameters
                    [ParameterType.ThicknessLowerEdge],
                    _parameters.NumericalParameters
                    [ParameterType.ThicknessUpperEdge],
                    2.0,
                    1.4
                );
                CheckDepended(textBoxThicknessUpperEdge,
                    ParameterType.ThicknessUpperEdge,
                        "Толщина верхней стенки");

                var binding = _parameterBindings
                    .First(b => b.Type == ParameterType.ThicknessUpperEdge);
                var upperParam = _parameters.NumericalParameters
                    [ParameterType.ThicknessUpperEdge];
                //TODO: to const
                binding.LimitLabel.Text = $"от {upperParam.MinValue:F1} до "+
                    $"{upperParam.MaxValue:F1} мм";

                CheckParametersBeforeBuilding();
            }
            catch (Exception ex)
            {
                textBoxThicknessLowerEdge.ForeColor = Color.Red;
                CheckParametersBeforeBuilding();
                textBoxError.Text += $"Ошибка при вводе " +
                    $"толщины нижней стенки: {ex.Message}\n";
            }
        }

        /// <summary>
        /// Обработчик потери фокуса полем ввода высоты дна
        /// </summary>
        private void textBoxHeightBottom_Leave(object sender, EventArgs e)
        {
            NumericParameter_Leave(sender, ParameterType.HeightBottom,
                ref _heightBottomCurrent, labelHeightBottom);
        }

        /// <summary>
        /// Обработчик потери фокуса полем ввода радиуса
        /// </summary>
        private void textBoxRadius_Leave(object sender, EventArgs e)
        {
            NumericParameter_Leave(sender, ParameterType.Radius,
                ref _radiusCurrent, labelRadius);
        }

        /// <summary>
        /// Проверка зависимых параметров
        /// </summary>
        /// <param name="target">Ссылка на текстовое 
        /// поле для парсинга</param>
        /// <param name="parametertype">Тип зависимого параметра</param>
        /// <param name="target_label">Имя параметра для
        /// отображения в окне ошибок</param>
        private void CheckDepended(object target,
            //TODO: RSDN
            ParameterType parametertype, string target_label)
        {
            Control target_control = (Control)target;
            try
            {
                if (target_control.Text != "")
                {
                    double value = 0;
                    if (double.TryParse(target_control.Text, out value))
                    {
                        _parameters.NumericalParameters
                            [parametertype].Value = value;
                    }
                    else
                    {
                        textBoxError.Text += $"Введено" +
                            $" некорректное значение!\n";
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Введено некорректное значение!")
                {
                    textBoxError.Text += $"В поле '{target_label}'" +
                        $" было введено неккоректное значение!\n";
                }

                if (ex.Message == "Введённое значение слишком маленькое!")
                {
                    textBoxError.Text += $" В поле '{target_label}'" +
                        $" было введено значение, что меньше диапазона" +
                            $" допустимых значений!\n";
                }

                if (ex.Message == "Введённое значение слишком большое!")
                {
                    textBoxError.Text += $" В поле '{target_label}' " +
                        $"было введено значение, что больше " +
                            $"диапазона допустимых значений!\n";
                }
            }
        }

        /// <summary>
        /// Обработчик потери фокуса полем ввода общей высоты стакана
        /// </summary>
        private void textBoxHeightTotal_Leave(object sender, EventArgs e)
        {
            try
            {
                textBoxError.Text = "";
                double value = Convert.ToDouble(textBoxHeightTotal.Text);
                _parameters.NumericalParameters
                    [ParameterType.HeightTotal].Value = value;
                textBoxHeightTotal.ForeColor = Color.Black;
                labelHeightTotal.ForeColor = Color.Black;
                CheckParametersBeforeBuilding();

                _parameters.SetDependencies(
                    _parameters.NumericalParameters
                        [ParameterType.HeightTotal],
                    _parameters.NumericalParameters
                        [ParameterType.HeightUpperEdge],
                    0.5,
                    0.2
                );
                CheckDepended(textBoxHeightUpperEdge,
                    ParameterType.HeightUpperEdge, "Высота верхней грани");

                var binding = _parameterBindings
                    .First(b => b.Type == ParameterType.HeightUpperEdge);
                var heightUpperEdgeParam = _parameters.
                    NumericalParameters[ParameterType.HeightUpperEdge];
                binding.LimitLabel.Text = $"от " +
                    $"{heightUpperEdgeParam.MinValue:F1} до " +
                        $"{heightUpperEdgeParam.MaxValue:F1} мм";
            }
            catch (Exception ex)
            {
                textBoxHeightTotal.ForeColor = Color.Red;
                CheckParametersBeforeBuilding();
                textBoxError.Text += $"Ошибка при вводе" +
                    $" общей высоты стакана: {ex.Message}\n";
            }
        }

        /// <summary>
        /// Проверяет корректность параметров перед построением
        /// и управляет доступностью кнопки
        /// </summary>
        private void CheckParametersBeforeBuilding()
        {
            buttonBuildFacetedGlass.Enabled = !_parameterBindings
                .Any(b => b.TextBox.ForeColor == Color.Red);
        }

        //TODO: XML
        private void comboBoxEdgeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TODO: {}
            switch (comboBoxEdgeType.SelectedIndex)
            {
                case 0: // Прямоугольная
                    _parameters.EdgeType = EdgeType.Rectangular;
                    break;
                case 1: // Овальная
                    _parameters.EdgeType = EdgeType.Oval;
                    break;
                case 2: // Трапециевидная
                    _parameters.EdgeType = EdgeType.Trapezoidal;
                    break;
            }

            // Опционально: показываем подсказку
            textBoxError.Text = $"Выбран тип грани: {comboBoxEdgeType.SelectedItem}";

            // Проверяем, можно ли строить с текущими параметрами
            CheckParametersBeforeBuilding();
        }
    }
}