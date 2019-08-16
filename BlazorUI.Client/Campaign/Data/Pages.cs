using System;

namespace BlazorUI.Client.Campaign.Data
{
  /// <summary>
  /// The pages that show CAM banners
  /// </summary>
  [Flags]
  public enum Pages
  {
    None = 0,
    Home = 1,
    Conditional = 2,
    All = Home | Conditional
  }
}