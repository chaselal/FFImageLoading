﻿using System;
using FFImageLoading.Extensions;
using FFImageLoading.Drawables;
using FFImageLoading.Views;
using FFImageLoading.Work;
using Android.Graphics.Drawables;

namespace FFImageLoading.Targets
{
    public class ImageViewTarget : Target<SelfDisposingBitmapDrawable, ImageViewAsync>
    {
        readonly WeakReference<ImageViewAsync> _controlWeakReference;

        public ImageViewTarget(ImageViewAsync control)
        {
            _controlWeakReference = new WeakReference<ImageViewAsync>(control);
        }

        public override bool IsValid
        {
            get
            {
                return Control != null && Control.Handle != IntPtr.Zero;
            }
        }

        public override void SetAsEmpty(IImageLoaderTask task)
        {
            if (task == null || task.IsCancelled)
                return;

            var control = Control;
            if (control == null)
                return;

            control.SetImageResource(global::Android.Resource.Color.Transparent);
        }

        public override void Set(IImageLoaderTask task, SelfDisposingBitmapDrawable image, bool animated)
        {
            if (task == null || task.IsCancelled)
                return;

            var control = Control;
            if (control == null || control.Drawable == image)
                return;

            control.SetImageDrawable(image);
            control.Invalidate();
        }

        public override ImageViewAsync Control
        {
            get
            {
                ImageViewAsync control;
                if (!_controlWeakReference.TryGetTarget(out control))
                    return null;

                if (control == null || control.Handle == IntPtr.Zero)
                    return null;

                return control;
            }
        }
    }
}

