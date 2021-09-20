﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#nullable enable

using DocumentFormat.OpenXml.Framework;
using System;
using System.Collections.Generic;

namespace DocumentFormat.OpenXml.Packaging
{
    /// <summary>
    /// Defines the RdRichValueWebImagePart
    /// </summary>
    [OfficeAvailability(FileFormatVersions.Office2021)]
    [ContentType(ContentTypeConstant)]
    [RelationshipTypeAttribute(RelationshipTypeConstant)]
    public partial class RdRichValueWebImagePart : OpenXmlPart, IFixedContentTypePart
    {
        internal const string ContentTypeConstant = "application/vnd.ms-excel.rdrichvaluewebimage+xml";
        internal const string RelationshipTypeConstant = "http://schemas.microsoft.com/office/2020/07/relationships/rdrichvaluewebimage";
        private DocumentFormat.OpenXml.Office2021.Excel.RichDataWebImage.WebImagesSupportingRichData? _rootElement;

        /// <summary>
        /// Creates an instance of the RdRichValueWebImagePart OpenXmlType
        /// </summary>
        internal protected RdRichValueWebImagePart()
        {
        }

        /// <inheritdoc/>
        public sealed override string ContentType => ContentTypeConstant;

        private protected override OpenXmlPartRootElement? InternalRootElement
        {
            get
            {
                return _rootElement;
            }

            set
            {
                _rootElement = value as DocumentFormat.OpenXml.Office2021.Excel.RichDataWebImage.WebImagesSupportingRichData;
            }
        }

        internal override OpenXmlPartRootElement? PartRootElement => WebImagesSupportingRichData;

        /// <inheritdoc/>
        public sealed override string RelationshipType => RelationshipTypeConstant;

        /// <inheritdoc/>
        internal sealed override string TargetName => "rdrichvaluewebimage";

        /// <inheritdoc/>
        internal sealed override string TargetPath => "richdata";

        /// <summary>
        /// Gets or sets the root element of this part.
        /// </summary>
        public DocumentFormat.OpenXml.Office2021.Excel.RichDataWebImage.WebImagesSupportingRichData WebImagesSupportingRichData
        {
            get
            {
                if (_rootElement is null)
                {
                    LoadDomTree<DocumentFormat.OpenXml.Office2021.Excel.RichDataWebImage.WebImagesSupportingRichData>();
                }

                return _rootElement!;
            }

            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                SetDomTree(value);
            }
        }

        internal override bool IsInVersion(FileFormatVersions version)
        {
            return version.AtLeast(FileFormatVersions.Office2021);
        }
    }
}
