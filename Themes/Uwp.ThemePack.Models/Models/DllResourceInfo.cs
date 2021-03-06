﻿using System;
using Windows.UI.Xaml;
using Uwp.ThemePack.Models.Models.Enums;

namespace Uwp.ThemePack.Models.Models
{
    /// <summary>
    /// Assembly <see cref="ResourceDictionary"/> info
    /// </summary>
    public class DllResourceInfo
    {
        /// <summary>
        /// Initialize new instance of <see cref="DllResourceInfo"/>
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="uri">Resource uri</param>
        /// <param name="resource">.xaml resource <see cref="ResourceDictionary"/></param>
        /// <param name="resourceType"><see cref="ResourceType"/></param>
        public DllResourceInfo(string name, Uri uri, XamlResourceType resourceType)
        {
            Name = name;
            Uri = uri;
            ResourceType = resourceType;
        }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Resource <see cref="Uri"/>
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// <see cref="ResourceType"/>
        /// </summary>
        public XamlResourceType ResourceType { get; set; }
    }
}
