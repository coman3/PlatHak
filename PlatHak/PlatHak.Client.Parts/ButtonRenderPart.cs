using PlatHak.Client.Common;
using PlatHak.Common.Maths;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using Factory = SharpDX.Direct2D1.Factory;

namespace PlatHak.Client.Parts
{
    public class ButtonRenderPart : RenderPart
    {
        private readonly RoundedRectangle _roundedRectangle;
        private TextFormat _textFormat;
        public override void OnDraw(RenderTarget target, GameTime time)
        {
            if (FillBrush != null)
                target.FillRoundedRectangle(_roundedRectangle, FillBrush);
            if (BorderBrush != null)
                target.DrawRoundedRectangle(_roundedRectangle, BorderBrush, StrokeWidth);
            if (!string.IsNullOrWhiteSpace(Text))
                target.DrawText(Text, _textFormat, BoundingBox.RawRectangleF, BorderBrush);
        }

        public override void OnInitialize(RenderTarget target, Factory factory, SharpDX.DirectWrite.Factory factoryDr)
        {
            _textFormat = new TextFormat(factoryDr, "Arial", 15)
            {
                TextAlignment = TextAlignment.Center
            };
        }

        public RectangleF BoundingBox { get; set; }
        public string Text { get; set; }
        public float BorderRadius { get; set; }
        public float StrokeWidth { get; set; }
        public Brush BorderBrush { get; set; }
        public Brush FillBrush { get; set; }

        public ButtonRenderPart(RectangleF boundingBox, string text, float borderRadius, Brush borderBrush, Brush fillBrush) : base(boundingBox.Posistion)
        {
            Text = text;
            StrokeWidth = 2;
            BoundingBox = boundingBox;
            BorderRadius = borderRadius;
            BorderBrush = borderBrush;
            FillBrush = fillBrush;
            _roundedRectangle = new RoundedRectangle
            {
                Rect = BoundingBox.RawRectangleF
            };
        }
    }
}