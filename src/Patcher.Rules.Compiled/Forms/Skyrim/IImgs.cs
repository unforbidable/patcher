using Patcher.Rules.Compiled.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Patcher.Rules.Compiled.Forms.Skyrim
{
    /// <summary>
    /// Represents an <b>Image Space</b> form.
    /// </summary>
    public interface IImgs : IForm
    {
        /// <summary>
        /// Gets or sets the HDR eye adaptation speed.
        /// </summary>
        float EyeAdaptSpeed { get; set; }
        /// <summary>
        /// Gets or sets the HDR eye adaptation strength.
        /// </summary>
        float EyeAdaptStrength { get; set; }
        /// <summary>
        /// Gets or sets the HDR bloom blur radius.
        /// </summary>
        float BloomBlurRadius { get; set; }
        /// <summary>
        /// Gets or sets the HDR bloom threshold.
        /// </summary>
        float BloomThreshold { get; set; }
        /// <summary>
        /// Gets or sets the HDR bloom scale.
        /// </summary>
        float BloomScale { get; set; }
        /// <summary>
        /// Gets or sets the HDR reveive bloom threshold.
        /// </summary>
        float ReceiveBloomThreshold { get; set; }
        /// <summary>
        /// Gets or sets the HDR white value.
        /// </summary>
        float White { get; set; }
        /// <summary>
        /// Gets or sets the HDR sunlight scale.
        /// </summary>
        float SunlightScale { get; set; }
        /// <summary>
        /// Gets or sets the HDR sky scale.
        /// </summary>
        float SkyScale { get; set; }
        /// <summary>
        /// Gets or sets the cinematic saturation.
        /// </summary>
        float Saturation { get; set; }
        /// <summary>
        /// Gets or sets the cinematic brightness.
        /// </summary>
        float Brightness { get; set; }
        /// <summary>
        /// Gets or sets the conematic contrast.
        /// </summary>
        float Contrast { get; set; }
        /// <summary>
        /// Gets or sets the tint amount.
        /// </summary>
        float TintAmount { get; set; }
        /// <summary>
        /// Gets or sets the tint color.
        /// </summary>
        IColor TintColor { get; set; }
        /// <summary>
        /// Gets or sets the Depth of Field strength.
        /// </summary>
        float DepthOfFieldStrength { get; set; }
        /// <summary>
        /// Gets or sets the Depth of Field distance.
        /// </summary>
        float DepthOfFieldDistance { get; set; }
        /// <summary>
        /// Gets or sets the Depth of Field range.
        /// </summary>
        float DepthOfFieldRange { get; set; }
        /// <summary>
        /// Gets or sets the Depth of Field blur radius.
        /// </summary>
        float DepthOfFieldBlurRadius { get; set; }
        /// <summary>
        /// Gets or sets the value that indicates whether the sky is affected by the Depth of Field effect.
        /// </summary>
        bool IsDepthOfFieldSkyDisabled { get; set; }
    }
}
