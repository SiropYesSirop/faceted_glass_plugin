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
    //TODO: RSDN +
    public partial class GUI : Form
    {
        /// <summary>
        /// Объект GlassBuilder для построения гранёного стакана
        /// </summary>
        private GlassBuilder _builder;

        /// <summary>
        /// Параметры гранёного стакана
        /// </summary>
        private Parameters _parameters;

        //TODO: RSDN +
        /// <summary>
        /// Текущее значение радиуса стакана
        /// </summary>
        private double _radiusCurrent;

        //TODO: RSDN +
        /// <summary>
        /// Текущее значение высоты дна стакана
        /// </summary>
        private double _heightBottomCurrent;

        //TODO: RSDN +
        /// <summary>
        /// Текущее значение толщины верхней стенки стакана
        /// </summary>
        private double _thicknessUpperEdgeCurrent;

        //TODO: RSDN +
        /// <summary>
        /// Текущее значение высоты верхней стенки стакана
        /// </summary>
        private double _heightUpperEdgeCurrent;

        //TODO: RSDN +
        /// <summary>
        /// Текущее количетства граней стакана
        /// </summary>
        private double _numberOfEdgeCurrent;

        /// <summary>
        /// Конструктор формы GUI
        /// </summary>
        public GUI()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик события загрузки формы
        /// </summary>
        private void GUI_Load(object sender, EventArgs e)
        {
            _parameters = new Parameters();
            _builder = new GlassBuilder();

            //TODO: RSDN +
            var heightTotalParam = _parameters.NumericalParameters
                [ParameterType.HeightTotal];
            labelLimitHeightTotal.Text = $"от {heightTotalParam.MinValue:F1}" +
                $" до " + $"{heightTotalParam.MaxValue:F1} мм";
            var radiusParam = _parameters.NumericalParameters
                [ParameterType.Radius];
            labelLimitRadius.Text = $"от {radiusParam.MinValue:F1} " +
                $"до {radiusParam.MaxValue:F1} мм";
            var heightBottomParam = _parameters.NumericalParameters
                [ParameterType.HeightBottom];
            labelLimitHeightBottom.Text = $"от " +
                $"{heightBottomParam.MinValue:F1}" +
                    $" до {heightBottomParam.MaxValue:F1} мм";
            var thicknessLowerEdgeParam = _parameters.NumericalParameters
                [ParameterType.ThicknessLowerEdge];
            labelLimitThicknessLowerEdge.Text = $"от" +
                $" {thicknessLowerEdgeParam.MinValue:F1}" +
                    $" до {thicknessLowerEdgeParam.MaxValue:F1} мм";
            var thicknessUpperEdgeParam = _parameters.NumericalParameters
                [ParameterType.ThicknessUpperEdge];
            labelLimitThicknessUpperEdge.Text = $"от " +
                $"{thicknessUpperEdgeParam.MinValue:F1}" +
                    $" до {thicknessUpperEdgeParam.MaxValue:F1} мм";
            var heightUpperEdgeParam = _parameters.NumericalParameters
                [ParameterType.HeightUpperEdge];
            labelLimitHeightUpperEdge.Text = $"от " +
                $"{heightUpperEdgeParam.MinValue:F1} до " +
                    $"{heightUpperEdgeParam.MaxValue:F1} мм";
            var numberOfEdgeParam = _parameters.NumericalParameters
                [ParameterType.NumberOfEdge];
            labelLimitNumberOfEdges.Text = $"от " +
                $"{numberOfEdgeParam.MinValue:F1} до" +
                    $" {numberOfEdgeParam.MaxValue:F1} шт.";

            //TODO: from parameters +
            textBoxHeightTotal.Text = heightTotalParam
                .Value.ToString();
            textBoxRadius.Text = radiusParam
                .Value.ToString();
            textBoxHeightBottom.Text = heightBottomParam
                .Value.ToString();
            textBoxThicknessLowerEdge.Text = thicknessLowerEdgeParam
                .Value.ToString();
            textBoxThicknessUpperEdge.Text = thicknessUpperEdgeParam
                .Value.ToString();
            textBoxHeightUpperEdge.Text = heightUpperEdgeParam
                .Value.ToString();
            textBoxNumberOfEdge.Text = numberOfEdgeParam
                .Value.ToString();
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
                    labelHeightTotal.ForeColor = Color.Black;
                    labelRadius.ForeColor = Color.Black;
                    labelHeightBottom.ForeColor = Color.Black;
                    labelThicknessLowerEdge.ForeColor = Color.Black;
                    labelThicknessUpperEdge.ForeColor = Color.Black;
                    labelHeightUpperEdge.ForeColor = Color.Black;
                    labelNumberOfEdge.ForeColor = Color.Black;
                    MessageBox.Show("Построение гранёного стакана начато!",
                        "Информация",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            try
            {
                foreach (var parameter in _parameters.NumericalParameters)
                {
                    if (parameter.Value.Value == 0)
                    {
                        throw new Exception(parameter.Key.
                            ToString() + "_null");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                switch (ex.Message)
                {
                    //TODO: {} +
                    case "HeightTotal_null":
                        if (textBoxError != null)
                        {
                            textBoxError.Text += "";
                            textBoxError.Text += " Неверно введено значение" +
                            " в поле 'Общая высота'!\n";
                        }
                        break;
                    case "Radius_null":
                        if (textBoxError != null)
                        {
                            textBoxError.Text += " Неверно введено значение" +
                                " в поле 'Радиус'!\n";
                            textBoxError.Text += "";
                        }
                        break;
                    case "HeightBottom_null":
                        if (textBoxError != null)
                            textBoxError.Text += " Неверно введено значение" +
                                " в поле 'Высота дна'!\n";
                        textBoxError.Text += "";
                        break;
                    case "ThicknessLowerEdge_null":
                        if (textBoxError != null)
                        {
                            textBoxError.Text += " Неверно введено значение" +
                                " в поле 'Толщина нижней стенки'!\n";
                            textBoxError.Text += "";
                        }
                        break;
                    case "ThicknessUpperEdge_null":
                        if (textBoxError != null)
                        {
                            textBoxError.Text += " Неверно введено значение" +
                                " в поле 'Толщина верхней стенки'!\n";
                            textBoxError.Text += "";
                        }
                        break;
                    case "HeightUpperEdge_null":
                        if (textBoxError != null)
                        {
                            textBoxError.Text += "Неверно введено значение" +
                                " в поле 'Высота верхней стенки'!\n";
                            textBoxError.Text += "";
                        }
                        break;
                    case "NumberOfEdge_null":
                        if (textBoxError != null)
                        {
                            textBoxError.Text += "Неверно введено значение" +
                                " в поле 'Количество граней'!\n";
                            textBoxError.Text += "";
                        }
                        break;
                }
                return false;
            }
        }

        /// <summary>
        /// Обработчик потери фокуса полем ввода количества граней
        /// </summary>
        private void textBoxNumberOfEdge_Leave(object sender, EventArgs e)
        {
            try
            {
                textBoxError.Text = "";
                if (textBoxNumberOfEdge.Text != "")
                {
                    double value = 0;
                    if (double.TryParse(textBoxNumberOfEdge.Text,
                        out value) && value != 0)
                    {
                        _parameters.NumericalParameters
                            [ParameterType.NumberOfEdge].Value = value;
                        _numberOfEdgeCurrent = value;
                        textBoxNumberOfEdge.ForeColor = Color.Black;
                        labelNumberOfEdge.ForeColor = Color.Black;
                        CheckParametersBeforeBuilding();
                    }
                    else
                    {
                        textBoxError.Text += $"В поле 'Количество граней'" +
                            $" было введено некорректное значение!\n";
                    }
                }
                else
                {
                    if (_numberOfEdgeCurrent != 0)
                    {
                        textBoxNumberOfEdge.Text =
                            _numberOfEdgeCurrent.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                textBoxNumberOfEdge.ForeColor = Color.Red;
                CheckParametersBeforeBuilding();
                textBoxError.Text += $"Ошибка при вводе количества" +
                    $" граней: {ex.Message}\n";
            }
        }

        /// <summary>
        /// Обработчик потери фокуса полем ввода высоты верхней стенки
        /// </summary>
        private void textBoxHeightUpperEdge_Leave(object sender, EventArgs e)
        {
            try
            {
                textBoxError.Text = "";
                if (textBoxHeightUpperEdge.Text != "")
                {
                    double value = 0;
                    if (double.TryParse(textBoxHeightUpperEdge.Text,
                        out value) && value != 0)
                    {
                        _parameters.NumericalParameters
                            [ParameterType.HeightUpperEdge].Value = value;
                        _heightUpperEdgeCurrent = value;
                        textBoxHeightUpperEdge.ForeColor = Color.Black;
                        labelHeightUpperEdge.ForeColor = Color.Black;
                        CheckParametersBeforeBuilding();
                    }
                    else
                    {
                        textBoxError.Text += $"В поле 'Высота верхней " +
                            $"стенки' было введено некорректное значение!\n";
                    }
                }
                else
                {
                    if (_heightUpperEdgeCurrent != 0)
                    {
                        textBoxHeightUpperEdge.Text =
                            _heightUpperEdgeCurrent.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                textBoxHeightUpperEdge.ForeColor = Color.Red;
                CheckParametersBeforeBuilding();
                textBoxError.Text += $"Ошибка при вводе " +
                    $"высоты верхней стенки: {ex.Message}\n";
            }
        }

        /// <summary>
        /// Обработчик потери фокуса полем ввода толщины верхней стенки
        /// </summary>
        private void textBoxThicknessUpperEdge_Leave(object sender,
            EventArgs e)
        {
            try
            {
                textBoxError.Text = "";
                if (textBoxThicknessUpperEdge.Text != "")
                {
                    double value = 0;
                    if (double.TryParse(textBoxThicknessUpperEdge.Text,
                        out value) && value != 0)
                    {
                        _parameters.NumericalParameters
                            [ParameterType.ThicknessUpperEdge].Value = value;
                        _thicknessUpperEdgeCurrent = value;
                        textBoxThicknessUpperEdge.ForeColor = Color.Black;
                        labelThicknessUpperEdge.ForeColor = Color.Black;
                        CheckParametersBeforeBuilding();
                    }
                    else
                    {
                        textBoxError.Text += $"В поле 'Толщина верхней" +
                            $" стенки' было введено некорректное значение!\n";
                    }
                }
                else
                {
                    if (_thicknessUpperEdgeCurrent != 0)
                    {
                        textBoxThicknessUpperEdge.Text =
                            _thicknessUpperEdgeCurrent.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                textBoxThicknessUpperEdge.ForeColor = Color.Red;
                CheckParametersBeforeBuilding();
                textBoxError.Text += $"Ошибка при вводе " +
                    $"толщины верхней стенки: {ex.Message}\n";
            }
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

                var upperParam = _parameters.NumericalParameters
                    [ParameterType.ThicknessUpperEdge];
                labelLimitThicknessUpperEdge.Text = $"от " +
                    $"{upperParam.MinValue:F1} до " +
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
            try
            {
                textBoxError.Text = "";
                if (textBoxHeightBottom.Text != "")
                {
                    double value = 0;
                    if (double.TryParse(textBoxHeightBottom.Text,
                        out value) && value != 0)
                    {
                        _parameters.NumericalParameters
                            [ParameterType.HeightBottom].Value = value;
                        _heightBottomCurrent = value;
                        textBoxHeightBottom.ForeColor = Color.Black;
                        labelHeightBottom.ForeColor = Color.Black;
                        CheckParametersBeforeBuilding();
                    }
                    else
                    {
                        textBoxError.Text += $"В поле 'Высота дна'" +
                            $" было введено некорректное значение!\n";
                    }
                }
                else
                {
                    if (_heightBottomCurrent != 0)
                    {
                        textBoxHeightBottom.Text =
                            _heightBottomCurrent.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                textBoxHeightBottom.ForeColor = Color.Red;
                CheckParametersBeforeBuilding();
                textBoxError.Text += $"Ошибка при вводе " +
                    $"высоты дна: {ex.Message}\n";
            }
        }

        /// <summary>
        /// Обработчик потери фокуса полем ввода радиуса
        /// </summary>
        private void textBoxRadius_Leave(object sender, EventArgs e)
        {
            try
            {
                textBoxError.Text = "";
                if (textBoxRadius.Text != "")
                {
                    double value = 0;
                    if (double.TryParse(textBoxRadius.Text,
                        out value) && value != 0)
                    {
                        _parameters.NumericalParameters
                            [ParameterType.Radius].Value = value;
                        _radiusCurrent = value;
                        textBoxRadius.ForeColor = Color.Black;
                        labelRadius.ForeColor = Color.Black;
                        CheckParametersBeforeBuilding();
                    }
                    else
                    {
                        textBoxError.Text += $"В поле 'Радиус'" +
                            $" было введено некорректное значение!\n";
                    }
                }
                else
                {
                    if (_radiusCurrent != 0)
                    {
                        textBoxRadius.Text = _radiusCurrent.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                textBoxRadius.ForeColor = Color.Red;
                CheckParametersBeforeBuilding();
                textBoxError.Text += $"Ошибка при вводе" +
                    $" радиуса: {ex.Message} \n";
            }
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

                var heightUpperEdgeParam = _parameters.
                    NumericalParameters[ParameterType.HeightUpperEdge];
                labelLimitHeightUpperEdge.Text = $"от " +
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

        //TODO: RSDN +
        private void CheckParametersBeforeBuilding()
        {
            if (textBoxHeightTotal.ForeColor == Color.Red
                || textBoxRadius.ForeColor == Color.Red
                || textBoxHeightBottom.ForeColor == Color.Red
                || textBoxThicknessLowerEdge.ForeColor == Color.Red
                || textBoxThicknessUpperEdge.ForeColor == Color.Red
                || textBoxHeightUpperEdge.ForeColor == Color.Red
                || textBoxNumberOfEdge.ForeColor == Color.Red)
            {
                buttonBuildFacetedGlass.Enabled = false;
            }
            else
            {
                buttonBuildFacetedGlass.Enabled = true;
            }
        }

        private void buttonExportSTL_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "STL files (*.stl)|*.stl";
                saveDialog.DefaultExt = "stl";
                saveDialog.FileName = "faceted_glass.stl";
                saveDialog.Title = "Экспорт модели в STL";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        // Используем новый метод для построения и экспорта
                        _builder.BuildAndExportToStl(_parameters, saveDialog.FileName);

                        Cursor.Current = Cursors.Default;

                        MessageBox.Show($"Модель успешно экспортирована в:\n{saveDialog.FileName}",
                            "Успешный экспорт",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show($"Ошибка экспорта: {ex.Message}",
                            "Ошибка",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
        }

    }
}