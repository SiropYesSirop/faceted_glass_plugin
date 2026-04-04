using Core;
using GlassPlugin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace faceted_glass_plugin
{
    public partial class GUI : Form
    {
        private GlassBuilder _builder;
        private Parameters _parameters;
        private List<ParameterControlBinding> _parameterBindings;
        private Dictionary<ParameterType, string> _errorMessages;

        /// <summary>
        /// Словарь: TextBox -> ParameterType
        /// Позволяет обрабатывать все TextBox одним методом
        /// </summary>
        private Dictionary<TextBox, ParameterType> _textBoxToParameterType;

        /// <summary>
        /// Словарь: TextBox -> Label для отображения ошибок
        /// </summary>
        private Dictionary<TextBox, Label> _textBoxToErrorLabel;

        public GUI()
        {
            InitializeComponent();
            InitializeErrorMessages();
        }

        private void GUI_Load(object sender, EventArgs e)
        {
            _parameters = new Parameters();
            _builder = new GlassBuilder();

            InitializeDictionaries();
            InitializeParameterBindings();
            UpdateAllLabels();
            SetInitialTextValues();
            InitializeEdgeTypeComboBox();
            SubscribeToTextBoxEvents();
        }

        /// <summary>
        /// Подписывает все TextBox на единый обработчик
        /// </summary>
        private void SubscribeToTextBoxEvents()
        {
            foreach (var textBox in _textBoxToParameterType.Keys)
            {
                textBox.Leave += NumericTextBox_Leave;
            }
        }

        /// <summary>
        /// Инициализирует словари для связи TextBox с ParameterType и Label
        /// </summary>
        private void InitializeDictionaries()
        {
            _textBoxToParameterType = new Dictionary<TextBox, ParameterType>
            {
                [textBoxHeightTotal] = ParameterType.HeightTotal,
                [textBoxRadius] = ParameterType.Radius,
                [textBoxHeightBottom] = ParameterType.HeightBottom,
                [textBoxThicknessLowerEdge] = ParameterType.
                    ThicknessLowerEdge,
                [textBoxThicknessUpperEdge] = ParameterType.
                    ThicknessUpperEdge,
                [textBoxHeightUpperEdge] = ParameterType.HeightUpperEdge,
                [textBoxNumberOfEdge] = ParameterType.NumberOfEdge
            };

            _textBoxToErrorLabel = new Dictionary<TextBox, Label>
            {
                [textBoxHeightTotal] = labelHeightTotal,
                [textBoxRadius] = labelRadius,
                [textBoxHeightBottom] = labelHeightBottom,
                [textBoxThicknessLowerEdge] = labelThicknessLowerEdge,
                [textBoxThicknessUpperEdge] = labelThicknessUpperEdge,
                [textBoxHeightUpperEdge] = labelHeightUpperEdge,
                [textBoxNumberOfEdge] = labelNumberOfEdge
            };
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
                [ParameterType.ThicknessLowerEdge] = "Неверно введено значение" +
                    " в поле 'Толщина нижней стенки'!",
                [ParameterType.ThicknessUpperEdge] = "Неверно введено значение" +
                    " в поле 'Толщина верхней стенки'!",
                [ParameterType.HeightUpperEdge] = "Неверно введено значение" +
                    " в поле 'Высота верхней стенки'!",
                [ParameterType.NumberOfEdge] = "Неверно введено значение" +
                    " в поле 'Количество граней'!"
            };
        }

        /// <summary>
        /// Инициализация ComboBox для выбора типа грани
        /// </summary>
        private void InitializeEdgeTypeComboBox()
        {
            switch (_parameters.EdgeType)
            {
                case EdgeType.Rectangular:
                {
                    comboBoxEdgeType.SelectedIndex = 0;
                    break;
                }
                case EdgeType.Oval:
                {
                    comboBoxEdgeType.SelectedIndex = 1;
                    break;
                }
                case EdgeType.Trapezoidal:
                {
                    comboBoxEdgeType.SelectedIndex = 2;
                    break;
                }
                default:
                {
                    comboBoxEdgeType.SelectedIndex = 0;
                    _parameters.EdgeType = EdgeType.Rectangular;
                    break;
                }
            }
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
                    ErrorLabel = labelHeightTotal
                },
                new ParameterControlBinding
                {
                    Type = ParameterType.Radius,
                    TextBox = textBoxRadius,
                    LimitLabel = labelLimitRadius,
                    ErrorLabel = labelRadius
                },
                new ParameterControlBinding
                {
                    Type = ParameterType.HeightBottom,
                    TextBox = textBoxHeightBottom,
                    LimitLabel = labelLimitHeightBottom,
                    ErrorLabel = labelHeightBottom
                },
                new ParameterControlBinding
                {
                    Type = ParameterType.ThicknessLowerEdge,
                    TextBox = textBoxThicknessLowerEdge,
                    LimitLabel = labelLimitThicknessLowerEdge,
                    ErrorLabel = labelThicknessLowerEdge
                },
                new ParameterControlBinding
                {
                    Type = ParameterType.ThicknessUpperEdge,
                    TextBox = textBoxThicknessUpperEdge,
                    LimitLabel = labelLimitThicknessUpperEdge,
                    ErrorLabel = labelThicknessUpperEdge
                },
                new ParameterControlBinding
                {
                    Type = ParameterType.HeightUpperEdge,
                    TextBox = textBoxHeightUpperEdge,
                    LimitLabel = labelLimitHeightUpperEdge,
                    ErrorLabel = labelHeightUpperEdge
                },
                new ParameterControlBinding
                {
                    Type = ParameterType.NumberOfEdge,
                    TextBox = textBoxNumberOfEdge,
                    LimitLabel = labelLimitNumberOfEdges,
                    ErrorLabel = labelNumberOfEdge
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
                binding.LimitLabel.Text = _parameters.
                    GetRangeString(binding.Type);
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
            }
        }

        /// <summary>
        /// Универсальный обработчик для всех TextBox
        /// </summary>
        private void NumericTextBox_Leave(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;
            var paramType = _textBoxToParameterType[textBox];
            Label errorLabel = null;

            if (_textBoxToErrorLabel.ContainsKey(textBox))
            {
                errorLabel = _textBoxToErrorLabel[textBox];
            }
            try
            {
                textBoxError.Text = "";

                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = _parameters.GetParameter(paramType)
                        .Value.ToString();
                    return;
                }

                if (double.TryParse(textBox.Text, out double value))
                {
                    _parameters.GetParameter(paramType).Value = value;
                    textBox.ForeColor = Color.Black;
                    if (errorLabel != null)
                    {
                        errorLabel.ForeColor = Color.Black;
                    }
                }
                else
                {
                    throw new FacetedGlassException(
                        FacetedGlassExceptionType.InvalidGlassParameters,
                            $"В поле '{GetParameterDisplayName(paramType)}" +
                                $"' введено некорректное значение!"
                    );
                }
                CheckParametersBeforeBuilding();
            }
            catch (FacetedGlassException ex)
            {
                HandleFacetedGlassException(ex, textBox, errorLabel);
            }
            catch (Exception ex)
            {
                textBox.ForeColor = Color.Red;
                if (errorLabel != null)
                { 
                    errorLabel.ForeColor = Color.Red;
                }
                textBoxError.Text += $"Ошибка: {ex.Message}\n";
                CheckParametersBeforeBuilding();
            }
        }

        /// <summary>
        /// Обработка специализированных исключений гранёного стакана
        /// </summary>
        private void HandleFacetedGlassException
            (FacetedGlassException ex, TextBox textBox, Label errorLabel)
        {
            textBox.ForeColor = Color.Red;
            if (errorLabel != null)
            {
                errorLabel.ForeColor = Color.Red;
            }

            string userMessage = GetUserMessageByExceptionType
                (ex.ExceptionType);

            if (string.IsNullOrEmpty(userMessage))
            {
                userMessage = ex.Message;
            }

            textBoxError.Text += userMessage + "\n";
            CheckParametersBeforeBuilding();
        }

        /// <summary>
        /// Получает сообщение по типу исключения
        /// </summary>
        private string GetUserMessageByExceptionType
            (FacetedGlassExceptionType exceptionType)
        {
            switch (exceptionType)
            {
                case FacetedGlassExceptionType.HeightTotalInvalid:
                {
                    return "Общая высота должна быть в" +
                        " диапазоне от 100 до 150 мм";
                }
                case FacetedGlassExceptionType.RadiusInvalid:
                {
                    return "Радиус должен быть в диапазоне от 45 до 60 мм";
                }
                case FacetedGlassExceptionType.HeightBottomInvalid:
                {
                    return "Высота дна должна быть в" +
                        " диапазоне от 10 до 25 мм";
                }
                case FacetedGlassExceptionType.ThicknessLowerEdgeInvalid:
                {
                    return "Толщина нижней стенки должна быть в" +
                        " диапазоне от 2 до 5 мм";
                }
                case FacetedGlassExceptionType.ThicknessUpperEdgeInvalid:
                {
                    return "Толщина верхней стенки должна быть в" +
                        " диапазоне от 4 до 7 мм";
                }
                case FacetedGlassExceptionType.HeightUpperEdgeInvalid:
                {
                    return "Высота верхней стенки должна быть положительной";
                }
                case FacetedGlassExceptionType.NumberOfEdgesInvalid:
                {
                    return "Количество граней должно быть от 8 до 11";
                }
                case FacetedGlassExceptionType.KompasConnectionFailed:
                {
                    return "Не удалось подключиться к КОМПАС-3D." +
                        " Проверьте, что программа установлена и запущена.";
                }
                case FacetedGlassExceptionType.InvalidGlassParameters:
                {
                    return "Некорректные параметры стакана";
                }
                default:
                {
                    return $"Неизвестная ошибка (код: {exceptionType})";
                }
            }
        }

        /// <summary>
        /// Получает отображаемое имя параметра для сообщений об ошибках
        /// </summary>
        private string GetParameterDisplayName(ParameterType paramType)
        {
            switch (paramType)
            {
                case ParameterType.HeightTotal:
                {
                    return "Общая высота";
                }
                case ParameterType.Radius:
                {
                    return "Радиус";
                }
                case ParameterType.HeightBottom:
                {
                    return "Высота дна";
                }
                case ParameterType.ThicknessLowerEdge:
                {
                    return "Толщина нижней стенки";
                }
                case ParameterType.ThicknessUpperEdge:
                {
                    return "Толщина верхней стенки";
                }
                case ParameterType.HeightUpperEdge:
                {
                    return "Высота верхней стенки";
                }
                case ParameterType.NumberOfEdge:
                {
                    return "Количество граней";
                }
                default:
                    return paramType.ToString();
            }
        }

        /// <summary>
        /// Обработчик для поля толщины нижней стенки
        /// </summary>
        private void TextBoxThicknessLowerEdge_Leave
            (object sender, EventArgs e)
        {
            try
            {
                textBoxError.Text = "";

                if (!double.TryParse(textBoxThicknessLowerEdge.Text,
                    out double value))
                {
                    throw new FacetedGlassException(
                        FacetedGlassExceptionType.InvalidGlassParameters,
                        "Некорректное значение толщины нижней стенки"
                    );
                }

                _parameters.GetParameter(ParameterType.ThicknessLowerEdge)
                    .Value = value;
                textBoxThicknessLowerEdge.ForeColor = Color.Black;
                labelThicknessLowerEdge.ForeColor = Color.Black;

                _parameters.SetDependencies(
                    _parameters.GetParameter
                        (ParameterType.ThicknessLowerEdge),
                    _parameters.GetParameter
                        (ParameterType.ThicknessUpperEdge),
                    2.0, 1.4
                );

                UpdateParameterDisplay(ParameterType.ThicknessUpperEdge);
                ValidateDependedParameter(textBoxThicknessUpperEdge,
                    ParameterType.ThicknessUpperEdge,
                        "Толщина верхней стенки");

                CheckParametersBeforeBuilding();
            }
            catch (FacetedGlassException ex)
            {
                HandleFacetedGlassException(ex,
                    textBoxThicknessLowerEdge, labelThicknessLowerEdge);
            }
            catch (Exception ex)
            {
                textBoxThicknessLowerEdge.ForeColor = Color.Red;
                textBoxError.Text += $"Ошибка: {ex.Message}\n";
                CheckParametersBeforeBuilding();
            }
        }

        /// <summary>
        /// Обработчик для поля общей высоты
        /// </summary>
        private void TextBoxHeightTotal_Leave(object sender, EventArgs e)
        {
            try
            {
                textBoxError.Text = "";

                if (!double.TryParse(textBoxHeightTotal.Text,
                    out double value))
                {
                    throw new FacetedGlassException(
                        FacetedGlassExceptionType.HeightTotalInvalid,
                        "Некорректное значение общей высоты"
                    );
                }

                _parameters.GetParameter(ParameterType.HeightTotal)
                    .Value = value;
                textBoxHeightTotal.ForeColor = Color.Black;
                labelHeightTotal.ForeColor = Color.Black;

                _parameters.SetDependencies(
                    _parameters.GetParameter(ParameterType.HeightTotal),
                    _parameters.GetParameter(ParameterType.HeightUpperEdge),
                    0.5, 0.2
                );

                UpdateParameterDisplay(ParameterType.HeightUpperEdge);
                ValidateDependedParameter(textBoxHeightUpperEdge,
                    ParameterType.HeightUpperEdge, "Высота верхней стенки");

                CheckParametersBeforeBuilding();
            }
            catch (FacetedGlassException ex)
            {
                HandleFacetedGlassException(ex,
                    textBoxHeightTotal, labelHeightTotal);
            }
            catch (Exception ex)
            {
                textBoxHeightTotal.ForeColor = Color.Red;
                textBoxError.Text += $"Ошибка: {ex.Message}\n";
                CheckParametersBeforeBuilding();
            }
        }

        /// <summary>
        /// Обновляет отображение метки с диапазоном для параметра
        /// </summary>
        private void UpdateParameterDisplay(ParameterType paramType)
        {
            var binding = _parameterBindings.FirstOrDefault
                (b => b.Type == paramType);
            if (binding != null)
            {
                binding.LimitLabel.Text = _parameters.GetRangeString
                    (paramType);
            }
        }

        /// <summary>
        /// Проверяет зависимый параметр на корректность
        /// </summary>
        private void ValidateDependedParameter
            (TextBox textBox, ParameterType paramType, string displayName)
        {
            try
            {
                if (double.TryParse(textBox.Text, out double value))
                {
                    _parameters.GetParameter(paramType).Value = value;
                    textBox.ForeColor = Color.Black;
                }
            }
            catch (FacetedGlassException ex)
            {
                textBox.ForeColor = Color.Red;
                textBoxError.Text += $"Поле '{displayName}': {ex.Message}\n";
            }
        }

        /// <summary>
        /// Проверяет корректность параметров перед
        /// построением и управляет доступностью кнопки
        /// </summary>
        private void CheckParametersBeforeBuilding()
        {
            bool hasErrors = false;
            foreach (var binding in _parameterBindings)
            {
                if (binding.TextBox.ForeColor == Color.Red)
                {
                    hasErrors = true;
                    break;
                }
            }
            buttonBuildFacetedGlass.Enabled = !hasErrors;
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

        /// <summary>
        /// Обработчик нажатия кнопки построения
        /// </summary>
        private void buttonBuildFacetedGlass_Click
            (object sender, EventArgs e)
        {
            if (!CheckAll())
            {
                MessageBox.Show("Пожалуйста, заполните все поля корректно!",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                _builder.BuildFacetedGlass(_parameters);

                foreach (var binding in _parameterBindings)
                {
                    binding.ErrorLabel.ForeColor = Color.Black;
                    binding.TextBox.ForeColor = Color.Black;
                }

                MessageBox.Show("Построение гранёного стакана начато!",
                    "Информация", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
            }
            catch (FacetedGlassException ex)
            {
                textBoxError.Text = GetUserFriendlyBuildMessage(ex);
            }
            catch (Exception ex)
            {
                textBoxError.Text = $"Ошибка при построении стакана:" +
                    $" {ex.Message}\n";
            }
        }

        /// <summary>
        /// Получает понятное пользователю сообщение из
        /// исключения при построении
        /// </summary>
        private string GetUserFriendlyBuildMessage(FacetedGlassException ex)
        {
            switch (ex.ExceptionType)
            {
                case FacetedGlassExceptionType.KompasConnectionFailed:
                {
                    return "Не удалось подключиться к КОМПАС-3D." +
                        "\nУбедитесь, что программа установлена и запущена.";
                }
                case FacetedGlassExceptionType.PartCreationFailed:
                {
                    return "Не удалось создать деталь в КОМПАС-3D.";
                }
                case FacetedGlassExceptionType.TangentPlaneCreationFailed:
                {
                    return "Не удалось создать касательную плоскость." +
                        "\nПроверьте корректность параметров радиуса и высоты.";
                }
                case FacetedGlassExceptionType.CircularArrayCreationFailed:
                {
                    return "Не удалось создать круговой массив граней." +
                        "\nПроверьте количество граней (должно быть от 8 до 11).";
                }
                case FacetedGlassExceptionType.InvalidGlassParameters:
                {
                    return $"Некорректные параметры стакана:\n{ex.Message}";
                }
                default:
                {
                    return $"Ошибка построения: {ex.Message}";
                }
            }
        }

        /// <summary>
        /// Обрабатывает изменение выбранного типа грани
        /// в выпадающем списке
        /// </summary>
        private void comboBoxEdgeType_SelectedIndexChanged
            (object sender, EventArgs e)
        {
            switch (comboBoxEdgeType.SelectedIndex)
            {
                case 0:
                {
                    _parameters.EdgeType = EdgeType.Rectangular;
                    break;
                }
                case 1:
                {
                    _parameters.EdgeType = EdgeType.Oval;
                    break;
                }
                case 2:
                {
                    _parameters.EdgeType = EdgeType.Trapezoidal;
                    break;
                }
            }
            textBoxError.Text = $"Выбран тип грани:" +
                $" {comboBoxEdgeType.SelectedItem}";
            CheckParametersBeforeBuilding();
        }

        /// <summary>
        /// Класс для связки параметра с элементами управления
        /// </summary>
        private class ParameterControlBinding
        {
            public ParameterType Type { get; set; }
            public TextBox TextBox { get; set; }
            public Label LimitLabel { get; set; }
            public Label ErrorLabel { get; set; }
        }
    }
}