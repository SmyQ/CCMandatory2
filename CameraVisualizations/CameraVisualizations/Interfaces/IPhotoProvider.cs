﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CameraVisualizations.Utils;

namespace CameraVisualizations.Interfaces
{
    interface IPhotoProvider
    {
        List<PhotoScatterViewItem> GetPhotos(Phone phone);
    }
}
