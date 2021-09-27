using System;
using System.IO;
using System.Windows.Media.Imaging;
using Autodesk.Revit.ApplicationServices;

namespace LearnRevitAPI.Lib
{
    internal class RevitAPIConstraint
    {
      internal BitmapImage IconWindow;
      /// <summary>
      /// C:\ProgramData\Autodesk\ApplicationPlugins\LearnRevitAPI.bundle\Contents
      /// </summary>
      internal string ContentsFolder;

      /// <summary>
      /// C:\ProgramData\Autodesk\ApplicationPlugins\LearnRevitAPI.bundle\Contents\Resources
      /// </summary>
      internal string ResourcesFolder;

      /// <summary>
      /// "C:\ProgramData\Autodesk\ApplicationPlugins\LearnRevitAPI.bundle\Contents\Resources\Help"
      /// </summary>
      internal string HelpFolder;

      /// <summary>
      /// C:\ProgramData\Autodesk\ApplicationPlugins\LearnRevitAPI.bundle\Contents\Resources\Image
      /// </summary>
      internal string ImageFolder;

      /// <summary>
      /// C:\ProgramData\Autodesk\ApplicationPlugins\LearnRevitAPI.bundle\Contents\Resources\Setting
      /// </summary>
      internal static string SettingFolder
            = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Autodesk", "ApplicationPlugins", "RevitAPI");

      /// <summary>
      ///  C:\ProgramData\Autodesk\ApplicationPlugins\LearnRevitAPI.bundle\Contents\2017\dll
      /// </summary>
      internal string DllFolder;

        /// <summary>
        /// ALPHA BIM - LEAD BY THE TRUST
        /// </summary>
        internal static string MessageBoxCaption = "MEOS - REVIT API";

        /// <summary>
        /// tên file ảnh 32x32 Other
        /// </summary>
        internal static string Other32x32 = "Other32x32.png";

        /// <summary>
        /// tên file ảnh 16x16 Other
        /// </summary>
        internal static string Other16x16 = "Other16x16.png";

      /// <summary>
      /// "C:\ProgramData\Autodesk\ApplicationPlugins\LearnRevitAPI.bundle\Contents\Resources\Help\Alpha BIM Guideline.pdf
      /// </summary>
      internal string HelperPath;

        /// <summary>
        /// Tên của file Share parameter
        /// </summary>
        internal string SharedParamsPath;

        /// <summary>
        /// file .dll chính
        /// </summary>
        internal string MainDllFileName;

        internal RevitAPIConstraint(ControlledApplication a = null)
        {
            ContentsFolder =
                "C:\\ProgramData\\Autodesk\\ApplicationPlugins\\LearnRevitAPI.bundle\\Contents";

            ResourcesFolder = Path.Combine(ContentsFolder, "Resources");
            //HelpFolder = Path.Combine(ResourcesFolder, "Help");
            ImageFolder = Path.Combine(ResourcesFolder, "Image");
            SettingFolder = Path.Combine(ResourcesFolder, "Setting");

            string iconWindowPath = Path.Combine(ImageFolder, "About.ico");
            Uri iconWindowUri = new Uri(iconWindowPath, UriKind.Relative);
            IconWindow = new BitmapImage(iconWindowUri);

            SharedParamsPath = Path.Combine(SettingFolder, "MEOS_ShareParameter.txt");
            //HelperPath = Path.Combine(HelpFolder);
            
            MainDllFileName = "LearnRevitAPI.dll";

            if (a != null)
            {
                switch (a.VersionNumber)
                {
                    case "2017":
                        DllFolder = Path.Combine(ContentsFolder, "2017");
                        break;
                    case "2018":
                        DllFolder = Path.Combine(ContentsFolder, "2018");
                        break;
                    case "2019":
                        DllFolder = Path.Combine(ContentsFolder, "2019");
                        break;
                    case "2020":
                        DllFolder = Path.Combine(ContentsFolder, "2020");
                        break;
                    case "2021":
                        DllFolder = Path.Combine(ContentsFolder, "2021");
                        break;
                    case "2022":
                        DllFolder = Path.Combine(ContentsFolder, "2022");
                        break;
                    case "2023":
                        DllFolder = Path.Combine(ContentsFolder, "2023");
                        break;
                    case "2024":
                        DllFolder = Path.Combine(ContentsFolder, "2024");
                        break;
                    case "2025":
                        DllFolder = Path.Combine(ContentsFolder, "2025");
                        break;
                }
            }
        }
    }
}