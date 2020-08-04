using System;
using System.IO;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace Probel.Lanceur.Helpers
{
    public class ToastMessage
    {
        #region Fields

        public const string ApplicationId = "Lanceur";

        #endregion Fields

        #region Constructors

        private ToastMessage(ToastTemplateType template, string imageSource = null)
        {
            XmlDocument = ToastNotificationManager.GetTemplateContent(template);

            if (string.IsNullOrWhiteSpace(imageSource) == false) { SetImage(imageSource); }
        }

        #endregion Constructors

        #region Properties

        public XmlDocument XmlDocument { get; }

        #endregion Properties

        #region Methods

        private void SetImage(string imageSource)
        {
            // Specify the absolute path to an image
            var imagePath = "file:///" + Path.GetFullPath(imageSource);
            var imageElements = XmlDocument.GetElementsByTagName("image");
            imageElements[0].Attributes.GetNamedItem("src").NodeValue = imagePath;
        }

        public static ToastMessage ImageAndText01(string imageSource = null) => new ToastMessage(ToastTemplateType.ToastImageAndText01, imageSource);

        public static ToastMessage ImageAndText02(string imageSource = null) => new ToastMessage(ToastTemplateType.ToastImageAndText02, imageSource);

        public static ToastMessage ImageAndText03(string imageSource = null) => new ToastMessage(ToastTemplateType.ToastImageAndText03, imageSource);

        public static ToastMessage ImageAndText04(string imageSource = null) => new ToastMessage(ToastTemplateType.ToastImageAndText04, imageSource);

        public static ToastMessage Text01() => new ToastMessage(ToastTemplateType.ToastText01);

        public static ToastMessage Text02() => new ToastMessage(ToastTemplateType.ToastText02);

        public static ToastMessage Text03() => new ToastMessage(ToastTemplateType.ToastText03);

        public static ToastMessage Text04() => new ToastMessage(ToastTemplateType.ToastText04);

        public void AppendText(params string[] messages)
        {
            // Fill in the text elements
            XmlNodeList stringElements = XmlDocument.GetElementsByTagName("text");

            if (stringElements.Length > messages.Length)
            {
                throw new IndexOutOfRangeException($"Too few messages for the Notifyer. Expected {stringElements.Length} message(s) but {messages.Length} message(s) was specified");
            }

            for (int i = 0; i < stringElements.Length; i++)
            {
                stringElements[i].AppendChild(XmlDocument.CreateTextNode(messages[i]));
            }
        }

        public override string ToString() => XmlDocument.GetXml();

        #endregion Methods
    }
}