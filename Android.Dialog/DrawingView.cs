using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Java.IO;

namespace Android.Dialog
{
    public class DrawingView : View
    {
        private Bitmap sigLine;
        private Bitmap mBitmap;
        private Canvas mCanvas;
        private Android.Graphics.Path mPath;
        private Paint mBitmapPaint;
        private Paint mPaint;
        private int h;
        private int w;
        private int sigLineW;
        private int sigLineH;
        private string oldImagePath;

        public DrawingView(Context context) :
                base (context)
        {
            Initialize();
        }

        public DrawingView(Context context, string oldImagePath) :
            base (context)
        {
            this.oldImagePath = oldImagePath; 
            Initialize();

        }

        public DrawingView(Context context, IAttributeSet attrs) :
                base (context, attrs)
        {
            Initialize();
        }

        public DrawingView(Context context,
                           IAttributeSet attrs,
                           int defStyle) :
                base (context, attrs, defStyle)
        {
            Initialize();
        }

        void Initialize()
        {

            mPaint = new Paint();

            mPaint.AntiAlias = true;
            mPaint.Dither = true;

            mPaint.Color = new Color(0, 0, 0, 255);
            mPaint.SetStyle(Paint.Style.Stroke);

            mPaint.StrokeJoin = Paint.Join.Round;
            mPaint.StrokeCap = Paint.Cap.Round;
            mPaint.StrokeWidth = 4;
            mPath = new Android.Graphics.Path();
            mBitmapPaint = new Paint(PaintFlags.AntiAlias);

           
            if (!String.IsNullOrEmpty(oldImagePath))
            {
                sigLine = ImageUtility.LoadImage(oldImagePath); 
            }

            if (sigLine == null)
            {
                sigLine = ImageUtility.LoadImage(DrawingActivity.BACKGROUND_FILE_PATH); 
            }

        }

        protected override void OnSizeChanged(int w,
                                              int h,
                                              int oldw,
                                              int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);
            this.h = h;
            this.w = w;

            mBitmap = Bitmap.CreateBitmap(w, h, Bitmap.Config.Argb8888);
        
            mCanvas = new Canvas(mBitmap);

            DrawBackgroundImage(sigLine); 




        }

        private void DrawBackgroundImage(Bitmap bitmap)
        {

            if (bitmap != null)
            {
                sigLineW = bitmap.Width;
                sigLineH = bitmap.Height;
     
           
                mCanvas.DrawBitmap(bitmap, (w / 2) - (sigLineW / 2), (h / 2)
                    - (sigLineH / 2), mBitmapPaint);
            }
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            canvas.DrawColor(new Color(255, 255, 255, 0));
            canvas.DrawBitmap(mBitmap, 0, 0, mBitmapPaint);
            canvas.DrawPath(mPath, mPaint);
        }

        private float mX, mY;
        private static float TOUCH_TOLERANCE = 4;

        private void touch_start(float x, float y)
        {
            mPath.Reset();
            mPath.MoveTo(x, y);
            mX = x;
            mY = y;
        }

        private void touch_move(float x, float y)
        {


            float dx = Math.Abs(x - mX);
            float dy = Math.Abs(y - mY);
            if (dx >= TOUCH_TOLERANCE || dy >= TOUCH_TOLERANCE)
            {
                mPath.QuadTo(mX, mY, (x + mX) / 2, (y + mY) / 2);
                mX = x;
                mY = y;
            }
        }

        private void touch_up()
        {
            mPath.LineTo(mX, mY);
            mCanvas.DrawPath(mPath, mPaint);
            mPath.Reset();
        }

        public override bool OnTouchEvent(MotionEvent motionEvent)
        {
            float x = motionEvent.GetX();
            float y = motionEvent.GetY();

            switch (motionEvent.Action)
            {

                case MotionEventActions.Down:
                    touch_start(x, y);
                    Invalidate();
                    break;
                case MotionEventActions.Move:
                    touch_move(x, y);
                    Invalidate();
                    break;
                case MotionEventActions.Up:
                    touch_up();
                    Invalidate();
                    break;
            }
            return true;
        }

        public void ClearImage()
        {
            mBitmap = Bitmap.CreateBitmap(w, h, Bitmap.Config.Argb8888);
            mCanvas = new Canvas(mBitmap);
            sigLine = ImageUtility.LoadImage(DrawingActivity.BACKGROUND_FILE_PATH); 
            if (sigLine != null)
            {
                sigLineW = sigLine.Width;
                sigLineH = sigLine.Height;
                mCanvas.DrawBitmap(sigLine, (w / 2) - (sigLineW / 2), (h / 2)
                    - (sigLineH / 2), mBitmapPaint);
            }
            this.Invalidate(); 
        }

        public void SaveImage(String fileName)
        {


            sigLineW = sigLine.Width;
            sigLineH = sigLine.Height;
            int calcX = (w / 2) - (sigLineW / 2); 
            int calcY = (h / 2) - (sigLineH / 2); 
            int calcW = sigLineW;
            int calcH = sigLineH; 

            if (calcW > w)
            {
                calcW = w; 
            }

            if (calcH > h)
            {
                calcH = h; 
            }

            if (calcX < 0)
            {
                calcX = 0; 
            }

            if (calcY < 0)
            {
                calcY = 0; 
            }

            Bitmap bitmapToSave = Bitmap.CreateBitmap(mBitmap,
                                                      calcX,
                                                      calcY,
                                                      calcW,
                                                      calcH);

            ImageUtility.SaveImage(bitmapToSave, fileName);

            if (bitmapToSave != null)
            {
                bitmapToSave.Recycle(); 
                bitmapToSave = null; 
            }
        }

        private void CleanUp()
        {
            if (sigLine != null)
            {
                sigLine.Recycle(); 
                sigLine = null;    
            }

            if (mBitmap != null)
            {
                mBitmap.Recycle(); 
                mBitmap = null; 

            }
        }
    }
}
