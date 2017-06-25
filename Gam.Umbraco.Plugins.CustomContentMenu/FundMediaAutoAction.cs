using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using umbraco.cms.businesslogic.web;
using umbraco.interfaces;
using umbraco.MacroEngines;

using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;


namespace Gam.Umbraco.Plugins
{

    /// <summary>
    /// 
    /// UMBRACO PLUG-IN : Fund Media Auto Action
    /// 
    /// When the user creates a MEDIA node of the type FundOrFundGroup, create 
    /// default media folders under. The folder types are dynamically interrogated
    /// from Umbraco "allowed" types. (The types defined in the STRUCTURE tab.)
    /// 
    /// The plug-in is depoyed into Umbraco by copying the assembly dll into the 
    /// Umbraco bin directory. Umbraco will then load the plug-in automatically.
    /// 
    /// To debug: attach Visual Studio to the w3wp.exe process (the IIS Umbraco host).
    /// 
    /// </summary>
    public class CustomEventHandler : IApplicationStartupHandler
    {
        public CustomEventHandler()
        {
            // event triggers each time a media item is saved
            MediaService.Saved += MediaService_Saved;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MediaService_Saved(IMediaService sender, global::Umbraco.Core.Events.SaveEventArgs<global::Umbraco.Core.Models.IMedia> e)
        {
            // the Umbraco method allows for multiple saves
            // in testing I've only been able to generate a single save at any time
            // however, the code honours the possibility of multiple media elements being saved

            foreach (var eachMediaEntity in e.SavedEntities)
            {
                if (folderNeedsAutoChildren(eachMediaEntity) && folderHasNoChildren(eachMediaEntity))
                {
                    createAllowedFolders(eachMediaEntity);
                }
            }

        }

        // is the media type of the sort we auto generate for
        private bool folderNeedsAutoChildren(IMedia media)
        {
            return media.ContentType.Alias == "FundOrFundGroup";
        }

        // we will only auto generate where no folders already exist
        // in other words only do this once and don't over write any changes the user may have made
        private bool folderHasNoChildren(IMedia media)
        { 
            var mediaNode = new DynamicMedia(media.Id);
            return (mediaNode.Children.Count() == 0);
        }


        private void createAllowedFolders(IMedia parentNode)
        {
            
            // umbraco services
            var typeService = ApplicationContext.Current.Services.ContentTypeService;
            var mediaService = ApplicationContext.Current.Services.MediaService;

            // types that can be created under the current node
            var allowedTypes = parentNode.ContentType.AllowedContentTypes;

            foreach (var eachAllowedType in allowedTypes)
            {
                var typeToCreate = typeService.GetMediaType(eachAllowedType.Id.Value);

                // Fact Sheet Folder -> Fact Sheets
                // Performance Chart Folder -> Performance Charts
                // Manager Commentary Folder -> Manager Commentary
                string folderName = typeToCreate.Name.Replace(" Folder", "");
                if (!folderName.EndsWith("y")) folderName = folderName + "s";

                // create folder under parent node
                var media = mediaService.CreateMedia(folderName, parentNode.Id, typeToCreate.Alias);
                mediaService.Save(media);
            }

        }

    }

}
