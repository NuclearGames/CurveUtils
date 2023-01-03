using System;
using System.Numerics;
using System.Threading.Tasks;
using CurvesWebEditor.Data.CanvasRendering.Interfaces;

namespace CurvesWebEditor.Data.CanvasRendering.Renderers
{
    internal sealed class TextRenderer : IRenderer {
        internal Vector2 Position { get; set; } = Vector2.Zero;
        internal string Text { get; set; } = string.Empty;
        internal string Color { get; set; } = "#000000";
        internal float FontSizePx { get; set; } = 0.05f;
        internal string Font { get; set; } = "serif";

        public async ValueTask Render(CanvasRenderContext context) {
            if (Text == string.Empty) {
                return;
            }

            var posSS = context.Transformer.Point(Position);
            int fontSizeSS = (int)MathF.Round(context.Transformer.Size(FontSizePx));

            await context.Canvas.BeginPathAsync();
            await context.Canvas.SetFontAsync($"{fontSizeSS}px {Font}");
            await context.Canvas.SetFillStyleAsync(Color);
            await context.Canvas.FillTextAsync(Text, posSS.X, posSS.Y);
        }
    }
}
