using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.media;

namespace UmbracoWorkbench
{

    public class PerformanceChartImageMediaFactory : UmbracoImageMediaFactory
    {

        private const string FUND_MEDIA_TYPE = "PerformanceChart";

        public override string MediaTypeAlias
        {
            get
            {
                // The type of media this factory produces
                return FUND_MEDIA_TYPE;
            }
        }

        public override int Priority
        {
            get
            {
                // Making sure my factory is earlier in the list of candidate factories (Base class uses 1000)
                return 500;
            }
        }

        public override bool CanHandleMedia(int parentNodeId, PostedMediaFile postedFile, User user)
        {
            // Decide whether this factory applies; If not, another factory will be used (i.e. the base class)
            bool isGalleryParent = false;

            if (parentNodeId != -1)
            {
                Media parent = new Media(parentNodeId);
                isGalleryParent = parent.ContentType.Alias == FUND_MEDIA_TYPE + "Folder";
            }
            else
            {
                isGalleryParent = false;
            }

            return isGalleryParent && base.CanHandleMedia(parentNodeId, postedFile, user);
        }

    }


}