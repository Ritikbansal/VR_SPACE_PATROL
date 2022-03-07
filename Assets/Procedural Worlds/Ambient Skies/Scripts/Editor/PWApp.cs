//Copyright © 2019 Procedural Worlds Pty Limited. All Rights Reserved.
using UnityEditor;
using PWCommon4;

namespace AmbientSkies.Internal
{
    [InitializeOnLoad]
    public static class PWApp
    {
        //Product name
        private const string CONF_NAME = "Ambient Skies";
        //App data information
        private static AppConfig conf;

        /// <summary>
        /// Setup config file
        /// </summary>
        public static AppConfig CONF
        {
            get
            {
                //Return info if present
                if (conf != null)
                {
                    //return
                    return conf;
                }
                //Get config file info
                else
                {
                    //Get config file
                    conf = AssetUtils.GetConfig(CONF_NAME, true);
                    if (conf != null)
                    {
                        //Set and return
                        Prod.Checkin(conf);
                        return conf;
                    }
                    else
                    {
                        //Return null
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// App data
        /// </summary>
        static PWApp()
        {
            conf = AssetUtils.GetConfig(CONF_NAME, true);
            if (conf != null)
            {
                Prod.Checkin(conf);
            }
        }

        /// <summary>
        /// Get an editor utils object that can be used for common Editor stuff - DO make sure to Dispose() the instance.
        /// </summary>
        /// <param name="editorObj">The class that uses the utils. Just pass in "this".</param>
        /// <param name="customUpdateMethod">(Optional) The method to be called when the GUI needs to be updated. (Repaint will always be called.)</param>
        /// <returns>Editor Utils</returns>
        public static EditorUtils GetEditorUtils(IPWEditor editorObj, System.Action customUpdateMethod = null)
        {
            return new EditorUtils(CONF, editorObj, null, customUpdateMethod);
        }
        /// <summary>
        /// Get an editor utils object that can be used for common Editor stuff - DO make sure to Dispose() the instance.
        /// </summary>
        /// <param name="editorObj">The class that uses the utils. Just pass in "this".</param>
        /// <param name="customUpdateMethod">(Optional) The method to be called when the GUI needs to be updated. (Repaint will always be called.)</param>
        /// <param name="customNewsURL">(Optional) Custom News URL to fetch the news messages from (will default to the News URL in app config if none provided)</param>
        /// <param name="overrideParameters">A custom set of URL Parameters to use when fetching news data. If left empty, the default set of parameters will be used</param>
        /// <returns>Editor Utils</returns>
        public static EditorUtils GetEditorUtils(IPWEditor editorObj, System.Action customUpdateMethod = null, string customNewsURL = null, URLParameters overrideParameters = null)
        {
            return new EditorUtils(CONF, editorObj, null, customUpdateMethod, customNewsURL, overrideParameters);
        }
    }
}
