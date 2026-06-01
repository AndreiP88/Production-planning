using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace Production_planning
{
    public class BiMergedCell : DataGridViewTextBoxCell
    {
        public int LeftColumn { get; set; }
        public int RightColumn { get; set; }
        public int TopRow { get; set; }
        public int BottomRow { get; set; }
        public FontStyle FontStyle { get; set; } = FontStyle.Bold;
        public Color MergeBackColor { get; set; } = Color.Gray;

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            try
            {
                if (this.DataGridView == null) return;

                // 1. Закрашиваем фон текущей ячейки
                using (SolidBrush bgBrush = new SolidBrush(MergeBackColor))
                {
                    graphics.FillRectangle(bgBrush, cellBounds);
                }

                // 2. Рисуем границы блока (только на крайних ячейках)
                if (ColumnIndex == RightColumn)
                {
                    graphics.DrawLine(SystemPens.ControlDark, cellBounds.Right - 1, cellBounds.Top, cellBounds.Right - 1, cellBounds.Bottom);
                }
                if (rowIndex == BottomRow)
                {
                    graphics.DrawLine(SystemPens.ControlDark, cellBounds.Left, cellBounds.Bottom - 1, cellBounds.Right, cellBounds.Bottom - 1);
                }

                // 3. Получаем текст из истинной мастер-ячейки
                object masterValue = this.DataGridView[LeftColumn, TopRow].Value;
                string strText = masterValue != null ? masterValue.ToString() : string.Empty;

                if (string.IsNullOrEmpty(strText)) return;

                // 4. Вычисляем экранные координаты всего объединенного блока целиком
                Rectangle rectLeftTop = this.DataGridView.GetCellDisplayRectangle(LeftColumn, TopRow, false);
                Rectangle rectRightBottom = this.DataGridView.GetCellDisplayRectangle(RightColumn, BottomRow, false);

                // Если весь блок полностью скрыт за экраном — выходим
                if (rectLeftTop == Rectangle.Empty && rectRightBottom == Rectangle.Empty) return;

                // Находим физические границы полного прямоугольника блока на экране
                int blockLeft = rectLeftTop != Rectangle.Empty ? rectLeftTop.Left : cellBounds.Left - GetXOffsetToLeft();
                int blockTop = rectLeftTop != Rectangle.Empty ? rectLeftTop.Top : cellBounds.Top - GetYOffsetToTop();

                int blockRight = rectRightBottom != Rectangle.Empty ? rectRightBottom.Right : cellBounds.Right + GetXOffsetToRight();
                int blockBottom = rectRightBottom != Rectangle.Empty ? rectRightBottom.Bottom : cellBounds.Bottom + GetYOffsetToBottom();

                Rectangle rectFullBlock = Rectangle.FromLTRB(blockLeft, blockTop, blockRight, blockBottom);

                // 5. ЗАЩИТА ЗАКРЕПЛЕННЫХ (FROZEN) ЯЧЕЕК
                // Вычисляем границы прокручиваемой зоны, куда тексту заходить нельзя
                int frozenWidth = this.DataGridView.RowHeadersVisible ? this.DataGridView.RowHeadersWidth : 0;
                for (int i = 0; i < this.DataGridView.Columns.Count; i++)
                    if (this.DataGridView.Columns[i].Frozen && this.DataGridView.Columns[i].Visible) frozenWidth += this.DataGridView.Columns[i].Width;

                int frozenHeight = this.DataGridView.ColumnHeadersVisible ? this.DataGridView.ColumnHeadersHeight : 0;
                for (int i = 0; i < this.DataGridView.Rows.Count; i++)
                    if (this.DataGridView.Rows[i].Frozen && this.DataGridView.Rows[i].Visible) frozenHeight += this.DataGridView.Rows[i].Height;

                // Прямоугольник доступной незакрепленной области экрана
                Rectangle safeScrollableArea = new Rectangle(
                    frozenWidth,
                    frozenHeight,
                    this.DataGridView.ClientRectangle.Width - frozenWidth,
                    this.DataGridView.ClientRectangle.Height - frozenHeight
                );

                // Корректируем видимый регион в зависимости от типа ячейки
                Region oldClip = graphics.Clip;
                if (!this.OwningColumn.Frozen && !this.OwningRow.Frozen)
                {
                    // Обрезаем зону рисования по границе Frozen-панелей
                    graphics.SetClip(safeScrollableArea, System.Drawing.Drawing2D.CombineMode.Intersect);
                }

                // Дополнительно обрезаем по границам текущей ячейки, чтобы текст не двоился при отрисовке соседей
                graphics.SetClip(cellBounds, System.Drawing.Drawing2D.CombineMode.Intersect);

                // 6. Рисуем текст строго по центру ВСЕГО виртуального блока rectFullBlock
                using (StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center,
                    Trimming = StringTrimming.EllipsisCharacter
                })
                using (Font font = new Font("Arial", 10, FontStyle))
                {
                    graphics.DrawString(strText, font, Brushes.White, rectFullBlock, sf);
                }

                graphics.Clip = oldClip;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка отрисовки: {ex}");
            }
        }

        // Вспомогательные методы для расчета смещений, если мастер-ячейки ушли за экран
        private int GetXOffsetToLeft()
        {
            int offset = 0;
            for (int i = LeftColumn; i < ColumnIndex; i++)
                if (this.DataGridView.Columns[i].Visible) offset += this.DataGridView.Columns[i].Width;
            return offset;
        }

        private int GetYOffsetToTop()
        {
            int offset = 0;
            for (int i = TopRow; i < RowIndex; i++)
                if (this.DataGridView.Rows[i].Visible) offset += this.DataGridView.Rows[i].Height;
            return offset;
        }

        private int GetXOffsetToRight()
        {
            int offset = 0;
            for (int i = ColumnIndex + 1; i <= RightColumn; i++)
                if (this.DataGridView.Columns[i].Visible) offset += this.DataGridView.Columns[i].Width;
            return offset;
        }

        private int GetYOffsetToBottom()
        {
            int offset = 0;
            for (int i = RowIndex + 1; i <= BottomRow; i++)
                if (this.DataGridView.Rows[i].Visible) offset += this.DataGridView.Rows[i].Height;
            return offset;
        }
    }
}
